using ModeloParcial.Datos.DAO;
using ModeloParcial.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloParcial.Servicios
{
    public class GestorOrdenes
    {
        private DaoOrden DaoOrden;
        public GestorOrdenes()
        {
            DaoOrden = new DaoOrden();
        }
        public bool InsertOrden(OrdenRetiro ordenretiro)
        {
            return DaoOrden.InsertarOrden(ordenretiro);
        }
    }
}
