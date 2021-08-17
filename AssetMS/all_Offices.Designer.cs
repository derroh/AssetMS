namespace AssetMS
{
    partial class all_Offices
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(all_Offices));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colOfficeId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOfficeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDateAdded = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAddedBy = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.DataMember = "officestable";
            this.gridControl1.DataSource = this.sqlDataSource1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(516, 261);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "localhost_wpams_getOffices";
            this.sqlDataSource1.Name = "sqlDataSource1";
            tableQuery1.Name = "officestable";
            tableInfo1.Name = "officestable";
            columnInfo1.Name = "OfficeId";
            columnInfo2.Name = "OfficeName";
            columnInfo3.Name = "DateAdded";
            columnInfo4.Name = "AddedBy";
            tableInfo1.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] {
            columnInfo1,
            columnInfo2,
            columnInfo3,
            columnInfo4});
            tableQuery1.Tables.AddRange(new DevExpress.DataAccess.Sql.TableInfo[] {
            tableInfo1});
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            tableQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colOfficeId,
            this.colOfficeName,
            this.colDateAdded,
            this.colAddedBy});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // colOfficeId
            // 
            this.colOfficeId.FieldName = "OfficeId";
            this.colOfficeId.Name = "colOfficeId";
            this.colOfficeId.Visible = true;
            this.colOfficeId.VisibleIndex = 0;
            // 
            // colOfficeName
            // 
            this.colOfficeName.FieldName = "OfficeName";
            this.colOfficeName.Name = "colOfficeName";
            this.colOfficeName.Visible = true;
            this.colOfficeName.VisibleIndex = 1;
            // 
            // colDateAdded
            // 
            this.colDateAdded.FieldName = "DateAdded";
            this.colDateAdded.Name = "colDateAdded";
            this.colDateAdded.Visible = true;
            this.colDateAdded.VisibleIndex = 2;
            // 
            // colAddedBy
            // 
            this.colAddedBy.FieldName = "AddedBy";
            this.colAddedBy.Name = "colAddedBy";
            this.colAddedBy.Visible = true;
            this.colAddedBy.VisibleIndex = 3;
            // 
            // all_Offices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 261);
            this.Controls.Add(this.gridControl1);
            this.Name = "all_Offices";
            this.Text = "All Offices";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.all_Offices_FormClosed);
            this.Load += new System.EventHandler(this.all_Offices_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colOfficeId;
        private DevExpress.XtraGrid.Columns.GridColumn colOfficeName;
        private DevExpress.XtraGrid.Columns.GridColumn colDateAdded;
        private DevExpress.XtraGrid.Columns.GridColumn colAddedBy;
    }
}