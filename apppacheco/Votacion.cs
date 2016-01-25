using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apppacheco
{
    public partial class Votacion : Form
    {
        private bool entro = false;
        private string fecha;
        string id_pregunta_actual;
        private string valor;
       
        public Votacion(string fecha, string valor)
        {
            InitializeComponent();
            this.fecha = fecha;
            this.valor = valor;
            this.AutoSize = true;
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.AutoSize = true;
            panel.FlowDirection = FlowDirection.TopDown;
            this.Controls.Add(panel);
            this.KeyPreview = true;
            this.KeyPress +=
            new KeyPressEventHandler(Votacion_KeyPress);
          }
        void Votacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '¡')
            {
                entro = true;
                return;
            }
            if (entro == true)
            {
                votarPorOpcion(e.KeyChar);
                entro = false;
            }
        }

        private void votarPorOpcion(char opcion)
        {
            //carcater esta entre 48='0' y 57= '9'  http://www.asciitable.com/
            if (48 <= opcion && opcion <= 57)
            {
                ConexionPostgres conn = new ConexionPostgres();
                var texto = this.textBox1.Text;
                string[] textos = texto.Split('\'');
                string numero_unidad = textos[1].Remove(textos[1].Length - 1);
                string nit = textos[0];
                MessageBox.Show("UNIDAD " + textos[1] + "LA OPCION SELECTIONADA FUE: " + opcion.ToString());
                var cadenaSql = "INSERT INTO modelo.voto (id_pregunta, numero_unidad, id_opcion, nit, fecha) VALUES ('"+this.id_pregunta_actual +"','"+numero_unidad+"','"+opcion.ToString()+"','"+nit+"','"+this.fecha+"');";
                conn.registrar(cadenaSql);
            }
    }
        void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 48)
            {
                MessageBox.Show("Control.KeyPress: '" +
                    e.KeyChar.ToString() + "' pressed.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Votacion_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            ConexionPostgres conn = new ConexionPostgres();
            string cadenaSql = "SELECT id_pregunta FROM modelo.pregunta_actual WHERE nit = '" + valor + "' AND fecha = '" + fecha + "';";
            var resultado = conn.consultar(cadenaSql);
            this.id_pregunta_actual = resultado[0]["id_pregunta"];
            string cadenaSql1 = "SELECT pregunta FROM modelo.pregunta WHERE id_pregunta='" + id_pregunta_actual + "';";
            var resultado1 = conn.consultar(cadenaSql1);
            string pregunta_actual = resultado1[0]["pregunta"];
            pregunta.Text = pregunta_actual;
            string cadenaSql2 = "SELECT id_opcion, opcion FROM modelo.opcion_pregunta WHERE  id_pregunta='" + id_pregunta_actual + "' ORDER BY id_opcion;";
            var resultado2 = conn.consultar(cadenaSql2);
            string opcion = resultado2[0]["opcion"];
            Label[] labels = new Label[10];
            labels[0] = label1;
            labels[1] = label2;
            labels[2] = label3;
            labels[3] = label4;
            labels[4] = label5;
            labels[5] = label6;
            labels[6] = label7;
            labels[7] = label8;
            labels[8] = label9;
            labels[9] = label10;
            foreach (Dictionary<string, string> fila in resultado2)
            {
                int indice = Int32.Parse(fila["id_opcion"]);
                indice = indice - 1;
                labels[indice].Text = fila["id_opcion"] + "-" + fila["opcion"];
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
        }
    }
}
