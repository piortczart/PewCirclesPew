using HelloGame.Client;
using HelloGame.Common.Logging;
using StructureMap;
using System;
using System.Threading;
using System.Windows.Forms;

namespace PewCircles
{
    static class Program
    {
        private static Container GetIoC()
        {
            return new Container(_ =>
            {
                _.ForSingletonOf<GraphicsBuffer>();
                _.ForSingletonOf<InputManager>();
                _.ForSingletonOf<ClientNetwork>();
                _.ForSingletonOf<Pewness>();
                _.For<LoggerFactory>().Use<LoggerFactory>().Ctor<string>("extraInfo").Is("Client");
            });
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var window = GetIoC().GetInstance<PewWindow>())
            {
                window.PewMadness();
            }
        }
    }
}
