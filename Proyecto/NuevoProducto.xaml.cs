using System;
using System.Windows;
using System.Windows.Input;

namespace Proyecto
{
    public partial class NuevoProducto
    {

        private readonly Productos _productos;

        public NuevoProducto(Productos productos)
        {
            InitializeComponent();
            this._productos = productos;
            try
            {
                SqlQueries.RetrieveCategories(TxtCat);
            }
            catch (Exception)
            {
                MessageBox.Show("Error inesperado al llenar ComboBox de Categorias.");

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) => Close();
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void AgregarProducto(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SqlQueries.NewProduct(TxtNombre.Text.Trim(), Convert.ToInt32(TxtQuantity.Text.Trim()),
                    Convert.ToSingle(TxtPrice.Text.Trim()), TxtCat.Text))
                {
                    _productos.Update_DataGrid();
                    Close();
                }
            }
            catch (Exception en)
            {
                MessageBox.Show("Error inesperado al agregar un producto. " + en);
            }
        }

    }
}