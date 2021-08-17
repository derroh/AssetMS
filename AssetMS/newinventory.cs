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
    public partial class newinventory : DevExpress.XtraEditors.XtraForm, IChildSave
    {
        private string currentUser, annotation;
        
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
        public newinventory()
        {
            InitializeComponent();
            FillComboOFfice();
            FillComboDept();
            getInventoryNumber();
        }
        public newinventory(string ActiveUser)
        {
            InitializeComponent();
            currentUser = ActiveUser;
            FillComboOFfice();
            FillComboDept();
            getInventoryNumber();
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            StatusList();
            ManufacturerList();
            PrinterNamesList();
            AgeList();
          //  picBoxPhoto.Focus();
        }
        public void SaveAction()
        {
            validate();
            string now = DateTime.Now.ToString("G");
            string nowDate = DateTime.Now.ToString("Y");
            string Age, inventoryNumber, userDept, Productname, SerialNumber, Manufacturer, location, status, whenFailed, InventoryBy;
            inventoryNumber = annotation;
            userDept = combo_department.Text;
            Productname = txt_productName.Text;
            SerialNumber = txt_serialNum.Text;
            Manufacturer = txt_manufacturer.Text;
            location = txt_loc.Text;
            status = txt_status.Text;
            whenFailed = dateEdit1.Text;
            InventoryBy = currentUser;
            Age = txt_age.Text;
            newItemClearance f = new newItemClearance();
           // newAssignItem g = new newAssignItem();
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist", "Invalid operation");
                return;
            }
            if (f.checkIflost().Equals(true))
            {
                XtraMessageBox.Show("The item is listed as lost. Kindly remove it from the list of lost items first.", "Operation terminated", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("INSERT INTO inventorytable ( inventoryNumber, UserDepartment, Productname, SerialNumber, Manufacturer, location, status, WhenItemFailed, InventoryBy, inventoryDate) VALUES (@inventoryNumber,@UserDepartment,@Productname,@SerialNumber,@Manufacturer,@location,@status,@WhenItemFailed,@InventoryBy,@inventoryDate)", conn))
                {
                    command.Parameters.AddWithValue("@inventoryNumber", inventoryNumber);
                    command.Parameters.AddWithValue("@UserDepartment", userDept);
                    command.Parameters.AddWithValue("@Productname", Productname);
                    command.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                    command.Parameters.AddWithValue("@Manufacturer", Manufacturer);
                    command.Parameters.AddWithValue("@location", location.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@WhenItemFailed", whenFailed);
                    command.Parameters.AddWithValue("@InventoryBy", currentUser);
                    command.Parameters.AddWithValue("@inventoryDate", nowDate.ToUpper());
                    command.ExecuteNonQuery();
                    using (MySqlCommand command2 = new MySqlCommand("UPDATE tblnewitem SET Warranty=@Warranty,Status=@Status, Age=@Age WHERE SerialNumber = @SerialNumber", conn))
                    {
                        command2.Parameters.AddWithValue("@status", status);
                        command2.Parameters.AddWithValue("@Age", Age);
                        command2.Parameters.AddWithValue("@Warranty", txt_warranty.Text);
                        command2.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                        command2.ExecuteNonQuery();
                        using (MySqlCommand command4 = new MySqlCommand("UPDATE assigneditems SET ItemCondition=@ItemCondition WHERE SerialNumber = @SerialNumber", conn))
                        {
                            command4.Parameters.AddWithValue("@ItemCondition", status);
                            command4.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                            command4.ExecuteNonQuery();
                            using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                            {
                                command3.Parameters.AddWithValue("@Activity", "Took an inventory in the " + userDept + ", for an item serial number " + SerialNumber);
                                command3.Parameters.AddWithValue("@User", currentUser);
                                command3.Parameters.AddWithValue("@Time", now);
                                command3.ExecuteNonQuery();
                                XtraMessageBox.Show("Inventory taken successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                reset();
                                
                            }
                            conn.Close();
                        }
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
            txt_serialNum.Text = "";
            txt_productName.Text = "";
            txt_manufacturer.Text = "";
            txt_value.Text = "";
            txt_loc.Text = "";
            txt_status.Text = "";
            txt_age.Text = "";
            combo_department.Text = "";
            txt_warranty.Text = "";
            combo_office.Text = "";
            picBoxPhoto.Image = null;
            dateEdit1.Text = "";
            getInventoryNumber();
        }

        public string Xport(string ext)
        {
            string x = "";
            return x;
        }

        public void updatethis()
        {
            if (ifInventoryExists().Equals(false))
            {
                XtraMessageBox.Show("The inventory number entered does not exist", "Invalid operation");
                return;
            }
            validate();
            string Age, inventoryNumber, userDept, Productname, SerialNumber, Manufacturer, location, status, whenFailed;
            inventoryNumber = txt_invNum.Text;
            userDept = combo_department.Text;
            Productname = txt_productName.Text;
            SerialNumber = txt_serialNum.Text;
            Manufacturer = txt_manufacturer.Text;
            location = txt_loc.Text;
            status = txt_status.Text;
            whenFailed = dateEdit1.Text;
            Age = txt_age.Text;
            try
            {
                string now = DateTime.Now.ToString("G");
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("UPDATE inventorytable SET UserDepartment=@UserDepartment,ProductName=@ProductName,SerialNumber=@SerialNumber,Manufacturer=@Manufacturer,Location=@Location,Status=@Status,WhenItemFailed=@WhenItemFailed WHERE InventoryNumber =@InventoryNumber", conn))
                {

                    command.Parameters.AddWithValue("@inventoryNumber", inventoryNumber);
                    command.Parameters.AddWithValue("@UserDepartment", userDept);
                    command.Parameters.AddWithValue("@Productname", Productname);
                    command.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                    command.Parameters.AddWithValue("@Manufacturer", Manufacturer);
                    command.Parameters.AddWithValue("@Location", location.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@WhenItemFailed", whenFailed);
                    command.ExecuteNonQuery();
                    using (MySqlCommand command2 = new MySqlCommand("UPDATE tblnewitem SET Warranty=@Warranty,Status=@Status, Age=@Age WHERE SerialNumber = @SerialNumber", conn))
                    {
                        command2.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@Age", Age);
                        command2.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                        command2.ExecuteNonQuery();
                        using (MySqlCommand command4 = new MySqlCommand("UPDATE assigneditems SET ItemCondition=@ItemCondition WHERE SerialNumber = @SerialNumber", conn))
                        {
                            command4.Parameters.AddWithValue("@ItemCondition", status);
                            command4.Parameters.AddWithValue("@SerialNumber", SerialNumber);
                            command4.ExecuteNonQuery();
                            using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                            {
                                command3.Parameters.AddWithValue("@Activity", "Updated inventory data for an item serial number " + SerialNumber);
                                command3.Parameters.AddWithValue("@User", currentUser);
                                command3.Parameters.AddWithValue("@Time", now);
                                command3.ExecuteNonQuery();
                                XtraMessageBox.Show("Inventory details updated successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                reset();
                                conn.Close();
                            }
                        }

                    }
                }

                conn.Close();
            }
            catch (MySqlException es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }

        public void deleteThis()
        {
            if (txt_invNum.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide an inventory number", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ifInventoryExists().Equals(false))
            {
                XtraMessageBox.Show("The inventory number entered does not exist", "Invalid operation");
                return;
            }
            DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to delete the selected item from the inventory list?", "System Confirmation", MessageBoxButtons.YesNo);

            if (dlr == DialogResult.Yes)
            {
                string now = DateTime.Now.ToString("G");
                try
                {
                    string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                    MySqlConnection conn = new MySqlConnection(conString);
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM inventorytable  WHERE inventoryNumber = @inventoryNumber", conn))
                    {

                        command.Parameters.AddWithValue("@inventoryNumber", txt_invNum.Text);
                        command.ExecuteNonQuery();
                        using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                        {
                            command3.Parameters.AddWithValue("@Activity", "Deleted inventory data for an item serial number " + txt_serialNum.Text);
                            command3.Parameters.AddWithValue("@User", currentUser);
                            command3.Parameters.AddWithValue("@Time", now);
                            command3.ExecuteNonQuery();
                            XtraMessageBox.Show("Inventory record deleted successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reset();
                            conn.Close();
                        }
                        // fillGrid();
                    }
                    //refresh table

                }
                catch (MySqlException es)
                {
                    XtraMessageBox.Show(es.ToString());
                }

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
        private void validate()
        {
            if (txt_invNum.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide an inventory number first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_serialNum.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a serial number first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (combo_department.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a user department first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_productName.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a product name first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (txt_manufacturer.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a manufacturer name first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_loc.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide a user incharge of the item first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_status.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly provide the status of the item first", "Required field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void ManufacturerList()
        {
            AutoCompleteStringCollection colValues = new AutoCompleteStringCollection();
            colValues.AddRange(new string[] { "Hewlett Packard(HP)", "Lenovo", "Apple", "Acer", "Toshiba", "Dell", "Sony", "Fujitsu ", "NEC", "Epson", "Panasonic", "Xerox ", "Kyocera", "Lexmark", "Samsung", "ABB", "Ablerex", "Active Power", "Acumentrics", "Aetes", "Alpha", "Alwayson" , "Amplimag", "APC", "Benning" , "Chloride" , "Clary", "Controlled Power" , "CP", "CS Eletro" , "Cyberex", "Energy Braz" , "Equisul GPL", "Falcon", "FUJI", "Gustav Klein", "Gutor", "IE Power", "IREM", "Jovy Atlas", "Log Master" , "LTI", "Marathon", "Minuteman", "Mitsubishi" , "NHS", "Piller" , "Powercom", "Powersun", "Power Kinetics", "Salicru", "Sanken", "Siel", "Socomec", "Solid State", "Sola" , "Staco", "Toshiba" , "Tripp Lite" , "TSI", "Yamabishi"
            });
            txt_manufacturer.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt_manufacturer.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_manufacturer.MaskBox.AutoCompleteCustomSource = colValues;
        }
        private void PrinterNamesList()
        {
            AutoCompleteStringCollection colValues = new AutoCompleteStringCollection();
            colValues.AddRange(new string[] {
                "HP LaserJet Pro 100 Color MFP M175nw","HP Color LaserJet Pro MFP M177fw","HP LaserJet Pro 200 Color MFP M276nw","HP LaserJet Pro 300 Color MFP M375nw","HP LaserJet Pro 400 Color MFP M475 Series","HP LaserJet Pro 500 color MFP M570dn","HP LaserJet Enterprise 500 color MFP M575 Series","HP LaserJet Enterprise Color Flow MFP M575c ","HP Color LaserJet Enterprise CM4540 MFP Series","HP LaserJet Enterprise 700 color MFP M775 Series","HP LaserJet CM6040f Color MFP","HP Color LaserJet Enterprise Flow M880 MFP Series ","HP LaserJet M1212nf MFP","HP LaserJet M1217nfw MFP","HP LaserJet Pro MFP M127 Series","HP LaserJet Pro M1536dnf MFP","HP LaserJet Pro 400 MFP M425dn","HP LaserJet Pro MFP M521dn","HP LaserJet Enterprise 500 MFP M525 Series","HP LaserJet Enterprise Flow MFP M525c","HP LaserJet Enterprise M4555 MFP Series","HP LaserJet Enterprise 700 MFP M725 Series","HP LaserJet M9050 MFP","HP LaserJet Enterprise Flow MFP M830z","HP LaserJet Pro 200 Color M251nw","HP LaserJet Pro 400 Color M451 Series","HP LaserJet Enterprise 500 Color M551 Series","HP LaserJet CP4025 Color printer Series","HP LaserJet CP4525 Color printer Series","HP LaserJet Pro CP5225 Color printer Series","HP Color LaserJet Enterprise M750 Series","HP Color LaserJet Enterprise M855 Series", "HP LaserJet Pro P1102w","HP LaserJet Pro P1606dn","HP LaserJet P2035","HP LaserJet Pro 400 M401 Series", "HP LaserJet P3015 Series","HP LaserJet Enterprise 600 M601 Series","HP LaserJet Enterprise 600 M602 Series","HP LaserJet Enterprise 600 M603 Series","HP LaserJet Enterprise 700 M712n Series","HP LaserJet Enterprise M806 Printer Series","HP Officejet 4630", "HP Officejet 6600","HP Officejet 6700 Premium","HP Officejet Pro 8600 Premium","HP Officejet Pro 8600 Plus","HP Officejet Pro 276dw MFP","HP Officejet Pro X476dn MFP","HP Officejet Pro X476dw MFP","HP Officejet Pro X576dw MFP", "HP Officejet 6100","HP Officejet Pro 8100","HP Officejet Pro 251dw", "HP Officejet Pro X451dn","HP Officejet Pro X451dw","HP Officejet Pro X551dw","HP Officejet 100", "HP Officejet 150", "HP Officejet 7110","HP Officejet 7610", "HP Deskjet 1510", "HP Deskjet 2540", "HP Envy 4500","HP Envy 5530","HP Photosmart 6520", "HP Photosmart 7520","HP Envy 120","HP Designjet T7100","HP Designjet T2500","HP Designjet T1500","HP Designjet T1300ps","HP Designjet T920","HP Designjet T790ps","HP Designjet T520","HP Designjet T120","HP Designjet Z6200","HP Designjet Z5400ps","HP Designjet Z5200ps","HP Designjet Z3200ps","HP Designjet Z2100", "HP Designjet T1200","HP Designjet HD Scanner","HP Latex 210","HP Latex 260", "HP Latex 280", "HP Scitex FB7600","HP Scitex TJ8600","HP Scitex TJ8350", "HP Scitex Xp5500","HP Latex 850","HP Scitex FB700","HP Scitex FB500","HP Latex 3000","HP Scitex FB10000","HP Indigo r5000","HP Indigo 3550","HP Indigo 5600", "HP Indigo 7600","HP Indigo 10000", "HP Indigo WS6000p", "HP Indigo W7250","HP Indigo WS4600","HP Indigo W6600p","HP Scanjet G3110","HP Scanjet G4050","HP Scanjet 5590","HP Scanjet N6310","HP Scanjet 1000","HP Scanjet Pro 3000 s2","HP Scanjet Enterprise flow 5000 s2","HP Scanjet 8270","HP Scanjet N6350","HP Scanjet Enterprise Flow 7000 s2","HP Scanjet Enterprise Flow 7500", "HP Digital Sender Flow 8500 fn1","HP Scanjet Enterprise Flow N9120", "HP Jetdirect Wireless print Servers","HP NFC/Wireless 1200w Mobile print Accessory","HP Jetdirect Internal Print Servers","HP Jetdirect 300x print Server"
                , "Uninterruptible Power Supply"
            });
            txt_productName.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt_productName.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_productName.MaskBox.AutoCompleteCustomSource = colValues;
            // "Samsung",
        }
        private void AgeList()
        {
            AutoCompleteStringCollection colValues = new AutoCompleteStringCollection();
            colValues.AddRange(new string[] { "1 year", "2 years", "3 years", "4 years", "5 years", "6 years", "7 years", "8 years ", "9 years", "10 years" });
            txt_age.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt_age.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_age.MaskBox.AutoCompleteCustomSource = colValues;
        }
        private void StatusList()
        {
            AutoCompleteStringCollection colValues = new AutoCompleteStringCollection();
            colValues.AddRange(new string[] { "Working", "Not Working", "Needs Maintenance" });
            txt_status.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt_status.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_status.MaskBox.AutoCompleteCustomSource = colValues;
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
                conn.Close();
            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());
            }

        }
        private void getUserAssigned()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from assigneditems where serialNumber='" + txt_invNum.Text.Trim() + "'", conn);
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
                combo_department.Text = dr2.GetString(2);
                txt_loc.Text = dr2.GetString(4);
            }
            conn.Close();
        }
        private void getInventoryNumber()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT COALESCE(MAX(inventoryId),0) as invent FROM inventorytable LIMIT 1", conn);
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

                    annotation = "WPC/EDU&ICT/" + annoo.ToString();
                    txt_invNum.Text = annotation;
                }
                conn.Close();
            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());
            }
        }

        private void txt_serialnum_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txt_invNum_TextChanged(object sender, EventArgs e)
        {

            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from inventorytable where InventoryNumber='" + txt_invNum.Text.Trim() + "'", conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txt_status.Text = dr.GetString(7);
                    combo_department.Text = dr.GetString(2);
                    txt_productName.Text = dr.GetString(3);
                    txt_serialNum.Text = dr.GetString(4);
                    txt_manufacturer.Text = dr.GetString(5);
                    txt_loc.Text = dr.GetString(6);
                    dateEdit1.Text = dr.GetString(8);

                }

                conn.Close();

            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());//
            }
        }

        private void txt_invNum_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txt_serialNum_TextChanged(object sender, EventArgs e)
        {
            string srn = txt_serialNum.Text.Trim();
            try
                {
                    string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                    MySqlConnection conn = new MySqlConnection(conString);
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblnewitem WHERE SerialNumber = '"+ txt_serialNum.Text + "' ", conn);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                    
                    
                    txt_productName.Text = dr.GetString(2);
                    txt_age.Text = dr.GetString(6);
                    txt_manufacturer.Text = dr.GetString(4);
                    txt_status.Text = dr.GetString(8);
                    txt_value.Text = dr.GetString(5);
                    //do your magic here
                    string warr = dr.GetString(7);
                    if (string.Equals(warr, "Expired"))

                    {
                        txt_warranty.Text = "Expired";
                    }

                    else

                    {
                        //get 
                        long result;

                        string itemCurrentAge = dr.GetString(6);

                        int y = itemCurrentAge[0];
                        int w = warr[0];

                        DateTime dateAdded = Convert.ToDateTime(dr["DateAdded"]);
                        DateTime currentDate = DateTime.Now;
                        result = newItemloan.DateTimeUtil.DateDiff(newItemloan.DateInterval.Year, dateAdded, currentDate);

                        int years = (int)result + Convert.ToInt32(itemCurrentAge.Substring(0, 1));
                        int warrantyValid = Convert.ToInt32(warr.Substring(0, 1)) - years;

                        if (warrantyValid > 0)
                        {
                            txt_warranty.Text = warrantyValid.ToString() + " years";
                        }
                        else
                        {
                            txt_warranty.Text = "Expired";
                        }

                        txt_age.Text = years.ToString() + " years";


                    }

                    byte[] img = (byte[])(dr["Photo"]);
                    if (img == null)
                        picBoxPhoto.Image = null;
                    else
                    {
                        MemoryStream mstream = new MemoryStream(img);
                        picBoxPhoto.Image = System.Drawing.Image.FromStream(mstream);
                     }
                    conn.Close();
                    conn.Open();

                    try
                    {
                        MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM assigneditems WHERE SerialNumber = '" + txt_serialNum.Text.Trim() + "'", conn);
                        MySqlDataReader dr2 = cmd2.ExecuteReader();
                        if (dr2.Read())
                        {
                            combo_department.Text = dr2.GetString(3);
                            combo_office.Text = dr2.GetString(4);
                            txt_loc.Text = dr2.GetString(5);

                        }
                    }
                    catch (MySqlException es)
                    {
                        XtraMessageBox.Show(es.ToString());
                    }
                }
                 conn.Close();

                }
                catch (MySqlException es)
                {
                    XtraMessageBox.Show(es.ToString());
                }
           // }
        }
        private bool ifInventoryExists()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from inventorytable where inventoryNumber='" + txt_invNum.Text.Trim() + "'", conn);
            MySqlDataReader dr2 = cmd2.ExecuteReader();
            if (dr2.Read())
            {
                return true;
            }
            conn.Close();
            return false;
        }
        public bool ifExists()
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