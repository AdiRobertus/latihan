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
    public partial class FrontOfficeForm : CoreForm
    {
        private readonly Employee employee;

        public FrontOfficeForm(Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
        }

        private void btnReservation_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            var form = new ReservationForm();
            form.TopLevel = false;

            panel.Controls.Add(form);
            form.Show();
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            var form = new ChekInForm();
            form.TopLevel = false;

            panel.Controls.Add(form);
            form.Show();
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            var form = new RequestForm();
            form.TopLevel = false;

            panel.Controls.Add(form);
            form.Show();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            var form = new CheckOutForm();
            form.TopLevel = false;

            panel.Controls.Add(form);
            form.Show();
        }

        private void btnMasterRoomType_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            var form = new MasterRoomTypeForm();
            form.TopLevel = false;

            panel.Controls.Add(form);
            form.Show();
        }

        private void btnMasterRoom_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            var form = new MasterRoom();
            form.TopLevel = false;

            panel.Controls.Add(form);
            form.Show();
        }

        private void btnMasterItem_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            var form = new MasterItem();
            form.TopLevel = false;

            panel.Controls.Add(form);
            form.Show();
        }
    }
}
