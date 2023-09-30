using ModeloParcial.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloParcial.Datos.Interfaces
{
    public interface IDaoOrden
    {
        bool InsertarOrden(OrdenRetiro od);
    }
}
