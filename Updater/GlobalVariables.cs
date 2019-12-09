using System;
using System.IO.Compression;
using System.Net;

namespace Updater
{
    public static class GlobalVariables
    {
        public static Version TargetVersion;
        public static string TargetCacheDir = "Cache";
        public static string TargetVerDir = "Versions";
        public static string ServerURI = "http://localhost/inventory-manager/versions/";

        public static int ExitCode = 0;


        public static WebClient WebClientDownloader;
        public static ZipArchive Decompressor;

        public static string PrimaryToken = "735dce99-59de-481b-a2b8-0cbcec9efa18";
        public static string SecondaryToken = "8e9d2382-6b9e-47d2-a959-bf4ac736ed15";
    }
}
