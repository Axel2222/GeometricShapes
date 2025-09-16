namespace GeometricShapes.Core
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}