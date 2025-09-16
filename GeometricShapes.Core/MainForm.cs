using GeometricShapes.Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GeometricShapes.App
{
    public partial class MainForm : Form
    {
        private readonly DrawingManager manager = new DrawingManager();
        private ShapeType currentShapeType = ShapeType.Rectangle;
        private Color currentColor = Color.Red;
        private Point? dragStart;
        private Shape selectedShape;
        private bool isDragging;

        private Label infoLabel;
        private Panel drawingPanel;
        private FlowLayoutPanel actionPanel;
        private Button deleteButton;
        private Button calculateButton;
        private Button resizeButton;
        private Button colorChangeButton;
        private Button saveButton;
        private Button loadButton;

        public MainForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Геометричен редактор";
            this.Size = new Size(970, 600);
            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;

            // Главен контейнер
            var mainContainer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                RowStyles =
                {
                    new RowStyle(SizeType.Percent, 5),
                    new RowStyle(SizeType.Percent, 85),
                    new RowStyle(SizeType.Percent, 10)
                }
            };

            var toolPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray
            };

            foreach (ShapeType type in Enum.GetValues(typeof(ShapeType)))
            {
                var btn = new Button
                {
                    Text = type.ToString(),
                    Width = 80,
                    Tag = type
                };
                btn.Click += ShapeButton_Click;
                toolPanel.Controls.Add(btn);
            }

            var colorBtn = new Button
            {
                Text = "Цвят",
                Width = 80
            };
            colorBtn.Click += ColorButton_Click;
            toolPanel.Controls.Add(colorBtn);

            var undoBtn = new Button { Text = "Отмени (Ctrl+Z)", Width = 120 };
            undoBtn.Click += (s, e) => UndoAction();

            var redoBtn = new Button { Text = "Повтори (Ctrl+Y)", Width = 120 };
            redoBtn.Click += (s, e) => RedoAction();

            saveButton = new Button { Text = "Запази", Width = 80 };
            saveButton.Click += SaveButton_Click;

            loadButton = new Button { Text = "Отвори", Width = 80 };
            loadButton.Click += LoadButton_Click;

            toolPanel.Controls.Add(undoBtn);
            toolPanel.Controls.Add(redoBtn);
            toolPanel.Controls.Add(saveButton);
            toolPanel.Controls.Add(loadButton);

            drawingPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            drawingPanel.MouseDown += DrawingPanel_MouseDown;
            drawingPanel.MouseMove += DrawingPanel_MouseMove;
            drawingPanel.MouseUp += DrawingPanel_MouseUp;
            drawingPanel.Paint += DrawingPanel_Paint;

            actionPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Visible = false,
                BackColor = Color.LightBlue
            };

            deleteButton = new Button { Text = "Изтрий", Width = 80 };
            deleteButton.Click += DeleteButton_Click;

            calculateButton = new Button { Text = "Изчисли", Width = 80 };
            calculateButton.Click += CalculateButton_Click;

            resizeButton = new Button { Text = "Промени размер", Width = 150 };
            resizeButton.Click += ResizeButton_Click;

            colorChangeButton = new Button { Text = "Промени цвят", Width = 100 };
            colorChangeButton.Click += ColorChangeButton_Click;

            actionPanel.Controls.Add(deleteButton);
            actionPanel.Controls.Add(calculateButton);
            actionPanel.Controls.Add(resizeButton);
            actionPanel.Controls.Add(colorChangeButton);

            infoLabel = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.LightGreen
            };

          
            mainContainer.Controls.Add(toolPanel, 0, 0);
            mainContainer.Controls.Add(drawingPanel, 0, 1);
            mainContainer.Controls.Add(actionPanel, 0, 2);

            this.Controls.Add(mainContainer);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z) UndoAction();
            if (e.Control && e.KeyCode == Keys.Y) RedoAction();
        }

        private void UndoAction()
        {
            manager.Undo();
            drawingPanel.Invalidate();
            UpdateStatus();
        }

        private void RedoAction()
        {
            manager.Redo();
            drawingPanel.Invalidate();
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (selectedShape != null)
            {
                infoLabel.Text = selectedShape.GetShapeInfo();
            }
        }

        private void UpdateActionPanel()
        {
            actionPanel.Visible = selectedShape != null;
        }

        private Shape CreateNewShape(int x, int y)
        {
            switch (currentShapeType)
            {
                case ShapeType.Rectangle:
                    return new RectangleShape(x, y, currentColor, 100, 60);
                case ShapeType.Circle:
                    return new CircleShape(x, y, currentColor, 50);
                case ShapeType.Triangle:
                    return new TriangleShape(x, y, currentColor, 80, 80, 80);
                case ShapeType.Square:
                    return new RectangleShape(x, y, currentColor, 80, 80);
                case ShapeType.Ellipse:
                    return new EllipseShape(x, y, currentColor, 120, 80);
                default:
                    return null;
            }
        }

        private void ShapeButton_Click(object sender, EventArgs e)
        {
            currentShapeType = (ShapeType)((Button)sender).Tag;
            actionPanel.Visible = false;
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    currentColor = colorDialog.Color;
                }
            }
        }
        private void ColorChangeButton_Click(object sender, EventArgs e)
        {
            if (selectedShape != null)
            {
                using (var colorChangeForm = new ColorChangeForm(selectedShape.Color))
                {
                    if (colorChangeForm.ShowDialog() == DialogResult.OK)
                    {
                        manager.ChangeShapeColor(selectedShape, colorChangeForm.SelectedColor);
                        drawingPanel.Invalidate();
                    }
                }
            }
        }

        private void DrawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            selectedShape = manager.GetShapeAt(e.Location);
            dragStart = e.Location;
            isDragging = selectedShape != null;

            if (!isDragging)
            {
                Shape newShape = CreateNewShape(e.X, e.Y);
                if (newShape != null)
                {
                    manager.ExecuteCommand(new AddShapeCommand(manager, newShape));
                    selectedShape = newShape;
                    drawingPanel.Invalidate();
                }
            }
            UpdateActionPanel();
        }

        private void DrawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && dragStart.HasValue && selectedShape != null)
            {
                int dx = e.X - dragStart.Value.X;
                int dy = e.Y - dragStart.Value.Y;
                selectedShape.Move(dx, dy);
                dragStart = e.Location;
                drawingPanel.Invalidate();
            }
        }

        private void DrawingPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedShape != null && dragStart.HasValue)
            {
                int dx = selectedShape.X - dragStart.Value.X;
                int dy = selectedShape.Y - dragStart.Value.Y;

                manager.ExecuteCommand(new MoveCommand(
                    selectedShape,
                    dragStart.Value.X,
                    dragStart.Value.Y,
                    selectedShape.X,
                    selectedShape.Y));
            }
            isDragging = false;
            dragStart = null;
            UpdateStatus();
        }

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            foreach (var shape in manager.GetShapes())
            {
                shape.Draw(e.Graphics);

                if (shape == selectedShape)
                {
                    using (var pen = new Pen(Color.Black, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                    {
                        var rect = shape.GetBoundingRectangle();
                        e.Graphics.DrawRectangle(pen, rect);
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (selectedShape != null)
            {
                manager.ExecuteCommand(new RemoveShapeCommand(manager, selectedShape));
                selectedShape = null;
                actionPanel.Visible = false;
                drawingPanel.Invalidate();
                infoLabel.Text = "";
            }
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (selectedShape != null)
            {
                string calculations = $"Лице: {selectedShape.CalculateArea():F2} | Периметър: {selectedShape.CalculatePerimeter():F2}";
                infoLabel.Text = calculations;
                MessageBox.Show(calculations, "Резултати от изчисления");
            }
        }

        private void ResizeButton_Click(object sender, EventArgs e)
        {
            if (selectedShape != null)
            {
                if (selectedShape is TriangleShape triangleShape)
                {
                    var oldDimensions = triangleShape.GetDimensions();

                    using (var resizeForm = new ResizeForm(triangleShape))
                    {
                        if (resizeForm.ShowDialog() == DialogResult.OK && resizeForm.TriangleDimensions != null)
                        {
                            manager.ExecuteCommand(new ResizeCommand(
                                triangleShape,
                                oldDimensions,
                                resizeForm.TriangleDimensions));

                            drawingPanel.Invalidate();
                        }
                    }
                }
                else
                {
                    var oldBounds = selectedShape.GetBoundingRectangle();

                    using (var resizeForm = new ResizeForm(selectedShape))
                    {
                        if (resizeForm.ShowDialog() == DialogResult.OK && resizeForm.NewWidth.HasValue && resizeForm.NewHeight.HasValue)
                        {
                            manager.ExecuteCommand(new ResizeCommand(
                                selectedShape,
                                oldBounds.Width,
                                oldBounds.Height,
                                resizeForm.NewWidth.Value,
                                resizeForm.NewHeight.Value));

                            drawingPanel.Invalidate();
                        }
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Shape files (*.shp)|*.shp|All files (*.*)|*.*";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    manager.SaveToFile(saveDialog.FileName);
                }
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "Shape files (*.shp)|*.shp|All files (*.*)|*.*";
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    manager.LoadFromFile(openDialog.FileName);
                    drawingPanel.Invalidate();
                }
            }
        }
    }

    public enum ShapeType
    {
        Rectangle,
        Circle,
        Triangle,
        Square,
        Ellipse
    }
}