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
    public partial class newItemloan : DevExpress.XtraEditors.XtraForm, IChildSave
    {
        string currentUser;
       
        interface IChildSave
        {
            void SaveAction();
            void reset();
            string Xport(String ext);
            void updatethis();
            void deleteThis();
            void NavigateDown();
            void NavigateUp();
        }
        public newItemloan()
        {
            InitializeComponent();
            getBorrowID();
        }
        public newItemloan(string ActiveUser)
        {
            InitializeComponent();
            currentUser = ActiveUser;
            getBorrowID();
            date_startBorrow.DateTime = DateTime.Now;
            date_return.DateTime = DateTime.Now.AddDays(1);
            date_startBorrow.Properties.MinValue = DateTime.Now;
            date_return.Properties.MinValue = DateTime.Now;
            
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            getBorrowID();
        }
        private void getBorrowID()
        {
         try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand command = new MySqlCommand("SELECT COALESCE(MAX(Id),0) as invent FROM borroweditems LIMIT 1", conn);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    String inventoryId = reader.GetString(0);
                    int anno;
                    int.TryParse(inventoryId, out anno);
                    int annoo = anno + 1;

                    txt_BorrowID.Text = "WPC/ICT/B/" + annoo.ToString();
                }

                conn.Close();
            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }

        public void SaveAction()
        {
            // MessageBox.Show();
            string borrowerName = txt_borrowersName.Text;
            string borrowersID = txt_BorrowersID.Text;
            string startdate = date_startBorrow.Text;
            string endDate = date_return.Text;
            string ItemSerialNumber = txt_serialNum.Text;
            string ItemCondition = txt_condition.Text;
            string AssignDate = DateTime.Now.ToString("G");
            string BorrowID = txt_BorrowID.Text;
            long result;
            DateTime date1 = date_startBorrow.DateTime;
            DateTime date2 = date_return.DateTime;

            result = DateTimeUtil.DateDiff(DateInterval.Day, date1, date2);
            
            if (ItemSerialNumber.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the Item's Serial number", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist", "Invalid operation");
                return;
            }
            if (isItemExists().Equals(true))
            {
                XtraMessageBox.Show("The item has already been loaned or assigned to a user.", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txt_BorrowID.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a borrower ID first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (borrowerName.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a borrowers name first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (borrowersID.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a borrowers ID number", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (startdate.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the date of borrowing", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (endDate.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the date of return", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //MessageBox.Show(result.ToString());
            int NumOfDays = (int)result;

            if (NumOfDays < 0)
            {
                XtraMessageBox.Show("Kindly provide a valid date of return. The item can be loaned out for a minimum of 1 day.", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("INSERT INTO borroweditems ( BorrowID,BorrowerName, BorrowerID, ItemSerialNumber, ItemCondition, BorrowDate, ReturnDate, AssignedBy, AssignDate) VALUES (@BorrowID, @BorrowerName,@BorrowerID,@ItemSerialNumber,@ItemCondition,@BorrowDate,@ReturnDate, @AssignedBy, @AssignDate)", conn))
                {
                    command.Parameters.AddWithValue("@BorrowID", txt_BorrowID.Text);
                    command.Parameters.AddWithValue("@BorrowerName", borrowerName.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@BorrowerID", borrowersID);
                    command.Parameters.AddWithValue("@ItemSerialNumber", ItemSerialNumber);
                    command.Parameters.AddWithValue("@ItemCondition", ItemCondition);
                    command.Parameters.AddWithValue("@BorrowDate", startdate);
                    command.Parameters.AddWithValue("@ReturnDate", endDate);
                    command.Parameters.AddWithValue("@AssignedBy", currentUser);
                    command.Parameters.AddWithValue("@AssignDate", AssignDate);
                    command.ExecuteNonQuery();
                    using (MySqlCommand command5 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command5.Parameters.AddWithValue("@Activity", "Loaned an item, serial number " + ItemSerialNumber + " to a user, id number " + borrowersID + " to be returned in " + NumOfDays + "days.");
                        command5.Parameters.AddWithValue("@User", currentUser);
                        command5.Parameters.AddWithValue("@Time", AssignDate);
                        command5.ExecuteNonQuery();
                        XtraMessageBox.Show("Records saved successfully. User has " + NumOfDays + " days to return Item.", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reset();
                        conn.Close();
                    }
                    //;

                }
                conn.Close();


            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }

        public void reset()
        {
            txt_serialNum.Text = "";
            txt_productName.Text = "";
            txt_manufacturer.Text = "";
            txt_value.Text = "";
            //txt_status.Text = "";
            txt_age.Text = "";
            txt_warranty.Text = "";
            txt_condition.Text = "";
            txt_BorrowID.Text = "";
            txt_BorrowersID.Text = "";
            txt_borrowersName.Text = "";
            date_startBorrow.DateTime = DateTime.Now;
            date_return.DateTime = DateTime.Now.AddDays(1);
            date_startBorrow.Properties.MinValue = DateTime.Now;
            date_return.Properties.MinValue = DateTime.Now;
        }

        public void deleteThis()
        {
            string assetSerialNum = txt_serialNum.Text;
            if (assetSerialNum.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter a serial number", "Required Field");
                return;
            }
            if (isItemExists().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist in the list of borrowed items or list of assigned items", "Invalid operation");
                return;
            }
            DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to delete the selected item from the list of borrowed items?", "System Confirmation", MessageBoxButtons.YesNo);

            if (dlr == DialogResult.Yes)
            {
                try
                {
                    string DateAdded = DateTime.Now.ToString("G");
                    string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                    MySqlConnection conn = new MySqlConnection(conString);
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM borroweditems  WHERE ItemSerialNumber = @ItemSerialNumber", conn))
                    {

                        command.Parameters.AddWithValue("@ItemSerialNumber", txt_serialNum.Text);
                        command.ExecuteNonQuery();

                        using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                        {
                            command3.Parameters.AddWithValue("@Activity", "Deleted an item serial number " + assetSerialNum + "from the list of assigned items");
                            command3.Parameters.AddWithValue("@User", currentUser);
                            command3.Parameters.AddWithValue("@Time", DateAdded);
                            command3.ExecuteNonQuery();
                            XtraMessageBox.Show("Item deleted successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reset();
                            conn.Close();
                        }


                    }
                    reset();//refresh table
                    conn.Close();
                }
                catch (MySqlException es)
                {
                    XtraMessageBox.Show(es.ToString());
                }

            }
        }

        public void updatethis()
        {
            string borrowerName = txt_borrowersName.Text;
            string borrowersID = txt_BorrowersID.Text;
            string startdate = date_startBorrow.Text;
            string endDate = date_return.Text;
            string ItemSerialNumber = txt_serialNum.Text;
            string AssignDate = DateTime.Now.ToString("d/MM/yyyy");
            string BorrowID = txt_BorrowID.Text;
            long result;
            DateTime date1 = date_startBorrow.DateTime;
            DateTime date2 = date_return.DateTime;

            result = DateTimeUtil.DateDiff(DateInterval.Day, date1, date2);
            if (isItemExists().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist in the list of borrowed items or list of assigned items", "Invalid operation");
                return;
            }
            if (txt_BorrowID.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a borrower ID first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (checkIfAssigned2().Equals(false))
            {
                XtraMessageBox.Show("The borrow ID provided does not exist", "Invalid operation");
                return;
            }
            if (borrowerName.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a borrowers name first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (borrowersID.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a borrowers ID number", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (startdate.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the date of borrowing", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (endDate.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the date of return", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //MessageBox.Show(result.ToString());
            int NumOfDays = (int)result;

            if (NumOfDays < 0)
            {
                XtraMessageBox.Show("Kindly provide a valid date of return. The item can be loaned out for a minimum of 1 day.", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("UPDATE borroweditems SET BorrowerName=@BorrowerName,BorrowerID=@BorrowerID,ItemSerialNumber=@ItemSerialNumber,BorrowDate=@BorrowDate,ReturnDate=@ReturnDate WHERE BorrowID=@BorrowID AND BorrowerID=@BorrowerID AND ItemSerialNumber=@ItemSerialNumber", conn))
                {

                    command.Parameters.AddWithValue("@BorrowID", txt_BorrowID.Text);
                    command.Parameters.AddWithValue("@BorrowerName", borrowerName.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@BorrowerID", borrowersID);
                    command.Parameters.AddWithValue("@ItemSerialNumber", ItemSerialNumber);
                    command.Parameters.AddWithValue("@BorrowDate", startdate);
                    command.Parameters.AddWithValue("@ReturnDate", endDate);
                    command.Parameters.AddWithValue("@AssignedBy", currentUser);
                    command.Parameters.AddWithValue("@AssignDate", AssignDate);
                    command.ExecuteNonQuery();
                    using (MySqlCommand command5 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command5.Parameters.AddWithValue("@Activity", "Updated loan data for an item, serial number " + ItemSerialNumber + " to a user, id number " + borrowersID + " to be returned in " + NumOfDays + "days.");
                        command5.Parameters.AddWithValue("@User", currentUser);
                        command5.Parameters.AddWithValue("@Time", AssignDate);
                        command5.ExecuteNonQuery();
                        XtraMessageBox.Show("Information has been updated successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reset();
                        conn.Close();
                    }

                }
                conn.Close();
            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }

        public void NavigateDown()
        {
           // throw new NotImplementedException();
        }

        public void NavigateUp()
        {
           // throw new NotImplementedException();
        }

        public string Xport(string ext)
        {
            // throw new NotImplementedException();
            string x = "";
            return x;
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
        private bool checkIfAssigned2()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from borroweditems where BorrowID='" + txt_BorrowID.Text.Trim() + "'", conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                // XtraMessageBox.Show("The item has already been borrowed. Item not available for lending", "Action unsuccesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }

            conn.Close();

            return false;
        }

        public enum DateInterval
        {
            Year,
            Month,
            Weekday,
            Day,
            Hour,
            Minute,
            Second
        }

        private void txt_BorrowID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select  borroweditems.BorrowerName, borroweditems.BorrowerID,  borroweditems.BorrowDate, borroweditems.ReturnDate ,tblnewitem.Name, tblnewitem.SerialNumber, tblnewitem.Manufacturer,tblnewitem.Status, tblnewitem.Value, tblnewitem.Age, tblnewitem.Warranty FROM borroweditems,tblnewitem WHERE borroweditems.BorrowID = '" + txt_BorrowID.Text + "' AND tblnewitem.SerialNumber = borroweditems.ItemSerialNumber", conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txt_borrowersName.Text = dr.GetString(0);
                    txt_BorrowersID.Text = dr.GetString(1);
                    date_startBorrow.Text = dr.GetString(2);
                    date_return.Text = dr.GetString(3);
                    txt_productName.Text = dr.GetString(4);
                    txt_serialNum.Text = dr.GetString(5);
                    txt_manufacturer.Text = dr.GetString(6);
                    txt_value.Text = dr.GetString(8);
                    txt_age.Text = dr.GetString(9);
                    txt_warranty.Text = dr.GetString(10);
                    txt_condition.Text = dr.GetString(7);

                  }

                conn.Close();

            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());//
            }
        }

        private void txt_serialNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txt_BorrowID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txt_serialNum_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand("select * from tblnewitem where SerialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
                MySqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.Read())
                {
                    txt_productName.Text = dr1.GetString(2);
                    txt_manufacturer.Text = dr1.GetString(4);
                    txt_value.Text = dr1.GetString(5);
                    txt_age.Text = dr1.GetString(6);
                    txt_condition.Text = dr1.GetString(8);
                    txt_warranty.Text = dr1.GetString(7);

                }

                conn.Close();

            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());//
            }
        }

        public static class DateTimeUtil
        {

            public static long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
            {

                TimeSpan ts = date2 - date1;

                switch (interval)
                {
                    case DateInterval.Year:
                        return date2.Year - date1.Year;
                    case DateInterval.Month:
                        return (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year));
                    case DateInterval.Weekday:
                        return Fix(ts.TotalDays) / 7;
                    case DateInterval.Day:
                        return Fix(ts.TotalDays);
                    case DateInterval.Hour:
                        return Fix(ts.TotalHours);
                    case DateInterval.Minute:
                        return Fix(ts.TotalMinutes);
                    default:
                        return Fix(ts.TotalSeconds);
                }
            }

            private static long Fix(double Number)
            {
                if (Number >= 0)
                {
                    return (long)Math.Floor(Number);
                }
                return (long)Math.Ceiling(Number);
            }
        }
        private bool ifExists()
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
       
    }
}