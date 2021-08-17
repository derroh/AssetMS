namespace AssetMS
{
    partial class all_inventories
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
            DevExpress.DataAccess.Sql.TableQuery tableQuery1 = new DevExpress.DataAccess.Sql.TableQuery();
            DevExpress.DataAccess.Sql.TableInfo tableInfo1 = new DevExpress.DataAccess.Sql.TableInfo();
            DevExpress.DataAccess.Sql.ColumnInfo columnInfo1 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo columnInfo2 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo columnInfo3 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo columnInfo4 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo columnInfo5 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo columnInfo6 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo columnInfo7 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo columnInfo8 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo columnInfo9 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo columnInfo10 = new DevExpress.DataAccess.Sql.ColumnInfo();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(all_inventories));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colInventoryDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInventoryNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserDepartment = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSerialNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colManufacturer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLocation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colWhenItemFailed = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInventoryBy = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.DataMember = "inventorytable";
            this.gridControl1.DataSource = this.sqlDataSource1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1025, 293);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "localhost_wpams_ConnectionInve";
            this.sqlDataSource1.Name = "sqlDataSource1";
            tableQuery1.Name = "inventorytable";
            tableInfo1.Name = "inventorytable";
            columnInfo1.Name = "InventoryDate";
            columnInfo2.Name = "InventoryNumber";
            columnInfo3.Name = "UserDepartment";
            columnInfo4.Name = "ProductName";
            columnInfo5.Name = "SerialNumber";
            columnInfo6.Name = "Manufacturer";
            columnInfo7.Name = "Status";
            columnInfo8.Name = "Location";
            columnInfo9.Name = "WhenItemFailed";
            columnInfo10.Name = "InventoryBy";
            tableInfo1.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] {
            columnInfo1,
            columnInfo2,
            columnInfo3,
            columnInfo4,
            columnInfo5,
            columnInfo6,
            columnInfo7,
            columnInfo8,
            columnInfo9,
            columnInfo10});
            tableQuery1.Tables.AddRange(new DevExpress.DataAccess.Sql.TableInfo[] {
            tableInfo1});
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            tableQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colInventoryDate,
            this.colInventoryNumber,
            this.colUserDepartment,
            this.colProductName,
            this.colSerialNumber,
            this.colManufacturer,
            this.colStatus,
            this.colLocation,
            this.colWhenItemFailed,
            this.colInventoryBy});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // colInventoryDate
            // 
            this.colInventoryDate.FieldName = "InventoryDate";
            this.colInventoryDate.Name = "colInventoryDate";
            this.colInventoryDate.Visible = true;
            this.colInventoryDate.VisibleIndex = 0;
            // 
            // colInventoryNumber
            // 
            this.colInventoryNumber.FieldName = "InventoryNumber";
            this.colInventoryNumber.Name = "colInventoryNumber";
            this.colInventoryNumber.Visible = true;
            this.colInventoryNumber.VisibleIndex = 1;
            // 
            // colUserDepartment
            // 
            this.colUserDepartment.FieldName = "UserDepartment";
            this.colUserDepartment.Name = "colUserDepartment";
            this.colUserDepartment.Visible = true;
            this.colUserDepartment.VisibleIndex = 2;
            // 
            // colProductName
            // 
            this.colProductName.FieldName = "ProductName";
            this.colProductName.Name = "colProductName";
            this.colProductName.Visible = true;
            this.colProductName.VisibleIndex = 3;
            // 
            // colSerialNumber
            // 
            this.colSerialNumber.FieldName = "SerialNumber";
            this.colSerialNumber.Name = "colSerialNumber";
            this.colSerialNumber.Visible = true;
            this.colSerialNumber.VisibleIndex = 4;
            // 
            // colManufacturer
            // 
            this.colManufacturer.FieldName = "Manufacturer";
            this.colManufacturer.Name = "colManufacturer";
            this.colManufacturer.Visible = true;
            this.colManufacturer.VisibleIndex = 5;
            // 
            // colStatus
            // 
            this.colStatus.FieldName = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 6;
            // 
            // colLocation
            // 
            this.colLocation.FieldName = "Location";
            this.colLocation.Name = "colLocation";
            this.colLocation.Visible = true;
            this.colLocation.VisibleIndex = 7;
            // 
            // colWhenItemFailed
            // 
            this.colWhenItemFailed.FieldName = "WhenItemFailed";
            this.colWhenItemFailed.Name = "colWhenItemFailed";
            this.colWhenItemFailed.Visible = true;
            this.colWhenItemFailed.VisibleIndex = 8;
            // 
            // colInventoryBy
            // 
            this.colInventoryBy.FieldName = "InventoryBy";
            this.colInventoryBy.Name = "colInventoryBy";
            this.colInventoryBy.Visible = true;
            this.colInventoryBy.VisibleIndex = 9;
            // 
            // all_inventories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 293);
            this.Controls.Add(this.gridControl1);
            this.Name = "all_inventories";
            this.Text = "All Inventories";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.all_inventories_FormClosed);
            this.Load += new System.EventHandler(this.all_inventories_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraGrid.Columns.GridColumn colInventoryDate;
        private DevExpress.XtraGrid.Columns.GridColumn colInventoryNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colUserDepartment;
        private DevExpress.XtraGrid.Columns.GridColumn colProductName;
        private DevExpress.XtraGrid.Columns.GridColumn colSerialNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colManufacturer;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colLocation;
        private DevExpress.XtraGrid.Columns.GridColumn colWhenItemFailed;
        private DevExpress.XtraGrid.Columns.GridColumn colInventoryBy;
    }
}