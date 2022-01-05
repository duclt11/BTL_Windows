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
    public partial class CapNhatChiTietPhieuNhap : Form
    {
        Model1 db = new Model1();
        public CapNhatChiTietPhieuNhap(int? id)
        {
            InitializeComponent();
            var phieuNhap = db.PhieuNhaps.Find(id);
            lbMaPhieu.Text = phieuNhap.SoPN + "";
            lbNCC.Text = phieuNhap.XuongNhap.MaXN + "";
            lbThoiGianLap.Text = phieuNhap.NgayNhap.ToString();
            if (string.IsNullOrEmpty(phieuNhap.NhanVien.HoTen))
            {
                lbNguoiLap.Text = phieuNhap.NhanVien.TenDangNhap;
            }
            lbNguoiLap.Text = phieuNhap.NhanVien.HoTen;
        }
        private void LoadData(List<ChiTietPhieuNhap> CTPN)
        {
            dgvChiTiet.DataSource = null;
            dgvChiTiet.Rows.Clear();
            dgvChiTiet.Refresh();
            var list = new List<ChiTietPhieuNhap>();
            foreach (var c in CTPN)
            {
                if (c.SoPN == int.Parse(lbMaPhieu.Text))
                {
                    list.Add(c);
                }
            }
            foreach (var l in list)
            {

                dgvChiTiet.Rows.Add(l.Sach.TieuDe, l.SoLuong, l.DonGia);
            }
            lbSL.Text = (dgvChiTiet.Rows.Count - 1) + "";
        }
        private void Clear()
        {
            CBBSach.SelectedIndex = 0;
            txtDonGia.Text = "";
            txtSoLuong.Text = "";
        }
        private void CapNhatChiTietPhieuNhap_Load(object sender, EventArgs e)
        {
            var c = db.ChiTietPhieuNhaps.ToList();
            LoadData(c);
            var sachs = db.Saches.ToList();
            CBBSach.DataSource = sachs;
            CBBSach.DisplayMember = "TieuDe";
            CBBSach.ValueMember = "MaSach";
            foreach (DataGridViewColumn col in dgvChiTiet.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("Microsoft San Serif", 8, FontStyle.Bold);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.Rows.Count <= 1)
            {
                MessageBox.Show(this, "Vui lòng nhập ít nhất 1 bản ghi!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.Hide();
            new QuanLyPhieuNhap().ShowDialog();
            this.Close();
        }

        private void btnXoaTrang_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnThemChiTiet_Click(object sender, EventArgs e)
        {
            try
            {
                ChiTietPhieuNhap ct = new ChiTietPhieuNhap();
                if (CBBSach.SelectedValue == null)
                {
                    MessageBox.Show(this, "Vui lòng chọn giá trị đúng trong combobox!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                ct.MaSach = int.Parse(CBBSach.SelectedValue.ToString());
                try
                {
                    if (string.IsNullOrEmpty(txtDonGia.Text) || string.IsNullOrEmpty(txtSoLuong.Text))
                    {
                        MessageBox.Show(this, "Vui lòng nhập đủ số lượng và đơn giá!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ct.SoPN = int.Parse(lbMaPhieu.Text);
                    ct.DonGia = int.Parse(txtDonGia.Text);
                    ct.SoLuong = int.Parse(txtSoLuong.Text);
                }
                catch
                {
                    MessageBox.Show(this, "Vui lòng nhập đúng định dạng số", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (db.ChiTietPhieuNhaps.Find(ct.MaSach, ct.SoPN) != null)
                {
                    MessageBox.Show(this, "Dữ liệu đã tồn tại, không thể thêm!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                db.ChiTietPhieuNhaps.Add(ct);
                db.Saches.Find(ct.MaSach).SoLuongCo = db.Saches.Find(ct.MaSach).SoLuongCo + ct.SoLuong;
                db.SaveChanges();
                MessageBox.Show(this, "Thêm thành công chi tiết phiếu nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var cts = db.ChiTietPhieuNhaps.ToList();
                LoadData(cts);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Có lỗi " + ex.Message + " xảy ra trong quá trình thực hiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void dgvChiTiet_SelectionChanged(object sender, EventArgs e)
        {
            txtSoLuong.Text = dgvChiTiet.CurrentRow.Cells["SoLuong"].FormattedValue.ToString();
            var td = dgvChiTiet.CurrentRow.Cells["TieuDe"].FormattedValue.ToString();
            var sach = db.Saches.FirstOrDefault(s => s.TieuDe == td);
            CBBSach.SelectedItem = sach;
            txtDonGia.Text = dgvChiTiet.CurrentRow.Cells["DonGia"].FormattedValue.ToString();
        }

        private void btnXoaChiTet_Click(object sender, EventArgs e)
        {
            if (DangNhap.NguoiDangNhap.isAdmin == false)
            {
                MessageBox.Show(this, "Bạn không phải admin nên không có quyền thực hiện chức năng này!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var rows = dgvChiTiet.SelectedRows;
                if (rows.Count > 0)
                {

                    int soPN = int.Parse(lbMaPhieu.Text);
                    string tieude = Convert.ToString(rows[0].Cells["TieuDe"].Value);
                    var sach = db.Saches.FirstOrDefault(s => s.TieuDe == tieude);
                    if (sach == null)
                    {
                        MessageBox.Show(this, "Vui lòng chọn bản ghi muốn xóa hợp lệ!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    int maSach = sach.MaSach;
                    if (MessageBox.Show(this, "Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (dgvChiTiet.Rows.Count <= 2)
                        {
                            if (MessageBox.Show(this, "Đây là bản ghi cuối cùng nếu xóa sẽ đồng thời xóa đi phiếu nhập này, bạn vẫn muốn tiếp" +
                                "tục chứ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                var chitiet = db.ChiTietPhieuNhaps.FirstOrDefault(c => c.SoPN == soPN && c.MaSach == maSach);
                                db.ChiTietPhieuNhaps.Remove(chitiet);
                                db.PhieuNhaps.Remove(db.PhieuNhaps.Find(int.Parse(lbMaPhieu.Text)));
                                db.Saches.Find(maSach).SoLuongCo = db.Saches.Find(maSach).SoLuongCo - chitiet.SoLuong;
                                db.SaveChanges();
                                this.Hide();
                                new QuanLyPhieuNhap().ShowDialog();
                                this.Close();
                                
                            }
                            else
                            {
                                return;
                            }
                        }
                        var chitiet2 = db.ChiTietPhieuNhaps.FirstOrDefault(c => c.SoPN == soPN && c.MaSach == maSach);
                        db.ChiTietPhieuNhaps.Remove(db.ChiTietPhieuNhaps.FirstOrDefault(c => c.SoPN == soPN && c.MaSach == maSach));
                        db.Saches.Find(maSach).SoLuongCo = db.Saches.Find(maSach).SoLuongCo - chitiet2.SoLuong;
                        db.SaveChanges();
                        MessageBox.Show(this, "Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        var ct = db.ChiTietPhieuNhaps.ToList();
                        LoadData(ct);
                    }
                        
                    

                }
                else
                {
                    MessageBox.Show("Vui lòng chọn dòng muốn xóa!", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Có lỗi " + ex.Message + " xảy ra trong quá trình thực hiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnSuaChiTiet_Click(object sender, EventArgs e)
        {
            try
            {
                ChiTietPhieuNhap ct = new ChiTietPhieuNhap();
                if (CBBSach.SelectedValue == null)
                {
                    MessageBox.Show(this, "Vui lòng chọn giá trị đúng trong combobox!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                ct.MaSach = int.Parse(CBBSach.SelectedValue.ToString());
                try
                {
                    if (string.IsNullOrEmpty(txtDonGia.Text) || string.IsNullOrEmpty(txtSoLuong.Text))
                    {
                        MessageBox.Show(this, "Vui lòng nhập đủ số lượng và đơn giá!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ct.SoPN = int.Parse(lbMaPhieu.Text);
                    ct.DonGia = int.Parse(txtDonGia.Text);
                    ct.SoLuong = int.Parse(txtSoLuong.Text);
                }
                catch
                {
                    MessageBox.Show(this, "Vui lòng nhập đúng định dạng số", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (db.ChiTietPhieuNhaps.Find(ct.MaSach, ct.SoPN) == null)
                {
                    MessageBox.Show(this, "Dữ liệu chưa tồn tại, không thể sửa!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int? soluong = db.ChiTietPhieuNhaps.Find(ct.MaSach, ct.SoPN).SoLuong;
                if (soluong <= int.Parse(txtSoLuong.Text))
                {
                    db.Saches.Find(ct.MaSach).SoLuongCo = db.Saches.Find(ct.MaSach).SoLuongCo +(int.Parse(txtSoLuong.Text)-soluong);
                }
                else
                {
                    db.Saches.Find(ct.MaSach).SoLuongCo = db.Saches.Find(ct.MaSach).SoLuongCo - (soluong - int.Parse(txtSoLuong.Text));
                }
                db.ChiTietPhieuNhaps.Find(ct.MaSach, ct.SoPN).DonGia = int.Parse(txtDonGia.Text);
                db.ChiTietPhieuNhaps.Find(ct.MaSach, ct.SoPN).SoLuong = int.Parse(txtSoLuong.Text);
                db.SaveChanges();
                MessageBox.Show(this, "Sửa thành công chi tiết phiếu nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var cts = db.ChiTietPhieuNhaps.ToList();
                LoadData(cts);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Có lỗi " + ex.Message + " xảy ra trong quá trình thực hiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
