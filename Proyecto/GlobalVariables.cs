using System;
using System.IO;
using System.Windows;

namespace Proyecto
{
    public static class GlobalVariables
    {
        private static string _conString2;
        private static int _actualUser;

        static GlobalVariables()
        {
            ConnectionFile();
        }

        private static void ConnectionFile()
        {
            try
            {
                
                if (!File.Exists("SqlConnectionString.txt"))
                {
                    StreamWriter file = File.AppendText("SqlConnectionString.txt");
                    file.WriteLine("Data Source=Flemidra;Initial Catalog=FlemidraDB;Integrated Security=True");
                    file.Close();
                    MessageBox.Show(
                        "El archivo de configuracion de la base de datos ha sido creado con valores por defecto.");
                }
                else
                {
                    StreamReader file = new StreamReader("SqlConnectionString.txt");
                    _conString2 = file.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static string  GetConString()
        {
            return _conString2;
        }

        public static void SetActualUser(int user)
        {
            _actualUser = user;
        }

        public static int GetActualUser()
        {
            return (_actualUser);
        }
    }
}
