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
    public partial class Votacion : Form
    {
        public Votacion()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Votacion_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            ConexionPostgres conn = new ConexionPostgres();
            var resultado = conn.consultar("SELECT * FROM modelo.propiedad_horizontal");
            //    string valores = "";
            //    foreach (Dictionary<string,string> fila in resultado)
            //    {
            //        //valores += string.Join(",", fila.ToArray());
            //        valores += "{";
            //        foreach (KeyValuePair<string, string> columna in fila)
            //        {
            //            valores += columna.Key + ":" + columna.Value + ",";
            //        }
            //        valores += "},\n";
            //    }
            //    MessageBox.Show("El resultado de la primera consulta es: \n" + valores);
            //    var nit = "13623";
            //    var cadenaSql = " INSERT INTO modelo.propiedad_horizontal";
            //    cadenaSql += " (";
            //    cadenaSql += " nit,";
            //    cadenaSql += " nombre,";
            //    cadenaSql += " numero_propiedades";
            //    cadenaSql += " )";
            //    cadenaSql += " VALUES";
            //    cadenaSql += " (";
            //    cadenaSql += " '" + nit + "',";
            //    cadenaSql += " 'Edificio',";
            //    cadenaSql += " '200'";
            //    cadenaSql += " )";
            //    var resultado2 = conn.registrar(cadenaSql);
            //    var mensaje = (resultado2) ? "Exitoso" : "No exitoso";
            //    MessageBox.Show("El resultado del registro fue: " + mensaje);
        }
    }
}
