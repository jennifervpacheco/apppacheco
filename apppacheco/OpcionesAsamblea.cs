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

using NPOI.XSSF.UserModel;
using System.IO; // File.Exists()

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

            double porcentaje = (100 * (asistenciaCasoCoeficientes - registradosCasoUnidadesdescargue) / registradosCasoCoeficientes);
            porcentaje = Math.Round(porcentaje, 2);
            quorum = (porcentaje).ToString();

            label5.Text = quorum + "%";

            label5.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label5.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ConexionPostgres conn = new ConexionPostgres();
            //Cómo tú usaste XLSX para la lectura de archivos utilicé esta librería
            //Si quieres XlS sería otro cuento
            //Se crea un libro de trabajo (como siempre pueden ser atributos de la clase si se desea usar en varios m[etodos)
            XSSFWorkbook workbook = new XSSFWorkbook();
            //Se crea una hoja para el libro de la (hoja de cálculo)
            XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet("Sheet1"); ;
            //Creo una variable que es similar a la que se retorna en las consultas SQL

            var cadenaSql = " SELECT ur.nit, ur.numero_unidad, ur.nombre_completo, ur.coeficiente, ur.documento, CASE WHEN aur.id_tipo_asistencia_inicial=1 THEN 'presencial' WHEN aur.id_tipo_asistencia_inicial=2 THEN 'poder' WHEN aur.id_tipo_asistencia_inicial=3 THEN 'no asistio' ELSE 'no asistio' END AS tipo_asistencia_inicial, CASE WHEN aur.id_tipo_asistencia_final=1 THEN 'presencial' WHEN aur.id_tipo_asistencia_final=2 THEN 'poder' WHEN aur.id_tipo_asistencia_final=3 THEN 'no asistio' WHEN aur.id_tipo_asistencia_final=4 THEN 'se retiro antes de finalizar' ELSE 'no asistio' END AS tipo_asistencia_final FROM modelo.unidad_residencial AS ur LEFT OUTER JOIN modelo.asamblea_unidad_residencial AS aur ON ur.numero_unidad = aur.numero_unidad WHERE ur.nit='"+this.valor+"' order by ur.numero_unidad asc;";
            var resultado = conn.consultar(cadenaSql);


            //List<Dictionary<string, string>> resultado = new List<Dictionary<string, string>>();
            //resultado.Add(new Dictionary<string, string>{
            //    { "nit", "999999999" },
            //    { "nombre", "Primera asamblea de Jennifer, mucha plata" },
            //    { "fecha", "2016-01-29" },
            //    { "id", "1" }
            //});
            //resultado.Add(new Dictionary<string, string>{
            //    { "nit", "999999999" },
            //    { "nombre", "Un gran día 1234569" },
            //    { "fecha", "2069-01-29" },
            //    { "id", "2" }
            //});
            ////Se termina la creación de la variable

            //Se escriben las cabeceras del reporte, primero se crea la fila
            var primerFilaExcel = sheet.CreateRow(0);
            var segundaFilaExcel = sheet.CreateRow(1);

            //También creo una fuente NEGRILLA para ponerselas a esas celdas
            var boldFont = workbook.CreateFont();
            boldFont.FontHeightInPoints = 11;
            boldFont.FontName = "Calibri";
            boldFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            var style = workbook.CreateCellStyle();
            style.SetFont(boldFont);

            //Se crea una celda, se le pone el estilo NEGRILLA y se le pone el valor
            var cell = primerFilaExcel.CreateCell(0);
            cell.CellStyle = style;
            cell.SetCellValue("NIT");
            //OTRA CELDA
            cell = primerFilaExcel.CreateCell(1);
            cell.CellStyle = style;
            cell.SetCellValue("NUMERO UNIDAD");
            //OTRA CELDA
            cell = primerFilaExcel.CreateCell(2);
            cell.CellStyle = style;
            cell.SetCellValue("NOMBRE");
            //OTRA CELDA
            cell = primerFilaExcel.CreateCell(3);
            cell.CellStyle = style;
            cell.SetCellValue("COEFICIENTE");
            //OTRA CELDA
            cell = primerFilaExcel.CreateCell(4);
            cell.CellStyle = style;
            cell.SetCellValue("DOCUMENTO");
            //OTRA CELDA
            cell = primerFilaExcel.CreateCell(5);
            cell.CellStyle = style;
            cell.SetCellValue("ASISTENCIA INICIAL");
            //OTRA CELDA
            cell = primerFilaExcel.CreateCell(6);
            cell.CellStyle = style;
            cell.SetCellValue("ASISTENCIA FINAL");
            //OTRA CELDA
            cell = primerFilaExcel.CreateCell(7);
            cell.CellStyle = style;
            cell.SetCellValue("QUORUM INICIAL");
            //OTRA CELDA
            cell = primerFilaExcel.CreateCell(8);
            cell.CellStyle = style;
            cell.SetCellValue("QUORUM FINAL");
            
            //Se desocupa la variables para que no ocupen espacio
            cell = null;
            primerFilaExcel = null;

            int contadornoasistio = 0, contadornoasistiof = 0;
            int contadornpoder = 0, contadornpoderf = 0;
            int contadorpresen = 0, contadorpresenf = 0;

            //Se crea un for como siempre que recorre el resultado
            for (int i = 0; i < resultado.Count; i++)
            {
                Dictionary<string, string> fila = resultado[i];
                var filaExcel = sheet.CreateRow(i + 1);//La fila comienza desde la posición 1
                filaExcel.CreateCell(0).SetCellValue(fila["nit"]);
                filaExcel.CreateCell(1).SetCellValue(fila["numero_unidad"]);
                filaExcel.CreateCell(2).SetCellValue(fila["nombre_completo"]);
                filaExcel.CreateCell(3).SetCellValue(fila["coeficiente"]);
                filaExcel.CreateCell(4).SetCellValue(fila["documento"]);
                filaExcel.CreateCell(5).SetCellValue(fila["tipo_asistencia_inicial"]);
                filaExcel.CreateCell(6).SetCellValue(fila["tipo_asistencia_final"]);

                if (fila["tipo_asistencia_inicial"] == "no asistio")

                {
                    contadornoasistio++;
                }

               filaExcel.CreateCell(7).SetCellValue(contadornoasistio);
                //aqui lo muestra en toda la fila pero no se como ponerlo en una sola celda al igual que los otros
                if (fila["tipo_asistencia_inicial"] == "poder")
                {
                    contadornpoder++;
                }
                if (fila["tipo_asistencia_inicial"] == "presencial")
                {
                    contadorpresen++;
                }
                

                if (fila["tipo_asistencia_final"] == "no asistio")
                {
                    contadornoasistiof++;
                }
                if (fila["tipo_asistencia_final"] == "poder")
                {
                    contadornpoderf++;
                }
                if (fila["tipo_asistencia_final"] == "presencial")
                {
                    contadorpresenf++;
                }

            }
           
            using (var fs = new FileStream("lista_asistencia"+this.valor+".xlsx", FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
                fs.Close();
                //borrar anuncio cuando ya no sea necesario
                MessageBox.Show("El archivo se guardó en la ruta: " + fs.Name);

            }
        }
    }
    }

