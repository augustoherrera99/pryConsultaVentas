using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryVentasHerreraAugusto
{
    public partial class frmConsultaDeComisiones : Form
    {
        public frmConsultaDeComisiones()
        {
            InitializeComponent();
        }

        // Declaración nombres de los archivos

        private const string NombreArchivoVendedores = "Vendedor.txt";
        private const string NombreArchivoClientes = "Cliente.txt";
        private const string NombreArchivoVentas = "Ventas.txt";

        private void Form1_Load(object sender, EventArgs e)
        {
            Archivo Vendedores = new Archivo();

            Vendedores.NombreArchivo = NombreArchivoVendedores;

            List<Vendedor> ListaVendedores = Vendedores.ListarVendedores();
            cmbVendedor.Items.Clear();

           // Inicialización ComboBox con nombres de vendedores de la lista "Vendedores"
            
            foreach (Vendedor Vendedor in ListaVendedores)
            {
                cmbVendedor.Items.Add(Vendedor.VendedorNombre);
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                // Declaración Listas
                
                Archivo Vendedores = new Archivo();
                Vendedores.NombreArchivo = NombreArchivoVendedores;
                List<Vendedor> ListaVendedores = Vendedores.ListarVendedores();

                Archivo Ventas = new Archivo();
                Ventas.NombreArchivo = NombreArchivoVentas;
                List<Ventas> ListaVentas = Ventas.ListarVentas();

                Archivo Clientes = new Archivo();
                Clientes.NombreArchivo = NombreArchivoClientes;
                List<Clientes> ListaClientes = Clientes.ListarClientes();

                // Inicializar controles
                dgvConsulta.Rows.Clear();
                lblComision.Text = ""; 
                lblTotalVentas.Text = "";

                // Declaración contador y variables booleanas
                decimal Total = 0;
                bool Comision = false;
                bool TuvoVentas = false;

                // Lectura de listas

                foreach (Vendedor Vendedor in ListaVendedores)
                {
                    foreach (Ventas Venta in ListaVentas)
                    {
                        foreach (Clientes Cliente in ListaClientes)
                        {
                            // Si el vendedor seleccionado en el ComboBox es el leido en la lista vendedores
                            if (cmbVendedor.Text == Vendedor.VendedorNombre)
                            {
                                // Se consulta si ambos ID son iguales para cruzar datos
                                if (Vendedor.VendedorId == Venta.VendedorId)
                                {
                                    // Se consulta si los ID de Cliente son iguales para identificar el nombre del cliente
                                    if (Cliente.ClienteId == Venta.ClienteId)
                                    {
                                        // Se comprueba si las ventas se encuentran dentro del rango de fechas establecido
                                        if (Venta.Fecha >= DateTime.Parse(dtpDesde.Text) && Venta.Fecha <= DateTime.Parse(dtpHasta.Text))
                                        {
                                            TuvoVentas = true;

                                            Total += Venta.Monto;

                                            dgvConsulta.Rows.Add(Venta.FacturaNumero, Venta.FacturaTipo, Venta.Fecha, Cliente.ClienteNombre, Venta.Monto);

                                            lblTotalVentas.Text = Total.ToString("0.00");

                                            if (int.Parse(Vendedor.CobraComision) == 1)
                                            {
                                                Comision = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (Comision == true)
                {
                    decimal TotalComision = Total * 0.15m;
                    lblComision.Text = TotalComision.ToString("0.00");
                }

                if (TuvoVentas == true && Comision == false)
                {
                    lblComision.Text = "No Asignado.";
                }

                if (TuvoVentas == false)
                {
                    MessageBox.Show("El vendedor no ha tenido ventas en ese periodo de tiempo.", "Estado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            else
            {
                
                DialogResult result = MessageBox.Show("Datos incorrectos.", "Estado", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (result == DialogResult.OK)
                {
                    dgvConsulta.Rows.Clear();
                    lblComision.Text = "";
                    lblTotalVentas.Text = "";
                }
            }
        }

        // Se valida que los controles no esten vacios, ni que las fechas sean incongruentes
        public bool ValidarDatos()
        {
            bool resultado = false;
            if (cmbVendedor.Text != "")
            {
                if (dtpDesde.Text != "")
                {
                    if (dtpHasta.Text != "")
                    {
                        if (DateTime.Parse(dtpDesde.Text) <= DateTime.Parse(dtpHasta.Text))
                        {
                            resultado = true;
                        }
                    }
                }
            }
            return resultado;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
