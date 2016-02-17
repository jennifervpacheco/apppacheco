using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//para leer el excel https://github.com/ExcelDataReader/ExcelDataReader
//using Excel;
using System.IO;
//Queda vetado el uso de Interop!!!
//using Excel = Microsoft.Office.Interop.Excel;
//Alternativas:
//http://www.codeproject.com/Articles/33850/Generate-Excel-files-without-using-Microsoft-Excel
//
using NPOI.XSSF.UserModel;
using System.IO; // File.Exists()

namespace apppacheco
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            IniciarAsamblea miformulario = new IniciarAsamblea();
            miformulario.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
         private string buscarDoc()
        {
            string file = "";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                file = openFileDialog1.FileName;
            }
            return file;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string ruta = this.buscarDoc();
            ConexionPostgres conn = new ConexionPostgres();
            FileStream fs;
            try
            {
                fs = new FileStream(ruta, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                MessageBox.Show("El archivo está siendo usado por otro programa. Asegurese de haber seleccionado el archivo correcto.");
                return;
            }
            XSSFWorkbook wb = new XSSFWorkbook(fs);
            string nombreHoja = "";
            for (int i = 0; i < wb.Count; i++)
            {
                nombreHoja = wb.GetSheetAt(i).SheetName;
            }
            XSSFSheet sheet = (XSSFSheet)wb.GetSheet(nombreHoja);
            try
            {
                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                    {
                        var fila = sheet.GetRow(row);
                        var nit = fila.GetCell(0).ToString();
                        var numero_unidad = fila.GetCell(1).ToString();
                        var nombre_completo = fila.GetCell(2).ToString();
                        var coeficiente = fila.GetCell(3).ToString().Replace(",", ".");
                        var documento = fila.GetCell(4).ToString();
                        var cadenaSql = string.Format(
                            @"INSERT INTO modelo.unidad_residencial
                            (
                            nit,
                            numero_unidad,
                            nombre_completo,
                            coeficiente,
                            documento
                            )
                            VALUES
                            (
                            '{0}',
                            '{1}',
                            '{2}',
                            '{3}',
                            '{4}'
                            );",
                                nit,
                                numero_unidad,
                                nombre_completo,
                                coeficiente,
                                documento
                            );
                        var resultado = conn.registrar(cadenaSql);
                        if (!resultado)//Falló la consulta
                        {
                            MessageBox.Show("El registro NIT:" + nit
                                + ", Número Unidad:" + numero_unidad
                                + ", Nombre Completo:" + nombre_completo
                                + ", Coeficiente:" + coeficiente
                                + ", Número Documento:" + documento
                                + " ,no pudo ser completado de manera exitosa, revise los datos (están mal o ya existen o se encontro elemento en blanco).");
                            MessageBox.Show(cadenaSql);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("realizado");
                    }

                }
                MessageBox.Show("Datos cargados correctamente.");
            }
            catch
            {
                MessageBox.Show("Algo pasó con la carga de archivos, asegúrese de que ha cargado el archivo correcto con datos válidos.");
            }
            //Se cierra el archivo
            fs.Close();
        }
 
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.cargarComboBoxNitPropiedad();

        }
        private void button4_Click(object sender, EventArgs e)
        {
            GenerarCodigo cod = new GenerarCodigo();
            cod.Show();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Select sl = comboBox1.SelectedItem as Select;
            string valor = sl.Value;
            string fecha = dtp.Value.Date.Year + "-" + dtp.Value.Date.Month + "-" + dtp.Value.Date.Day;
            ConexionPostgres conn = new ConexionPostgres();
            var cadenaSql = "select count(*)  from modelo.asamblea  where nit='"+ valor+ "' AND fecha='"+fecha+"';";
            var cuenta = conn.consultar(cadenaSql)[0]["count"];
            if (cuenta== "1")
            {
                var cadenaSql1 = "select nombre   from modelo.asamblea  where nit='" + valor + "' AND fecha='"+fecha+"';";
                var nombre = conn.consultar(cadenaSql1)[0]["nombre"]; 
                MessageBox.Show("ESTA INGRESANDO A: "+nombre);
                OpcionesAsamblea op = new OpcionesAsamblea(fecha, valor);
                op.Show();
            }
         }
        private void button6_Click(object sender, EventArgs e)
        {
            ConfiguracionServidorBaseDatos ser = new ConfiguracionServidorBaseDatos();
            ser.Show();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.cargarComboBoxNitPropiedad();
        }
        private void cargarComboBoxNitPropiedad()
        {
            ConexionPostgres conn = new ConexionPostgres();
            var resultado = conn.consultar("SELECT * FROM modelo.asamblea; ");
            List<Select> sl = new List<Select>();
            foreach (Dictionary<string, string> fila in resultado)
            {
                int numVal = Int32.Parse(fila["nit"]);
                // tipoasambleabix.Items.Add(new ListItem ( fila["nombre"], numVal));
                sl.Add(new Select() { Text = fila["nit"] + "-" + fila["nombre"]+"-"+fila["fecha"], Value = fila["nit"] });
            }
            comboBox1.DataSource = sl;
            comboBox1.DisplayMember = "Text";
            //http://stackoverflow.com/questions/3063320/combobox-adding-text-and-value-to-an-item-no-binding-source 
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void button8_Click(object sender, EventArgs e)
        {
            ConexionPostgres conn1 = new ConexionPostgres();
            var cadenaSql = string.Format("SELECT * FROM modelo.asamblea; ");
            var resultado = conn1.registrar(cadenaSql);
            if (resultado)
            {
                MessageBox.Show("verificada la conexion");
            }
        }
    }
}
