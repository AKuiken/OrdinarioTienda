using System;
using System.Web.UI;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;

namespace OrdinarioTienda.Views
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public string EncriptarContrasena(string contrasena)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contrasena));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string correo = txtUsuario.Text; 
            string contrasena = txtContrasena.Text; 
            string contrasenaEncriptada = EncriptarContrasena(contrasena);
            string connectionString = "Server=localhost;Database=tiendaescolar;User ID=root;Password=root;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM clientes WHERE Correo = @Correo AND Contraseña = @Contrasena";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Correo", correo);
                        cmd.Parameters.AddWithValue("@Contrasena", contrasenaEncriptada);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            Response.Redirect("Articulos.aspx");
                        }
                        else
                        {
                            Response.Write("<script>alert('Correo o contraseña incorrectos');</script>");
                        }
                    }
                }
            }
            catch (Exception)
            {
                Response.Write("<script>alert('Error al intentar iniciar sesión. Intente nuevamente más tarde.');</script>");
            }
        }
    }
}



