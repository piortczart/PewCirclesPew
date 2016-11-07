using PewCircles.Extensions;
using System;
using System.Drawing;

namespace PewCircles.Game
{
    public abstract class GameObject
    {
        public int Id { get; protected set; }
        public int? CreatorId { get; protected set; }
        public bool IsDead { get; private set; }
        public PhysicsIsNot Physics = new PhysicsIsNot();

        public virtual void Render(Graphics graphics)
        {
            graphics.DrawString(Id.ToString(), new Font("monospace", 10), Brushes.Black, PointF.Add(Physics.Center, new Size(5, 10)));
        }

        public abstract void Update(TimeSpan timeDelta);

        public string Serialize()
        {
            return this.SerializeJson();
        }

        protected void Die()
        {
            IsDead = true;
        }
    }
}
