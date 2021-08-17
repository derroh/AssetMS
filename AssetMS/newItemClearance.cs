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
    public partial class newItemClearance : DevExpress.XtraEditors.XtraForm, IChildSave
    {
        string currentUser;
        
        interface IChildSave
        {
            void SaveAction();
            void reset();
            void updatethis();
            void deleteThis();
            void NavigateDown();
            void NavigateUp();
        }
        public newItemClearance()
        {
            InitializeComponent();
        }
        public newItemClearance(String ActiveUser)
        {
            InitializeComponent();
            currentUser = ActiveUser;
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            StatusList();
            FillComboDept();
            FillComboOFfice();
        }

        public void SaveAction()
        {
           
            if (checkIflost().Equals(true))
            {
                XtraMessageBox.Show("The item is listed as lost. Kindly remove it from the list of lost items first", "Operation stopped", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string ItemSerialNumber = txt_serialNum.Text;
            string BorrowID = txt_borrowID.Text;
            string BorrowerID = txt_BorrowersID.Text;
            string DateAuthorized = DateTime.Now.ToString("G");
            string ConditionOfItem = combo_office.Text;
            string DateOfReturn = date_return.Text;
            if (ItemSerialNumber.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the item's Serial Number", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (checkIfAssigned().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist in the list of borrowed items or list of assigned items", "Invalid operation");
                return;
            }
            if (DateOfReturn.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the date when the item was returned", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ConditionOfItem.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the item's current working state", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to clear this user from the list of borrowed items?", "System Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dlr == DialogResult.Yes)
            {
                try
                {
                    //delete from borrowed items
                    string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                    MySqlConnection conn = new MySqlConnection(conString);
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM borroweditems  WHERE BorrowID = @BorrowID AND ItemSerialNumber = @ItemSerialNumber AND BorrowerID = @BorrowerID", conn))
                    {
                        command.Parameters.AddWithValue("@BorrowID", BorrowID);
                        command.Parameters.AddWithValue("@ItemSerialNumber", ItemSerialNumber);
                        command.Parameters.AddWithValue("@BorrowerID", BorrowerID);
                        command.ExecuteNonQuery();

                        //list item as available for borrowing ana assigning
                        // Del();

                        using (MySqlCommand command2 = new MySqlCommand("INSERT INTO unassigneditems(serialNumber) VALUES (@serialNumber)", conn))
                        {
                            command2.Parameters.AddWithValue("@serialNumber", ItemSerialNumber);
                            command2.ExecuteNonQuery();

                            //record this transaction

                            using (MySqlCommand command3 = new MySqlCommand("INSERT INTO returneditems( ItemSerialNumber, DateOfReturn, ConditionOfItem, AuthorizedBy, DateAuthorized) VALUES (@ItemSerialNumber,@DateOfReturn,@ConditionOfItem,@AuthorizedBy,@DateAuthorized)", conn))
                            {
                                command3.Parameters.AddWithValue("@ItemSerialNumber", ItemSerialNumber);
                                command3.Parameters.AddWithValue("@DateOfReturn", DateOfReturn);
                                command3.Parameters.AddWithValue("@ConditionOfItem", ConditionOfItem);
                                command3.Parameters.AddWithValue("@AuthorizedBy", currentUser);
                                command3.Parameters.AddWithValue("@DateAuthorized", DateAuthorized);
                                command3.ExecuteNonQuery();
                                //update condition of item, if working or not working
                                using (MySqlCommand command4 = new MySqlCommand("UPDATE tblnewitem SET Status=@Status WHERE SerialNumber = @SerialNumber", conn))
                                {
                                    command4.Parameters.AddWithValue("@Status", combo_office.Text);
                                    command4.Parameters.AddWithValue("@SerialNumber", ItemSerialNumber);
                                    command4.ExecuteNonQuery();
                                    Del();
                                    using (MySqlCommand command5 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                                    {
                                        command5.Parameters.AddWithValue("@Activity", "Cleared a user, id number " + BorrowerID + " in charge an item serial number " + ItemSerialNumber);
                                        command5.Parameters.AddWithValue("@User", currentUser);
                                        command5.Parameters.AddWithValue("@Time", DateAuthorized);
                                        command5.ExecuteNonQuery();
                                        XtraMessageBox.Show("User has been cleared succesfully", "Action succesfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        reset();
                                        conn.Close();
                                    }

                                }
                            }

                        }

                    }
                    //refresh table

                }
                catch (MySqlException es)
                {
                    XtraMessageBox.Show(es.ToString());
                }

            }

        }
       
        public void reset()
        {
            txt_productName.Text = "";
            txt_age.Text = "";
            txt_manufacturer.Text = "";
            txt_value.Text = "";
            txt_status.Text = "";
            txt_BorrowersID.Text = "";
            date_startBorrow.Text = "";
            date_return.Text = "";
            txt_borrowID.Text = "";
            txt_status.Text = "";
            combo_department.Text = "";
            combo_office.Text = "";
            txt_serialNum.Text = "";
            txt_borrowersName.Text = "";
        }

        public void deleteThis()
        {
            ////throw new NotImplementedException();
        }

        public void updatethis()
        {
           // //throw new NotImplementedException();
        }

        public void NavigateDown()
        {
            ////throw new NotImplementedException();
        }

        public void NavigateUp()
        {
           // //throw new NotImplementedException();
        }

        public string Xport(string ext)
        {
            string x = "";
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
                    //  XtraMessageBox.Show(itemList);
                }
                conn.Close();
            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());
            }
            
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
                    String itemList = reader.GetString(2);
                    // combo_category.Items.Add(itemList);
                    combo_department.Properties.Items.Add(itemList);
                    //  XtraMessageBox.Show(itemList);
                }
                conn.Close();
            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());
            }
            
        }

        private void txt_serialNum_TextChanged(object sender, EventArgs e)
        {
            search();
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tblNewItem where serialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    // combo_category.Text = dr.GetString(1);
                    txt_productName.Text = dr.GetString(2);
                    txt_age.Text = dr.GetString(6);
                    txt_manufacturer.Text = dr.GetString(4);
                    txt_value.Text = dr.GetString(5);
                    // txt_status.Text = dr.GetString(7);
                    //
                    conn.Close();
                    conn.Open();
                    MySqlCommand cmd2 = new MySqlCommand("select * from borroweditems where ItemSerialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
                    MySqlDataReader dr2 = cmd2.ExecuteReader();
                    if (dr2.Read())
                    {
                        txt_borrowersName.Text = dr2.GetString(2);
                        txt_BorrowersID.Text = dr2.GetString(3);
                        date_startBorrow.Text = dr2.GetString(6);
                        date_return.Text = dr2.GetString(7);
                        txt_borrowID.Text = dr2.GetString(1);
                        txt_status.Text = dr2.GetString(5);


                    }

                }
                conn.Close();

            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }
        private void StatusList()
        {
            AutoCompleteStringCollection colValues = new AutoCompleteStringCollection();
            colValues.AddRange(new string[] { "Working", "Not Working" });
            txt_status.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt_status.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_status.MaskBox.AutoCompleteCustomSource = colValues;
        }

        private void txt_serialNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }
        private void search()
        {
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand cmd2 = new MySqlCommand("select * from assigneditems where SerialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
                MySqlDataReader dr2 = cmd2.ExecuteReader();
                if (dr2.Read())
                {
                    txt_borrowersName.Text = dr2.GetString(5);
                    txt_status.Text = dr2.GetString(2);
                    combo_office.Text = dr2.GetString(4);
                    combo_department.Text = dr2.GetString(3);
                }
                conn.Close();
            }
            catch (MySqlException e)
            {
                XtraMessageBox.Show(e.ToString());
            }
        }
        public bool checkIflost()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM lostitems WHERE ItemSerialNumber = '" + txt_serialNum.Text.Trim() + "'", conn);
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
                // XtraMessageBox.Show("The item has already been assigned. Item not available for lending", "Action unsuccesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            conn.Close();
            return false;
        }
        private void Del()
        {
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("DELETE FROM assigneditems WHERE serialNumber = @serialNumber", conn))
                {

                    command.Parameters.AddWithValue("@SerialNumber", txt_serialNum.Text);
                    command.ExecuteNonQuery();
                    //   XtraMessageBox.Show("Item deleted successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reset();
                }

            }
            catch (MySqlException e)
            {
                XtraMessageBox.Show(e.ToString());
            }
        }

        private void newItemClearance_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((admin)this.MdiParent).DisableWithNoGrid(false);
        }
        private bool ifExists()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from borroweditems where ItemSerialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
                return true;
            }
            conn.Close();
            return false;
        }
        private bool checkIfAssigned()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from assigneditems where serialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
                // XtraMessageBox.Show("The item has already been assigned. Item not available for lending", "Action unsuccesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    // XtraMessageBox.Show("The item has already been borrowed. Item not available for lending", "Action unsuccesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
            }
            conn.Close();

            return false;
        }
    }
}