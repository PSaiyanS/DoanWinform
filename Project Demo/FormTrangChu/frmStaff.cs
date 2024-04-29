using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace Project_Demo
{
    public partial class frmStaff : KryptonForm
    {
        public frmStaff()
        {
            InitializeComponent();
        }
        //Khai báo các biến toàn cục
        SqlConnection con;//Khai báo đối tượng thực hiện kết nối đến cơ sở dữ liệu
        SqlCommand cmd;//Khai báo đối tượng thực hiện các câu lệnh truy vấn
        SqlDataAdapter dap;//Khai báo đối tượng gắn kết DataSource với DataSet
        DataSet ds;//Đối tượng chứa dữ liệu tại local
        private void Form4_Load(object sender, EventArgs e)
        {
            //Tạo đối tượng Connection
            con = new SqlConnection();
            //Truyền vào chuỗi kết nối tới cơ sở dữ liệu
            //Gọi Application.StartupPath để lấy đường dẫn tới thư mục chứa file chạy chương trình 
            con.ConnectionString = @"Data Source=PSAIYANS\SQLEXPRESS;Initial Catalog=CuaHangNuoc;Integrated Security=True";
            LoadDuLieu("Select * from Nuoc");
            //Khi Form mới Load lên thì ẩn các bút Sửa và Xóa
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            //An groupbox chi tiet
            HienChiTiet(false);
        }
        //Viết một hàm nạp dữ liệu lên DataGrid
        private void LoadDuLieu(String sql)
        {
            //tạo đối tượng DataSet
            ds = new DataSet();
            //Khởi tạo đối tượng DataAdapter và cung cấp vào câu lệnh SQL và đối tượng Connection
            dap = new SqlDataAdapter(sql, con);
            //Dùng phương thức Fill của DataAdapter để đổ dữ liệu từ DataSource tới DataSet
            dap.Fill(ds);
            //Gắn dữ liệu từ DataSet lên DataGridView
            dgvKetQua.DataSource = ds.Tables[0];
        }
        //Phương thức ẩn hiện các control ở groupbox chi tiết
        private void HienChiTiet(Boolean hien)
        {
            txtMaSP.Enabled = hien;
            txtTenSP.Enabled = hien;
            txtSoLuong.Enabled = hien;
            txtDonVi.Enabled = hien;
            txtDonGia.Enabled = hien;
            txtGhiChu.Enabled = hien;
            //Ẩn hiện 2 nút Lưu và Hủy
            btnLuu.Enabled = hien;
            btnHuy.Enabled = hien;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            //Cập nhật trên nhãn tiêu đề
            lblTieuDe.Text = "Tìm Kiếm Mặt Hàng";
            //Cấm nút Sửa và Xóa
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            //Viet cau lenh SQL cho tim kiem
            String sql = "SELECT * FROM Nuoc";
            String dk = "";
            //Tim theo MaSP khac rong
            if (txtTKMaSP.Text.Trim() != "")
            {
                dk += " MaSP like '%" + txtTKMaSP.Text + "%'";
            }
            //kiem tra TenSP va MaSP khac rong
            if (txtTKTenSP.Text.Trim() != "" && dk != "")
            {
                dk += " AND TenSP like N'%" + txtTKTenSP.Text + "%'";
            }
            //Tim kiem theo TenSP khi MaSP la rong
            if (txtTKTenSP.Text.Trim() != "" && dk == "")
            {
                dk += " TenSP like N'%" + txtTKTenSP.Text + "%'";
            }
            //Ket hoi dk
            if (dk != "")
            {
                sql += " WHERE" + dk;
            }
            //Goi phương thức Load dữ liệu kết hợp điều kiện tìm kiếm
            LoadDuLieu(sql);
        }
        private void XoaTrangChiTiet()
        {
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            txtSoLuong.Text = "0";
            txtDonVi.Text = "";
            txtDonGia.Text = "0";
            txtGhiChu.Text = "";
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
                lblTieuDe.Text = "Thêm Mặt Hàng";
                XoaTrangChiTiet();
                //Cam nut sua xoa
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                //Hiện GroupBox Chi tiết
                HienChiTiet(true);
        }
        private void dgvKetQua_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Hien thi nut sua
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnThem.Enabled = false;
            
            try
            {
                txtMaSP.Text = dgvKetQua[0, e.RowIndex].Value.ToString();
                txtTenSP.Text = dgvKetQua[1, e.RowIndex].Value.ToString();
                txtSoLuong.Text=dgvKetQua[2, e.RowIndex].Value.ToString();  
                txtDonVi.Text = dgvKetQua[3, e.RowIndex].Value.ToString();
                txtDonGia.Text = dgvKetQua[4, e.RowIndex].Value.ToString();
                txtGhiChu.Text = dgvKetQua[5, e.RowIndex].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex+"Lỗi");
            }
        }
        //Sự kiện Click của nút sửa
        private void btnSua_Click(object sender, EventArgs e)
        {
            //Cập nhật tiêu đề
            lblTieuDe.Text = "Cập Nhật Mặt Hàng";
            //Ẩn hai nút Thêm và Sửa
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            //Hiện gropbox chi tiết
            HienChiTiet(true);
        }
        //Sự kiện Click của nút Xóa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            //Bật Message Box cảnh báo người sử dụng
            if (MessageBox.Show("Bạn có chắc chắn xóa mã mặt hàng " + txtMaSP.Text + " không? Nếu có ấn nút Lưu, không thì ấn nút Hủy", "Xóa sản phẩm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                lblTieuDe.Text = "Xoá Mặt Hàng";
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                //Hiện gropbox chi tiết
                HienChiTiet(true);
            }
        }
        //Sự kiện click vào button Lưu
        private void btnLuu_Click(object sender, EventArgs e)
        {

            string sql = "";
            //Kiếm tra nếu kết nối chưa mở thì thực hiện mở kết nối
            if (con.State != ConnectionState.Open)
                con.Open();
            //Chúng ta sử dụng control ErrorProvider để hiển thị lỗi
            //Kiểm tra tên sản phầm có bị để trống không
            if (txtTenSP.Text.Trim() == "")
            {
                errChiTiet.SetError(txtTenSP, "Bạn không để trống tên sản phẩm!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            } 
            //Kiểm tra đơn vị xem có để trống không 
            if (txtDonVi.Text.Trim() == "")
            {
                errChiTiet.SetError(txtDonVi, "Bạn không để trống đơn vi!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }
            //Kiểm tra đơn giá 
            if (txtDonGia.Text.Trim() == "")
            {
                errChiTiet.SetError(txtDonGia, "Bạn không để trống đơn giá!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }
        
            //Nếu nút Thêm enable thì thực hiện thêm mới 
            //Dùng ký tự N' trước mỗi giá trị kiểu text để insert giá trị có dấu tiếng việt vào CSDL được đúng 
            if (btnThem.Enabled == true)
            {
                //Kiểm tra xem ô nhập MaSP có bị trống không 
                if (txtMaSP.Text.Trim() == "")
                {
                    errChiTiet.SetError(txtMaSP, "Bạn không để trống mã sản phẩm trường này!");
                    return;
                }
                else
                {
                    //Kiểm tra xem mã sản phẩm đã tồn tại chưa đẻ tránh việc insert mới bị lỗi 
                    sql = "Select Count(*) From Nuoc Where MaSP ='" + txtMaSP.Text + "'";
                    cmd = new SqlCommand(sql, con);
                    int val = (int)cmd.ExecuteScalar();
                    if (val > 0)
                    {
                        errChiTiet.SetError(txtMaSP, "Mã sản phẩm trùng trong cơ sở dữ liệu");
                        return;
                    }
                    errChiTiet.Clear();
                }
                //Insert vao CSDL
               
                sql = "INSERT INTO Nuoc(MaSP,TenSP,SoLuong,DonVi,DonGia,GhiChu) VALUES (";
                sql += "'" + txtMaSP.Text + "',N'" + txtTenSP.Text + "','" + txtSoLuong.Text + "',N'" + txtDonVi.Text + "','" + txtDonGia.Text + "',N'" + txtGhiChu.Text + "')";
            }
            //Nếu nút Sửa enable thì thực hiện cập nhật dữ liệu
            if (btnSua.Enabled == true)
            {
                sql = "Update Nuoc SET ";
                sql += "TenSP = N'" + txtTenSP.Text + "',";
                sql += "SoLuong = '" + txtSoLuong.Text + "',"; 
                sql += "DonVi = N'" + txtDonVi.Text + "',";
                sql += "DonGia = '" + txtDonGia.Text + "',";
                sql += "GhiChu = N'" + txtGhiChu.Text + "' ";
                sql += "Where MaSP = N'" + txtMaSP.Text + "'";
            }
            //Nếu nút Xóa enable thì thực hiện xóa dữ liệu
            if (btnXoa.Enabled == true)
            {
                sql = "Delete From Nuoc Where MaSP =N'" + txtMaSP.Text + "'";
            }
            //Thuc thi cau lenh sql
            cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            //Cap nhat lai DataGrid
            sql = "Select * from Nuoc";
            LoadDuLieu(sql);
            //dong ket noi
            con.Close();
            //Ẩn hiện các nút phù hợp chức năng
            HienChiTiet(false);
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }
        private void btnHuy_Click(object sender, EventArgs e)
        {
            //Thiết lập lại các nút như ban đầu
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            btnThem.Enabled = true;
            //xoa trang
            XoaTrangChiTiet();
            //Cam nhap
            HienChiTiet(false);
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            //Đóng form
            this.Close();
        }
    }
}


