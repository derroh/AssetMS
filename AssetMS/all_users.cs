﻿using System;
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

namespace AssetMS
{
    public partial class all_users : DevExpress.XtraEditors.XtraForm, IChildSave
    {
        public string pathToPDF;
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
        public all_users()
        {
            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
            // Fill a SqlDataSource
            sqlDataSource1.Fill();
            // This line of code is generated by Data Source Configuration Wizard
            // Fill a SqlDataSource
            sqlDataSource2.Fill();

            gridView1.ShowFindPanel();
        }

        private void all_users_Load(object sender, EventArgs e)
        {

        }

        public void SaveAction()
        {
            ////throw new NotImplementedException();
        }

        public void reset()
        {
           // //throw new NotImplementedException();
        }

        public string Xport(string ext)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = ext;
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;

                    switch (fileExtenstion)
                    {
                        case ".xls":
                            gridControl1.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            gridControl1.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            gridControl1.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            gridControl1.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            gridControl1.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            gridControl1.ExportToMht(exportFilePath);
                            break;
                        default:
                            break;
                    }

                    if (File.Exists(exportFilePath))
                    {
                        try
                        {
                            //Try to open the file and let windows decide how to open it.
                            // System.Diagnostics.Process.Start(exportFilePath);

                            //string ext = Path.GetExtension(exportFilePath);
                            if (!(fileExtenstion.Equals(".pdf")))
                            {
                                System.Diagnostics.Process.Start(exportFilePath);
                            }
                            else
                            {
                                // MessageBox.Show("pdf detected", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                pathToPDF = exportFilePath;

                            }

                            // pdfviewer pf = new pdfviewer();
                            //pf.
                        }
                        catch
                        {
                            String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                            XtraMessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                        XtraMessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            return pathToPDF;
        }

        public void updatethis()
        {
            // //throw new NotImplementedException();
            gridControl1.RefreshDataSource();
        }

        public void deleteThis()
        {
          //  //throw new NotImplementedException();
        }

        public void NavigateDown()
        {
            gridView1.MovePrev();
        }

        public void NavigateUp()
        {
            gridView1.MoveNext();
        }

        private void all_users_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((admin)this.MdiParent).DisableControls(false);
        }

        private void all_users_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}