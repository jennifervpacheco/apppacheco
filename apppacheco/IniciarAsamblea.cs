using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apppacheco
{
    public partial class IniciarAsamblea : Form
    {
        public IniciarAsamblea()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConexionPostgres conn = new ConexionPostgres();
            var cadenaSql = "INSERT INTO modelo.propiedad_horizontal(nit, nombre, numero_propiedades) values ('"+nit.Text+"','"+nombrePropiedad.Text+"','"+numunidades.Text+"');";
            conn.registrar(cadenaSql);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ConexionPostgres conn = new ConexionPostgres();
            var cadenaSql = "INSERT INTO modelo.asamblea(nit, nombre, fecha, ) values ('" + nit.Text + "','" + nombrePropiedad.Text + "','" + numunidades.Text + "');";
            conn.registrar(cadenaSql);
            
        }


        private void IniciarAsamblea_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            ConexionPostgres conn = new ConexionPostgres();
            var resultado = conn.consultar("SELECT * FROM modelo.tipo_asamblea; ");

            List<Select> sl = new List<Select>();
            
            foreach (Dictionary<string, string> fila in resultado)
            {
                int numVal = Int32.Parse(fila["id_tipo_asamblea"]);
               // tipoasambleabix.Items.Add(new ListItem ( fila["nombre"], numVal));
                sl.Add(new Select() { Text =  fila["nombre"], Value = fila["id_tipo_asamblea"] });
            }
            tipoasambleabix.DataSource = sl;
            tipoasambleabix.DisplayMember = "Text";
            //http://stackoverflow.com/questions/3063320/combobox-adding-text-and-value-to-an-item-no-binding-source 
        }

        private void tipoasambleabix_SelectedIndexChanged(object sender, EventArgs e)
        {

            

            

         }

        private void button3_Click(object sender, EventArgs e)
        {
            Pregunta miformulario = new Pregunta();
            miformulario.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
