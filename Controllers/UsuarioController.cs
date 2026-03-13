using apiRESTBdUsuarioTADS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace apiRESTBdUsuarioTADS.Controllers
{
    public class UsuarioController : ApiController
    {
        [HttpGet]
        [Route("check/checkbd/mysqlconectioncheckbd")]
        public clsApiStatus MysqlConectionCheckBd()
        {
            // -------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -------------------------------------
            // Ejecución del método de conexión
            clsUsuario objCheckBd = new clsUsuario();
            objCheckBd.checkBd();
            // Validar resultado de la ejecución
            if (objCheckBd.ban == 1)
                objRespuesta.statusExec = true;
            else
                objRespuesta.statusExec = false;

            objRespuesta.ban = objCheckBd.ban;
            objRespuesta.msg = objCheckBd.statusMsg;
            jsonResp.Add("msgData", objCheckBd.statusMsg);
            objRespuesta.datos = jsonResp;
            return objRespuesta;       // < -----------------------
        }
   
        [HttpPost]
        [Route("tads/usuario/spinsusuario")]
        public clsApiStatus spInsUsuario([FromBody] clsUsuario modelo)
        {
            // -----------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------
            try
            {
                // Creación del objeto, en base al Modelo
                clsUsuario objUsuario = new clsUsuario(modelo.nombre,
                                                       modelo.apellidoPaterno,
                                                       modelo.apellidoMaterno,
                                                       modelo.usuario,
                                                       modelo.contrasena,
                                                       modelo.ruta,
                                                       modelo.tipo);
                DataSet ds = new DataSet();
                // Ejecución del Método del Modelo (y recepción de datos)
                ds = objUsuario.spInsUsuario();
                // Configuración del objeto de salida
                objRespuesta.statusExec = true;
                objRespuesta.msg = "Usuario registrado exitosamente !";
                objRespuesta.ban =
                         int.Parse(ds.Tables[0].Rows[0][0].ToString());
                jsonResp.Add("msgData", "Usuario registrado exitosamente !");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception e)
            {
                // Configuración del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.msg = "Usuario NO registrado ...";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", e.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }

        // endpoint para validación de acceso spValidarAcceso
        [HttpPost]     // <-------------
        [Route("tads/usuario/spvalidaracceso")]
        public clsApiStatus spValidarAcceso([FromBody] clsUsuario modelo)
        {
            // -----------------------------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------------------------
            DataSet ds = new DataSet();
            try
            {
                // Creación del objeto del modelo clsUsuario
                clsUsuario objUsuario = new clsUsuario(modelo.usuario,
                                                       modelo.contrasena);
                ds = objUsuario.spValidarAcceso();
                // Configuración del objeto de salida
                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                // Validar el valor recibido en bandera
                if (objRespuesta.ban == 1)
                {
                    objRespuesta.msg = "Usuario validado exitosamente!";
                    jsonResp.Add("usu_nombre_completo", ds.Tables[0].Rows[0][1].ToString());
                    jsonResp.Add("usu_ruta", ds.Tables[0].Rows[0][2].ToString());
                    jsonResp.Add("usu_usuario", ds.Tables[0].Rows[0][3].ToString());
                    jsonResp.Add("tip_descripcion", ds.Tables[0].Rows[0][4].ToString());
                    objRespuesta.datos = jsonResp;
                }
                else
                {
                    objRespuesta.msg = "Acceso denegado, verificar ...";
                    jsonResp.Add("msgData", "Acceso denegado, verificar ...");
                    objRespuesta.datos = jsonResp;
                }

            }    // <<----------fin del try
            catch (Exception ex)
            {
                // Configuración del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error de conexión con el servicio de datos";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            // Retorno del obj de Salida objRespuesta
            return objRespuesta;
        }   // <<-------------fin del endpoint


        // Endpoint para consulta de usuarios vwRptUsuario
        [HttpGet]
        [Route("tads/usuario/vwrptusuario")]
        public clsApiStatus vwRptUsuario(string filtro)
        {
            // -----------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------
            DataSet ds = new DataSet();
            try
            {
                clsUsuario objUsuario = new clsUsuario(filtro);
                ds = objUsuario.vwRptUsuario(filtro);
                // Configuración del objeto de salida
                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Consulta de usuario " +
                                    "realizada exitosamente";
                // Migración del ds(DataSet) al objeto Json
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");
                // DataSet migrado, se envía clsApiStatus
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                // Configuración del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.msg =
                    "Fallo en consulta de reporte - Usuario ...";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }
            // Salida  del objeto configurado
            return objRespuesta;
        }

        [HttpGet]
        [Route("tads/usuario/vwTipoUsuario")]
        public clsApiStatus vwTipoUsuario()
        {
            // -----------------------
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            // -----------------------
            DataSet ds = new DataSet();
            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwTipoUsuario();
                // Configuración del objeto de salida
                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Consulta de usuario " +
                                    "realizada exitosamente";
                // Migración del ds(DataSet) al objeto Json
                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"{ds.Tables[0].TableName}\": {jsonString}}}");
                // DataSet migrado, se envía clsApiStatus
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                // Configuración del objeto de salida
                objRespuesta.statusExec = false;
                objRespuesta.msg =
                    "Fallo en consulta de reporte - Usuario ...";
                objRespuesta.ban = -1;
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }
            // Salida  del objeto configurado
            return objRespuesta;
        }

    }

}
