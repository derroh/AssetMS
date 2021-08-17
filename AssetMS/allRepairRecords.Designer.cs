namespace AssetMS
{
    partial class allRepairRecords
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
            this.components = new System.ComponentModel.Container();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(allRepairRecords));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colRepairID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItemSerialNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDateOfRepair = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRepairedBy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRepairsMade = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCost = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.gridControl1.Size = new System.Drawing.Size(806, 302);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.Click += new System.EventHandler(this.gridControl1_Click);
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "localhost_wpams_GetRepairs";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "Query";
            customSqlQuery1.Sql = "SELECT `RepairID`, `ItemSerialNumber`, `DateOfRepair`, `RepairedBy`, `RepairsMade" +
    "`, `Cost` FROM `itemrepairs`";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRepairID,
            this.colItemSerialNumber,
            this.colDateOfRepair,
            this.colRepairedBy,
            this.colRepairsMade,
            this.colCost});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // colRepairID
            // 
            this.colRepairID.FieldName = "RepairID";
            this.colRepairID.Name = "colRepairID";
            this.colRepairID.Visible = true;
            this.colRepairID.VisibleIndex = 0;
            // 
            // colItemSerialNumber
            // 
            this.colItemSerialNumber.FieldName = "ItemSerialNumber";
            this.colItemSerialNumber.Name = "colItemSerialNumber";
            this.colItemSerialNumber.Visible = true;
            this.colItemSerialNumber.VisibleIndex = 1;
            // 
            // colDateOfRepair
            // 
            this.colDateOfRepair.FieldName = "DateOfRepair";
            this.colDateOfRepair.Name = "colDateOfRepair";
            this.colDateOfRepair.Visible = true;
            this.colDateOfRepair.VisibleIndex = 2;
            // 
            // colRepairedBy
            // 
            this.colRepairedBy.FieldName = "RepairedBy";
            this.colRepairedBy.Name = "colRepairedBy";
            this.colRepairedBy.Visible = true;
            this.colRepairedBy.VisibleIndex = 3;
            // 
            // colRepairsMade
            // 
            this.colRepairsMade.FieldName = "RepairsMade";
            this.colRepairsMade.Name = "colRepairsMade";
            this.colRepairsMade.Visible = true;
            this.colRepairsMade.VisibleIndex = 4;
            // 
            // colCost
            // 
            this.colCost.FieldName = "Cost";
            this.colCost.Name = "colCost";
            this.colCost.Visible = true;
            this.colCost.VisibleIndex = 5;
            // 
            // allRepairRecords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 302);
            this.Controls.Add(this.gridControl1);
            this.Name = "allRepairRecords";
            this.Text = "All Repair Records";
            this.Load += new System.EventHandler(this.allRepairRecords_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraGrid.Columns.GridColumn colRepairID;
        private DevExpress.XtraGrid.Columns.GridColumn colItemSerialNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colDateOfRepair;
        private DevExpress.XtraGrid.Columns.GridColumn colRepairedBy;
        private DevExpress.XtraGrid.Columns.GridColumn colRepairsMade;
        private DevExpress.XtraGrid.Columns.GridColumn colCost;
    }
}