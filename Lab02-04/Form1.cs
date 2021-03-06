using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab02_04
{
    public partial class Form1 : Form
    {
        private List<KhachHang> ls;
        public Form1()
        {
            ls = new List<KhachHang>();
            InitListItem();


            InitializeComponent();
            LoadListView();
        }


        // tạo data ảo khách hàng
        private void InitListItem()
        {
            KhachHang kh;
            Random rd = new Random();
            for (int i = 0; i < 20; i++)
            {
                kh = new KhachHang();
                kh.Stk = rd.Next(10000000, 99999999);
                kh.HoTen = i % 2 == 0 ? "Huy Vũ " + i : "Hu Vuỹ " + i;
                kh.DiaChi = i % 2 == 0 ? "Khu 2 Hoàng Cương Thanh Ba Phú Thọ " + i : "Quảng Ngãi " + i;
                kh.SoTien = Math.Round(rd.NextDouble() * 10000000, 2);
                ls.Add(kh);
            }
        }

        // làm mới danh sách khách hàng
        void LoadListView()
        {
            lsvDSKH.Clear();
            LoadHeaderListView();
            LoadListViewItem();

            lsvDSKH.View = View.Details;
            lsvDSKH.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lsvDSKH.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
  
        }

        // In item của listview
        private void LoadListViewItem()
        {
            ListViewItem item;
            int i = 1;
            double tongTien = 0;
            foreach (KhachHang kh in ls)
            {
                item = new ListViewItem(new string[] { (i++).ToString(), kh.Stk.ToString(), kh.HoTen, kh.DiaChi, kh.GetCurrencyFormat() });
                tongTien += kh.SoTien;
                lsvDSKH.Items.Add(item);
            }
            txtTongTien.Text = KhachHang.GetCurrencyFormat(tongTien);
        }

        // In header của listview
        private void LoadHeaderListView()
        {
            lsvDSKH.Columns.Add("STT");
            lsvDSKH.Columns.Add("Mã tài khoản");
            lsvDSKH.Columns.Add("Tên khách hàng");
            lsvDSKH.Columns.Add("Địa chỉ");
            lsvDSKH.Columns.Add("Số tiền");
        }

        // bắt sự kiện chọn một hàng trong danh sách khách hàng
        private void lsvDSKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;
            try
            {
                if (lv.SelectedItems.Count > 0)
                {
                    ListViewItem item = lv.SelectedItems[0];
                    long stk = long.Parse(item.SubItems[1].Text);
                    KhachHang kh = FindKhachHangByStk(stk);
                    SetKhachHangToInputField(kh);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.ToString());
            }
        }

        // tìm khách hàng bằng số tài khoản
        private KhachHang FindKhachHangByStk(long stk)
        {
            try
            {
                return ls.Where(s => s.Stk.Equals(stk)).First();
            }
            catch
            {
                throw new KhachHangNotFoundException("Không tìm thấy khách hàng!");
            }
        }

        
        // Bắt sự kiện click nút Thêm/Cập nhật
        private void btnLuu_Click(object sender, EventArgs e)
        {
            KhachHang kh;
            try
            {
                kh = GetKhachHangFromInputField();
                SaveKhachHang(kh);
            }
            catch(Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        // Lưu khách hàng
        private void SaveKhachHang(KhachHang kh)
        {
            try
            {
                KhachHang temp = FindKhachHangByStk(kh.Stk);
                temp.HoTen = kh.HoTen;
                temp.DiaChi = kh.DiaChi;
                temp.SoTien = kh.SoTien;
                MessageBox.Show("Cập nhật dữ liệu thành công!");
            }
            catch (KhachHangNotFoundException)
            {
                ls.Add(kh);
                MessageBox.Show("Thêm mới dữ liệu thành công!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                LoadListView();
            }
        }

        // in thông tin khách hàng lên field nhập liệu
        private void SetKhachHangToInputField(KhachHang kh)
        {
            try
            {
                txtStk.Text = kh.Stk.ToString();
                txtTen.Text = kh.HoTen.ToString();
                txtDiaChi.Text = kh.DiaChi.ToString();
                txtSoTien.Text = kh.SoTien.ToString();
            }
            catch
            {
                throw new Exception("Không thể xuất thông tin khách hàng!");
            }
        }
        // Lấy thông tin khách hàng từ các textbox
        private KhachHang GetKhachHangFromInputField()
        {
            errKhachHang.Clear();
            try
            {
                long stk = GetStkFromField();
                string hoTen = GetTenFromField();
                string diaChi = GetDiaChiFromField();
                double soTien = GetSoTienFromField();
                return new KhachHang(stk, hoTen, diaChi, soTien);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Lấy số tài khoản từ textbox
        private long GetStkFromField()
        {
            string s = txtStk.Text;

            if(s.Length == 0)
            {
                errKhachHang.SetError(txtStk, "Không được để trống");
                throw new FormatException("Vui lòng không để trống số tài khoản!");
            }

            string pattern = "^[0-9]+$";
            Match m = Regex.Match(s, pattern);
            if (!m.Success)
            {
                errKhachHang.SetError(txtStk ,"Số tài khoản sai định dạng");
                throw new FormatException("Sai định dạng!\n Số tài khoản phải là số.");
            }
            else
            {
                return long.Parse(s);
            }
        }

        // Lấy tên khách hàng từ textbox
        private string GetTenFromField()
        {
            string s = txtTen.Text;

            if (s.Length == 0)
            {
                errKhachHang.SetError(txtTen, "Không được để trống");
                throw new FormatException("Vui lòng không để trống tên khách hàng!");
            }
            return s;
        }
        // Lấy địa chỉ từ textbox
        private string GetDiaChiFromField()
        {
            string s = txtDiaChi.Text;

            if (s.Length == 0)
            {
                errKhachHang.SetError(txtDiaChi, "Không được để trống");
                throw new FormatException("Vui lòng không để trống địa chỉ");
            }
            return s;
        }
        
        // lấy số tiền từ textbox

        private double GetSoTienFromField()
        {
            string s = txtSoTien.Text;

            if (s.Length == 0)
            {
                errKhachHang.SetError(txtSoTien, "Không được để trống");
                throw new FormatException("Vui lòng không để trống số tiền");
            }

            string pattern = "^[0-9\\.]+$";
            Match m = Regex.Match(s, pattern);
            if (!m.Success)
            {
                errKhachHang.SetError(txtSoTien, "Số tiền sai định dạng");
                throw new FormatException("Sai định dạng!\n Số tiền phải là số.");
            }
            try
            {
                return double.Parse(s);
            }
            catch
            {
                throw new FormatException("Sai định dạng!\n Số tiền phải là số.");
            }
        }

        // Bắt sự kiện click xoá tài khoản
        private void btnXoa_Click(object sender, EventArgs e)
        {

            KhachHang kh;
            try
            {
                long stk = GetStkFromField();
                kh = FindKhachHangByStk(stk);
                DialogResult dr = MessageBox.Show("Xác nhận xoá tài khoản?", "", MessageBoxButtons.OKCancel);

                if (dr == DialogResult.Cancel) return;

                DeleteKhachHangByStk(kh.Stk);
            }
            catch(Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        // Xoá khách hàng bằng số tài khoản
        private void DeleteKhachHangByStk(long stk)
        {
            ls.RemoveAll(s => s.Stk == stk);
            
            LoadListView();
        }

        // Bắt sự kiện thoát ứng dụng
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Xác nhận thoát?", "", MessageBoxButtons.OKCancel);

            if (dr == DialogResult.Cancel) return;
            Application.Exit();
        }
    }
}
