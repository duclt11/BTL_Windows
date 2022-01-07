using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_WinDow
{
    public partial class QuanLiHoaDon : Form
    {
        private Model1 db = new Model1();
        public QuanLiHoaDon()
        {
            InitializeComponent();
            Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void QuanLiHoaDon_Load(object sender, EventArgs e1)
        {
            //var res = db.DonHangs.Select(e => new { e.MaDH, e.TenKhachHang, e.SDT, e.DiaChi, e.NhanVien.HoTen,e.NgayLapDon }).ToList();
            //dataGridView1.DataSource = res;
        }
        private void Show()
        {
            var res = db.DonHangs.Select(e => new { e.MaDH, e.TenKhachHang, e.SDT, e.DiaChi, e.NhanVien.HoTen, e.NgayLapDon }).ToList();
            dataGridView1.DataSource = res;
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            Form hd = new ThemHoaDon();
            hd.Show();
            hd.ControlBox = true;
        }
    }
}
