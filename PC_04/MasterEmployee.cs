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
    public partial class MasterEmployee : CoreForm, ICrud<Employee>
    {
        public MasterEmployee()
        {
            InitializeComponent();
        }

        public Employee Data { get; set; }
        public bool IsInserting { get; set; }

        public bool IsValid()
        {
            if (Controls.IsAnyEmpty())
            {
                MessageBox.Show("Don't leave any fields empty");
                return false;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Password did not match");
                return false;
            }

            if (!txtEmail.Text.IsEmail())
            {
                MessageBox.Show("Email format is wrong");
                return false;
            }

            if (dtpDate.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Date of Birth cannot be from future");
                return false;
            }

            if (db.Employees.Select(x => x.Username).Contains(txtUsername.Text))
            {
                MessageBox.Show("That username already exist");
                return false;
            }

            return true;
        }

        public void NormalMode()
        {
            Controls.ChangeState(false);
            btnInsert.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            Controls.ClearFields();
            Data = null;
            dgv.Enabled = true;
        }

        public void SaveData()
        {
            if (!IsValid())
            {
                return;
            }

            if (IsInserting)
            {
                Data = new Employee();
            }

            Data.Username = txtUsername.Text;
            Data.Password = txtPassword.Text.ToSha256();
            Data.Name = txtName.Text;
            Data.Email = txtEmail.Text;
            Data.DateOfBirth = dtpDate.Value.Date;
            Data.JobID = int.Parse(cbJob.SelectedValue.ToString());
            Data.Address = txtAddress.Text;
            
            if (IsInserting)
            {
                db.Employees.Add(Data);
            }

            db.SaveChanges();
            LoadData();
        }

        public void SaveMode()
        {
            Controls.ChangeState(true);
            btnInsert.Enabled = false;
            btnUpdate.Enabled = false; 
            btnDelete.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            dgv.Enabled = false;
        }

        public void LoadData()
        {
            dgv.DataSource = db.Employees
                .ToArray()
                .Select(x => new
                {
                    x.ID,
                    x.Username,
                    x.Name,
                    x.Email,
                    DateOfBirth = x.DateOfBirth.ToString("dd/MM/yyyy"),
                    Job = x.Job.Name,
                    x.Address
                })
                .ToArray();

            dgv.Columns[0].Visible = false;
            NormalMode();
        }

        private void MasterRoomTypeForm_Load(object sender, EventArgs e)
        {
            cbJob.DisplayMember = "Name";
            cbJob.ValueMember = "ID";
            cbJob.DataSource = db.Jobs
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToArray();

            LoadData();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            int id = int.Parse(dgv[0, e.RowIndex].Value.ToString());
            Data = db.Employees.Find(id);
            txtUsername.Text = Data.Username;
            txtName.Text = Data.Name;
            txtEmail.Text = Data.Email;
            dtpDate.Value = Data.DateOfBirth.Date;
            cbJob.Text = Data.Job.Name;
            txtAddress.Text = Data.Address;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            IsInserting = true;
            Controls.ClearFields();
            SaveMode();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Data == null)
            {
                MessageBox.Show("Select Data first");
                return;
            }

            IsInserting = false;
            SaveMode();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Data == null)
            {
                MessageBox.Show("Select Data first");
                return;
            }

            if (MessageBox.Show("Are you sure?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                db.Employees.Remove(Data);
                db.SaveChanges();
                LoadData();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            NormalMode();
        }
    }
}
