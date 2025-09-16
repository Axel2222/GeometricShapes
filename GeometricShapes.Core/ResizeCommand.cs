using System;

namespace GeometricShapes.Core
{
    public class ResizeCommand : ICommand
    {
        private readonly Shape shape;
        private readonly int? oldWidth;
        private readonly int? oldHeight;
        private readonly int? newWidth;
        private readonly int? newHeight;
        private readonly TriangleDimensions oldTriangleDimensions;
        private readonly TriangleDimensions newTriangleDimensions;

        public ResizeCommand(Shape shape, int oldWidth, int oldHeight, int newWidth, int newHeight)
        {
            this.shape = shape ?? throw new ArgumentNullException(nameof(shape));
            this.oldWidth = oldWidth;
            this.oldHeight = oldHeight;
            this.newWidth = newWidth;
            this.newHeight = newHeight;
        }

        public ResizeCommand(TriangleShape triangle, TriangleDimensions oldDimensions, TriangleDimensions newDimensions)
        {
            shape = triangle ?? throw new ArgumentNullException(nameof(triangle));
            oldTriangleDimensions = oldDimensions ?? throw new ArgumentNullException(nameof(oldDimensions));
            newTriangleDimensions = newDimensions ?? throw new ArgumentNullException(nameof(newDimensions));
        }

        public void Execute()
        {
            if (shape is TriangleShape triangle)
            {
                if (newTriangleDimensions == null)
                {
                    throw new InvalidOperationException("Не са предоставени стойности за страните на триъгълника.");
                }

                ApplyTriangleDimensions(triangle, newTriangleDimensions);
            }
            else
            {
                ApplyStandardDimensions(newWidth, newHeight);
            }
        }

        public void Undo()
        {
            if (shape is TriangleShape triangle)
            {
                if (oldTriangleDimensions == null)
                {
                    throw new InvalidOperationException("Липсват оригиналните стойности на страните на триъгълника.");
                }

                ApplyTriangleDimensions(triangle, oldTriangleDimensions);
            }
            else
            {
                ApplyStandardDimensions(oldWidth, oldHeight);
            }
        }

        private void ApplyStandardDimensions(int? width, int? height)
        {
            if (!width.HasValue || !height.HasValue)
            {
                throw new InvalidOperationException("Размерите трябва да са предоставени за тази фигура.");
            }

            if (width.Value <= 0 || height.Value <= 0)
            {
                throw new ArgumentException("Размерите трябва да са положителни числа");
            }

            shape.Resize(width.Value, height.Value);
        }

        private static void ApplyTriangleDimensions(TriangleShape triangle, TriangleDimensions dimensions)
        {
            triangle.ResizeSides(dimensions.SideA, dimensions.SideB, dimensions.SideC);
        }
    }
}