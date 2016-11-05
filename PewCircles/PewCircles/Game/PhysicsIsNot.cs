﻿using PewCircles.Extensions;
using System;
using System.Drawing;

namespace PewCircles.Game
{
    public class PhysicsIsNot
    {
        public PointF Center;
        public PointF TopLeft => new PointF(Center.X - Size / 2, Center.Y - Size / 2);
        public float Size;
        public float? DirectionRads;
        public float Speed;
        public PointF LastCenter;
        //public float CurrentSpeed;

        internal void Move(PointF delta, TimeSpan timeDelta)
        {
            LastCenter = Center;
            Center = new PointF(Center.X + delta.X, Center.Y + delta.Y);

            //CurrentSpeed = (float)(Center.DistanceTo(LastCenter)/timeDelta.TotalSeconds);
        }

        internal PointF GetDirectionAsPoint(float scale)
        {
            if (DirectionRads == null)
            {
                return PointF.Empty;
            }

            return new PointF((float)Math.Cos(DirectionRads.Value) * scale, (float)Math.Sin(DirectionRads.Value) * scale);
        }

        internal void SetDirectionAbsolute(Point heading)
        {
            if (heading == Point.Empty)
            {
                DirectionRads = null;
                return;
            }

            float xDiff = heading.X - Center.X;
            float yDiff = heading.Y - Center.Y;
            DirectionRads = (float)Math.Atan2(yDiff, xDiff);
        }

        internal void SetDirectionRelative(Point heading)
        {
            if (heading == Point.Empty)
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