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
            var cadenaSql = "INSERT INTO modelo.propiedad_horizontal(nit, nombre, numero_propiedades) values ('"+nit.Text+"','"+nombrePropiedad.Text+"','0');";
            conn.registrar(cadenaSql);
            MessageBox.Show("LA PROPIEDAD HA SIDO REGISTRADA CONTINUE CON EL REGISTRO DE LA ASAMBLEA");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {  Select sl = tipoasambleabix.SelectedItem as Select;
           string valor = sl.Value;
            Select sl1 = comboBox1.SelectedItem as Select;
            string valor1 = sl1.Value;
            ConexionPostgres conn = new ConexionPostgres();
            var cadenaSql = "INSERT INTO modelo.asamblea(nit, nombre, fecha, tiempo_inicial, tiempo_final, id_tipo_asamblea) VALUES('"+ valor1+ "','" + nombreasamblea.Text + "', '" + dtp.Value.Date.Year + "-" +dtp.Value.Date.Month + "-" + dtp.Value.Date.Day + "', '" + horaasamblea.Text+ "', '"+ horafinal.Text + "', '"+valor+ "'); ";
            conn.registrar(cadenaSql);
            MessageBox.Show("SE REALIZO EL REGISTRO DE LA ASAMBLEA CONTINUAR EN EL FORMULARIO PRINCIPAL");
        }


        private void IniciarAsamblea_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
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

            var resultado1 = conn.consultar("SELECT * FROM modelo.propiedad_horizontal; ");
            List<Select> sl1 = new List<Select>();
            foreach (Dictionary<string, string> fila in resultado1)
            {
                int numVal1 = Int32.Parse(fila["nit"]);
                // tipoasambleabix.Items.Add(new ListItem ( fila["nombre"], numVal));
                sl1.Add(new Select() { Text = fila["nit"] + "- " + fila["nombre"], Value = fila["nit"] });
            }
            comboBox1.DataSource = sl1;
            comboBox1.DisplayMember = "Text";

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
