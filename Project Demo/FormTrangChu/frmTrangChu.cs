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
using System.Data.SqlTypes;
using ComponentFactory.Krypton.Toolkit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project_Demo
{
    public partial class frmTrangChu : KryptonForm
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
        public frmTrangChu()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        } 
        //check
        private int check(){
            sqlCon.Close();
            conSQL();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "select count(*) from TaiKhoan where ChucVu = N'Khách hàng' and TenDN= '"+txtTenDangNhap.Text.Trim()+"' and MatKhau= "+"'"+txtMatKhau.Text.Trim()+"'";

            sqlCmd.Connection = sqlCon;
            int count = (int)sqlCmd.ExecuteScalar();
            if(count > 0)
                return 1;
            else
            {
                sqlCmd.CommandText = "select count(*) from TaiKhoan where ChucVu = N'Nhân viên' and TenDN= '" + txtTenDangNhap.Text.Trim() + "' and MatKhau= " + "'" + txtMatKhau.Text.Trim() + "'";

                sqlCmd.Connection = sqlCon;
                count = (int)sqlCmd.ExecuteScalar();
                if(count>0)
                    return 2;
            }
            return 0;
        }
        private void DangNhap_Click(object sender, EventArgs e)
        {
            if (txtTenDangNhap.Text == "")
            {
                ThongBao.Text = "Không được để trống tên đăng nhập";
                txtTenDangNhap.Focus();
                return ;
            }

            if (txtMatKhau.Text == "")
            {
                ThongBao.Text = "Không được để trống mật khẩu";
                txtMatKhau.Focus();
                return ;
            }

            conSQL();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "select * from TaiKhoan where TenDN= @TenDN and MatKhau= @MatKhau";
            SqlParameter parTenDN = new SqlParameter("@TenDN", SqlDbType.Char);
            parTenDN.Value = txtTenDangNhap.Text.Trim();
            SqlParameter parMK = new SqlParameter("@MatKhau", SqlDbType.Char);
            parMK.Value = txtMatKhau.Text.Trim();
            sqlCmd.Parameters.Add(parTenDN);
            sqlCmd.Parameters.Add(parMK);
            sqlCmd.Connection = sqlCon;

            //thực thi
            SqlDataReader reader = sqlCmd.ExecuteReader();
            if (reader.Read() == true)
            {
                if (check() == 1)
                {
                    FormGiaoDien form3 = new FormGiaoDien();
                    form3.Show();
                }
                else if(check() == 0)
                {
                    frmAdmin form5 = new frmAdmin();  
                    form5.Show();
                }
                else{
                    frmStaff form4 = new frmStaff();
                    form4.Show();   
                }
            }
            else
            {
                ThongBao.Text = "Sai tên đăng nhập hoặc mật khẩu !";
            }
            reader.Close();
            return ;

        }

        private void Thoat_Click(object sender, EventArgs e)
        {
            DialogResult thoat = MessageBox.Show("Bạn có muốn thoát không ?", "Thoát", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(thoat == DialogResult.Yes)
                Application.Exit();
        }

        private void DangKy_Click(object sender, EventArgs e)
        {
            //Load form đăng ký 
            frmDangKi form = new frmDangKi();
            form.Show();
        }


        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
      
        }

        private void txtMatKhau_MouseClick(object sender, MouseEventArgs e)
        {
        
        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void kryptonPalette1_PalettePaint(object sender, PaletteLayoutEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
        private bool isTextBoxEmpty = false;
        private void txtTenDangNhap_TextChanged(object sender, EventArgs e)
        {
            isTextBoxEmpty = string.IsNullOrEmpty(txtTenDangNhap.Text);
        }

        private void txtTenDangNhap_Click(object sender, EventArgs e)
        {
            if (!isTextBoxEmpty)
            {
                txtTenDangNhap.Text = ""; // Xoá nội dung trong text box
                isTextBoxEmpty = true; // Đánh dấu là text box đã rỗng
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
