using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace GeometricShapes.Core
{
    [Serializable]
    public abstract class Shape
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }

        protected Shape(int x, int y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
        }

        public virtual void Move(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        public abstract void Draw(Graphics g);
        public abstract Rectangle GetBoundingRectangle();
        public abstract void Resize(int width, int height);
        public abstract double CalculateArea();
        public abstract double CalculatePerimeter();
        public abstract string GetShapeInfo();

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X", X);
            info.AddValue("Y", Y);
            info.AddValue("Color", Color.ToArgb());
        }

        protected Shape(SerializationInfo info, StreamingContext context)
        {
            X = info.GetInt32("X");
            Y = info.GetInt32("Y");
            Color = Color.FromArgb(info.GetInt32("Color"));
        }
    }
}





















































































































































