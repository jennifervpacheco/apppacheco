using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Excel2 = Microsoft.Office.Interop.Excel;

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

            cadenaSql = "SELECT sum(b.coeficiente) FROM modelo.asamblea_unidad_residencial AS a LEFT JOIN modelo.unidad_residencial AS b ON (a.numero_unidad = b.numero_unidad AND a.nit = b.nit) WHERE a.nit='" + this.valor + "' AND a.fecha='" + this.fecha + "' and id_tipo_asistencia_final ='4';";
            double registradosCasoUnidadesdescargue = Double.Parse(conn.consultar(cadenaSql)[0]["sum"]);

            double porcentaje = (100 * (asistenciaCasoCoeficientes- registradosCasoUnidadesdescargue) / registradosCasoCoeficientes);
            porcentaje = Math.Round(porcentaje, 2);
            quorum = (porcentaje).ToString();

            label5.Text = quorum +"%";

            label5.Visible=true;
            }

        private void button6_Click(object sender, EventArgs e)
        {
            label5.Visible =false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ConexionPostgres conn = new ConexionPostgres();
            string nit = "999999999";
            //var cadenaSql = "SELECT * FROM modelo.unidad_residencial where nit='"+nit+"';";
            var cadenaSql = " SELECT ur.nit, ur.numero_unidad, ur.nombre_completo, ur.coeficiente, ur.documento, CASE WHEN aur.id_tipo_asistencia_inicial=1 THEN 'presencial' WHEN aur.id_tipo_asistencia_inicial=2 THEN 'poder' WHEN aur.id_tipo_asistencia_inicial=3 THEN 'no asistio' ELSE 'no asistio' END AS tipo_asistencia_inicial, CASE WHEN aur.id_tipo_asistencia_final=1 THEN 'presencial' WHEN aur.id_tipo_asistencia_final=2 THEN 'poder' WHEN aur.id_tipo_asistencia_final=3 THEN 'no asistio' WHEN aur.id_tipo_asistencia_final=4 THEN 'se retiro antes de finalizar' ELSE 'no asistio' END AS tipo_asistencia_final FROM modelo.unidad_residencial AS ur LEFT OUTER JOIN modelo.asamblea_unidad_residencial AS aur ON ur.numero_unidad = aur.numero_unidad WHERE ur.nit='999999999' order by ur.numero_unidad asc;";
            var resultado = conn.consultar(cadenaSql);

            //Excel2.Application excelApp = new Excel2.Application();
            //Excel2.Workbook workbook = null;
            //Excel2.Workbooks workbooks = null;
            //Excel2._Worksheet worksheet = null;
            //workbooks = excelApp.Workbooks;
            //workbook = workbooks.Add(1);
            //worksheet = (Excel2.Worksheet)workbook.Sheets[1];
            //excelApp.Visible = true;
            //worksheet.Cells[1, 1] = "NIT";
            //worksheet.Cells[1, 2] = "NUMERO DE UNIDAD";
            //worksheet.Cells[1, 3] = "NOMBRE";
            //worksheet.Cells[1, 4] = "COEFICIENTE";
            //worksheet.Cells[1, 5] = "DOCUMENTO";
            //worksheet.Cells[1, 6] = "ASISTENCIA INICIAL";
            //worksheet.Cells[1, 7] = "ASISTENCIA FINAL";
            //worksheet.Cells[1, 9] = "QUORUM INICIAL";
            //worksheet.Cells[1, 10] = "QUORUM FINAL";
            //worksheet.Cells[1, 10] = "";

            //int contadornoasistio = 0, contadornoasistiof = 0;
            //int contadornpoder = 0, contadornpoderf = 0;
            //int contadorpresen = 0, contadorpresenf = 0;
            //for (int indice = 0; indice < resultado.ToArray().Length; indice++)
            //{
            //    Dictionary<string, string> fila = resultado.ToArray()[indice];
            //    worksheet.Cells[indice + 2, 1] = fila["nit"];
            //    worksheet.Cells[indice + 2, 2] = fila["numero_unidad"];
            //    worksheet.Cells[indice + 2, 3] = fila["nombre_completo"];
            //    worksheet.Cells[indice + 2, 4] = fila["coeficiente"];
            //    worksheet.Cells[indice + 2, 5] = fila["documento"];
            //    worksheet.Cells[indice + 2, 6] = fila["tipo_asistencia_inicial"];
            //    worksheet.Cells[indice + 2, 7] = fila["tipo_asistencia_final"];
            //    if (fila["tipo_asistencia_inicial"] == "no asistio")
            //    {
            //        contadornoasistio++;
            //    }
            //    if (fila["tipo_asistencia_inicial"] == "poder")
            //    {
            //        contadornpoder++;
            //    }
            //    if (fila["tipo_asistencia_inicial"] == "presencial")
            //    {
            //        contadorpresen++;
            //    }
            //    worksheet.Cells[2, 9] = contadorpresen;
            //    worksheet.Cells[3, 9] = contadornpoder;
            //    worksheet.Cells[4, 9] = contadornoasistio;
            //    if (fila["tipo_asistencia_final"] == "no asistio")
            //    {
            //        contadornoasistiof++;
            //    }
            //    if (fila["tipo_asistencia_final"] == "poder")
            //    {
            //        contadornpoderf++;
            //    }
            //    if (fila["tipo_asistencia_final"] == "presencial")
            //    {
            //        contadorpresenf++;
            //    }
            //    worksheet.Cells[2, 10] = contadorpresenf;
            //    worksheet.Cells[3, 10] = contadornpoderf;
            //    worksheet.Cells[4, 10] = contadornoasistiof;
            //}
        }
    }
    }

