using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Proyecto
{

    public partial class Productos
    {
        public Productos()
        {
            InitializeComponent();
        }

        public void Update_DataGrid()
        {
            try
            {
                TablaProductos.ItemsSource = SqlQueries.UpdateDataGrid("SELECT [FlemidraDB].[dbo].[Products].[id] As 'ID', [FlemidraDB].[dbo].[Products].[name] As 'Nombre', [FlemidraDB].[dbo].[Products].[quantity] As 'Existencia', CONCAT('$. ', [FlemidraDB].[dbo].[Products].[price]) As 'Precio', CONCAT('$. ', [FlemidraDB].[dbo].[Products].[price] *1.12) As 'PrecioIva',  [FlemidraDB].[dbo].[Categories].[name] As 'Categoria', [FlemidraDB].[dbo].[Categories].[id] As 'IDcat' FROM [FlemidraDB].[dbo].[Products] LEFT JOIN [FlemidraDB].[dbo].[Categories] on [FlemidraDB].[dbo].[Products].[id_cat] = [FlemidraDB].[dbo].[Categories].[id]").DefaultView;
                TablaProductos.AutoGenerateColumns = false;
                TablaProductos.CanUserAddRows = false;
            }
            catch (Exception)
            {
                // ignored
            }
        }
        private void Product_Table_Loaded(object sender, RoutedEventArgs e)
        {
            Update_DataGrid();
        }

        private void Eliminar_Event(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
            var deleteConfirmation = new DeleteConfirmation("DELETE FROM [FlemidraDB].[dbo].[Products] Where id =", dataRowView[0].ToString());
            deleteConfirmation.ShowDialog();
            Update_DataGrid();
        }

        private void Editar_Event(object sender, RoutedEventArgs e)
        {
            var dataRowView = (DataRowView)((Button)e.Source).DataContext;
            var editarProducto = new EditarProducto(dataRowView[0].ToString(), dataRowView[1].ToString(), dataRowView[2].ToString(), dataRowView[3].ToString().Replace("$. ", ""),this);
            editarProducto.Show();

        }

        private void NuevoProducto(object sender, RoutedEventArgs e)
        {
            var nuevoProducto = new NuevoProducto(this);
            nuevoProducto.Show();
        }

        private void Search_Name(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtSearch.Text))
            {
                Update_DataGrid();
            }
            else
            {
                DataView dataView1 = new DataView(SqlQueries.UpdateDataGrid("SELECT [FlemidraDB].[dbo].[Products].[id] As 'ID', [FlemidraDB].[dbo].[Products].[name] As 'Nombre', [FlemidraDB].[dbo].[Products].[quantity] As 'Existencia', CONCAT('Bs ', [FlemidraDB].[dbo].[Products].[price]) As 'Precio', CONCAT('Bs ', [FlemidraDB].[dbo].[Products].[price] *1.12) As 'PrecioIva',  [FlemidraDB].[dbo].[Categories].[name] As 'Categoria', [FlemidraDB].[dbo].[Categories].[id] As 'IDcat' FROM [FlemidraDB].[dbo].[Products] LEFT JOIN [FlemidraDB].[dbo].[Categories] on [FlemidraDB].[dbo].[Products].[id_cat] = [FlemidraDB].[dbo].[Categories].[id]"))
                {
                    RowFilter = $"Nombre LIKE '%{TxtSearch.Text}%'"
                };
                TablaProductos.ItemsSource = dataView1;
            }
        }
    }
}