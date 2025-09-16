namespace GeometricShapes.Core
{
    public class AddShapeCommand : ICommand
    {
        private readonly DrawingManager manager;
        private readonly Shape shape;

        public AddShapeCommand(DrawingManager manager, Shape shape)
        {
            this.manager = manager;
            this.shape = shape;
        }

        public void Execute()
        {
            manager.AddShape(shape);
        }

        public void Undo()
        {
            manager.RemoveShape(shape);
        }
    }
}