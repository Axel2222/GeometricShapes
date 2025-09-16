using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.IO;

namespace GeometricShapes.Core
{
    public class DrawingManager
    {
        private readonly List<Shape> shapes = new List<Shape>();
        private readonly Stack<ICommand> undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> redoStack = new Stack<ICommand>();

        public IEnumerable<Shape> GetShapes() => shapes.AsReadOnly();

        public Shape GetShapeAt(Point location)
        {
            return shapes.LastOrDefault(shape =>
                shape.GetBoundingRectangle().Contains(location));
        }

        public void AddShape(Shape shape)
        {
            shapes.Add(shape);
        }

        public void RemoveShape(Shape shape)
        {
            shapes.Remove(shape);
        }

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear();
        }

        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                var command = undoStack.Pop();
                command.Undo();
                redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                var command = redoStack.Pop();
                command.Execute();
                undoStack.Push(command);
            }
        }
        public void ChangeShapeColor(Shape shape, Color newColor)
        {
            ExecuteCommand(new ChangeColorCommand(shape, newColor));
        }

        // LINQ операции
        public IEnumerable<Shape> GetShapesOrderedByArea() =>
            shapes.OrderBy(s => s.CalculateArea());

        public IEnumerable<Shape> GetShapesByColor(Color color) =>
            shapes.Where(s => s.Color == color);

        public double GetTotalArea() =>
            shapes.Sum(s => s.CalculateArea());

        public bool HasShapeAt(Point location) =>
            shapes.Any(s => s.GetBoundingRectangle().Contains(location));

        public void SaveToFile(string filePath)
        {
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formatter.Serialize(stream, shapes);
            }
        }

        public void LoadFromFile(string filePath)
        {
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var loadedShapes = (List<Shape>)formatter.Deserialize(stream);
                shapes.Clear();
                shapes.AddRange(loadedShapes);
            }
        }
    }
}