using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloParcial.Entidades
{
    public class Material
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public Material(int codigo, string nombre, int stock)
        {
			Codigo = codigo;
			Nombre = nombre;
			Stock = stock;
        }
        public Material()
        {
            
        }
        public override string ToString()
        {
			return Nombre;
        }
    }
}
