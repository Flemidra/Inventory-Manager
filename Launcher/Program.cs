using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Launcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Directory.SetCurrentDirectory(new FileInfo(Assembly.GetEntryAssembly().Location).Directory.FullName);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
