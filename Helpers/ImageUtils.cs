using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;

namespace InfoRAMApp.Helpers
{
    public static class ImageUtils
    {
        /// <summary>
        /// Escala una imagen conservando alta calidad, útil para UI.
        /// </summary>
        public static Image EscalarImagenAltaCalidad(Image original, int maxWidth, int maxHeight)
        {
            float ratioX = (float)maxWidth / original.Width;
            float ratioY = (float)maxHeight / original.Height;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(original.Width * ratio);
            int newHeight = (int)(original.Height * ratio);

            Bitmap bmp = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                g.Clear(Color.Transparent);
                g.DrawImage(original, 0, 0, newWidth, newHeight);
            }
            return bmp;
        }

        /// <summary>
        /// Carga una imagen embebida en el ensamblado desde el nombre de archivo (por ejemplo: "robot.png").
        /// </summary>
        public static Image CargarImagenDesdeRecurso(string nombreArchivo)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string recursoCompleto = $"InfoRAMApp.images.{nombreArchivo}"; // Intentar con el nombre completo esperado

            Stream? stream = assembly.GetManifestResourceStream(recursoCompleto);

            if (stream is null)
            {
                // Si no se encuentra con el nombre completo, intentar buscar por el final del nombre (fallback)
                string? foundResource = Array.Find(
                    assembly.GetManifestResourceNames(),
                    n => n.EndsWith(nombreArchivo, StringComparison.OrdinalIgnoreCase)
                );

                if (string.IsNullOrEmpty(foundResource))
                    throw new FileNotFoundException($"No se encontró el recurso embebido: {nombreArchivo}");

                stream = assembly.GetManifestResourceStream(foundResource);
            }

            if (stream is null)
                throw new FileNotFoundException($"El recurso '{nombreArchivo}' existe pero no pudo abrirse.");

            return Image.FromStream(stream);
        }
    }
}
