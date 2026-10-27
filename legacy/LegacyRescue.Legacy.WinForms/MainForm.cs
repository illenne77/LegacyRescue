using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ProdMgr
{
    public partial class MainForm : Form
    {
        DataTable dtList = new DataTable();
        int nSelId = 0;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = "생산실적관리 v1.3 - " + G.UserName;
            dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpTo.Value = DateTime.Today;
            dtpWorkDate.Value = DateTime.Today;
            lblStatus.Text = "DB: " + G.DbPath;

            try
            {
                InitCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("초기화 중 오류가 발생했습니다.\n" + ex.Message, "오류");
            }
        }

        void InitCombo()
        {
            cboLine.Items.Clear();
            cboLine.Items.Add("전체");
            cboLineEdit.Items.Clear();
            for (int i = 0; i < G.LineCodes.Length; i++)
            {
                cboLine.Items.Add(G.LineCodes[i] + " " + G.LineNames[i]);
                cboLineEdit.Items.Add(G.LineCodes[i] + " " + G.LineNames[i]);
            }
            cboLine.SelectedIndex = 0;
            if (cboLineEdit.Items.Count > 0) cboLineEdit.SelectedIndex = 0;

            cboShift.Items.Clear();
            cboShift.Items.Add("주간");
            cboShift.Items.Add("야간");
            cboShift.SelectedIndex = 0;

            cboItem.Items.Clear();
            SQLiteConnection conn = new SQLiteConnection(G.ConnStr());
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand("SELECT ItemCode, ItemName FROM Item ORDER BY ItemCode", conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                cboItem.Items.Add(rdr["ItemCode"].ToString() + " " + rdr["ItemName"].ToString());
            }
            rdr.Close();
            conn.Close();
            if (cboItem.Items.Count > 0) cboItem.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string line = "";
            if (cboLine.SelectedIndex > 0)
                line = G.LineCodes[cboLine.SelectedIndex - 1];

            this.Cursor = Cursors.WaitCursor;
            lblStatus.Text = "조회 중...";
            try
            {
                LoadList(dtpFrom.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd"), line);
                lblStatus.Text = "조회 완료 (" + dtList.Rows.Count + "건)";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "조회 실패";
                MessageBox.Show("조회 중 오류가 발생했습니다.\n" + ex.Message, "오류");
            }
            this.Cursor = Cursors.Default;
        }

        public void LoadList(string from, string to, string line)
        {
            dtList = new DataTable();
            dtList.Columns.Add("ID");
            dtList.Columns.Add("지시번호");
            dtList.Columns.Add("작업일");
            dtList.Columns.Add("교대");
            dtList.Columns.Add("라인");
            dtList.Columns.Add("품목코드");
            dtList.Columns.Add("품목명");
            dtList.Columns.Add("계획수량");
            dtList.Columns.Add("양품수량");
            dtList.Columns.Add("불량수량");
            dtList.Columns.Add("달성률");
            dtList.Columns.Add("불량률");
            dtList.Columns.Add("비고");

            string sql = "SELECT * FROM WorkOrder WHERE WorkDate >= '" + from + "' AND WorkDate <= '" + to + "'";
            if (line != "")
                sql += " AND LineCode = '" + line + "'";
            sql += " ORDER BY WorkDate, LineCode, Shift DESC, OrderNo";

            SQLiteConnection conn = new SQLiteConnection(G.ConnStr());
            conn.Open();
            SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow r in dt.Rows)
            {
                DataRow nr = dtList.NewRow();
                nr["ID"] = r["WorkOrderId"].ToString();
                nr["지시번호"] = r["OrderNo"].ToString();
                nr["작업일"] = r["WorkDate"].ToString();
                nr["교대"] = r["Shift"].ToString();
                nr["라인"] = G.LineName(r["LineCode"].ToString());
                nr["품목코드"] = r["ItemCode"].ToString();

                SQLiteCommand cmd = new SQLiteCommand("SELECT ItemName FROM Item WHERE ItemCode = '" + r["ItemCode"].ToString() + "'", conn);
                object o = cmd.ExecuteScalar();
                nr["품목명"] = o == null ? "" : o.ToString();

                int plan = Convert.ToInt32(r["PlanQty"]);
                int good = 0;
                int bad = 0;
                cmd = new SQLiteCommand("SELECT SUM(GoodQty), SUM(DefectQty) FROM ProductionResult WHERE WorkOrderId = " + r["WorkOrderId"].ToString(), conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    if (rdr[0] != DBNull.Value) good = Convert.ToInt32(rdr[0]);
                    if (rdr[1] != DBNull.Value) bad = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();

                nr["계획수량"] = plan.ToString();
                nr["양품수량"] = good.ToString();
                nr["불량수량"] = bad.ToString();
                if (plan > 0)
                    nr["달성률"] = Math.Round(good * 100.0 / plan, 1).ToString("0.0");
                else
                    nr["달성률"] = "";
                if (good + bad > 0)
                    nr["불량률"] = Math.Round(bad * 100.0 / (good + bad), 1).ToString("0.0");
                else
                    nr["불량률"] = "";
                nr["비고"] = r["Remark"] == DBNull.Value ? "" : r["Remark"].ToString();

                dtList.Rows.Add(nr);
            }
            conn.Close();

            dgvList.DataSource = dtList;
        }

        private void dgvList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvList.Columns["ID"].Visible = false;
            dgvList.Columns["지시번호"].Width = 130;
            dgvList.Columns["품목명"].Width = 130;
            dgvList.Columns["비고"].Width = 150;
        }

        private void dgvList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvList.CurrentRow == null) return;
            DataGridViewRow row = dgvList.CurrentRow;

            nSelId = int.Parse(row.Cells["ID"].Value.ToString());
            txtOrderNo.Text = row.Cells["지시번호"].Value.ToString();
            dtpWorkDate.Value = DateTime.Parse(row.Cells["작업일"].Value.ToString());
            cboShift.Text = row.Cells["교대"].Value.ToString();
            txtPlan.Text = row.Cells["계획수량"].Value.ToString();
            txtRemark.Text = row.Cells["비고"].Value.ToString();

            for (int i = 0; i < cboItem.Items.Count; i++)
            {
                if (cboItem.Items[i].ToString().StartsWith(row.Cells["품목코드"].Value.ToString() + " "))
                    cboItem.SelectedIndex = i;
            }
            for (int i = 0; i < G.LineNames.Length; i++)
            {
                if (G.LineNames[i] == row.Cells["라인"].Value.ToString())
                    cboLineEdit.SelectedIndex = i;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            nSelId = 0;
            txtOrderNo.Text = "(자동)";
            txtPlan.Text = "";
            txtRemark.Text = "";
            dtpWorkDate.Value = DateTime.Today;
            cboShift.SelectedIndex = 0;
            txtPlan.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int plan;
            try
            {
                plan = int.Parse(txtPlan.Text);
            }
            catch
            {
                MessageBox.Show("계획수량을 숫자로 입력하세요.", "입력 오류");
                txtPlan.Focus();
                return;
            }
            if (plan < 0)
            {
                MessageBox.Show("계획수량은 0 이상이어야 합니다.", "입력 오류");
                return;
            }
            if (cboItem.SelectedIndex < 0 || cboLineEdit.SelectedIndex < 0)
            {
                MessageBox.Show("품목과 라인을 선택하세요.", "입력 오류");
                return;
            }

            string itemCode = cboItem.Text.Split(' ')[0];
            string lineCode = G.LineCodes[cboLineEdit.SelectedIndex];
            string workDate = dtpWorkDate.Value.ToString("yyyy-MM-dd");
            string shift = cboShift.Text;

            try
            {
                SQLiteConnection conn = new SQLiteConnection(G.ConnStr());
                conn.Open();
                SQLiteCommand cmd;

                if (nSelId == 0)
                {
                    cmd = new SQLiteCommand("SELECT COUNT(*) FROM WorkOrder WHERE WorkDate = '" + workDate + "' AND LineCode = '" + lineCode + "' AND Shift = '" + shift + "'", conn);
                    int seq = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                    string orderNo = "WO-" + dtpWorkDate.Value.ToString("yyMMdd") + "-" + lineCode + (shift == "주간" ? "D" : "N") + "-" + seq.ToString("00");

                    string sql = "INSERT INTO WorkOrder (OrderNo, ItemCode, LineCode, PlanQty, WorkDate, Shift, Remark, CreatedAt) VALUES ('"
                        + orderNo + "', '" + itemCode + "', '" + lineCode + "', " + plan + ", '" + workDate + "', '" + shift + "', '"
                        + txtRemark.Text + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    txtOrderNo.Text = orderNo;
                }
                else
                {
                    string sql = "UPDATE WorkOrder SET ItemCode = '" + itemCode + "', LineCode = '" + lineCode + "', PlanQty = " + plan
                        + ", WorkDate = '" + workDate + "', Shift = '" + shift + "', Remark = '" + txtRemark.Text + "' WHERE WorkOrderId = " + nSelId;
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();

                MessageBox.Show("저장되었습니다.", "생산실적관리");
                btnSearch_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장 중 오류가 발생했습니다.\n" + ex.Message, "오류");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (nSelId == 0)
            {
                MessageBox.Show("삭제할 작업지시를 선택하세요.", "생산실적관리");
                return;
            }
            if (MessageBox.Show(txtOrderNo.Text + " 작업지시를 삭제하시겠습니까?", "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                SQLiteConnection conn = new SQLiteConnection(G.ConnStr());
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("DELETE FROM WorkOrder WHERE WorkOrderId = " + nSelId, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                nSelId = 0;
                btnSearch_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("삭제 중 오류가 발생했습니다.\n" + ex.Message, "오류");
            }
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            if (nSelId == 0)
            {
                MessageBox.Show("작업지시를 먼저 선택하세요.", "생산실적관리");
                return;
            }
            ResultForm f = new ResultForm(nSelId);
            f.ShowDialog();
            btnSearch_Click(null, null);
        }

        private void mnuResult_Click(object sender, EventArgs e)
        {
            btnResult_Click(sender, e);
        }

        private void mnuReport_Click(object sender, EventArgs e)
        {
            ReportForm f = new ReportForm();
            f.Show();
        }

        private void mnuSaveCsv_Click(object sender, EventArgs e)
        {
            if (dtList.Rows.Count == 0)
            {
                MessageBox.Show("저장할 데이터가 없습니다. 먼저 조회하세요.", "생산실적관리");
                return;
            }
            try
            {
                if (!Directory.Exists(G.ExportDir))
                    Directory.CreateDirectory(G.ExportDir);
                string path = G.ExportDir + "작업지시목록_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                SaveListCsv(path);
                MessageBox.Show("저장되었습니다.\n" + path, "생산실적관리");
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장 중 오류가 발생했습니다.\n" + ex.Message, "오류");
            }
        }

        public void SaveListCsv(string path)
        {
            StreamWriter sw = new StreamWriter(path, false, Encoding.Default);
            string line = "";
            for (int c = 1; c < dtList.Columns.Count; c++)
            {
                if (c > 1) line += ",";
                line += dtList.Columns[c].ColumnName;
            }
            sw.WriteLine(line);

            foreach (DataRow r in dtList.Rows)
            {
                line = "";
                for (int c = 1; c < dtList.Columns.Count; c++)
                {
                    if (c > 1) line += ",";
                    line += r[c].ToString();
                }
                sw.WriteLine(line);
            }
            sw.Close();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("생산실적관리 v1.3\n\n문의: 전산 담당 (내선 123)", "프로그램 정보");
        }
    }
}
