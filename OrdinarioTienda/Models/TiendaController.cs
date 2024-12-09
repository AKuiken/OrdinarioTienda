using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using OrdinarioTienda.Models;
using System.Configuration;

namespace OrdinarioTienda.Controllers
{
    public class TiendaController
    {
        private static string connectionString = "Server=localhost;Database=tiendaescolar;User ID=root;Password=root;";
        public static List<Articulo> ObtenerArticulos()
        {
            List<Articulo> articulos = new List<Articulo>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Articulos"; 
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Articulo articulo = new Articulo
                    {
                        ID_Articulo = reader.GetInt32("ID_Articulo"),
                        Nombre = !reader.IsDBNull(reader.GetOrdinal("Nombre")) ? reader.GetString("Nombre") : string.Empty,
                        Categoria = !reader.IsDBNull(reader.GetOrdinal("Categoria")) ? reader.GetString("Categoria") : string.Empty,
                        Marca = !reader.IsDBNull(reader.GetOrdinal("Marca")) ? reader.GetString("Marca") : string.Empty,
                        Precio = !reader.IsDBNull(reader.GetOrdinal("Precio")) ? reader.GetDecimal("Precio") : 0.0m,
                        Cantidad_Disponible = !reader.IsDBNull(reader.GetOrdinal("Cantidad_Disponible")) ? reader.GetInt32("Cantidad_Disponible") : 0,
                        Descripcion = !reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? reader.GetString("Descripcion") : string.Empty,
                        Fecha_Entrada = !reader.IsDBNull(reader.GetOrdinal("Fecha_Entrada")) ? reader.GetDateTime("Fecha_Entrada") : DateTime.MinValue
                    };

                    articulos.Add(articulo);
                }
            }

            return articulos;
        }


        // Método para obtener un artículo por su ID
        public static Articulo ObtenerArticuloPorId(int idArticulo)
        {
            Articulo articulo = null;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Articulos WHERE ID_Articulo = @ID_Articulo";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID_Articulo", idArticulo);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    articulo = new Articulo
                    {
                        ID_Articulo = reader.GetInt32("ID_Articulo"),
                        Nombre = reader.GetString("Nombre"),
                        //Categoria = reader.GetString("Categoria"),
                        Marca = reader.GetString("Marca"),
                        Precio = reader.GetDecimal("Precio"),
                        //Cantidad_Disponible = reader.GetInt32("Cantidad_Disponible"),
                        Descripcion = reader.GetString("Descripcion"),
                        //Fecha_Entrada = reader.GetDateTime("Fecha_Entrada")
                    };
                }
            }

            return articulo;
        }

        // Método para agregar un artículo a la base de datos
        public static void AgregarArticulo(Articulo nuevoArticulo)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Articulos (Nombre, Categoria, Marca, Precio, Cantidad_Disponible, Descripcion, Fecha_Entrada) " +
                               "VALUES (@Nombre, @Categoria, @Marca, @Precio, @Cantidad_Disponible, @Descripcion, @Fecha_Entrada)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nuevoArticulo.Nombre);
                cmd.Parameters.AddWithValue("@Categoria", nuevoArticulo.Categoria);
                cmd.Parameters.AddWithValue("@Marca", nuevoArticulo.Marca);
                cmd.Parameters.AddWithValue("@Precio", nuevoArticulo.Precio);
                cmd.Parameters.AddWithValue("@Cantidad_Disponible", nuevoArticulo.Cantidad_Disponible);
                cmd.Parameters.AddWithValue("@Descripcion", nuevoArticulo.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha_Entrada", nuevoArticulo.Fecha_Entrada);

                cmd.ExecuteNonQuery();
            }
        }

        // Método para actualizar un artículo existente
        public static bool ActualizarArticulo(int idArticulo, Articulo articuloActualizado)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Articulos SET " +
                               "Nombre = @Nombre, " +
                               "Categoria = @Categoria, " +
                               "Marca = @Marca, " +
                               "Precio = @Precio, " +
                               "Cantidad_Disponible = @Cantidad_Disponible, " +
                               "Descripcion = @Descripcion, " +
                               "Fecha_Entrada = @Fecha_Entrada " +
                               "WHERE ID_Articulo = @ID_Articulo";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID_Articulo", idArticulo);
                cmd.Parameters.AddWithValue("@Nombre", articuloActualizado.Nombre);
                cmd.Parameters.AddWithValue("@Categoria", articuloActualizado.Categoria);
                cmd.Parameters.AddWithValue("@Marca", articuloActualizado.Marca);
                cmd.Parameters.AddWithValue("@Precio", articuloActualizado.Precio);
                cmd.Parameters.AddWithValue("@Cantidad_Disponible", articuloActualizado.Cantidad_Disponible);
                cmd.Parameters.AddWithValue("@Descripcion", articuloActualizado.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha_Entrada", articuloActualizado.Fecha_Entrada);

                int filasAfectadas = cmd.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }


        // Método para guardar un artículo (agregar o actualizar)
        public static bool GuardarArticulo(Articulo articulo)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = articulo.ID_Articulo == 0 ?
                    "INSERT INTO Articulos (Nombre, Marca, Precio, Descripcion) VALUES (@Nombre, @Marca, @Precio, @Descripcion)" :
                    "UPDATE Articulos SET Nombre = @Nombre, Marca = @Marca, Precio = @Precio, Descripcion = @Descripcion WHERE ID_Articulo = @ID_Articulo";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", articulo.Nombre);
                cmd.Parameters.AddWithValue("@Marca", articulo.Marca);
                cmd.Parameters.AddWithValue("@Precio", articulo.Precio);
                cmd.Parameters.AddWithValue("@Descripcion", articulo.Descripcion);

                if (articulo.ID_Articulo != 0)
                    cmd.Parameters.AddWithValue("@ID_Articulo", articulo.ID_Articulo);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        // Método para eliminar un artículo
        public static bool EliminarArticulo(int idArticulo)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM Articulos WHERE ID_Articulo = @ID_Articulo";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID_Articulo", idArticulo);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }
    }
}


