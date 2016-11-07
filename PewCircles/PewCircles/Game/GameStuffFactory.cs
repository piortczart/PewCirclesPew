using PewCircles.Extensions;
using PewCircles.Mechanics;
using StructureMap;
using System.Drawing;

namespace PewCircles.Game
{
    public class GameStuffFactory
    {
        readonly IContainer _container;


        public GameStuffFactory(IContainer container)
        {
            _container = container;
        }

        internal PewCircle CreatePewCircle(PewCircleSettings settings, bool isControlable)
        {
            PewCircle result = _container.GetInstance<PewCircle>();
            result.Initialize(settings, isControlable);
            return result;
        }

        internal LazerPewPew CreateLazer(LazerPewPewSettings settings)
        {
            LazerPewPew result = _container.GetInstance<LazerPewPew>();
            result.Initialize(settings);
            return result;
        }

        public string SerializeLazer(LazerPewPew lazer)
        {
            return lazer.GetSettings().SerializeJson();
        }

        public string SerializePewCircle(PewCircle pewCircle)
        {
            return pewCircle.GetSettings().SerializeJson();
        }
    }
}
