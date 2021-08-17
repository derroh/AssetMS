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
    public partial class newItemStore : DevExpress.XtraEditors.XtraForm, IChildSave
    {
        private string currentUser;
       
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
        public newItemStore()
        {
            InitializeComponent();
        }
        public newItemStore(String ActiveUser)
        {
            InitializeComponent();
            currentUser = ActiveUser;
        }


        private void XtraForm1_Load(object sender, EventArgs e)
        {
            StatusList();
        }

        public void SaveAction()
        {
            DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to move this item to the store?", "System Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr == DialogResult.Yes)
            {
                if (ifExists().Equals(false))
                {
                    XtraMessageBox.Show("The item serial number entered does not exist", "Invalid operation");
                    return;
                }
                if (checkAssigned().Equals(false))
                {
                    XtraMessageBox.Show("Item cannot be moved to store as it is assigned to a user", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string SerialNumber = txt_serialNum.Text;
                string DateAdded = DateTime.Now.ToString("G");
                try
                {
                    string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                    MySqlConnection conn = new MySqlConnection(conString);
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand("INSERT INTO storeitems(SerialNumber, ItemCondition, AddedBy, DateAdded) VALUES (@SerialNumber,@ItemCondition,@AddedBy,@DateAdded)", conn))
                    {
                        command.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                        command.Parameters.AddWithValue("@ItemCondition", txt_status.Text);
                        command.Parameters.AddWithValue("@AddedBy", currentUser);
                        command.Parameters.AddWithValue("@DateAdded", DateAdded);
                        command.ExecuteNonQuery();
                        Del();
                        using (MySqlCommand command5 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                        {
                            command5.Parameters.AddWithValue("@Activity", "Moved an item, serial number " + SerialNumber + " to the store.");
                            command5.Parameters.AddWithValue("@User", currentUser);
                            command5.Parameters.AddWithValue("@Time", DateAdded);
                            command5.ExecuteNonQuery();
                            XtraMessageBox.Show("Item moved to store successfully.", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reset();
                            conn.Close();
                        }
                        //  fillGrid();
                    }
                    conn.Close();

                }
                catch (MySqlException es)
                {
                    XtraMessageBox.Show(es.ToString());
                }

            }
            else if (dlr == DialogResult.No)
            {
                // e.Cancel = true;
            }
        }

        public void reset()
        {
            txt_productName.Text = "";
            txt_age.Text = "";
            txt_manufacturer.Text = "";
            txt_value.Text = "";
            txt_status.Text = "";
        }

        public void deleteThis()
        {
           // //throw new NotImplementedException();
        }

        public void updatethis()
        {
            ////throw new NotImplementedException();
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
            string x = "";
            return x;
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

        private void txt_serialNum_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tblNewItem where serialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txt_productName.Text = dr.GetString(2);
                    txt_age.Text = dr.GetString(6);
                    txt_manufacturer.Text = dr.GetString(4);
                    txt_value.Text = dr.GetString(5);
                    txt_status.Text = dr.GetString(7);

                }
                conn.Close();
            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }
        private bool checkAssigned()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from unassigneditems where serialNumber='" + txt_serialNum.Text.Trim() + "'", conn);
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
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
                using (MySqlCommand command = new MySqlCommand("DELETE FROM unassigneditems  WHERE 	serialNumber = @serialNumber", conn))
                {

                    command.Parameters.AddWithValue("@serialNumber", txt_serialNum.Text);
                    command.ExecuteNonQuery();
                    reset();
                }

            }
            catch (MySqlException e)
            {
                XtraMessageBox.Show(e.ToString());
            }
        }

        private void XtraForm1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((admin)this.MdiParent).DisableWithNoGrid(false);
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