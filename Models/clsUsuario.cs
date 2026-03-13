using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace apiRESTBdUsuarioTADS.Models
{
    public class clsUsuario
    {

        // Definición de atributos
        public string cve { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string ruta { get; set; }
        public string tipo { get; set; }

        // Definición de cadena de Conexión
        private string cadConn = ConfigurationManager.
                    ConnectionStrings["bdControlAcceso"].
                    ConnectionString;
        private string filtro;

        // Para pruebas de chequeo de conexión a BD MySql
        public int ban { get; set; }
        public string statusMsg { get; set; }

        // Definición de Constructores del Modelo
        public clsUsuario(string usuario,
                          string contrasena)
        {
            this.usuario = usuario;
            this.contrasena = contrasena;
        }
        public clsUsuario(string nombre,
                          string apellidoPaterno,
                          string apellidoMaterno,
                          string usuario,
                          string contrasena,
                          string ruta,
                          string tipo)
        {
            this.nombre = nombre;
            this.apellidoPaterno = apellidoPaterno;
            this.apellidoMaterno = apellidoMaterno;
            this.usuario = usuario;
            this.contrasena = contrasena;
            this.ruta = ruta;
            this.tipo = tipo;
        }

        public clsUsuario()
        {
        }

        public clsUsuario(string filtro)
        {
            this.filtro = filtro;
        }

        // Definición del método de conexión a MySql
        public void checkBd()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(cadConn);
                conn.Open();
                conn.Close();
                // Conexión exitosa, enviar salida
                ban = 1;
                statusMsg = "Conexión exitosa a MySql!";
            }
            catch (Exception ex)
            {
                // Conexión fallida, enviar salida:
                ban = 0;
                statusMsg = ex.Message.ToString();
            }
        }


        // Definición de Métodos de Proceso
        public DataSet spInsUsuario()
        {
            // Creación del comando SQL
            string cadSql = "CALL spInsUsuario('" + this.nombre + "', '"
                                                  + this.apellidoPaterno + "','"
                                                  + this.apellidoMaterno + "', '"
                                                  + this.usuario + "', '"
                                                  + this.contrasena + "', '"
                                                  + this.ruta + "', "
                                                  + this.tipo + ");";
            // Configuración de los objetosd de conexión a MySQL
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
            DataSet ds = new DataSet();
            // Ejecución del Adaptadora de Datos
            da.Fill(ds, "spInsUsuario");
            return ds;
        }

        public DataSet spValidarAcceso()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "call spValidarAcceso('" + this.usuario + "','"
                                              + this.contrasena + "');";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "spValidarAcceso");
            return ds;
        }

        // Proceso de Reporte de usuarios (vwRptUsuario)
        public DataSet vwRptUsuario(string filtro)
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "select * from vwRptUsuario "+" where Nombre like '%"+filtro+"%' and usuario like '%"+filtro+"%';";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "vwRptUsuario");
            return ds;
        }

        public DataSet vwTipoUsuario()
        {
            // Crear el comando SQL
            string cadSQL = "";
            cadSQL = "select * from vwTipoUsuario;";
            // Configuración de objetos de conexión
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSQL, cnn);
            DataSet ds = new DataSet();
            // Ejecución y salida
            da.Fill(ds, "vwTipoUsuario");
            return ds;
        }

    }
}