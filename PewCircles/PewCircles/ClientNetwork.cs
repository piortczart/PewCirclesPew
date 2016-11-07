using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using HelloGame.Common.Logging;
using PewCircles.Server;
using PewCircles.Extensions;
using PewCircles.Game;
using PewCircles;

namespace HelloGame.Client
{
    public class ClientNetwork
    {
        private Stream _serverStream;
        private readonly MessageTransciever _sender;
        private readonly ILogger _logger;
        private Timer _sendMeTimer;

        public Func<MyGameObjects> GetMyGameObjects { get; set; }

        internal delegate void ServerWelcomeDelegate(int myId);
        internal delegate void ServerUpdateLazersDelegate(List<LazerPewPewSettings> lazersSettings);
        internal delegate void ServerUpdateCircleDelegate(PewCircleSettings circleSettings);

        internal event ServerWelcomeDelegate OnServerWelcome;
        internal event ServerUpdateLazersDelegate OnServerUpdateLazers;
        internal event ServerUpdateCircleDelegate OnServerUpdateCircle;

        public ClientNetwork(LoggerFactory loggerFactory, MessageTransciever sender)
        {
            _sender = sender;
            _logger = loggerFactory.CreateLogger(GetType());
            StartPropagation();
        }

        private void StartPropagation()
        {
            _sendMeTimer = new Timer(state =>
            {
                SendMyInfo();
            }, null, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(20));
        }

        private void SendMyInfo()
        {
            if (GetMyGameObjects != null && _serverStream != null)
            {
                MyGameObjects objects = GetMyGameObjects();

                if (objects.MehLazers.Any())
                {
                    _sender.Send(new NetworkMessage
                    {
                        Type = NetworkMessageType.UpdateLazers,
                        Payload = objects.MehLazers.Select(o => o.GetSettings()).SerializeJson()
                    }, _serverStream);
                }
                if (objects.PewCircle != null)
                {
                    var message = new NetworkMessage
                    {
                        Type = NetworkMessageType.UpdateMe,
                        Payload = objects.PewCircle.GetSettings().SerializeJson()
                    };
                    _sender.Send(message, _serverStream);
                }
            }
        }

        public void StartConnection(string serverAddress, int port, string playerName, CancellationTokenSource cancellation)
        {
            _logger.LogInfo($"Starting connection to {serverAddress}:{port}");

            Connect(serverAddress, port);

            Task.Run(async () =>
                { await Receive(cancellation.Token); }, cancellation.Token)
                .ContinueWith(t =>
                    {
                        if (t.Exception != null)
                        {
                            _logger.LogError("Client receive error.", t.Exception);
                        }
                    });
        }

        private void Connect(string server, int port)
        {
            TcpClient client = new TcpClient(server, port);
            _serverStream = client.GetStream();
        }

        /// <summary>
        /// This is a bad, bad hack to start testing quickly.
        /// </summary>
        public async Task WaitAndParseMessageTest()
        {
            await WaitAndParseMessage();
        }

        private async Task Receive(CancellationToken token)
        {
            _logger.LogInfo("Starting receive loop.");

            while (!token.IsCancellationRequested)
            {
                await WaitAndParseMessage();
            }
        }

        private async Task WaitAndParseMessage()
        {
            NetworkMessage message = await _sender.GetAsync(_serverStream);
            _logger.LogInfo("Got a message: " + message.Type);

            switch (message.Type)
            {
                case NetworkMessageType.Welcome:
                    {
                        int id = message.Payload.DeSerializeJson<int>();
                        // Not checking for null since if it is null, something very bad is happening.
                        OnServerWelcome.Invoke(id);
                        break;
                    }
                case NetworkMessageType.UpdateLazers:
                    {
                        var lazers = message.Payload.DeSerializeJson<List<LazerPewPewSettings>>();
                        // Not checking for null since if it is null, something very bad is happening.
                        OnServerUpdateLazers.Invoke(lazers);
                        break;
                    }
                case NetworkMessageType.UpdateMe:
                    {
                        PewCircleSettings circleSettings = message.Payload.DeSerializeJson<PewCircleSettings>();
                        _logger.LogInfo("Circle settings: " + message.Payload);

                        // Not checking for null since if it is null, something very bad is happening.
                        OnServerUpdateCircle(circleSettings);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}