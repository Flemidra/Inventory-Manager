using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

namespace Proyecto
{
    /// <summary>
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class Reports
    {
        private DataTable _dataTable;
        public Reports()
        {
            InitializeComponent();
            TxtCat.Items.Add("Sin Seleccion");
            SqlQueries.RetrieveCategories(TxtCat, 1);
        }

        public void Update_DataGrid()
        {
            try
            {
                //TablaProductos.ItemsSource = SqlQueries.UpdateDataGrid("SELECT [FlemidraDB].[dbo].[Records].[type] As 'Evento', [FlemidraDB].[dbo].[Products].[name] As 'Producto', [FlemidraDB].[dbo].[Categories].[name] As 'Categoria', [FlemidraDB].[dbo].[Records].[quantity] As 'Cantidad', CONVERT(date, [FlemidraDB].[dbo].[Records].[time]) As 'Fecha', [FlemidraDB].[dbo].[Users].[username] As 'Usuario' FROM [FlemidraDB].[dbo].[Records] INNER JOIN [FlemidraDB].[dbo].[Products] on [FlemidraDB].[dbo].[Records].[id_Product] = [FlemidraDB].[dbo].[Products].[id] INNER JOIN [FlemidraDB].[dbo].[Users] ON [FlemidraDB].[dbo].[Records].[id_Users] = [FlemidraDB].[dbo].[Users].[id] INNER JOIN [FlemidraDB].[dbo].[Categories] on [FlemidraDB].[dbo].[Products].[id_cat] = [FlemidraDB].[dbo].[Categories].[id]").DefaultView;
                TablaProductos.ItemsSource = SqlQueries.UpdateDataGrid("SELECT [FlemidraDB].[dbo].[Records].[type] As 'Evento', [FlemidraDB].[dbo].[Products].[name] As 'Producto', [FlemidraDB].[dbo].[Categories].[name] As 'Categoria', [FlemidraDB].[dbo].[Records].[quantity] As 'Cantidad', CONVERT(date, [FlemidraDB].[dbo].[Records].[time]) As 'Fecha', [FlemidraDB].[dbo].[Users].[username] As 'Usuario' FROM [FlemidraDB].[dbo].[Records] INNER JOIN [FlemidraDB].[dbo].[Products] on [FlemidraDB].[dbo].[Records].[id_Product] = [FlemidraDB].[dbo].[Products].[id] INNER JOIN [FlemidraDB].[dbo].[Users] ON [FlemidraDB].[dbo].[Records].[id_Users] = [FlemidraDB].[dbo].[Users].[id] INNER JOIN [FlemidraDB].[dbo].[Categories] on [FlemidraDB].[dbo].[Products].[id_cat] = [FlemidraDB].[dbo].[Categories].[id]").DefaultView;
                TablaProductos.AutoGenerateColumns = false;
                TablaProductos.CanUserAddRows = false;
            }
            catch (Exception en)
            {
                MessageBox.Show("Error al realizar operacion de actualizacion de DataGrid. " + en);
            }
        }
        private void Product_Table_Loaded(object sender, RoutedEventArgs e)
        {
            Update_DataGrid();
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchInfo();
        }

        private void FinishDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchInfo();
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            Excel.Application excel;
            Excel.Workbook wb;

            Excel.Worksheet ws;

            try
            {
                excel = new Excel.Application();
                wb = excel.Workbooks.Add();
                ws = (Excel.Worksheet)wb.ActiveSheet;

                DataView view = (DataView)TablaProductos.ItemsSource;
                DataTable dt = DataViewAsDataTable(view);

                for (int idx = 0; idx < dt.Columns.Count; idx++)
                {
                    ws.Range["A1"].Offset[0, idx].Value = dt.Columns[idx].ColumnName;
                }

                for (int idx = 0; idx < dt.Rows.Count; idx++)
                {  

                    ws.Range["A2"].Offset[idx].Resize[1, dt.Columns.Count].Value = dt.Rows[idx].ItemArray;
                }


                excel.Visible = true;
                wb.Activate();
            }
            catch (COMException ex)
            {
                MessageBox.Show("Error accessing Excel: " + ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }
        public static DataTable DataViewAsDataTable(DataView dv)
        {
            DataTable dt = dv.Table.Clone();
            foreach (DataRowView drv in dv)
                dt.ImportRow(drv.Row);
            return dt;
        }
        private void TxtCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchInfo();
        }

        private void SearchInfo()
        {
            try
            {
                if (StartDate.SelectedDate == null && FinishDate.SelectedDate == null && (TxtCat.SelectedIndex == -1 || TxtCat.SelectedItem.ToString() == "Sin Seleccion"))
                {
                    Update_DataGrid();
                    return;
                }

                if (StartDate.SelectedDate != null && FinishDate.SelectedDate == null && (TxtCat.SelectedIndex == -1 || TxtCat.SelectedItem.ToString() == "Sin Seleccion"))
                { 
                    TablaProductos.ItemsSource = SqlQueries.FilterReport("SELECT [FlemidraDB].[dbo].[Records].[type] As 'Evento', [FlemidraDB].[dbo].[Products].[name] As 'Producto', [FlemidraDB].[dbo].[Categories].[name] As 'Categoria', [FlemidraDB].[dbo].[Records].[quantity] As 'Cantidad', CONVERT(date, [FlemidraDB].[dbo].[Records].[time]) As 'Fecha', [FlemidraDB].[dbo].[Users].[username] As 'Usuario' FROM [FlemidraDB].[dbo].[Records] INNER JOIN [FlemidraDB].[dbo].[Products] on [FlemidraDB].[dbo].[Records].[id_Product] = [FlemidraDB].[dbo].[Products].[id] INNER JOIN [FlemidraDB].[dbo].[Users] ON [FlemidraDB].[dbo].[Records].[id_Users] = [FlemidraDB].[dbo].[Users].[id] INNER JOIN [FlemidraDB].[dbo].[Categories] on [FlemidraDB].[dbo].[Products].[id_cat] = [FlemidraDB].[dbo].[Categories].[id] WHERE [FlemidraDB].[dbo].[Records].[time] >= @Parameter", StartDate.SelectedDate.Value.Date.ToString("yyyy-MM-dd")).DefaultView;
                    /*TablaProductos.AutoGenerateColumns = false;
                    TablaProductos.CanUserAddRows = false;*/
                    return;
                }

                if (StartDate.SelectedDate == null && FinishDate.SelectedDate != null && (TxtCat.SelectedIndex == -1 || TxtCat.SelectedItem.ToString() == "Sin Seleccion"))
                {

                    TablaProductos.ItemsSource = SqlQueries.FilterReport("SELECT [FlemidraDB].[dbo].[Records].[type] As 'Evento', [FlemidraDB].[dbo].[Products].[name] As 'Producto', [FlemidraDB].[dbo].[Categories].[name] As 'Categoria', [FlemidraDB].[dbo].[Records].[quantity] As 'Cantidad', CONVERT(date, [FlemidraDB].[dbo].[Records].[time]) As 'Fecha', [FlemidraDB].[dbo].[Users].[username] As 'Usuario' FROM [FlemidraDB].[dbo].[Records] INNER JOIN [FlemidraDB].[dbo].[Products] on [FlemidraDB].[dbo].[Records].[id_Product] = [FlemidraDB].[dbo].[Products].[id] INNER JOIN [FlemidraDB].[dbo].[Users] ON [FlemidraDB].[dbo].[Records].[id_Users] = [FlemidraDB].[dbo].[Users].[id] INNER JOIN [FlemidraDB].[dbo].[Categories] on [FlemidraDB].[dbo].[Products].[id_cat] = [FlemidraDB].[dbo].[Categories].[id] WHERE [FlemidraDB].[dbo].[Records].[time] <= @Parameter", FinishDate.SelectedDate.Value.Date.ToString("yyyy-MM-dd")).DefaultView;
                    /*TablaProductos.AutoGenerateColumns = false;
                    TablaProductos.CanUserAddRows = false;*/
                    return;
                }

                if (StartDate.SelectedDate == null && FinishDate.SelectedDate == null && (TxtCat.SelectedIndex != -1 || TxtCat.SelectedItem.ToString() != "Sin Seleccion"))
                {
                    TablaProductos.ItemsSource = SqlQueries.FilterReport("SELECT [FlemidraDB].[dbo].[Records].[type] As 'Evento', [FlemidraDB].[dbo].[Products].[name] As 'Producto', [FlemidraDB].[dbo].[Categories].[name] As 'Categoria', [FlemidraDB].[dbo].[Records].[quantity] As 'Cantidad', CONVERT(date, [FlemidraDB].[dbo].[Records].[time]) As 'Fecha', [FlemidraDB].[dbo].[Users].[username] As 'Usuario' FROM [FlemidraDB].[dbo].[Records] INNER JOIN [FlemidraDB].[dbo].[Products] on [FlemidraDB].[dbo].[Records].[id_Product] = [FlemidraDB].[dbo].[Products].[id] INNER JOIN [FlemidraDB].[dbo].[Users] ON [FlemidraDB].[dbo].[Records].[id_Users] = [FlemidraDB].[dbo].[Users].[id] INNER JOIN [FlemidraDB].[dbo].[Categories] on [FlemidraDB].[dbo].[Products].[id_cat] = [FlemidraDB].[dbo].[Categories].[id] WHERE [FlemidraDB].[dbo].[Categories].[name] = @Parameter", TxtCat.SelectedValue.ToString().Trim()).DefaultView;
                    /*TablaProductos.AutoGenerateColumns = false;
                    TablaProductos.CanUserAddRows = false;*/
                    return;
                }

                if (StartDate.SelectedDate != null && FinishDate.SelectedDate != null && (TxtCat.SelectedIndex == -1 || TxtCat.SelectedItem.ToString() == "Sin Seleccion"))
                {
                    TablaProductos.ItemsSource = SqlQueries.FilterReport("SELECT [FlemidraDB].[dbo].[Records].[type] As 'Evento', [FlemidraDB].[dbo].[Products].[name] As 'Producto', [FlemidraDB].[dbo].[Categories].[name] As 'Categoria', [FlemidraDB].[dbo].[Records].[quantity] As 'Cantidad', CONVERT(date, [FlemidraDB].[dbo].[Records].[time]) As 'Fecha', [FlemidraDB].[dbo].[Users].[username] As 'Usuario' FROM [FlemidraDB].[dbo].[Records] INNER JOIN [FlemidraDB].[dbo].[Products] on [FlemidraDB].[dbo].[Records].[id_Product] = [FlemidraDB].[dbo].[Products].[id] INNER JOIN [FlemidraDB].[dbo].[Users] ON [FlemidraDB].[dbo].[Records].[id_Users] = [FlemidraDB].[dbo].[Users].[id] INNER JOIN [FlemidraDB].[dbo].[Categories] on [FlemidraDB].[dbo].[Products].[id_cat] = [FlemidraDB].[dbo].[Categories].[id] WHERE [FlemidraDB].[dbo].[Records].[time] >= @ParameterA AND [FlemidraDB].[dbo].[Records].[time] <= @ParameterB", StartDate.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), FinishDate.SelectedDate.Value.Date.ToString("yyyy-MM-dd")).DefaultView;
                    /*TablaProductos.AutoGenerateColumns = false;
                    TablaProductos.CanUserAddRows = false;*/
                    return;
                }

                if (StartDate.SelectedDate != null && FinishDate.SelectedDate == null && (TxtCat.SelectedIndex != -1 || TxtCat.SelectedItem.ToString() != "Sin Seleccion"))
                {
                    TablaProductos.ItemsSource = SqlQueries.FilterReport("SELECT [FlemidraDB].[dbo].[Records].[type] As 'Evento', [FlemidraDB].[dbo].[Products].[name] As 'Producto', [FlemidraDB].[dbo].[Categories].[name] As 'Categoria', [FlemidraDB].[dbo].[Records].[quantity] As 'Cantidad', CONVERT(date, [FlemidraDB].[dbo].[Records].[time]) As 'Fecha', [FlemidraDB].[dbo].[Users].[username] As 'Usuario' FROM [FlemidraDB].[dbo].[Records] INNER JOIN [FlemidraDB].[dbo].[Products] on [FlemidraDB].[dbo].[Records].[id_Product] = [FlemidraDB].[dbo].[Products].[id] INNER JOIN [FlemidraDB].[dbo].[Users] ON [FlemidraDB].[dbo].[Records].[id_Users] = [FlemidraDB].[dbo].[Users].[id] INNER JOIN [FlemidraDB].[dbo].[Categories] on [FlemidraDB].[dbo].[Products].[id_cat] = [FlemidraDB].[dbo].[Categories].[id] WHERE [FlemidraDB].[dbo].[Records].[time] >= @ParameterA AND [FlemidraDB].[dbo].[Categories].[name] = @ParameterB", StartDate.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), TxtCat.SelectedValue.ToString().Trim()).DefaultView;
                    /*TablaProductos.AutoGenerateColumns = false;
                    TablaProductos.CanUserAddRows = false;*/
                    return;
                }

                if (StartDate.SelectedDate == null && FinishDate.SelectedDate != null && (TxtCat.SelectedIndex != -1 || TxtCat.SelectedItem.ToString() != "Sin Seleccion"))
                {
                    TablaProductos.ItemsSource = SqlQueries.FilterReport("SELECT [FlemidraDB].[dbo].[Records].[type] As 'Evento', [FlemidraDB].[dbo].[Products].[name] As 'Producto', [FlemidraDB].[dbo].[Categories].[name] As 'Categoria', [FlemidraDB].[dbo].[Records].[quantity] As 'Cantidad', CONVERT(date, [FlemidraDB].[dbo].[Records].[time]) As 'Fecha', [FlemidraDB].[dbo].[Users].[username] As 'Usuario' FROM [FlemidraDB].[dbo].[Records] INNER JOIN [FlemidraDB].[dbo].[Products] on [FlemidraDB].[dbo].[Records].[id_Product] = [FlemidraDB].[dbo].[Products].[id] INNER JOIN [FlemidraDB].[dbo].[Users] ON [FlemidraDB].[dbo].[Records].[id_Users] = [FlemidraDB].[dbo].[Users].[id] INNER JOIN [FlemidraDB].[dbo].[Categories] on [FlemidraDB].[dbo].[Products].[id_cat] = [FlemidraDB].[dbo].[Categories].[id] WHERE [FlemidraDB].[dbo].[Records].[time] <= @ParameterA AND [FlemidraDB].[dbo].[Categories].[name] = @ParameterB", FinishDate.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), TxtCat.SelectedValue.ToString().Trim()).DefaultView;
                    /*TablaProductos.AutoGenerateColumns = false;
                    TablaProductos.CanUserAddRows = false;*/
                    return;
                }

                if (StartDate.SelectedDate != null && FinishDate.SelectedDate != null && (TxtCat.SelectedIndex != -1 || TxtCat.SelectedItem.ToString() != "Sin Seleccion"))
                {
                    TablaProductos.ItemsSource = SqlQueries.FilterReport("SELECT [FlemidraDB].[dbo].[Records].[type] As 'Evento', [FlemidraDB].[dbo].[Products].[name] As 'Producto', [FlemidraDB].[dbo].[Categories].[name] As 'Categoria', [FlemidraDB].[dbo].[Records].[quantity] As 'Cantidad', CONVERT(date, [FlemidraDB].[dbo].[Records].[time]) As 'Fecha', [FlemidraDB].[dbo].[Users].[username] As 'Usuario' FROM [FlemidraDB].[dbo].[Records] INNER JOIN [FlemidraDB].[dbo].[Products] on [FlemidraDB].[dbo].[Records].[id_Product] = [FlemidraDB].[dbo].[Products].[id] INNER JOIN [FlemidraDB].[dbo].[Users] ON [FlemidraDB].[dbo].[Records].[id_Users] = [FlemidraDB].[dbo].[Users].[id] INNER JOIN [FlemidraDB].[dbo].[Categories] on [FlemidraDB].[dbo].[Products].[id_cat] = [FlemidraDB].[dbo].[Categories].[id] WHERE [FlemidraDB].[dbo].[Records].[time] >= @ParameterA AND [FlemidraDB].[dbo].[Records].[time] <= @ParameterB AND [FlemidraDB].[dbo].[Categories].[name] = @ParameterC", StartDate.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), FinishDate.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), TxtCat.SelectedValue.ToString().Trim()).DefaultView;

                    const string select = "SELECT [FlemidraDB].[dbo].[Records].[type] As 'Evento', [FlemidraDB].[dbo].[Products].[name] As 'Producto', [FlemidraDB].[dbo].[Categories].[name] As 'Categoria', [FlemidraDB].[dbo].[Records].[quantity] As 'Cantidad', CONVERT(date, [FlemidraDB].[dbo].[Records].[time]) As 'Fecha', [FlemidraDB].[dbo].[Users].[username] As 'Usuario' FROM [FlemidraDB].[dbo].[Records] INNER JOIN [FlemidraDB].[dbo].[Products] on [FlemidraDB].[dbo].[Records].[id_Product] = [FlemidraDB].[dbo].[Products].[id] INNER JOIN [FlemidraDB].[dbo].[Users] ON [FlemidraDB].[dbo].[Records].[id_Users] = [FlemidraDB].[dbo].[Users].[id] INNER JOIN [FlemidraDB].[dbo].[Categories] on [FlemidraDB].[dbo].[Products].[id_cat] = [FlemidraDB].[dbo].[Categories].[id] WHERE [FlemidraDB].[dbo].[Records].[time] >= @StartDate AND [FlemidraDB].[dbo].[Records].[time] <= @FinishDate AND [FlemidraDB].[dbo].[Categories].[name] = @catName";
                    var con = new SqlConnection(GlobalVariables.GetConString());
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Parameters.AddWithValue("@StartDate",
                        StartDate.SelectedDate.Value.Date.ToString("yyyy-MM-dd"));
                    sqlCommand.Parameters.AddWithValue("@FinishDate",
                        FinishDate.SelectedDate.Value.Date.ToString("yyyy-MM-dd"));
                    sqlCommand.Parameters.AddWithValue("@catName",
                        TxtCat.SelectedValue.ToString().Trim());
                    sqlCommand.CommandText = select;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = con;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    _dataTable = new DataTable();
                    sqlDataAdapter.Fill(_dataTable);
                    TablaProductos.ItemsSource = _dataTable.DefaultView;
                    TablaProductos.AutoGenerateColumns = false;
                    TablaProductos.CanUserAddRows = false;
                    con.Close();
                }

            }
            catch (Exception en)
            {
                MessageBox.Show("Error al realizar operacion de filtrado. " + en);
            }
        }
    }
}