using ModeloParcial.Entidades;
using ModeloParcial.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloParcial.Datos
{
    public class DBHelper
    {
        private static DBHelper instancia = null;
        private SqlConnection conexion;
        private string cadena = @"Data Source=DESKTOP-0D1NIRP;Initial Catalog=db_ordenes;Integrated Security=True";
        public DBHelper()
        {
            conexion = new SqlConnection(cadena);
        }
        public static DBHelper ObtenerInstancia()
        {
            if (instancia == null)
            {
                instancia = new DBHelper();
            }
            return instancia;
        }
        public DataTable Consultar(string nombreSP)
        {
            DataTable dt = new DataTable();
            conexion.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexion;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = nombreSP;
            dt.Load(cmd.ExecuteReader());
            conexion.Close();
            return dt;
        }
        public DataTable Consultar(string nombreSP, List<Parametro> listaParametros)
        {
            conexion.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexion;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = nombreSP;

            cmd.Parameters.Clear();
            foreach (Parametro param in listaParametros)
            {
                cmd.Parameters.AddWithValue(param.Nombre, param.Valor);
            }
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conexion.Close();
            return dt;
        }
        public bool ConfirmarOrden(OrdenRetiro oRetiro)
        {
            bool resultado = true;

            SqlTransaction transaction = null;
            try
            {
                conexion.Open();
                transaction = conexion.BeginTransaction();
                SqlCommand comando = new SqlCommand();
                comando.Connection = conexion;
                comando.Transaction = transaction;
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = "SP_INSERTAR_ORDEN";
                comando.Parameters.AddWithValue("@responsable", oRetiro.Responsable);

                SqlParameter parametro = new SqlParameter();
                parametro.ParameterName = "@nro";
                parametro.SqlDbType = SqlDbType.Int;
                parametro.Direction = ParameterDirection.Output;
                comando.Parameters.Add(parametro);

                comando.ExecuteNonQuery();

                int ordenNro = (int)parametro.Value;
                int nroDetalle = 1;

                foreach (DetalleOrden deo in oRetiro.detalles)
                {
                    SqlCommand cmdDetalle = new SqlCommand("SP_INSERTAR_DETALLES", conexion, transaction);
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@nro_orden", ordenNro);
                    cmdDetalle.Parameters.AddWithValue("@detalle", nroDetalle);
                    cmdDetalle.Parameters.AddWithValue("@codigo", deo.Material.Codigo);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", deo.Cantidad);
                    cmdDetalle.ExecuteNonQuery();
                    nroDetalle++;
                }
                transaction.Commit();
            }
            catch
            {
                if (transaction != null)
                    transaction.Rollback();
                resultado = false;
            }
            finally
            {
                if (conexion != null && conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return resultado;
        }
    }
}
