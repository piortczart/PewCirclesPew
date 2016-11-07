using PewCircles.Extensions;
using System;
using System.Drawing;

namespace PewCircles.Game
{
    public class PhysicsIsNot
    {
        public PointF Center { get; set; }
        public PointF TopLeft => new PointF(Center.X - Size / 2, Center.Y - Size / 2);
        public float Size { get; set; }
        public float? DirectionRads { get; set; }
        public float Speed { get; set; }
        public PointF LastCenter { get; set; }

        internal void Move(PointF delta, TimeSpan timeDelta)
        {
            LastCenter = Center;
            Center = new PointF(Center.X + delta.X, Center.Y + delta.Y);
        }

        internal PointF GetDirectionAsPointAbsolute(float scale)
        {
            return Center.Add(GetDirectionAsPoint(scale));
        }

        internal PointF GetDirectionAsPoint(float scale)
        {
            if (DirectionRads == null)
            {
                return PointF.Empty;
            }

            return new PointF((float)Math.Cos(DirectionRads.Value) * scale, (float)Math.Sin(DirectionRads.Value) * scale);
        }
        
        internal void SetDirectionRelative(PointF heading)
        {
            if (heading == PointF.Empty)
            {
                DirectionRads = null;
                return;
            }

            float xDiff = heading.X;
            float yDiff = heading.Y;
            DirectionRads = (float)Math.Atan2(yDiff, xDiff);
        }
    }
}
