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
        //public bool IsControllable { get; set; }
        public PointF SpawnPoint { get; set; }
    }

    public class PewCircle : GameObject
    {
        InputManager _inputManager;
        Pewness _pewness;
        Limiter _lazerLimiter;
        bool _isControlable;

        public string Name { get; set; }

        public PewCircle(InputManager inputManager, Pewness pewness, TimeSource timeSource)
        {
            Physics.Size = 10;
            Physics.Speed = 200;

            _inputManager = inputManager;
            _pewness = pewness;
            _lazerLimiter = new Limiter(TimeSpan.FromSeconds(1), timeSource);

        }

        public PewCircle(int id, string name, PhysicsIsNot physics, InputManager inputManager, Pewness pewness, TimeSource timeSource)
        {
            Physics = physics;
            Name = name;
            _isControlable = false;
        }

        public void Initialize(PewCircleSettings settings, bool isControlable)
        {
            Id = settings.Id;
            Name = settings.Name;
            Physics.Center = settings.SpawnPoint;

            _isControlable = isControlable;
            if (_isControlable)
            {
                _inputManager.OnClick += Clicked;
            }
        }
        public PewCircleSettings GetSettings()
        {
            return new PewCircleSettings
            {
                Id = Id,
                Name = Name,
                SpawnPoint = Physics.Center
            };
        }

        private void Clicked(MouseEventArgs e)
        {
            if (_lazerLimiter.CanHappen(true))
            {
                _pewness.GoGoLazer(this);
            }
        }

        public override void Render(Graphics graphics)
        {
            base.Render(graphics);

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

                Physics.SetDirectionRelative(new Point(deltaX, deltaY));

                PointF direction = Physics.GetDirectionAsPoint((float)timeDelta.TotalSeconds * Physics.Speed);

                Physics.Move(direction, timeDelta);
            }
        }
    }
}
