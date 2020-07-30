using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace FFXIVBackupTool
{
    public partial class Form4 : Form
    {
        public static IPublicClientApplication PublicClientApp;
        public static string ClientId = "954b1a06-5cbe-42e1-9061-d16329fe40e6";
        public static string[] scopes = { "user.read", "Files.Read.All", "Files.ReadWrite.All", "Sites.Read.All", "Sites.ReadWrite.All" };//权限
        public string AccessToken;
        public Form4()
        {
            InitializeComponent();
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            PublicClientApp = PublicClientApplicationBuilder.Create(ClientId)
            .WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
            .Build();
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
            DialogResult result = MessageBox.Show("即将把当前目录下的备份文件上传到当前用户OneDrive网盘，特别提醒：由于本功能为实验性功能，可能会有诸多不稳定因素，请不要过于依赖该功能！\n\n确认要开始备份吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                if (System.IO.File.Exists(@".\FFXIVBackupPackage-CHN.zip") && System.IO.File.Exists(@".\FFXIVBackupPackage-Intl.zip"))
                {
                    toolStripStatusLabel1.Text = "正在上传文件，在提示完成前请勿关闭本窗口！";
                    UploadToOneDrive(@".\FFXIVBackupPackage-CHN.zip", @"/FFXIVBackupPackage-CHN.zip");
                    UploadToOneDrive(@".\FFXIVBackupPackage-Intl.zip", @"/FFXIVBackupPackage-Intl.zip");
                }
                else
                {
                    MessageBox.Show("请将国服和国际服都备份后再上传！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
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
            using (var fileStream = System.IO.File.OpenRead(filepath))
            {
                // Use properties to specify the conflict behavior
                // in this case, replace
                var uploadProps = new DriveItemUploadableProperties
                {
                    ODataType = null,
                    AdditionalData = new Dictionary<string, object>
        {
            { "@microsoft.graph.conflictBehavior", "replace" }
        }
                };
                // Create the upload session
                // itemPath does not need to be a path to an existing item
                var uploadSession = await graphServiceClient.Me.Drive.Root.ItemWithPath(onedrivepath).CreateUploadSession(uploadProps).Request().PostAsync();
                // Max slice size must be a multiple of 320 KiB
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
        }
        private void PictureBox2_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
        private void PictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private async void PictureBox1_Click_1(object sender, EventArgs e)
        {
            AuthenticationResult authResult = null;
            try
            {
                authResult = await PublicClientApp.AcquireTokenInteractive(scopes).ExecuteAsync();
            }
            catch (MsalException msalex)
            {
                if (msalex.ErrorCode == MsalError.AuthenticationCanceledError)
                {
                    //MessageBox.Show("中止登录");
                    toolStripStatusLabel1.Text = "中止登录";
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
            if (authResult != null)
            {
                toolStripStatusLabel1.Text = "当前登录账户：" + authResult.Account.Username;
                AccessToken = authResult.AccessToken;
                pictureBox1.Enabled = false;
                button2.Visible = true;
            }
        }
    }
}
