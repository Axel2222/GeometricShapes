using System;

namespace GeometricShapes.Core
{
    public class ResizeCommand : ICommand
    {
        private readonly Shape shape;
        private readonly int oldWidth;
        private readonly int oldHeight;
        private readonly int newWidth;
        private readonly int newHeight;

        public ResizeCommand(Shape shape, int oldWidth, int oldHeight, int newWidth, int newHeight)
        {
            this.shape = shape;
            this.oldWidth = oldWidth;
            this.oldHeight = oldHeight;
            this.newWidth = newWidth;
            this.newHeight = newHeight;
        }

        public void Execute()
        {
            if (newWidth <= 0 || newHeight <= 0)
                throw new ArgumentException("Размерите трябва да са положителни числа");

            shape.Resize(newWidth, newHeight);
        }

        public void Undo()
        {
            shape.Resize(oldWidth, oldHeight);
        }
    }
}