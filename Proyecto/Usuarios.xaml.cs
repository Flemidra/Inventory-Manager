using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Proyecto
{
    /// <summary>
    /// Lógica de interacción para Usuarios.xaml
    /// </summary>
    public partial class Usuarios
    {
        public Usuarios()
        {
            InitializeComponent();
        }
        public void Update_DataGrid()
        {
            try
            {
                TablaUsuarios.ItemsSource = SqlQueries.UpdateDataGrid("SELECT [FlemidraDB].[dbo].[Users].[id], [FlemidraDB].[dbo].[Users].[username], [FlemidraDB].[dbo].[Users].[password], [FlemidraDB].[dbo].[Users].[rec_key], [FlemidraDB].[dbo].[Users].[type] FROM [FlemidraDB].[dbo].[Users]").DefaultView;
                TablaUsuarios.AutoGenerateColumns = false;
                TablaUsuarios.CanUserAddRows = false;
            }
            catch (Exception en)
            {
                MessageBox.Show("Error al realizar operacion. " + en);
            }
        }
        private void Tabla_Usuarios_Loaded(object sender, RoutedEventArgs e)
        {
            Update_DataGrid();
        }

        private  void Eliminar_Event(object sender, RoutedEventArgs e)
        {
            try
            {
                var dataRowView = (DataRowView)((Button)e.Source).DataContext;
                var deleteConfirmation = new DeleteConfirmation("DELETE FROM dbo.Users Where id = ", dataRowView[0].ToString());
                deleteConfirmation.ShowDialog();
                Update_DataGrid();

            }
            catch (Exception exception)
            {
                MessageBox.Show("Error inesperado al eliminar usuario. " + exception);

            }
            
        }

        private void Editar_Event(object sender, RoutedEventArgs e)
        {
            var dataRowView = (DataRowView)((Button)e.Source).DataContext;
            var editarUsuario = new EditarUsuario(dataRowView[0].ToString(), dataRowView[1].ToString(), dataRowView[2].ToString(), dataRowView[3].ToString(), this, dataRowView[4].ToString());
            editarUsuario.Show();

        }

        private void Search_Name(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtSearch.Text))
            {
                Update_DataGrid();
            }
            else
            {
                DataView dataView = new DataView(SqlQueries.UpdateDataGrid("SELECT [FlemidraDB].[dbo].[Users].[id], [FlemidraDB].[dbo].[Users].[username], [FlemidraDB].[dbo].[Users].[password], [FlemidraDB].[dbo].[Users].[rec_key], [FlemidraDB].[dbo].[Users].[type] FROM [FlemidraDB].[dbo].[Users]"));
                dataView.RowFilter = string.Format("username LIKE '%{0}%'", TxtSearch.Text);
                TablaUsuarios.ItemsSource = dataView;
            }
        }
    }

}