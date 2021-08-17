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
using DevExpress.XtraSplashScreen;
using System.Threading;
using MySql.Data.MySqlClient;
using System.IO;
using System.IO.Compression;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;

namespace AssetMS
{
    interface IChildSave
    {
        void SaveAction();
        void reset();
       // void Xport(String ext);
        void deleteThis();
        void updatethis();
        void NavigateDown();
        void NavigateUp();
        string Xport(string ext);
    }
    public partial class admin : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public string currentUser;
        private Icon ico;

        static string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
        MySqlConnection conn = new MySqlConnection(conString);

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
       
        public admin()
        {
            InitializeComponent();
        }
        public admin(string role, string user)
        {
            InitializeComponent();
            currentUser = user;
            barStaticItem1.Caption = "Current User: " + currentUser;

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            //refresh here...
            getNot();
            // timer.Stop();
        }
        public void closeAllChildren()
        {
            foreach (Form frm in this.MdiChildren)
            {
                frm.Close();
            }
        }
        private void admin_Load(object sender, EventArgs e)
        {
            
            ico = notifyIcon1.Icon;
            timer.Interval = (10 * 1000); // 10 secs
            timer.Tick += new EventHandler(timer_Tick);
            
            if (InternetAvailability.IsInternetAvailable().Equals(true))
            {
                barStaticItem2.Caption ="Internet connection available";

            }
            else
            {
                barStaticItem2.Caption = " No internet connection available";
            }
           // timer.Start();
        }
        public void setNormalUserActions(bool offOn)
        {
            btn_prodCategory.Enabled = offOn;
            btn_newItem.Enabled = offOn;
            btn_newOffice.Enabled = offOn;
            btn_newDepartment.Enabled = offOn;
            btn_assignUser.Enabled = offOn;
            btn_addUser.Enabled = offOn;
            btn_addUser.Enabled = offOn;
            btn_createBackUp.Enabled = offOn;
            btn_restoreDB.Enabled = offOn;
        }
        public void setAdminActions(bool offOn)
        {
            timer.Start();
        }
        public void setUser(bool offOn)
        {
            ribbonPage1.Visible = offOn;
            ribbonPageGroup6.Visible = offOn;
            ribbonPageGroup5.Visible = offOn;
            navBarControl1.Visible = offOn;
            barButtonItem16.Enabled = offOn;

        }
        public void DisableWithNoGrid(bool State)
        {
            btn_close.Enabled = State;
            btn_delete.Enabled = State;
            btn_update.Enabled = State;
            btn_saveClose.Enabled = State;
            btn_save.Enabled = State;
            barButtonItem17.Enabled = State;
        }
        public void DisableWithGrid(bool State)
        {
            btn_toPDo.Enabled = State;
            btn_toExel.Enabled = State;
            btn_moveDown.Enabled = State;
            btn_moveUp.Enabled = State;
            btn_refresh.Enabled = State;
        }
        public void disable()
        {
            btn_save.Enabled = false;
            btn_saveClose.Enabled = false;
            btn_toPDo.Enabled = false;
            btn_toExel.Enabled = false;
            btn_update.Enabled = false;
            btn_delete.Enabled = false;
            btn_moveDown.Enabled = false;
            btn_moveUp.Enabled = false;
            btn_refresh.Enabled = false;
            btn_close.Enabled = false;
            barButtonItem17.Enabled = false;
        }
        public void DisableControls(bool State)
        {
            if (this.MdiChildren.Length == 1)
            {
                btn_save.Enabled = State;
                btn_saveClose.Enabled = State;
                btn_toPDo.Enabled = State;
                btn_toExel.Enabled = State;
                btn_update.Enabled = State;
                btn_delete.Enabled = State;
                btn_moveDown.Enabled = State;
                btn_moveUp.Enabled = State;
                btn_refresh.Enabled = State;
                btn_close.Enabled = State;
            }
        }
        private void openForm(Form frm)
        {

            if (!isFormActive(frm))
            {
                frm.MdiParent = this;
                frm.Show();
            }
        }
        private bool isFormActive(Form frm)
        {
            bool isOpened = false;
            if (MdiChildren.Count() > 0)
            {
                foreach (var item in MdiChildren)
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

        private void btn_newItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newItem))
                {
                    form.Activate();
                   // System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newItem vi = new newItem(currentUser);
            vi.Name = "Add New Item";
            openForm(vi);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    DisableWithNoGrid(true);
                }
               
            }
        }

        private void btn_prodCategory_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newCategory))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           
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
                    DisableWithNoGrid(true);
                }
                
            }
        }

        private void btn_newOffice_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newOffice))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newOffice u = new newOffice(currentUser);
            u.Name = "Add Office";
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);
                }
                
            }
        }

        private void btn_newDepartment_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newDepartment))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            newDepartment u = new newDepartment(currentUser);
            u.Name = "Add Department";
            openForm(u);
            SplashScreenManager.CloseForm();
            //
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);
                }
                
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btn_moveUp_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).NavigateDown();
                }
                
            }
            
        }

        private void btn_moveDown_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).NavigateUp();
                }
                
            }
           
        }

        private void btn_keyboard_ItemClick(object sender, ItemClickEventArgs e)
        {
            openOnScreenKeyboard();
        }
        private static void openOnScreenKeyboard()
        {
            System.Diagnostics.Process.Start("C:\\Program Files\\Common Files\\Microsoft shared\\ink\\TabTip.exe");

        }

        private void btn_saveClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).SaveAction();
                    activeChild.Close();
                }
               
            }
        }

        private void btn_close_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;
            if (activeChild != null)
            {
                activeChild.Close();
            }
        }

        private void btn_save_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).SaveAction();
                }
                
            }
           
        }

        private void btn_update_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).updatethis();
                }
                
            }
            
        }

        private void btn_delete_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).deleteThis();
                }
                
            }
            
        }

        private void btn_createBackUp_ItemClick(object sender, ItemClickEventArgs e)
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";

            try
            {
                SaveFileDialog f = new SaveFileDialog();
                f.Filter = "Zip|*.zip";
                f.FileName = "ZipTest " + DateTime.Now.ToString("yyyyMMdd HHmmss") + ".zip";
                if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                string zipFilePath = f.FileName;
                string zipFileName = "SqlDump.sql";

                using (MemoryStream ms = new MemoryStream())
                {
                    using (TextWriter tw = new StreamWriter(ms, new UTF8Encoding(false)))
                    {
                        using (MySqlConnection conn = new MySqlConnection(conString))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                using (MySqlBackup mb = new MySqlBackup(cmd))
                                {
                                    cmd.Connection = conn;
                                    conn.Open();

                                    mb.ExportToTextWriter(tw);
                                    conn.Close();

                                    using (ZipStorer zip = ZipStorer.Create(zipFilePath, "MySQL Dump"))
                                    {
                                        ms.Position = 0;
                                        zip.AddStream(ZipStorer.Compression.Deflate, zipFileName, ms, DateTime.Now, "MySQL Dump");
                                    }
                                }
                            }
                        }
                    }
                }

                XtraMessageBox.Show("Done.");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.ToString());
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";

            try
            {
                OpenFileDialog of = new OpenFileDialog();
                of.Filter = "Zip|*.zip";
                of.Title = "Select the Zip file";
                of.Multiselect = false;
                if (of.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                string zipfile = of.FileName;

                FolderBrowserDialog f = new FolderBrowserDialog();
                f.Description = "Extract the dump file to which folder?";
                if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                string folder = f.SelectedPath;
                string dumpFile = "";

                using (ZipStorer zip = ZipStorer.Open(zipfile, FileAccess.Read))
                {
                    List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();
                    dumpFile = folder + "\\" + dir[0].FilenameInZip;
                    zip.ExtractFile(dir[0], dumpFile);
                }

                using (MySqlConnection conn = new MySqlConnection(conString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();

                            mb.ImportFromFile(dumpFile);

                            conn.Close();
                        }
                    }
                }

                XtraMessageBox.Show("Finished.");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.ToString());
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about a = new about();
            a.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void btn_about_ItemClick(object sender, ItemClickEventArgs e)
        {
            about a = new about();
            a.ShowDialog();
        }

        private void btn_contact_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraMessageBox.Show("Welcome To West Pokot County Asset Management Software.\nContact me - 0701964636", "About the developer", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void admin_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to exit the system? Any information not saved will be lost", "System Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dlr == DialogResult.Yes)
            {
               closeAllChildren();

            }
            else if (dlr == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(viewAllItems))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            viewAllItems vi = new viewAllItems();
            openForm(vi);
            SplashScreenManager.CloseForm();
            // Determine the active child form.
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }
                
            }
        }

        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newAssignItem))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newAssignItem u = new newAssignItem(currentUser);
            u.Name = "Asign Item";
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);
                }
                
            }
        }

        private void navBarItem13_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(all_Departments))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
          //  SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            all_Departments u = new all_Departments();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }

            }
        }

        private void btn_toPDF_ItemClick(object sender, ItemClickEventArgs e)
        {
           String ext = "Pdf File (.pdf)|*.pdf";
           Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    String path = ((IChildSave)this.ActiveMdiChild).Xport(ext);
                    pdfviewer u = new pdfviewer(path);
                    u.MdiParent = this;
                    u.Show();
                }

            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            String ext = "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx";
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).Xport(ext);
                }
                
            }
           
        }

        private void navBarItem12_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(all_Offices))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           // SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            all_Offices u = new all_Offices();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }

            }
        }

        private void navBarItem14_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(all_users))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            all_users u = new all_users();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }

            }
        }

        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(all_inventories))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
          //  SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            all_inventories u = new all_inventories();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }

            }
        }

        private void navBarItem10_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
           
        }

        private void btn_find_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(viewAllItems))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            viewAllItems vi = new viewAllItems();
            openForm(vi);
            SplashScreenManager.CloseForm();
            // Determine the active child form.
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                }

            }
        }

        private void barButtonItem4_ItemClick_1(object sender, ItemClickEventArgs e)
        {

            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Just a moment...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            systemInfo s = new systemInfo("CPU Information", "Win32_Processor");
            s.DeviceInformation("Win32_Processor");
            s.MdiParent = this;
            s.Show();
            SplashScreenManager.CloseForm();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Just a moment...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            systemInfo s = new systemInfo("System Information");
           // s.SystemInformation();
            s.MdiParent = this;
            s.Show();
            SplashScreenManager.CloseForm();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Just a moment...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            systemInfo s = new systemInfo("Local Drives Information", "Win32_LogicalDisk");
           // s.DeviceInformation("Win32_LogicalDisk");
            s.MdiParent = this;
            s.Show();
            SplashScreenManager.CloseForm();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Just a moment...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            systemInfo s = new systemInfo("Video Controller Information", "Win32_VideoController");
           // s.DeviceInformation("Win32_VideoController");
            s.MdiParent = this;
            s.Show();
            SplashScreenManager.CloseForm();
        }

        private void btn_assignUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newAssignItem))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newAssignItem u = new newAssignItem(currentUser);
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);

                }

            }
        }

        private void alertControl1_AlertClick(object sender, DevExpress.XtraBars.Alerter.AlertClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(allLoanedItems))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            allLoanedItems u = new allLoanedItems();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();

                }

            }
        }
        public void getNot()
        {
            conn.Open();
            
            MySqlCommand command = new MySqlCommand("SELECT `BorrowID`, `BorrowerName`, `BorrowerID`, `ItemSerialNumber`, `BorrowDate`, `ReturnDate`, datediff(ReturnDate, BorrowDate) as dueDays FROM borroweditems WHERE datediff(ReturnDate, BorrowDate) < 5 ", conn);
            MySqlDataReader reader;
            try
            {

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //timer.Start();
                  //  MessageBox.Show("dd");
                    long result;

                    DateTime Return = Convert.ToDateTime(reader["ReturnDate"]);
                    DateTime AssignDate = DateTime.Now;
                    result = newItemloan.DateTimeUtil.DateDiff(newItemloan.DateInterval.Day, AssignDate, Return);

                    string user = reader.GetString(1);
                    string id = reader.GetString(0);
                    string serialnum = reader.GetString(2);
                    string borrowedOn = reader.GetString(3);
                    string returnDate = reader.GetString(4);
                    // alertControl1.Show(this, "Pending Item", days );
                    int NumOfDays = (int)result;
                   // MessageBox.Show(NumOfDays.ToString());
                    if (NumOfDays > 0)
                    {
                        int da = System.Math.Abs(NumOfDays);

                        alertControl1.Show(this, "Pending Item", user + " ,has a pending item, serial number " + serialnum + ", to be returned on " + returnDate + ", " + NumOfDays + " day(s) remaining.");

                    }
                    else
                    {//converrt negative value to positive value 
                        int da = System.Math.Abs(NumOfDays);
                        alertControl1.Show(this, "Pending Item", user + " ,has a pending item, serial number " + serialnum + ", that was to be returned on " + returnDate + ", " + da + " days ago");

                    }

                }
                timer.Stop();
            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());
            }

        }

        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(allLoanedItems))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            allLoanedItems u = new allLoanedItems();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }

            }
        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(allItemsCondition))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            allItemsCondition u = new allItemsCondition();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }

            }
        }

        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(allItemsCondition))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            allItemsCondition u = new allItemsCondition();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }

            }
        }

        private void navBarItem15_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(allCategories))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           // SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            allCategories u = new allCategories();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }

            }
        }

        private void navBarItem11_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(allAssigned))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           // SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            allAssigned u = new allAssigned();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }

            }
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newQuery))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newQuery u = new newQuery();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();

                }

            }
        }

        private void admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).SaveAction();
                }

            }
        }

        private void btn_addUser_ItemClick(object sender, ItemClickEventArgs e)
        {
           // newsystemUser u = new newsystemUser();
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newsystemUser))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newsystemUser u = new newsystemUser(currentUser);
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);

                }

            }
        }

        private void btn_userSettings_ItemClick(object sender, ItemClickEventArgs e)
        {
            // newsystemUser u = new newsystemUser();
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newsystemUser))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newsystemUser u = new newsystemUser(currentUser);
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);

                }

            }
        }

        private void btn_refresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
                    SplashScreenManager.Default.SetWaitFormCaption("Please wait");
                    SplashScreenManager.Default.SetWaitFormDescription("Refreshing...");
                    for (int i = 0; i < 100; i++)
                    {
                        Thread.Sleep(5);
                    }
                    SplashScreenManager.CloseForm();
                    ((IChildSave)this.ActiveMdiChild).updatethis();
                }

            }
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            Help.ShowHelp(this, "file://C:\\Users\\HiGHROLLER\\Documents\\Visual Studio 2015\\Projects\\AssetMS\\help.chm");
        }

        private void barButtonItem3_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newItemRepair))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newItemRepair u = new newItemRepair();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);
                }

            }
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(allQueries))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            allQueries u = new allQueries();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }

            }
        }

        private void navBarItem2_LinkClicked_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(allRepairRecords))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           // SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            allRepairRecords u = new allRepairRecords();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }

            }
        }

        private void btn_returnItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newItemClearance))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
  
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newItemClearance u = new newItemClearance(currentUser);
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);
                    btn_update.Enabled = false;
                    btn_delete.Enabled = false;
                }

            }
        }

        private void btn_store_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newItemStore))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newItemStore u = new newItemStore(currentUser);
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);
                    btn_delete.Enabled = false;
                    btn_update.Enabled = false;

                }

            }
        }

        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(allStoreItems))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
           
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            allStoreItems u = new allStoreItems(currentUser);
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);

                }

            }
        }

        private void barButtonItem10_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newItemloan))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newItemloan u = new newItemloan(currentUser);
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                   // ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);

                }

            }
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newinventory))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newinventory u = new newinventory(currentUser);
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);

                }

            }
        }

        private void navBarItem16_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(allunassigned))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            allunassigned u = new allunassigned();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);

                }

            }
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(transactionlog))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            // SplashScreenManager.Default.SetWaitFormCaption("Loading...");

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            transactionlog u = new transactionlog();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }

            }
        }

        private void navBarItem9_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            // Create a report. 
            inventoryReport report = new inventoryReport();

            // Show the report's preview. 
            ReportPrintTool tool = new ReportPrintTool(report);
            tool.ShowPreview();

            ////// Print the report. 
            ////ReportPrintTool tool = new ReportPrintTool(report);
            ////tool.Print();

            //////////ReportDesignTool tool = new ReportDesign
            //////////// Open the report in the End-User DesignerTool(report);
            //////////tool.ShowDesignerDialog();

        }


        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
           // getNot();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                }

            }
        }

        private void navBarItem10_LinkClicked_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(allQueries))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            allQueries u = new allQueries();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);

                }

            }
        }

        private void btn_lostItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newItemLost))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newItemLost u = new newItemLost(currentUser);
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);

                }

            }
        }

        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(all_Untraceable ))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            all_Untraceable u = new all_Untraceable();
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                    ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithGrid(true);
                }

            }
        }

        private void barButtonItem14_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                // If frmHome is Opened, set focus to it and exit subroutine.
                if (form.GetType() == typeof(newItemloan))
                {
                    form.Activate();
                    System.Media.SystemSounds.Exclamation.Play();
                    return;

                }
            }
            SplashScreenManager.ShowForm(this, typeof(wait_form), true, true, false);

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
            }
            //  SplashScreenManager.CloseForm();
            newItemloan u = new newItemloan(currentUser);
            openForm(u);
            SplashScreenManager.CloseForm();
            Form activeChild = this.ActiveMdiChild;

            if (activeChild != null)
            {
                if (this.ActiveMdiChild is IChildSave)
                {
                   // ((IChildSave)this.ActiveMdiChild).reset();
                    DisableWithNoGrid(true);

                }

            }
        }
    }
}