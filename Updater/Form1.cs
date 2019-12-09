using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Updater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Cerrar_Ventana(object sender, EventArgs e)
        {
            Close();
        }

        private void Minimizar(object sender, EventArgs e)
        {
            WindowState =  FormWindowState.Minimized;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            label2.Visible = true;
            GlobalVariables.WebClientDownloader = new WebClient();
            DirectoryInfo dirCacheInfo = new DirectoryInfo(GlobalVariables.TargetCacheDir);
            if (dirCacheInfo.Exists)
            {
                dirCacheInfo.Delete(true);
                dirCacheInfo.Refresh();
            }
            dirCacheInfo.Create();
            
            DirectoryInfo dirVersionsInfo = new DirectoryInfo(GlobalVariables.TargetVerDir);
            if (!dirVersionsInfo.Exists)
                dirVersionsInfo.Create();

            GlobalVariables.WebClientDownloader.DownloadFileCompleted += WebClientDownloader_DownloadFileCompleted;
            GlobalVariables.WebClientDownloader.DownloadProgressChanged += WebClientDownloader_DownloadProgressChanged;
            GlobalVariables.WebClientDownloader.DownloadFileAsync(
                //new Uri(string.Format("{0}{1}.zip", GlobalVariables.ServerURI, GlobalVariables.TargetVersion.ToString(4))),
                new Uri("https://cdimage.debian.org/debian-cd/current/amd64/iso-cd/debian-10.2.0-amd64-netinst.iso"),
                string.Format("{0}\\download.zip", dirCacheInfo.FullName));

        }

        private string FormatBytesTo(long bytes)
        {
            float bytesToFloat = bytes;
            string[] Multiplos = new string[] 
            { 
                " B",
                " KB",
                " MB",
                " GB"
            };
            int i = 0;
            while(bytesToFloat > 1024.0f)
            {
                bytesToFloat = bytesToFloat / 1024.0f;
                i++;
                if (i >= Multiplos.Length-1)
                    break;
            }
            return bytesToFloat.ToString("N2") + Multiplos[i];
        }

        private void WebClientDownloader_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            label2.Text = string.Format(
                "Archivo version: {0} - {1} / {2}",
                GlobalVariables.TargetVersion, 
                FormatBytesTo(e.BytesReceived),
                FormatBytesTo(e.TotalBytesToReceive));
            progressBar1.Value = e.ProgressPercentage;
        }

        private void WebClientDownloader_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                GlobalVariables.ExitCode = 0;
                Close();
            }
            else
            {
                GlobalVariables.ExitCode = 1;
                MessageBox.Show("Ha ocurrido un error al intentar actualizar el programa.");
                Close();
            }
        }
    }
}
