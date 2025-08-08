
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace InfoRAMApp.Utils
{
    public static class FabricantesManager
    {
        private static readonly Dictionary<string, string> Fabricantes;

        static FabricantesManager()
        {
            try
            {
                using Stream? stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("InfoRAMApp.fabricantes.json");

                if (stream != null)
                {
                    using StreamReader reader = new StreamReader(stream);
                    string json = reader.ReadToEnd();
                    var temp = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    if (temp != null)
                        Fabricantes = new Dictionary<string, string>(temp, StringComparer.OrdinalIgnoreCase);
                    else
                        Fabricantes = new Dictionary<string, string>();
                }
                else
                {
                    Fabricantes = new Dictionary<string, string>();
                }
            }
            catch
            {
                Fabricantes = new Dictionary<string, string>();
            }
        }

        public static string ObtenerFabricante(string codigo)
        {
            string limpio = SafeConvert.LimpiarCodificacion(codigo);

            // Intenta buscar por el código hexadecimal limpio
            if (Fabricantes.TryGetValue(limpio, out var nombre))
            {
                return nombre;
            }

            // Si no se encuentra por código, intenta buscar por el nombre directamente (ignorando mayúsculas/minúsculas)
            foreach (var entry in Fabricantes)
            {
                if (entry.Value.Equals(limpio, StringComparison.OrdinalIgnoreCase))
                {
                    return entry.Value; // Devuelve el nombre tal como está en el JSON
                }
            }

            return $"Código desconocido: {limpio}";
        }
    }
}
