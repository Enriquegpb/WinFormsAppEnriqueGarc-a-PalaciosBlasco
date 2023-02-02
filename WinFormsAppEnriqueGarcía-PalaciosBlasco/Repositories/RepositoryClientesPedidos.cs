using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using WinFormsAppEnriqueGarcía_PalaciosBlasco.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata;
using System.Windows.Forms;

namespace WinFormsAppEnriqueGarcía_PalaciosBlasco.Repositories
#region PROCDURE CARGAR CLIENTES
//    CREATE PROCEDURE SP_CLIENTES AS
//    SELECT EMPRESA FROM CLIENTES
//GO
//EXEC SP_CLIENTES
#endregion
#region PROCEDURE CARGAR DATOS CLIENTE
//    CREATE PROCEDURE SP_CLIENTE_EMPRESA(@EMPRESA NVARCHAR(50)) AS

//    SELECT* FROM CLIENTES WHERE EMPRESA = @EMPRESA
//GO
//EXEC SP_CLIENTE_EMPRESA 'BOCACHO'
#endregion
#region PROCEDURE ACTUALIZAR CLIENTE
//ALTER PROCEDURE SP_MODIFICAR_CLIENTE(@CODIGO NVARCHAR(50),@EMPRESA NVARCHAR(50),@EMPRESANEW NVARCHAR(50),
//@CONTACTO NVARCHAR(50), @CARGO NVARCHAR(50), @CIUDAD NVARCHAR(50), @TELEFONO NVARCHAR(50)) AS
//    UPDATE CLIENTE SET EMPRESA = @EMPRESANEW, CONTACTO = @CONTACTO,
//	CARGO = @CARGO, CIUDAD = @CIUDAD, TELEFONO = @TELEFONO WHERE CODIGO = @CODIGO
//GO
#endregion

#region PROCEDURE MOSTRAR PEDIDOS
//    CREATE PROCEDURE SP_PEDIDOS(@EMPRESA NVARCHAR(50)) AS
//    DECLARE @CODCLIENTE NVARCHAR(50)

//    SELECT @CODCLIENTE = CODIGOCLIENTE FROM CLIENTES WHERE EMPRESA = @EMPRESA


//    SELECT* FROM PEDIDOS WHERE CODIGOCLIENTE = @CODCLIENTE
//GO
#endregion
#region INSERTAR CLIENTES
//    CREATE PROCEDURE SP_INSERTAR_PEDIDOS(@CODIGOP NVARCHAR(50), @CODIGOC NVARCHAR(50), @FECHA NVARCHAR(50), @FORMAENTREGA NVARCHAR(50), @IMPORTE INT) AS
//    INSERT INTO pedidos VALUES(@CODIGOP, @CODIGOC, @FECHA, @FORMAENTREGA, @IMPORTE)
//GO
#endregion
{
    public class RepositoryClientesPedidos
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;
        public RepositoryClientesPedidos()
        {
            string connectionstring = "Data Source=LOCALHOST\\DESARROLLO;Initial Catalog=PRACTICAADO;User ID=SA;Password=MCSD2023";
            cn = new SqlConnection(connectionstring);
            this.com = new SqlCommand();
            this.com.Connection= cn;
            
        }

        public List<string> GetClientes()
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTES";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List <string> listaClientes = new List<string>();
            while(this.reader.Read())
            {
                listaClientes.Add(this.reader["EMPRESA"].ToString());
            }
            this.cn.Close();
            this.reader.Close();
            return listaClientes;
        }

        public DatosCliente GetDatosCliente(string empresa)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTE_EMPRESA";
            SqlParameter pamempresa = new SqlParameter("@EMPRESA", empresa);
            this.com.Parameters.Add(pamempresa);

            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            DatosCliente datosClienteEmpresa = new DatosCliente();
            while (this.reader.Read())
            {
                string codigoCliente = this.reader["CodigoCliente"].ToString();
                string empresaCliente = this.reader["Empresa"].ToString();
                string contactoCliente = this.reader["Contacto"].ToString();
                string cargoCliente = this.reader["Cargo"].ToString();
                string ciudadCliente = this.reader["Ciudad"].ToString();
                string telefonoCliente = this.reader["Telefono"].ToString();


                datosClienteEmpresa.CodigoCliente = codigoCliente;
                datosClienteEmpresa.Empresa = empresaCliente;
                datosClienteEmpresa.Contacto = contactoCliente;
                datosClienteEmpresa.Cargo = cargoCliente;
                datosClienteEmpresa.Ciudad = ciudadCliente;
                datosClienteEmpresa.Telefono = telefonoCliente;
            }
            

            this.cn.Close();
            this.com.Parameters.Clear();
            return datosClienteEmpresa;
        }
        public int UpdateClientes(string empresa, DatosCliente dc)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_MODIFICAR_CLIENTE";
            SqlParameter pamcodigo = new SqlParameter("@CODIGO", dc.CodigoCliente);
            this.com.Parameters.Add(pamcodigo); 
            SqlParameter pamempresa = new SqlParameter("@EMPRESA", empresa);
            this.com.Parameters.Add(pamempresa);
            SqlParameter pamnuevaempresa = new SqlParameter("@EMPRESANEW", dc.Empresa);
            this.com.Parameters.Add(pamnuevaempresa);
            SqlParameter pamcontacto = new SqlParameter("@CONTACTO", dc.Contacto);
            this.com.Parameters.Add(pamcontacto);
            SqlParameter pamcargo = new SqlParameter("@CARGO", dc.Cargo);
            this.com.Parameters.Add(pamcargo);
            SqlParameter pamciudad = new SqlParameter("@CIUDAD", dc.Ciudad);
            this.com.Parameters.Add(pamciudad);
            SqlParameter pamtelefono = new SqlParameter("@TELEFONO", dc.Ciudad);
            this.com.Parameters.Add(pamtelefono);

            this.cn.Open();
            int modificados = this.com.ExecuteNonQuery();
            this.cn.Close();

            return modificados;
        }

        public List<DatosPedido> GetPedidos(string empresa)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_PEDIDOS";
            SqlParameter pamempresa = new SqlParameter("@EMPRESA", empresa);
            this.com.Parameters.Add(pamempresa);
            this.cn.Open();
            List<DatosPedido> listadp = new List<DatosPedido>();
            DatosPedido dp = new DatosPedido();
            this.reader = this.com.ExecuteReader();
            while (this.reader.NextResult())
            {
                dp.CodigoPedido = this.reader["CodigoPedido"].ToString();
                dp.CodigoCliente = this.reader["CodigoCliente"].ToString();
                dp.FechaEntrega = this.reader["FechaEntrega"].ToString();
                dp.FormaEnvio = this.reader["FormaEnvio"].ToString();
                dp.Importe = int.Parse( this.reader["Importe"].ToString());
                listadp.Add(dp);

            }
            this.cn.Close();
            this.reader.Close();
            this.com.Parameters.Clear();
            return listadp;
        }

        public int InsertarPedidos(DatosPedido dp)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_PEDIDOS";
            SqlParameter pamcodigop = new SqlParameter("@CODIGOP", dp.CodigoPedido);
            this.com.Parameters.Add(pamcodigop);
            SqlParameter pamcodigoc = new SqlParameter("@CODIGOC", dp.CodigoCliente);
            this.com.Parameters.Add(pamcodigoc);
            SqlParameter pamfecha = new SqlParameter("@FECHA", dp.FechaEntrega);
            this.com.Parameters.Add(pamfecha);
            SqlParameter pamenvio = new SqlParameter("@FORMAENVIO", dp.FormaEnvio);
            this.com.Parameters.Add(pamenvio);
            SqlParameter pamimporte = new SqlParameter("@IMPORTE", dp.Importe);
            this.com.Parameters.Add(pamenvio);
            this.cn.Open();
            int insertados = this.com.ExecuteNonQuery();
            this.cn.Close();
            
            return insertados;
        }

    }
}
