using System;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Windows.Forms;
using BackupTool;
namespace FFXIVBackupTool
{
    [SupportedOSPlatform("windows")]
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form license = new Form3();
            license.Show();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://bbs.nga.cn/read.php?tid=22513951",
                UseShellExecute = true
            });
        }

        private void Form2_Load(object sender, System.EventArgs e)
        {
            label1.Text += " " + Form1.ToolVersion;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form license = new Form3();
            license.Show();
        }
    }
}
