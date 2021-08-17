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
    public partial class newQuery : DevExpress.XtraEditors.XtraForm, IChildSave
    {
        string querryNum;
        
        interface IChildSave
        {
            void SaveAction();
            void reset();
            void updatethis();
            void deleteThis();
            void NavigateDown();
            void NavigateUp();
        }
        public newQuery()
        {
            InitializeComponent();
            FillComboOFfice();
            FillComboDept();
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {

        }

        public void SaveAction()
        {
            string user = txt_name.Text;
            string Office = combo_office.Text;
            string Department = combo_department.Text;
            string Description = memo_description.Text;
            string now = DateTime.Now.ToString("G");
            if (txt_name.Text.Equals(""))
            {
                XtraMessageBox.Show("Oops... Name cannot be balnk. kindly add your name for better service", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (memo_description.Text.Equals(""))
            {
                XtraMessageBox.Show("Oops... Message cannot be balnk. kindly add a short description for better service", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("INSERT INTO reportedissues(QueryID, user, Date, Office, Department, Description, IssueSolved) VALUES (@QueryID,@user,@Date,@Office,@Department,@Description, @IssueSolved)", conn))
                {

                    command.Parameters.AddWithValue("@QueryID", getQueryNumber());
                    command.Parameters.AddWithValue("@Date", now);
                    command.Parameters.AddWithValue("@Office", Office.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@Department", Department.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@IssueSolved", "FALSE");
                    command.Parameters.AddWithValue("@user", user);
                    command.ExecuteNonQuery();
                    XtraMessageBox.Show("Query sent Succesfully", "Action Succesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reset();
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
            combo_office.Text = "";
            combo_department.Text = "";
            txt_name.Text = "";
            memo_description.Text = "";
        }

        public void deleteThis()
        {
           // //throw new NotImplementedException();
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
            ////throw new NotImplementedException();
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
                }
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
            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
            conn.Close();
        }
        private string getQueryNumber()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT COALESCE(MAX(ID),0) as invent FROM reportedissues LIMIT 1", conn);
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

                    string annotation = "WPC/QUERIES/" + annoo.ToString();
                    querryNum = annotation;
                }
            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());
            }
            return querryNum;
        }

        private void XtraForm1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((admin)this.MdiParent).DisableWithNoGrid(false);
        }
    }
}