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
    public partial class RegistroFinal : Form
    {
            private bool entro = false;
            private string fecha;
            
            public RegistroFinal(string fecha)
            {
                InitializeComponent();
                this.fecha = fecha;
                this.AutoSize = true;
                FlowLayoutPanel panel = new FlowLayoutPanel();
                panel.AutoSize = true;
                panel.FlowDirection = FlowDirection.TopDown;
                this.Controls.Add(panel);
                this.KeyPreview = true;
                this.KeyPress +=
                new KeyPressEventHandler(Registro_KeyPress);
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
                    var texto = this.textBox1.Text;
                    string[] textos = texto.Split('\'');
                    MessageBox.Show(textos[1]);
                    if (e.KeyChar == 49)
                    {
                        string uni = textos[1].Remove(textos[1].Length - 1);
                        MessageBox.Show("UNIDAD " + textos[1] + "LA OPCION SELECTIONADA FUE: '" +
                        e.KeyChar.ToString() + "'PRESENCIAL .");
                        var cadenaSql = "UPDATE modelo.asamblea_unidad_residencial SET id_tipo_asistencia_final ='1' WHERE nit = '" + textos[0] + "'AND numero_unidad= '"+uni+ "' AND fecha='" + this.fecha + "' AND id_tipo_asistencia_final ='3';";
                        conn.registrar(cadenaSql);
                    }
                    if (e.KeyChar == 50)
                    {
                        string uni = textos[1].Remove(textos[1].Length - 1);
                        MessageBox.Show("UNIDAD " + textos[1] + "LA OPCION SELECTIONADA FUE: '" +
                        e.KeyChar.ToString() + "'PODER .");
                    var cadenaSql = "UPDATE modelo.asamblea_unidad_residencial SET id_tipo_asistencia_final ='2' WHERE nit = '" + textos[0] + "'AND numero_unidad= '" + uni + "' AND fecha='" + this.fecha + "' AND id_tipo_asistencia_final ='3';";
                    conn.registrar(cadenaSql);
                    }
                if (e.KeyChar == 51)
                {
                    string uni = textos[1].Remove(textos[1].Length - 1);
                    MessageBox.Show("UNIDAD " + textos[1] + "LA OPCION SELECTIONADA FUE: '" +
                    e.KeyChar.ToString() + "'DESCARGAR ANTES DE FINALIZAR .");
                    var cadenaSql = "UPDATE modelo.asamblea_unidad_residencial SET id_tipo_asistencia_final ='4' WHERE nit = '" + textos[0] + "'AND numero_unidad= '" + uni + "' AND fecha='" + this.fecha + "' AND id_tipo_asistencia_final ='3';";
                    conn.registrar(cadenaSql);
                }

                entro = false;
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

        private void RegistroFinal_Load(object sender, EventArgs e)
        {

        }
    }
    }

