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
using ComponentFactory.Krypton.Toolkit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project_Demo
{
    public partial class frmDangKi : KryptonForm
    { 
        //tao 2 bien cuc bo
        string strCon = @"Data Source=PSAIYANS\SQLEXPRESS;Initial Catalog=CuaHangNuoc;Integrated Security=True";
        //Đối tượng kết nối 
        SqlConnection sqlCon = null;
        private void conSQL()
        {
            try
            {
                if (sqlCon == null)
                {
                    //Rỗng thì tạo mới
                    sqlCon = new SqlConnection(strCon);
                }
                if (sqlCon.State == ConnectionState.Closed)
                {
                    //Mở kết nối 
                    sqlCon.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public frmDangKi()
        {
            InitializeComponent();
        }


        private void btThoat_Click(object sender, EventArgs e)
        {
            //Đóng form 2
            this.Close();
        }
        
        private void btDangKy_Click(object sender, EventArgs e)
        { 
            conSQL();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "select count(*) from TaiKhoan where TenDN = '"+ txtDangKy.Text.Trim() + "'";
            sqlCmd.Connection = sqlCon;

                int count =(int) sqlCmd.ExecuteScalar();
            //Trường hợp tên đăng nhập rỗng
            if(txtDangKy.Text =="Tên đăng nhập" || txtDangKy.Text == "")
            {
                mess.Text = "Vui lòng nhập tên đăng nhập";
                //đưa con chuột vào text đăng nhập
                txtDangKy.Focus();
                return;
            }    

            //Trường hợp 2 mật khẩu không khớp
            if (String.Compare(txtMK1.Text.Trim(), txtMK2.Text.Trim(),true) != 0)
            {
                mess.Text = "Mật khẩu không trùng khớp!";
                txtMK2.Focus();
                txtMK2.SelectAll();
                return;
            }  
            
            //Trường hợp rỗng MK1 
            if(txtMK1.Text.Trim() == "")
            {
                mess.Text = "Vui lòng nhập mật khẩu";
                //đưa con chuột vào text mk1
                txtMK1.Focus();
                return;
            }

            //kiểm tra khóa chính
            if (count == 0 && String.Compare(txtMK1.Text, txtMK2.Text,true) == 0)
            {

                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "insert into TaiKhoan values (@tenDN,@MK1,@ChucVu)";
                SqlParameter parTenDN = new SqlParameter("@tenDN", SqlDbType.Char);
                SqlParameter parMK = new SqlParameter("@MK1", SqlDbType.Char);
                SqlParameter parChucVu = new SqlParameter("@ChucVu", SqlDbType.NVarChar);
                parTenDN.Value = txtDangKy.Text.Trim();
                parMK.Value = txtMK1.Text.Trim();
                if (rdoQuanLy.Checked == true)
                {
                    parChucVu.Value = "Quản Lý";
                }
                else if (rdoNhanVien.Checked == true)
                {
                    parChucVu.Value = "Nhân Viên";
                }
                else
                    parChucVu.Value = "Khách hàng";
    
                sqlCmd.Parameters.Add(parTenDN);
                sqlCmd.Parameters.Add(parMK);
                sqlCmd.Parameters.Add(parChucVu);
                sqlCmd.Connection = sqlCon;
                int kq = sqlCmd.ExecuteNonQuery();
                MessageBox.Show("Tạo tài khoản thành công", "Thông báo", buttons: MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                Close();
                
            }
            else
            {
                mess.Text = "Tên đăng nhập đã tồn tại mời bạn chọn tên khác";
                txtDangKy.Focus();
                txtDangKy.SelectAll();
            }
          

        }

     
        //set lại màu và cho vị trí chữ nhập vào bên trái biến chữ đang hiện trên text box thành nulls
        private void txtDangKy_MouseClick(object sender, MouseEventArgs e)
        {
            txtDangKy.Text = "";
            txtDangKy.ForeColor = Color.Black;
            txtDangKy.TextAlign = HorizontalAlignment.Left;
        }

        private void txtMK1_MouseClick(object sender, MouseEventArgs e)
        {
            txtMK1.Text = "";
            txtMK1.ForeColor = Color.Black;
            txtMK1.TextAlign = HorizontalAlignment.Left;
            txtMK1.PasswordChar = '*';
        }

        private void txtMK2_MouseClick(object sender, MouseEventArgs e)
        {
            txtMK2.Text = "";
            txtMK2.ForeColor = Color.Black;
            txtMK2.TextAlign = HorizontalAlignment.Left;
            txtMK2.PasswordChar = '*';
        }

        private void kryptonPalette1_PalettePaint(object sender, PaletteLayoutEventArgs e)
        {

        }

        private void txtMK2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDangKy_Click(object sender, EventArgs e)
        {
           
        }
    }
}
