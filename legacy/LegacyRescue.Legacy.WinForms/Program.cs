using System;
using System.IO;
using System.Windows.Forms;

namespace ProdMgr
{
    static class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            G.LoadConfig();

            if (args.Length >= 3 && args[0] == "--export-golden")
                return GoldenExport.Run(args[1], args[2]);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!File.Exists(G.DbPath))
            {
                MessageBox.Show("DB 파일을 찾을 수 없습니다.\n" + G.DbPath, "생산실적관리", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }

            Application.Run(new MainForm());
            return 0;
        }
    }
}
