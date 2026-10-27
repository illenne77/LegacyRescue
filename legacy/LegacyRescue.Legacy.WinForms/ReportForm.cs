using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ProdMgr
{
    public partial class ReportForm : Form
    {
        DataTable dtReport = new DataTable();
        DataTable dtDefect = new DataTable();
        string sDate = "";

        public ReportForm()
        {
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            dtpDate.Value = DateTime.Today.AddDays(-1);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                LoadReport(dtpDate.Value.ToString("yyyy-MM-dd"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("리포트 조회 중 오류가 발생했습니다.\n" + ex.Message, "오류");
            }
            this.Cursor = Cursors.Default;
        }

        public void LoadReport(string date)
        {
            sDate = date;
            int n = G.LineCodes.Length;
            int[,] plan = new int[n, 2];
            int[,] good = new int[n, 2];
            int[,] bad = new int[n, 2];
            int[,] work = new int[n, 2];

            SQLiteConnection conn = new SQLiteConnection(G.ConnStr());
            conn.Open();

            SQLiteCommand cmd = new SQLiteCommand("SELECT LineCode, Shift, SUM(PlanQty) AS PlanQty FROM WorkOrder WHERE WorkDate = '" + date + "' GROUP BY LineCode, Shift", conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int li = G.LineIndex(rdr["LineCode"].ToString());
                if (li < 0) continue;
                int si = rdr["Shift"].ToString() == "야간" ? 1 : 0;
                plan[li, si] += Convert.ToInt32(rdr["PlanQty"]);
            }
            rdr.Close();

            cmd = new SQLiteCommand("SELECT w.LineCode, w.Shift, r.GoodQty, r.DefectQty, r.StartTime, r.EndTime FROM ProductionResult r "
                + "INNER JOIN WorkOrder w ON r.WorkOrderId = w.WorkOrderId WHERE r.EnteredAt LIKE '" + date + "%'", conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int li = G.LineIndex(rdr["LineCode"].ToString());
                if (li < 0) continue;
                int si = rdr["Shift"].ToString() == "야간" ? 1 : 0;
                good[li, si] += Convert.ToInt32(rdr["GoodQty"]);
                bad[li, si] += Convert.ToInt32(rdr["DefectQty"]);
                work[li, si] += G.CalcMinutes(rdr["StartTime"].ToString(), rdr["EndTime"].ToString());
            }
            rdr.Close();

            dtReport = new DataTable();
            dtReport.Columns.Add("라인");
            dtReport.Columns.Add("라인명");
            dtReport.Columns.Add("교대");
            dtReport.Columns.Add("계획수량");
            dtReport.Columns.Add("양품수량");
            dtReport.Columns.Add("불량수량");
            dtReport.Columns.Add("달성률(%)");
            dtReport.Columns.Add("불량률(%)");
            dtReport.Columns.Add("가동률(%)");

            int tPlan = 0, tGood = 0, tBad = 0, tWork = 0, tAvail = 0;
            for (int li = 0; li < n; li++)
            {
                for (int si = 0; si < 2; si++)
                {
                    int avail = G.ShiftMinutes(si);
                    double ach = 0, def = 0, op = 0;
                    if (plan[li, si] > 0)
                        ach = Math.Round(good[li, si] * 100.0 / plan[li, si], 1);
                    if (good[li, si] + bad[li, si] > 0)
                        def = Math.Round(bad[li, si] * 100.0 / (good[li, si] + bad[li, si]), 1);
                    if (avail > 0)
                        op = Math.Round(work[li, si] * 100.0 / avail, 1);

                    dtReport.Rows.Add(G.LineCodes[li], G.LineNames[li], si == 0 ? "주간" : "야간",
                        plan[li, si].ToString(), good[li, si].ToString(), bad[li, si].ToString(),
                        ach.ToString("0.0"), def.ToString("0.0"), op.ToString("0.0"));

                    tPlan += plan[li, si];
                    tGood += good[li, si];
                    tBad += bad[li, si];
                    tWork += work[li, si];
                    tAvail += avail;
                }
            }

            double tAch = 0, tDef = 0, tOp = 0;
            if (tPlan > 0) tAch = Math.Round(tGood * 100.0 / tPlan, 1);
            if (tGood + tBad > 0) tDef = Math.Round(tBad * 100.0 / (tGood + tBad), 1);
            if (tAvail > 0) tOp = Math.Round(tWork * 100.0 / tAvail, 1);
            dtReport.Rows.Add("합계", "", "", tPlan.ToString(), tGood.ToString(), tBad.ToString(),
                tAch.ToString("0.0"), tDef.ToString("0.0"), tOp.ToString("0.0"));

            dtDefect = new DataTable();
            SQLiteDataAdapter da = new SQLiteDataAdapter(
                "SELECT r.DefectCode AS 불량유형, d.Name AS 유형명, SUM(r.DefectQty) AS 수량 FROM ProductionResult r "
                + "LEFT JOIN DefectCode d ON r.DefectCode = d.Code "
                + "WHERE r.EnteredAt LIKE '" + date + "%' AND r.DefectQty > 0 GROUP BY r.DefectCode, d.Name ORDER BY r.DefectCode", conn);
            da.Fill(dtDefect);
            conn.Close();

            dgvReport.DataSource = dtReport;
            dgvDefect.DataSource = dtDefect;
            lblTitle.Text = date + " 일별 생산 실적";
        }

        private void btnCsv_Click(object sender, EventArgs e)
        {
            if (sDate == "")
            {
                MessageBox.Show("먼저 리포트를 조회하세요.", "일별 리포트");
                return;
            }
            try
            {
                if (!Directory.Exists(G.ExportDir))
                    Directory.CreateDirectory(G.ExportDir);
                string path = G.ExportDir + "일별리포트_" + sDate.Replace("-", "") + ".csv";
                ExportCsv(path);
                MessageBox.Show("저장되었습니다.\n" + path, "일별 리포트");
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장 중 오류가 발생했습니다.\n" + ex.Message, "오류");
            }
        }

        public void ExportCsv(string path)
        {
            StreamWriter sw = new StreamWriter(path, false, Encoding.Default);
            sw.WriteLine("일별 생산 실적 리포트");
            sw.WriteLine("작업일," + sDate);
            sw.WriteLine();

            string line = "";
            for (int c = 0; c < dtReport.Columns.Count; c++)
            {
                if (c > 0) line += ",";
                line += dtReport.Columns[c].ColumnName;
            }
            sw.WriteLine(line);
            foreach (DataRow r in dtReport.Rows)
            {
                line = "";
                for (int c = 0; c < dtReport.Columns.Count; c++)
                {
                    if (c > 0) line += ",";
                    line += r[c].ToString();
                }
                sw.WriteLine(line);
            }

            sw.WriteLine();
            sw.WriteLine("불량유형,유형명,수량");
            foreach (DataRow r in dtDefect.Rows)
            {
                sw.WriteLine(r[0].ToString() + "," + r[1].ToString() + "," + r[2].ToString());
            }
            sw.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
