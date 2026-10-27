namespace ProdMgr
{
    partial class ResultForm
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
            this.grpOrder = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblOrderNo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblWorkDate = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLine = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblItem = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblPlan = new System.Windows.Forms.Label();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGood = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDefect = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cboDefect = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtEnd = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.grpInput.SuspendLayout();
            this.SuspendLayout();
            //
            // grpOrder
            //
            this.grpOrder.Controls.Add(this.label1);
            this.grpOrder.Controls.Add(this.lblOrderNo);
            this.grpOrder.Controls.Add(this.label2);
            this.grpOrder.Controls.Add(this.lblWorkDate);
            this.grpOrder.Controls.Add(this.label3);
            this.grpOrder.Controls.Add(this.lblLine);
            this.grpOrder.Controls.Add(this.label4);
            this.grpOrder.Controls.Add(this.lblItem);
            this.grpOrder.Controls.Add(this.label5);
            this.grpOrder.Controls.Add(this.lblPlan);
            this.grpOrder.Location = new System.Drawing.Point(10, 10);
            this.grpOrder.Name = "grpOrder";
            this.grpOrder.Size = new System.Drawing.Size(620, 76);
            this.grpOrder.TabIndex = 0;
            this.grpOrder.TabStop = false;
            this.grpOrder.Text = "작업지시";
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 14);
            this.label1.Text = "지시번호";
            //
            // lblOrderNo
            //
            this.lblOrderNo.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.lblOrderNo.Location = new System.Drawing.Point(72, 22);
            this.lblOrderNo.Name = "lblOrderNo";
            this.lblOrderNo.Size = new System.Drawing.Size(140, 14);
            //
            // label2
            //
            this.label2.Location = new System.Drawing.Point(220, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 14);
            this.label2.Text = "작업일";
            //
            // lblWorkDate
            //
            this.lblWorkDate.Location = new System.Drawing.Point(272, 22);
            this.lblWorkDate.Name = "lblWorkDate";
            this.lblWorkDate.Size = new System.Drawing.Size(130, 14);
            //
            // label3
            //
            this.label3.Location = new System.Drawing.Point(420, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 14);
            this.label3.Text = "라인";
            //
            // lblLine
            //
            this.lblLine.Location = new System.Drawing.Point(458, 22);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(100, 14);
            //
            // label4
            //
            this.label4.Location = new System.Drawing.Point(10, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 14);
            this.label4.Text = "품목";
            //
            // lblItem
            //
            this.lblItem.Location = new System.Drawing.Point(72, 48);
            this.lblItem.Name = "lblItem";
            this.lblItem.Size = new System.Drawing.Size(240, 14);
            //
            // label5
            //
            this.label5.Location = new System.Drawing.Point(420, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 14);
            this.label5.Text = "계획수량";
            //
            // lblPlan
            //
            this.lblPlan.Location = new System.Drawing.Point(482, 48);
            this.lblPlan.Name = "lblPlan";
            this.lblPlan.Size = new System.Drawing.Size(80, 14);
            //
            // dgvResult
            //
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Location = new System.Drawing.Point(10, 94);
            this.dgvResult.MultiSelect = false;
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            this.dgvResult.RowHeadersWidth = 24;
            this.dgvResult.RowTemplate.Height = 20;
            this.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResult.Size = new System.Drawing.Size(620, 180);
            this.dgvResult.TabIndex = 1;
            this.dgvResult.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResult_CellClick);
            //
            // lblTotal
            //
            this.lblTotal.Location = new System.Drawing.Point(10, 280);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(620, 14);
            //
            // grpInput
            //
            this.grpInput.Controls.Add(this.label6);
            this.grpInput.Controls.Add(this.txtGood);
            this.grpInput.Controls.Add(this.label7);
            this.grpInput.Controls.Add(this.txtDefect);
            this.grpInput.Controls.Add(this.label8);
            this.grpInput.Controls.Add(this.cboDefect);
            this.grpInput.Controls.Add(this.label9);
            this.grpInput.Controls.Add(this.txtStart);
            this.grpInput.Controls.Add(this.label10);
            this.grpInput.Controls.Add(this.txtEnd);
            this.grpInput.Location = new System.Drawing.Point(10, 302);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(620, 80);
            this.grpInput.TabIndex = 2;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "실적 입력";
            //
            // label6
            //
            this.label6.Location = new System.Drawing.Point(10, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 14);
            this.label6.Text = "양품수량";
            //
            // txtGood
            //
            this.txtGood.Location = new System.Drawing.Point(72, 20);
            this.txtGood.Name = "txtGood";
            this.txtGood.Size = new System.Drawing.Size(70, 21);
            this.txtGood.TabIndex = 0;
            this.txtGood.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // label7
            //
            this.label7.Location = new System.Drawing.Point(160, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 14);
            this.label7.Text = "불량수량";
            //
            // txtDefect
            //
            this.txtDefect.Location = new System.Drawing.Point(222, 20);
            this.txtDefect.Name = "txtDefect";
            this.txtDefect.Size = new System.Drawing.Size(60, 21);
            this.txtDefect.TabIndex = 1;
            this.txtDefect.Text = "0";
            this.txtDefect.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // label8
            //
            this.label8.Location = new System.Drawing.Point(300, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 14);
            this.label8.Text = "불량유형";
            //
            // cboDefect
            //
            this.cboDefect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDefect.Location = new System.Drawing.Point(362, 20);
            this.cboDefect.Name = "cboDefect";
            this.cboDefect.Size = new System.Drawing.Size(130, 20);
            this.cboDefect.TabIndex = 2;
            //
            // label9
            //
            this.label9.Location = new System.Drawing.Point(10, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 14);
            this.label9.Text = "시작시각";
            //
            // txtStart
            //
            this.txtStart.Location = new System.Drawing.Point(72, 48);
            this.txtStart.MaxLength = 5;
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(70, 21);
            this.txtStart.TabIndex = 3;
            //
            // label10
            //
            this.label10.Location = new System.Drawing.Point(160, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 14);
            this.label10.Text = "종료시각";
            //
            // txtEnd
            //
            this.txtEnd.Location = new System.Drawing.Point(222, 48);
            this.txtEnd.MaxLength = 5;
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(60, 21);
            this.txtEnd.TabIndex = 4;
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(330, 392);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(96, 28);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnDelete
            //
            this.btnDelete.Location = new System.Drawing.Point(432, 392);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(96, 28);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "선택 삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnClose
            //
            this.btnClose.Location = new System.Drawing.Point(534, 392);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 28);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // ResultForm
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(640, 430);
            this.Controls.Add(this.grpOrder);
            this.Controls.Add(this.dgvResult);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.grpInput);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResultForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "실적 입력";
            this.Load += new System.EventHandler(this.ResultForm_Load);
            this.grpOrder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox grpOrder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOrderNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblWorkDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblPlan;
        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.GroupBox grpInput;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGood;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDefect;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboDefect;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtEnd;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
    }
}
