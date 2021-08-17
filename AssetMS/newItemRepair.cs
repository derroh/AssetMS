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
    public partial class newItemRepair : DevExpress.XtraEditors.XtraForm, IChildSave
    {
        string querryNum, currentUser;
        
        interface IChildSave
        {
            void SaveAction();
            void reset();
            void updatethis();
            void deleteThis();
            void NavigateDown();
            void NavigateUp();
        }
        public newItemRepair()
        {
            InitializeComponent();
            txt_RepairID.Text = getQueryNumber();
        }
        public newItemRepair(string ActiveUser)
        {
            InitializeComponent();
            currentUser = ActiveUser;
           // txt_RepairID.Text = "WPC/ICT/REPAIRS/" + getQueryNumber();
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {

        }

        public void SaveAction()
        {
            string RepairID = getQueryNumber();
            string ItemSerialNumber = txt_serialNumber.Text;
            string DateOfRepair = dateEdit1.Text;
            string RepairedBy = txt_repairedBy.Text;
            string RepairsMade = txt_repairsMade.Text;
            string Cost = txt_cost.Text;
            string DateAdded = DateTime.Now.ToString("G");
            if (ItemSerialNumber.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's serial number", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist", "Invalid operation");
                return;
            }
            if (DateOfRepair.Equals(""))
            {
                XtraMessageBox.Show("Kindly select a date first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (RepairedBy.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the name of whoever repaired it", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (RepairsMade.Equals(""))
            {
                XtraMessageBox.Show("Kindly give a short description of the repairs done", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Cost.Equals(""))
            {
                XtraMessageBox.Show("Kindly give the cost of the repairs done", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("INSERT INTO itemrepairs(RepairID, ItemSerialNumber, DateOfRepair, RepairedBy, RepairsMade, Cost, AddedBy, DateAdded) VALUES (@RepairID,@ItemSerialNumber,@DateOfRepair,@RepairedBy,@RepairsMade,@Cost, @AddedBy, @DateAdded)", conn))
                {
                    command.Parameters.AddWithValue("@RepairID", RepairID);
                    command.Parameters.AddWithValue("@ItemSerialNumber", ItemSerialNumber);
                    command.Parameters.AddWithValue("@DateOfRepair", DateOfRepair);
                    command.Parameters.AddWithValue("@RepairedBy", RepairedBy);
                    command.Parameters.AddWithValue("@RepairsMade", RepairsMade);
                    command.Parameters.AddWithValue("@Cost", Cost);
                    command.Parameters.AddWithValue("@AddedBy", currentUser);
                    command.Parameters.AddWithValue("@DateAdded", DateAdded);
                    command.ExecuteNonQuery();
                    using (MySqlCommand command5 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command5.Parameters.AddWithValue("@Activity", "Added repair data for an item, serial number " + ItemSerialNumber + " done by " + RepairedBy + " on " + DateOfRepair + ".");
                        command5.Parameters.AddWithValue("@User", currentUser);
                        command5.Parameters.AddWithValue("@Time", DateAdded);
                        command5.ExecuteNonQuery();
                        XtraMessageBox.Show("Repair records saved succesfully", "Action Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txt_cost.Text = "";
            txt_itemName.Text = "";
            txt_repairedBy.Text = "";
            txt_repairsMade.Text = "";
            txt_serialNumber.Text = "";
            dateEdit1.Text = "";
        }

        public void deleteThis()
        {
            string RepairID = txt_RepairID.Text;
            string serialNumber = txt_serialNumber.Text;
            string now = DateTime.Now.ToString("G");
            if (ifExists2().Equals(false))
            {
                XtraMessageBox.Show("The item repair ID entered does not exist", "Invalid operation");
                return;
            }
            DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to delete the repair records for repair ID '"+ RepairID + "' from the list of repaired items?", "System Confirmation", MessageBoxButtons.YesNo);

            if (dlr == DialogResult.Yes)
            {
                try
                {
                    string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                    MySqlConnection conn = new MySqlConnection(conString);
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM itemrepairs WHERE RepairID = @RepairID", conn))
                    {

                        command.Parameters.AddWithValue("@RepairID", RepairID);
                        command.ExecuteNonQuery();
                        using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                        {
                            command3.Parameters.AddWithValue("@Activity", "Deleted repair records for an item serial number '"+serialNumber+"' , Repair ID " + RepairID);
                            command3.Parameters.AddWithValue("@User", currentUser);
                            command3.Parameters.AddWithValue("@Time", now);
                            command3.ExecuteNonQuery();
                            XtraMessageBox.Show("Records deleted successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public void updatethis()
        {
            string RepairID = getQueryNumber();
            string ItemSerialNumber = txt_serialNumber.Text;
            string DateOfRepair = dateEdit1.Text;
            string RepairedBy = txt_repairedBy.Text;
            string RepairsMade = txt_repairsMade.Text;
            string Cost = txt_cost.Text;
            string DateAdded = DateTime.Now.ToString("G");
            if (ifExists2().Equals(false))
            {
                XtraMessageBox.Show("The item repair ID entered does not exist", "Invalid operation");
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist", "Invalid operation");
                return;
            }
            if (DateOfRepair.Equals(""))
            {
                XtraMessageBox.Show("Kindly select a date first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (RepairedBy.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the name of whoever repaired it", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (RepairsMade.Equals(""))
            {
                XtraMessageBox.Show("Kindly give a short description of the repairs done", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Cost.Equals(""))
            {
                XtraMessageBox.Show("Kindly give the cost of the repairs done", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("UPDATE itemrepairs SET ItemSerialNumber =@ItemSerialNumber,DateOfRepair=@DateOfRepair,RepairedBy=@RepairedBy,RepairsMade=@RepairsMade,Cost=@Cost WHERE RepairID = @RepairID", conn))
                {
                    command.Parameters.AddWithValue("@ItemSerialNumber", ItemSerialNumber);
                    command.Parameters.AddWithValue("@DateOfRepair", DateOfRepair);
                    command.Parameters.AddWithValue("@RepairedBy", RepairedBy);
                    command.Parameters.AddWithValue("@RepairsMade", RepairsMade);
                    command.Parameters.AddWithValue("@Cost", Cost);
                    command.Parameters.AddWithValue("@RepairID", RepairID);
                    command.ExecuteNonQuery();
                    using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command3.Parameters.AddWithValue("@Activity", "Updated an item repair record ID '"+ RepairID);
                        command3.Parameters.AddWithValue("@User", currentUser);
                        command3.Parameters.AddWithValue("@Time", DateAdded);
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
        private string getQueryNumber()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT COALESCE(MAX(ID),0) as invent FROM itemrepairs LIMIT 1", conn);
            MySqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string inventoryId = reader.GetString(0);
                    int anno;
                    int.TryParse(inventoryId, out anno);
                    int annoo = anno + 1;

                    string annotation = "WPC/REPAIRS/" + annoo.ToString();
                    querryNum = annotation;
                }
            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
            return querryNum;
        }

        private void txt_serialNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void newItemRepair_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((admin)this.MdiParent).DisableWithNoGrid(false);
        }

        private void txt_serialNumber_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select Name from tblNewItem where serialNumber='" + txt_serialNumber.Text.Trim() + "'", conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txt_itemName.Text = dr.GetString(0);

                }
                conn.Close();

            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }
        private bool ifExists()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from tblnewitem where SerialNumber='" + txt_serialNumber.Text.Trim() + "'", conn);
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
            MySqlCommand cmd2 = new MySqlCommand("select * from itemrepairs where RepairID='" + txt_RepairID.Text.Trim() + "'", conn);
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