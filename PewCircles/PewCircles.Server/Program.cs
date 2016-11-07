using HelloGame.Common.Logging;
using StructureMap;
using System.Threading;

namespace PewCircles.Server
{
    class Program
    {
        private static Container GetIoC()
        {
            return new Container(_ =>
            {
                _.For<LoggerFactory>().Use<LoggerFactory>().Ctor<string>("extraInfo").Is("Server");
            });
        }

        static void Main(string[] args)
        {
            var server = GetIoC().GetInstance<Server>();
            server.Start(9999);
            server.Process(new CancellationTokenSource().Token);
        }
    }
}
