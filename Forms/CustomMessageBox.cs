using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using InfoRAMApp.Helpers;

namespace InfoRAMApp
{
    public class CustomMessageBox : Form
    {
        private PictureBox infoPictureBox = null!;
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

            Panel infoPanel = new Panel
            {
                Size = new Size(300, 200), // Tamaño explícito para el panel contenedor de la imagen
                Anchor = AnchorStyles.None, // Centrar el panel dentro de la celda del TableLayoutPanel
                BackColor = Color.Transparent, // Fondo transparente
                Padding = new Padding(35, 25, 15, 25) // Ajustar padding para mover la imagen a la derecha
            };
            infoPictureBox = ImageUtils.CrearPictureBoxConImagenOriginal("informacion.png", Point.Empty);
            infoPictureBox.Dock = DockStyle.Fill; // La imagen rellena el panel
            infoPictureBox.SizeMode = PictureBoxSizeMode.Zoom; // Asegurar que escala dentro de la caja
            infoPanel.Controls.Add(infoPictureBox);
            mainLayout.Controls.Add(infoPanel, 0, 0);

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
                Font = new Font("Segoe UI", 10, FontStyle.Bold), // El escalado de fuente ahora es automático
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

        
    }
}