using PewCircles.Extensions;
using PewCircles.Mechanics;
using System;
using System.Drawing;

namespace PewCircles.Game
{

    public class LazerPewPewSettings
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public PointF SpawnPoint { get; set; }
        public PointF Direction { get; set; }
    }

    public class LazerPewPew : GameObject
    {
        TimeSource _timeSource;
        TimeSpan? _creationTime;

        public LazerPewPew(TimeSource timeSource)
        {
            _timeSource = timeSource;
            _creationTime = timeSource.ElapsedSinceStart;
        }

        public void Initialize(LazerPewPewSettings settings)
        {
            Id = settings.Id;
            CreatorId = settings.CreatorId;

            Physics.Speed = 300;
            Physics.Size = 2;
            Physics.Center = settings.SpawnPoint;

            Physics.SetDirectionAbsolute(settings.Direction);
        }

        public LazerPewPewSettings GetSettings()
        {
            return new LazerPewPewSettings
            {
                Id = Id,
                CreatorId = CreatorId.Value,
                Direction = Physics.GetDirectionAsPoint(1),
                SpawnPoint = Physics.Center
            };
        }

        public override void Render(Graphics graphics)
        {
            if (IsDead)
            {
                return;
            }

            base.Render(graphics);

            var pp = PointF.Add(Physics.Center, Physics.GetDirectionAsPoint(9).Invert().AsSizeF());
            graphics.DrawLine(Pens.Red, Physics.Center, pp);
            graphics.DrawCircle(pp, 2, Color.Red);
        }

        public override void Update(TimeSpan timeDelta)
        {
            if (IsDead)
            {
                return;
            }

            if (_timeSource != null && _creationTime != null)
            {
                if (_creationTime.Value.Add(TimeSpan.FromSeconds(1)) < _timeSource.ElapsedSinceStart)
                {
                    Die();
                    return;
                }
            }

            var direction = Physics.GetDirectionAsPoint((float)timeDelta.TotalSeconds * Physics.Speed);
            Physics.Move(direction, timeDelta);
        }
    }
}
