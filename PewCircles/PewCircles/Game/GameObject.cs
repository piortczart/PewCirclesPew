using System;
using System.Drawing;

namespace PewCircles.Game
{
    public abstract class GameObject
    {
        public PhysicsIsNot Physics = new PhysicsIsNot();

        public abstract void Render(Graphics graphics);
        public abstract void Update(TimeSpan timeDelta);
    }
}
