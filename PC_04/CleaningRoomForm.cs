using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PC_04
{
    public partial class CleaningRoomForm : CoreForm
    {
        public CleaningRoomForm()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var excel = new Excel.Application();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

        }
    }
}
