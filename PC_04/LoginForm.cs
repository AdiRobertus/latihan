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
    public partial class LoginForm : CoreForm
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        bool IsValid()
        {
            if (Controls.IsAnyEmpty())
            {
                MessageBox.Show("Don't leave any fields empty");
                return false;
            }

            return true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!IsValid())
            {
                return;
            }

            string password = txtPassword.Text.ToSha256();
            var employee = db.Employees
                .FirstOrDefault(x =>
                    x.Username == txtUsername.Text &&
                    x.Password == password
                );

            if (employee == null)
            {
                MessageBox.Show("User not found");
                return;
            }

            Hide();
            switch (employee.JobID)
            {
                case 1:
                    new FrontOfficeForm(employee).ShowDialog();
                    break;

                case 4:
                    new HouseKeeperForm(employee).ShowDialog();
                    break;

                case 6:
                    new SupervisorForm(employee).ShowDialog();
                    break;

                case 7:
                    new AdminForm(employee).ShowDialog();
                    break;

                default:
                    MessageBox.Show("Job Type not found");
                    break;
            }
            Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
