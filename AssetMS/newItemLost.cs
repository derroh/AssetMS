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
using System.IO;

namespace AssetMS
{
    public partial class newItemLost : DevExpress.XtraEditors.XtraForm, IChildSave
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
        public newItemLost()
        {
            InitializeComponent();
        }
        public newItemLost(string ActiveUser)
        {
            InitializeComponent();
            currentUser = ActiveUser;
        }

        private void txt_SerialNumber_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tblNewItem where serialNumber='" + txt_SerialNumber.Text.Trim() + "'", conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    combo_category.Text = dr.GetString(1);
                    txt_ProductName.Text = dr.GetString(2);
                    txt_Age.Text = dr.GetString(6);
                    txt_warranty.Text = dr.GetString(7);
                    txt_Manufacturer.Text = dr.GetString(4);
                    txt_Value.Text = dr.GetString(5);
                    txt_Status.Text = dr.GetString(8);
                    byte[] img = (byte[])(dr["Photo"]);
                    if (img == null)
                        picBoxPhoto.Image = null;
                    else
                    {
                        MemoryStream mstream = new MemoryStream(img);
                        picBoxPhoto.Image = System.Drawing.Image.FromStream(mstream);
                        //  this.picBoxPhoto.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    conn.Close();
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd2 = new MySqlCommand("SELECT  AssignedUser FROM assigneditems WHERE SerialNumber = '" + txt_SerialNumber.Text.Trim() + "'", conn);
                        MySqlDataReader dr2 = cmd2.ExecuteReader();
                        if (dr2.Read())
                        {
                            txt_UserIncharge.Text = dr2.GetString(0);
                        }
                        conn.Close();

                    }
                    catch (MySqlException es)
                    {
                        XtraMessageBox.Show(es.ToString());
                    }
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
            string assetSerialNum = txt_SerialNumber.Text;
            string UserIncharge = txt_UserIncharge.Text;
            string DateLost = dtaeEdit.Text;
            string DateAdded = DateTime.Now.ToString("G");
            if (assetSerialNum.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter a serial number", "Required Field");
                return;
            }
            if (assetSerialNum.Equals(""))
            {
                XtraMessageBox.Show("Kindly select the date the item got lost first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ifExists2().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist", "Invalid operation");
                return;
            }
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("INSERT INTO lostitems(ItemSerialNumber, UserIncharge, DateLost, AddedBy, DateAdded) VALUES (@ItemSerialNumber,@UserIncharge,@DateLost,@AddedBy,@DateAdded)", conn))
                {
                    command.Parameters.AddWithValue("@UserIncharge", UserIncharge);
                    command.Parameters.AddWithValue("@ItemSerialNumber", assetSerialNum);
                    command.Parameters.AddWithValue("@DateLost", DateLost);
                    command.Parameters.AddWithValue("@AddedBy", currentUser);
                    command.Parameters.AddWithValue("@DateAdded", DateAdded);
                    command.ExecuteNonQuery();
                     using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command3.Parameters.AddWithValue("@Activity", "Added an item serial number " + assetSerialNum+ "to the list of lost items.");
                        command3.Parameters.AddWithValue("@User", currentUser);
                        command3.Parameters.AddWithValue("@Time", DateAdded);
                        command3.ExecuteNonQuery();
                        XtraMessageBox.Show("Item Added Succesfully", "Action Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public void reset()
        {
            txt_Age.Text = "";
            txt_Manufacturer.Text = "";
            txt_ProductName.Text = "";
            txt_SerialNumber.Text = "";
            txt_Value.Text = "";
            txt_Status.Text = "";
            combo_category.Text = "";
            txt_SerialNumber.Focus();
            txt_warranty.Text = "";
            picBoxPhoto.Image = null;
            txt_UserIncharge.Text = "";
            dtaeEdit.Text = "";
        }

        public void deleteThis()
        {
            string assetSerialNum = txt_SerialNumber.Text;
            if (assetSerialNum.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter a serial number", "Required Field");
                return;
            }
            if (ifExists2().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist", "Invalid operation");
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist in the list of lost items", "Invalid operation");
                return;
            }
        
            DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to delete the selected item from the list of lost items?", "System Confirmation", MessageBoxButtons.YesNo);

            if (dlr == DialogResult.Yes)
            {
                try
                {
                    string DateAdded = DateTime.Now.ToString("G");
                    string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                    MySqlConnection conn = new MySqlConnection(conString);
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM lostitems  WHERE ItemSerialNumber = @ItemSerialNumber", conn))
                    {

                        command.Parameters.AddWithValue("@ItemSerialNumber", txt_SerialNumber.Text);
                        command.ExecuteNonQuery();

                        using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                        {
                            command3.Parameters.AddWithValue("@Activity", "Deleted an item serial number " + assetSerialNum+"from the list of lost items.");
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
            string ItemSerialNumber = txt_SerialNumber.Text;
            string DateLost = dtaeEdit.Text;
            string UserIncharge = txt_UserIncharge.Text;
            if (txt_SerialNumber.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's serial number first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ifExists2().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist" ,"Invalid operation");
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist in the list of lost items", "Invalid operation");
                return;
            }
            if (UserIncharge.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the user incharge first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (DateLost.Equals(""))
            {
                XtraMessageBox.Show("Kindly select the date the item got lost first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string DateAdded = DateTime.Now.ToString("G");
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("UPDATE lostitems SET UserIncharge=@UserIncharge,DateLost=@DateLost WHERE ItemSerialNumber = @ItemSerialNumber", conn))
                {
                    command.Parameters.AddWithValue("@UserIncharge", UserIncharge.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@DateLost", DateLost);
                    command.Parameters.AddWithValue("@ItemSerialNumber", ItemSerialNumber);
                   
                    command.ExecuteNonQuery();
                    using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command3.Parameters.AddWithValue("@Activity", "Updated data for an item serial number " + ItemSerialNumber);
                        command3.Parameters.AddWithValue("@User", currentUser);
                        command3.Parameters.AddWithValue("@Time", DateAdded);
                        command3.ExecuteNonQuery();
                        XtraMessageBox.Show("Item data updated succesfully", "Action Succesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            //  throw new NotImplementedException();
            string x = "";
            return x;
        }

        private void newItemLost_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((admin)this.MdiParent).DisableWithNoGrid(false);
        }

        private void newItemLost_Load(object sender, EventArgs e)
        {

        }
        private bool ifExists()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from lostitems where ItemSerialNumber='" + txt_SerialNumber.Text.Trim() + "'", conn);
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
            MySqlCommand cmd2 = new MySqlCommand("select * from tblnewitem where SerialNumber='" + txt_SerialNumber.Text.Trim() + "'", conn);
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