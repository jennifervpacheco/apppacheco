using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;


namespace apppacheco
{
    public partial class GenerarCodigo : Form
    {
        public GenerarCodigo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GenerarCodigo_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            ConexionPostgres conn = new ConexionPostgres();
            var resultado = conn.consultar("SELECT * FROM modelo.propiedad_horizontal; ");
            List<Select> sl = new List<Select>();
            foreach (Dictionary<string, string> fila in resultado)
            {
                int numVal = Int32.Parse(fila["nit"]);
                // tipoasambleabix.Items.Add(new ListItem ( fila["nombre"], numVal));
                sl.Add(new Select() { Text = fila["nit"] + "- " + fila["nombre"], Value = fila["nit"] });
            }
            comboBox1.DataSource = sl;
            comboBox1.DisplayMember = "Text";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ESTE PROCESO TARDARA UNOS MINUTOS, ESPERE MIENTRAS SE GENERA EL ARCHIVO, SALDRA LA CONFIRMACION DEL MISMO");
            ConexionPostgres conn = new ConexionPostgres();
            Select sl1 = comboBox1.SelectedItem as Select;
            string nit = sl1.Value;

            //string nit = Nit.Text;
            var resultado = conn.consultar("SELECT * FROM modelo.unidad_residencial WHERE nit = '"+nit+"'; ");
            foreach (Dictionary<string, string> fila in resultado)
            {
                var codigo = fila["nit"] + "-" + fila["numero_unidad"]+"+";
                string barcode = codigo;
                //Bitmap bitm = new Bitmap(barcode.Length * 30, 90);
                Bitmap bitm = new Bitmap(barcode.Length * 30, 100);
                using (Graphics graphic = Graphics.FromImage(bitm))
                {
                    Font newfont = new Font("c39hrp48dhtt", 55);
                    PointF point = new PointF(2f, 2f);
                    SolidBrush black = new SolidBrush(Color.Black);
                    SolidBrush white = new SolidBrush(Color.White);
                    graphic.FillRectangle(white, 0, 0, bitm.Width, bitm.Height);
                    graphic.DrawString("*" + barcode + "*", newfont, black, point);
                }
                using (MemoryStream Mmst = new MemoryStream())
                {
                    string nombreImagen = "ImagenesBarras\\"+ fila["nit"] + "-" + fila["numero_unidad"] + ".png";
                    bitm.Save(nombreImagen, ImageFormat.Jpeg);
                 }
            }
            string[] fotos = new string[resultado.ToArray().Length];
            for (int indice = 0; indice < resultado.ToArray().Length; indice++)
            {
                Dictionary<string, string> fila = resultado.ToArray()[indice];
                fotos[indice] = "ImagenesBarras\\" + fila["nit"] + "-" + fila["numero_unidad"] + ".png";
            }
            Pdf documentoPdf = new Pdf();
            string nuevo = documentoPdf.crearPdf(fotos);
            MessageBox.Show("El documento ha sido creado en la ruta: "+nuevo);
            }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Nit_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
