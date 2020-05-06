using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeanMsalWithWinForm.Client
{
    public partial class Form1 : Form
    {
        private static string ClientId = "171e1cf7-cdff-427f-bbfb-e546663c9f4d";
        private static string Tenant = "028db01b-7420-42ce-ba2e-6efb6ac11c10";
        private static IPublicClientApplication _clientApp;
        public static IPublicClientApplication PublicClientApp { get { return _clientApp; } }

        public Form1()
        {
            InitializeComponent();
            _clientApp = PublicClientApplicationBuilder.Create(ClientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, Tenant)
                .Build();
            try
            {
                var account = _clientApp.GetAccountsAsync().Result;
                var token = _clientApp.AcquireTokenInteractive(new List<string> { "api://171e1cf7-cdff-427f-bbfb-e546663c9f4d/read" }).ExecuteAsync().Result;
            }
            catch (MsalUiRequiredException me)
            {
                throw me;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}
