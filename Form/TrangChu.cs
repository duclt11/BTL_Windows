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
    public partial class TrangChu : Form
    {
        public TrangChu()
        {
            InitializeComponent();
            
        }

        private void TrangChu_Load(object sender, EventArgs e)
        {
            
            
        }

       
      
        private void rbtQLSach_Click(object sender, EventArgs e)
        {
            CreateNewTabControl("Quản lí sách", new SachForm());
            
        }

        private void CreateNewTabControl(string title, Form f)
        {
            tabControl1.Visible = true;
            pictureBox1.Visible = false;
            TabPage myTabPage = new TabPage(title);

            
            if (!tabControl1.TabPages.ContainsKey(myTabPage.Text))
            {

                myTabPage.Name = title;
                
                tabControl1.TabPages.Add(myTabPage);

                TextBox tb = new TextBox();
                f.TopLevel = false;
                f.Dock = DockStyle.Fill;
                f.Show();
                myTabPage.Controls.Add(f);
                
                tabControl1.SelectedTab = tabControl1.TabPages[title];
            }
            else
            {
                tabControl1.SelectedTab = tabControl1.TabPages[title];
            }




        }

        private void rbtNCC_Click(object sender, EventArgs e)
        {
            if (DangNhap.NguoiDangNhap.isAdmin == false)
            {
                MessageBox.Show("Bạn không có quyền thực hiện chức năng này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            CreateNewTabControl("Danh sách nhà cung cấp", new DanhSachNCC());
        }

        private void rbtDoiMatKhau_Click(object sender, EventArgs e)
        {
            new DoiMatKhau().ShowDialog();
        }

        private void rbtThongTin_Click(object sender, EventArgs e)
        {
            new ThongTin().Show();
        }

        private void rbtThongKe_Click(object sender, EventArgs e)
        {
            CreateNewTabControl("Thống kê", new ThongKe());
        }

        private void rbtDangXuat_Click(object sender, EventArgs e)
        {
            Hide();
            new DangNhap().ShowDialog();

            Close();

        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.DrawString("x", e.Font, Brushes.Black, e.Bounds.Right - 15, e.Bounds.Top + 4);
            e.Graphics.DrawString(this.tabControl1.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);
            e.DrawFocusRectangle();
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            Rectangle r = tabControl1.GetTabRect(this.tabControl1.SelectedIndex);
            Rectangle closeButton = new Rectangle(r.Right - 15, r.Top + 4, 9, 7);
            if (closeButton.Contains(e.Location))
            {
                this.tabControl1.TabPages.Remove(this.tabControl1.SelectedTab);
            }
        }

        private void rbtNhapKho_Click(object sender, EventArgs e)
        {
            CreateNewTabControl("Phiếu nhập", new QuanLyPhieuNhap());
        }

        private void rbtQLTK_Click(object sender, EventArgs e)
        {
            if (DangNhap.NguoiDangNhap.isAdmin == false)
            {
                MessageBox.Show("Bạn không có quyền thực hiện chức năng này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            CreateNewTabControl("Danh sach nhan vien", new QuanLiTaiKhoan());
        }
    }
}
