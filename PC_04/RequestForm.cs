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
    public partial class RequestForm : CoreForm
    {
        List<ReservationRequestItem> items = new List<ReservationRequestItem>();
        public RequestForm()
        {
            InitializeComponent();
        }

        private void RequestForm_Load(object sender, EventArgs e)
        {
            cbRoomNumber.ValueMember = "ID";
            cbRoomNumber.DisplayMember = "RoomNumber";
            cbRoomNumber.DataSource = db.ReservationRooms
                .Select(x => new
                {
                    x.ID,
                    x.Room.RoomNumber
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
        }

        void LoadItem()
        {
            var q = items.Select(x => new
            {
                x.ItemID,
                Item = x.Item.Name,
                x.Qty,
                Price = x.Item.RequestPrice
            })
            .ToArray();
            labelTotal.Text = $"Total Charge : Rp. {q.Sum(x => x.Qty * x.Price)}";

            dgv.Columns.Clear();
            dgv.DataSource = q;
            var column = new DataGridViewButtonColumn
            {
                HeaderText = "Remove",
                Name = "Remove",
                UseColumnTextForButtonValue = true,
                Text = "Remove"
            };

            dgv.Columns.Add(column);
            dgv.Columns["ItemID"].Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int itemId = int.Parse(cbItem.SelectedValue.ToString());
            var item = db.Items.Find(itemId);

            int reservationId = int.Parse(cbRoomNumber.SelectedValue.ToString());
            var reservation = db.ReservationRooms.Find(reservationId);
            var item2 = items.FirstOrDefault(x => x.ItemID == item.ID);
            if (item2 == null)
            {
                items.Add(new ReservationRequestItem
                {
                    ReservationRoomID = reservationId,
                    ReservationRoom = reservation,
                    ItemID = itemId,
                    Item = item,
                    Qty = (int)nudQty.Value,
                    TotalPrice = item.RequestPrice * (int)nudQty.Value
                });
            }
            else
            {
                int index = items.IndexOf(item2);
                items[index].Qty += (int)nudQty.Value;
                items[index].TotalPrice = item.RequestPrice * items[index].Qty;
            }
            

            LoadItem();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (dgv.Columns[e.ColumnIndex].HeaderText == "Remove")
            {
                int id = int.Parse(dgv["itemID", e.RowIndex].Value.ToString());
                items.Remove(items.FirstOrDefault(x => x.ItemID == id));
                LoadItem();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (ReservationRequestItem i in items)
            {
                var _item = db.ReservationRequestItems
                    .FirstOrDefault(x => x.ItemID == i.ItemID && x.ReservationRoomID == i.ReservationRoomID);

                if (_item == null)
                {
                    db.ReservationRequestItems.Add(i);
                }
                else
                {
                    _item.Qty += i.Qty;
                    _item.TotalPrice = _item.Qty * _item.Item.RequestPrice;
                }
            }

            db.SaveChanges();
            Reset();
        }

        void Reset()
        {
            items.Clear();
            LoadItem();
        }

        private void cbRoomNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            int reservationId = int.Parse(cbRoomNumber.SelectedValue.ToString());
            var reservation = db.ReservationRooms.Find(reservationId);

            for (int i = 0; i < items.Count; i++)
            {
                items[i].ReservationRoomID = reservationId;
                items[i].ReservationRoom = reservation;
            }
        }
    }
}
