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
    public partial class MasterItem : CoreForm, ICrud<Item>
    {
        public MasterItem()
        {
            InitializeComponent();
        }

        public Item Data { get; set; }
        public bool IsInserting { get; set; }

        public bool IsValid()
        {
            if (Controls.IsAnyEmpty(txtCompensationPrice))
            {
                MessageBox.Show("Don't leave any fields empty");
                return false;
            }

            if (!txtRequestPrice.Text.IsPositifNumber())
            {
                MessageBox.Show("Request Price must be number");
                return false;
            }

            if (txtCompensationPrice.Text.Trim() != "")
            {
                if (!txtCompensationPrice.Text.IsPositifNumber())
                {
                    MessageBox.Show("Compensation Price must be positif number or empty");
                    return false;
                }
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
                Data = new Item();
            }

            Data.Name = txtName.Text;
            Data.RequestPrice = int.Parse(txtRequestPrice.Text);
            Data.CompensationFee = txtCompensationPrice.Text.Trim() == "" ? new int?() : int.Parse(txtCompensationPrice.Text);

            if (IsInserting)
            {
                db.Items.Add(Data);
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
            dgv.DataSource = db.Items
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.RequestPrice,
                    x.CompensationFee
                })
                .ToArray();

            dgv.Columns[0].Visible = false;
            NormalMode();
        }

        private void MasterRoomTypeForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            int id = int.Parse(dgv[0, e.RowIndex].Value.ToString());
            Data = db.Items.Find(id);
            txtName.Text = Data.Name;
            txtRequestPrice.Text = Data.RequestPrice.ToString();
            txtCompensationPrice.Text = Data.CompensationFee.ToString();
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
                db.Items.Remove(Data);
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
