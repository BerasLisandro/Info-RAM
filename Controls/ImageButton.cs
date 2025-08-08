using System;
using System.Drawing;
using System.Windows.Forms;
using InfoRAMApp.Helpers;

namespace InfoRAMApp
{
    public class ImageButton : Button
    {
        private Color _defaultForeColor = Color.White;
        private Color _hoverForeColor = Color.Gold;
        private Point _originalLocation;

        public ImageButton()
        {
            SetStyle(ControlStyles.Selectable, false);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            TabStop = false;

            Font = new Font("Segoe UI", 10, FontStyle.Bold);
            ForeColor = _defaultForeColor;
        }

        protected override bool ShowFocusCues => false;

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            if (Focused)
                ControlPaint.DrawFocusRectangle(pevent.Graphics, ClientRectangle, Color.Transparent, Color.Transparent);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _originalLocation = this.Location;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ForeColor = _hoverForeColor;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ForeColor = _defaultForeColor;
            Location = _originalLocation;
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            Location = new Point(_originalLocation.X, _originalLocation.Y + 1);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            Location = _originalLocation;
        }

        /// <summary>
        /// Establece la imagen de fondo usando un recurso embebido (nombre archivo, ej "boton.png").
        /// </summary>
        public void SetBackgroundImageSafe(string nombreRecurso, Color fallbackColor)
        {
            try
            {
                using var original = ImageUtils.CargarImagenDesdeRecurso(nombreRecurso);
                BackgroundImage = ImageUtils.EscalarImagenAltaCalidad(original, this.Width, this.Height);
                BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch
            {
                BackgroundImage = null;
                BackColor = fallbackColor;
            }
        }

        public Color HoverForeColor
        {
            get => _hoverForeColor;
            set => _hoverForeColor = value;
        }

        public new Color DefaultForeColor
        {
            get => _defaultForeColor;
            set
            {
                _defaultForeColor = value;
                ForeColor = value;
            }
        }
    }
}
