using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;

namespace AssetMS
{
    public partial class newDepartment : DevExpress.XtraEditors.XtraForm, IChildSave
    {
        private string currentUser;
       
        interface IChildSave
        {
            void SaveAction();
            void reset();
            void ToExcel();
            string Xport(string ext);
            void updatethis();
            void deleteThis();
            void NavigateDown();
            void NavigateUp();
        }
        public newDepartment()
        {
            InitializeComponent();
        }
        public newDepartment(string ActiveUser)
        {
            InitializeComponent();
            currentUser = ActiveUser;
            getDepartmentID();
            //simpleButton1.Enabled = false;
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            FillComboOFfice();
        }

        public void SaveAction()
        {
            string now = DateTime.Now.ToString("G");
            string categoryName = txt_DepatmentName.Text;
            string parentOffeName = combo_office.Text;
            string DepartmentId = txt_deptID.Text;
            if (DepartmentId.Equals(""))
            {
                XtraMessageBox.Show("Kindly select a department ID first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (parentOffeName.Equals(""))
            {
                XtraMessageBox.Show("Kindly select a parent office first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //validate 
            if (categoryName.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a department name", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("INSERT INTO officeDepartments(DepartmentId, DepartmentName, ParentOffice, DateAdded, AddedBy) values(@DepartmentId, @DepartmentName, @ParentOffice, @DateAdded, @AddedBy)", conn))
                {
                    command.Parameters.AddWithValue("@DepartmentId", DepartmentId);
                    command.Parameters.AddWithValue("@DepartmentName", categoryName.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@ParentOffice", parentOffeName);
                    command.Parameters.AddWithValue("@DateAdded", now);
                    command.Parameters.AddWithValue("@AddedBy", currentUser);
                    command.ExecuteNonQuery();
                    using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command3.Parameters.AddWithValue("@Activity", "Added a department, Department ID " + DepartmentId);
                        command3.Parameters.AddWithValue("@User", currentUser);
                        command3.Parameters.AddWithValue("@Time", now);
                        command3.ExecuteNonQuery();
                        XtraMessageBox.Show("Department Added successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reset();
                       
                    }
                    conn.Close();

                }
                // conn.Close();
            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }

        public void reset()
        {
            getDepartmentID();
            txt_DepatmentName.Text = "";
            combo_office.Text = "";
        }

        public string Xport(string ext)
        {
            string x = "";
            return x;
        }

        public void updatethis()
        {
            string deptName = txt_DepatmentName.Text;
            string parentOffeName = combo_office.Text;
            string DepartmentId = txt_deptID.Text;
            string now = DateTime.Now.ToString("G");
            if (DepartmentId.Equals(""))
            {
                XtraMessageBox.Show("Kindly give a department ID first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The department ID entered does not exist", "Invalid operation");
                return;
            }
            if (parentOffeName.Equals(""))
            {
                XtraMessageBox.Show("Kindly select a parent office first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //validate 
            if (deptName.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a department name", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("UPDATE officeDepartments SET DepartmentName = @departmentName, parentOffice = @parentOffice  WHERE DepartmentId = @DepartmentId", conn))
                {
                    command.Parameters.AddWithValue("@departmentName", deptName);
                    command.Parameters.AddWithValue("@parentOffice", parentOffeName);
                    command.Parameters.AddWithValue("@DepartmentId", DepartmentId);
                    command.ExecuteNonQuery();
                    using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command3.Parameters.AddWithValue("@Activity", "Updated a department, Department ID " + DepartmentId);
                        command3.Parameters.AddWithValue("@User", currentUser);
                        command3.Parameters.AddWithValue("@Time", now);
                        command3.ExecuteNonQuery();
                        XtraMessageBox.Show("Department updated successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reset();
                       
                    }
                    conn.Close();
                }

                conn.Close();
            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }

        public void deleteThis()
        {
            string categoryName = txt_DepatmentName.Text;
            string parentOffeName = combo_office.Text;
            string DepartmentId = txt_deptID.Text;
            string now = DateTime.Now.ToString("G");
            if (DepartmentId.Equals(""))
            {
                XtraMessageBox.Show("Kindly select a give a department first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The department ID entered does not exist", "Invalid operation");
                return;
            }
            DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to delete the selected department from the system?", "System Confirmation", MessageBoxButtons.YesNo);

            if (dlr == DialogResult.Yes)
            {
                try
                {
                    string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                    MySqlConnection conn = new MySqlConnection(conString);
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM officeDepartments  WHERE DepartmentId = @DepartmentId", conn))
                    {

                        command.Parameters.AddWithValue("@DepartmentId", DepartmentId);
                        command.ExecuteNonQuery();
                        using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                        {
                            command3.Parameters.AddWithValue("@Activity", "Deleted a department, Department ID " + DepartmentId);
                            command3.Parameters.AddWithValue("@User", currentUser);
                            command3.Parameters.AddWithValue("@Time", now);
                            command3.ExecuteNonQuery();
                            XtraMessageBox.Show("Department deleted successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reset();
                           
                        }
                        conn.Close();
                    }
               
                }
                catch (MySqlException es)
                {
                    XtraMessageBox.Show(es.ToString());
                }

            }
        }

        public void NavigateDown()
        {
           // //throw new NotImplementedException();
        }

        public void NavigateUp()
        {
          //  //throw new NotImplementedException();
        }
        public void FillComboOFfice()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM officesTable", conn);
            MySqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string itemList = reader.GetString(2);
                    combo_office.Properties.Items.Add(itemList);
                }
                conn.Close();
            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
            
        }
        private void getDepartmentID()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT COALESCE(MAX(Id),0) as user FROM officedepartments LIMIT 1", conn);
            MySqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string userId = reader.GetString(0);
                    int anno;
                    int.TryParse(userId, out anno);
                    int annoo = anno + 1;

                    string ID = "WPC/ICT/D/" + annoo.ToString();
                    txt_deptID.Text = ID;

                }
                conn.Close();
            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }

        private void txt_deptID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * FROM officedepartments WHERE DepartmentId ='" + txt_deptID.Text.Trim() + "'", conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txt_deptID.Text = dr.GetString(1);
                    txt_DepatmentName.Text = dr.GetString(2);
                    combo_office.Text = dr.GetString(3);

                }
                conn.Close();

            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }

        private void XtraForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void txt_DepatmentName_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void newDepartment_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((admin)this.MdiParent).DisableWithNoGrid(false);
        }
        private bool ifExists()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * FROM officedepartments WHERE DepartmentId ='" + txt_deptID.Text.Trim() + "'", conn);
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
                return true;
            }
            conn.Close();
            return false;
        }
    }
}