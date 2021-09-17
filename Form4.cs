using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Graph;
using Microsoft.Identity.Client;
namespace FFXIVBackupTool
{
    public partial class Form4 : Form
    {
        private static IPublicClientApplication publicClientApp;
        public string ClientId = "954b1a06-5cbe-42e1-9061-d16329fe40e6";
        public string[] scopes = { "user.read", "Files.Read.All", "Files.ReadWrite.All", "Sites.Read.All", "Sites.ReadWrite.All" };//权限
        public string AccessToken;

        public static IPublicClientApplication PublicClientApp { get => publicClientApp; set => publicClientApp = value; }

        public Form4()
        {
            InitializeComponent();
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            PublicClientApp = PublicClientApplicationBuilder.Create(ClientId).WithRedirectUri("http://localhost").Build();
            

        }
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void PictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked || checkBox2.Checked)
            {
                var result = MessageBox.Show("即将把当前目录下的备份文件上传到当前用户OneDrive网盘。\n\n提醒：由于本功能为实验性功能，可能会有诸多不稳定因素，请不要过于依赖该功能！\n\n确认要开始备份吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    toolStripSplitButton1.Visible = false;
                    //看国服是否选择了
                    if (checkBox1.Checked)
                    {
                        if (System.IO.File.Exists(@".\FFXIVBackupPackage-CHN.zip")){
                            toolStripStatusLabel1.Text = "正在上传国服文件，在提示完成前请勿关闭本窗口！";
                            UploadToOneDrive(@".\FFXIVBackupPackage-CHN.zip", @"/FFXIVBackupPackage-CHN.zip");
                        }
                        else
                        {
                            MessageBox.Show("国服备份文件不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    if (checkBox2.Checked)
                    {
                        if (System.IO.File.Exists(@".\FFXIVBackupPackage-Intl.zip"))
                        {
                            toolStripStatusLabel1.Text = "正在上传国际服文件，在提示完成前请勿关闭本窗口！";
                            UploadToOneDrive(@".\FFXIVBackupPackage-Intl.zip", @"/FFXIVBackupPackage-Intl.zip");
                        }
                        else
                        {
                            MessageBox.Show("国际服备份文件不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    
                }

            }
            else
            {
                MessageBox.Show("请选择要备份的功能！", "提示",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public async void UploadToOneDrive(string filepath, string onedrivepath)
        {
            var graphServiceClient = new GraphServiceClient(
            new DelegateAuthenticationProvider((requestMessage) =>
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", AccessToken);
                return Task.CompletedTask;
            }));
            //GraphServiceClient graphClient = new GraphServiceClient("https://graph.microsoft.com/v1.0", new DelegateAuthenticationProvider(async (requestMessage) =>
            //{
            //    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", AccessToken);
            //}));
            using var fileStream = System.IO.File.OpenRead(filepath);
            var uploadProps = new DriveItemUploadableProperties
            {
                ODataType = null,
                AdditionalData = new Dictionary<string, object>
                {
                    { "@microsoft.graph.conflictBehavior", "replace" }
                }
            };
            var uploadSession = await graphServiceClient.Me.Drive.Root.ItemWithPath(onedrivepath).CreateUploadSession(uploadProps).Request().PostAsync();
            // OneDrive 分片 320 * 1024KB
            int maxSliceSize = 320 * 1024;
            var fileUploadTask =
                new LargeFileUploadTask<DriveItem>(uploadSession, fileStream, maxSliceSize);
            try
            {
                // Upload the file
                var uploadResult = await fileUploadTask.UploadAsync();
                if (uploadResult.UploadSucceeded)
                {
                    toolStripStatusLabel1.Text = filepath + " 上传成功";
                    toolStripSplitButton1.Visible = true;
                }
                else
                {
                    MessageBox.Show("上传失败，请检查网络！");
                }
            }
            catch (ServiceException ex)
            {
                MessageBox.Show($"在上传过程中出现错误: {ex}");
            }
        }
        private async void PictureBox1_Click_1(object sender, EventArgs e)
        {
            var options = new SystemWebViewOptions()
            {
                HtmlMessageSuccess = "<b>Login Success, now you may close this window.</b>",
                HtmlMessageError = "<b>Authorization Failed!　Please try again?</b>",
            };
            AuthenticationResult authResult = null;
            var accounts = await PublicClientApp.GetAccountsAsync();
            try
            {
                authResult = await PublicClientApp.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                try
                {
                    toolStripStatusLabel1.Text = "请在新打开的浏览器窗口完成授权操作。";
                    authResult = await PublicClientApp.AcquireTokenInteractive(scopes).WithSystemWebViewOptions(options).ExecuteAsync();
                }
                catch (MsalClientException msalex)
                {
                    if (msalex.ErrorCode == MsalError.AuthenticationCanceledError)
                    {
                        toolStripStatusLabel1.Text = "用户中止登陆，操作取消";
                    }
                    else if (msalex.ErrorCode == MsalError.RequestTimeout)
                    {
                        toolStripStatusLabel1.Text = "登录超时，请重试！";
                    }
                    else if (msalex.ErrorCode == MsalError.AccessDenied)
                    {
                        toolStripStatusLabel1.Text = "用户拒绝授权本应用，请授权后再试！";
                    }
                }
            }
            if (authResult != null)
            {
                toolStripStatusLabel1.Text = "当前登录账户：" + authResult.Account.Username;
                AccessToken = authResult.AccessToken;
                pictureBox1.Enabled = false;
                button2.Visible = true;
                toolStripSplitButton1.Visible = true;
                checkBox1.Visible = true; 
                checkBox2.Visible = true;

            }
        }
        private async void 退出登录XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("确认要退出登录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                var accounts = (await publicClientApp.GetAccountsAsync()).ToList();
                toolStripStatusLabel1.Text = "已退出登录";
                while (accounts.Any())
                {
                    await publicClientApp.RemoveAsync(accounts.First());
                    accounts = (await publicClientApp.GetAccountsAsync()).ToList();
                }
                button2.Visible = false;
                pictureBox1.Enabled = true;
                toolStripSplitButton1.Visible = false;
                checkBox1.Visible = false;
                checkBox2.Visible = false;
            }
        }
    }
}
