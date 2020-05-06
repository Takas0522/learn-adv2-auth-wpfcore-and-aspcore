using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeanMsalWithWinForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static readonly string AadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static readonly string Tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static readonly string ClientId = ConfigurationManager.AppSettings["ida:ClientId"];

        private static readonly string Authority = string.Format(CultureInfo.InvariantCulture, AadInstance, Tenant);

        // To authenticate to the To Do list service, the client needs to know the service's App ID URI and URL

        private static readonly string WebApiScope = ConfigurationManager.AppSettings["api:WebApiScope"];
        private static readonly string WebApiBaseAddress = ConfigurationManager.AppSettings["api:WebApiBaseAddress"];
        private static readonly string[] Scopes = { WebApiScope };

        private readonly IPublicClientApplication _app;

        public MainWindow()
        {
            InitializeComponent();

            _app = PublicClientApplicationBuilder.Create(ClientId)
                .WithAuthority(Authority)
                .WithDefaultRedirectUri()
                .Build();
            GetData();
        }

        private async void GetData()
        {
            AuthenticationResult result = null;
            result = await _app.AcquireTokenInteractive(Scopes).ExecuteAsync();

            string token = result.AccessToken;
            string url = $"{WebApiBaseAddress}WeatherForecast";
            var data = await GetData(url, token);

        }

        private async Task<string> GetData(string url, string token)
        {
            var httpClient = new HttpClient();
            HttpResponseMessage response;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
