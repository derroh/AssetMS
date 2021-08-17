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
using System.Data.SqlClient;
using System.Threading;
using MySql.Data.MySqlClient;
using DevExpress.XtraSplashScreen;

namespace AssetMS
{
    public partial class newUserLogin : DevExpress.XtraEditors.XtraForm
    {
        [System.Runtime.InteropServices.DllImport("advapi32.dll")]

        public static extern bool LogonUser(string userName, string domainName, string password, int LogonType, int LogonProvider, ref IntPtr phToken);

        public newUserLogin()
        {
            InitializeComponent();
        }
        public string GetloggedinUserName()//get the current logged in user details.
        {

            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            return currentUser.Name;
        }

        public bool IsValidateCredentials(string userName, string password, string domain)
        {
            IntPtr tokenHandler = IntPtr.Zero;
            bool isValid = LogonUser(userName, domain, password, 2, 0, ref tokenHandler);
            return isValid;
        }
        private void separatorControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
             userLoginn();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void userLogin_Load(object sender, EventArgs e)
        {
            Thread.Sleep(8000);
        }

        private void txt_username_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txt_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                userLoginn();
            }
        }
        private void userLoginn()
        {
            if (txt_username.Text.Trim().Equals(""))
            {
                XtraMessageBox.Show("Kindly input username", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_password.Text.Trim().Equals(""))
            {
                XtraMessageBox.Show("Kindly input password", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string username = txt_username.Text.Trim();
            string password = EnCryptDecrypt.Encrypt(txt_password.Text.Trim(), true);

             try
                {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlDataAdapter adpata = new MySqlDataAdapter("SELECT Role, Name FROM profilemaster WHERE UserId = '" + username + "' AND Password = '" + password + "' ", conn);
                    DataTable dt = new DataTable();
                    adpata.Fill(dt);

                    if (IsValidateCredentials(username, txt_password.Text.Trim(), "MyDomain").Equals(true))
                    {
                        string role = "worker";
                        admin adm = new admin(role, username);
                        this.Hide();
                        adm.setUser(false);
                        SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
                        SplashScreenManager.Default.SetWaitFormCaption("Welcome...");

                        for (int i = 0; i < 100; i++)
                        {
                            Thread.Sleep(15);
                        }
                        SplashScreenManager.CloseForm();
                        adm.Show();
                    }
                else if (dt.Rows.Count == 1)
                {
                    string role = dt.Rows[0][0].ToString();
                    string usersaname = dt.Rows[0][1].ToString();

                    admin adm = new admin(role, usersaname);


                    if (role.Equals("Administrator"))
                    {
                        this.Hide();
                        adm.setAdminActions(false);
                        adm.disable();
                        SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
                        SplashScreenManager.Default.SetWaitFormCaption("Welcome...");

                        for (int i = 0; i < 100; i++)
                        {
                            Thread.Sleep(15);
                        }
                        SplashScreenManager.CloseForm();
                        adm.Show();

                        //adm.Show();

                    }
                    else if (role.Equals("User"))
                    {
                        this.Hide();
                        // adm.Show();
                        adm.setUser(false);
                        SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
                        SplashScreenManager.Default.SetWaitFormCaption("Welcome...");

                        for (int i = 0; i < 100; i++)
                        {
                            Thread.Sleep(15);
                        }
                        SplashScreenManager.CloseForm();
                        adm.Show();


                    }

                }
                else
                    {
                        XtraMessageBox.Show("Wrong password or Username", "Authentication failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    conn.Close();
                }
                catch (Exception es)
                {
                    throw es;

                }
            }
        

    }
}