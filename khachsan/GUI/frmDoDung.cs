﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace khachsan
{
    public partial class frmDoDung : Form
    {
        DataTable dt;
        public static SqlConnection conn;
        public frmDoDung()
        {
            InitializeComponent();
        }
        private bool trangthai = true;
        public void Mo_txt()
        {
            txtMaDD.Enabled = true;
            txtTenDD.Enabled = true;
            txtGia.Enabled = true;
        }
        public void Khoa_txt()
        {
            //các txt khóa, ko cho nhập 
            txtMaDD.Enabled = false;
            txtTenDD.Enabled = false;
            txtGia.Enabled = false;
        }

        public void Null()
        {
            txtMaDD.Text = "";
            txtTenDD.Text = "";
            txtGia.Text = "";
        }
        private void Load_DoDung()
        {

            conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Nam_3_Kỳ2\Thuctapnhom\Project de day len GitHub\khachsan\khachsan\Hotel.mdf;Integrated Security=True;Connect Timeout=30");
            conn.Open();
            SqlDataAdapter daDoDung = new SqlDataAdapter("select MaDo, TenDo, Gia from DoDung ", conn);
            dt = new DataTable();
            daDoDung.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void DoDung_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'hotelDataSet1.DoDung' table. You can move, or remove it, as needed.
            //this.doDungTableAdapter.Fill(this.hotelDataSet1.DoDung);
            Load_DoDung();
            btnGhi.Enabled = false;
            Khoa_txt();
        }

        private void btnGhi_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Nam_3_Kỳ2\Thuctapnhom\Project de day len GitHub\khachsan\khachsan\Hotel.mdf;Integrated Security=True;Connect Timeout=30");
            conn.Open();
            if (trangthai == true)
            {               
                SqlCommand cmd = new SqlCommand("ThemDo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter p = new SqlParameter("@ma", txtMaDD.Text);
                cmd.Parameters.Add(p);
                p = new SqlParameter("@ten", txtTenDD.Text);
                cmd.Parameters.Add(p);
                p = new SqlParameter("@gia", txtGia.Text);
                cmd.Parameters.Add(p);
                cmd.ExecuteNonQuery();
                Load_DoDung();
                btn_Sua.Enabled = true;
            }
            else if(trangthai == false)
            {
                SqlCommand cmd = new SqlCommand("SuaDo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter p = new SqlParameter("@ma", txtMaDD.Text);
                cmd.Parameters.Add(p);
                p = new SqlParameter("@ten", txtTenDD.Text);
                cmd.Parameters.Add(p);
                p = new SqlParameter("@gia", txtGia.Text);
                cmd.Parameters.Add(p);
                cmd.ExecuteNonQuery();
                Load_DoDung();
                btnThem.Enabled = true;
            }
            Khoa_txt();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Dispose();
            }
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            Mo_txt();
            Null();
            trangthai = true;
            btnGhi.Enabled = true;
            btn_Sua.Enabled = false;
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            Mo_txt();
            btnGhi.Enabled = true;
            trangthai = false;
            btnThem.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {           
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Nam_3_Kỳ2\Thuctapnhom\Project de day len GitHub\khachsan\khachsan\Hotel.mdf;Integrated Security=True;Connect Timeout=30");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("XoaDo", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter p = new SqlParameter("@ma", txtMaDD.Text);
                    cmd.Parameters.Add(p);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công!");
                    Load_DoDung();                 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi" + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                txtMaDD.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["MaDD"].Value);
                txtTenDD.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["TenDD"].Value);
                txtGia.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["Gia"].Value);                
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                txtMaDD.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["MaDD"].Value);
                txtTenDD.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["TenDD"].Value);
                txtGia.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["Gia"].Value);

            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string tenncc = string.Format("[TenDo] like '%{0}%'", txtTimKiem.Text);
            dt.DefaultView.RowFilter = tenncc;
        }
        public static void ExportToExcel(DataGridView dtgr)
        {
            // Creating a Excel object.
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {
                worksheet = workbook.ActiveSheet;

                worksheet.Name = "ExportedFromDatGrid";

                int cellRowIndex = 1;
                int cellColumnIndex = 1;

                //Loop through each row and read value from each column.
                for (int i = -1; i < dtgr.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dtgr.Columns.Count; j++)
                    {
                        // Excel index starts from 1,1. As first Row would have the Column headers, adding a condition check.
                        if (cellRowIndex == 1)
                        {
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dtgr.Columns[j].HeaderText;
                        }
                        else
                        {
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dtgr.Rows[i].Cells[j].Value.ToString();
                        }
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }
                worksheet.Columns.AutoFit();
                //Getting the location and file name of the excel to save from user.
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveDialog.FilterIndex = 2;

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Export Successful");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }
        }
        private void btn_XuatExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel(dataGridView1);
        }
    }
}
