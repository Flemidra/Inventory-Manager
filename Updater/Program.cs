using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Updater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static int Main(string[] args)
        {
            if (args != null && args.Length > 0 && !string.IsNullOrEmpty(args[0]))
            {
                try
                {
                    GlobalVariables.TargetVersion = new Version(args[0].Trim());
                    Directory.SetCurrentDirectory(new FileInfo(Assembly.GetEntryAssembly().Location).Directory.FullName);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                    return GlobalVariables.ExitCode;
                }
                catch (Exception)
                {
                    return 5;
                }
                
            }
            return 5;
        }
    }
}
