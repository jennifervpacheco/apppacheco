using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//para el excel
using Excel;
using System.IO;

namespace apppacheco
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

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

        }

        private void button4_Click(object sender, EventArgs e)
        {
            GenerarCodigo cod = new GenerarCodigo();
            cod.Show();
        }
    }
}
