using System;
using System.Windows;
using System.Windows.Input;

namespace Proyecto
{
    /// <summary>
    /// Interaction logic for DeleteConfirmation.xaml
    /// </summary>
    public partial class DeleteConfirmation : Window
    {
        private string _query;
        private string _id;
        public DeleteConfirmation(string query, string id)
        {
            InitializeComponent();
            _query = query;
            _id = id;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Eliminar_Event(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlQueries.DeleteElement(_query, _id);
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error inesperado al eliminar usuario. " + exception);
                Close();

            }
            
        }
    }
}
