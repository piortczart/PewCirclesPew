using HelloGame.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace PewCircles.Server
{
    class Server
    {
        TcpListener _tcpListener;
        MessageTransciever _transciever;
        List<TcpClient> _clients = new List<TcpClient>();
        object _clientsSynchro = new object();
        ILogger _logger;

        int lastClientId = 0;

        public Server(MessageTransciever transciever, LoggerFactory loggerFactory)
        {
            _transciever = transciever;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public void Start(int port)
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _tcpListener.Start();
        }

        public void Process(CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                // Blocks until a client has connected to the server
                TcpClient client = _tcpListener.AcceptTcpClient();

                Task.Run(async () => { await HandleClientComm(client, cancellation); }, cancellation);
            }
        }

        private async Task HandleClientComm(TcpClient client, CancellationToken cancellation)
        {
            try
            {
                TcpClient tcpClient = client;
                NetworkStream clientStream = tcpClient.GetStream();
                lock (_clientsSynchro)
                {
                    _clients.Add(client);
                }
                SayHelloToClient(clientStream);
                while (!cancellation.IsCancellationRequested)
                {
                    NetworkMessage message = await _transciever.GetAsync(clientStream);
                    ProcessMessage(message, clientStream);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Error while handling a client message.", exception);

                Disconnect(client);
            }
        }

        private void Disconnect(TcpClient client)
        {
            lock (_clientsSynchro)
            {
                _clients.Remove(client);
            }

            try
            {
                client.Close();
                client.Dispose();
            }
            catch (Exception)
            {
                // Ignore.
            }
        }

        private void SayHelloToClient(NetworkStream clientStream)
        {
            int currentId = Interlocked.Add(ref lastClientId, 100000);

            _transciever.Send(new NetworkMessage
            {
                Type = NetworkMessageType.Welcome,
                Payload = currentId.ToString()
            }, clientStream);
        }

        private void ProcessMessage(NetworkMessage message, NetworkStream sourceClientStream)
        {
            List<TcpClient> clients;
            lock (_clientsSynchro)
            {
                clients = _clients.ToList();
            }

            foreach (TcpClient client in clients)
            {
                try
                {
                    NetworkStream target = client.GetStream();
                    if (target != sourceClientStream)
                    {
                        _logger.LogInfo("Sending message to a client: " + message.Type);
                        _logger.LogInfo(message.Payload);

                        _transciever.Send(message, target);
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError("Error while sending message to a client.", exception);
                    Disconnect(client);
                }
            }
        }
    }
}
