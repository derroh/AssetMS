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
    public partial class newOffice : DevExpress.XtraEditors.XtraForm, IChildSave
    {
        private string currentUser;
       
        interface IChildSave
        {
            void SaveAction();
            void reset();
            void Xport(String ext);
            void deleteThis();
            void updatethis();
            void NavigateDown();
            void NavigateUp();
        }
        public newOffice()
        {
            InitializeComponent();
        }
        public newOffice(String ActiveUser)
        {
            InitializeComponent();
            currentUser = ActiveUser;
            getOfficeID();
        }
        private void XtraForm1_Load(object sender, EventArgs e)
        {

        }

        public void SaveAction()
        {
            string DateAdded = DateTime.Now.ToString("G");
            string officename = txt_OfficeName.Text.Trim();
            string OfficeLocation = txt_officeLocation.Text.Trim();
            string OfficeId = txt_officeID.Text.Trim();
            if (OfficeId.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide an office ID", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (officename.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide an office name", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (OfficeLocation.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the office's location", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("INSERT INTO officestable( OfficeId, OfficeName, OfficeLocation, DateAdded, AddedBy) VALUES (@OfficeId,@OfficeName,@OfficeLocation,@DateAdded,@AddedBy)", conn))
                {
                    command.Parameters.AddWithValue("@OfficeId", OfficeId);
                    command.Parameters.AddWithValue("@OfficeName", officename.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@OfficeLocation", officename.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@DateAdded", DateAdded);
                    command.Parameters.AddWithValue("@AddedBy", currentUser.ToTitleCase(TitleCase.All));
                    command.ExecuteNonQuery();
                    using (MySqlCommand command5 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command5.Parameters.AddWithValue("@Activity", "Added an office, " + officename + ".");
                        command5.Parameters.AddWithValue("@User", currentUser);
                        command5.Parameters.AddWithValue("@Time", DateAdded);
                        command5.ExecuteNonQuery();
                        XtraMessageBox.Show("Office Added successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reset();
                        conn.Close();
                    }
                    
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
            txt_OfficeName.Text = "";
            txt_officeLocation.Text = "";
            getOfficeID();
        }

        public void deleteThis()
        {
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The office ID entered does not exist", "Invalid operation");
                return;
            }
            string OfficeId = txt_officeID.Text.Trim();
            if (OfficeId.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide an office ID", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to delete this office from the list of offices?", "System Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr == DialogResult.Yes)
            {
                string DateAdded = DateTime.Now.ToString("G");
                try
                {
                    string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                    MySqlConnection conn = new MySqlConnection(conString);
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM officestable WHERE OfficeId = @OfficeId", conn))
                    {
                        command.Parameters.AddWithValue("@OfficeId", OfficeId);
                        command.ExecuteNonQuery();
                        using (MySqlCommand command5 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                        {
                            command5.Parameters.AddWithValue("@Activity", "Deleted an office, " + txt_OfficeName.Text + ".");
                            command5.Parameters.AddWithValue("@User", currentUser);
                            command5.Parameters.AddWithValue("@Time", DateAdded);
                            command5.ExecuteNonQuery();
                            XtraMessageBox.Show("Office deleted successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reset();
                            conn.Close();
                        }
                        
                    }
                    // conn.Close();
                }
                catch (MySqlException es)
                {
                    XtraMessageBox.Show(es.ToString());
                }
            }
            else if (dlr == DialogResult.No)
            {
                //e.Cancel = true;
            }
           
        }

        public void updatethis()
        {
            DateTime now = DateTime.Now;
            //validate first
            string OfficeName = txt_OfficeName.Text.Trim();
            string OfficeLocation = txt_officeLocation.Text.Trim();
            string OfficeId = txt_officeID.Text.Trim();
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The office ID entered does not exist", "Invalid operation");
                return;
            }
            if (OfficeId.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide an office ID", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (OfficeName.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide an office name", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (OfficeLocation.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the office's location", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string DateAdded = DateTime.Now.ToString("G");
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("UPDATE officestable SET OfficeName =@OfficeName,OfficeLocation=@OfficeLocation WHERE OfficeId = @OfficeId", conn))
                {

                    command.Parameters.AddWithValue("@OfficeName", OfficeName.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@OfficeLocation", OfficeLocation.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@OfficeId", txt_officeID.Text);
                    command.ExecuteNonQuery();
                    using (MySqlCommand command5 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command5.Parameters.AddWithValue("@Activity", "Updated data for an office, " + txt_OfficeName.Text + ".");
                        command5.Parameters.AddWithValue("@User", currentUser);
                        command5.Parameters.AddWithValue("@Time", DateAdded);
                        command5.ExecuteNonQuery();
                        XtraMessageBox.Show("Office details updated successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void getOfficeID()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT COALESCE(MAX(Id),0) as user FROM officestable LIMIT 1", conn);
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

                    string ID = "WPC/ICT/O/" + annoo.ToString();
                    txt_officeID.Text = ID;

                }
            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }

        private void XtraForm1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((admin)this.MdiParent).DisableWithNoGrid(false);
        }

        private void txt_officeID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * FROM officestable WHERE OfficeId='" + txt_officeID.Text.Trim() + "'", conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txt_OfficeName.Text = dr.GetString(2);
                    txt_officeLocation.Text = dr.GetString(3);

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
            MySqlCommand cmd2 = new MySqlCommand("select * FROM officestable WHERE OfficeId = '" + txt_officeID.Text.Trim() + "'", conn);
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