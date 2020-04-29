using System;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO.Compression;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.ComponentModel;

namespace BackupTool1
{
    public partial class Form1 : Form
    {
        //国服线程打包
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
                MessageBox.Show("备份出错，请关闭游戏后再试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

        }
        //国服线程打包完毕
        void CompressDone(object sender, RunWorkerCompletedEventArgs e)
        {
            label3.Visible = false;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button7.Enabled = true;
            MessageBox.Show("国服数据已备份至当前目录下FFXIVBackupPackage-CHN.zip", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        //国际服线程打包 
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

       //国际服线程打包完毕 
        void CompressIntlDone(object sender, RunWorkerCompletedEventArgs e)
        {
            label3.Visible = false;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button8.Enabled = true;
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
            label3.Visible = false;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button8.Enabled = true;
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
            label3.Visible = false;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button7.Enabled = true;
            MessageBox.Show("国服数据已还原", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //国服策略，读注册表Uninstall键值获取目录
            string InstallPath = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\FFXIV", "UninstallString", "Default if TestExpand does not exist.");
            if (InstallPath == null)
            {
                MessageBox.Show("没有读取到安装目录，请手动选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else
            {
                //国服的文件在安装目录下，去除后面的Uninst.exe，加上game\My Games
                textBox1.Text = InstallPath.Replace("uninst.exe", "") + "game\\My Games";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //国际服策略，读取环境变量拼接目录
            string userfolder = Environment.GetEnvironmentVariable("USERPROFILE");
            if (Directory.Exists(userfolder + "\\Documents\\My Games\\FINAL FANTASY XIV - A Realm Reborn"))
            {
                textBox2.Text = userfolder + "\\Documents\\My Games";
            }
            else
            {
                MessageBox.Show("没有读取到国际服用户数据目录，请手动选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //先判断输入框有没有路径，没有就不做任何操作
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先选择或获取国服游戏路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else
            {
                //判断当前目录是否存在
                if (File.Exists(@".\FFXIVBackupPackage-CHN.zip"))
                {
                    DialogResult result = MessageBox.Show("当前目录下已有国服数据备份，是否覆盖？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.No)
                    {
                        return;

                    }
                }
                label3.Visible = true;
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button7.Enabled = false;
                File.Delete(@".\FFXIVBackupPackage-CHN.zip");
                using (BackgroundWorker bw = new BackgroundWorker())
                {
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompressDone);
                    bw.DoWork += new DoWorkEventHandler(CompressFile);
                    bw.RunWorkerAsync();
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string defaultPath = "";
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择国服游戏目录";
            dialog.ShowNewFolderButton = false;
            if (defaultPath != "")
            {
                dialog.SelectedPath = defaultPath;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string defaultPath = "";
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择当前用户文档下的My Games文件夹";
            dialog.ShowNewFolderButton = false;
            if (defaultPath != "")
            {
                dialog.SelectedPath = defaultPath;
            }
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
                label3.Visible = true;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button8.Enabled = false;
                File.Delete(@".\FFXIVBackupPackage-Intl.zip");
                using (BackgroundWorker bw = new BackgroundWorker())
                {
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompressIntlDone);
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
                        MessageBox.Show("用户选择不还原");
                        return;
                    }
                
                }
                else
                {
                    MessageBox.Show("未发现数据包", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return;
                }
                MessageBox.Show("用户选择还原");
                //如果有目录先删除
                if (Directory.Exists(textBox1.Text + "\\FINAL FANTASY XIV - A Realm Reborn"))
                {
                    Directory.Delete(textBox1.Text + "\\FINAL FANTASY XIV - A Realm Reborn", true);
                }
                label3.Visible = true;
             button1.Enabled = false;
             button2.Enabled = false;
             button3.Enabled = false;
             button7.Enabled = false;
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
            }
            else
            {
                //判断当前目录是否存在还原包
                if (File.Exists(@".\FFXIVBackupPackage-Intl.zip"))
                {
                    DialogResult result = MessageBox.Show("你真的要还原国际服数据吗？原有数据将会被删除并覆盖！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.No)
                    {
                        MessageBox.Show("用户选择不还原");
                        return;
                    }

                }
                else
                {
                    MessageBox.Show("未发现数据包", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return;
                }
                MessageBox.Show("用户选择还原");
                //如果有目录先删除
               if(Directory.Exists(textBox2.Text + "\\FINAL FANTASY XIV - A Realm Reborn"))
                {
                    Directory.Delete(textBox2.Text + "\\FINAL FANTASY XIV - A Realm Reborn", true);
                }
            
             label3.Visible = true;
             button4.Enabled = false;
             button5.Enabled = false;
             button6.Enabled = false;
             button8.Enabled = false;
             using (BackgroundWorker bw2 = new BackgroundWorker())
             {
                   
                 bw2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RestoreFileIntlDone);
                 bw2.DoWork += new DoWorkEventHandler(RestoreFileIntl);
                 bw2.RunWorkerAsync();
                 
            }

            }
        }
    }
}
