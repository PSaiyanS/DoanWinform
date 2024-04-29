using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Project_Demo;
using ComponentFactory.Krypton.Toolkit;


namespace Project_Demo
{
    public partial class FormGiaoDien : KryptonForm
    {

        private DataTable tblH; 
      
        
        public FormGiaoDien()   
        {
            InitializeComponent();
        }


        private void frmDMHang_Load(object sender, EventArgs e)
        {    
            LoadDataGridView();        
        }

   
        private void LoadDataGridView()
        {
            Functions.Connect();
            string sql;
            sql = "SELECT * from Nuoc";
            tblH = Functions.GetDataToTable(sql);
            dvgHang.DataSource = tblH;
            dvgHang.Columns[0].HeaderText = "Mã hàng";
            dvgHang.Columns[1].HeaderText = "Tên hàng";
            dvgHang.Columns[2].HeaderText = "Số lượng";
            dvgHang.Columns[3].HeaderText = "Đơn vi";
            dvgHang.Columns[4].HeaderText = "Đơn giá";
            dvgHang.Columns[5].HeaderText = "Ghi chú";
            dvgHang.Columns[0].Width = 80;
            dvgHang.Columns[1].Width = 140;
            dvgHang.Columns[2].Width = 80;
            dvgHang.Columns[3].Width = 80;
            dvgHang.Columns[4].Width = 100;
            dvgHang.Columns[5].Width = 100;
            dvgHang.AllowUserToAddRows = false;
            dvgHang.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
        private List<GioHangItem> gioHang = new List<GioHangItem>();

        private void UpdateGioHangDataGridView()
        {
            dgvGioHang.DataSource = null;
            dgvGioHang.DataSource = gioHang;
        }
        private GioHangItem monHangDuocChon;
        private void dvgHang_SelectionChanged(object sender, EventArgs e)
        {
            if (dvgHang.SelectedRows.Count > 0)
            {
                // Lấy thông tin món hàng được chọn từ DataGridView
                int rowIndex = dvgHang.SelectedRows[0].Index;
                monHangDuocChon = new GioHangItem
                {
                    MaHang = dvgHang.Rows[rowIndex].Cells[0].Value.ToString(),
                    TenHang = dvgHang.Rows[rowIndex].Cells[1].Value.ToString(),
                    SoLuong = 1, // Mặc định số lượng là 1
                    DonGia = decimal.Parse(dvgHang.Rows[rowIndex].Cells[4].Value.ToString())
                };

                // Hiển thị thông tin món hàng được chọn lên TextBox
                txtTenHang.Text = monHangDuocChon.TenHang;

               
                txtDonVi.Text = dvgHang.Rows[rowIndex].Cells[3].Value.ToString();

            }
            else
            {
                monHangDuocChon = null;
            }
        }
        private void CapNhatTongTien()
        {
            decimal tongTien = 0;
            foreach (var item in gioHang)
            {
                decimal thanhTien = item.SoLuong * item.DonGia;
                tongTien += thanhTien;
            }

            // Hiển thị tổng tiền cần thanh toán trong txtTongTien
            txtTong.Text = tongTien.ToString("C0");
        }
        private void btnMua_Click(object sender, EventArgs e)
        {
            // Hiển thị thông tin hoá đơn (ví dụ: MessageBox)
            decimal tongTien = 0;
            StringBuilder hoaDon = new StringBuilder();
            hoaDon.AppendLine("HOÁ ĐƠN MUA HÀNG");
            hoaDon.AppendLine("Tên hàng - Số lượng - Thành tiền");

            foreach (var item in gioHang)
            {
                decimal thanhTien = item.SoLuong * item.DonGia;
                tongTien += thanhTien;

                hoaDon.AppendLine($"{item.TenHang} - {item.SoLuong} - {thanhTien:C0}");
            }

            hoaDon.AppendLine($"Tổng tiền cần thanh toán: {tongTien:C0}");

            MessageBox.Show(hoaDon.ToString());

            // Sau khi xuất hoá đơn, bạn có thể xóa giỏ hàng để chuẩn bị cho đơn hàng tiếp theo.
            gioHang.Clear();
            CapNhatTongTien();
            UpdateGioHangDataGridView();
        }
        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (monHangDuocChon != null)
            {
                if (!string.IsNullOrEmpty(txtSoLuong.Text) && int.TryParse(txtSoLuong.Text, out int soLuong) && soLuong > 0)
                {
                    // Cập nhật số lượng của món hàng được chọn
                    monHangDuocChon.SoLuong = soLuong;

                    // Thêm món hàng vào giỏ hàng
                    gioHang.Add(monHangDuocChon);

                    // Xóa thông tin số lượng sau khi thêm vào giỏ hàng
                    txtSoLuong.Text = "";

                    // Hiển thị thông tin giỏ hàng trong DataGridView dgvGioHang
                    UpdateGioHangDataGridView();
                    CapNhatTongTien();
                    // Xóa thông tin món hàng được chọn để chuẩn bị cho lần chọn món hàng tiếp theo
                    monHangDuocChon = null;
                    
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn món hàng.");
            }
        }

    }
}

