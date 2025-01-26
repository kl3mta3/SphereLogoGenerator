using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace SphereLogoGenerator
{
    public class ComplexLogoForm : Form
    {
        private Button saveButton;

        public ComplexLogoForm()
        {
            this.Text = "Sphere Logo Generator";
            this.Size = new Size(800, 800);
            this.BackColor = Color.Black;

            // Add save button
            saveButton = new Button
            {
                Text = "Save Logo",
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.LightGray
            };
            saveButton.Click += SaveButton_Click;
            this.Controls.Add(saveButton);

            this.Paint += new PaintEventHandler(DrawLogo);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            using (Bitmap bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.Black);
                    DrawLogoInternal(g);
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PNG Image|*.png|JPEG Image|*.jpg",
                    Title = "Save the Logo"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    ImageFormat format = filePath.EndsWith(".png") ? ImageFormat.Png : ImageFormat.Jpeg;
                    bitmap.Save(filePath, format);
                    MessageBox.Show("Logo saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void DrawLogo(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawLogoInternal(g);
        }

        private void DrawLogoInternal(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;
            int radius = 200;

            // Main sphere
            using (var gradientBrush = new LinearGradientBrush(
                new Rectangle(centerX - radius, centerY - radius, radius * 2, radius * 2),
                Color.FromArgb(30, 144, 255),
                Color.FromArgb(0, 0, 139),
                LinearGradientMode.ForwardDiagonal))
            {
                g.FillEllipse(gradientBrush, centerX - radius, centerY - radius, radius * 2, radius * 2);
            }

            // Orbiting nodes
            for (int i = 0; i < 8; i++)
            {
                double angle = i * Math.PI / 4;
                int orbitRadius = 300;
                int nodeRadius = 20;

                int nodeX = centerX + (int)(orbitRadius * Math.Cos(angle)) - nodeRadius;
                int nodeY = centerY + (int)(orbitRadius * Math.Sin(angle)) - nodeRadius;

                using (Brush nodeBrush = new SolidBrush(Color.FromArgb(255, 165, 0)))
                {
                    g.FillEllipse(nodeBrush, nodeX, nodeY, nodeRadius * 2, nodeRadius * 2);
                }
            }

            // Connecting lines
            Pen connectionPen = new Pen(Color.FromArgb(173, 216, 230), 2);
            for (int i = 0; i < 8; i++)
            {
                double angle1 = i * Math.PI / 4;
                double angle2 = (i + 1) % 8 * Math.PI / 4;

                int orbitRadius = 300;

                int x1 = centerX + (int)(orbitRadius * Math.Cos(angle1));
                int y1 = centerY + (int)(orbitRadius * Math.Sin(angle1));

                int x2 = centerX + (int)(orbitRadius * Math.Cos(angle2));
                int y2 = centerY + (int)(orbitRadius * Math.Sin(angle2));

                g.DrawLine(connectionPen, x1, y1, x2, y2);
            }

            // 3D grid lines
            Pen gridPen = new Pen(Color.Black, 2);
            for (int i = -4; i <= 4; i++)
            {
                float yOffset = i * (radius / 5.0f);
                float ellipseHeight = radius * 2 - Math.Abs(yOffset) * 2;
                g.DrawEllipse(gridPen, centerX - radius, centerY + yOffset - ellipseHeight / 2, radius * 2, ellipseHeight);
            }
            for (int i = -4; i <= 4; i++)
            {
                float xOffset = i * (radius / 5.0f);
                float ellipseWidth = radius * 2 - Math.Abs(xOffset) * 2;
                g.DrawEllipse(gridPen, centerX + xOffset - ellipseWidth / 2, centerY - radius, ellipseWidth, radius * 2);
            }

            // Keyhole
            int circleRadius = 30;
            int keyholeWidthTop = 28;
            int keyholeWidthBottom = 55;
            int keyholeHeight = 60;
            g.FillEllipse(Brushes.Black, centerX - circleRadius, centerY - 60, circleRadius * 2, circleRadius * 2);
            PointF[] keyholeShape =
            {
                new PointF(centerX - keyholeWidthTop / 2, centerY - 30),
                new PointF(centerX + keyholeWidthTop / 2, centerY - 30),
                new PointF(centerX + keyholeWidthBottom / 2, centerY + keyholeHeight),
                new PointF(centerX - keyholeWidthBottom / 2, centerY + keyholeHeight)
            };
            g.FillPolygon(Brushes.Black, keyholeShape);

            // Text
            using (Font font = new Font("Arial", 24, FontStyle.Bold))
            using (Brush textBrush = new SolidBrush(Color.White))
            {
                string text = "S.P.H.E.R.E";
                SizeF textSize = g.MeasureString(text, font);
                g.DrawString(text, font, textBrush, centerX - textSize.Width / 2, centerY - radius +55);
            }

            using (Font font = new Font("Arial", 12, FontStyle.Bold))
            using (Brush textBrush = new SolidBrush(Color.White))
            {
                string description = "Secure Peer-to-Peer";
                g.DrawString(description, font, textBrush, new RectangleF(centerX - 121, centerY + radius -125, 400, 100));
            }

            using (Font font = new Font("Arial", 12, FontStyle.Bold))
            using (Brush textBrush = new SolidBrush(Color.White))
            {
                string description = "Hosted Encryption Record";
                g.DrawString(description, font, textBrush, new RectangleF(centerX - 156, centerY + radius -95, 400, 100));
            }

            using (Font font = new Font("Arial", 12, FontStyle.Bold))
            using (Brush textBrush = new SolidBrush(Color.White))
            {
                string description = "Exchange";
                g.DrawString(description, font, textBrush, new RectangleF(centerX - 60, centerY + radius -65, 400, 100));
            }
        }

        [STAThread]
        public static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new ComplexLogoForm());
        }
    }
}
