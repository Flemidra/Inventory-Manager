using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Launcher
{
    public partial class Form1 : Form
    {
        private Thread _thread;
        //private const string Url = "http://flemidra-repository.com/inventory-manager/";
        private const string Url = "http://localhost/inventory-manager/";
        private const string AppVersionTxt = "AppVersion.txt";
        private const string UpdaterVersionTxt = "UpdaterVersion.txt";
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Shown(object sender, System.EventArgs e)
        {
            if (_thread == null)
            {
                _thread = new Thread(BackgroundTask);
                _thread.Name = "Background Task";
                _thread.IsBackground = true;
                _thread.Priority = ThreadPriority.Lowest;
                _thread.Start();
            }
        }

        private void BackgroundTask()
        {
            try
            {
                StreamReader file;
                string tmp;
                var webClient = new WebClient();
               

                // Creacion de versiones locales
                if (!File.Exists(UpdaterVersionTxt) || !File.Exists("Updater.exe") || new FileInfo("Updater.exe").Length < 0)
                {
                    File.WriteAllText(UpdaterVersionTxt, "0.0.0.0");
                }

                file = new StreamReader(UpdaterVersionTxt);
                tmp = file.ReadLine();
                file.Close();
                var localUpdaterVersion = Version.Parse(tmp.Trim());

                if (!File.Exists(AppVersionTxt))
                {
                    StreamWriter newFile = File.AppendText(AppVersionTxt);
                    newFile.WriteLine("0.0.0.0");
                    newFile.Close();
                }

                file = new StreamReader(AppVersionTxt);
                tmp = file.ReadLine();
                file.Close();
                var localAppVersion = Version.Parse(tmp.Trim());

                tmp = webClient.DownloadString(Url + UpdaterVersionTxt);
                var remoteUpdaterVersion = Version.Parse(tmp.Trim());
                if (remoteUpdaterVersion > localUpdaterVersion)
                {
                    if (File.Exists("Updater.exe"))
                    {
                        File.Delete("Updater.exe");
                    }

                    webClient.DownloadFile(Url + "Updater.exe", "Updater.exe");
                    File.WriteAllText(UpdaterVersionTxt, remoteUpdaterVersion.ToString());
                }
                //FIN UPDATER

                //Descarga nueva  APPVERSION
                tmp = webClient.DownloadString(Url + AppVersionTxt);
                var remoteAppVersion = Version.Parse(tmp.Trim());
                if (remoteAppVersion > localAppVersion)
                {
                    var updaterProcess = Process.Start("Updater.exe", remoteAppVersion.ToString());
                    Invoke(new Action(
                        () => { Hide(); }));

                    updaterProcess.WaitForExit();
                    Invoke(new Action(
                        () => { Show();}));
                    File.WriteAllText(AppVersionTxt, remoteAppVersion.ToString());
                    localAppVersion = remoteAppVersion;
                }
                //FIN APPVERSION

                //Creacion de la ruta del ejecutable del programa principal
                tmp = Path.Combine(Directory.GetCurrentDirectory(), "versions", localAppVersion.ToString(),
                    "Flemidra Inventory Manager.exe");

                //Invocacion del programa
                if (File.Exists(tmp))
                {
                    Process.Start(tmp);
                    Invoke(new Action(() => { Close(); }));
                }
                else
                {
                    Invoke(new Action(() =>
                    {
                        label1.Text = "Ocurrio un error inesperado al actualizar la aplicacion.";
                    }));
                }

            }
            catch (Exception e)
            {
                if (e is WebException)
                {

                    var file = new StreamReader(AppVersionTxt);
                    var tmp = file.ReadLine();
                    file.Close();

                    var localAppVersion = Version.Parse(tmp.Trim());
                    tmp = Path.Combine(Directory.GetCurrentDirectory(), "versions", localAppVersion.ToString(),
                        "Flemidra Inventory Manager.exe");
                    if (!File.Exists(tmp))
                    {
                        Invoke(new Action(() =>
                        {
                            label1.Text = "Es necesario una actualizacion de la aplicacion.";
                        }));
                    }
                    else
                    {
                        Process.Start(tmp);
                        Invoke(new Action(() => { Close(); }));
                    }
                }
                else
                {
                    //muestra de error detallado en caso de excepcion
                    Invoke(new Action(() =>
                    {
                        label1.Text = "Ocurrio un error inesperado al actualizar la aplicacion.";
                        label2.Text = e.GetType().Name + " " + e.Message;
                        label2.Visible = true;
                    }));
                }
            }
        }
    }
}
