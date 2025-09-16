using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace GeometricShapes.Core
{
    [Serializable]
    public class TriangleShape : Shape
    {
        public int Size { get; set; }

        public TriangleShape(int x, int y, Color color, int size)
            : base(x, y, color)
        {
            Size = size;
        }

        public override void Draw(Graphics g)
        {
            Point[] points = {
                new Point(X, Y - Size),
                new Point(X + Size, Y + Size),
                new Point(X - Size, Y + Size)
            };

            using (var brush = new SolidBrush(Color))
            {
                g.FillPolygon(brush, points);
            }
        }

        public override Rectangle GetBoundingRectangle()
        {
            // Fix the bounding rectangle calculation
            return new Rectangle(X - Size, Y - Size, Size * 2, Size * 2);
        }

        public override void Resize(int width, int height)
        {
            Size = Math.Max(width, height) / 2;
        }

        public override double CalculateArea()
        {
            return (Math.Sqrt(3) / 4) * Size * Size;
        }

        public override double CalculatePerimeter()
        {
            return 3 * Size;
        }

        public override string GetShapeInfo()
        {
            return $"Триъгълник ({X},{Y}) Размер={Size}";
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Size", Size);
        }

        protected TriangleShape(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Size = info.GetInt32("Size");
        }
    }
}