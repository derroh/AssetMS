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
    public partial class newCategory : DevExpress.XtraEditors.XtraForm, IChildSave
    {
        private string currentUser;
        OpenFileDialog filePhoto = new OpenFileDialog();
        Image img = null;
        
        interface IChildSave
        {
            void SaveAction();
            void reset();
            void Xport(String ext);
            void updatethis();
            void NavigateDown();
            void NavigateUp();
        }
        public newCategory()
        {
            InitializeComponent();

        }
        public newCategory(string ActiveUser)
        {
            InitializeComponent();
            currentUser = ActiveUser;
            getCategoryID();
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {

        }

        public void SaveAction()
        {
            string now = DateTime.Now.ToString("G");
            string categoryName = txt_CategoryName.Text.Trim();
            string categoryId = txt_categoryID.Text.Trim();
            string photoName = txt_fname.Text;
            if (categoryName.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a category ID", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (categoryName.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a category name", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (photoName.Equals(""))
            {
                XtraMessageBox.Show("Kindly upload a photo for the category", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("INSERT INTO availableCategory( CategoryId, CategoryName, Photo, DateAdded, AddedBy) values(@CategoryId, @categoryName, @Photo, @dateAdded, @addedBy)", conn))
                {
                    command.Parameters.AddWithValue("@CategoryId", txt_categoryID.Text.Trim());
                    command.Parameters.AddWithValue("@categoryName", categoryName.ToTitleCase(TitleCase.All));
                    MemoryStream stream = new MemoryStream();
                    byte[] pic = ImageToByteArray(picBoxPhoto.Image);
                    command.Parameters.AddWithValue("@Photo", pic);
                    command.Parameters.AddWithValue("@dateAdded", now);
                    command.Parameters.AddWithValue("@addedBy", currentUser);
                    command.ExecuteNonQuery();
                    //conn.Close();
                    using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command3.Parameters.AddWithValue("@Activity", "Added a product category, category ID " + categoryId);
                        command3.Parameters.AddWithValue("@User", currentUser);
                        command3.Parameters.AddWithValue("@Time", now);
                        command3.ExecuteNonQuery();
                        XtraMessageBox.Show("Category Added successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txt_CategoryName.Text = "";
            picBoxPhoto.Image = null;
        }

        public void deleteThis()
        {
            string categoryID = txt_categoryID.Text.Trim();
            string now = DateTime.Now.ToString("G");
            if (categoryID.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter a category ID", "Required Field");
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The category ID entered does not exist", "Invalid operation");
                return;
            }
            DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to delete the specified product category from the system?", "System Confirmation", MessageBoxButtons.YesNo);

            if (dlr == DialogResult.Yes)
            {
                try
                {
                    string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                    MySqlConnection conn = new MySqlConnection(conString);
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM availablecategory  WHERE CategoryId = @categoryID", conn))
                    {

                        command.Parameters.AddWithValue("@categoryID", categoryID);
                        command.ExecuteNonQuery();

                        using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                        {
                            command3.Parameters.AddWithValue("@Activity", "Deleted a product category, category ID " + categoryID);
                            command3.Parameters.AddWithValue("@User", currentUser);
                            command3.Parameters.AddWithValue("@Time", now);
                            command3.ExecuteNonQuery();
                            XtraMessageBox.Show("Category deleted successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reset();
                        }
                        conn.Close();

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
            string categoryName = txt_CategoryName.Text;
            string categoryId = txt_categoryID.Text;
            string now = DateTime.Now.ToString("G");
            if (categoryName.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a category ID", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The category ID entered does not exist", "Invalid operation");
                return;
            }
            if (categoryName.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a category name", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("UPDATE availablecategory SET CategoryName = @categoryName, Photo = @Photo WHERE CategoryId = @CategoryId", conn))
                {
                    command.Parameters.AddWithValue("@CategoryId", txt_categoryID.Text.Trim());
                    command.Parameters.AddWithValue("@categoryName", categoryName.ToTitleCase(TitleCase.All));
                    MemoryStream stream = new MemoryStream();
                    byte[] pic = ImageToByteArray(picBoxPhoto.Image);
                    command.Parameters.AddWithValue("@Photo", pic);
                    command.ExecuteNonQuery();
                    
                    using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command3.Parameters.AddWithValue("@Activity", "Updated a product category, category ID " + categoryId);
                        command3.Parameters.AddWithValue("@User", currentUser);
                        command3.Parameters.AddWithValue("@Time", now);
                        command3.ExecuteNonQuery();
                        XtraMessageBox.Show("Category updated successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            string x = "";
            return x;
        }
        public byte[] ImageToByteArray(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        private void getCategoryID()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT COALESCE(MAX(Id),0) as user FROM availablecategory LIMIT 1", conn);
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

                    string ID = "WPC/ICT/C/" +annoo.ToString();
                    txt_categoryID.Text = ID;
                    
                }
                conn.Close();
            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }

        private void txt_categoryID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * FROM availablecategory WHERE CategoryId='" + txt_categoryID.Text.Trim() + "'", conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txt_CategoryName.Text = dr.GetString(2);

                    byte[] img = (byte[])(dr["Photo"]);
                    if (img == null)
                        picBoxPhoto.Image = null;
                    else
                    {
                        MemoryStream mstream = new MemoryStream(img);
                        picBoxPhoto.Image = System.Drawing.Image.FromStream(mstream);
                       // this.picBoxPhoto.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
                conn.Close();

            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }

        private void txt_categoryID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void XtraForm1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((admin)this.MdiParent).DisableWithNoGrid(false);
        }

        private void XtraForm1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            filePhoto.Filter = "Image Files|*.jpg;*.bmp;*.png;*.jpeg;";
            DialogResult result = filePhoto.ShowDialog();
            string filename;
            if (result == DialogResult.Cancel)
                return;
            filename = filePhoto.FileName;
            txt_fname.Text = Path.GetFileName(filename);

            try
            {
                img = Image.FromFile(filename);
                picBoxPhoto.Image = img;
                // this.picBoxPhoto.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch
            {
                return;
            }
        }
        private bool ifExists()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from availablecategory where CategoryId='" + txt_categoryID.Text.Trim() + "'", conn);
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