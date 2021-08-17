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
using System.IO;
using MySql.Data.MySqlClient;

namespace AssetMS
{
    public partial class newItem : DevExpress.XtraEditors.XtraForm, IChildSave
    {
        OpenFileDialog filePhoto = new OpenFileDialog();
        Image img = null;
        
        private string currentUser;
        interface IChildSave
        {
            void SaveAction();
            void reset();
            void updatethis();
            void deleteThis();
            void NavigateDown();
            void NavigateUp();
        }
        public newItem()
        {
            InitializeComponent();

            FillCombo();
        }
        public newItem(string ActiveUser)
        {
            InitializeComponent();
            currentUser = ActiveUser;
            FillCombo();
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

        public void SaveAction()
        {
            //get data
            string assetName = txt_ProductName.Text.Trim();
            string assetManufacturer = txt_Manufacturer.Text.Trim();
            string assetAge = txt_Age.Text.Trim();
            string assetSerialNum = txt_SerialNumber.Text.Trim();
            string assetValue = txt_Value.Text.Trim();
            string assetCategory = combo_category.Text.Trim();
            string assetCondition = txt_Status.Text.Trim();
            string warranty = txt_warranty.Text.Trim();
            string DateAdded = DateTime.Now.ToString("G");
            //validate first
            if (txt_SerialNumber.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's serial number first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_ProductName.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's name first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (combo_category.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly select item's category first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_Manufacturer.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's manufacturer name first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_Value.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's value first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_Age.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's age first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_Status.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's condition first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_warranty.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's warranty period first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (picBoxPhoto.Image.Equals(null))
            {
                XtraMessageBox.Show("Kindly upload the item's photo first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //save data
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("INSERT INTO tblNewItem (category, name, serialNumber,manufacturer, value, age, Warranty, status, Photo, AddedBy, DateAdded) values(@category, @name, @serialNumber, @manufacturer, @value, @age, @Warranty, @status, @Photo, @AddedBy, @DateAdded)", conn))
                {
                    command.Parameters.AddWithValue("@category", assetCategory);
                    command.Parameters.AddWithValue("@name", assetName);
                    command.Parameters.AddWithValue("@serialNumber", assetSerialNum);
                    command.Parameters.AddWithValue("@manufacturer", assetManufacturer.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@value", assetValue);
                    command.Parameters.AddWithValue("@age", assetAge.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@Warranty", warranty.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@status", assetCondition.ToTitleCase(TitleCase.All));
                    MemoryStream stream = new MemoryStream();
                    byte[] pic = ImageToByteArray(picBoxPhoto.Image);
                    command.Parameters.AddWithValue("@Photo", pic);
                    command.Parameters.AddWithValue("@AddedBy", currentUser);
                    command.Parameters.AddWithValue("@DateAdded", DateAdded);
                    command.ExecuteNonQuery();
                    using (MySqlCommand command2 = new MySqlCommand("INSERT INTO unassignedItems (serialNumber) values(@serialNumber)", conn))
                    {
                        command2.Parameters.AddWithValue("@serialNumber", assetSerialNum);
                        command2.ExecuteNonQuery();
                        using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                        {
                            command3.Parameters.AddWithValue("@Activity", "Added an item serial number " + assetSerialNum);
                            command3.Parameters.AddWithValue("@User", currentUser);
                            command3.Parameters.AddWithValue("@Time", DateAdded);
                            command3.ExecuteNonQuery();
                            XtraMessageBox.Show("Item Added Succesfully", "Action Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            reset();
                            conn.Close();
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

        public void reset()
        {
            txt_Age.Text = "";
            txt_Manufacturer.Text = "";
            txt_ProductName.Text = "";
            txt_SerialNumber.Text = "";
            txt_Value.Text = "";
            txt_Status.Text = "";
            combo_category.SelectedIndex = -1;
            txt_SerialNumber.Focus();
            txt_warranty.Text = "";
            picBoxPhoto.Image = null;
            txt_fname.Text = "";
        }

        public string Xport(string ext)
        {
            //  //throw new NotImplementedException();
            string x = "";
            return x;
        }

        public void updatethis()
        {
            //get data
            string assetName = txt_ProductName.Text.Trim();
            string assetManufacturer = txt_Manufacturer.Text.Trim();
            string assetAge = txt_Age.Text.Trim();
            string assetSerialNum = txt_SerialNumber.Text.Trim();
            string assetValue = txt_Value.Text.Trim();
            string assetCategory = combo_category.Text.Trim();
            string assetCondition = txt_Status.Text.Trim();
            string warranty = txt_warranty.Text.Trim();
            //validate first
            if (txt_SerialNumber.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's serial number first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_ProductName.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's name first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (combo_category.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly select item's category first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_Manufacturer.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's manufacturer name first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_Value.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's value first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_Age.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's age first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_Status.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's condition first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_warranty.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's warranty period first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (picBoxPhoto.Image.Equals(null))
            {
                XtraMessageBox.Show("Kindly upload the item's photo first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist", "Invalid operation");
                return;
            }
            //update data
            try
            {
                string DateAdded = DateTime.Now.ToString("G");
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("UPDATE officeDepartments SET category = @category, name = @name,  manufacturer = @manufacturer,  value = @value,  age = @age, Warranty = @Warranty,  status = @status,  Photo = @Photo WHERE serialNumber = @serialNumber", conn))
                {
                    command.Parameters.AddWithValue("@category", assetCategory.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@name", assetName);
                    command.Parameters.AddWithValue("@serialNumber", assetSerialNum);
                    command.Parameters.AddWithValue("@manufacturer", assetManufacturer.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@value", assetValue.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@age", assetAge.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@Warranty", warranty.ToTitleCase(TitleCase.All));
                    command.Parameters.AddWithValue("@status", assetCondition.ToTitleCase(TitleCase.All));
                    MemoryStream stream = new MemoryStream();
                    byte[] pic = ImageToByteArray(picBoxPhoto.Image);

                    command.Parameters.AddWithValue("@Photo", pic);
                    command.ExecuteNonQuery();
                    using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                    {
                        command3.Parameters.AddWithValue("@Activity", "Updated data for an item serial number " + assetSerialNum);
                        command3.Parameters.AddWithValue("@User", currentUser);
                        command3.Parameters.AddWithValue("@Time", DateAdded);
                        command3.ExecuteNonQuery();
                        XtraMessageBox.Show("Item data updated succesfully", "Action Succesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public void deleteThis()
        {
            string assetSerialNum = txt_SerialNumber.Text;
            if (assetSerialNum.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter a serial number", "Required Field");
                return;
            }
            if (ifExists().Equals(false))
            {
                XtraMessageBox.Show("The item serial number entered does not exist", "Invalid operation");
                return;
            }
            DialogResult dlr = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to delete the selected item from the system?", "System Confirmation", MessageBoxButtons.YesNo);

            if (dlr == DialogResult.Yes)
            {
                try
                {
                    string DateAdded = DateTime.Now.ToString("G");
                    string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                    MySqlConnection conn = new MySqlConnection(conString);
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand("DELETE FROM tblNewItem  WHERE serialNumber = @serialNumber", conn))
                    {

                        command.Parameters.AddWithValue("@serialNumber", txt_SerialNumber.Text);
                        command.ExecuteNonQuery();
                        using (MySqlCommand command2 = new MySqlCommand("DELETE FROM unassignedItems WHERE serialNumber = @serialNumber", conn))
                        {
                            command2.Parameters.AddWithValue("@serialNumber", assetSerialNum);
                            command2.ExecuteNonQuery();
                            using (MySqlCommand command3 = new MySqlCommand(" INSERT INTO transactionlog(Activity, User, Time) VALUES (@Activity,@User,@Time)", conn))
                            {
                                command3.Parameters.AddWithValue("@Activity", "Deleted an item serial number " + assetSerialNum);
                                command3.Parameters.AddWithValue("@User", currentUser);
                                command3.Parameters.AddWithValue("@Time", DateAdded);
                                command3.ExecuteNonQuery();
                                XtraMessageBox.Show("Item deleted successfully", "Action Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                reset();
                                conn.Close();
                            }

                        }

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

        public void NavigateDown()
        {
            ////throw new NotImplementedException();
        }

        public void NavigateUp()
        {
            ////throw new NotImplementedException();
        }
        private void validate()
        {
            if (txt_SerialNumber.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's serial number first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_ProductName.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's name first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
             if (combo_category.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly select item's category first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_Manufacturer.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's manufacturer name first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_Value.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's value first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_Age.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's age first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_Status.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's condition first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txt_warranty.Text.Equals(""))
            {
                XtraMessageBox.Show("Kindly enter the item's warranty period first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (picBoxPhoto.Image.Equals(null))
            {
                XtraMessageBox.Show("Kindly upload the item's photo first", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }
        public byte[] ImageToByteArray(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            ManufacturerList();
            PrinterNamesList();
            AgeList();
            StatusList();
        }
        private void ManufacturerList()
        {
            AutoCompleteStringCollection colValues = new AutoCompleteStringCollection();
            colValues.AddRange(new string[] { "Hewlett Packard(HP)", "Lenovo", "Apple", "Acer", "Toshiba", "Dell", "Sony", "Fujitsu ", "NEC", "Epson", "Panasonic", "Xerox ", "Kyocera", "Lexmark", "Samsung", "ABB", "Ablerex", "Active Power", "Acumentrics", "Aetes", "Alpha", "Alwayson" , "Amplimag", "APC", "Benning" , "Chloride" , "Clary", "Controlled Power" , "CP", "CS Eletro" , "Cyberex", "Energy Braz" , "Equisul GPL", "Falcon", "FUJI", "Gustav Klein", "Gutor", "IE Power", "IREM", "Jovy Atlas", "Log Master" , "LTI", "Marathon", "Minuteman", "Mitsubishi" , "NHS", "Piller" , "Powercom", "Powersun", "Power Kinetics", "Salicru", "Sanken", "Siel", "Socomec", "Solid State", "Sola" , "Staco", "Toshiba" , "Tripp Lite" , "TSI", "Yamabishi"
            });
            txt_Manufacturer.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt_Manufacturer.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_Manufacturer.MaskBox.AutoCompleteCustomSource = colValues;
        }
        private void PrinterNamesList()
        {
            AutoCompleteStringCollection colValues = new AutoCompleteStringCollection();
            colValues.AddRange(new string[] {
                "HP LaserJet Pro 100 Color MFP M175nw","HP Color LaserJet Pro MFP M177fw","HP LaserJet Pro 200 Color MFP M276nw","HP LaserJet Pro 300 Color MFP M375nw","HP LaserJet Pro 400 Color MFP M475 Series","HP LaserJet Pro 500 color MFP M570dn","HP LaserJet Enterprise 500 color MFP M575 Series","HP LaserJet Enterprise Color Flow MFP M575c ","HP Color LaserJet Enterprise CM4540 MFP Series","HP LaserJet Enterprise 700 color MFP M775 Series","HP LaserJet CM6040f Color MFP","HP Color LaserJet Enterprise Flow M880 MFP Series ","HP LaserJet M1212nf MFP","HP LaserJet M1217nfw MFP","HP LaserJet Pro MFP M127 Series","HP LaserJet Pro M1536dnf MFP","HP LaserJet Pro 400 MFP M425dn","HP LaserJet Pro MFP M521dn","HP LaserJet Enterprise 500 MFP M525 Series","HP LaserJet Enterprise Flow MFP M525c","HP LaserJet Enterprise M4555 MFP Series","HP LaserJet Enterprise 700 MFP M725 Series","HP LaserJet M9050 MFP","HP LaserJet Enterprise Flow MFP M830z","HP LaserJet Pro 200 Color M251nw","HP LaserJet Pro 400 Color M451 Series","HP LaserJet Enterprise 500 Color M551 Series","HP LaserJet CP4025 Color printer Series","HP LaserJet CP4525 Color printer Series","HP LaserJet Pro CP5225 Color printer Series","HP Color LaserJet Enterprise M750 Series","HP Color LaserJet Enterprise M855 Series", "HP LaserJet Pro P1102w","HP LaserJet Pro P1606dn","HP LaserJet P2035","HP LaserJet Pro 400 M401 Series", "HP LaserJet P3015 Series","HP LaserJet Enterprise 600 M601 Series","HP LaserJet Enterprise 600 M602 Series","HP LaserJet Enterprise 600 M603 Series","HP LaserJet Enterprise 700 M712n Series","HP LaserJet Enterprise M806 Printer Series","HP Officejet 4630", "HP Officejet 6600","HP Officejet 6700 Premium","HP Officejet Pro 8600 Premium","HP Officejet Pro 8600 Plus","HP Officejet Pro 276dw MFP","HP Officejet Pro X476dn MFP","HP Officejet Pro X476dw MFP","HP Officejet Pro X576dw MFP", "HP Officejet 6100","HP Officejet Pro 8100","HP Officejet Pro 251dw", "HP Officejet Pro X451dn","HP Officejet Pro X451dw","HP Officejet Pro X551dw","HP Officejet 100", "HP Officejet 150", "HP Officejet 7110","HP Officejet 7610", "HP Deskjet 1510", "HP Deskjet 2540", "HP Envy 4500","HP Envy 5530","HP Photosmart 6520", "HP Photosmart 7520","HP Envy 120","HP Designjet T7100","HP Designjet T2500","HP Designjet T1500","HP Designjet T1300ps","HP Designjet T920","HP Designjet T790ps","HP Designjet T520","HP Designjet T120","HP Designjet Z6200","HP Designjet Z5400ps","HP Designjet Z5200ps","HP Designjet Z3200ps","HP Designjet Z2100", "HP Designjet T1200","HP Designjet HD Scanner","HP Latex 210","HP Latex 260", "HP Latex 280", "HP Scitex FB7600","HP Scitex TJ8600","HP Scitex TJ8350", "HP Scitex Xp5500","HP Latex 850","HP Scitex FB700","HP Scitex FB500","HP Latex 3000","HP Scitex FB10000","HP Indigo r5000","HP Indigo 3550","HP Indigo 5600", "HP Indigo 7600","HP Indigo 10000", "HP Indigo WS6000p", "HP Indigo W7250","HP Indigo WS4600","HP Indigo W6600p","HP Scanjet G3110","HP Scanjet G4050","HP Scanjet 5590","HP Scanjet N6310","HP Scanjet 1000","HP Scanjet Pro 3000 s2","HP Scanjet Enterprise flow 5000 s2","HP Scanjet 8270","HP Scanjet N6350","HP Scanjet Enterprise Flow 7000 s2","HP Scanjet Enterprise Flow 7500", "HP Digital Sender Flow 8500 fn1","HP Scanjet Enterprise Flow N9120", "HP Jetdirect Wireless print Servers","HP NFC/Wireless 1200w Mobile print Accessory","HP Jetdirect Internal Print Servers","HP Jetdirect 300x print Server"
                , "Uninterruptible Power Supply"
            });
            txt_ProductName.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt_ProductName.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_ProductName.MaskBox.AutoCompleteCustomSource = colValues;
            // "Samsung",
        }
        private void AgeList()
        {
            AutoCompleteStringCollection colValues = new AutoCompleteStringCollection();
            colValues.AddRange(new string[] { "1 year", "2 years", "3 years", "4 years", "5 years", "6 years", "7 years", "8 years ", "9 years", "10 years" });
            txt_Age.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt_Age.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_Age.MaskBox.AutoCompleteCustomSource = colValues;
        }
        private void StatusList()
        {
            AutoCompleteStringCollection colValues = new AutoCompleteStringCollection();
            colValues.AddRange(new string[] { "Working", "Not Working" });
            txt_Status.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt_Status.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_Status.MaskBox.AutoCompleteCustomSource = colValues;
        }

        private void combo_category_SelectedIndexChanged(object sender, EventArgs e)
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();

            MySqlCommand command = new MySqlCommand("SELECT * FROM availablecategory WHERE categoryName = '" + combo_category.Text + "'", conn);
            MySqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    String itemList = reader.GetString(1);
                    // combo_category.Items.Add(itemList);
                    combo_category.Properties.Items.Add(itemList);
                    //  XtraMessageBox.Show(itemList
                    byte[] img = (byte[])(reader["Photo"]);
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

        private void txt_SerialNumber_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
                MySqlConnection conn = new MySqlConnection(conString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tblNewItem where serialNumber='" + txt_SerialNumber.Text.Trim() + "'", conn);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    combo_category.Text = dr.GetString(1);
                    txt_ProductName.Text = dr.GetString(2);
                    txt_Age.Text = dr.GetString(6);
                    txt_warranty.Text = dr.GetString(7);
                    txt_Manufacturer.Text = dr.GetString(4);
                    txt_Value.Text = dr.GetString(5);
                    txt_Status.Text = dr.GetString(8);
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
                XtraMessageBox.Show(es.ToString());
            }
        }

        private void XtraForm1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((admin)this.MdiParent).DisableWithNoGrid(false);
        }
        public void FillCombo()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT CategoryName FROM availablecategory", conn);
            MySqlDataReader reader;
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string itemList = reader.GetString(0);
                    combo_category.Properties.Items.Add(itemList);
                  
                }
                conn.Close();
            }
            catch (Exception es)
            {
                XtraMessageBox.Show(es.ToString());
            }

        }

        private void txt_SerialNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txt_SerialNumber_EditValueChanged(object sender, EventArgs e)
        {
            //isChanged = true;
        }

        private void XtraForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
        private bool ifExists()
        {
            string conString = "Server=localhost; Database=wpams; Uid=root; Pwd=";
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            MySqlCommand cmd2 = new MySqlCommand("select * from tblnewitem where SerialNumber='" + txt_SerialNumber.Text.Trim() + "'", conn);
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