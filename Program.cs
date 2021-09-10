using System.Runtime.Versioning;
using System.Windows.Forms;

namespace BackupTool
{
    [SupportedOSPlatform("windows")]
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
