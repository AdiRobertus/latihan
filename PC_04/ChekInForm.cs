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
    public partial class ChekInForm : CoreForm
    {
        public ChekInForm()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgv.DataSource = db.ReservationRooms
                .Where(x => x.Reservation.Code == txtBookingCode.Text)
                .ToArray()
                .Select(x => new
                {
                    x.Room.RoomNumber,
                    x.Room.RoomFloor,
                    RoomType = x.Room.RoomType.Name,
                    StartDate = x.StartDateTime.ToString("dd MMMM yyyy"),
                    x.DurationNights
                })
                .ToArray();
        }

        bool IsValid()
        {
            if (gbCustomer.Controls.IsAnyEmpty())
            {
                MessageBox.Show("Don't leave any fields empty");
                return false;
            }

            return true;
        }

        private void btnCheckin_Click(object sender, EventArgs e)
        {
            if (!IsValid())
            {
                return;
            }

            //var q = db.ReservationRooms
            //    .Where(x => x.Reservation.Code == txtBookingCode.Text)
            //    .ToArray();

            //for (int i = 0; i < q.Length; i++)
            //{
            //    q[i].CheckInDateTime = DateTime.Now;
            //}

            //db.SaveChanges();
        }
    }
}
