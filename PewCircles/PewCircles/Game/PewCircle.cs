using PewCircles.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PewCircles.Game
{
    class PewCircle : GameObject
    {
        InputManager _inputManager;
        Pewness _pewness;

        public PewCircle(InputManager inputManager, Pewness pewness)
        {
            Physics.Size = 10;
            Physics.Speed = 200;
            Physics.Center = new PointF(100, 100);

            _inputManager = inputManager;
            inputManager.OnClick += Clicked;
            _pewness = pewness;
        }

        private void Clicked(MouseEventArgs e)
        {
            _pewness.GoGoLazer(this);
        }

        public override void Render(Graphics graphics)
        {
            graphics.DrawCircle(Physics.Center, (int)Physics.Size, Color.Azure);
        }

        public override void Update(TimeSpan timeDelta)
        {
            int deltaX = 0;
            int deltaY = 0;

            if (_inputManager.GetKeyState(Keys.A))
            {
                deltaX = -1;
            }
            else if (_inputManager.GetKeyState(Keys.D))
            {
                deltaX = 1;
            }

            if (_inputManager.GetKeyState(Keys.W))
            {
                deltaY = -1;
            }
            else if (_inputManager.GetKeyState(Keys.S))
            {
                deltaY = 1;
            }

            Physics.SetDirectionRelative(new Point(deltaX, deltaY));

            PointF direction = Physics.GetDirectionAsPoint((float)timeDelta.TotalSeconds * Physics.Speed);

            Physics.Move(direction, timeDelta); 
        }
    }
}
