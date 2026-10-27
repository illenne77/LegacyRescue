using System;
using System.Data.SQLite;
using System.IO;

namespace ProdMgr
{
    /// <summary>
    /// Characterization-test seam: the only code added to the legacy app that is not an original feature.
    /// It drives the existing report and list code paths without showing any window and writes their
    /// CSV output unchanged, so the golden files are exactly what users get from the export buttons.
    /// Usage: ProdMgr.exe --export-golden &lt;db&gt; &lt;outdir&gt;
    /// </summary>
    static class GoldenExport
    {
        public static int Run(string dbPath, string outDir)
        {
            try
            {
                if (!File.Exists(dbPath))
                    throw new FileNotFoundException("DB not found", dbPath);

                G.DbPath = Path.GetFullPath(dbPath);
                string dailyDir = Path.Combine(outDir, "daily");
                Directory.CreateDirectory(dailyDir);

                DateTime first, last;
                using (SQLiteConnection conn = new SQLiteConnection(G.ConnStr()))
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(
                        "SELECT MIN(WorkDate), MAX(MAX(WorkDate), (SELECT MAX(substr(EnteredAt, 1, 10)) FROM ProductionResult)) FROM WorkOrder", conn);
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        first = DateTime.ParseExact(rdr.GetString(0), "yyyy-MM-dd", null);
                        last = DateTime.ParseExact(rdr.GetString(1), "yyyy-MM-dd", null);
                    }
                }

                using (ReportForm report = new ReportForm())
                {
                    for (DateTime d = first; d <= last; d = d.AddDays(1))
                    {
                        string date = d.ToString("yyyy-MM-dd");
                        report.LoadReport(date);
                        report.ExportCsv(Path.Combine(dailyDir, date + ".csv"));
                    }
                }

                using (MainForm main = new MainForm())
                {
                    main.LoadList("0000-01-01", "9999-12-31", "");
                    main.SaveListCsv(Path.Combine(outDir, "workorders.csv"));
                }

                return 0;
            }
            catch (Exception ex)
            {
                Directory.CreateDirectory(outDir);
                File.WriteAllText(Path.Combine(outDir, "golden-error.txt"), ex.ToString());
                return 1;
            }
        }
    }
}
