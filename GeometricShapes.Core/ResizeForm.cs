using GeometricShapes.Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GeometricShapes.App
{
    public class ResizeForm : Form
    {
        public int? NewWidth { get; private set; }
        public int? NewHeight { get; private set; }
        public TriangleDimensions TriangleDimensions { get; private set; }

        public ResizeForm(Shape shape)
        {
            this.Text = "Промяна на размера";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;

            if (shape is TriangleShape)
            {

                InitializeTriangleControls(shape as TriangleShape);
            }
            else
            {
                InitializeStandardControls(shape.GetBoundingRectangle());
            }
        }

        private void InitializeStandardControls(Rectangle bounds)
        {
            this.Size = new Size(300, 180);

            var widthLabel = new Label { Text = "Ширина:", Left = 20, Top = 20, Width = 80 };
            var widthBox = new NumericUpDown
            {
                Left = 100,
                Top = 20,
                Width = 100,
                Minimum = 10,
                Maximum = 1000,
                Value = bounds.Width
            };

            var heightLabel = new Label { Text = "Височина:", Left = 20, Top = 60, Width = 80 };
            var heightBox = new NumericUpDown
            {
                Left = 100,
                Top = 60,
                Width = 100,
                Minimum = 10,
                Maximum = 1000,
                Value = bounds.Height
            };

            var okButton = new Button { Text = "OK", Left = 50, Top = 100, Width = 80 };
            okButton.Click += (s, e) =>
            {
                NewWidth = (int)widthBox.Value;
                NewHeight = (int)heightBox.Value;
                TriangleDimensions = null;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            var cancelButton = new Button { Text = "Отказ", Left = 150, Top = 100, Width = 80 };
            cancelButton.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.Add(widthLabel);
            this.Controls.Add(widthBox);
            this.Controls.Add(heightLabel);
            this.Controls.Add(heightBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }

        private void InitializeTriangleControls(TriangleShape triangle)
        {
            this.Size = new Size(340, 240);

            var dimensions = triangle.GetDimensions();

            var sideALabel = new Label { Text = "Страна A:", Left = 20, Top = 20, Width = 120 };
            var sideABox = CreateSideInput(dimensions.SideA, 20);

            var sideBLabel = new Label { Text = "Страна B:", Left = 20, Top = 60, Width = 120 };
            var sideBBox = CreateSideInput(dimensions.SideB, 60);

            var sideCLabel = new Label { Text = "Страна C:", Left = 20, Top = 100, Width = 120 };
            var sideCBox = CreateSideInput(dimensions.SideC, 100);

            var okButton = new Button { Text = "OK", Left = 70, Top = 150, Width = 80 };
            okButton.Click += (s, e) =>
            {
                int sideA = (int)sideABox.Value;
                int sideB = (int)sideBBox.Value;
                int sideC = (int)sideCBox.Value;

                if (!IsValidTriangle(sideA, sideB, sideC))
                {
                    MessageBox.Show(
                        "Страните трябва да са положителни и да удовлетворяват неравенството на триъгълника.",
                        "Невалиден триъгълник",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                TriangleDimensions = new TriangleDimensions(sideA, sideB, sideC);
                NewWidth = null;
                NewHeight = null;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            var cancelButton = new Button { Text = "Отказ", Left = 180, Top = 150, Width = 80 };
            cancelButton.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.Add(sideALabel);
            this.Controls.Add(sideABox);
            this.Controls.Add(sideBLabel);
            this.Controls.Add(sideBBox);
            this.Controls.Add(sideCLabel);
            this.Controls.Add(sideCBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }

        private NumericUpDown CreateSideInput(int value, int top)
        {
            return new NumericUpDown
            {
                Left = 150,
                Top = top,
                Width = 120,
                Minimum = 10,
                Maximum = 1000,
                Value = value
            };
        }

        private static bool IsValidTriangle(int sideA, int sideB, int sideC)
        {
            if (sideA <= 0 || sideB <= 0 || sideC <= 0)
            {
                return false;
            }

            return sideA + sideB > sideC && sideA + sideC > sideB || sideB + sideC > sideA;
        }
    }
}