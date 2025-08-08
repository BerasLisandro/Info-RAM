using System;
using System.Windows.Forms;

namespace InfoRAMApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
#if DEBUG
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string[] resourceNames = assembly.GetManifestResourceNames();
            System.Diagnostics.Debug.WriteLine("--- Embedded Resources ---");
            foreach (string resourceName in resourceNames)
            {
                System.Diagnostics.Debug.WriteLine(resourceName);
            }
            System.Diagnostics.Debug.WriteLine("--------------------------");
#endif
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2); // ✅ DPI-aware
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
