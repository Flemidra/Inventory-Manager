using System;
using System.Windows;
using System.Windows.Input;

namespace Proyecto
{
    /// <summary>
    /// Interaction logic for EditCategory.xaml
    /// </summary>
    public partial class EditCategory
    {
        private readonly Categories categories;

        private readonly string id;

        public EditCategory(string id, string nombre, Categories categories)
        {
            InitializeComponent();
            TxtNombre.Text = nombre;
            this.categories = categories;
            this.id = id;
        }
        private void Button_Click(object sender, RoutedEventArgs e) => Close();
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public void Editar_Categoria(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtNombre.Text))
                {
                    MessageBox.Show("El nombre de la categoria no puede estar en blanco.");
                }
                else
                {
                    SqlQueries.EditCategory(TxtNombre.Text.Trim(), Convert.ToInt32(id.Trim()));
                    categories.Update_DataGrid();
                    Close();
                }
                
            }
            catch (Exception en)
            {
                MessageBox.Show("Error al modificar categoria. " + en);
            }
        }

    }
}
