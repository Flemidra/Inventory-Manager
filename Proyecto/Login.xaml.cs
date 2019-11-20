using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace Proyecto
{

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            NuevoUsuario newUser = new NuevoUsuario();
            newUser.Show();
        }

        private void VentanaPrincipal(object sender, RoutedEventArgs e)
        {
            LogIn();
        }

        private void LogIn()
        {
            if (SqlQueries.SearchUser(TxtUsername.Text.Trim(), TxtPassword.Password.Trim()) != -1)
            {
                var newVentana = new VentanaPrincipal();
                newVentana.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Usuario no encontrado.");
            }
        }
        private void Recuperar_Usuario(object sender, RoutedEventArgs e)
        {
            RecuperarUsuario restoreUser = new RecuperarUsuario();
            restoreUser.Show();
            Close();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtUsername.Focus();
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                LogIn();
            }
        }
    }
}
