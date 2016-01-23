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
    public partial class Pregunta : Form
    {
        public Pregunta()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Votacion mmformulario = new Votacion();
            mmformulario.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Registro miformilario = new Registro();
            miformilario.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RegistroFinal mformulario = new RegistroFinal();
            mformulario.Show();
        }

        private void OpcionesAsamblea_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            ConexionPostgres conn = new ConexionPostgres();
        }
    }
}
