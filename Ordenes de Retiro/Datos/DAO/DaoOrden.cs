using ModeloParcial.Datos.Interfaces;
using ModeloParcial.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloParcial.Datos.DAO
{
    public class DaoOrden : IDaoOrden
    {
        public bool InsertarOrden(OrdenRetiro od)
        {
            return DBHelper.ObtenerInstancia().ConfirmarOrden(od);
        }
    }
}
