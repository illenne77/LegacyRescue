using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ProdMgr
{
    public partial class ResultForm : Form
    {
        int nWoId;
        string sShift = "";
        int nSelResultId = 0;

        public ResultForm(int woId)
        {
            InitializeComponent();
            nWoId = woId;
        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(G.ConnStr());
                conn.Open();

                SQLiteCommand cmd = new SQLiteCommand("SELECT w.*, i.ItemName FROM WorkOrder w LEFT JOIN Item i ON w.ItemCode = i.ItemCode WHERE w.WorkOrderId = " + nWoId, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    sShift = rdr["Shift"].ToString();
                    lblOrderNo.Text = rdr["OrderNo"].ToString();
                    lblWorkDate.Text = rdr["WorkDate"].ToString() + " (" + sShift + ")";
                    lblLine.Text = G.LineName(rdr["LineCode"].ToString());
                    lblItem.Text = rdr["ItemCode"].ToString() + " " + rdr["ItemName"].ToString();
                    lblPlan.Text = rdr["PlanQty"].ToString();
                }
                rdr.Close();

                cboDefect.Items.Clear();
                cboDefect.Items.Add("(없음)");
                cmd = new SQLiteCommand("SELECT Code, Name FROM DefectCode ORDER BY Code", conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    cboDefect.Items.Add(rdr["Code"].ToString() + " " + rdr["Name"].ToString());
                }
                rdr.Close();
                conn.Close();
                cboDefect.SelectedIndex = 0;

                txtStart.Text = sShift == "야간" ? G.NightStart : G.DayStart;
                LoadResults();
            }
            catch (Exception ex)
            {
                MessageBox.Show("작업지시 정보를 읽는 중 오류가 발생했습니다.\n" + ex.Message, "오류");
            }
        }

        void LoadResults()
        {
            SQLiteConnection conn = new SQLiteConnection(G.ConnStr());
            conn.Open();
            SQLiteDataAdapter da = new SQLiteDataAdapter(
                "SELECT ResultId, GoodQty AS 양품, DefectQty AS 불량, DefectCode AS 불량유형, StartTime AS 시작, EndTime AS 종료, EnteredAt AS 입력일시 "
                + "FROM ProductionResult WHERE WorkOrderId = " + nWoId + " ORDER BY ResultId", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();

            dgvResult.DataSource = dt;
            if (dgvResult.Columns.Contains("ResultId"))
                dgvResult.Columns["ResultId"].Visible = false;

            int good = 0, bad = 0, min = 0;
            foreach (DataRow r in dt.Rows)
            {
                good += Convert.ToInt32(r["양품"]);
                bad += Convert.ToInt32(r["불량"]);
                min += G.CalcMinutes(r["시작"].ToString(), r["종료"].ToString());
            }
            lblTotal.Text = "합계  양품 " + good + " / 불량 " + bad + " / 작업시간 " + min + "분";
            nSelResultId = 0;
        }

        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            nSelResultId = Convert.ToInt32(dgvResult.Rows[e.RowIndex].Cells["ResultId"].Value);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int good, bad;
            if (!int.TryParse(txtGood.Text, out good) || good < 0)
            {
                MessageBox.Show("양품수량을 확인하세요.", "입력 오류");
                txtGood.Focus();
                return;
            }
            if (txtDefect.Text == "") txtDefect.Text = "0";
            if (!int.TryParse(txtDefect.Text, out bad) || bad < 0)
            {
                MessageBox.Show("불량수량을 확인하세요.", "입력 오류");
                txtDefect.Focus();
                return;
            }
            if (bad > 0 && cboDefect.SelectedIndex <= 0)
            {
                MessageBox.Show("불량 유형을 선택하세요.", "입력 오류");
                return;
            }

            DateTime t;
            if (!DateTime.TryParseExact(txtStart.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out t)
                || !DateTime.TryParseExact(txtEnd.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out t))
            {
                MessageBox.Show("시간은 HH:mm 형식으로 입력하세요. (예: 08:00)", "입력 오류");
                return;
            }

            try
            {
                SQLiteConnection conn = new SQLiteConnection(G.ConnStr());
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(
                    "INSERT INTO ProductionResult (WorkOrderId, GoodQty, DefectQty, DefectCode, StartTime, EndTime, EnteredAt) "
                    + "VALUES (@wo, @good, @bad, @code, @start, @end, @at)", conn);
                cmd.Parameters.AddWithValue("@wo", nWoId);
                cmd.Parameters.AddWithValue("@good", good);
                cmd.Parameters.AddWithValue("@bad", bad);
                if (cboDefect.SelectedIndex > 0)
                    cmd.Parameters.AddWithValue("@code", cboDefect.Text.Split(' ')[0]);
                else
                    cmd.Parameters.AddWithValue("@code", DBNull.Value);
                cmd.Parameters.AddWithValue("@start", txtStart.Text);
                cmd.Parameters.AddWithValue("@end", txtEnd.Text);
                cmd.Parameters.AddWithValue("@at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                conn.Close();

                txtGood.Text = "";
                txtDefect.Text = "0";
                cboDefect.SelectedIndex = 0;
                txtStart.Text = txtEnd.Text;
                txtEnd.Text = "";
                LoadResults();
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장 중 오류가 발생했습니다.\n" + ex.Message, "오류");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (nSelResultId == 0)
            {
                MessageBox.Show("삭제할 실적을 선택하세요.", "실적 입력");
                return;
            }
            if (MessageBox.Show("선택한 실적을 삭제하시겠습니까?", "삭제 확인", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                SQLiteConnection conn = new SQLiteConnection(G.ConnStr());
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("DELETE FROM ProductionResult WHERE ResultId = " + nSelResultId, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadResults();
            }
            catch (Exception ex)
            {
                MessageBox.Show("삭제 중 오류가 발생했습니다.\n" + ex.Message, "오류");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
