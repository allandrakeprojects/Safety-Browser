using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Safety_Browser
{
    public partial class Form_Main : Form
    {
        private string[] web_service = { "http://www.ssicortex.com/GetTxt2Search", "www.ssitectonic.com/GetTxt2Search", "www.ssihedonic.com/GetTxt2Search" };
        private string[] domain_test = { "http://www.ssicortex.com/GetDomains", "www.ssitectonic.com/GetText2Search", "www.ssihedonic.com/GetText2Search" };
        private string text_search;
        private bool close = true;
        private bool isHijacked;
        private int current_domain_index = 0;
        private int total_domain_index;
        private int current_web_service = 0;
        private int current_domain = 0;
        private string domain_get;
        private bool last_index_hijacked_get = false;
        private bool networkIsAvailable;
        private bool completed = true;
        private bool timeout = true;
        private string webbrowser_handler_title;
        private Uri webbrowser_handler_url;
        private string replace_domain_get;
        private bool domain_one_time = true;
        private bool load_not_hijacked = false;
        private bool connection_handler = false;
        private string _mac_address;
        private string _external_ip;
        private string _city;
        private string _region;
        private string _country;
        private string BRAND_CODE = "YB";
        private string API_KEY_SSIHEDONIC = "0397c2be1d97aac330bc3d5278c47696";
        private string API_KEY_SSITECTONIC = "561b9fd16b50553213e4be2024fb4cf9";
        private string API_KEY_SSICORTEX = "6b8c7e5617414bf2d4ace37600b6ab71";

        public Form_Main()
        {
            InitializeComponent();
        }

        // Form Load
        private void Form_Main_Load(object sender, EventArgs e)
        {
            NetworkAvailability();
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            GetTextToTextAsync(web_service[current_web_service]);
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            PictureBoxCenter();
        }

        // Network Handler
        private void NetworkAvailability()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface nic in nics)
            {
                if (
                    (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel) &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    networkIsAvailable = true;
                }
            }

            if (networkIsAvailable)
            {
                GetIPInfo();
            }
            else
            {
                timer_handler.Stop();

                webBrowser_handler.Visible = false;
                pictureBox_loader.Visible = false;

                panel_connection.Visible = true;
                panel_connection.Enabled = true;
            }

            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
        }

        // Network Handler Changed
        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            Invoke(new Action(() =>
            {
                networkIsAvailable = e.IsAvailable;

                if (networkIsAvailable)
                {
                    if (dataGridView_domain.RowCount == 0 && last_index_hijacked_get)
                    {
                        if (current_web_service < web_service.Length)
                        {
                            ByPassCalling();
                        
                            panel_connection.Visible = false;
                            panel_connection.Enabled = false;
                        }
                        else
                        {
                            panel_connection.Visible = false;
                            panel_connection.Enabled = false;

                            webBrowser_handler.Visible = false;
                            pictureBox_loader.Visible = false;
                        }
                    }
                    else if (dataGridView_domain.RowCount == 0 && !connection_handler)
                    {
                        #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        GetTextToTextAsync(web_service[current_web_service]);
                        #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                        panel_connection.Visible = false;
                        panel_connection.Enabled = false;

                        webBrowser_handler.Visible = false;
                        pictureBox_loader.Visible = false;
                    }
                    else if (!last_index_hijacked_get)
                    {
                        panel_connection.Visible = false;
                        panel_connection.Enabled = false;
                        pictureBox_loader.Visible = false;
                        
                        webBrowser_handler.Visible = true;
                    }
                    else
                    {
                        webBrowser_handler.Visible = false;
                        panel_connection.Visible = false;
                        panel_connection.Enabled = false;

                        if (webBrowser_handler.Visible == true)
                        {
                            pictureBox_loader.Visible = false;
                        }
                        else
                        {
                            if (!connection_handler)
                            {
                                pictureBox_loader.Visible = true;
                            }
                            else
                            {
                                webBrowser_handler.Visible = true;
                            }
                        }

                        if (!connection_handler)
                        {
                            dataGridView_domain.ClearSelection();
                            dataGridView_domain.Rows[current_domain_index].Selected = true;
                        }
                    }
                }
                else
                {
                    webBrowser_handler.Stop();
                    timer_handler.Stop();

                    webBrowser_handler.Visible = false;
                    pictureBox_loader.Visible = false;

                    panel_connection.Visible = true;
                    panel_connection.Enabled = true;
                }
            }));
        }

        // Loader Center
        private void PictureBoxCenter()
        {
            pictureBox_loader.Left = (ClientSize.Width - pictureBox_loader.Width) / 2;
            pictureBox_loader.Top = (ClientSize.Height - pictureBox_loader.Height) / 2;

            panel_connection.Left = (ClientSize.Width - panel_connection.Width) / 2;
            panel_connection.Top = (ClientSize.Height - panel_connection.Height) / 2;
        }

        // Get Text to Search
        private async Task GetTextToTextAsync(string web_service_test)
        {
            if (networkIsAvailable)
            {
                label_current_web_service.Text = web_service_test;

                try
                {
                    var client = new HttpClient();
                    var requestContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("api_key", API_KEY_SSICORTEX),
                    new KeyValuePair<string, string>("brand_code", BRAND_CODE),
                });

                    HttpResponseMessage response = await client.PostAsync(
                        web_service_test,
                        requestContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        text_search = "";
                        close = true;
                        current_domain_index = 0;
                        total_domain_index = 0;

                        dataGridView_domain.DataSource = null;
                        current_web_service++;
                        current_domain++;

                        if (current_web_service < web_service.Length)
                        {
                            ByPassCalling();
                        }
                        else
                        {
                            pictureBox_loader.Visible = false;
                            pictureBox_loader.Enabled = false;
                            connection_handler = true;
                        }

                        return;
                    }

                    HttpContent responseContent = response.Content;

                    using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                    {
                        string json = await reader.ReadToEndAsync();
                        string pattern = @"\[(.*?)\]";
                        var matches = Regex.Matches(json, pattern);

                        foreach (Match m in matches)
                        {
                            text_search = Regex.Unescape(m.Groups[1].ToString().Replace("\"", ""));
                        }

                        if (!String.IsNullOrEmpty(text_search))
                        {
                            // Get Domains
                            await GetDomainsAsync(domain_test[current_domain]);
                            total_domain_index = dataGridView_domain.RowCount;
                            dataGridView_domain.ClearSelection();
                            dataGridView_domain.Rows[current_domain_index].Selected = true;
                            domain_one_time = true;
                        }
                        else
                        {
                            text_search = "";
                            close = true;
                            current_domain_index = 0;
                            total_domain_index = 0;

                            dataGridView_domain.DataSource = null;
                            current_web_service++;
                            current_domain++;

                            if (current_web_service < web_service.Length)
                            {
                                ByPassCalling();
                            }
                            else
                            {
                                pictureBox_loader.Visible = false;
                                pictureBox_loader.Enabled = false;
                                connection_handler = true;
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1001", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    close = false;
                    Close();
                }
            }
            else
            {
                timer_handler.Stop();

                webBrowser_handler.Visible = false;
                pictureBox_loader.Visible = false;

                panel_connection.Visible = true;
                panel_connection.Enabled = true;
            }
        }

        // ByPass Calling
        private void ByPassCalling()
        {
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            GetTextToTextAsync(web_service[current_web_service]);
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        // Get Domain
        private async Task GetDomainsAsync(string test_domain)
        {
            label_current_domain_service.Text = test_domain;

            try
            {
                var client = new HttpClient();
                var requestContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("api_key", API_KEY_SSICORTEX),
                    new KeyValuePair<string, string>("brand_code", BRAND_CODE),
                });

                HttpResponseMessage response = await client.PostAsync(
                    test_domain,
                    requestContent);

                HttpContent responseContent = response.Content;

                using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                {
                    string json = await reader.ReadToEndAsync();
                    string pattern = @"\[(.*?)\]";
                    var matches = Regex.Matches(json, pattern);
                    string domain_get = string.Empty;

                    foreach (Match m in matches)
                    {
                        domain_get = Regex.Unescape(m.Groups[1].ToString().Replace("\"", ""));
                    }

                    StringBuilder sb = new StringBuilder(domain_get);
                    sb.Replace("domain_ur", "");
                    sb.Replace("\"", "");
                    sb.Replace("l:", "");
                    sb.Replace("{", "");
                    sb.Replace("}", "");
                    
                    var elements = sb.ToString().Split(new[]
                    { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string items in elements)
                    {
                        dataGridView_domain.Rows.Add(items);
                    }

                    //int count = jArray.Count;
                    //int i = 0;

                    //while (i < count)
                    //{
                    //    string get = jArray[i]["domain_name"].Value<string>();
                    //    dataGridView_domain.ClearSelection();
                    //    dataGridView_domain.Rows.Add(get);
                    //    i++;
                    //}
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1002", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        
        // Domain Selection Changed
        private void DataGridView_domain_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_domain.CurrentCell == null || dataGridView_domain.CurrentCell.Value == null)
            {
                return;
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView_domain.SelectedRows)
                {
                    try
                    {
                        domain_get = row.Cells[0].Value.ToString();

                        webBrowser_handler.Visible = false;

                        pictureBox_loader.Visible = true;

                        timer_handler.Stop();
                        timer_handler.Start();

                        completed = true;
                        timeout = true;

                        webBrowser_handler.Navigate(domain_get);
                    }
                    catch (Exception)
                    {
                        // Leave blank
                    }
                }
            }
        }

        // asd123
        // Web Browser Loaded
        private void WebBrowser_handler_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (domain_one_time)
            {
                if (completed)
                {
                    if (webBrowser_handler.ReadyState == WebBrowserReadyState.Complete)
                    {
                        if (e.Url == webBrowser_handler.Url)
                        {
                            // handlers
                            webbrowser_handler_title = webBrowser_handler.DocumentTitle;
                            webbrowser_handler_url = webBrowser_handler.Url;
                            timeout = false;
                            timer_handler.Stop();

                            string strValue = text_search;
                            string[] strArray = strValue.Split(',');

                            foreach (string obj in strArray)
                            {
                                bool contains = webbrowser_handler_title.Contains(obj);

                                if (contains == true)
                                {
                                    Invoke(new Action(() =>
                                    {
                                        isHijacked = false;
                                    }));

                                    break;
                                }
                                else if (!contains)
                                {
                                    Invoke(new Action(() =>
                                    {
                                        isHijacked = true;
                                    }));
                                }
                            }

                            if (isHijacked)
                            {
                                var html = "";

                                if (!domain_get.Contains("http"))
                                {
                                    try
                                    {
                                        replace_domain_get = "http://" + domain_get;
                                        html = new WebClient().DownloadString(replace_domain_get);
                                    }
                                    catch (Exception)
                                    {
                                        html = "";
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        html = new WebClient().DownloadString(domain_get);
                                    }
                                    catch (Exception)
                                    {
                                        html = "";
                                    }
                                }

                                if (html.Contains("landing_image"))
                                {
                                    domain_one_time = false;
                                    last_index_hijacked_get = false;
                                    pictureBox_loader.Visible = false;
                                    webBrowser_handler.Visible = true;
                                }
                                else
                                {
                                    last_index_hijacked_get = true;
                                    label_loadingstate.Text = "1";
                                    label_loadingstate.Text = "0";
                                }
                            }
                            else
                            {
                                domain_one_time = false;
                                last_index_hijacked_get = false;
                                pictureBox_loader.Visible = false;
                                webBrowser_handler.Visible = true;
                            }
                        }
                    }
                }
            }
            else if (load_not_hijacked)
            {
                if (webBrowser_handler.ReadyState == WebBrowserReadyState.Complete)
                {
                    if (e.Url == webBrowser_handler.Url)
                    {
                        pictureBox_loader.Visible = false;
                        webBrowser_handler.Visible = true;
                        load_not_hijacked = false;
                    }
                }
            }
        }

        // Timeout
        private void Timer_handler_Tick(object sender, EventArgs e)
        {
            if (domain_one_time)
            {
                if (timeout)
                {
                    // handlers
                    webbrowser_handler_title = webBrowser_handler.DocumentTitle;
                    webbrowser_handler_url = webBrowser_handler.Url;
                    completed = false;
                    webBrowser_handler.Stop();
                    timer_handler.Stop();

                    string strValue = text_search;
                    string[] strArray = strValue.Split(',');

                    foreach (string obj in strArray)
                    {
                        bool contains = webbrowser_handler_title.Contains(obj);

                        if (contains == true)
                        {
                            Invoke(new Action(() =>
                            {
                                isHijacked = false;
                            }));

                            break;
                        }
                        else if (!contains)
                        {
                            Invoke(new Action(() =>
                            {
                                isHijacked = true;
                            }));
                        }
                    }

                    if (isHijacked)
                    {
                        var html = "";

                        if (!domain_get.Contains("http"))
                        {
                            try
                            {
                                replace_domain_get = "http://" + domain_get;
                                html = new WebClient().DownloadString(replace_domain_get);
                            }
                            catch (Exception)
                            {
                                html = "";
                            }
                        }
                        else
                        {
                            try
                            {
                                html = new WebClient().DownloadString(domain_get);
                            }
                            catch (Exception)
                            {
                                html = "";
                            }
                        }

                        if (html.Contains("landing_image"))
                        {
                            domain_one_time = false;
                            last_index_hijacked_get = false;
                            pictureBox_loader.Visible = false;
                            webBrowser_handler.Visible = true;
                        }
                        else
                        {
                            last_index_hijacked_get = true;
                            label_loadingstate.Text = "1";
                            label_loadingstate.Text = "0";
                        }
                    }
                    else
                    {
                        domain_one_time = false;
                        last_index_hijacked_get = false;
                        pictureBox_loader.Visible = false;
                        webBrowser_handler.Visible = true;
                    }
                }
            }
            else if (load_not_hijacked)
            {
                webBrowser_handler.Stop();
                timer_handler.Stop();

                pictureBox_loader.Visible = false;
                webBrowser_handler.Visible = true;
                load_not_hijacked = false;
            }
        }

        // Loading State Changed
        private void Label_loadingstate_TextChanged(object sender, EventArgs e)
        {
            if (label_loadingstate.Text == "0")
            {
                current_domain_index = dataGridView_domain.SelectedRows[0].Index + 1;
                
                if (current_domain_index == total_domain_index)
                {
                    if (domain_one_time)
                    {
                        domain_one_time = false;
                        text_search = "";
                        close = true;
                        current_domain_index = 0;
                        total_domain_index = 0;

                        dataGridView_domain.Rows.Clear();
                        dataGridView_domain.Refresh();

                        current_web_service++;
                        current_domain++;

                        if (current_web_service < web_service.Length)
                        {
                            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                            GetTextToTextAsync(web_service[current_web_service]);
                            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        }
                        else
                        {
                            pictureBox_loader.Visible = false;
                            pictureBox_loader.Enabled = false;
                            connection_handler = true;
                        }
                    }
                }
                else
                {
                    dataGridView_domain.FirstDisplayedScrollingRowIndex = current_domain_index;
                    dataGridView_domain.ClearSelection();
                    dataGridView_domain.Rows[current_domain_index].Selected = true;
                }
            }
        }

        // Menu
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (close)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to exit the program?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    close = false;
                    //Cef.Shutdown();
                    Close();
                }
            }
            else
            {
                //Cef.Shutdown();
            }
        }

        // Form Closing
        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (close)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to exit the program?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    //Cef.Shutdown();
                }
            }
            else
            {
                //Cef.Shutdown();
            }
        }

        // Get external IP
        private string GetExternalIp()
        {
            try
            {
                string externalIP;
                externalIP = (new WebClient()).DownloadString("https://canihazip.com/s");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();
                
                return externalIP;
            }
            catch (Exception err)
            {
                MessageBox.Show("There is a problem! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: RC1000", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Get MAC Address
        public static string GetMACAddress()
        {
            try
            {
                string macAddress = string.Empty;
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        nic.OperationalStatus == OperationalStatus.Up && !nic.Name.Contains("Tunnel") && !nic.Name.Contains("VirtualBox") && !nic.Name.Contains("Adapter") && !nic.Description.Contains("Npcap") && !nic.Description.Contains("Loopback"))
                        macAddress += nic.GetPhysicalAddress().ToString();
                }

                return macAddress;
            }
            catch (Exception err)
            {
                MessageBox.Show("There is a problem! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: RC1000", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Get IP Info
        private void GetIPInfo()
        {
            try
            {
                var API_PATH_IP_API = "http://ip-api.com/json/" + GetExternalIp();

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.BaseAddress = new Uri(API_PATH_IP_API);
                    HttpResponseMessage response = client.GetAsync(API_PATH_IP_API).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        var locationDetails = response.Content.ReadAsAsync<IpInfo>().GetAwaiter().GetResult();
                        if (locationDetails != null)
                        {
                            _mac_address = GetMACAddress();
                            _external_ip = GetExternalIp();
                            _city = locationDetails.city;
                            _region = locationDetails.regionName;
                            _country = locationDetails.country;

                            //MessageBox.Show(_mac_address + "\n" + _external_ip + "\n" + _city + "\n" + _region + "\n" + _country);
                            string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("There is a problem! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: RC1000", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
