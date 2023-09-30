using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloParcial.Entidades
{
    public class OrdenRetiro
    {
        public int NroOrden { get; set; }
        public DateTime fecha { get; set; }
		public List<DetalleOrden> detalles { get; set; }
        public string Responsable { get; set; }
        public OrdenRetiro()
        {
			detalles = new List<DetalleOrden>();
        }
        public void AgregarDetalle(DetalleOrden detalleOrden)
        {
            detalles.Add(detalleOrden);
        }
        public void EliminarDetalle(int index)
        {
            detalles.RemoveAt(index);
        }
    }
}
