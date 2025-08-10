using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Management;
using Microsoft.VisualBasic.Devices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Reflection;               // << para recursos embebidos
using InfoRAMApp.Forms;
using InfoRAMApp.Helpers;
using InfoRAMApp.Utils;

namespace InfoRAMApp
{
    public partial class MainForm : Form
    {
        
        private PictureBox titlePictureBox = null!;
        private PictureBox leftIcon = null!;
        private PictureBox rightIcon = null!;
        private RichTextBox infoBox = null!;
        private Button btnActualizar = null!;
        private Button btnGuardar = null!;
        private Button btnInfo = null!;

        public MainForm()
        {
            InitializeComponent();
            MostrarInformacion();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Configuración del formulario principal
            this.Text = "Info - RAM";
            this.AutoScaleMode = AutoScaleMode.Dpi; // Habilitar escalado automático
            this.ClientSize = new Size(660, 656);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#023047");

            // Cargar icono embebido
            try
            {
                using Stream? iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("InfoRAMApp.images.logo.ico");
                if (iconStream != null)
                    this.Icon = new Icon(iconStream);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando ícono embebido: " + ex.Message);
            }

            // Layout principal con TableLayoutPanel
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // Fila para el encabezado (ahora autoajustable)
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Fila para el RichTextBox (ocupa el espacio restante)
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));  // Fila para los botones (aumentada en 15px)
            this.Controls.Add(mainLayout);

            // --- ENCABEZADO ---
            TableLayoutPanel headerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = ColorTranslator.FromHtml("#1F4B6E"),
                Margin = new Padding(0, 5, 0, 0) // Margen superior para efecto de franja
            };
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); // Icono izquierdo
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F)); // Título
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); // Icono derecho
            mainLayout.Controls.Add(headerLayout, 0, 0);

            leftIcon = CrearPictureBoxSeguroEscalado("mem.png", new Size(90, 90), Point.Empty);
            leftIcon.Dock = DockStyle.None; // Quitar Dock para que Anchor funcione
            leftIcon.Anchor = AnchorStyles.None; // Centrar en la celda
            headerLayout.Controls.Add(leftIcon, 0, 0);

            titlePictureBox = ImageUtils.CrearPictureBoxConImagenOriginal("titulo.png", Point.Empty);
            titlePictureBox.Dock = DockStyle.Fill;
            headerLayout.Controls.Add(titlePictureBox, 1, 0);

            rightIcon = CrearPictureBoxSeguroEscalado("mem.png", new Size(90, 90), Point.Empty);
            rightIcon.Dock = DockStyle.None; // Quitar Dock para que Anchor funcione
            rightIcon.Anchor = AnchorStyles.None; // Centrar en la celda
            headerLayout.Controls.Add(rightIcon, 2, 0);

            // --- ÁREA DE TEXTO ---
            // Usamos un Panel como contenedor para crear el borde 3D y el margen interno.
            Panel infoBoxContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#A1D6E2"), // Color de fondo del área de texto
                Margin = new Padding(15, 15, 15, 0),      // Margen exterior
                Padding = new Padding(10, 10, 0, 0),                   // Margen interno (entre el borde y el texto), 0 a la derecha y abajo
                BorderStyle = BorderStyle.Fixed3D          // Borde de bajo relieve
            };
            mainLayout.Controls.Add(infoBoxContainer, 0, 1);

            infoBox = new RichTextBox
            {
                Font = new Font("Segoe UI", 11, FontStyle.Bold), // El escalado de fuente ahora es automático
                ForeColor = ColorTranslator.FromHtml("#023047"),
                BackColor = ColorTranslator.FromHtml("#A1D6E2"), // Debe coincidir con el contenedor
                ReadOnly = true,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None, // El borde lo proporciona el contenedor
                ScrollBars = RichTextBoxScrollBars.Vertical,
                WordWrap = true,
                TabStop = false,
                Cursor = Cursors.Default
            };
            infoBoxContainer.Controls.Add(infoBox); // Añadir el RichTextBox dentro del Panel

            infoBox.GotFocus += (s, e) => this.ActiveControl = null;
            infoBox.MouseDown += (s, e) => { infoBox.SelectionLength = 0; this.ActiveControl = null; };
            infoBox.SelectionChanged += (s, e) => { if (infoBox.SelectionLength > 0) infoBox.SelectionLength = 0; };

            // --- BOTONES ---
            TableLayoutPanel buttonLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(10, 0, 10, 0) // Espacio a los lados
            };
            buttonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            buttonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            buttonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            mainLayout.Controls.Add(buttonLayout, 0, 2);

            btnActualizar = CrearBotonConImagen("Actualizar Datos", "boton.png", Point.Empty);
            btnActualizar.Anchor = AnchorStyles.None;
            buttonLayout.Controls.Add(btnActualizar, 0, 0);

            btnGuardar = CrearBotonConImagen("Guardar Reporte", "boton.png", Point.Empty);
            btnGuardar.Anchor = AnchorStyles.None;
            buttonLayout.Controls.Add(btnGuardar, 1, 0);

            btnInfo = CrearBotonConImagen("Información", "boton.png", Point.Empty);
            btnInfo.Anchor = AnchorStyles.None;
            buttonLayout.Controls.Add(btnInfo, 2, 0);

            btnActualizar.Click += BtnActualizar_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnInfo.Click += BtnInfo_Click;

            this.ResumeLayout(false);
        }

        

        private PictureBox CrearPictureBoxSeguroEscalado(string nombreImagen, Size size, Point location)
        {
            PictureBox pb = new PictureBox
            {
                Size = size,
                Location = location,
                SizeMode = PictureBoxSizeMode.Zoom // Cambiado de Normal a Zoom
            };

            try
            {
                Debug.WriteLine($"[DEBUG] Intentando cargar imagen: {nombreImagen}");
                using var original = ImageUtils.CargarImagenDesdeRecurso(nombreImagen);
                if (original != null)
                {
                    Debug.WriteLine($"[DEBUG] Imagen original '{nombreImagen}' cargada. Dimensiones: {original.Width}x{original.Height}");
                    pb.Image = ImageUtils.EscalarImagenAltaCalidad(original, size.Width, size.Height);
                    if (pb.Image != null)
                    {
                        Debug.WriteLine($"[DEBUG] Imagen '{nombreImagen}' escalada. Dimensiones: {pb.Image.Width}x{pb.Image.Height}");
                    }
                    else
                    {
                        Debug.WriteLine($"[DEBUG] Error: pb.Image es nulo después de escalar para '{nombreImagen}'.");
                    }
                }
                else
                {
                    Debug.WriteLine($"[DEBUG] Error: Imagen original '{nombreImagen}' es nula.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DEBUG] Excepción al cargar/escalar imagen '{nombreImagen}': {ex.Message}");
#if DEBUG
                Debug.WriteLine("Error cargando imagen embebida: " + ex.Message);
#else
                MessageBox.Show("Error cargando imagen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
            }

            return pb;
        }

        

        private ImageButton CrearBotonConImagen(string texto, string nombreImagen, Point location)
        {
            var boton = new ImageButton
            {
                Text = texto,
                Font = new Font("Segoe UI", 12, FontStyle.Bold), // El escalado de fuente ahora es automático
                ForeColor = Color.White,
                Size = new Size(180, 50),
                Location = location,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                FlatAppearance = { MouseDownBackColor = Color.Transparent, MouseOverBackColor = Color.Transparent },
                DefaultForeColor = Color.White,
                HoverForeColor = Color.Gold
            };

            try
            {
                using var img = ImageUtils.CargarImagenDesdeRecurso(nombreImagen);
                boton.BackgroundImage = ImageUtils.EscalarImagenAltaCalidad(img, 180, 50);
                boton.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Error cargando imagen de botón: " + ex.Message);
#else
                MessageBox.Show("Error cargando imagen de botón: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
            }

            return boton;
        }

        private string LimpiarCodificacion(string entrada)
        {
            return Encoding.Default.GetString(Encoding.Default.GetBytes(entrada)).Trim();
        }
        private string TraducirMemoryType(int code)
        {
            return code switch
            {
                20 => "DDR",
                21 => "DDR2",
                22 => "DDR2 FB-DIMM",
                24 => "DDR3",
                26 => "DDR4",
                27 => "LPDDR",
                28 => "LPDDR2",
                29 => "LPDDR3",
                30 => "LPDDR4",
                34 => "DDR5",
                _ => "Desconocido"
            };
        }

        private string TraducirFormFactor(int code)
        {
            return code switch
            {
                8 => "DIMM",
                12 => "SO-DIMM",
                _ => "Desconocido"
            };
        }

        private void MostrarInformacion()
        {
            string info = HardwareInfoHelper.ObtenerInformacionRAMYMotherboard(LimpiarCodificacion, TraducirMemoryType, TraducirFormFactor);

            // Limpiar el control antes de añadir texto nuevo
            infoBox.Clear();

            // Añadir el texto, que heredará la sangría establecida
            infoBox.AppendText(info);

            // Devolver el cursor al inicio y quitar la selección
            infoBox.SelectionStart = 0;
            infoBox.SelectionLength = 0;
            infoBox.ScrollToCaret();
        }

        private void BtnActualizar_Click(object? sender, EventArgs e) => MostrarInformacion();

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            using SaveFileDialog guardar = new SaveFileDialog
            {
                Filter = "Archivo de Texto (*.txt)|*.txt",
                Title = "Guardar informe de RAM y Placa Madre",
                FileName = "Reporte_RAM.txt"
            };

            if (guardar.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string banner_ascii =
@"_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ 
	 ____       _     _ _ _                   
	|  _ \ __ _| |__ | (_) |_ _   _ ___       
	| |_) / _  | '_ \| | | __| | | / __|      
	|  __/ (_| | |_) | | | |_| |_| \__ \_ _ _ 
	|_|   \__,_|_.__/|_|_|\__|\__,_|___(_|_|_)
_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _


";
                    using (var writer = new StreamWriter(guardar.FileName, false, Encoding.UTF8))
                    {
                        writer.Write(banner_ascii);
                        writer.Write(infoBox.Text);
                    }

                    MessageBox.Show("✅ El reporte se guardó correctamente.", "Guardado exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error al guardar el archivo:\n{ex.Message}", "Error de guardado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnInfo_Click(object? sender, EventArgs e)
        {
            CustomMessageBox.Mostrar();
        }
    }
}
