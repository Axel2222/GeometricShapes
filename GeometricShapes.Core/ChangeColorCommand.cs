using System.Drawing;

namespace GeometricShapes.Core
{
    public class ChangeColorCommand : ICommand
    {
        private readonly Shape shape;
        private readonly Color oldColor;
        private readonly Color newColor;

        public ChangeColorCommand(Shape shape, Color newColor)
        {
            this.shape = shape;
            this.oldColor = shape.Color;
            this.newColor = newColor;
        }

        public void Execute() => shape.Color = newColor;
        public void Undo() => shape.Color = oldColor;
    }
}