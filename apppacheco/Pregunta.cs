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
    public partial class Pregunta : Form
    {
        private string fecha;
        private string valor;
        public Pregunta(string fecha, string valor)
        {
            InitializeComponent();
            this.fecha = fecha;
            this.valor = valor;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] opcionesText = new string[10];
            opcionesText[0] = textBox1.Text;
            opcionesText[1] = textBox2.Text;
            opcionesText[2] = textBox3.Text;
            opcionesText[3] = textBox4.Text;
            opcionesText[4] = textBox5.Text;
            opcionesText[5] = textBox6.Text;
            opcionesText[6] = textBox7.Text;
            opcionesText[7] = textBox8.Text;
            opcionesText[8] = textBox9.Text;
            opcionesText[9] = textBox10.Text;
            List<string> opciones = new List<string>();
            for (int i = 0; i<opcionesText.ToArray().Length; i++)
            {
                if (opcionesText[i] != "")
                {
                    opciones.Add(opcionesText[i]);
                }
            }
            ConexionPostgres conn = new ConexionPostgres();
            var cadenaSql = "INSERT INTO modelo.pregunta(pregunta) VALUES('"+ textBox12.Text+ "') RETURNING id_pregunta;";
            var id_pregunta = conn.consultar(cadenaSql)[0]["id_pregunta"];
            Select sl1 = comboBox1.SelectedItem as Select;
            //var cadenaSql2 = "INSERT INTO modelo.pregunta_actual(nit, fecha,id_pregunta) VALUES('" + this.valor + "','"+this.fecha+"','"+id_pregunta+"') ;";
            var cadenaSql2 = "DELETE FROM modelo.pregunta_actual WHERE nit = '"+this.valor+"'; INSERT INTO modelo.pregunta_actual(nit, fecha, id_pregunta) VALUES('"+this.valor+"', '"+this.fecha+"', '"+id_pregunta+"'); ";
            conn.registrar(cadenaSql2);
            for (int i = 0; i < opciones.ToArray().Length; i++)
            {
                var id_opcion = i + 1;
                var cadenaSql1 = "INSERT INTO modelo.opcion_pregunta(id_opcion, id_pregunta, opcion) VALUES('" + id_opcion + "', '"+id_pregunta+"', '" + opciones[i] + "')";
                conn.registrar(cadenaSql1);
            }
            MessageBox.Show("LA PREGRUNTA HA SIDO REGISTRADA CONTINUE CON LA VOTACION");
            Votacion voto = new Votacion(this.fecha, this.valor);
            voto.Show();
 }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //RegistroFinal mformulario = new RegistroFinal();
            //mformulario.Show();
        }

        private void OpcionesAsamblea_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            ConexionPostgres conn = new ConexionPostgres();
            var resultado = conn.consultar("SELECT * FROM modelo.asamblea; ");
            List<Select> sl = new List<Select>();
            foreach (Dictionary<string, string> fila in resultado)
            {
                int numVal = Int32.Parse(fila["nit"]);
                // tipoasambleabix.Items.Add(new ListItem ( fila["nombre"], numVal));
                sl.Add(new Select() { Text = fila["nit"] + "-" + fila["nombre"], Value = fila["nit"] });
            }
            comboBox1.DataSource = sl;
            comboBox1.DisplayMember = "Text";
            //http://stackoverflow.com/questions/3063320/combobox-adding-text-and-value-to-an-item-no-binding-source 

        }
    }
}
