using PewCircles.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PewCircles.Mechanics
{
    class Overlay
    {
        Font _font = new Font("monospace", 10);
        EventPerSecond _rendersPerSecond;

        Func<List<PewCircle>> _pewCirclesSource;

        public Overlay(Func<List<PewCircle>> pewCirclesSource, TimeSource timeSource)
        {
            _rendersPerSecond = new EventPerSecond(timeSource);
            _pewCirclesSource = pewCirclesSource;
        }


        public void Render(Graphics graphics)
        {
            _rendersPerSecond.Add();
            graphics.DrawString(_rendersPerSecond.GetPerSecond().ToString(), _font, Brushes.White, new Point(10, 250));

            if (_pewCirclesSource != null)
            {
                var circles = _pewCirclesSource();
                foreach (var circle in circles.Select((c,i)=>new { c, i }))
                {
                    graphics.DrawString(circle.c.Name.ToString(), _font, Brushes.White, new Point(10, circle.i * 25));
                }
            }
        }
    }
}
