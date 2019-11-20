using System;
using System.Windows;
using System.Windows.Input;

namespace Proyecto
{
    /// <summary>
    /// Lógica de interacción para EditarUsuario.xaml
    /// </summary>
    public partial class EditarUsuario
    {
        private readonly string idUsuario;
        private readonly Usuarios usuarios;
        public EditarUsuario(string idUsuario, string username, string password, string recoveryKey, Usuarios usuarios, string type)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            TxtUsername.Text = username;
            TxtPassword.Text = password;
            TxtRecoveryKey.Text = recoveryKey;
            this.usuarios = usuarios;
            TxType.Items.Add("Administrador");
            TxType.Items.Add("Empleado");
            TxType.Items.Add("Contable");
        }
        private void Button_Click(object sender, RoutedEventArgs e) => Close();
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Editar_Usuario(object sender, RoutedEventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(TxtUsername.Text) || string.IsNullOrEmpty(TxType.Text) || string.IsNullOrEmpty(TxtPassword.Text) || string.IsNullOrEmpty(TxtRecoveryKey.Text))
                {
                    MessageBox.Show("Algunos campos se encuentran sin llenar");
                }
                else
                {
                    if (SqlQueries.EditUser(TxtUsername.Text.Trim(), TxtPassword.Text.Trim(), TxtRecoveryKey.Text.Trim(),
                        TxType.SelectedValue.ToString().Trim(), Convert.ToInt32(idUsuario)))
                    {
                        usuarios.Update_DataGrid();
                        MessageBox.Show("Actualizado exitosamente");

                    }
                    else
                    {
                        MessageBox.Show("El usuario no pudo ser modificado");
                    }
                    Close();
                }
                
            }
            catch (Exception en)
            {
                MessageBox.Show("Error inesperado al editar usuario. " + en);
            }
        }
    }
}
