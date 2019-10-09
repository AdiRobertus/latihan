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
    public partial class ReservationForm : CoreForm
    {
        List<Room> selectedRooms = new List<Room>();
        List<Customer> customers = new List<Customer>();
        List<ReservationRequestItem> items = new List<ReservationRequestItem>();
        Customer selectedCustomer;
        Room room1;
        Room room2;

        public ReservationForm()
        {
            InitializeComponent();
        }

        void LoadData()
        {
            dgvAvailable.DataSource = db.Rooms
                .ToArray()
                .Where(x =>
                    !selectedRooms.Select(y => y.ID).Contains(x.ID)
                )
                .Select(x => new
                {
                    x.ID,
                    x.RoomNumber,
                    x.RoomFloor,
                    x.Description
                })
                .ToArray();

            dgvAvailable.Columns["ID"].Visible = false;

            dgvSelected.DataSource = selectedRooms
                .Select(x => new
                {
                    x.ID,
                    x.RoomNumber,
                    x.RoomFloor,
                    x.Description
                })
                .ToArray();

            dgvSelected.Columns["ID"].Visible = false;
        }

        private void ReservationForm_Load(object sender, EventArgs e)
        {
            cbRoomType.ValueMember = "ID";
            cbRoomType.DisplayMember = "Name";
            cbRoomType.DataSource = db.RoomTypes
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToArray();

            cbItem.ValueMember = "ID";
            cbItem.DisplayMember = "Name";
            cbItem.DataSource = db.Items
                .Select(x => new
                {
                    x.ID,
                    x.Name
                })
                .ToArray();

            customers = db.Customers.ToList();
            LoadData();
            LoadCustomerData();
        }

        void LoadCustomerData()
        {
            dgvSearch.DataSource = customers.Select(x => new
            {
                x.ID,
                Choose = selectedCustomer == null ? false : x.ID == selectedCustomer.ID,
                x.Name,
                x.Email,
                x.Gender,
                x.PhoneNumber,
                x.Age
            })
            .ToArray();

            dgvSearch.Columns["ID"].Visible = false;
        }

        private void tpSearch_Click(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        private void tpAddNew_Click(object sender, EventArgs e)
        {
            tpAddNew.Controls.ClearFields();
        }

        private void btnToSelected_Click(object sender, EventArgs e)
        {
            if (room1 == null)
            {
                return;
            }

            selectedRooms.Add(room1);
            room1 = null;
            LoadData();
            SetTotal();
        }

        private void btnToAvailable_Click(object sender, EventArgs e)
        {
            if (room2 == null)
            {
                return;
            }

            selectedRooms.Remove(room2);
            room2 = null;
            LoadData();
            SetTotal();
        }

        private void dgvAvailable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            int roomId = int.Parse(dgvAvailable["ID", e.RowIndex].Value.ToString());
            room1 = db.Rooms.Find(roomId);
        }

        private void dgvSelected_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            int roomId = int.Parse(dgvSelected["ID", e.RowIndex].Value.ToString());
            room2 = selectedRooms.FirstOrDefault(x => x.ID == roomId);
            selectedRooms.Remove(selectedRooms.FirstOrDefault(x => x.ID == roomId));
        }

        private void dgvSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (dgvSearch.Columns[e.ColumnIndex].HeaderText == "Choose")
            {
                int customerId = int.Parse(dgvSearch["ID", e.RowIndex].Value.ToString());
                selectedCustomer = customers.FirstOrDefault(x => x.ID == customerId);
                LoadCustomerData();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int itemId = int.Parse(cbItem.SelectedValue.ToString());
            var item = db.Items.Find(itemId);
            items.Add(new ReservationRequestItem
            {
                ItemID = item.ID,
                Qty = (int)nudQty.Value,
                Item = item
            });

            dgvRequest.DataSource = items.Select(x => new
            {
                x.ID,
                x.Item.Name,
                x.Qty
            })
            .ToArray();
            dgvRequest.Columns["ID"].Visible = false;
            
            SetTotal();
        }

        void SetTotal()
        {
            int night = int.Parse(txtNumberOfNights.Text);
            int roomSum = selectedRooms.Sum(x => x.RoomType.RoomPrice * night);
            int itemSum = items.Sum(x => x.Item.RequestPrice * x.Qty);

            labelTotal.Text = $"Total Price: Rp. {roomSum + itemSum}";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //var reservation = new Reservation();
            //reservation.DateTime = DateTime.Now;
            //reservation.EmployeeID = ;
            //reservation.CustomerID = ;
            //reservation.Code = "";

            //db.Reservations.Add(reservation);
            //db.SaveChanges();

            //int night = int.Parse(txtNumberOfNights.Text);
            //var reservationRooms = new List<ReservationRoom>();
            //foreach (var room in selectedRooms)
            //{
            //    reservationRooms.Add(new ReservationRoom
            //    {
            //        ReservationID = reservation.ID,
            //        RoomID = room.ID,
            //        StartDateTime = dtpStartDate.Value.Date,
            //        DurationNights = night,
            //        RoomPrice = 
            //    });
            //}
        }
    }
}
