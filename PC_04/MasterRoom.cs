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
    public partial class MasterRoom : CoreForm, ICrud<Room>
    {
        public MasterRoom()
        {
            InitializeComponent();
        }

        public Room Data { get; set; }
        public bool IsInserting { get; set; }

        public bool IsValid()
        {
            if (Controls.IsAnyEmpty(txtDescription))
            {
                MessageBox.Show("Don't leave any fields empty");
                return false;
            }

            if (!txtRoomNumber.Text.IsNumber())
            {
                MessageBox.Show("Room Number must  be number");
                return false;
            }

            if (!txtRoomFloor.Text.IsNumber())
            {
                MessageBox.Show("Room Floor must be number");
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
                Data = new Room();
            }

            Data.RoomNumber = txtRoomNumber.Text;
            Data.RoomTypeID = int.Parse(cbRoomType.SelectedValue.ToString());
            Data.RoomFloor = txtRoomFloor.Text;
            Data.Description = txtDescription.Text == "" ? null : txtDescription.Text;
            
            if (IsInserting)
            {
                db.Rooms.Add(Data);
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
            dgv.DataSource = db.Rooms
                .Select(x => new
                {
                    x.ID,
                    x.RoomNumber,
                    RoomType = x.RoomType.Name,
                    x.RoomFloor,
                    x.Description
                })
                .ToArray();

            dgv.Columns[0].Visible = false;
            NormalMode();
        }

        private void MasterRoomTypeForm_Load(object sender, EventArgs e)
        {
            cbRoomType.DisplayMember = "Name";
            cbRoomType.ValueMember = "ID";
            cbRoomType.DataSource = db.RoomTypes
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

            try
            {
                int id = int.Parse(dgv[0, e.RowIndex].Value.ToString());
                Data = db.Rooms.Find(id);
                txtRoomNumber.Text = Data.RoomNumber;
                cbRoomType.Text = Data.RoomType.Name;
                txtRoomFloor.Text = Data.RoomFloor;
                txtDescription.Text = Data.Description;
            }
            catch
            {
                
            }
            
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
                db.Rooms.Remove(Data);
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
