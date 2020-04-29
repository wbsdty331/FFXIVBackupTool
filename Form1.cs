using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
namespace BackupTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string path = ReadGamePath(1);
            if (path == null)
            {
                MessageBox.Show("没有读取到国服游戏目录，请手动选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            textBox1.Text = path;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string path = ReadGamePath(2);
            if (path == null)
            {
                MessageBox.Show("没有读取到国际服游戏目录，请手动选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            textBox2.Text = path;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //先判断输入框有没有路径，没有就不做任何操作
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先选择或获取国服游戏路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            //判断当前目录是否存在
            if (File.Exists(@".\FFXIVBackupPackage-CHN.zip"))
            {
                DialogResult result = MessageBox.Show("当前目录下已有国服数据备份，是否覆盖？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            CHNButtonDisable();
            File.Delete(@".\FFXIVBackupPackage-CHN.zip");
            using (BackgroundWorker bw = new BackgroundWorker())
            {
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompressFileDone);
                bw.DoWork += new DoWorkEventHandler(CompressFile);
                bw.RunWorkerAsync();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择国服游戏目录";
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择当前用户文档下的My Games文件夹";
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dialog.SelectedPath;
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            //先判断输入框有没有路径，没有就不做任何操作
            if (textBox2.Text == "")
            {
                MessageBox.Show("请先选择或获取国际服游戏路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else
            {
                //判断当前目录是否存在文件
                if (File.Exists(@".\FFXIVBackupPackage-Intl.zip"))
                {
                    DialogResult result = MessageBox.Show("当前目录下已有国际服备份文件，是否覆盖？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }
                IntlButtonDisable();
                File.Delete(@".\FFXIVBackupPackage-Intl.zip");
                using (BackgroundWorker bw = new BackgroundWorker())
                {
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompressIntlFileDone);
                    bw.DoWork += new DoWorkEventHandler(CompressIntlFile);
                    bw.RunWorkerAsync();
                }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            //先判断输入框有没有路径，没有就不做任何操作
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先选择或获取国服游戏路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else
            {
                //判断当前目录是否存在还原包
                if (File.Exists(@".\FFXIVBackupPackage-CHN.zip"))
                {
                    DialogResult result = MessageBox.Show("你真的要还原国服数据吗？原有数据将会被删除并覆盖！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("未发现国服备份数据包", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return;
                }
                //如果有目录先删除
                if (Directory.Exists(textBox1.Text + "\\FINAL FANTASY XIV - A Realm Reborn"))
                {
                    Directory.Delete(textBox1.Text + "\\FINAL FANTASY XIV - A Realm Reborn", true);
                }
                CHNButtonDisable();
                using (BackgroundWorker bw = new BackgroundWorker())
                {
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RestoreFileDone);
                    bw.DoWork += new DoWorkEventHandler(RestoreFile);
                    bw.RunWorkerAsync();
                }
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            //先判断输入框有没有路径，没有就不做任何操作
            if (textBox2.Text == "")
            {
                MessageBox.Show("请先选择或获取国际服游戏路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            //判断当前目录是否存在还原包
            if (File.Exists(@".\FFXIVBackupPackage-Intl.zip"))
            {
                DialogResult result = MessageBox.Show("你真的要还原国际服数据吗？原有数据将会被删除并覆盖！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("未发现国际服备份数据包", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            //如果有目录先删除
            if (Directory.Exists(textBox2.Text + "\\FINAL FANTASY XIV - A Realm Reborn"))
            { Directory.Delete(textBox2.Text + "\\FINAL FANTASY XIV - A Realm Reborn", true); }
            IntlButtonDisable();
            using (BackgroundWorker bw2 = new BackgroundWorker())
            {
                bw2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RestoreFileIntlDone);
                bw2.DoWork += new DoWorkEventHandler(RestoreFileIntl);
                bw2.RunWorkerAsync();
            }
        }
        /* 以下是公共类 */
        //国服相关控件置灰
        public void CHNButtonDisable()
        {
            label3.Visible = true;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button7.Enabled = false;
        }
        //国服相关控件还原
        public void CHNButtonEnable()
        {
            label3.Visible = false;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button7.Enabled = true;
        }
        //国际服相关控件置灰
        public void IntlButtonDisable()
        {
            label3.Visible = true;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button8.Enabled = false;
        }
        //国际服相关控件还原
        public void IntlButtonEnable()
        {
            label3.Visible = false;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button8.Enabled = true;
        }
        //国服打包
        void CompressFile(object sender, DoWorkEventArgs e)
        {
            try
            {
                string startPath = textBox1.Text;
                string zipPath = @".\FFXIVBackupPackage-CHN.zip";
                ZipFile.CreateFromDirectory(startPath, zipPath);
            }
            catch
            {
                MessageBox.Show("备份失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }
        //国服打包完毕
        void CompressFileDone(object sender, RunWorkerCompletedEventArgs e)
        {
            CHNButtonEnable();
            MessageBox.Show("国服数据已备份至当前目录下FFXIVBackupPackage-CHN.zip", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        //国际服打包 
        void CompressIntlFile(object sender, DoWorkEventArgs e)
        {
            try
            {
                string startPath = textBox2.Text + "\\FINAL FANTASY XIV - A Realm Reborn";
                string zipPath = @".\FFXIVBackupPackage-Intl.zip";
                ZipFile.CreateFromDirectory(startPath, zipPath, 0, true);
            }
            catch
            {
                MessageBox.Show("备份失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }
        //国际服打包完毕 
        void CompressIntlFileDone(object sender, RunWorkerCompletedEventArgs e)
        {
            //置灰的按钮还原
            IntlButtonEnable();
            MessageBox.Show("国际服数据已备份至当前目录下FFXIVBackupPackage-Intl.zip", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        //国际服还原
        void RestoreFileIntl(object sender, DoWorkEventArgs e)
        {
            string zipPath = @".\FFXIVBackupPackage-Intl.zip";
            string extractPath = textBox2.Text;
            ZipFile.ExtractToDirectory(zipPath, extractPath);
        }
        //国际服还原完毕
        void RestoreFileIntlDone(object sender, RunWorkerCompletedEventArgs e)
        {
            //置灰的按钮还原
            IntlButtonEnable();
            MessageBox.Show("国际服数据已还原", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        //国服还原
        void RestoreFile(object sender, DoWorkEventArgs e)
        {
            string zipPath = @".\FFXIVBackupPackage-CHN.zip";
            string extractPath = textBox1.Text;
            ZipFile.ExtractToDirectory(zipPath, extractPath);
        }
        //国服还原完毕
        void RestoreFileDone(object sender, RunWorkerCompletedEventArgs e)
        {
            CHNButtonEnable();
            MessageBox.Show("国服数据已还原", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        //读目录策略
        public string ReadGamePath(int a)
        {
            if (a == 1)
            {
                //国服策略，通过注册表读目录
                try
                {
                    string value32 = String.Empty;
                    RegistryKey localKey32 = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
                    localKey32 = localKey32.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\FFXIV");
                    if (localKey32 != null)
                    {
                        value32 = localKey32.GetValue("UninstallString").ToString();
                        localKey32.Close();
                    }
                    return value32.Replace("uninst.exe", "") + "game\\My Games";
                }
                catch
                {
                    return null;
                }
            }
            else if (a == 2)
            {
            }
            //国际服策略，读取环境变量拼接目录
            try
            {
                string userfolder = Environment.GetEnvironmentVariable("USERPROFILE");
                if (Directory.Exists(userfolder + "\\Documents\\My Games\\FINAL FANTASY XIV - A Realm Reborn"))
                {
                    return userfolder + "\\Documents\\My Games";
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
    }
}
