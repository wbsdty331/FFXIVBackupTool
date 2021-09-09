using BackupTool;
using System;
using System.Windows.Forms;
using System.Runtime.Versioning;
namespace FFXIVBackupTool
{
    [SupportedOSPlatform("windows")]
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            label1.Text += " " + Form1.ToolVersion;
        }
        private void label1_Click(object sender, EventArgs e)
        {
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
            System.Diagnostics.Process.Start("https://bbs.nga.cn/read.php?tid=22513951");
        }
    }
}
