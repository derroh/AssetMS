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

namespace AssetMS
{
    public partial class pdfviewer : DevExpress.XtraEditors.XtraForm
    {
        public String documentPath;
        public pdfviewer()
        {
            InitializeComponent();
        }
        public pdfviewer(String path)
        {
            InitializeComponent();
            documentPath = path;
        }

        private void pdfviewer_Load(object sender, EventArgs e)
        {
            this.pdfViewer1.LoadDocument(documentPath);
            this.Text = Path.GetFileName(documentPath); 
            //barButtonItem12.
        }

        private void pdfviewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((admin)this.MdiParent).DisableControls(false);
        }
    }
}