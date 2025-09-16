using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace GeometricShapes.Core
{
    [Serializable]
    public class EllipseShape : Shape
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public EllipseShape(int x, int y, Color color, int width, int height)
            : base(x, y, color)
        {
            Width = width;
            Height = height;
        }

        public override void Draw(Graphics g)
        {
            using (var brush = new SolidBrush(Color))
            {
                g.FillEllipse(brush, X, Y, Width, Height);
            }
        }

        public override Rectangle GetBoundingRectangle()
        {
            return new Rectangle(X, Y, Width, Height);
        }

        public override void Resize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override double CalculateArea()
        {
            return Math.PI * (Width / 2.0) * (Height / 2.0);
        }

        public override double CalculatePerimeter()
        {
            // Approximation for ellipse perimeter
            double a = Width / 2.0;
            double b = Height / 2.0;
            return Math.PI * (3 * (a + b) - Math.Sqrt((3 * a + b) * (a + 3 * b)));
        }

        public override string GetShapeInfo()
        {
            return $"Елипса ({X},{Y}) {Width}x{Height}";
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Width", Width);
            info.AddValue("Height", Height);
        }

        protected EllipseShape(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Width = info.GetInt32("Width");
            Height = info.GetInt32("Height");
        }
    }
}