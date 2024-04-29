using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Project_Demo
{

    class Functions
    {
        public static SqlConnection con;  //Khai báo đối tượng kết nối        

        public static void Connect()
        {
            con = new SqlConnection();   //Khởi tạo đối tượng
            con.ConnectionString = @"Data Source=PSAIYANS\SQLEXPRESS;Initial Catalog=CuaHangNuoc;Integrated Security=True";
            con.Open();                  //Mở kết nốis
        }
        public static void Disconnect()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();   	//Đóng kết nối
                con.Dispose(); 	//Giải phóng tài nguyên
                con = null;
            }
        }

        public static void FillCombo(string sql, ComboBox cbo, string ma, string ten)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, con);
            DataTable table = new DataTable();
            dap.Fill(table);
            cbo.DataSource = table;
            cbo.ValueMember = ma; //Trường giá trị
            cbo.DisplayMember = ten; //Trường hiển thị
        }

        public static DataTable GetDataToTable(string sql)
        {
            DataTable table = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter(sql,con);
            dap.Fill(table);
            return table;
        }
        public static void RunSQL(string sql)
        {
            SqlCommand cmd; 
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            try{
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex){
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();
            cmd = null;
        }
        public static bool CheckKey(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, con);
            DataTable table = new DataTable();
            dap.Fill(table);
            if (table.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
 
    }
}
