using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ProdMgr
{
    public class G
    {
        public static string IniPath = @"C:\ProdMgr\ProdMgr.ini";
        public static string DbPath = @"C:\ProdMgr\data\prodmgr.db";
        public static string ExportDir = @"C:\ProdMgr\export\";
        public static string UserName = "관리자";

        public static string[] LineCodes = new string[] { "L1", "L2", "L3" };
        public static string[] LineNames = new string[] { "프레스", "용접", "조립" };

        public static string DayStart = "08:00";
        public static string DayEnd = "20:00";
        public static string NightStart = "20:00";
        public static string NightEnd = "08:00";

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        public static string ReadIni(string section, string key, string def)
        {
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString(section, key, def, sb, 255, IniPath);
            return sb.ToString();
        }

        public static void WriteIni(string section, string key, string val)
        {
            WritePrivateProfileString(section, key, val, IniPath);
        }

        public static void LoadConfig()
        {
            if (!File.Exists(IniPath))
                IniPath = Application.StartupPath + "\\ProdMgr.ini";

            DbPath = ReadIni("DB", "Path", DbPath);
            ExportDir = ReadIni("EXPORT", "Dir", ExportDir);
            UserName = ReadIni("USER", "Name", UserName);

            int cnt = int.Parse(ReadIni("LINE", "Count", "3"));
            LineCodes = new string[cnt];
            LineNames = new string[cnt];
            for (int i = 0; i < cnt; i++)
            {
                string[] s = ReadIni("LINE", "Line" + (i + 1), "").Split(',');
                LineCodes[i] = s[0];
                LineNames[i] = s.Length > 1 ? s[1] : s[0];
            }

            DayStart = ReadIni("SHIFT", "DayStart", DayStart);
            DayEnd = ReadIni("SHIFT", "DayEnd", DayEnd);
            NightStart = ReadIni("SHIFT", "NightStart", NightStart);
            NightEnd = ReadIni("SHIFT", "NightEnd", NightEnd);
        }

        public static string ConnStr()
        {
            return "Data Source=" + DbPath + ";Version=3;";
        }

        public static int LineIndex(string code)
        {
            for (int i = 0; i < LineCodes.Length; i++)
                if (LineCodes[i] == code) return i;
            return -1;
        }

        public static string LineName(string code)
        {
            int i = LineIndex(code);
            if (i < 0) return code;
            return LineNames[i];
        }

        // shift: 0 = 주간, 1 = 야간
        public static int ShiftMinutes(int shift)
        {
            string s = shift == 0 ? DayStart : NightStart;
            string e = shift == 0 ? DayEnd : NightEnd;
            return CalcMinutes(s, e);
        }

        public static int CalcMinutes(string start, string end)
        {
            DateTime ds = DateTime.ParseExact(start, "HH:mm", null);
            DateTime de = DateTime.ParseExact(end, "HH:mm", null);
            int m = (int)(de - ds).TotalMinutes;
            if (m < 0) m += 1440;
            return m;
        }
    }
}
