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
    public partial class newsystemUser : DevExpress.XtraEditors.XtraForm, IChildSave
    {

        string currentUser;
        OpenFileDialog filePhoto = new OpenFileDialog();
        Image img = null;
       
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
        public newsystemUser()
        {
            InitializeComponent();
        }
        public newsystemUser(string activeUser)
        {
            InitializeComponent();
            currentUser = activeUser;
            getUserID();
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            UserLevels();
        }

        public void SaveAction()
        {
            string now = DateTime.Now.ToString("G");
            string  Name, Email, PhoneNumber, Role, Status;
            
            Name = txt_name.Text.Trim().ToTitleCase(TitleCase.All);
            Email = txt_mail.Text.Trim();
            PhoneNumber = txt_phone.Text.Trim();
            string Password1 = txt_password.Text.Trim();
            string Password2 = txt_password2.Text.Trim();

            Role = txt_role.Text;
            Status = "";

            if (Name.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the user's name first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Email.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the user's email first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!(TestEmail.IsEmail(Email)))
            {
                XtraMessageBox.Show("Kindly provide a valid email address", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (PhoneNumber.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the user's phone number first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Password1.Equals(Password2))
            {
                if (!(TestPassword.ValidatePassword(Password1)))
                {
                    XtraMessageBox.Show("Oops, your password did not meet the requirements.\nA strong password should\n\t-Have at least one lower case letter\n\t-Have at least one upper case letter\n\t-Have at least one special character\n\t-Have at least one number\n\t-Be at least 8 characters long", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                XtraMessageBox.Show("Kindly make sure your passwords match", "Required fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string cipherText = EnCryptDecrypt.Encrypt(Password1, true);
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("INSERT INTO profilemaster( UserId, Name, Email, PhoneNumber, Password, Role, Status, Photo, DateAdded, AddedBy) VALUES (@UserId,@Name,@Email,@PhoneNumber,@Password,@Role,@Status,@Photo,@DateAdded, @AddedBy)", conn))
                {
                    command.Parameters.AddWithValue("@UserId", txt_userID.Text);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                    command.Parameters.AddWithValue("@Password", cipherText);
                    command.Parameters.AddWithValue("@Role", Role);
                    command.Parameters.AddWithValue("@Status", Status);
                    MemoryStream stream = new MemoryStream();
                    byte[] pic = ImageToByteArray(picBoxPhoto.Image);
                    command.Parameters.AddWithValue("@Photo", pic);
                    command.Parameters.AddWithValue("@DateAdded", now);
                    command.Parameters.AddWithValue("@AddedBy", currentUser);
                    command.ExecuteNonQuery();
                     try
                    {

                        using (MySqlCommand command5 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                        {
                            command5.Parameters.AddWithValue("@Activity", "Added a new user, " + Name + ".");
                            command5.Parameters.AddWithValue("@User", currentUser);
                            command5.Parameters.AddWithValue("@Time", now);
                            command5.ExecuteNonQuery();
                            XtraMessageBox.Show("User Added successfully. User's User ID is " + txt_userID.Text + ".", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reset();
                            conn.Close();
                        }
                    }
                    catch (MySqlException es)
                    {
                        XtraMessageBox.Show(es.ToString());
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
            txt_name.Text = "";
            txt_mail.Text = "";
            txt_password.Text = "";
            txt_password2.Text = "";
            txt_phone.Text = "";
            txt_role.Text = "";
            getUserID();
        }

        public void deleteThis()
        {
            string UserId = txt_userID.Text;
            string now = DateTime.Now.ToString("G");
            if (UserId.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a user ID first", "Invalid operation");
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The user ID entered does not exist", "Invalid operation");
                return;
            }
            DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to delete this user from the system?", "System Confirmation", MessageBoxButtons.YesNo);

            if (dlr == DialogResult.Yes)
            {
                try
                {
                    string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                    MySqlConnection conn = new MySqlConnection(conString);
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM profilemaster  WHERE UserId = @UserId", conn))
                    {

                        command.Parameters.AddWithValue("@UserId", UserId);
                        command.ExecuteNonQuery();
                        using (MySqlCommand command5 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                        {
                            command5.Parameters.AddWithValue("@Activity", "Deleted a new user, " + txt_name.Text + ".");
                            command5.Parameters.AddWithValue("@User", currentUser);
                            command5.Parameters.AddWithValue("@Time", now);
                            command5.ExecuteNonQuery();
                            XtraMessageBox.Show("User deleted successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reset();
                            conn.Close();
                        }
 
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
            string now = DateTime.Now.ToString("G");
            string UserId, Name, Email, PhoneNumber, Role, Status;
            UserId = txt_userID.Text.Trim();
            Name = txt_name.Text.Trim().ToTitleCase(TitleCase.All);
            Email = txt_mail.Text.Trim();
            PhoneNumber = txt_phone.Text.Trim();
            string Password1 = txt_password.Text.Trim();
            string Password2 = txt_password2.Text.Trim();

            Role = txt_role.Text;
            Status = "";
            if (UserId.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a user ID first", "Invalid operation");
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The user ID entered does not exist", "Invalid operation");
                return;
            }
            if (Name.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the user's name first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Email.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the user's email first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!(TestEmail.IsEmail(Email)))
            {
                XtraMessageBox.Show("Kindly provide a valid email address", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (PhoneNumber.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the user's phone number first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Password1.Equals(Password2))
            {
                if (!(TestPassword.ValidatePassword(Password1)))
                {
                    XtraMessageBox.Show("Oops, your password did not meet the requirements.\nA strong password should\n\t-Have at least one lower case letter\n\t-Have at least one upper case letter\n\t-Have at least one special character\n\t-Have at least one number\n\t-Be at least 8 characters long", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                XtraMessageBox.Show("Kindly make sure your passwords match", "Required fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (picBoxPhoto.Image.Equals(null))
            {
                XtraMessageBox.Show("Kindly provide the user's phone number first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string cipherText = EnCryptDecrypt.Encrypt(Password1, true);
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("UPDATE profilemaster SET Name =@Name,Email=@Email,PhoneNumber=@PhoneNumber,Password=@Password,Photo=@Photo,Role=@Role,Status=@Status WHERE UserId=@UserId", conn))
                {

                    command.Parameters.AddWithValue("@UserId", UserId);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                    command.Parameters.AddWithValue("@Password", cipherText);
                    command.Parameters.AddWithValue("@Role", Role);
                    command.Parameters.AddWithValue("@Status", Status);
                    MemoryStream stream = new MemoryStream();
                    byte[] pic = ImageToByteArray(picBoxPhoto.Image);
                    command.Parameters.AddWithValue("@Photo", pic);
                    command.ExecuteNonQuery();
                    using (MySqlCommand command5 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command5.Parameters.AddWithValue("@Activity", "Updated data for a user, " + txt_name.Text + ".");
                        command5.Parameters.AddWithValue("@User", currentUser);
                        command5.Parameters.AddWithValue("@Time", now);
                        command5.ExecuteNonQuery();
                        XtraMessageBox.Show("User details updated successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void getUserID()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT COALESCE(MAX(Id),0) as user FROM profilemaster LIMIT 1", conn);
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

                    string ID = "WPC/ICT/" + annoo.ToString();

                    txt_userID.Text = ID;

                }
                conn.Close();
            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
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

        public byte[] ImageToByteArray(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void newsystemUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((admin)this.MdiParent).DisableWithNoGrid(false);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SaveAction();
        }

        private void textEdit1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from profilemaster where UserId='" + txt_userID.Text.Trim() + "'", conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txt_name.Text = dr.GetString(2);
                    txt_mail.Text = dr.GetString(3);
                    string decryptedText = EnCryptDecrypt.Decrypt(dr.GetString(5), true);
                    txt_password.Text = decryptedText;
                    txt_password2.Text = decryptedText;
                    txt_phone.Text = dr.GetString(4);
                    txt_role.Text = dr.GetString(7);
                    byte[] img = (byte[])(dr["Photo"]);
                    if (img == null)
                        picBoxPhoto.Image = null;
                    else
                    {
                        MemoryStream mstream = new MemoryStream(img);
                        picBoxPhoto.Image = System.Drawing.Image.FromStream(mstream);
                      //  this.picBoxPhoto.SizeMode = PictureBoxSizeMode.Zoom;
                    }

                }

                conn.Close();

            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());//
            }
        }

        private void UserLevels()
        {
            AutoCompleteStringCollection colValues = new AutoCompleteStringCollection();
            colValues.AddRange(new string[] { "Administator", "User" });
            txt_role.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt_role.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_role.MaskBox.AutoCompleteCustomSource = colValues;
        }
        private bool ifExists()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from profilemaster where UserId='" + txt_userID.Text.Trim() + "'", conn);
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