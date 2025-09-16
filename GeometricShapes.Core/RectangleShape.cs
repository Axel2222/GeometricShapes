using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace GeometricShapes.Core
{
    [Serializable]
    public class RectangleShape : Shape
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public RectangleShape(int x, int y, Color color, int width, int height)
            : base(x, y, color)
        {
            Width = width;
            Height = height;
        }

        public override void Draw(Graphics g)
        {
            using (var brush = new SolidBrush(Color))
            {
                g.FillRectangle(brush, X, Y, Width, Height);
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
            return Width * Height;
        }

        public override double CalculatePerimeter()
        {
            return 2 * (Width + Height);
        }

        public override string GetShapeInfo()
        {
            return $"Правоъгълник ({X},{Y}) {Width}x{Height}";
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Width", Width);
            info.AddValue("Height", Height);
        }

        protected RectangleShape(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Width = info.GetInt32("Width");
            Height = info.GetInt32("Height");
        }
    }
}