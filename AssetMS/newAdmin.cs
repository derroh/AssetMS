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
    public partial class newAdmin : DevExpress.XtraEditors.XtraForm
    {
        string newID, user_Id;
        
        public newAdmin()
        {
            InitializeComponent();
           // picBoxPhoto.Visible = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string now = DateTime.Now.ToString("G");
            string UserId, Name, Status;
            UserId = "";
            Name = txt_name.Text.Trim().ToTitleCase(TitleCase.All);
            string Password1 = txt_password.Text.Trim();
            string Password2 = txt_password2.Text.Trim();

            Status = "";

            if (Name.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the user's name first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    command.Parameters.AddWithValue("@UserId", UserId);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Email", "");
                    command.Parameters.AddWithValue("@PhoneNumber", "");
                    command.Parameters.AddWithValue("@Password", cipherText);
                    command.Parameters.AddWithValue("@Role", "Administrator");
                    command.Parameters.AddWithValue("@Status", Status);
                    MemoryStream stream = new MemoryStream();
                    Bitmap image1 = (Bitmap)Image.FromFile(@"C:\Users\HiGHROLLER\Documents\Visual Studio 2015\Projects\AssetMS\AssetMS\imgg.jpg", true);
                 
                    byte[] pic = ImageToByteArray(image1);
                    command.Parameters.AddWithValue("@Photo", pic);
                    command.Parameters.AddWithValue("@DateAdded", now);
                    command.Parameters.AddWithValue("@AddedBy", Name);
                    command.ExecuteNonQuery();

                    //get USer ID
                    getUserID();
                    //update User ID
                    try
                    {
                        using (MySqlCommand command2 = new MySqlCommand("UPDATE profilemaster SET UserId = @UserId WHERE Id = @user_Id", conn))
                        {
                            command2.Parameters.AddWithValue("@UserId", newID);
                            command2.Parameters.AddWithValue("@user_Id", user_Id);
                            command2.ExecuteNonQuery();
                            using (MySqlCommand command5 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                            {
                                command5.Parameters.AddWithValue("@Activity", "Added a new user, " + Name + ".");
                                command5.Parameters.AddWithValue("@User", Name);
                                command5.Parameters.AddWithValue("@Time", now);
                                command5.ExecuteNonQuery();
                                XtraMessageBox.Show("User Added successfully. User's User ID is " + newID + ".", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                // conn.Close();
            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }
        private void getUserID()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT MAX(Id) as user FROM profilemaster LIMIT 1", conn);
            MySqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string userId = reader.GetString(0);
                    user_Id = userId;
                    String ID = "WPC/ICT/" + userId;
                    newID = ID;

                }
            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }

        private void newAdmin_Load(object sender, EventArgs e)
        {

        }

        public void reset()
        {
            txt_name.Text = "";
            txt_password.Text = "";
            txt_password2.Text = "";
          }
        public byte[] ImageToByteArray(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

    }
}