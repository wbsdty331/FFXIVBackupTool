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
            Close();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/wbsdty331/FFXIVBackupTool",
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
    }
}
