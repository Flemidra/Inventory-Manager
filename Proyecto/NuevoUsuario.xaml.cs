using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace Proyecto
{
    /// <summary>
    /// Lógica de interacción para NuevoUsuario.xaml
    /// </summary>
    public partial class NuevoUsuario
    {
        public NuevoUsuario()
        {
            InitializeComponent();
            TxType.Items.Add("Empleado");
            TxType.Items.Add("Contable");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Agregar_Usuario(object sender, RoutedEventArgs e)
        {
            NewUser();
        }

        private void NewUser()
        {
            try
            {
                if (SqlQueries.NewUser(TxtUsername.Text.Trim(), TxtPassword.Password.Trim(), TxtRecoveryKey.Password.Trim(),
                    TxType.SelectedValue.ToString().Trim()))
                {
                    Close();
                }
                else
                {
                    MessageBox.Show("El usuario no pudo ser agregado.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error Inesperado: Uno o mas campos se encuentran sin llenar " + e);
            }
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                NewUser();
            }
        }
    }
}
