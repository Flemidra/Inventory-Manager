using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace Proyecto
{
    /// <summary>
    /// Interaction logic for NewCategory.xaml
    /// </summary>
    public partial class NewCategory
    {
        private readonly Categories categories;


        public NewCategory(Categories categories)
        {
            InitializeComponent();
            this.categories = categories;

        }
        private void Button_Click(object sender, RoutedEventArgs e) => Close();
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Agregar_Categoria(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtNombre.Text))
                {
                    MessageBox.Show("El nombre de la categoria no puede estar en blanco.");
                }
                else
                {
                    SqlQueries.NewCategory(TxtNombre.Text.Trim());
                    categories.Update_DataGrid();
                    Close();
                }
            }
            catch (Exception en)
            {
                MessageBox.Show("El programa se encuentra indispuesto, disculpelo. (Error: " + en + " )");
            }
        }
    }
}
