using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Data;
using Npgsql;
using System.Collections.Generic;


namespace apppacheco
{
    class ConexionPostgres
    {
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
        private List<List<KeyValuePair<string, string>>> filas = new List<List<KeyValuePair<string, string>>>();
        // PostgeSQL-style connection string
        private string connstring = String.Format(
            "Server={0};" +
            "Port={1};" +
            "User Id={2};" +
            "Password={3};" +
            "Database={4};",
            "127.0.0.1",
            "5432",
            "postgres",
            "postgres",
            "apppacheco");
        public List<List<KeyValuePair<string, string>>> consultar(string sql)
        {
            try
            {
                // Making connection with Npgsql provider
                NpgsqlConnection conn = new NpgsqlConnection(this.connstring);

                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    // Insert some data
                    //cmd.CommandText = "INSERT INTO data (some_field) VALUES ('Hello world')";
                    //cmd.ExecuteNonQuery();

                    // Retrieve all rows
                    cmd.CommandText = sql;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            List<KeyValuePair<string, string>> columnas = new List<KeyValuePair<string, string>>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                columnas.Add(new KeyValuePair<string, string>(reader.GetName(i), reader[i].ToString()));
                            }
                            filas.Add(columnas);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why
                Console.WriteLine(msg.ToString());
            }

            return this.filas;
        }

        public bool registrar(string sql)
        {
            try
            {
                // Making connection with Npgsql provider
                NpgsqlConnection conn = new NpgsqlConnection(this.connstring);

                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                return true;
            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why
                return false;
            }
        }
    }
}

