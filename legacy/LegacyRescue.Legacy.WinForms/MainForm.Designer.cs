namespace ProdMgr
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWork = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuResult = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cboLine = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.grpEdit = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOrderNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpWorkDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.cboShift = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboLineEdit = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cboItem = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPlan = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnResult = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.grpEdit.SuspendLayout();
            this.SuspendLayout();
            //
            // menuStrip1
            //
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuWork,
            this.mnuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1024, 24);
            this.menuStrip1.TabIndex = 0;
            //
            // mnuFile
            //
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSaveCsv,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(57, 20);
            this.mnuFile.Text = "파일(&F)";
            //
            // mnuSaveCsv
            //
            this.mnuSaveCsv.Name = "mnuSaveCsv";
            this.mnuSaveCsv.Size = new System.Drawing.Size(180, 22);
            this.mnuSaveCsv.Text = "목록 엑셀(CSV) 저장";
            this.mnuSaveCsv.Click += new System.EventHandler(this.mnuSaveCsv_Click);
            //
            // mnuExit
            //
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(180, 22);
            this.mnuExit.Text = "종료(&X)";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            //
            // mnuWork
            //
            this.mnuWork.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuResult,
            this.mnuReport});
            this.mnuWork.Name = "mnuWork";
            this.mnuWork.Size = new System.Drawing.Size(59, 20);
            this.mnuWork.Text = "작업(&W)";
            //
            // mnuResult
            //
            this.mnuResult.Name = "mnuResult";
            this.mnuResult.Size = new System.Drawing.Size(180, 22);
            this.mnuResult.Text = "실적 입력";
            this.mnuResult.Click += new System.EventHandler(this.mnuResult_Click);
            //
            // mnuReport
            //
            this.mnuReport.Name = "mnuReport";
            this.mnuReport.Size = new System.Drawing.Size(180, 22);
            this.mnuReport.Text = "일별 리포트";
            this.mnuReport.Click += new System.EventHandler(this.mnuReport_Click);
            //
            // mnuHelp
            //
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(72, 20);
            this.mnuHelp.Text = "도움말(&H)";
            //
            // mnuAbout
            //
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(180, 22);
            this.mnuAbout.Text = "프로그램 정보";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            //
            // grpSearch
            //
            this.grpSearch.Controls.Add(this.label1);
            this.grpSearch.Controls.Add(this.dtpFrom);
            this.grpSearch.Controls.Add(this.label2);
            this.grpSearch.Controls.Add(this.dtpTo);
            this.grpSearch.Controls.Add(this.label3);
            this.grpSearch.Controls.Add(this.cboLine);
            this.grpSearch.Controls.Add(this.btnSearch);
            this.grpSearch.Location = new System.Drawing.Point(12, 30);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(1000, 52);
            this.grpSearch.TabIndex = 1;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "조회 조건";
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 14);
            this.label1.Text = "작업일";
            //
            // dtpFrom
            //
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(64, 19);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(100, 21);
            this.dtpFrom.TabIndex = 0;
            //
            // label2
            //
            this.label2.Location = new System.Drawing.Point(168, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 14);
            this.label2.Text = "~";
            //
            // dtpTo
            //
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(184, 19);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(100, 21);
            this.dtpTo.TabIndex = 1;
            //
            // label3
            //
            this.label3.Location = new System.Drawing.Point(308, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 14);
            this.label3.Text = "라인";
            //
            // cboLine
            //
            this.cboLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLine.Location = new System.Drawing.Point(346, 19);
            this.cboLine.Name = "cboLine";
            this.cboLine.Size = new System.Drawing.Size(120, 20);
            this.cboLine.TabIndex = 2;
            //
            // btnSearch
            //
            this.btnSearch.Location = new System.Drawing.Point(490, 17);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 25);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            //
            // dgvList
            //
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Location = new System.Drawing.Point(12, 90);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersWidth = 24;
            this.dgvList.RowTemplate.Height = 20;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(1000, 390);
            this.dgvList.TabIndex = 2;
            this.dgvList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvList_DataBindingComplete);
            this.dgvList.SelectionChanged += new System.EventHandler(this.dgvList_SelectionChanged);
            //
            // grpEdit
            //
            this.grpEdit.Controls.Add(this.label4);
            this.grpEdit.Controls.Add(this.txtOrderNo);
            this.grpEdit.Controls.Add(this.label5);
            this.grpEdit.Controls.Add(this.dtpWorkDate);
            this.grpEdit.Controls.Add(this.label6);
            this.grpEdit.Controls.Add(this.cboShift);
            this.grpEdit.Controls.Add(this.label7);
            this.grpEdit.Controls.Add(this.cboLineEdit);
            this.grpEdit.Controls.Add(this.label8);
            this.grpEdit.Controls.Add(this.cboItem);
            this.grpEdit.Controls.Add(this.label9);
            this.grpEdit.Controls.Add(this.txtPlan);
            this.grpEdit.Controls.Add(this.label10);
            this.grpEdit.Controls.Add(this.txtRemark);
            this.grpEdit.Controls.Add(this.btnNew);
            this.grpEdit.Controls.Add(this.btnSave);
            this.grpEdit.Controls.Add(this.btnDelete);
            this.grpEdit.Controls.Add(this.btnResult);
            this.grpEdit.Location = new System.Drawing.Point(12, 488);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.Size = new System.Drawing.Size(1000, 150);
            this.grpEdit.TabIndex = 3;
            this.grpEdit.TabStop = false;
            this.grpEdit.Text = "작업지시 등록/수정";
            //
            // label4
            //
            this.label4.Location = new System.Drawing.Point(12, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 14);
            this.label4.Text = "지시번호";
            //
            // txtOrderNo
            //
            this.txtOrderNo.Location = new System.Drawing.Point(78, 24);
            this.txtOrderNo.Name = "txtOrderNo";
            this.txtOrderNo.ReadOnly = true;
            this.txtOrderNo.Size = new System.Drawing.Size(140, 21);
            this.txtOrderNo.TabIndex = 0;
            this.txtOrderNo.Text = "(자동)";
            //
            // label5
            //
            this.label5.Location = new System.Drawing.Point(240, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 14);
            this.label5.Text = "작업일";
            //
            // dtpWorkDate
            //
            this.dtpWorkDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpWorkDate.Location = new System.Drawing.Point(292, 24);
            this.dtpWorkDate.Name = "dtpWorkDate";
            this.dtpWorkDate.Size = new System.Drawing.Size(100, 21);
            this.dtpWorkDate.TabIndex = 1;
            //
            // label6
            //
            this.label6.Location = new System.Drawing.Point(410, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 14);
            this.label6.Text = "교대";
            //
            // cboShift
            //
            this.cboShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShift.Location = new System.Drawing.Point(448, 24);
            this.cboShift.Name = "cboShift";
            this.cboShift.Size = new System.Drawing.Size(70, 20);
            this.cboShift.TabIndex = 2;
            //
            // label7
            //
            this.label7.Location = new System.Drawing.Point(540, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 14);
            this.label7.Text = "라인";
            //
            // cboLineEdit
            //
            this.cboLineEdit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLineEdit.Location = new System.Drawing.Point(578, 24);
            this.cboLineEdit.Name = "cboLineEdit";
            this.cboLineEdit.Size = new System.Drawing.Size(110, 20);
            this.cboLineEdit.TabIndex = 3;
            //
            // label8
            //
            this.label8.Location = new System.Drawing.Point(12, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 14);
            this.label8.Text = "품목";
            //
            // cboItem
            //
            this.cboItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboItem.Location = new System.Drawing.Point(78, 58);
            this.cboItem.Name = "cboItem";
            this.cboItem.Size = new System.Drawing.Size(220, 20);
            this.cboItem.TabIndex = 4;
            //
            // label9
            //
            this.label9.Location = new System.Drawing.Point(316, 62);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 14);
            this.label9.Text = "계획수량";
            //
            // txtPlan
            //
            this.txtPlan.Location = new System.Drawing.Point(378, 58);
            this.txtPlan.Name = "txtPlan";
            this.txtPlan.Size = new System.Drawing.Size(80, 21);
            this.txtPlan.TabIndex = 5;
            this.txtPlan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // label10
            //
            this.label10.Location = new System.Drawing.Point(12, 96);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 14);
            this.label10.Text = "비고";
            //
            // txtRemark
            //
            this.txtRemark.Location = new System.Drawing.Point(78, 92);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(610, 21);
            this.txtRemark.TabIndex = 6;
            //
            // btnNew
            //
            this.btnNew.Location = new System.Drawing.Point(720, 22);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(120, 28);
            this.btnNew.TabIndex = 7;
            this.btnNew.Text = "신규";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(850, 22);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 28);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnDelete
            //
            this.btnDelete.Location = new System.Drawing.Point(720, 58);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 28);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnResult
            //
            this.btnResult.Location = new System.Drawing.Point(850, 58);
            this.btnResult.Name = "btnResult";
            this.btnResult.Size = new System.Drawing.Size(120, 28);
            this.btnResult.TabIndex = 10;
            this.btnResult.Text = "실적 입력...";
            this.btnResult.UseVisualStyleBackColor = true;
            this.btnResult.Click += new System.EventHandler(this.btnResult_Click);
            //
            // lblStatus
            //
            this.lblStatus.Location = new System.Drawing.Point(12, 648);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1000, 16);
            this.lblStatus.Text = "준비";
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 672);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.grpSearch);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.grpEdit);
            this.Controls.Add(this.lblStatus);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "생산실적관리 v1.3";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.grpEdit.ResumeLayout(false);
            this.grpEdit.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveCsv;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuWork;
        private System.Windows.Forms.ToolStripMenuItem mnuResult;
        private System.Windows.Forms.ToolStripMenuItem mnuReport;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboLine;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.GroupBox grpEdit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOrderNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpWorkDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboShift;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboLineEdit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboItem;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPlan;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnResult;
        private System.Windows.Forms.Label lblStatus;
    }
}
