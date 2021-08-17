using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.Skins;
using DevExpress.XtraNavBar;
using DevExpress.XtraSplashScreen;
using System.Threading;
using MySql.Data.MySqlClient;
using DevExpress.XtraEditors.Controls;

namespace AssetMS
{
    public partial class adminPanel : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public String currentUser;
        public String selectedDepartment;
       // DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        // public string userRole;

        public adminPanel()
        {
            InitializeComponent();
        }
        public adminPanel(string role, string user)
        {
            InitializeComponent();
            currentUser = user;
          //  userRole = role;
            barStaticItem1.Caption = "Current User: " + currentUser;
          //  newDpartment frm = new newDpartment();
          //  frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);

        }
        private Icon ico;
        public void setNormalUserActions(bool offOn)
        {
            btn_save.Enabled = offOn;
            btn_saveclose.Enabled = offOn;
            ribbonPage4.Visible = offOn;
            
        }
        public bool DisableControls()
        {
            bool condition = false;

            if (this.MdiChildren.Length > 1)
            {
               // MessageBox.Show("Windows Still Open");

                return condition = true;
            }
            return condition;
        }
        public void setAdminActions(bool offOn)
        {
            btn_save.Enabled = offOn;
            btn_saveclose.Enabled = offOn;
            timer.Start();
        }
        public void CallCustomMethod()
        {

            //if (MdiChildren.Count() == 0)
            //{
            //    MessageBox.Show("All windows Closed");

            //    btn_save.Enabled = false;
            //    this.btn_saveclose.Enabled = false;
            //    btn_save.Enabled = false;

            //}
            foreach (Form f in this.MdiChildren)
            {
                //do whatever you want with the mdi child form
                //if (isFormActive(f))
                //{
                //    MessageBox.Show("All windows Closed");
                //}
                ////if (!MdiChildren.Any())
                ////{
                ////    // all child forms closed
                ////    btn_saveclose.Enabled = false;
                ////    btn_save.Enabled = false;
                ////}
                ////else
                ////{
                ////    //MessageBox.Show("All windows Closed");
                ////}
                //////foreach (Form frm in this.MdiChildren)
                //////{
                //////    if (frm == null)
                //////    {
                //////        MessageBox.Show("All windows Closed");
                //////    }
                //////}
                    
                    
            }
            if (this.MdiChildren.Length == 1)
            {
                btn_saveclose.Enabled = false;
                btn_save.Enabled = false;
            }
        }
        

        void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Call your method here.
            CallCustomMethod();
        }
        private void RibbonForm1_Load(object sender, EventArgs e)
        {
            dockPanel3.Hide();
            ///   InternetAvailability.InternetGetConnectedState();


            //// barButtonItem3.Enabled = false;
            //btn_save.Enabled = false;
            //ribbonPage1.Visible = false;
            //ribbonPage2.Visible = false;
            //ribbonPage3.Visible = false;
            //ribbonPage4.Visible = false;
            ico = notifyIcon1.Icon;

            
            //notifyIcon1.BalloonTipText = currentUser+ ", Welcome to West Pokot County Asset Management Software ";
            //notifyIcon1.ShowBalloonTip(2000);
            //auth();

            timer.Interval = (10 * 1000); // 10 secs
            timer.Tick += new EventHandler(timer_Tick);
            

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            //refresh here...
            getNot();
           // timer.Stop();
        }
        private void openForm(Form frm)
        {
            
            if (!isFormActive(frm))
            {
                frm.MdiParent = this;
                frm.Show();
            }
            else
            {
                frm.Activate();
            }
        }
        private bool isFormActive(Form frm)
        {
            bool isOpened = false;
            if(MdiChildren.Count()> 0)
            {
                foreach(var item in MdiChildren)
                {
                    if (frm.Name == item.Name)
                    {
                        xtraTabbedMdiManager1.Pages[item].MdiChild.Activate();
                        isOpened = true;
                    }
                   
                }
            }
            return isOpened;
        }
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            
        }

        private void dockPanel3_Click(object sender, EventArgs e)
        {

        }

        private void navBarControl3_Click(object sender, EventArgs e)
        {
            
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            viewAllItems vi = new viewAllItems();
           
            //vi.Name = "View Items";
            openForm(vi);
            // Determine the active child form.
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                }
                else
                {
                  //  MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
           
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
           
        }

        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            
        }

        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            
        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

        }

        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            
        }

        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            
        }

        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            inventory p = new inventory();
            p.Name = "Printers";
            openForm(p);
        }

        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            
        }

        private void navBarItem9_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Determine the active child form.
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).SaveAction();
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
            else {
                //MessageBox.Show(selectedDepartment);
            }
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Determine the active child form.
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                activeChild.Close();
            }
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            inventory vi = new inventory(currentUser);
            vi.Name = "Take Inventory";
            openForm(vi);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    btn_save.Enabled = true;
                    btn_saveclose.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
        }

        private void RibbonForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DisableControls().Equals(true))
            {

            }
            else
            {
                DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to exit the system?", "System Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dlr == DialogResult.Yes)
                {
                    //Application.Exit();

                }
                else if (dlr == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void adminPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void barButtonItem20_ItemClick_1(object sender, ItemClickEventArgs e)
        {
           
        }

        private void barButtonItem23_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
          //DevExpress.LookAndFeel.UserLookAndFeel.Default.SetOffice2003Style
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void barButtonItem25_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).SaveAction();
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).SaveAction();
                    activeChild.Close();
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
        }

        private void createNavGroup()
        {
           // navBarControl3.
           // group1.Name = "New Dept";

        }
        private void createNavGroupItem()
        {

        }

        private void navBarItem1_LinkClicked_1(object sender, NavBarLinkEventArgs e)
        {
            btn_save.Enabled = true;
            selectedDepartment = navBarItem1.Caption;
            
        }

        private void barButtonItem26_ItemClick(object sender, ItemClickEventArgs e)
        {
            dockPanel1.Show();
            dockPanel3.Show();
        }

        private void dockPanel1_Click(object sender, EventArgs e)
        {

        }

        private void navBarItem4_LinkClicked_1(object sender, NavBarLinkEventArgs e)
        {
           
        }

        private void navBarItem5_LinkClicked_1(object sender, NavBarLinkEventArgs e)
        {
            
            
        }

        private void barButtonItem27_ItemClick(object sender, ItemClickEventArgs e)
        {
            //splashy
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newCategory u = new newCategory(currentUser);
            u.Name = "Add Category";
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    btn_save.Enabled = true;
                    btn_saveclose.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
        }

        private void barButtonItem23_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            //splashy
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            NewOffice u = new NewOffice(currentUser);
            u.Name = "Add Office";
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    btn_save.Enabled = true;
                    btn_saveclose.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
        }

        private void barButtonItem28_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            //splashy
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for(int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            
            SplashScreenManager.CloseForm();
            //
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    btn_save.Enabled = true;
                    btn_saveclose.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            //splashy
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            AssignItem u = new AssignItem(currentUser);
            u.Name = "Asign Item";
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    btn_save.Enabled = true;
                    btn_saveclose.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
        }

        private void barButtonItem15_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            string sourceDir = Application.StartupPath;
            string backupDir = "D:\\WPAMS Backup\\";

            this.Cursor = Cursors.WaitCursor;
            try
            {
                string[] db = Directory.GetFiles(sourceDir, "*.mdb");

                if (!Directory.Exists("D:\\WPAMS Backup\\"))
                {
                    Directory.CreateDirectory("D:\\WPAMS Backup\\");
                }

                // Copy picture files.
                foreach (string f in db)
                {
                    // Remove path from the file name.
                    string fName = f.Substring(sourceDir.Length + 1);
                    string bfName = DateTime.Today.Day.ToString() + "_" + DateTime.Today.Month.ToString() + "_" +
                        DateTime.Today.Year.ToString() + "_" + fName;

                    // Use the Path.Combine method to safely append the file name to the path.
                    // Will overwrite if the destination file already exists.
                    File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDir, bfName), true);
                }

                this.Cursor = Cursors.Arrow;
                DevExpress.XtraEditors.XtraMessageBox.Show("Backup done successfully.", "West Pokot County Asset Management Software");

            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "West Pokot County Asset Management Software", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
 
            String ext = "Pdf File (.pdf)|*.pdf";
            // Determine the active child form.
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                  //  ((IChildSave)this.ActiveMdiChild).Xport(ext);
                    //method shoul return a name
                    ///load pdf viewer
                    String path = ((IChildSave)this.ActiveMdiChild).Xport(ext);
                    pdfviewer u = new pdfviewer(path);
                    //openForm(u);
                    u.MdiParent = this;
                    u.Show();
                }
                
            }
           
            

        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            String ext = "Excel (2003)(.xls)|*.xls";
            // Determine the active child form.
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).Xport(ext);
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
            else
            {
                //MessageBox.Show(selectedDepartment);
            }
        }

        private void barButtonItem22_ItemClick(object sender, ItemClickEventArgs e)
        {
            String ext = "Excel (2010) (.xlsx)|*.xlsx";
            // Determine the active child form.
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).Xport(ext);
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
            else
            {
                //MessageBox.Show(selectedDepartment);
            }
        }

        private void navBarItem6_LinkClicked_1(object sender, NavBarLinkEventArgs e)
        {
            
           
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).updatethis();
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
            else
            {
                //MessageBox.Show(selectedDepartment);
            }
        }

        private void barButtonItem31_ItemClick(object sender, ItemClickEventArgs e)
        {
          
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).deleteThis();
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
            else
            {
                MessageBox.Show(selectedDepartment);
            }
        }

        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            //splashy
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
           
            SplashScreenManager.CloseForm();
            //
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    btn_exportExcelxls.Enabled = true;
                    btn_exportExcelxlsx.Enabled = true;
                    btn_exportPDF.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
        }

        private void barButtonItem3_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            openOnScreenKeyboard();
        }
        private static void killOnScreenKeyboard()
        {
            if (System.Diagnostics.Process.GetProcessesByName("TabTip").Count() > 0)
            {
                System.Diagnostics.Process asd = System.Diagnostics.Process.GetProcessesByName("TabTip").First();
                asd.Kill();
            }

        }

        private static void openOnScreenKeyboard()
        {
            System.Diagnostics.Process.Start("C:\\Program Files\\Common Files\\Microsoft shared\\ink\\TabTip.exe");

        }

        private void barButtonItem4_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).NavigateDown();
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
            else
            {
                //MessageBox.Show(selectedDepartment);
            }
        }

        private void barButtonItem11_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).NavigateUp();
                }
                else
                {
                    MessageBox.Show("Child Form does not implement IChildSave.");
                }
            }
            else
            {
                //MessageBox.Show(selectedDepartment);
            }
        }

        private void adminPanel_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void adminPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)       // Ctrl-S Save
            {
                MessageBox.Show("f");
                // Do what you want here
                e.SuppressKeyPress = true;  // Stops other controls on the form receiving event.
            }
        }

        private void adminPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //if (e.Control && e.KeyCode == Keys.S)
            //{
            //    MessageBox.Show("f");
            //    e.SuppressKeyPress = true;
            //}
        }

        private void barButtonItem22_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about a = new about();
            a.ShowDialog();
        }

        private void barButtonItem24_ItemClick(object sender, ItemClickEventArgs e)
        {
            string now = DateTime.Now.ToString("d/MM/yyyy");
            XtraMessageBox.Show(now);
        }

        private void barButtonItem29_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Hide();
        }

        private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            store u = new store();
            u.Name = "Asign Item";
            openForm(u);
        }

        private void barButtonItem32_ItemClick(object sender, ItemClickEventArgs e)
        {
           // browser u = new browser();
          //  u.Name = "Asign Item";
         //   openForm(u);
        }

        private void barButtonItem34_ItemClick(object sender, ItemClickEventArgs e)
        {
            loanItem i = new loanItem(currentUser);
            openForm(i);
        }

        private void barButtonItem35_ItemClick(object sender, ItemClickEventArgs e)
        {
            timer.Stop();
        }

        private void barButtonItem33_ItemClick(object sender, ItemClickEventArgs e)
        {
            systemInfo s = new systemInfo();
            openForm(s);
        }

        private void barButtonItem36_ItemClick(object sender, ItemClickEventArgs e)
        {
            UserControl myControl = new UserControl();
          //  myControl.a
            DevExpress.XtraEditors.XtraDialog.Show(myControl, "Sign in", MessageBoxButtons.OKCancel);

        }
        
        public void getNot()
        {
            ;

            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT `BorrowID`, `BorrowerName`, `BorrowerID`, `ItemSerialNumber`, `BorrowDate`, `ReturnDate`, datediff(ReturnDate, BorrowDate) as dueDays FROM borroweditems WHERE datediff(ReturnDate, BorrowDate) < 5 ", conn);
            MySqlDataReader reader;
            try
            {

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //timer.Start();
                    long result;
                    
                    DateTime Return = Convert.ToDateTime(reader["ReturnDate"]);
                    DateTime AssignDate = DateTime.Now;
                    result = loanItem.DateTimeUtil.DateDiff(loanItem.DateInterval.Day, AssignDate, Return);
                    string user = reader.GetString(1);
                    string id = reader.GetString(0);
                    string serialnum = reader.GetString(2);
                    string borrowedOn = reader.GetString(3);
                    string returnDate = reader.GetString(4);
                    // alertControl1.Show(this, "Pending Item", days );
                    int NumOfDays = (int)result;
                    
                    if (NumOfDays > 0)
                    {
                        int da = System.Math.Abs(NumOfDays);

                        alertControl1.Show(this, "Pending Item", user + " ,has a pending item, serial number " + serialnum + ", to be returned on " + returnDate+", "+ NumOfDays + " day(s) remaining.");

                    }
                    else
                    {//converrt negative value to positive value 
                        int da = System.Math.Abs(NumOfDays);
                        alertControl1.Show(this, "Pending Item", user + " ,has a pending item, serial number " + serialnum + ", that was to be returned on " + returnDate+", "+ da + " days ago");

                    }

                }
                timer.Stop();
            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());
            }
           
        }

        private void barButtonItem37_ItemClick(object sender, ItemClickEventArgs e)
        {
         
        }

        private void barButtonItem38_ItemClick(object sender, ItemClickEventArgs e)
        {
            //XtraMessageBox.Show();
         //   InternetAvailability.IsInternetAvailable().Equals(true);
            if (InternetAvailability.IsInternetAvailable().Equals(true))
            {
                XtraMessageBox.Show("Kuna Net");

            }
            else
            {
                XtraMessageBox.Show("HaKuna Net");
            }
        }

        private void alertControl1_AlertClick(object sender, DevExpress.XtraBars.Alerter.AlertClickEventArgs e)
        {
            allLoanedItems l = new allLoanedItems();
            openForm(l);
        }

        private void barButtonItem42_ItemClick(object sender, ItemClickEventArgs e)
        {
            allQueries l = new allQueries();
            openForm(l);
        }
    }
}