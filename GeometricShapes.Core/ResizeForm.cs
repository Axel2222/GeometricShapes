using GeometricShapes.Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GeometricShapes.App
{
    public class ResizeForm : Form
    {
        public int NewWidth { get; private set; }
        public int NewHeight { get; private set; }

        public ResizeForm(Shape shape)
        {
            var bounds = shape.GetBoundingRectangle();

            this.Text = "Промяна на размера";
            this.Size = new Size(300, 180);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;

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
    }
}