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
            Votacion vo = new Votacion(this.fecha, this.valor );
            vo.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
