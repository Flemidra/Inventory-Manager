using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Proyecto
{
    /// <summary>
    /// Interaction logic for Categories.xaml
    /// </summary>
    public partial class Categories
    {

        public Categories()
        {
            InitializeComponent();
        }

         public void Update_DataGrid()
        {
            try
            {
                TablaCategorias.ItemsSource = SqlQueries.UpdateDataGrid("SELECT [FlemidraDB].[dbo].[Categories].[id] As 'ID', [FlemidraDB].[dbo].[Categories].[name] As 'Nombre' FROM [FlemidraDB].[dbo].[Categories]").DefaultView;
                TablaCategorias.AutoGenerateColumns = false;
                TablaCategorias.CanUserAddRows = false;
            }
            catch (Exception en)
            {
                MessageBox.Show("Error al realizar operacion de actualizacion de Datagrid. " + en);
            }
        }
        private void Product_Table_Loaded(object sender, RoutedEventArgs e)
        {
            Update_DataGrid();
        }

        private void Eliminar_Event(object sender, RoutedEventArgs e)
        {
            try
            {
                var dataRowView = (DataRowView)((Button)e.Source).DataContext;

                var deleteConfirmation = new DeleteConfirmation("DELETE FROM [FlemidraDB].[dbo].[Categories] Where id =", dataRowView[0].ToString());
                deleteConfirmation.ShowDialog();
                Update_DataGrid();
            }
            catch (Exception en)
            {
                MessageBox.Show("Error al realizar operacion de borrado de categoria. " + en);
            }
        }

        private void Editar_Event(object sender, RoutedEventArgs e)
        {
            var dataRowView = (DataRowView)((Button)e.Source).DataContext;
            var editCategory = new EditCategory(dataRowView[0].ToString(), dataRowView[1].ToString(),this);
            editCategory.Show();

        }

        private void NuevoProducto(object sender, RoutedEventArgs e)
        {
            var newCategory = new NewCategory(this);
            newCategory.Show();
        }

        private void Search_Name(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrEmpty(TxtSearch.Text))
            {
                Update_DataGrid();
            }
            else
            {
                DataView dataView1 = new DataView(SqlQueries.UpdateDataGrid("SELECT [FlemidraDB].[dbo].[Categories].[id] As 'ID', [FlemidraDB].[dbo].[Categories].[name] As 'Nombre' FROM [FlemidraDB].[dbo].[Categories]"))
                {
                    RowFilter = $"Nombre LIKE '%{TxtSearch.Text}%'"
                };
                TablaCategorias.ItemsSource = dataView1;
            }
        }

    }
}
