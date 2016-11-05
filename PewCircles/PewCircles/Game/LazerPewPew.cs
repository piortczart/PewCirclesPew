using PewCircles.Extensions;
using System;
using System.Drawing;

namespace PewCircles.Game
{
    public class LazerPewPew : GameObject
    {
        public LazerPewPew(PointF center, Point direction, float startingSpeed)
        {
            Physics.Speed = 300 + startingSpeed;
            Physics.Size = 2;
            Physics.Center = center;

            Physics.SetDirectionAbsolute(direction);
        }

        public override void Render(Graphics graphics)
        {
            var pp = PointF.Add(Physics.Center, Physics.GetDirectionAsPoint(9).Invert().AsSizeF());
            graphics.DrawLine(Pens.Red, Physics.Center, pp);
            graphics.DrawCircle(pp, 2, Color.Red);
        }

        public override void Update(TimeSpan timeDelta)
        {
            var direction = Physics.GetDirectionAsPoint((float)timeDelta.TotalSeconds * Physics.Speed);
            Physics.Move(direction, timeDelta);
        }
    }
}
