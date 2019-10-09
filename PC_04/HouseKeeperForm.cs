using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_04
{
    public partial class HouseKeeperForm : CoreForm
    {
        private readonly Employee employee;

        public HouseKeeperForm(Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
        }

        private void btnCleaningRoom_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            var form = new CleaningRoomForm();
            form.TopLevel = false;

            panel.Controls.Add(form);
            form.Show();
        }
    }
}
