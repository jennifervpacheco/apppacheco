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
using Excel;
using System.IO;
//Queda vetado el uso de Interop!!!
//using Excel = Microsoft.Office.Interop.Excel;
//Alternativas:
//http://www.codeproject.com/Articles/33850/Generate-Excel-files-without-using-Microsoft-Excel
//

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

        private void button2_Click(object sender, EventArgs e)
        {
            string ruta = this.buscarDoc();
            ConexionPostgres conn = new ConexionPostgres();
            var stream = File.Open(ruta, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet result = excelReader.AsDataSet();
            //excelReader.IsFirstRowAsColumnNames = true;
            //DataSet columnnames = excelReader.AsDataSet();
            foreach (DataRow row in result.Tables[0].Rows.Cast<DataRow>().Skip(1))
            {
                var cadenaSql = "INSERT INTO modelo.unidad_residencial(nit, numero_unidad, nombre_completo, coeficiente, documento) values ('" + row.ItemArray[0] + "','" + row.ItemArray[1] + "','" + row.ItemArray[2] + "','" + row.ItemArray[3] + "','" + row.ItemArray[4] + "');";
                conn.registrar(cadenaSql);
            }
            excelReader.Close();
            MessageBox.Show("Datos cargados correctamente");
        }

        private string buscarDoc()
        {
            string file = "";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                file = openFileDialog1.FileName;
            }
            return file ;
         }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
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
    }
}
