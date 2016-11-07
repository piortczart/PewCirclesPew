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
        TimeSpan _lifeSpan = TimeSpan.FromSeconds(1);

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

            Physics.SetDirectionRelative(settings.Direction);
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

            PointF directionPointer = PointF.Add(Physics.Center, Physics.GetDirectionAsPoint(9).Invert().AsSizeF());
            graphics.DrawLine(Pens.Red, Physics.Center, directionPointer);

            graphics.DrawCircle(directionPointer, 2, Color.Red);
        }

        public override void Update(TimeSpan timeDelta)
        {
            // Do nothing if dead.
            if (IsDead) { return; }

            // Is it time to die?
            if (_timeSource != null && _creationTime != null)
            {
                if (_creationTime.Value.Add(_lifeSpan) < _timeSource.ElapsedSinceStart)
                {
                    Die();
                    return;
                }
            }

            // Perform the "regular" move.
            MoveOnUpdate(timeDelta);
        }
    }
}
