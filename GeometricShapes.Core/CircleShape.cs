using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace GeometricShapes.Core
{
    [Serializable]
    public class CircleShape : Shape
    {
        public int Radius { get; set; }

        public CircleShape(int x, int y, Color color, int radius)
            : base(x, y, color)
        {
            Radius = radius;
        }

        public override void Draw(Graphics g)
        {
            using (var brush = new SolidBrush(Color))
            {
                g.FillEllipse(brush, X - Radius, Y - Radius, Radius * 2, Radius * 2);
            }
        }

        public override Rectangle GetBoundingRectangle()
        {
            return new Rectangle(X - Radius, Y - Radius, Radius * 2, Radius * 2);
        }

        public override void Resize(int width, int height)
        {
            Radius = Math.Min(width, height) / 2;
        }

        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }

        public override double CalculatePerimeter()
        {
            return 2 * Math.PI * Radius;
        }

        public override string GetShapeInfo()
        {
            return $"Кръг ({X},{Y}) R={Radius}";
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Radius", Radius);
        }

        protected CircleShape(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Radius = info.GetInt32("Radius");
        }
    }
}