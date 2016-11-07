using PewCircles.Extensions;
using PewCircles.Mechanics;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PewCircles.Game
{
    public class PewCircleSettings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PointF SpawnPoint { get; set; }
        public float? DirectionRads { get; set; }
    }

    public class PewCircle : GameObject
    {
        InputManager _inputManager;
        Pewness _pewness;
        Limiter _lazerLimiter;
        bool _isControlable;
        TimeSpan _lazerFrequency = TimeSpan.FromSeconds(0.5);

        public string Name { get; set; }

        public PewCircle(InputManager inputManager, Pewness pewness, TimeSource timeSource)
        {
            Physics.Size = 10;
            Physics.Speed = 200;

            _inputManager = inputManager;
            _pewness = pewness;
            _lazerLimiter = new Limiter(_lazerFrequency, timeSource);

        }

        public void Initialize(PewCircleSettings settings, bool isControlable)
        {
            Id = settings.Id;
            Name = settings.Name;
            Physics.Center = settings.SpawnPoint;
            Physics.DirectionRads = settings.DirectionRads;

            _isControlable = isControlable;
            if (_isControlable)
            {
                _inputManager.OnMouseDown += MouseDown;
            }
        }
        public PewCircleSettings GetSettings()
        {
            return new PewCircleSettings
            {
                Id = Id,
                Name = Name,
                SpawnPoint = Physics.Center,
                DirectionRads = Physics.DirectionRads
            };
        }

        private void MouseDown(MouseEventArgs e)
        {
            if (_lazerLimiter.CanHappen(true))
            {
                _pewness.GoGoLazer(this);
            }
        }

        public override void Render(Graphics graphics)
        {
            base.Render(graphics);

            PointF directionPointer1 = Physics.GetDirectionAsPointAbsolute(5);
            PointF directionPointer2 = Physics.GetDirectionAsPointAbsolute(12);
            graphics.DrawLine(Pens.Red, directionPointer1, directionPointer2);

            graphics.DrawCircle(Physics.Center, (int)Physics.Size, Color.Azure);
        }

        public override void Update(TimeSpan timeDelta)
        {
            if (_isControlable)
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

                Physics.SetDirectionRelative(new PointF(deltaX, deltaY));
            }

            // Perform the "regular" move.
            MoveOnUpdate(timeDelta);
        }
    }
}
