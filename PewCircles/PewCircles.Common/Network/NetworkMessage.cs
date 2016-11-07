using PewCircles.Extensions;
using System;

namespace PewCircles.Server
{
    [Serializable]
    public class NetworkMessage
    {
        public NetworkMessageType Type { get; set; }
        public string Payload { get; set; }

        public override string ToString()
        {
            return $"[{Type}] {Payload.SubstringSafe(0, 50)}";
        }
    }

    [Serializable]
    public enum NetworkMessageType
    {
        UpdateLazers = 1,
        Welcome = 2,
        UpdateMe = 3,
    }
}