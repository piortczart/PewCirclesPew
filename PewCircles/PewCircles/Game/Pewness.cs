using PewCircles.Game;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace PewCircles
{
    public class Pewness
    {
        private InputManager _inputManager;

        private PewCircle _pewzor;

        private List<LazerPewPew> _lazers = new List<LazerPewPew>();

        public Pewness(InputManager inputManager)
        {
            _inputManager = inputManager;
            _pewzor = new PewCircle(inputManager, this);
        }

        internal void Render(Graphics graphics)
        {
            _pewzor.Render(graphics);
            foreach (LazerPewPew lazer in _lazers)
            {
                lazer.Render(graphics);
            }
        }

        internal void Update(TimeSpan timeDelta)
        {
            _pewzor.Update(timeDelta);

            foreach (LazerPewPew lazer in _lazers)
            {
                lazer.Update(timeDelta);
            }
        }

        internal void GoGoLazer(PewCircle pewCircle)
        {
            Point mouse = _inputManager.GetMousePosition();
            var lazer = new LazerPewPew(pewCircle.Physics.Center, mouse, 0);
            _lazers.Clear();
            _lazers.Add(lazer);
        }
    }
}
