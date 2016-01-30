using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel2 = Microsoft.Office.Interop.Excel;

namespace apppacheco
{
    public partial class docs : Form
    {
        public docs()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConexionPostgres conn = new ConexionPostgres();
            string nit = "999999999";
            //var cadenaSql = "SELECT * FROM modelo.unidad_residencial where nit='"+nit+"';";
            var cadenaSql = "SELECT modelo.unidad_residencial.nit, modelo.unidad_residencial.numero_unidad, modelo.unidad_residencial.nombre_completo, modelo.unidad_residencial.coeficiente,modelo.unidad_residencial.documento,  modelo.asamblea_unidad_residencial.id_tipo_asistencia_inicial, modelo.asamblea_unidad_residencial.id_tipo_asistencia_final FROM modelo.unidad_residencial LEFT OUTER JOIN modelo.asamblea_unidad_residencial ON modelo.unidad_residencial.numero_unidad = modelo.asamblea_unidad_residencial.numero_unidad WHERE modelo.unidad_residencial.nit='" + nit + "'  order by modelo.unidad_residencial.numero_unidad asc;";
            var resultado = conn.consultar(cadenaSql);

            Excel2.Application excelApp = new Excel2.Application();
            Excel2.Workbook workbook = null;
            Excel2.Workbooks workbooks = null;
            Excel2._Worksheet worksheet = null;
            workbooks = excelApp.Workbooks;
            workbook = workbooks.Add(1);
            worksheet = (Excel2.Worksheet)workbook.Sheets[1];
            excelApp.Visible = true;
            for (int indice = 0; indice < resultado.ToArray().Length; indice++)
            {
                worksheet.Cells[1, 1] = "";

                Dictionary<string, string> fila = resultado.ToArray()[indice];
                worksheet.Cells[indice + 2, 1] = fila["nit"];
                worksheet.Cells[indice + 2, 2] = fila["numero_unidad"];
                worksheet.Cells[indice + 2, 3] = fila["nombre_completo"];
                worksheet.Cells[indice + 2, 4] = fila["coeficiente"];
                worksheet.Cells[indice + 2, 5] = fila["documento"];
                worksheet.Cells[indice + 2, 6] = fila["id_tipo_asistencia_inicial"];
                worksheet.Cells[indice + 2, 7] = fila["id_tipo_asistencia_final"];


            }



        }
    }
}
