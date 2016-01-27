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
    public partial class OpcionesAsamblea : Form
    {
        private string fecha;
        private string valor;
        public OpcionesAsamblea(string fecha, string valor)
        {
            InitializeComponent();
            this.fecha = fecha;
            this.valor = valor;
        }

        private void OpcionesAsamblea_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Registro re = new Registro(this.fecha);
            re.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RegistroFinal refi = new RegistroFinal(this.fecha);
            refi.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Pregunta pre = new Pregunta(this.fecha, this.valor);
            pre.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string quorum = "";
            ConexionPostgres conn = new ConexionPostgres();
            string cadenaSql = "SELECT count(*) FROM modelo.asamblea_unidad_residencial WHERE nit='" + this.valor + "' AND fecha='" + this.fecha + "';";
            double asistenciaCasoUnidades = Double.Parse(conn.consultar(cadenaSql)[0]["count"]);
            string cadenaSql1 = "SELECT count(*) FROM modelo.unidad_residencial WHERE nit='" + this.valor + "';";
            double registradosCasoUnidades = Double.Parse(conn.consultar(cadenaSql)[0]["count"]);
            //double porcentaje = (100 * asistenciaCasoUnidades / registradosCasoUnidades);
            //porcentaje = Math.Round(porcentaje, 2);
            //quorum = (porcentaje).ToString();

            cadenaSql = "SELECT sum(b.coeficiente) FROM modelo.asamblea_unidad_residencial AS a LEFT JOIN modelo.unidad_residencial AS b ON (a.numero_unidad = b.numero_unidad AND a.nit = b.nit) WHERE a.nit='" + this.valor + "' AND a.fecha='" + this.fecha + "';";
            double asistenciaCasoCoeficientes = Double.Parse(conn.consultar(cadenaSql)[0]["sum"]);

            cadenaSql = "SELECT sum(coeficiente) FROM modelo.unidad_residencial WHERE nit='" + this.valor + "';";
            double registradosCasoCoeficientes = Double.Parse(conn.consultar(cadenaSql)[0]["sum"]);

            double porcentaje = (100 * asistenciaCasoCoeficientes / registradosCasoCoeficientes);
            porcentaje = Math.Round(porcentaje, 2);
            quorum = (porcentaje).ToString();

            label5.Text = quorum +"%";

            label5.Visible=true;
            }

        private void button6_Click(object sender, EventArgs e)
        {
            label5.Visible =false;
        }
    }
    }

