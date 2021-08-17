namespace AssetMS
{
    partial class allLoanedItems
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(allLoanedItems));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource();
            this.colBorrowID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemSerialNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBorrowerName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBorrowerID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBorrowDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReturnDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAssignedBy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAssignDate = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.DataMember = "Query";
            this.gridControl1.DataSource = this.sqlDataSource1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(833, 288);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colBorrowID,
            this.colName,
            this.colItemSerialNumber,
            this.colStatus,
            this.colBorrowerName,
            this.colBorrowerID,
            this.colBorrowDate,
            this.colReturnDate,
            this.colAssignedBy,
            this.colAssignDate});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "localhost_wpams_Connection 11";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "Query";
            customSqlQuery1.Sql = resources.GetString("customSqlQuery1.Sql");
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // colBorrowID
            // 
            this.colBorrowID.FieldName = "BorrowID";
            this.colBorrowID.Name = "colBorrowID";
            this.colBorrowID.Visible = true;
            this.colBorrowID.VisibleIndex = 0;
            // 
            // colName
            // 
            this.colName.FieldName = "Name";
            this.colName.Name = "colName";
            this.colName.Visible = true;
            this.colName.VisibleIndex = 1;
            // 
            // colItemSerialNumber
            // 
            this.colItemSerialNumber.FieldName = "ItemSerialNumber";
            this.colItemSerialNumber.Name = "colItemSerialNumber";
            this.colItemSerialNumber.Visible = true;
            this.colItemSerialNumber.VisibleIndex = 2;
            // 
            // colStatus
            // 
            this.colStatus.FieldName = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 3;
            // 
            // colBorrowerName
            // 
            this.colBorrowerName.FieldName = "BorrowerName";
            this.colBorrowerName.Name = "colBorrowerName";
            this.colBorrowerName.Visible = true;
            this.colBorrowerName.VisibleIndex = 4;
            // 
            // colBorrowerID
            // 
            this.colBorrowerID.FieldName = "BorrowerID";
            this.colBorrowerID.Name = "colBorrowerID";
            this.colBorrowerID.Visible = true;
            this.colBorrowerID.VisibleIndex = 5;
            // 
            // colBorrowDate
            // 
            this.colBorrowDate.FieldName = "BorrowDate";
            this.colBorrowDate.Name = "colBorrowDate";
            this.colBorrowDate.Visible = true;
            this.colBorrowDate.VisibleIndex = 6;
            // 
            // colReturnDate
            // 
            this.colReturnDate.FieldName = "ReturnDate";
            this.colReturnDate.Name = "colReturnDate";
            this.colReturnDate.Visible = true;
            this.colReturnDate.VisibleIndex = 7;
            // 
            // colAssignedBy
            // 
            this.colAssignedBy.FieldName = "AssignedBy";
            this.colAssignedBy.Name = "colAssignedBy";
            this.colAssignedBy.Visible = true;
            this.colAssignedBy.VisibleIndex = 8;
            // 
            // colAssignDate
            // 
            this.colAssignDate.FieldName = "AssignDate";
            this.colAssignDate.Name = "colAssignDate";
            this.colAssignDate.Visible = true;
            this.colAssignDate.VisibleIndex = 9;
            // 
            // allLoanedItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 288);
            this.Controls.Add(this.gridControl1);
            this.Name = "allLoanedItems";
            this.Text = "All Borrowed Items";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.allLoanedItems_FormClosed);
            this.Load += new System.EventHandler(this.allLoanedItems_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colBorrowID;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colItemSerialNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colBorrowerName;
        private DevExpress.XtraGrid.Columns.GridColumn colBorrowerID;
        private DevExpress.XtraGrid.Columns.GridColumn colBorrowDate;
        private DevExpress.XtraGrid.Columns.GridColumn colReturnDate;
        private DevExpress.XtraGrid.Columns.GridColumn colAssignedBy;
        private DevExpress.XtraGrid.Columns.GridColumn colAssignDate;
    }
}