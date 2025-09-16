namespace GeometricShapes.Core
{
    public class RemoveShapeCommand : ICommand
    {
        private readonly DrawingManager manager;
        private readonly Shape shape;

        public RemoveShapeCommand(DrawingManager manager, Shape shape)
        {
            this.manager = manager;
            this.shape = shape;
        }

        public void Execute()
        {
            manager.RemoveShape(shape);
        }

        public void Undo()
        {
            manager.AddShape(shape);
        }
    }
}