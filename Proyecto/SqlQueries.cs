using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto
{
    internal static class SqlQueries
    {
        public static int SearchUser(string txtUsername, string txtPassword)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtUsername) && !string.IsNullOrEmpty(txtPassword))
                {

                    var select = "SELECT [FlemidraDB].[dbo].[Users].id FROM [FlemidraDB].[dbo].[Users] WHERE [FlemidraDB].[dbo].[Users].username =@user AND [FlemidraDB].[dbo].[Users].password =@pass";
                    var con = new SqlConnection(GlobalVariables.GetConString());
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Parameters.AddWithValue("@user", txtUsername);
                    sqlCommand.Parameters.AddWithValue("@pass", txtPassword);
                    sqlCommand.CommandText = select;
                    sqlCommand.Connection = con;
                    sqlCommand.CommandType = CommandType.Text;
                    var resultQueryId = sqlCommand.ExecuteScalar();
                    if (resultQueryId != null)
                    {
                        con.Close();
                        GlobalVariables.SetActualUser(Convert.ToInt32(resultQueryId));
                        return Convert.ToInt32(resultQueryId);
                    }

                    con.Close();
                    return -1;

                }

                return -1;
            }
            catch (Exception en)
            {
                MessageBox.Show("Error 01: al buscar usuario en la base de datos " + en);
            }
            return -1;
        }

        public static bool RecoverUser(string txtUsername, string txtRecoveryKey)
        {
            try
            {
                var select = "SELECT [FlemidraDB].[dbo].[Users].password FROM [FlemidraDB].[dbo].[Users] WHERE [FlemidraDB].[dbo].[Users].username =@user AND [FlemidraDB].[dbo].[Users].rec_key =@pass";
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Parameters.AddWithValue("@user", txtUsername);
                sqlCommand.Parameters.AddWithValue("@pass", txtRecoveryKey);
                sqlCommand.CommandText = select;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = con;
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    MessageBox.Show(sqlDataReader.GetString(0));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error 02: al buscar usuario en la base de datos " + e);
                return false;
            }
        }

        public static bool NewUser(string txtUsername, string txtPassword, string txtRecoveryKey, string txType)
        {
            try
            {
                var InsertUsuario = "INSERT INTO [FlemidraDB].[dbo].[Users] ([FlemidraDB].[dbo].[Users].[username], [FlemidraDB].[dbo].[Users].[password], [FlemidraDB].[dbo].[Users].[rec_key], [FlemidraDB].[dbo].[Users].[type]) VALUES (@user, @pass, @key, @type)";
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                var sqlCommand = new SqlCommand();
                sqlCommand.Parameters.AddWithValue("@user", txtUsername);
                sqlCommand.Parameters.AddWithValue("@pass", txtPassword);
                sqlCommand.Parameters.AddWithValue("@key", txtRecoveryKey);
                sqlCommand.Parameters.AddWithValue("@type", txType);
                sqlCommand.CommandText = InsertUsuario;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = con;
                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Registro exitoso.");
                con.Close();
                return true;
            }
            catch (Exception en)
            {
                MessageBox.Show("Error 03: al agregar un usuario en la base de datos " + en);
                return false;
            }
        }

        public static bool EditUser(string txtUsername, string txtPassword, string txtRecoveryKey, string txType, int id)
        {
            try
            {
                var updateUsuario = "UPDATE [FlemidraDB].[dbo].[Users] SET username =@user, password=@pass, rec_key=@key, type=@type where id = @id";
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Parameters.AddWithValue("@user", txtUsername);
                sqlCommand.Parameters.AddWithValue("@pass", txtPassword);
                sqlCommand.Parameters.AddWithValue("@key", txtRecoveryKey);
                sqlCommand.Parameters.AddWithValue("@type", txType);
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlCommand.CommandText = updateUsuario;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = con;
                sqlCommand.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception en)
            {
                MessageBox.Show("Error 04: al editar un usuario en la base de datos " + en);
                return false;
            }
        }

        public static void DeleteElement(string query, string id)
        {
            try
            {
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                var sqlCommand = new SqlCommand(query + id, con);
                sqlCommand.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Elemento borrado con exito");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error 05: al borrar un elemento de la base de datos " + e);

            }
            
        }

        public static DataTable UpdateDataGrid(string query)
        {
            try
            {
                var select = query;
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(select, con);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                var dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                con.Close();
                return dataTable;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error 06: al actualizar DataGrid " + e);

                return null;
            }
        }

        public static bool NewProduct(string txtNombre, int txtQuantity, float txtPrice, string txtCat)
        {
            try
            {
                var insertPersona = "INSERT INTO [FlemidraDB].[dbo].[Products] ([FlemidraDB].[dbo].[Products].[name], [FlemidraDB].[dbo].[Products].[quantity], [FlemidraDB].[dbo].[Products].[price], [FlemidraDB].[dbo].[Products].[id_cat]) VALUES (@name, @quantity, @price, @id_cat ); SELECT SCOPE_IDENTITY()";
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                SqlCommand sqlCommand = new SqlCommand();
                var auxString = txtCat;
                string getId = "";
                for (var i = 0; i < auxString.Length; i++)
                {

                    getId = getId + auxString[i];
                    if (auxString[i + 1] == '-')
                    {
                        break;
                    }
                }

                sqlCommand.Parameters.AddWithValue("@name", txtNombre);
                sqlCommand.Parameters.AddWithValue("@quantity", txtQuantity);
                sqlCommand.Parameters.AddWithValue("@price", txtPrice);
                sqlCommand.Parameters.AddWithValue("@id_cat", Convert.ToInt32(getId));
                sqlCommand.CommandText = insertPersona;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = con;
                var id = sqlCommand.ExecuteScalar();
                MessageBox.Show("Registro exitoso.");
                con.Close();
                NewRecord(Convert.ToInt32(id), txtQuantity, "Entrada");
                return true;

            }
            catch (Exception e)
            {
                MessageBox.Show("Error 07: al agregar un producto a la base de datos" + e);
                return false;
            }
        }

        public static void NewRecord(int id, int txtQuantity, string type)
        {
            try
            {
                var insertRecord = "INSERT INTO [FlemidraDB].[dbo].[Records] ([FlemidraDB].[dbo].[Records].[type], [FlemidraDB].[dbo].[Records].[time], [FlemidraDB].[dbo].[Records].[id_Product], [FlemidraDB].[dbo].[Records].[quantity], [FlemidraDB].[dbo].[Records].[id_Users]) VALUES (@type, @time, @id_Prod, @quantity, @id_User )";
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Parameters.AddWithValue("@type", type);
                sqlCommand.Parameters.AddWithValue("@time",  DateTime.Now.ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@id_Prod", id);
                sqlCommand.Parameters.AddWithValue("@quantity", txtQuantity);
                sqlCommand.Parameters.AddWithValue("@id_User", GlobalVariables.GetActualUser());
                sqlCommand.CommandText = insertRecord;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = con;
                sqlCommand.ExecuteNonQuery();
                //MessageBox.Show("Record exitoso.");
                con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error 08: al agregar una entrada al registro de la base de datos. " + e);
            }
        }

        public static void RetrieveCategories(ComboBox txtCat)
        {
            try
            {
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                var select = "SELECT [FlemidraDB].[dbo].[Categories].[id], [FlemidraDB].[dbo].[Categories].[name] FROM [FlemidraDB].[dbo].[Categories]";
                var sqlCommand = new SqlCommand();
                sqlCommand.CommandText = select;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = con;
                var sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        txtCat.Items.Add(sqlDataReader.GetInt32(0) + "- Categoria: " + sqlDataReader.GetString(1).Trim());
                    }
                }
                else
                {
                    MessageBox.Show("No hay categorias para agregar.");
                }
                sqlDataReader.Close();
                con.Close();
                
            }
            catch (Exception e)
            {
                MessageBox.Show("Error 09: al llenar comboBox de categorias. " + e);

            }
        }

        public static void RetrieveCategories(ComboBox txtCat, int b)
        {
            try
            {
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                var select = "SELECT [FlemidraDB].[dbo].[Categories].[id], [FlemidraDB].[dbo].[Categories].[name] FROM [FlemidraDB].[dbo].[Categories]";
                var sqlCommand = new SqlCommand();
                sqlCommand.CommandText = select;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = con;
                var sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        txtCat.Items.Add(sqlDataReader.GetString(1).Trim());
                    }
                }
                else
                {
                    MessageBox.Show("No hay categorias para agregar.");
                }
                sqlDataReader.Close();
                con.Close();
                
            }
            catch (Exception e)
            {
                MessageBox.Show("Error 09: al llenar comboBox de categorias. " + e);

            }
        }

        public static string UserLevel()
        {
            try
            {
                var select = "SELECT [FlemidraDB].[dbo].[Users].type FROM [FlemidraDB].[dbo].[Users] WHERE [FlemidraDB].[dbo].[Users].[id] =@id";
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Parameters.AddWithValue("@id", GlobalVariables.GetActualUser());
                sqlCommand.CommandText = select;
                sqlCommand.Connection = con;
                sqlCommand.CommandType = CommandType.Text;
                var resultQueryId = sqlCommand.ExecuteScalar();
                con.Close();
                return resultQueryId.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error 10: al identificador tipo de usuario. " + e);
                return null;
            }
            
        }

        public static void NewCategory(string txtNombre)
        {
            try
            {
                var insertCategory = "INSERT INTO [FlemidraDB].[dbo].[Categories] ([FlemidraDB].[dbo].[Categories].[name]) VALUES (@name);";
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Parameters.AddWithValue("@name", txtNombre);
                sqlCommand.CommandText = insertCategory;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = con;
                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Registro exitoso.");
                con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error 11: al agregar nueva categoria. " + e);
            }
            
        }

        public static void EditCategory(string txtNombre, int id)
        {
            try
            {
                    var updateCategory = "UPDATE [FlemidraDB].[dbo].[Categories] SET name = @name where id = @id";
                    var con = new SqlConnection(GlobalVariables.GetConString());
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Parameters.AddWithValue("@name", txtNombre);
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.CommandText = updateCategory;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = con;
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Actualizado exitosamente");
                    con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error 12: al editar categoria. " + e);
            }
        }

        public static bool EditProduct(string txtCat, string txtNombre, int txtQuantity, float txtPrice, string id, int quantity)
        {
            try
            {
                var updatePersona = "UPDATE [FlemidraDB].[dbo].[Products] SET name = @name, quantity = @quantity, price = @price, id_cat = @cat where id = @id";
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                var sqlCommand = new SqlCommand();
                var auxString = txtCat;
                var getId = "";
                for (var i = 0; i < auxString.Length; i++)
                {

                    getId = getId + auxString[i];
                    if (auxString[i + 1] == '-')
                    {
                        break;
                    }
                }
                sqlCommand.Parameters.AddWithValue("@name", txtNombre);
                sqlCommand.Parameters.AddWithValue("@quantity", txtQuantity);
                sqlCommand.Parameters.AddWithValue("@price", txtPrice);
                sqlCommand.Parameters.AddWithValue("@cat", Convert.ToInt32(getId));
                sqlCommand.Parameters.AddWithValue("id", id);
                sqlCommand.CommandText = updatePersona;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = con;
                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Actualizado exitosamente");
                con.Close();
                if (quantity != txtQuantity)
                {
                    if (quantity < txtQuantity)
                    {
                        NewRecord(Convert.ToInt32(id), (txtQuantity - quantity), "Entrada");
                    }
                    else
                    {
                        NewRecord(Convert.ToInt32(id), (txtQuantity - quantity), "Salida");

                    }
                }

                return true;
            }
            catch (Exception en)
            {
                MessageBox.Show("Error  al editar producto: " + en);
                return false;
            }
        }

        public static DataTable FilterReport(string query, string parameterA)
        {
            try
            {
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                var sqlCommand = new SqlCommand();
                sqlCommand.CommandText = query;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = con;
                sqlCommand.Parameters.AddWithValue("@Parameter",
                    parameterA);
                var sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                var dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                con.Close();
                return dataTable;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al filtrar un elemento " + e);
                return null;
            }
            
        }

        public static DataTable FilterReport(string query, string parameterA, string parameterB)
        {
            try
            {
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                var sqlCommand = new SqlCommand();
                sqlCommand.CommandText = query;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = con;
                sqlCommand.Parameters.AddWithValue("@ParameterA",
                    parameterA);
                sqlCommand.Parameters.AddWithValue("@ParameterB",
                    parameterB);
                var sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                var dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                con.Close();
                return dataTable;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al filtrar dos elementos " + e);
                return null;
            }

        }
        public static DataTable FilterReport(string query, string parameterA, string parameterB, string parameterC)
        {
            try
            {
                var con = new SqlConnection(GlobalVariables.GetConString());
                con.Open();
                var sqlCommand = new SqlCommand();
                sqlCommand.CommandText = query;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = con;
                sqlCommand.Parameters.AddWithValue("@ParameterA",
                    parameterA);
                sqlCommand.Parameters.AddWithValue("@ParameterB",
                    parameterB);
                sqlCommand.Parameters.AddWithValue("@ParameterC",
                    parameterC);
                var sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                var dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                con.Close();
                return dataTable;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al filtrar tres elementos " + e);
                return null;
            }
        }
    }
}
