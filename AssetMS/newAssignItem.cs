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
    public partial class newAssignItem : DevExpress.XtraEditors.XtraForm, IChildSave
    {
        private string currentUser;
        public static string pathToPDF = "";
        
        interface IChildSave
        {
            void SaveAction();
            void reset();
            void updatethis();
            void deleteThis();
            void NavigateDown();
            void NavigateUp();
        }
        public newAssignItem()
        {
            InitializeComponent();
        }
        public newAssignItem( string activeUser)
        {
            InitializeComponent();
            currentUser = activeUser;
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            FillComboDept();
            FillComboOFfice();
        }

        public void SaveAction()
        {
            string now = DateTime.Now.ToString("G");
            string serialNumber = txt_serialNum.Text.ToUpper();
            string departmentName = combo_department.Text;
            string officeName = combo_office.Text;
            string assignedUser = txt_assignedUser.Text;
            string ItemCondition = txt_condition.Text;
            if (serialNumber.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a serial number", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (officeName.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide an office", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (departmentName.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a department", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (assignedUser.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a user", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist", "Invalid operation");
                return;
            }
            if (isItemExists().Equals(true))
            {
                XtraMessageBox.Show("The item has been alredy assigned and not available for assigning", "Invalid operation");
                return;
            }
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("INSERT INTO assigneditems(SerialNumber, ItemCondition, DepartmentName, OfficeName, AssignedUser, DateAssigned, AssignedBy) VALUES (@SerialNumber, @ItemCondition, @DepartmentName, @OfficeName, @AssignedUser, @DateAssigned, @AssignedBy)", conn))
                {
                    command.Parameters.AddWithValue("@SerialNumber", serialNumber);
                    command.Parameters.AddWithValue("@ItemCondition", ItemCondition);
                    command.Parameters.AddWithValue("@DepartmentName", departmentName.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@OfficeName", officeName.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@AssignedUser", assignedUser.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@DateAssigned", now);
                    command.Parameters.AddWithValue("@AssignedBy", currentUser);
                    command.ExecuteNonQuery();
                    using (MySqlCommand command2 = new MySqlCommand("DELETE FROM unassignedItems  WHERE serialNumber = @serialNumber", conn))
                    {

                        command2.Parameters.AddWithValue("@serialNumber", serialNumber);
                        command2.ExecuteNonQuery();
                        using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                        {
                            command3.Parameters.AddWithValue("@Activity", "Assigned an item, serial number "+serialNumber+" to "+ assignedUser.ToTitleCase(TitleCase.All));
                            command3.Parameters.AddWithValue("@User", currentUser);
                            command3.Parameters.AddWithValue("@Time", now);
                            command3.ExecuteNonQuery();
                            XtraMessageBox.Show("Item assigned to user successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reset();
                        }
                        conn.Close();
                    }
                   // conn.ActiveCon().Close();
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
            txt_serialNum.Text = "";
            txt_category.Text = "";
            txt_ItemName.Text = "";
            txt_Manufacturer.Text = "";
            combo_office.Text = "";
            combo_department.Text = "";
            txt_assignedUser.Text = "";
            txt_condition.Text = "";
        }

        public void deleteThis()
        {
            string now = DateTime.Now.ToString("G");
            string serialNumber = txt_serialNum.Text.ToUpper();
            string assignedUser = txt_assignedUser.Text;
            if (serialNumber.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a serial number", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ifExists3().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist in the list of assigned items", "Invalid operation");
                return;
            }
            DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to delete the specified item from the list of assigned items?", "System Confirmation", MessageBoxButtons.YesNo);

            if (dlr == DialogResult.Yes)
            {
                try
                {
                    string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                    MySqlConnection conn = new MySqlConnection(conString);
                    conn.Open();

                    using (MySqlCommand command = new MySqlCommand("INSERT INTO unassigneditems( serialNumber) VALUES (@serialNumber)", conn))
                    {
                        command.Parameters.AddWithValue("@serialNumber", serialNumber);
                        command.ExecuteNonQuery();
                        using (MySqlCommand command2 = new MySqlCommand("DELETE FROM assigneditems  WHERE SerialNumber = @SerialNumber", conn))
                        {

                            command2.Parameters.AddWithValue("@SerialNumber", serialNumber);
                            command2.ExecuteNonQuery();
                            using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                            {
                                command3.Parameters.AddWithValue("@Activity", "Deleted an item, serial number " + serialNumber + "assigned to " + assignedUser.ToTitleCase(TitleCase.All));
                                command3.Parameters.AddWithValue("@User", currentUser);
                                command3.Parameters.AddWithValue("@Time", now);
                                command3.ExecuteNonQuery();
                                XtraMessageBox.Show("Item deleted successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                reset();
                            }
                            conn.Close();
                        }
                        // conn.Close();
                    }
                    // conn.Close();
                }
                catch (MySqlException es)
                {
                    XtraMessageBox.Show(es.ToString());
                }
            }

        }

        public void updatethis()
        {
            string serialNumber = txt_serialNum.Text.ToUpper();
            string departmentName = combo_department.Text;
            string officeName = combo_office.Text;
            string assignedUser = txt_assignedUser.Text;
            if (serialNumber.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a serial number", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ifExists2().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist", "Invalid operation");
                return;
            }
            if (officeName.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide an office", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (departmentName.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a department", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (assignedUser.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a user", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string now = DateTime.Now.ToString("G");
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("UPDATE assigneditems SET DepartmentName=@DepartmentName,OfficeName=@OfficeName,AssignedUser=@AssignedUser WHERE SerialNumber = @SerialNumber", conn))
                {
                    command.Parameters.AddWithValue("@SerialNumber", serialNumber);
                    command.Parameters.AddWithValue("@DepartmentName", departmentName);
                    command.Parameters.AddWithValue("@OfficeName", officeName);
                    command.Parameters.AddWithValue("@AssignedUser", assignedUser.ToTitleCase(TitleCase.All));
                    command.ExecuteNonQuery();
                    using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command3.Parameters.AddWithValue("@Activity", "Updated details for an item, serial number " + serialNumber + " assigned to " + assignedUser.ToTitleCase(TitleCase.All));
                        command3.Parameters.AddWithValue("@User", currentUser);
                        command3.Parameters.AddWithValue("@Time", now);
                        command3.ExecuteNonQuery();
                        XtraMessageBox.Show("Information has been updated successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public void NavigateDown()
        {
           // //throw new NotImplementedException();
        }

        public void NavigateUp()
        {
           // //throw new NotImplementedException();
        }

        public string Xport(string ext)
        {
            string x ="";
            return x;
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
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());
            }
            conn.Close();
        }
        public void FillComboDept()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM officeDepartments ", conn);
            MySqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string itemList = reader.GetString(2);
                    combo_department.Properties.Items.Add(itemList);
                }
                conn.Close();
            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());
            }
          //  conn.Close();
        }

        private void txt_serialNum_TextChanged(object sender, EventArgs e)
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from tblNewItem where serialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txt_category.Text = dr.GetString(1);
                txt_ItemName.Text = dr.GetString(2);
                txt_Manufacturer.Text = dr.GetString(4);
                txt_condition.Text= dr.GetString(8);

            }
            conn.Close();
        }

        private void txt_serialNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void XtraForm1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((admin)this.MdiParent).DisableWithNoGrid(false);
        }
        private bool isItemExists()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from assigneditems where serialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
                return true;
            }
            else
            {
                conn.Close();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from borroweditems where ItemSerialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return true;
                }
            }
            conn.Close();
            return false;
        }
        public bool ifExists()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from tblnewitem where SerialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
                return true;
            }
            conn.Close();
            return false;
        }
        private bool ifExists2()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from assigneditems where SerialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
                return true;
            }
            conn.Close();
            return false;
        }
        private bool ifExists3()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from assigneditems where serialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
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