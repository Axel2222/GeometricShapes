using System;
using System.Drawing;
using System.Windows.Forms;

namespace GeometricShapes.App
{
    public class ColorChangeForm : Form
    {
        public Color SelectedColor { get; private set; }

        public ColorChangeForm(Color currentColor)
        {
            this.Text = "Промяна на цвят";
            this.Size = new Size(300, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;

            var colorLabel = new Label { Text = "Изберете цвят:", Left = 20, Top = 20 };

            var colorPanel = new Panel
            {
                BackColor = currentColor, 
                BorderStyle = BorderStyle.FixedSingle,
                Left = 120,
                Top = 20,
                Width = 50,
                Height = 50
            };

            var selectColorButton = new Button
            {
                Text = "Избери цвят",
                Left = 20,
                Top = 80,
                Width = 100
            };

            selectColorButton.Click += (s, e) =>
            {
                using (var colorDialog = new ColorDialog())
                {
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        SelectedColor = colorDialog.Color;
                        colorPanel.BackColor = SelectedColor; 
                    }
                }
            };

            var okButton = new Button { Text = "OK", Left = 150, Top = 80, Width = 80 };
            okButton.Click += (s, e) => this.DialogResult = DialogResult.OK;

            this.Controls.AddRange(new Control[]
            {
                colorLabel, colorPanel,
                selectColorButton, okButton
            });
        }
    }
}