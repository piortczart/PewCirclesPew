using StructureMap;
using System;
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
