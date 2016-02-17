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
            var resultado = conn.consultar("SELECT * FROM modelo.unidad_residencial WHERE nit = '"+nit+"'; ");
            foreach (Dictionary<string, string> fila in resultado)
            {
                var codigo = fila["nit"] + "-" + fila["numero_unidad"]+"+";
                string barcode = codigo;
                Bitmap bitm = new Bitmap(barcode.Length * 25, 130);
                using (Graphics graphic = Graphics.FromImage(bitm))
                {
                     Font newfont = new Font("Bar-code 39 lesbar", 25);
                    PointF point = new PointF(10f, 50f);
                    SolidBrush black = new SolidBrush(Color.Black);
                    SolidBrush white = new SolidBrush(Color.White);
                    graphic.FillRectangle(white, 0, 0, bitm.Width, bitm.Height);
                    graphic.DrawString("*" + barcode + "*", newfont, black, point);
                    //dibujar el rectangulo del rededor
                    Pen blackpen = new Pen(Color.Black, 1);
                    Rectangle rec = new Rectangle(0,0,290,120);
                    graphic.DrawRectangle(blackpen, rec);
                    //escribir encabezado y pie de pagina
                    var cadenaSql1 = "select nombre   from modelo.propiedad_horizontal  where nit='" + nit+"' ";
                    var nombre1 = conn.consultar(cadenaSql1);
                    var nombre = conn.consultar(cadenaSql1)[0]["nombre"];
                    string drawstring = nombre;
                    Font drawfont = new Font("arial", 16);
                    SolidBrush drawbrush = new SolidBrush(Color.Black);
                    float x = 5f;
                    float y = 10f;
                    graphic.DrawString(drawstring,drawfont, drawbrush,x,y);
                    var codigo1 = fila["nombre_completo"] + "-" + fila["numero_unidad"];
                    string drawstring1 = codigo1;
                    Font drawfont1 = new Font("arial", 10);
                    SolidBrush drawbrush1 = new SolidBrush(Color.Black);
                    float x1 = 10f;
                    float y1= 95f;
                    graphic.DrawString(drawstring1, drawfont1, drawbrush1, x1, y1);
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

        private void GenerarCodigo_Resize(object sender, EventArgs e)
        {
        }
    }
}
