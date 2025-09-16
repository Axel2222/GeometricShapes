namespace GeometricShapes.Core
{
    public class MoveCommand : ICommand
    {
        private readonly Shape shape;
        private readonly int oldX;
        private readonly int oldY;
        private readonly int newX;
        private readonly int newY;

        public MoveCommand(Shape shape, int oldX, int oldY, int newX, int newY)
        {
            this.shape = shape;
            this.oldX = oldX;
            this.oldY = oldY;
            this.newX = newX;
            this.newY = newY;
        }

        public void Execute()
        {
            shape.X = newX;
            shape.Y = newY;
        }

        public void Undo()
        {
            shape.X = oldX;
            shape.Y = oldY;
        }
    }
}