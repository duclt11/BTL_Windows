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
    public partial class ThemHoaDon : Form
    {
        private Model1 db = new Model1();
        List<NhanVien> nv;
        List<Sach> sach;
        public ThemHoaDon()
        {
            InitializeComponent();
            nv = db.NhanViens.ToList();
            sach = db.Saches.ToList();

            Show();
        }


        private void Show()
        {
            nv = db.NhanViens.ToList();
            sach = db.Saches.ToList();
            dateTimePicker1.Value = DateTime.Now;
            cbNguoiLap.DataSource = nv.ToList();
            cbNguoiLap.DisplayMember = "HoTen";
            cbNguoiLap.ValueMember = "MaNV";

            cbTenSach.DataSource = db.Saches.ToList();
            cbTenSach.DisplayMember = "TieuDe";
            cbTenSach.ValueMember = "MaSach";
            

            cbTenSach.SelectedIndex = 0;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveDonHang()
        {
            
               DonHang dh = new DonHang
            {
                NgayLapDon = dateTimePicker1.Value,
                TenKhachHang = txtKhachHang.Text,
                SDT = txtSDT.Text,
                DiaChi = txtDiaChi.Text,
                MaNV = nv.Find(e1 => e1.HoTen == cbNguoiLap.Text).MaNV,

            };
            db.DonHangs.Add(dh);
            db.SaveChanges();
            foreach (var l in ctdh)
            {
                l.MaDH = dh.MaDH;
               
            }
            db.ChiTietDonHangs.AddRange(ctdh);
            db.SaveChanges();
            MessageBox.Show("Tạo đơn hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            //DonHang dh = new DonHang();
            //ChiTietDonHang ctdh = new ChiTietDonHang();
            if(txtKhachHang.Text==string.Empty)
            {
                MessageBox.Show("Tên khách hàng không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (dgvPhieuNhap.RowCount - 1 == 0)
                {
                    MessageBox.Show("Cần ít nhất thêm 1 mặt hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    SaveDonHang();
                    Close();
                }
               
            }
           
        }
        

        private bool CheckValid()
        {
            int a;
            if (txtSL.Text != string.Empty)
            {
                if (!int.TryParse(txtSL.Text, out a)) {
                    MessageBox.Show("Vui lòng nhập đúng số lượng", "Thông báo", MessageBoxButtons.OK);
                    txtSL.Focus();

                }

                return false;
            }
            if (txtSL.Text == string.Empty)
            {
                MessageBox.Show("Không được bỏ trống số lượng", "Thông báo", MessageBoxButtons.OK);
                txtSL.Focus();
                return false;
            }
            if (txtKhachHang.Text == string.Empty)
            {
                MessageBox.Show("Không được để trống tên khách hàng", "Thông báo", MessageBoxButtons.OK);
                txtSL.Focus();
                return false;
            }
            return true;
        }
        List<ChiTietDonHang> ctdh = new List<ChiTietDonHang>();
        private void button1_Click(object sender, EventArgs e1)
        {
            dgvPhieuNhap.DataSource = null;
            dgvPhieuNhap.Rows.Clear();
            dgvPhieuNhap.Refresh();

            if (CheckValid())
            {

            }
            else
            {
                int? tongTien = 0;
                //db.DonHangs()
                Sach s = (Sach)cbTenSach.SelectedItem;
                ChiTietDonHang ct = new ChiTietDonHang { MaSach = s.MaSach, SoLuong = int.Parse(txtSL.Text), DonGia = int.Parse(txtDonGia.Text) , Sach  = (Sach)cbTenSach.SelectedItem };               
                ctdh.Add(ct);

                ctdh = ctdh.GroupBy(e2 => e2.MaSach).Select(e => new ChiTietDonHang{ MaSach = s.MaSach, SoLuong = e.Sum(d => d.SoLuong), DonGia = e.First().DonGia, Sach = e.First().Sach }).ToList();
                foreach (var l in ctdh)
                {
                    
                    dgvPhieuNhap.Rows.Add(l.MaSach, l.Sach.TieuDe, l.SoLuong, l.DonGia);
                    tongTien += l.SoLuong * l.DonGia;
                }
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK);
                label8.Text = tongTien.ToString();
                label3.Text = (dgvPhieuNhap.RowCount-1).ToString();
            }
        }
        
        private void cbTenSach_SelectedValueChanged(object sender, EventArgs e)
        {
            
            Sach s = (Sach)cbTenSach.SelectedItem;
            txtDonGia.Text = sach.Where(e1 => e1.MaSach==s.MaSach).FirstOrDefault().GiaBan.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

        }

        private void ThemHoaDon_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qLCuaHangSachDataSet.Sach' table. You can move, or remove it, as needed.
            this.sachTableAdapter.Fill(this.qLCuaHangSachDataSet.Sach);

        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            Show();
        }
    }
}
