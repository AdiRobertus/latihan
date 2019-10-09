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
    public partial class AddHouseKeepingSchedule : CoreForm
    {
        public AddHouseKeepingSchedule()
        {
            InitializeComponent();
        }

        public bool IsValid()
        {
            if (dtpDate.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Cannot add Schedule to the past");
                return false;
            }

            int idEmployee = int.Parse(cbHousekeeper.SelectedValue.ToString());

            var detail = db.CleaningRoomDetails
                .FirstOrDefault(x =>
                    x.CleaningRoom.Date == dtpDate.Value.Date &&
                    x.CleaningRoom.EmployeeID == idEmployee &&
                    x.Room.RoomNumber == cbRoomNumber.Text
                );

            if (detail != null)
            {
                MessageBox.Show("There's already data with dame Date, Employee, and Room Number");
                return false;
            }

            return true;
        }

        public void SaveData()
        {
            if (!IsValid())
            {
                return;
            }

            int idEmployee = int.Parse(cbHousekeeper.SelectedValue.ToString());

            var cleaning = db.CleaningRooms
                .FirstOrDefault(x => x.Date == dtpDate.Value.Date && x.EmployeeID == idEmployee);

            if (cleaning == null)
            {
                cleaning = new CleaningRoom
                {
                    Date = dtpDate.Value,
                    EmployeeID = idEmployee
                };

                db.CleaningRooms.Add(cleaning);
                db.SaveChanges();
            }

            int idRoom = int.Parse(cbRoomNumber.SelectedValue.ToString());
            db.CleaningRoomDetails.Add(new CleaningRoomDetail
            {
                CleaningRoomID = cleaning.ID,
                RoomID = idRoom
            });

            db.SaveChanges();
            LoadData();
        }

        public void LoadData()
        {
            int id = int.Parse(cbHousekeeper.SelectedValue.ToString());

            dgv.Columns.Clear();
            dgv.DataSource = db.CleaningRoomDetails
                .Where(x =>
                    x.CleaningRoom.EmployeeID == id
                )
                .Select(x => new
                {
                    x.ID,
                    x.CleaningRoom.Date,
                    Housekeeper = x.CleaningRoom.Employee.Name,
                    x.Room.RoomNumber
                })
                .ToArray();

            var column = new DataGridViewButtonColumn
            {
                Name = "Remove",
                HeaderText = "Remove",
                Text = "Remove",
                UseColumnTextForButtonValue = true
            };

            dgv.Columns.Add(column);
            dgv.Columns["ID"].Visible = false;
        }

        private void MasterRoomTypeForm_Load(object sender, EventArgs e)
        {
            cbHousekeeper.ValueMember = "ID";
            cbHousekeeper.DisplayMember = "Name";
            cbHousekeeper.DataSource = db.Employees
                .Where(x => x.JobID == 4)
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToArray();

            cbRoomNumber.ValueMember = "ID";
            cbRoomNumber.DisplayMember = "RoomNumber";
            cbRoomNumber.DataSource = db.Rooms
                .Select(x => new
                {
                    x.ID,
                    x.RoomNumber
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

            if (dgv.Columns[e.ColumnIndex].HeaderText == "Remove")
            {
                int id = int.Parse(dgv["ID", e.RowIndex].Value.ToString());
                var detail = db.CleaningRoomDetails.Find(id);
                db.CleaningRoomDetails.Remove(detail);
                db.SaveChanges();
                LoadData();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cbHousekeeper_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cbRoomNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
