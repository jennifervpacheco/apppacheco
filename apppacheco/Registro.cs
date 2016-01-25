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
    public partial class Registro : Form
    {
        private bool entro = false;
        private string fecha;
        TextBox TextBox1 = new TextBox();

        public Registro(string fecha)
        {
            InitializeComponent();
            this.fecha = fecha;
            this.AutoSize = true;
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.AutoSize = true;
            panel.FlowDirection = FlowDirection.TopDown;
            panel.Controls.Add(TextBox1);
            this.Controls.Add(panel);
            this.KeyPreview = true;
            this.KeyPress +=
            new KeyPressEventHandler(Registro_KeyPress);
            TextBox1.KeyPress +=
            new KeyPressEventHandler(TextBox1_KeyPress);
        }
        void Registro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '¡')
            {
                entro = true;
                return;
            }
            if (entro == true)
            {
                ConexionPostgres conn = new ConexionPostgres();
                var resultado = conn.consultar("SELECT * FROM modelo.asamblea; ");
                var texto = this.lectura.Text;
                string[] textos = texto.Split('\'');
                if (e.KeyChar == 49)
                {
                    string uni = textos[1].Remove(textos[1].Length - 1);
                    MessageBox.Show("UNIDAD " + textos[1] + "LA OPCION SELECTIONADA FUE: '" +
                    e.KeyChar.ToString() + "'PRESENCIAL .");
                    var cadenaSql = "INSERT INTO modelo.asamblea_unidad_residencial(nit, numero_unidad, fecha, id_tipo_asistencia_inicial) VALUES ('" + textos[0] + "','" + uni + "','" + this.fecha + "','1');";
                    conn.registrar(cadenaSql);
                }
                if (e.KeyChar == 50)
                {
                    string uni = textos[1].Remove(textos[1].Length - 1);
                    MessageBox.Show("UNIDAD " + textos[1] + "LA OPCION SELECTIONADA FUE: '" +
                    e.KeyChar.ToString() + "'PODER .");
                    var cadenaSql = "INSERT INTO modelo.asamblea_unidad_residencial(nit, numero_unidad, fecha, id_tipo_asistencia_inicial) VALUES ('" + textos[0] + "','" + uni + "','" + this.fecha + "','2');";
                    conn.registrar(cadenaSql);
                }
                entro = false;
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

        private void Registro_Load(object sender, EventArgs e)
        {
            //$("body").keyup(function(e){ console.log(e)})
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            ConexionPostgres conn = new ConexionPostgres();
        }
    }
}
