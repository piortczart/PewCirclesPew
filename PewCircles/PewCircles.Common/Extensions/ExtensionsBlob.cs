using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace PewCircles.Extensions
{
    public static class ExtensionsBlob
    {
        public static IEnumerable<TQueueType> DequeueAll<TQueueType>(this ConcurrentQueue<TQueueType> queue)
        {
            TQueueType q;
            while (queue.TryDequeue(out q))
            {
                yield return q;
            }
        }

        public static PointF Add(this PointF a, PointF b)
        {
            return PointF.Add(a, new SizeF(b.X, b.Y));
        }

        public static PointF Subtract(this PointF a, PointF b)
        {
            return new PointF(a.X - b.X, a.Y - b.Y);
        }

        public static Point ToPoint(this PointF point)
        {
            return new Point((int)point.X, (int)point.Y);
        }

        public static Point ToPointF(this Point point)
        {
            return new Point(point.X, point.Y);
        }

        public static TType DeSerializeJson<TType>(this string json)
        {
            return new JavaScriptSerializer().Deserialize<TType>(json);
        }

        public static string SerializeJson(this object thing)
        {
            return new JavaScriptSerializer().Serialize(thing);
        }

        public static string SubstringSafe(this string text, int start, int length)
        {
            return text.Length <= start ? ""
                : text.Length - start <= length ? text.Substring(start)
                : text.Substring(start, length);
        }

        public static float DistanceTo(this PointF a, PointF b)
        {
            return (float)Math.Sqrt(Math.Pow((b.X - a.X), 2) + Math.Pow((b.Y - a.Y), 2));
        }

        public static PointF Invert(this PointF point)
        {
            return new PointF(-point.X, -point.Y);
        }

        public static SizeF AsSizeF(this PointF point)
        {
            return new SizeF(point.X, point.Y);
        }

        public static SizeF Invert(this SizeF sizeF)
        {
            return new SizeF(-sizeF.Width, -sizeF.Height);
        }

        public static Point GetPoint(this PointF pointF)
        {
            return new Point((int)Math.Round(pointF.X), (int)Math.Round(pointF.Y));
        }

        public static void DrawCircle(this Graphics graphics, PointF center, int radius, Color color)
        {
            var leftTop = new PointF(center.X - radius, center.Y - radius);
            graphics.DrawEllipse(new Pen(color), leftTop.X, leftTop.Y, radius * 2, radius * 2);
        }

        public static void DrawCircle(this Graphics graphics, Point center, int radius, Color color)
        {
            var leftTop = new Point(center.X - radius, center.Y - radius);
            graphics.DrawEllipse(new Pen(color), leftTop.X, leftTop.Y, radius * 2, radius * 2);
        }

        /// <summary>
        /// Draws the string centered on the given point.
        /// </summary>
        public static void DrawStringCentered(this Graphics graphics, string text, Font font, Brush brush,
            Point centerPosition)
        {
            Size textSize = TextRenderer.MeasureText(text, font);
            PointF position = new PointF(centerPosition.X - textSize.Width / 2, centerPosition.Y - textSize.Height / 2);
            graphics.DrawString(text, font, brush, position);
        }
    }
}
