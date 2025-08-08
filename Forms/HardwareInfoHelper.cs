using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;
using InfoRAMApp.Utils;

namespace InfoRAMApp.Forms
{
    public static class HardwareInfoHelper
    {
        public static string ObtenerInformacionRAMYMotherboard(Func<string, string> limpiarCodificacion, Func<int, string> traducirMemoryType, Func<int, string> traducirFormFactor)
        {
            string nl = Environment.NewLine;
            var computadora = new ComputerInfo();

            double totalRAM = computadora.TotalPhysicalMemory / (1024.0 * 1024 * 1024);
            double disponibleRAM = computadora.AvailablePhysicalMemory / (1024.0 * 1024 * 1024);
            double usadoRAM = totalRAM - disponibleRAM;

            string info = "Memoria RAM información" + nl +
                          "------------------------------------------------------------" + nl +
                          $"Total:      {totalRAM:F2} GB" + nl +
                          $"Usado:      {usadoRAM:F2} GB" + nl +
                          $"Disponible: {disponibleRAM:F2} GB" + nl + nl;

            info += "Detalles de la Memoria RAM" + nl +
                    "------------------------------------------------------------" + nl +
                    $"{"Slot",-8}{"GB",-9}{"MHz",-9}{"Tipo",-10}{"Formato",-10}{"Fabricante"}" + nl;

            int totalSlots = 0;
            double capacidadMaxima = 0;

            try
            {
                using var searcherArray = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemoryArray");
                foreach (ManagementObject obj in searcherArray.Get())
                {
                    totalSlots += SafeConvert.ToInt(obj["MemoryDevices"]);
                    capacidadMaxima += SafeConvert.ToDouble(obj["MaxCapacity"]) / (1024 * 1024);
                }
            }
            catch
            {
                // Fallback si no se puede obtener la información del array de memoria
                totalSlots = 4; // Asumir 4 slots por defecto
                capacidadMaxima = 64; // Asumir 64 GB de capacidad máxima por defecto
                info += "Advertencia: No se pudo obtener información detallada del array de memoria. Se usarán valores predeterminados." + nl;
            }

            List<string> detalles = Enumerable.Repeat(string.Empty, totalSlots).ToList();
            double totalInstalada = 0;

            try
            {
                var searcherRAM = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
                foreach (ManagementObject obj in searcherRAM.Get())
                {
                    double sizeGB = SafeConvert.ToDouble(obj["Capacity"]) / (1024 * 1024 * 1024);
                    string velocidad = SafeConvert.ToStr(obj["Speed"]);
                    int tipo = SafeConvert.ToInt(obj["SMBIOSMemoryType"]);
                    int forma = SafeConvert.ToInt(obj["FormFactor"]);
                    string fabricanteRaw = SafeConvert.ToStr(obj["Manufacturer"]).Trim();
                    string locator = limpiarCodificacion(SafeConvert.ToStr(obj["DeviceLocator"]).ToUpperInvariant());

                    string fabricanteHex = SafeConvert.LimpiarCodificacion(
                        fabricanteRaw.Replace("0X", "", StringComparison.OrdinalIgnoreCase).ToUpperInvariant());
#if DEBUG
                    Debug.WriteLine($"Fabricante Raw: {fabricanteRaw}, Fabricante Hex: {fabricanteHex}");
#endif

                    string nombreFabricante = FabricantesManager.ObtenerFabricante(fabricanteHex);

                    string tipoTexto = traducirMemoryType(tipo);
                    string formatoTexto = traducirFormFactor(forma);

                    int idx = detalles.FindIndex(string.IsNullOrWhiteSpace);
                    if (idx != -1)
                    {
                        detalles[idx] = $"{idx + 1,-8}{sizeGB,-9:F1}{velocidad,-9}{tipoTexto,-10}{formatoTexto,-10}{nombreFabricante}";
                    }

                    totalInstalada += sizeGB;
                }
            }
            catch
            {
                info += "Advertencia: No se pudo obtener información detallada de los módulos de memoria RAM." + nl;
            }

            for (int i = 0; i < totalSlots; i++)
            {
                info += !string.IsNullOrWhiteSpace(detalles[i])
                    ? detalles[i] + nl
                    : $"{i + 1,-8}Vacío" + nl;
            }

            info += nl + $"Total de Memoria RAM instalada: {totalInstalada:F2} GB" + nl + nl;
            info += "Información de la Motherboard" + nl + "------------------------------------------------------------" + nl;

            try
            {
                var searcherMB = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                foreach (ManagementObject obj in searcherMB.Get())
                {
                    info += $"{SafeConvert.ToStr(obj["Manufacturer"])}, {SafeConvert.ToStr(obj["Product"])}" + nl;
                }
            }
            catch
            {
                info += "Advertencia: No se pudo obtener información de la placa madre." + nl;
            }

            info += nl + $"Tu Placa Madre soporta hasta: {capacidadMaxima:F2} GB" + nl;
            return info;
        }
    }
}
