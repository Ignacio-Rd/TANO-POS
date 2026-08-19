using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocios;
using Negocio_;
using Dominio;

namespace TPFinalNivel2_RuizDiaz
{
    public partial class ListaDeudores : Form
    {
        List<Cliente> lista = new List<Cliente>();

        private ClienteNegocio clienteNegocio = new ClienteNegocio();


        public ListaDeudores()
        {
            InitializeComponent();
        }

        private void Cargar()
        {
            ClienteNegocio negocio = new ClienteNegocio();
            lista = negocio.listarActivos();
            DgvDeudores.DataSource = lista;

            // --- ESTO ES LO QUE FALTA PARA QUE NO SE VEA VACÍO ---
            DgvDeudores.Columns["Nombre"].DataPropertyName = "Nombre";
            // Si NumeroID es el nombre en tu clase Cliente:
            if (DgvDeudores.Columns.Contains("NumeroID"))
                DgvDeudores.Columns["NumeroID"].DataPropertyName = "NumeroID";

            FormatearGrilla();
        }

        private void CargarComboDeudores()
        {
            try
            {
                // 1. Configuramos el comportamiento primero
                cboListaDeudores.ValueMember = "Id";
                cboListaDeudores.DisplayMember = "Nombre";

                // 2. Asignamos los datos después
                cboListaDeudores.DataSource = clienteNegocio.listarActivos();

                // 3. Limpiamos la selección inicial
                cboListaDeudores.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ListaDeudores_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true; // ← AGREGÁ ESTO
            VentaNegocio negocio = new VentaNegocio();
            Cargar();
            CargarComboDeudores();
            DeudaHistorica.Text = "deuda total fiados";
            CargarSaldos();
           
        }

        // Método dentro de tu Form ListaDeudores
        private void CargarSaldos()
        {
            decimal acumuladorSaldos = 0;

            foreach (DataGridViewRow fila in DgvDeudores.Rows)
            {
                // Verificamos que no sea nulo y que sea un valor numérico
                if (fila.Cells["Deuda Pendiente"].Value != null && fila.Cells["Deuda Pendiente"].Value != DBNull.Value)
                {
                    // Ahora la conversión no fallará porque es un decimal puro
                    acumuladorSaldos += Convert.ToDecimal(fila.Cells["Deuda Pendiente"].Value);
                }
            }

            lblDeuda.Text = acumuladorSaldos.ToString("C2");
        }



        private void cboListaDeudores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboListaDeudores.SelectedIndex == -1)
            {
                DgvDeudores.DataSource = lista;
            }
            else
            {
                Cliente seleccionado = (Cliente)cboListaDeudores.SelectedItem;
                // Importante: .ToList() para que cree una lista nueva
                DgvDeudores.DataSource = lista.Where(x => x.Id == seleccionado.Id).ToList();
            }

            // Llamamos al formateo que ahora crea la columna si falta
            FormatearGrilla();
        }

        private void FormatearGrilla()
        {
            // 1. Validamos si la columna de deuda NO existe, y si no, la creamos
            if (!DgvDeudores.Columns.Contains("Deuda Pendiente"))
            {
                DgvDeudores.Columns.Add("Deuda Pendiente", "Saldo Pendiente");
            }

            // 2. Ocultamos las columnas técnicas (como venías haciendo)
            string[] columnasAOcultar = { "Id", "Estado", "NombreYDocumento", "NumeroID" };
            foreach (var col in columnasAOcultar)
            {
                if (DgvDeudores.Columns.Contains(col)) DgvDeudores.Columns[col].Visible = false;
            }

            // 3. Ordenamos: Nombre primero (0), Deuda segundo (1)
            if (DgvDeudores.Columns.Contains("Nombre")) DgvDeudores.Columns["Nombre"].DisplayIndex = 0;
            if (DgvDeudores.Columns.Contains("Deuda Pendiente")) DgvDeudores.Columns["Deuda Pendiente"].DisplayIndex = 1;

            // 4. Cargamos los datos (esto es lo que te tiraba error, ahora ya no)
            ClienteNegocio negocio = new ClienteNegocio();
            foreach (DataGridViewRow fila in DgvDeudores.Rows)
            {
                Cliente cliente = (Cliente)fila.DataBoundItem;
                // Dentro del foreach de FormatearGrilla
                if (cliente != null)
                {
                    decimal deuda = negocio.ObtenerDeudaActual(cliente.Id);

                    // GUARDAMOS EL DECIMAL PURO (sin signo $)
                    fila.Cells["Deuda Pendiente"].Value = deuda;
                }

                // CONFIGURAMOS LA COLUMNA PARA QUE SE VEA COMO MONEDA AUTOMÁTICAMENTE
                DgvDeudores.Columns["Deuda Pendiente"].DefaultCellStyle.Format = "C2";
            }

            DgvDeudores.Columns["Nombre"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Alineamos el saldo a la derecha para que se lea mejor como dinero
            DgvDeudores.Columns["Deuda Pendiente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Evita la fila vacía al final que queda fea visualmente
            DgvDeudores.AllowUserToAddRows = false;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            
             cboListaDeudores.SelectedIndex = -1;

            // 2. Volver a mostrar la lista completa en la grilla
            // Usamos la variable 'lista' que ya tenías cargada para que sea instantáneo
            DgvDeudores.DataSource = lista;

            // 3. Aplicar el formato para que se oculten las columnas y se vea el saldo
            FormatearGrilla();
        }

        private void DgvDeudores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Cliente seleccionado = (Cliente)DgvDeudores.Rows[e.RowIndex].DataBoundItem;

                // Pasamos el cliente seleccionado al constructor del nuevo form
                DetalleDeCompra ventana = new DetalleDeCompra(seleccionado);
                ventana.ShowDialog();
            }
        }

     
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Verificamos si hay una fila seleccionada actualmente
                if (DgvDeudores.CurrentRow != null)
                {
                    // Usamos CurrentRow en lugar de e.RowIndex
                    Cliente seleccionado = (Cliente)DgvDeudores.CurrentRow.DataBoundItem;

                    string nombre = seleccionado.Nombre;
                    int id = seleccionado.Id;

                    // Importante: Asegúrate que el nombre de la columna sea exacto al de tu grilla
                    string saldoTexto = DgvDeudores.CurrentRow.Cells["Deuda Pendiente"].Value.ToString();
                    decimal deudaActual = decimal.Parse(saldoTexto, System.Globalization.NumberStyles.Currency);

                    frmCancelaroBajarDeuda formulario = new frmCancelaroBajarDeuda(nombre, deudaActual, id);
                    formulario.ShowDialog();
                    Cargar();
                    CargarSaldos();
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione una fila en la tabla primero.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void ListaDeudores_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyCode == Keys.Escape) 
            {
                this.Close();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
    }


