using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.IO.Compression;

namespace Updater
{
    public partial class Form1 : Form
    {
        private DirectoryInfo dirCacheInfo;
        private string zipFullNameDestination;
        private string targetVersionDir;
        private DirectoryInfo dirVersionsInfo;
        public Form1()
        {
            InitializeComponent();
            dirCacheInfo = new DirectoryInfo(GlobalVariables.TargetCacheDir);
            zipFullNameDestination = string.Format("{0}\\{1}.zip", dirCacheInfo.FullName, GlobalVariables.TargetVersion.ToString(4));
            dirVersionsInfo = new DirectoryInfo(GlobalVariables.TargetVerDir);
            targetVersionDir = string.Format("{0}\\{1}", dirVersionsInfo.FullName, GlobalVariables.TargetVersion.ToString(4));
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
            
            if (dirCacheInfo.Exists)
            {
                dirCacheInfo.Delete(true);
                dirCacheInfo.Refresh();
            }
            dirCacheInfo.Create();
            
            if (!dirVersionsInfo.Exists)
                dirVersionsInfo.Create();

            GlobalVariables.WebClientDownloader.DownloadFileCompleted += WebClientDownloader_DownloadFileCompleted;
            GlobalVariables.WebClientDownloader.DownloadProgressChanged += WebClientDownloader_DownloadProgressChanged;
            GlobalVariables.WebClientDownloader.DownloadFileAsync(
                new Uri(string.Format("{0}{1}.zip", GlobalVariables.ServerURI, GlobalVariables.TargetVersion.ToString(4))),
                //new Uri("https://cdimage.debian.org/debian-cd/current/amd64/iso-cd/debian-10.2.0-amd64-netinst.iso"),
                    zipFullNameDestination);

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
            if (e.Error == null)
            {
                try
                {
                    Decompress(zipFullNameDestination, targetVersionDir);
                    GlobalVariables.ExitCode = 0;
                }
                catch (Exception exception)
                {
                    GlobalVariables.ExitCode = 1;
                    
                }

                
                Close();
            }
            else
            {
                GlobalVariables.ExitCode = 1;
                MessageBox.Show("Ha ocurrido un error al intentar actualizar el programa.");
                Close();
            }
        }

        private void Decompress(string fileName, string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            FileInfo fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("El archivo descargado no se encuentra.");
            }

            using (FileStream fileStream =
                new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read);
                foreach (ZipArchiveEntry tmpEntry in zipArchive.Entries)
                {
                    FileInfo zipFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, tmpEntry.FullName.Replace('/', '\\')));
                    if (zipFileInfo.Exists)
                    {
                        zipFileInfo.Delete();
                        
                    }

                    if (!zipFileInfo.Directory.Exists)
                    {
                        zipFileInfo.Directory.Create();
                    }

                    if (!string.IsNullOrWhiteSpace(zipFileInfo.Name))
                    {
                        FileStream unzipFileStream = new FileStream(zipFileInfo.FullName, FileMode.Create, FileAccess.Write, FileShare.None);
                        Stream compressedFiles = tmpEntry.Open();
                        compressedFiles.CopyTo(unzipFileStream);
                        unzipFileStream.Flush();
                        unzipFileStream.Close();
                        compressedFiles.Close();
                    }
                }
            }

        }
    }
}


