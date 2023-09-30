using ModeloParcial.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloParcial.Datos.DAO
{
    public class DaoMaterial : IDaoMaterial
    {
        public bool InsertarMateriales()
        {
            return false;
        }
        public List<Material> ObtenerMateriales()
        {
            List<Material> materiales = new List<Material>();
            DataTable dt = DBHelper.ObtenerInstancia().Consultar("[SP_CONSULTAR_MATERIALES]");
            foreach (DataRow row in dt.Rows)
            {
                Material m = new Material();
                m.Codigo = Convert.ToInt32(row["codigo"]);
                m.Nombre = Convert.ToString(row["nombre"]);
                m.Stock = Convert.ToInt32(row["stock"]);
                materiales.Add(m);
            }
            return materiales;
        }
    }
}
