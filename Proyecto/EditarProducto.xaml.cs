using System;
using System.Windows;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace Proyecto
{
    public partial class EditarProducto
    {
        private readonly string _idProducto;
        private readonly Productos _productos;
        private readonly int _quantity;

        public EditarProducto(string idProducto, string nombre, string quantity, string price, Productos productos)
        {
            InitializeComponent();
            _idProducto = idProducto;
            TxtNombre.Text = nombre.Trim();
            TxtQuantity.Text = quantity.Trim();
            _quantity = Convert.ToInt32(quantity.Trim());
            TxtPrice.Text = price.Trim();
            _productos = productos;
            SqlQueries.RetrieveCategories(TxtCat);
        }
        private void Button_Click(object sender, RoutedEventArgs e) => Close();
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        public void Editar_Cliente(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TxtCat.SelectedIndex == -1)
                {
                    MessageBox.Show("La categoria no puede estar vacia.");
                }
                else
                {
                    if(SqlQueries.EditProduct(TxtCat.Text, TxtNombre.Text.Trim(), Convert.ToInt32(TxtQuantity.Text.Trim()), Convert.ToSingle(TxtPrice.Text.Trim()), _idProducto, _quantity))
                    {
                        _productos.Update_DataGrid();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Algunos campos se encuentran sin llenar");
                    }
                }
            }
            catch (Exception en)
            {
                MessageBox.Show("Error al editar un producto. " + en);
            }
        }
    }
}
