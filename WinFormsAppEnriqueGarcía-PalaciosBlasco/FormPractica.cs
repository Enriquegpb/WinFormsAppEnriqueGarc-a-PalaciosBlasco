using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsAppEnriqueGarcía_PalaciosBlasco.Models;
using WinFormsAppEnriqueGarcía_PalaciosBlasco.Repositories;

namespace WinFormsAppEnriqueGarcía_PalaciosBlasco
{
    public partial class FormPractica : Form
    {
        RepositoryClientesPedidos repository;
        List<string> empresas;
        DatosCliente datosClienteEmpresa;
        public FormPractica()
        {
            InitializeComponent();
            this.repository = new RepositoryClientesPedidos();
            this.empresas = new List<string>();
            datosClienteEmpresa = new DatosCliente();
            this.LoadEmpresas();

        }
        private void LoadEmpresas()
        {
            this.empresas = this.repository.GetClientes();
            foreach(string emp in empresas)
            {
                this.cmbclientes.Items.Add(emp);
            }
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.cmbclientes.SelectedIndex != -1)
            {
                string empresa = this.cmbclientes.SelectedItem.ToString();
                datosClienteEmpresa = this.repository.GetDatosCliente(empresa);

                this.txtempresa.Text = datosClienteEmpresa.Empresa;
                this.txtcontacto.Text = datosClienteEmpresa.Contacto;
                this.txtcargo.Text = datosClienteEmpresa.Cargo;
                this.txtciudad.Text = datosClienteEmpresa.Ciudad;
                this.txttelefono.Text = datosClienteEmpresa.Telefono;

                List<DatosPedido> listapedidos = new List<DatosPedido>();
                listapedidos = this.repository.GetPedidos(empresa);
                foreach(DatosPedido dp in listapedidos)
                {
                    this.lstpedidos.Items.Add(dp.CodigoPedido);
                    
                }
            }
            
        }

        private void btnmodificarcliente_Click(object sender, EventArgs e)
        {
             
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            DatosPedido dp = new DatosPedido();
            dp.CodigoPedido = this.txtcodigopedido.Text;
            dp.FechaEntrega = this.txtcodigopedido.Text;
            dp.FormaEnvio = this.txtcodigopedido.Text;
            dp.Importe = int.Parse(this.txtimporte.Text);

            this.repository.InsertarPedidos(dp);

        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            

        }
    }
}
