using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace GeometricShapes.Core
{
    [Serializable]
    public class TriangleShape : Shape
    {
        public int SideA { get; private set; }
        public int SideB { get; private set; }
        public int SideC { get; private set; }

        public TriangleShape(int x, int y, Color color, int sideA, int sideB, int sideC)
            : base(x, y, color)
        {
            ResizeSides(sideA, sideB, sideC);
        }

        public override void Draw(Graphics g)
        {
            var points = GetDrawingPoints();

            using (var brush = new SolidBrush(Color))
            {
                g.FillPolygon(brush, points);
            }
        }

        public override Rectangle GetBoundingRectangle()
        {
            var vertices = GetVertices();

            float minX = vertices[0].X;
            float maxX = vertices[0].X;
            float minY = vertices[0].Y;
            float maxY = vertices[0].Y;

            for (int i = 1; i < vertices.Length; i++)
            {
                var point = vertices[i];
                if (point.X < minX) minX = point.X;
                if (point.X > maxX) maxX = point.X;
                if (point.Y < minY) minY = point.Y;
                if (point.Y > maxY) maxY = point.Y;
            }

            int left = (int)Math.Floor(minX);
            int top = (int)Math.Floor(minY);
            int right = (int)Math.Ceiling(maxX);
            int bottom = (int)Math.Ceiling(maxY);

            int width = right - left;
            int height = bottom - top;

            if (width <= 0) width = 1;
            if (height <= 0) height = 1;

            return new Rectangle(left, top, width, height);
        }

        public override void Resize(int width, int height)
        {
            var bounds = GetBoundingRectangle();
            if (bounds.Width == 0 || bounds.Height == 0)
            {
                return;
            }

            float scaleX = width / (float)bounds.Width;
            float scaleY = height / (float)bounds.Height;
            float scale = Math.Min(scaleX, scaleY);

            if (scale <= 0)
            {
                throw new ArgumentException("Размерите трябва да са положителни числа");
            }

            ResizeSides(
                Math.Max(1, (int)Math.Round(SideA * scale)),
                Math.Max(1, (int)Math.Round(SideB * scale)),
                Math.Max(1, (int)Math.Round(SideC * scale)));
        }

        public void ResizeSides(int sideA, int sideB, int sideC)
        {
            ValidateSides(sideA, sideB, sideC);

            SideA = sideA;
            SideB = sideB;
            SideC = sideC;
        }

        public TriangleDimensions GetDimensions()
        {
            return new TriangleDimensions(SideA, SideB, SideC);
        }

        public override double CalculateArea()
        {
            double semiPerimeter = (SideA + SideB + SideC) / 2.0;
            double area = semiPerimeter * (semiPerimeter - SideA) * (semiPerimeter - SideB) * (semiPerimeter - SideC);
            return Math.Sqrt(Math.Max(area, 0));
        }

        public override double CalculatePerimeter()
        {
            return SideA + SideB + SideC;
        }

        public override string GetShapeInfo()
        {
            return $"Триъгълник ({X},{Y}) A={SideA}, B={SideB}, C={SideC}";
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("SideA", SideA);
            info.AddValue("SideB", SideB);
            info.AddValue("SideC", SideC);
        }

        protected TriangleShape(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            try
            {
                SideA = info.GetInt32("SideA");
                SideB = info.GetInt32("SideB");
                SideC = info.GetInt32("SideC");
            }
            catch (SerializationException)
            {
                int size = info.GetInt32("Size");
                SideA = size;
                SideB = size;
                SideC = size;
            }
        }

        private Point[] GetDrawingPoints()
        {
            var vertices = GetVertices();
            var drawingPoints = new Point[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                drawingPoints[i] = Point.Round(vertices[i]);
            }

            return drawingPoints;
        }

        private PointF[] GetVertices()
        {
            var localVertices = GetCenteredLocalVertices();
            var absoluteVertices = new PointF[localVertices.Length];

            for (int i = 0; i < localVertices.Length; i++)
            {
                absoluteVertices[i] = new PointF(localVertices[i].X + X, localVertices[i].Y + Y);
            }

            return absoluteVertices;
        }

        private PointF[] GetCenteredLocalVertices()
        {
            double baseLength = SideC;
            double leftSide = SideA;
            double rightSide = SideB;

            double halfBase = baseLength / 2.0;
            double apexX = (leftSide * leftSide - rightSide * rightSide) / (2.0 * baseLength);
            double apexYPart = leftSide * leftSide - Math.Pow(apexX + halfBase, 2.0);
            if (apexYPart < 0)
            {
                apexYPart = 0;
            }

            double apexY = Math.Sqrt(apexYPart);

            var vertices = new[]
            {
                new PointF((float)(-halfBase), 0f),
                new PointF((float)halfBase, 0f),
                new PointF((float)apexX, (float)(-apexY))
            };

            float minX = vertices[0].X;
            float maxX = vertices[0].X;
            float minY = vertices[0].Y;
            float maxY = vertices[0].Y;

            for (int i = 1; i < vertices.Length; i++)
            {
                var point = vertices[i];
                if (point.X < minX) minX = point.X;
                if (point.X > maxX) maxX = point.X;
                if (point.Y < minY) minY = point.Y;
                if (point.Y > maxY) maxY = point.Y;
            }

            float centerX = (minX + maxX) / 2f;
            float centerY = (minY + maxY) / 2f;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new PointF(vertices[i].X - centerX, vertices[i].Y - centerY);
            }

            return vertices;
        }

        private static void ValidateSides(int sideA, int sideB, int sideC)
        {
            if (sideA <= 0 || sideB <= 0 || sideC <= 0)
            {
                throw new ArgumentException("Страните трябва да са положителни числа");
            }

            if (sideA + sideB <= sideC || sideA + sideC <= sideB || sideB + sideC <= sideA)
            {
                throw new ArgumentException("Страните трябва да удовлетворяват неравенството на триъгълника");
            }
        }
    }

    public class TriangleDimensions
    {
        public int SideA { get; }
        public int SideB { get; }
        public int SideC { get; }

        public TriangleDimensions(int sideA, int sideB, int sideC)
        {
            SideA = sideA;
            SideB = sideB;
            SideC = sideC;
        }
    }
}
