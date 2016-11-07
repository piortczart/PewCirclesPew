using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PewCircles.Server
{
    public class MessageTransciever
    {
        readonly BinaryFormatter _formatter = new BinaryFormatter();

        private readonly object _synchro = new object();

        public bool Send(NetworkMessage message, Stream stream)
        {
            lock (_synchro)
            {
                try
                {
                    _formatter.Serialize(stream, message);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public NetworkMessage Get(Stream stream)
        {
            return (NetworkMessage)_formatter.Deserialize(stream);
        }

        public Task<NetworkMessage> GetAsync(Stream stream)
        {
            return Task.Run(() => (NetworkMessage)_formatter.Deserialize(stream));
        }
    }
}
