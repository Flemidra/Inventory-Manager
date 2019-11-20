using System;
using System.Windows;
using System.Windows.Input;

namespace Proyecto
{
    /// <summary>
    /// Lógica de interacción para RecuperarUsuario.xaml
    /// </summary>
    public partial class RecuperarUsuario
    {
        public RecuperarUsuario()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var programStart = new MainWindow();
            programStart.Show();
        }

        private void Get_Password(object sender, RoutedEventArgs e)
        {
            
            ValidUser();
        }

        private void ValidUser()
        {
            try
            {
                if (SqlQueries.RecoverUser(TxtUsername.Text.Trim(), TxtRecoveryKey.Password.Trim()))
                {
                    var programStart = new MainWindow();
                    programStart.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("El usuario o clave secreta es invalida.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado al validar usuario. " + e);
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtUsername.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ValidUser();
            }
        }
    }
}
