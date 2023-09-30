using ModeloParcial.Entidades;
using ModeloParcial.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModeloParcial
{
    public partial class FrmRegistro : Form
    {
        GestorOrdenes gestorOrdenes;
        OrdenRetiro ordenRetiro;
        GestorMateriales gestorMateriales;
        public FrmRegistro()
        {
            InitializeComponent();
            gestorMateriales = new GestorMateriales();
            ordenRetiro = new OrdenRetiro();
            gestorOrdenes = new GestorOrdenes();
        }
        private void FrmRegistro_Load(object sender, EventArgs e)
        {
            dtpFecha.Enabled = false;
            dtpFecha.Value = DateTime.Now;
            CargarComboMateriales();
        }
        private void CargarComboMateriales()
        {
            cboMateriales.DataSource = gestorMateriales.GetAll();
            cboMateriales.DisplayMember = "Nombre";
            cboMateriales.ValueMember = "Codigo";
            cboMateriales.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private bool ValidarDetalle()
        {
            if (cboMateriales.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un material", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            foreach (DataGridViewRow row in dgvDetalleOrden.Rows)
            {
                if (row.Cells["ColMaterial"].Value.ToString() == cboMateriales.Text)
                {
                    MessageBox.Show("El material ya fue agregado", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            if (NumCantidad.Value == 0)
            {
                MessageBox.Show("Debe ingresar una cantidad mayor a 0", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private bool ValidarCantidad(int cantidad, int stock)
        {
            if (cantidad > stock)
            {
                MessageBox.Show($"Debe ingresar una cantidad menor al stock disponible,hay {stock} unidades", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        private bool ValidarOrden()
        {
            if (txtResponsable.Text == string.Empty)
            {
                MessageBox.Show("Debe ingresar un responsable válido", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (dgvDetalleOrden.Rows.Count == 0)
            {
                if (txtResponsable.Text == string.Empty)
                {
                    MessageBox.Show("Debe ingresar mínimo un material", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidarDetalle())
            {
                Material m = (Material)cboMateriales.SelectedItem;
                int cantidad = (int)NumCantidad.Value;
                if (ValidarCantidad(cantidad, m.Stock))
                {

                    DetalleOrden detalleOrden = new DetalleOrden(m, cantidad);

                    ordenRetiro.AgregarDetalle(detalleOrden);

                    object[] row = new object[]
                    {
                    m.Codigo,m.Nombre,m.Stock,cantidad,"Quitar"
                    };

                    dgvDetalleOrden.Rows.Add(row);
                }
            }
        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (ValidarOrden())
            {
                ordenRetiro.fecha = Convert.ToDateTime(dtpFecha.Value);
                ordenRetiro.Responsable = Convert.ToString(txtResponsable.Text);
                if (gestorOrdenes.InsertOrden(ordenRetiro))
                {
                    MessageBox.Show("Se ingresó la orden correctamente", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No pudo ingresarse la orden correctamente", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LimpiarCampos()
        {
            txtResponsable.Text = string.Empty;
            cboMateriales.SelectedIndex = -1;
            NumCantidad.Text = string.Empty;
            dgvDetalleOrden.Rows.Clear();
            ordenRetiro = new OrdenRetiro();
        }

        private void dgvDetalleOrden_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalleOrden.CurrentCell.ColumnIndex == 4)
            {
                ordenRetiro.EliminarDetalle(dgvDetalleOrden.CurrentRow.Index);
                dgvDetalleOrden.Rows.RemoveAt(dgvDetalleOrden.CurrentRow.Index);
            }
        }

    }
}
