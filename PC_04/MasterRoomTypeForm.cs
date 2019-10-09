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
    public partial class MasterRoomTypeForm : CoreForm, ICrud<RoomType>
    {
        public MasterRoomTypeForm()
        {
            InitializeComponent();
        }

        public RoomType Data { get; set; }
        public bool IsInserting { get; set; }

        public bool IsValid()
        {
            if (Controls.IsAnyEmpty())
            {
                MessageBox.Show("Don't leave any fields empty");
                return false;
            }

            if (!txtRoomPrice.Text.IsPositifNumber())
            {
                MessageBox.Show("Room Price must be positif number");
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
        }

        public void SaveData()
        {
            if (!IsValid())
            {
                return;
            }

            if (IsInserting)
            {
                Data = new RoomType();
            }

            Data.Name = txtName.Text;
            Data.Capacity = (int)nudCapacity.Value;
            Data.RoomPrice = int.Parse(txtRoomPrice.Text);

            if (IsInserting)
            {
                db.RoomTypes.Add(Data);
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
        }

        public void LoadData()
        {
            dgv.DataSource = db.RoomTypes
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.Capacity,
                    Price = x.RoomPrice
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
            Data = db.RoomTypes.Find(id);
            txtName.Text = Data.Name;
            nudCapacity.Value = Data.Capacity;
            txtRoomPrice.Text = Data.RoomPrice.ToString();
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

            if (db.Rooms.Count(x => x.RoomTypeID == Data.ID) != 0)
            {
                MessageBox.Show("Please delete or update data in Room with this RoomType first");
                return;
            }

            if (MessageBox.Show("Are you sure?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                db.RoomTypes.Remove(Data);
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
