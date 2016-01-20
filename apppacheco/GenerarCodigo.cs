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
            pictureBox1.Font = new Font("IDAutomationHC39M", 12, FontStyle.Regular);
            //label1.Font = new Font("IDAutomationHC39M", 12, FontStyle.Regular);
        } 

        private void button2_Click(object sender, EventArgs e)
        { 
            ConexionPostgres conn = new ConexionPostgres();
            var resultado= conn.consultar("SELECT * FROM modelo.unidad_residencial WHERE nit = '12345'; ");
            foreach (Dictionary<string,string> fila in resultado)
            {
                var codigo = fila["nit"]+"-" + fila["numero_unidad"];
                string barcode = codigo;
                Bitmap bitm = new Bitmap(barcode.Length * 45, 160);
                using (Graphics graphic = Graphics.FromImage(bitm))
                {
                  Font newfont = new Font("IDAutomationHC39M", 20);
                  PointF point = new PointF(2f, 2f);
                  SolidBrush black = new SolidBrush(Color.Black);
                  SolidBrush white = new SolidBrush(Color.White);
                  graphic.FillRectangle(white, 0, 0, bitm.Width, bitm.Height);
                  graphic.DrawString("*" + barcode + "*", newfont, black, point);
                }
                using (MemoryStream Mmst = new MemoryStream())
                {
                  bitm.Save("ms", ImageFormat.Jpeg);
                  pictureBox1.Image = bitm;
                  pictureBox1.Width = bitm.Width;
                  pictureBox1.Height = bitm.Height;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
