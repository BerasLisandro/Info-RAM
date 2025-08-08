using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using InfoRAMApp.Helpers;

namespace InfoRAMApp
{
    public class CustomMessageBox : Form
    {
        public static void Mostrar()
        {
            using var ventana = new CustomMessageBox();
            ventana.ShowDialog();
        }

        public CustomMessageBox()
        {
            this.Text = "Información";
            this.AutoScaleMode = AutoScaleMode.Dpi; // Habilitar escalado automático
            this.Size = new Size(420, 270); // Ancho aumentado para mejor distribución
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(2, 48, 71); // #023047

            try
            {
                using Stream? iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("InfoRAMApp.images.logo.ico");
                if (iconStream != null)
                    this.Icon = new Icon(iconStream);
            }
            catch { }

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = Color.Transparent
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Fila para el botón Cerrar
            this.Controls.Add(mainLayout);

            Label texto = new Label
            {
                Text = @"Desarrollado por:
Pablo Téllez A.

Tarija - 2025",
                Font = ScaleFont(new Font("Comic Sans MS", 14, FontStyle.Bold)), // Escalado de fuente
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
                AutoSize = true // Añadido para autoescalado de texto
            };
            mainLayout.Controls.Add(texto, 0, 0);

            PictureBox robot = new PictureBox
            {
                Size = new Size(150, 150),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill,
                Margin = new Padding(5, 20, 15, 0)
            };
            try
            {
                using var rawImage = ImageUtils.CargarImagenDesdeRecurso("robot.png");
                robot.Image = ImageUtils.EscalarImagenAltaCalidad(rawImage, robot.Width, robot.Height);
            }
            catch { }
            mainLayout.Controls.Add(robot, 1, 0);

            Button btnCerrar = new Button
            {
                Text = "Cerrar",
                Font = ScaleFont(new Font("Segoe UI", 10, FontStyle.Bold)), // Escalado de fuente
                ForeColor = Color.White,
                Size = new Size(100, 36),
                FlatStyle = FlatStyle.Flat,
                BackgroundImageLayout = ImageLayout.Stretch,
                TabStop = false,
                Dock = DockStyle.None,
                Anchor = AnchorStyles.Bottom, // Anclar al fondo
                Margin = new Padding(0, 0, 0, 15) // Margen inferior de 15px
            };

            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnCerrar.FlatAppearance.MouseOverBackColor = Color.Transparent;

            try
            {
                using var rawImageBtn = ImageUtils.CargarImagenDesdeRecurso("boton.png");
                btnCerrar.BackgroundImage = ImageUtils.EscalarImagenAltaCalidad(rawImageBtn, btnCerrar.Width, btnCerrar.Height);
            }
            catch
            {
                btnCerrar.BackColor = Color.FromArgb(0, 158, 166);
            }

            btnCerrar.MouseEnter += (s, e) => btnCerrar.ForeColor = Color.Gold;
            btnCerrar.MouseLeave += (s, e) => btnCerrar.ForeColor = Color.White;
            btnCerrar.MouseDown += (s, e) => btnCerrar.Location = new Point(btnCerrar.Left, btnCerrar.Top + 1);
            btnCerrar.MouseUp += (s, e) => btnCerrar.Location = new Point(btnCerrar.Left, btnCerrar.Top - 1);

            btnCerrar.Click += (s, e) => this.Close();
            mainLayout.Controls.Add(btnCerrar, 0, 1);
            mainLayout.SetColumnSpan(btnCerrar, 2); // Ocupar ambas columnas
        }

        // Método auxiliar para escalar fuentes (copiado de MainForm)
        private Font ScaleFont(Font originalFont)
        {
            float currentDpi = this.DeviceDpi;
            float defaultDpi = 96f; // DPI estándar de Windows

            // Calcula el factor de escalado
            float scaleFactor = currentDpi / defaultDpi;

            // Escala el tamaño de la fuente
            float newSize = originalFont.Size * scaleFactor;

            return new Font(originalFont.FontFamily, newSize, originalFont.Style);
        }
    }
}