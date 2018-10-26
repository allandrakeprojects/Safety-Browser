using CefSharp;
using CefSharp.WinForms;
using ChoETL;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using Microsoft.Win32;
using System.Security;
using System.Security.Permissions;

namespace Safety_Browser
{
    public partial class Form_YB : Form
    {
        private string[] web_service = { "http://www.ssicortex.com/GetTxt2Search", "http://www.ssitectonic.com/GetTxt2Search", "http://www.ssihedonic.com/GetTxt2Search" };
        private string[] domain_service = { "http://www.ssicortex.com/GetDomains", "http://www.ssitectonic.com/GetDomains", "http://www.ssihedonic.com/GetDomains" };
        private string[] send_service = { "http://www.ssicortex.com/SendDetails", "http://www.ssitectonic.com/SendDetails", "http://www.ssihedonic.com/SendDetails" };
        private string[] diagnostics_service = { "http://www.ssicortex.com/SendDiagnostic", "http://www.ssitectonic.com/SendDiagnostic", "http://www.ssihedonic.com/SendDiagnostic" };
        private string[] notifications_service = { "http://www.ssicortex.com/GetNotifications", "http://www.ssitectonic.com/GetNotifications", "http://www.ssihedonic.com/GetNotifications" };
        private string[] notifications_delete_service = { "http://www.ssicortex.com/GetMessageX", "http://www.ssitectonic.com/GetMessageX", "http://www.ssihedonic.com/GetMessageX" };
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
        private bool isDNSInserted = false;
        private string _mac_address;
        private string _external_ip;
        private string _isp;
        private string _city;
        private string _country;
        private string _province;
        private string BRAND_CODE = "YB";
        private string API_KEY = "6b8c7e5617414bf2d4ace37600b6ab71";
        private string BRAND_ID = "1";
        private ChromiumWebBrowser chromeBrowser;
        private double defaultValue = 0;
        private GlobalKeyboardHook gHook;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private const int
             HTLEFT = 10,
             HTRIGHT = 11,
             HTTOP = 12,
             HTTOPLEFT = 13,
             HTTOPRIGHT = 14,
             HTBOTTOM = 15,
             HTBOTTOMLEFT = 16,
             HTBOTTOMRIGHT = 17;
        const int _ = 1; // you can rename this variable if you like
        new Rectangle Top { get { return new Rectangle(0, 0, this.ClientSize.Width, _); } }
        new Rectangle Left { get { return new Rectangle(0, 0, _, this.ClientSize.Height); } }
        new Rectangle Bottom { get { return new Rectangle(0, this.ClientSize.Height - _, this.ClientSize.Width, _); } }
        new Rectangle Right { get { return new Rectangle(this.ClientSize.Width - _, 0, _, ClientSize.Height); } }
        Rectangle TopLeft { get { return new Rectangle(0, 0, _, _); } }
        Rectangle TopRight { get { return new Rectangle(this.ClientSize.Width - _, 0, _, _); } }
        Rectangle BottomLeft { get { return new Rectangle(0, this.ClientSize.Height - _, _, _); } }
        Rectangle BottomRight { get { return new Rectangle(this.ClientSize.Width - _, this.ClientSize.Height - _, _, _); } }
        public bool IsCloseVisible { get; private set; }
        public bool not_hijacked = false;
        private bool hard_refresh;
        private string handler_title;
        private int fully_loaded;
        private int elseloaded_i;
        private bool help_click = true;
        private int radius = 30;
        private bool notification_click = true;
        private static string result_ping = "";
        private static string result_traceroute = "";
        private string dumpPath;
        private int back_button_i;
        private bool elseload_return;
        private bool isNotHijackedLoaded = false;
        private string notifications_get;
        private string _message_id;
        private string _message_edited_id;
        private string _message_title;
        private string _message_type;
        private string _message_content;
        private string _message_date;
        private string _message_status;
        private string _message_unread;
        private int notificationscount = 0;
        private string _message_id_inner;
        private string _message_date_inner;
        private string _message_title_inner;
        private string _message_content_inner;
        private string _message_status_inner;
        private string _message_type_inner;
        private string _message_edited_id_inner;
        private string _message_unread_inner;
        private string get_back_button_i = String.Empty;
        private Uri currentUri;
        private bool isNewEntry = false;
        private bool isFirstOpened = true;
        private bool isNoNotification;
        static List<string> inaccessble_lists = new List<string>();
        public static string SetAmount = "";

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        public Form_YB()
        {
            InitializeComponent();

            Opacity = 0;

            timer.Interval = 20;
            timer.Tick += new EventHandler(FadeIn);
            timer.Start();
        }

        private void label_changedns_Click(object sender, EventArgs e)
        {            
            DNSServer();
        }

        public void DisplayDnsAddresses()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            int count = 0;
            foreach (NetworkInterface adapter in adapters)
            {
                if (isDNSInserted)
                {
                    break;
                }

                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
                if (dnsServers.Count > 0)
                {
                    foreach (IPAddress dns in dnsServers)
                    {
                        count++;

                        if (count == 1)
                        {
                            if (dns.ToString() == "114.114.114.114" || dns.ToString() == "114.114.115.115")
                            {
                                isDNSInserted = true;

                                MessageBox.Show("DNS Already Set.");
                                break;
                            }
                        }
                        else if (count == 2)
                        {
                            if (dns.ToString() == "114.114.114.114" || dns.ToString() == "114.114.115.115")
                            {
                                isDNSInserted = true;
                                MessageBox.Show("DNS Already Set.");
                                break;
                            }
                        }
                        else
                        {
                            isDNSInserted = false;
                            break;
                        }
                    }
                }
            }
        }
        
        private void DNSServer()
        {
            try
            {
                string networkIntefaceType = string.Empty;
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nic.OperationalStatus == OperationalStatus.Up && !nic.Name.Contains("Tunnel") && !nic.Name.Contains("VirtualBox") && !nic.Name.Contains("Adapter") && !nic.Description.Contains("Npcap") && !nic.Description.Contains("Loopback"))
                    {
                        networkIntefaceType = nic.Name;
                        
                        break;
                    }
                }

                if (!isDNSInserted)
                {
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.UseShellExecute = true;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    if (label_changedns.Text == "Change to China DNS")
                    {
                        string command_1 = "&netsh dnsclient add dnsservers " + networkIntefaceType + " 114.114.114.114 index=1";
                        string command_2 = "&netsh dnsclient add dnsservers " + networkIntefaceType + " 114.114.115.115 index=1";
                        string command_3 = "&netsh dnsclient add dnsservers " + networkIntefaceType + " 8.8.8.8 index=1";
                        string command_4 = "netsh dnsclient add dnsservers " + networkIntefaceType + " 8.8.4.4 index=1";
                        startInfo.Arguments = "/user:Administrator \"cmd /K " + command_4 + command_3 + command_2 + command_1 + "\"";
                    }
                    else
                    {
                        string command_1 = "&netsh dnsclient delete dnsservers " + networkIntefaceType + " 114.114.114.114";
                        string command_2 = "&netsh dnsclient delete dnsservers " + networkIntefaceType + " 114.114.115.115";
                        string command_3 = "&netsh dnsclient add dnsservers " + networkIntefaceType + " 8.8.8.8 index=1";
                        string command_4 = "netsh dnsclient add dnsservers " + networkIntefaceType + " 8.8.4.4 index=1";
                        startInfo.Arguments = "/user:Administrator \"cmd /K " + command_4 + command_3 + command_2 + command_1 + "\"";
                    }
                    if (Environment.OSVersion.Version.Major >= 6)
                    {
                        startInfo.Verb = "runas";
                    }
                    process.StartInfo = startInfo;
                    process.Start();
                    
                    panel_help.Visible = false;
                    help_click = true;

                    if (pictureBox_loader.Visible == true)
                    {
                        panel_cefsharp.Visible = false;
                    }
                    else
                    {
                        panel_cefsharp.Visible = true;
                    }

                    if (!notification_click)
                    {
                        panel_notification.Visible = true;
                        label_separator.Visible = true;
                        button_notification.Visible = true;
                    }

                    pictureBox_help.BackColor = Color.FromArgb(235, 99, 6);
                    pictureBox_helphover.BackColor = Color.FromArgb(235, 99, 6);

                    if (label_changedns.Text == "Change to China DNS")
                    {
                        label_changedns.Text = "Remove China DNS";
                        MessageBox.Show("China DNS Set。");
                    }
                    else
                    {
                        label_changedns.Text = "Change to China DNS";
                        MessageBox.Show("China DNS Removed。");
                    }
                }

                isDNSInserted = false;
            }
            catch (Exception err)
            {
                // Leave blank
            }
        }

        public void DisplayDnsAddressesLoad()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            int count = 0;
            foreach (NetworkInterface adapter in adapters)
            {
                if (isDNSInserted)
                {
                    break;
                }

                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
                if (dnsServers.Count > 0)
                {
                    foreach (IPAddress dns in dnsServers)
                    {
                        count++;

                        if (count == 1)
                        {
                            if (dns.ToString() == "114.114.114.114" || dns.ToString() == "114.114.115.115")
                            {
                                isDNSInserted = true;
                                label_changedns.Text = "Remove China DNS";
                                break;
                            }
                        }
                        else if (count == 2)
                        {
                            if (dns.ToString() == "114.114.114.114" || dns.ToString() == "114.114.115.115")
                            {
                                isDNSInserted = true;
                                label_changedns.Text = "Remove China DNS";
                                break;
                            }
                        }
                        else
                        {
                            isDNSInserted = false;
                            break;
                        }
                    }
                }
            }

            isDNSInserted = false;
        }

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        // Form Load
        private void Form_Main_Load(object sender, EventArgs e)
        {
            string path_result = Path.GetTempPath() + "\\sb_result.txt";

            if (File.Exists(path_result))
            {
                File.Delete(path_result);
            }
            
            if (File.Exists(Path.GetTempPath() + "\\deviceinfo.txt"))
            {
                File.Delete(Path.GetTempPath() + "\\deviceinfo.txt");
            }

            if (File.Exists(Path.GetTempPath() + "\\traceroute.txt"))
            {
                File.Delete(Path.GetTempPath() + "\\traceroute.txt");
            }

            if (File.Exists(Path.GetTempPath() + "\\ping.txt"))
            {
                File.Delete(Path.GetTempPath() + "\\ping.txt");
            }

            if (File.Exists(Path.GetTempPath() + "\\Diagnostics.zip"))
            {
                File.Delete(Path.GetTempPath() + "\\Diagnostics.zip");
            }

            PictureBoxCenter();
            GetMACAddress();
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
            InitializeChromium();
            NetworkAvailabilityAsync();
            gHook = new GlobalKeyboardHook();
            gHook.KeyDown += new KeyEventHandler(gHook_KeyDown);
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                gHook.HookedKeys.Add(key);
            }
            gHook.hook();

            DisplayDnsAddressesLoad();
        }

        private void FadeIn(object sender, EventArgs e)
        {
            if (Opacity >= 1)
            {
                timer_landing.Start();
            }
            else
            {
                Opacity += 0.05;
            }
        }

        private void GetInaccessibleLists()
        {
            try
            {
                WebRequest.DefaultWebProxy = new WebProxy();
                using (var client = new WebClient())
                {
                    string auth = "r@inCh3ckd234b70";
                    string type = "category";
                    string request = "http://raincheck.ssitex.com/api/api.php";
                    string mac_id = get_macAddress;

                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "auth", auth },
                        { "type", type }
                    };

                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(request, postData));
                    inaccessble_lists.Add(pagesource);
                }
            }
            catch (Exception ex)
            {
                //var st = new StackTrace(ex, true);
                //var frame = st.GetFrame(0);
                //var line = frame.GetFileLineNumber();
                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: 1004", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Close();
            }
        }

        public void gHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (not_hijacked)
            {
                if (ContainsFocus)
                {
                    if (e.KeyData.ToString().ToUpper().IndexOf("Control".ToUpper()) >= 0 && e.KeyData.ToString().ToUpper().IndexOf("Shift".ToUpper()) >= 0 && e.KeyCode == Keys.R)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                chromeBrowser.Reload(false);
                            }));

                        }).Start();
                    }
                    else if (e.KeyData.ToString().ToUpper().IndexOf("Control".ToUpper()) >= 0 && e.KeyCode == Keys.R)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                chromeBrowser.Reload(true);
                            }));

                        }).Start();
                    }
                    else if (e.KeyData.ToString().ToUpper().IndexOf("Control".ToUpper()) >= 0 && e.KeyCode == Keys.H)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                if (!help_click)
                                {
                                    panel_help.Visible = false;
                                    help_click = true;

                                    if (pictureBox_loader.Visible == true)
                                    {
                                        panel_cefsharp.Visible = false;
                                    }
                                    else
                                    {
                                        panel_cefsharp.Visible = true;
                                    }

                                    if (!notification_click)
                                    {
                                        panel_notification.Visible = true;
                                        label_separator.Visible = true;
                                        button_notification.Visible = true;
                                    }

                                    pictureBox_help.BackColor = Color.FromArgb(235, 99, 6);
                                    pictureBox_helphover.BackColor = Color.FromArgb(235, 99, 6);
                                }

                                chromeBrowser.Load(domain_get);
                            }));

                        }).Start();
                    }
                    else if (e.KeyCode == Keys.Left)
                    {
                        if (pictureBox_back.Enabled == true)
                        {
                            new Thread(() =>
                            {
                                Invoke(new Action(delegate
                                {
                                    chromeBrowser.Back();
                                }));

                            }).Start();
                        }
                    }
                    else if (e.KeyCode == Keys.Right)
                    {
                        if (pictureBox_forward.Enabled == true)
                        {
                            new Thread(() =>
                            {
                                Invoke(new Action(delegate
                                {
                                    chromeBrowser.Forward();
                                }));

                            }).Start();
                        }
                    }
                    else if (e.KeyData.ToString().ToUpper().IndexOf("Control".ToUpper()) >= 0 && e.KeyCode == Keys.D0)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                defaultValue = 0;
                                chromeBrowser.SetZoomLevel(0);
                            }));

                        }).Start();
                    }
                    else if (e.KeyData.ToString().ToUpper().IndexOf("Control".ToUpper()) >= 0 && e.KeyCode == Keys.Oemplus)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                chromeBrowser.SetZoomLevel(++defaultValue);
                            }));

                        }).Start();
                    }
                    else if (e.KeyData.ToString().ToUpper().IndexOf("Control".ToUpper()) >= 0 && e.KeyCode == Keys.OemMinus)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                chromeBrowser.SetZoomLevel(--defaultValue);
                            }));

                        }).Start();
                    }
                }
            }
        }

        // Network Handler
        private async void NetworkAvailabilityAsync()
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
                panel_cefsharp.Controls.Add(chromeBrowser);

                GetInaccessibleLists();

                //#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                await GetTextToTextAsync(web_service[current_web_service]);
                //#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                GetIPInfo();

                //#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                await GetNotificationAsync(notifications_service[current_web_service]);
                //#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                label_chatus2.Enabled = true;
                label_emailus1.Enabled = true;
                label_chatus2.ForeColor = Color.FromArgb(235, 99, 6);
                label_emailus1.ForeColor = Color.FromArgb(235, 99, 6);
            }
            else
            {
                panel_cefsharp.Controls.Clear();

                timer_handler.Stop();

                panel_cefsharp.Visible = false;
                pictureBox_loader.Visible = false;
                label_loader.Visible = false;

                panel_connection.Visible = true;
                panel_connection.Enabled = true;

                pictureBox_nofication.Enabled = true;
                pictureBox_noficationhover.Enabled = true;
                pictureBox_nofication.Image = Properties.Resources.notification;
                timer_detectifhijacked.Stop();

                NotificationsAsync();
            }

            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
        }

        // Network Handler Changed
        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            Invoke(new Action(async () =>
            {
                networkIsAvailable = e.IsAvailable;

                if (networkIsAvailable)
                {
                    panel_cefsharp.Controls.Add(chromeBrowser);

                    if (!domain_one_time)
                    {
                        chromeBrowser.Reload();
                    }

                    GetMACAddress();

                    bool isEmpty = !inaccessble_lists.Any();
                    if (isEmpty)
                    {
                        GetInaccessibleLists();
                    }

                    if (!isIPInserted)
                    {
                        GetIPInfo();
                    }

                    if (dataGridView_domain.RowCount == 0 && last_index_hijacked_get)
                    {
                        if (current_web_service < web_service.Length)
                        {
                            ByPassCallingAsync();

                            panel_connection.Visible = false;
                            panel_connection.Enabled = false;
                        }
                        else
                        {
                            panel_connection.Visible = false;
                            panel_connection.Enabled = false;

                            panel_cefsharp.Visible = false;
                            pictureBox_loader.Visible = false;
                            label_loader.Visible = false;
                        }
                    }
                    else if (dataGridView_domain.RowCount == 0 && !connection_handler)
                    {
                        //#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        await GetTextToTextAsync(web_service[current_web_service]);
                        //#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                        panel_connection.Visible = false;
                        panel_connection.Enabled = false;

                        panel_cefsharp.Visible = false;
                        pictureBox_loader.Visible = false;
                        label_loader.Visible = false;
                    }
                    else if (!last_index_hijacked_get)
                    {
                        panel_connection.Visible = false;
                        panel_connection.Enabled = false;
                        pictureBox_loader.Visible = false;
                        label_loader.Visible = false;

                        panel_cefsharp.Visible = true;
                    }
                    else
                    {
                        panel_cefsharp.Visible = false;
                        panel_connection.Visible = false;
                        panel_connection.Enabled = false;

                        if (panel_cefsharp.Visible == true)
                        {
                            pictureBox_loader.Visible = false;
                            label_loader.Visible = false;
                        }
                        else
                        {
                            if (!connection_handler)
                            {
                                pictureBox_loader.Visible = true;
                                label_loader.Visible = true;
                            }
                            else
                            {
                                panel_cefsharp.Visible = true;
                            }
                        }

                        if (!connection_handler)
                        {
                            dataGridView_domain.ClearSelection();
                            dataGridView_domain.Rows[current_domain_index].Selected = true;
                        }
                    }

                    //#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    await GetNotificationAsync(notifications_service[current_web_service]);
                    //#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                    if (!domain_one_time)
                    {
                        label_chatus2.Enabled = true;
                        label_emailus1.Enabled = true;
                        label_chatus2.ForeColor = Color.FromArgb(235, 99, 6);
                        label_emailus1.ForeColor = Color.FromArgb(235, 99, 6);
                        label_clearcache.Enabled = true;
                        label_getdiagnostics.Enabled = true;
                        label_changedns.Enabled = true;

                        timer_detectifhijacked.Start();
                    }
                }
                else
                {
                    panel_cefsharp.Controls.Clear();

                    chromeBrowser.Stop();
                    timer_handler.Stop();
                    timer_notifications.Stop();

                    panel_cefsharp.Visible = false;
                    pictureBox_loader.Visible = false;
                    label_loader.Visible = false;

                    panel_connection.Visible = true;
                    panel_connection.Enabled = true;

                    label_chatus2.Enabled = false;
                    label_emailus1.Enabled = false;
                    label_chatus2.ForeColor = Color.Black;
                    label_emailus1.ForeColor = Color.Black;
                    label_clearcache.Enabled = false;
                    label_getdiagnostics.Enabled = false;
                    label_changedns.Enabled = false;

                    timer_detectifhijacked.Stop();
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

            panel_help.Left = (ClientSize.Width - panel_help.Width) / 2;
            panel_help.Top = (ClientSize.Height - panel_help.Height) / 2;
        }

        // Get Text to Search
        private async Task GetTextToTextAsync(string web_service_test)
        {
            if (networkIsAvailable)
            {
                label_current_web_service.Text = web_service_test;

                try
                {
                    WebRequest.DefaultWebProxy = new WebProxy();
                    var client = new HttpClient();
                    var requestContent = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string, string>("api_key", API_KEY),
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
                            ByPassCallingAsync();
                        }
                        else
                        {
                            pictureBox_loader.Visible = false;
                            label_loader.Visible = false;
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
                            await GetDomainsAsync(domain_service[current_domain]);
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
                                ByPassCallingAsync();
                            }
                            else
                            {
                                pictureBox_loader.Visible = false;
                                label_loader.Visible = false;
                                pictureBox_loader.Enabled = false;
                                connection_handler = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("请查询你的网络连接！謝謝。", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    close = false;
                    Close();
                }
            }
            else
            {
                timer_handler.Stop();

                panel_cefsharp.Visible = false;
                pictureBox_loader.Visible = false;
                label_loader.Visible = false;

                panel_connection.Visible = true;
                panel_connection.Enabled = true;
            }
        }

        // ByPass Calling
        private async void ByPassCallingAsync()
        {
            //#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            await GetTextToTextAsync(web_service[current_web_service]);
            //#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        // Get Domain
        private async Task GetDomainsAsync(string test_domain)
        {
            label_current_domain_service.Text = test_domain;

            try
            {
                WebRequest.DefaultWebProxy = new WebProxy();
                var client = new HttpClient();
                var requestContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("api_key", API_KEY),
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请查询你的网络连接！謝謝。", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Error);
                close = false;
                Close();
            }
        }

        // Get Notifications
        private async Task GetNotificationAsync(string webservice_notifications)
        {
            try
            {
                var client = new HttpClient();
                var requestContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("macid", get_macAddress),
                    new KeyValuePair<string, string>("api_key", API_KEY),
                    new KeyValuePair<string, string>("brand_code", BRAND_CODE),
                });

                HttpResponseMessage response = await client.PostAsync(
                    webservice_notifications,
                    requestContent);
                HttpContent responseContent = response.Content;

                using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                {
                    string json = await reader.ReadToEndAsync();
                    string notifications_file = Path.Combine(Path.GetTempPath(), "sb_notifications.txt");
                    string temp_file = Path.Combine(Path.GetTempPath(), "sb_notifications_temp.txt");

                    string pattern = @"\[(.*?)\]";
                    var matches = Regex.Matches(json, pattern);

                    foreach (Match m in matches)
                    {
                        notifications_get = Regex.Unescape(m.Groups[1].ToString());
                    }

                    if (!String.IsNullOrEmpty(notifications_get))
                    {
                        //isNewEntry = true;
                        using (var csv = new ChoCSVWriter(temp_file).WithFirstLineHeader())
                        {
                            using (var p = ChoJSONReader.LoadText(json).WithJSONPath("$..data"))
                            {
                                csv.Write(p.Select(i => new {
                                    Header_Test_Header = i.id + "*|*" + i.message_date + "*|*" + "★ " + i.message_title + "*|*" + i.message_content + "*|*" + i.status + "*|*" + i.message_type + "*|*" + i.edited_id + "*|*U" + i.brand_id
                                }));
                            }
                        }

                        if (File.Exists(notifications_file))
                        {
                            string line_sr;
                            StreamReader streamReader = new StreamReader(temp_file);
                            while ((line_sr = streamReader.ReadLine()) != null)
                            {
                                if (line_sr != "")
                                {
                                    if (!line_sr.Contains("Header_Test_Header"))
                                    {
                                        char last = line_sr[line_sr.Length - 1];
                                        string replace_line = line_sr.Remove(line_sr.Length - 1);

                                        if (last.ToString() == "\"")
                                        {
                                            last = line_sr[line_sr.Length - 2];
                                            replace_line = line_sr.Remove(line_sr.Length - 2);
                                        }

                                        if (BRAND_ID == last.ToString())
                                        {
                                            string[] strArray = replace_line.Split("*|*");

                                            int count_update = 0;
                                            foreach (object obj in strArray)
                                            {
                                                count_update++;

                                                if (count_update == 6)
                                                {
                                                    if (obj.ToString() == "0" || obj.ToString() == "1")
                                                    {
                                                        isNewEntry = true;
                                                        StreamWriter sw = new StreamWriter(notifications_file, true, Encoding.UTF8);
                                                        sw.WriteLine(replace_line);
                                                        sw.Close();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            streamReader.Close();

                            label_notificationstatus.Visible = false;
                            flowLayoutPanel_notifications.Visible = true;
                            flowLayoutPanel_notifications.BringToFront();

                            NotificationsAsync();
                        }
                        else
                        {
                            string line_sr;
                            StreamReader streamReader = new StreamReader(temp_file);
                            while ((line_sr = streamReader.ReadLine()) != null)
                            {
                                if (line_sr != "")
                                {
                                    if (!line_sr.Contains("Header_Test_Header"))
                                    {
                                        char last = line_sr[line_sr.Length - 1];
                                        string replace_line = line_sr.Remove(line_sr.Length - 1);

                                        if (last.ToString() == "\"")
                                        {
                                            last = line_sr[line_sr.Length - 2];
                                            replace_line = line_sr.Remove(line_sr.Length - 2);
                                        }

                                        if (BRAND_ID == last.ToString())
                                        {
                                            string[] strArray = replace_line.Split("*|*");

                                            int count_update = 0;
                                            foreach (object obj in strArray)
                                            {
                                                count_update++;

                                                if (count_update == 6)
                                                {
                                                    if (obj.ToString() == "0" || obj.ToString() == "1")
                                                    {
                                                        isNewEntry = true;
                                                        StreamWriter sw = new StreamWriter(notifications_file, true, Encoding.UTF8);
                                                        sw.WriteLine(replace_line);
                                                        sw.Close();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            streamReader.Close();

                            label_notificationstatus.Visible = false;
                            flowLayoutPanel_notifications.Visible = true;
                            flowLayoutPanel_notifications.BringToFront();

                            if (File.Exists(notifications_file))
                            {
                                NotificationsAsync();
                            }
                        }
                    }
                    else
                    {
                        if (isFirstOpened)
                        {
                            if (File.Exists(notifications_file))
                            {
                                NotificationsAsync();
                            }
                            else
                            {
                                label_notificationstatus.Location = new Point(7, 32);
                                label_notificationstatus.Visible = true;
                                label_notificationstatus.BringToFront();
                                flowLayoutPanel_notifications.Visible = false;
                            }
                        }
                        else
                        {
                            if (networkIsAvailable)
                            {
                                // delete
                                await GetNotificationDeleteAsync(notifications_delete_service[current_web_service]);
                                // end delete
                            }

                            if (isNewEntry)
                            {
                                isNewEntry = false;

                                if (File.Exists(notifications_file))
                                {
                                    label_notificationstatus.Visible = false;
                                    flowLayoutPanel_notifications.Visible = true;
                                    flowLayoutPanel_notifications.BringToFront();

                                    NotificationsAsync();
                                }
                                else
                                {
                                    label_notificationstatus.Location = new Point(7, 32);
                                    label_notificationstatus.Visible = true;
                                    label_notificationstatus.BringToFront();
                                    flowLayoutPanel_notifications.Visible = false;
                                }
                            }
                            else
                            {
                                isNewEntry = false;
                            }
                        }
                    }
                }

                isFirstOpened = false;
                timer_notifications.Start();
                timer_notifications_detect.Start();
            }
            catch (Exception err)
            {
                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1002", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //close = false;
                //Close();
            }
        }

        private async Task NotificationsAsync()
        {
            string notifications_file = Path.Combine(Path.GetTempPath(), "sb_notifications.txt");
            if (File.Exists(notifications_file))
            {
                notificationscount = 0;

                //update
                List<string> line_to_delete = new List<string>();
                int line_count = 0;
                string line_update;
                StreamReader sr_update = new StreamReader(notifications_file);
                while ((line_update = sr_update.ReadLine()) != null)
                {
                    if (line_update != "")
                    {
                        string[] strArray = line_update.Split("*|*");

                        int count_update = 0;
                        foreach (object obj in strArray)
                        {
                            count_update++;

                            // Message Type
                            if (count_update == 7)
                            {
                                if (obj.ToString() != "0")
                                {
                                    string delete_id = obj.ToString();
                                    string line_delete;
                                    StreamReader sr_delete = new StreamReader(notifications_file);
                                    while ((line_delete = sr_delete.ReadLine()) != null)
                                    {
                                        if (line_delete != "")
                                        {
                                            string[] strArray_inner = line_delete.Split("*|*");

                                            int count_update_inner = 0;
                                            foreach (object obje in strArray_inner)
                                            {
                                                count_update_inner++;

                                                if (count_update_inner == 1)
                                                {
                                                    if (delete_id == obje.ToString().Replace("\"", ""))
                                                    {
                                                        line_to_delete.Add(line_delete);
                                                        line_count++;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    sr_delete.Close();
                                }
                            }
                        }
                    }
                }

                sr_update.Close();

                for (int i = 0; i < line_count; i++)
                {
                    string text = File.ReadAllText(notifications_file);
                    text = text.Replace(line_to_delete[i], "");
                    File.WriteAllText(notifications_file, text);
                }

                if (isFirstOpened)
                {
                    if (networkIsAvailable)
                    {
                        // delete
                        await GetNotificationDeleteAsync(notifications_delete_service[current_web_service]);
                        // end delete
                    }
                }

                flowLayoutPanel_notifications.Controls.Clear();

                string line_count_inner;
                int line_count_panel = 0;
                StreamReader sr_count = new StreamReader(notifications_file);
                while ((line_count_inner = sr_count.ReadLine()) != null)
                {
                    if (line_count_inner != "")
                    {
                        line_count_panel++;
                    }
                }

                sr_count.Close();
                int detect_no_notification = 0;
                string line;
                StreamReader sr = new StreamReader(notifications_file);
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        string[] strArray = line.Split("*|*");

                        int count = 0;
                        foreach (object obj in strArray)
                        {
                            count++;
                            detect_no_notification++;

                            if (count == 1)
                            {
                                _message_id = obj.ToString().Replace("\"", "");
                            }
                            else if (count == 2)
                            {
                                _message_date = obj.ToString();
                            }
                            else if (count == 3)
                            {
                                _message_title = ReplaceString(obj.ToString());
                            }
                            else if (count == 4)
                            {
                                _message_content = ReplaceString(obj.ToString());
                            }
                            else if (count == 5)
                            {
                                _message_status = obj.ToString();
                            }
                            else if (count == 6)
                            {
                                _message_type = obj.ToString();
                            }
                            else if (count == 7)
                            {
                                _message_edited_id = obj.ToString();
                            }
                            else if (count == 8)
                            {
                                _message_unread = obj.ToString();
                            }
                        }

                        if (_message_type == "0")
                        {
                            Panel p = new Panel();
                            p.Name = "panel_notification_" + _message_id;
                            p.BackColor = Color.White;
                            p.Size = new Size(270, 83);

                            Label label_title = new Label();
                            Label label_content = new Label();

                            if (_message_title.Contains("★"))
                            {
                                label_title.Name = "label_title_notification_" + _message_id;
                                label_title.Text = Ellipsis(_message_title, 20);

                                if (_message_unread.Contains("U"))
                                {
                                    notificationscount++;
                                    label_notificationscount.Text = notificationscount.ToString();
                                }

                                label_title.Location = new Point(3, 0);
                                label_title.AutoSize = true;
                                label_title.ForeColor = Color.FromArgb(0, 0, 0);
                                label_title.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);

                                label_content.Name = "label_content_notification_" + _message_id;
                                label_content.Text = Ellipsis(_message_content, 130);
                                label_content.Location = new Point(4, 19);
                                label_content.AutoSize = true;
                                label_content.MaximumSize = new Size(248, 40);
                                label_content.ForeColor = Color.FromArgb(0, 0, 0);
                                label_content.Font = new Font("Microsoft Sans Serif", 8);
                            }
                            else
                            {
                                label_title.Name = "label_title_notification_" + _message_id;
                                label_title.Text = Ellipsis(_message_title, 20);

                                if (_message_unread.Contains("U"))
                                {
                                    notificationscount++;
                                    label_notificationscount.Text = notificationscount.ToString();
                                }

                                label_title.Location = new Point(3, 0);
                                label_title.AutoSize = true;
                                label_title.ForeColor = Color.FromArgb(72, 72, 72);
                                label_title.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);

                                label_content.Name = "label_content_notification_" + _message_id;
                                label_content.Text = Ellipsis(_message_content, 130);
                                label_content.Location = new Point(4, 19);
                                label_content.AutoSize = true;
                                label_content.MaximumSize = new Size(248, 40);
                                label_content.ForeColor = Color.FromArgb(72, 72, 72);
                                label_content.Font = new Font("Microsoft Sans Serif", 8);
                            }

                            Label label_date = new Label();
                            label_date.Name = "label_date_notification_" + _message_id;

                            const int SECOND = 1;
                            const int MINUTE = 60 * SECOND;
                            const int HOUR = 60 * MINUTE;
                            const int DAY = 24 * HOUR;
                            const int MONTH = 30 * DAY;

                            string date_now_parse = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            DateTime date_now = DateTime.ParseExact(date_now_parse, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                            string get_date_parse = _message_date;
                            DateTime get_date = DateTime.ParseExact(get_date_parse, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                            var ts = new TimeSpan(date_now.Ticks - get_date.Ticks);
                            double delta = Math.Abs(ts.TotalSeconds);

                            if (delta < 1 * MINUTE)
                            {
                                label_date.Text = "刚刚";
                            }
                            else if (delta < 2 * MINUTE)
                            {
                                label_date.Text = "一分钟前";
                            }
                            else if (delta < 45 * MINUTE)
                            {
                                if (ts.Minutes == 1)
                                {
                                    label_date.Text = "一分钟前";
                                }
                                else
                                {
                                    label_date.Text = ts.Minutes + "分钟前";
                                }
                            }
                            else if (delta < 90 * MINUTE)
                            {
                                label_date.Text = "一小时前";
                            }
                            else if (delta < 24 * HOUR)
                            {
                                if (ts.Hours == 1)
                                {
                                    label_date.Text = "一小时前";
                                }
                                else
                                {
                                    label_date.Text = ts.Hours + "小时前";
                                }
                            }
                            else if (delta < 48 * HOUR)
                            {
                                label_date.Text = "昨日";
                            }
                            else if (delta < 30 * DAY)
                            {
                                if (ts.Days == 1)
                                {
                                    label_date.Text = ts.Days + "天前";
                                }
                                else
                                {
                                    label_date.Text = ts.Days + "天前";
                                }
                            }
                            else
                            {
                                label_date.Text = "已读信息";
                            }

                            label_date.AutoSize = true;
                            label_date.Location = new Point(4, 61);
                            label_date.ForeColor = Color.FromArgb(168, 168, 168);
                            label_date.Font = new Font("Microsoft Sans Serif", 8);

                            Label label_view = new Label();
                            label_view.Name = "label_view_notification_" + _message_id;
                            label_view.Text = "观看";

                            if (line_count_panel > 7)
                            {
                                label_view.Location = new Point(220, 61);
                                p.Size = new Size(250, 83);
                                label_content.MaximumSize = new Size(230, 40);
                                flowLayoutPanel_notifications.AutoScroll = true;
                            }
                            else
                            {
                                label_view.Location = new Point(232, 61);
                                flowLayoutPanel_notifications.AutoScroll = false;
                            }

                            label_view.AutoSize = true;
                            label_view.ForeColor = Color.FromArgb(72, 72, 72);
                            label_view.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Underline);
                            label_view.Cursor = Cursors.Hand;
                            label_view.Click += new EventHandler(click_event);

                            Label label_separator_notification = new Label();
                            label_separator_notification.Name = "label_separator_notification_" + _message_id;
                            label_separator_notification.AutoSize = false;
                            label_separator_notification.Text = " ";
                            label_separator_notification.Size = new Size(270, 1);
                            label_separator_notification.Location = new Point(0, 81);
                            label_separator_notification.BackColor = Color.FromArgb(238, 238, 238);

                            flowLayoutPanel_notifications.Controls.Add(p);
                            flowLayoutPanel_notifications.Controls.SetChildIndex(p, 0);

                            p.Controls.Add(label_title);
                            p.Controls.Add(label_content);
                            p.Controls.Add(label_date);
                            p.Controls.Add(label_view);
                            p.Controls.Add(label_separator_notification);
                            flowLayoutPanel_notifications.Invalidate();
                        }
                    }
                }

                if (detect_no_notification == 0)
                {
                    label_notificationstatus.Location = new Point(7, 32);
                    label_notificationstatus.Visible = true;
                    label_notificationstatus.BringToFront();
                    flowLayoutPanel_notifications.Visible = false;
                }
                else
                {
                    label_notificationstatus.Visible = false;
                    flowLayoutPanel_notifications.Visible = true;
                    flowLayoutPanel_notifications.BringToFront();
                }

                if (notificationscount == 0)
                {
                    label_notificationscount.Visible = false;
                }
                else
                {
                    if (isNotHijackedLoaded)
                    {
                        label_notificationscount.Visible = true;
                    }
                    else if (!networkIsAvailable)
                    {
                        label_notificationscount.Visible = true;
                    }
                }

                sr.Close();


                Update();
            }
            else
            {
                label_notificationstatus.Location = new Point(7, 32);
                label_notificationstatus.Visible = true;
                label_notificationstatus.BringToFront();
                flowLayoutPanel_notifications.Visible = false;
            }
        }

        private new void Update()
        {
            string notifications_file = Path.Combine(Path.GetTempPath(), "sb_notifications.txt");

            string line_count_inner;
            int line_count_panel = 0;
            StreamReader sr_count = new StreamReader(notifications_file);
            while ((line_count_inner = sr_count.ReadLine()) != null)
            {
                if (line_count_inner != "")
                {
                    line_count_panel++;
                }
            }

            sr_count.Close();

            string line;
            StreamReader sr = new StreamReader(notifications_file);
            while ((line = sr.ReadLine()) != null)
            {
                if (line != "")
                {
                    string[] strArray = line.Split("*|*");

                    int count = 0;
                    foreach (object obj in strArray)
                    {
                        count++;

                        if (count == 1)
                        {
                            _message_id = obj.ToString().Replace("\"", "");
                        }
                        else if (count == 2)
                        {
                            _message_date = obj.ToString();
                        }
                        else if (count == 3)
                        {
                            _message_title = ReplaceString(obj.ToString());
                        }
                        else if (count == 4)
                        {
                            _message_content = ReplaceString(obj.ToString());
                        }
                        else if (count == 5)
                        {
                            _message_status = obj.ToString();
                        }
                        else if (count == 6)
                        {
                            _message_type = obj.ToString();
                        }
                        else if (count == 7)
                        {
                            _message_edited_id = obj.ToString();
                        }
                        else if (count == 8)
                        {
                            _message_unread = obj.ToString();
                        }
                    }

                    if (_message_type == "1")
                    {
                        Panel p = new Panel();
                        p.Name = "panel_notification_" + _message_id;
                        p.BackColor = Color.White;
                        p.Size = new Size(270, 83);

                        Label label_title = new Label();
                        label_title.Name = "label_title_notification_" + _message_id;
                        label_title.Text = Ellipsis(_message_title, 20);

                        if (_message_unread.Contains("U"))
                        {
                            notificationscount++;
                            label_notificationscount.Text = notificationscount.ToString();
                        }

                        label_title.Location = new Point(3, 0);
                        label_title.AutoSize = true;
                        label_title.ForeColor = Color.FromArgb(235, 99, 6);
                        label_title.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);

                        Label label_content = new Label();
                        label_content.Name = "label_content_notification_" + _message_id;
                        label_content.Text = Ellipsis(_message_content, 130);
                        label_content.Location = new Point(4, 19);
                        label_content.AutoSize = true;
                        label_content.MaximumSize = new Size(248, 40);
                        label_content.ForeColor = Color.FromArgb(235, 99, 6);
                        label_content.Font = new Font("Microsoft Sans Serif", 8);

                        Label label_date = new Label();
                        label_date.Name = "label_date_notification_" + _message_id;

                        const int SECOND = 1;
                        const int MINUTE = 60 * SECOND;
                        const int HOUR = 60 * MINUTE;
                        const int DAY = 24 * HOUR;
                        const int MONTH = 30 * DAY;

                        string date_now_parse = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        DateTime date_now = DateTime.ParseExact(date_now_parse, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                        string get_date_parse = _message_date;
                        DateTime get_date = DateTime.ParseExact(get_date_parse, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                        var ts = new TimeSpan(date_now.Ticks - get_date.Ticks);
                        double delta = Math.Abs(ts.TotalSeconds);

                        if (delta < 1 * MINUTE)
                        {
                            label_date.Text = "刚刚";
                        }
                        else if (delta < 2 * MINUTE)
                        {
                            label_date.Text = "一分钟前";
                        }
                        else if (delta < 45 * MINUTE)
                        {
                            if (ts.Minutes == 1)
                            {
                                label_date.Text = "一分钟前";
                            }
                            else
                            {
                                label_date.Text = ts.Minutes + "分钟前";
                            }
                        }
                        else if (delta < 90 * MINUTE)
                        {
                            label_date.Text = "一小时前";
                        }
                        else if (delta < 24 * HOUR)
                        {
                            if (ts.Hours == 1)
                            {
                                label_date.Text = "一小时前";
                            }
                            else
                            {
                                label_date.Text = ts.Hours + "小时前";
                            }
                        }
                        else if (delta < 48 * HOUR)
                        {
                            label_date.Text = "昨日";
                        }
                        else if (delta < 30 * DAY)
                        {
                            if (ts.Days == 1)
                            {
                                label_date.Text = ts.Days + "天前";
                            }
                            else
                            {
                                label_date.Text = ts.Days + "天前";
                            }
                        }
                        else
                        {
                            label_date.Text = "已读信息";
                        }

                        label_date.AutoSize = true;
                        label_date.Location = new Point(4, 61);
                        label_date.ForeColor = Color.FromArgb(168, 168, 168);
                        label_date.Font = new Font("Microsoft Sans Serif", 8);

                        Label label_view = new Label();
                        label_view.Name = "label_view_notification_" + _message_id;
                        label_view.Text = "观看";

                        if (line_count_panel > 7)
                        {
                            label_view.Location = new Point(220, 61);
                            p.Size = new Size(250, 83);
                            label_content.MaximumSize = new Size(230, 40);
                            flowLayoutPanel_notifications.AutoScroll = true;
                        }
                        else
                        {
                            label_view.Location = new Point(232, 61);
                            flowLayoutPanel_notifications.AutoScroll = false;
                        }

                        label_view.AutoSize = true;
                        label_view.ForeColor = Color.FromArgb(72, 72, 72);
                        label_view.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Underline);
                        label_view.Cursor = Cursors.Hand;
                        label_view.Click += new EventHandler(click_event_update);

                        Label label_separator_notification = new Label();
                        label_separator_notification.Name = "label_separator_notification_" + _message_id;
                        label_separator_notification.AutoSize = false;
                        label_separator_notification.Text = " ";
                        label_separator_notification.Size = new Size(270, 1);
                        label_separator_notification.Location = new Point(0, 81);
                        label_separator_notification.BackColor = Color.FromArgb(238, 238, 238);

                        flowLayoutPanel_notifications.Controls.Add(p);
                        flowLayoutPanel_notifications.Controls.SetChildIndex(p, 0);

                        p.Controls.Add(label_title);
                        p.Controls.Add(label_content);
                        p.Controls.Add(label_date);
                        p.Controls.Add(label_view);
                        p.Controls.Add(label_separator_notification);
                        flowLayoutPanel_notifications.Invalidate();
                    }
                }
            }

            sr.Close();
        }

        private void click_event_update(object sender, EventArgs e)
        {

            try
            {
                string get_line = string.Empty;
                string parent_name = ((Label)sender).Parent.Name;
                string output = Regex.Match(parent_name, @"\d+").Value;
                string text = string.Empty;

                string notifications_file = Path.Combine(Path.GetTempPath(), "sb_notifications.txt");
                string line;
                StreamReader sr = new StreamReader(notifications_file);
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        string[] strArray = line.Split("*|*");

                        int count = 0;
                        int count_inner = 0;
                        foreach (object obj in strArray)
                        {
                            count++;

                            if (count == 1)
                            {
                                if (output == obj.ToString().Replace("\"", ""))
                                {
                                    foreach (object obje in strArray)
                                    {
                                        count_inner++;

                                        if (count_inner == 1)
                                        {
                                            _message_id_inner = obje.ToString().Replace("\"", "");
                                        }
                                        else if (count_inner == 2)
                                        {
                                            _message_date_inner = obje.ToString();
                                        }
                                        else if (count_inner == 3)
                                        {
                                            _message_title_inner = ReplaceString(obje.ToString());
                                        }
                                        else if (count_inner == 4)
                                        {
                                            _message_content_inner = ReplaceString(obje.ToString());
                                        }
                                        else if (count_inner == 5)
                                        {
                                            _message_status_inner = obje.ToString();
                                        }
                                        else if (count_inner == 6)
                                        {
                                            _message_type_inner = obje.ToString();
                                        }
                                        else if (count_inner == 7)
                                        {
                                            _message_edited_id_inner = obje.ToString();
                                        }
                                        else if (count_inner == 8)
                                        {
                                            const int SECOND = 1;
                                            const int MINUTE = 60 * SECOND;
                                            const int HOUR = 60 * MINUTE;
                                            const int DAY = 24 * HOUR;
                                            const int MONTH = 30 * DAY;

                                            string date_now_parse = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            DateTime date_now = DateTime.ParseExact(date_now_parse, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                                            string get_date_parse = _message_date_inner;
                                            DateTime get_date = DateTime.ParseExact(get_date_parse, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                                            var ts = new TimeSpan(date_now.Ticks - get_date.Ticks);
                                            double delta = Math.Abs(ts.TotalSeconds);

                                            if (delta < 1 * MINUTE)
                                            {
                                                _message_date_inner = "刚刚";
                                            }
                                            else if (delta < 2 * MINUTE)
                                            {
                                                _message_date_inner = "一分钟前";
                                            }
                                            else if (delta < 45 * MINUTE)
                                            {
                                                if (ts.Minutes == 1)
                                                {
                                                    _message_date_inner = "一分钟前";
                                                }
                                                else
                                                {
                                                    _message_date_inner = ts.Minutes + "分钟前";
                                                }
                                            }
                                            else if (delta < 90 * MINUTE)
                                            {
                                                _message_date_inner = "一小时前";
                                            }
                                            else if (delta < 24 * HOUR)
                                            {
                                                if (ts.Hours == 1)
                                                {
                                                    _message_date_inner = "一小时前";
                                                }
                                                else
                                                {
                                                    _message_date_inner = ts.Hours + "小时前";
                                                }
                                            }
                                            else if (delta < 48 * HOUR)
                                            {
                                                _message_date_inner = "昨日";
                                            }
                                            else if (delta < 30 * DAY)
                                            {
                                                if (ts.Days == 1)
                                                {
                                                    _message_date_inner = ts.Days + "天前";
                                                }
                                                else
                                                {
                                                    _message_date_inner = ts.Days + "天前";
                                                }
                                            }
                                            else
                                            {
                                                _message_date_inner = "已读信息";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (MessageBox.Show(_message_content_inner + "\n\n", _message_title_inner.Replace("★", ""), MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    Process.Start(@"updater.exe");
                }
            }
            catch (Exception)
            {
                // Leave blank
            }
        }

        private void click_event(object sender, EventArgs e)
        {
            try
            {
                string get_line = string.Empty;
                string parent_name = ((Label)sender).Parent.Name;
                string output = Regex.Match(parent_name, @"\d+").Value;
                string text = string.Empty;

                string notifications_file = Path.Combine(Path.GetTempPath(), "sb_notifications.txt");
                string line;
                StreamReader sr = new StreamReader(notifications_file);
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        string[] strArray = line.Split("*|*");

                        int count = 0;
                        int count_inner = 0;
                        foreach (object obj in strArray)
                        {
                            count++;

                            if (count == 1)
                            {
                                if (output == obj.ToString().Replace("\"", ""))
                                {
                                    foreach (object obje in strArray)
                                    {
                                        count_inner++;

                                        if (count_inner == 1)
                                        {
                                            _message_id_inner = obje.ToString().Replace("\"", "");
                                        }
                                        else if (count_inner == 2)
                                        {
                                            _message_date_inner = obje.ToString();
                                        }
                                        else if (count_inner == 3)
                                        {
                                            _message_title_inner = ReplaceString(obje.ToString());
                                        }
                                        else if (count_inner == 4)
                                        {
                                            _message_content_inner = ReplaceString(obje.ToString());
                                        }
                                        else if (count_inner == 5)
                                        {
                                            _message_status_inner = obje.ToString();
                                        }
                                        else if (count_inner == 6)
                                        {
                                            _message_type_inner = obje.ToString();
                                        }
                                        else if (count_inner == 7)
                                        {
                                            _message_edited_id_inner = obje.ToString();
                                        }
                                        else if (count_inner == 8)
                                        {
                                            const int SECOND = 1;
                                            const int MINUTE = 60 * SECOND;
                                            const int HOUR = 60 * MINUTE;
                                            const int DAY = 24 * HOUR;
                                            const int MONTH = 30 * DAY;

                                            string date_now_parse = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            DateTime date_now = DateTime.ParseExact(date_now_parse, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                                            string get_date_parse = _message_date_inner;
                                            DateTime get_date = DateTime.ParseExact(get_date_parse, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                                            var ts = new TimeSpan(date_now.Ticks - get_date.Ticks);
                                            double delta = Math.Abs(ts.TotalSeconds);

                                            if (delta < 1 * MINUTE)
                                            {
                                                _message_date_inner = "刚刚";
                                            }
                                            else if (delta < 2 * MINUTE)
                                            {
                                                _message_date_inner = "一分钟前";
                                            }
                                            else if (delta < 45 * MINUTE)
                                            {
                                                if (ts.Minutes == 1)
                                                {
                                                    _message_date_inner = "一分钟前";
                                                }
                                                else
                                                {
                                                    _message_date_inner = ts.Minutes + "分钟前";
                                                }
                                            }
                                            else if (delta < 90 * MINUTE)
                                            {
                                                _message_date_inner = "一小时前";
                                            }
                                            else if (delta < 24 * HOUR)
                                            {
                                                if (ts.Hours == 1)
                                                {
                                                    _message_date_inner = "一小时前";
                                                }
                                                else
                                                {
                                                    _message_date_inner = ts.Hours + "小时前";
                                                }
                                            }
                                            else if (delta < 48 * HOUR)
                                            {
                                                _message_date_inner = "昨日";
                                            }
                                            else if (delta < 30 * DAY)
                                            {
                                                if (ts.Days == 1)
                                                {
                                                    _message_date_inner = ts.Days + "天前";
                                                }
                                                else
                                                {
                                                    _message_date_inner = ts.Days + "天前";
                                                }
                                            }
                                            else
                                            {
                                                _message_date_inner = "已读信息";
                                            }
                                        }
                                    }

                                    text = File.ReadAllText(notifications_file);
                                    string get_last_char = line.Substring(line.Length - 1, 1);
                                    string output_line = string.Empty;
                                    if (get_last_char == "\"")
                                    {
                                        output_line = line.Remove(line.Length - 2);

                                        if (output_line.Substring(0, 1) == "\"")
                                        {
                                            string replace_output_line = output_line.Remove(0, 1);
                                            text = text.Replace(line, replace_output_line + "R");
                                        }
                                        else
                                        {
                                            text = text.Replace(line, output_line + "R");
                                        }
                                    }
                                    else
                                    {
                                        output_line = line.Remove(line.Length - 1);

                                        if (output_line.Substring(0, 1) == "\"")
                                        {
                                            string replace_output_line = output_line.Remove(0, 1);
                                            text = text.Replace(line, replace_output_line + "R");
                                        }
                                        else
                                        {
                                            text = text.Replace(line, output_line + "R");
                                        }
                                    }

                                    get_line = output_line + "R";
                                }
                            }
                        }
                    }
                }

                MessageBox.Show(_message_content_inner + "\n\n", _message_title_inner.Replace("★", ""), MessageBoxButtons.OK, MessageBoxIcon.Information);

                string final_replace_message_title_inner = string.Empty;

                sr.Close();
                File.WriteAllText(notifications_file, text);

                if (_message_title_inner.Contains("★"))
                {
                    string replace_message_title_inner = _message_title_inner.Replace("★", "");
                    final_replace_message_title_inner = "" + replace_message_title_inner.Remove(0, 1);
                    string text_get = File.ReadAllText(notifications_file);
                    string final_replace_message_title_inner_text_file = string.Empty;
                    string replace_message_title_inner_text_file = get_line.Replace("★", String.Empty);
                    string[] strArray_inner = replace_message_title_inner_text_file.Split("*|*");

                    ((Label)flowLayoutPanel_notifications.Controls.Find("label_title_notification_" + output, true)[0]).Text = Ellipsis(final_replace_message_title_inner, 20);
                    ((Label)flowLayoutPanel_notifications.Controls.Find("label_title_notification_" + output, true)[0]).ForeColor = Color.FromArgb(72, 72, 72);
                    ((Label)flowLayoutPanel_notifications.Controls.Find("label_content_notification_" + output, true)[0]).ForeColor = Color.FromArgb(72, 72, 72);

                    int count = 0;
                    string get_text = string.Empty;
                    foreach (object obj in strArray_inner)
                    {
                        count++;

                        if (count == 3)
                        {

                            get_text += obj.ToString().Remove(0, 1) + "*|*";
                        }
                        else if (count == 8)
                        {
                            get_text += obj.ToString();
                        }
                        else
                        {
                            get_text += obj.ToString() + "*|*";
                        }

                    }

                    text_get = text_get.Replace(get_line, get_text);
                    File.WriteAllText(notifications_file, text_get);

                    int get_notifcount = Convert.ToInt32(label_notificationscount.Text) - 1;

                    if (get_notifcount != 0)
                    {
                        label_notificationscount.Text = get_notifcount.ToString();
                    }
                    else
                    {
                        label_notificationscount.Text = "";
                    }
                }
            }
            catch (Exception)
            {
                // Leave blank
            }
        }

        private static string Ellipsis(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "。。。";
        }

        public string ReplaceString(string txtVal)
        {
            string newText = "";
            Regex regex = new Regex(@"(<br />|<br/>|</ br>|</br>)");
            newText = regex.Replace(txtVal, "\r\n");
            newText = newText.Replace("&amp;", "&");
            newText = newText.Replace("&lt;", "<");
            newText = newText.Replace("&gt;", ">");
            // Result    
            return newText;
        }

        // Delete Notifications
        private async Task GetNotificationDeleteAsync(string webservice_notifications_delete)
        {
            try
            {
                string notifications_file = Path.Combine(Path.GetTempPath(), "sb_notifications.txt");
                if (File.Exists(notifications_file))
                {
                    WebRequest.DefaultWebProxy = new WebProxy();
                    var client = new HttpClient();
                    var requestContent = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string, string>("macid", get_macAddress),
                        new KeyValuePair<string, string>("api_key", API_KEY),
                        new KeyValuePair<string, string>("brand_code", BRAND_CODE),
                    });

                    //notifications_delete_service
                    HttpResponseMessage response = await client.PostAsync(
                        webservice_notifications_delete,
                        requestContent);
                    HttpContent responseContent = response.Content;

                    using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                    {
                        string json = await reader.ReadToEndAsync();
                        string temp_file = Path.Combine(Path.GetTempPath(), "sb_notifications_temp.txt");
                        List<string> line_to_delete_1 = new List<string>();
                        int line_count_1 = 0;

                        StringBuilder sb = new StringBuilder();
                        using (var p = ChoJSONReader.LoadText(json).WithJSONPath("$..id"))
                        {
                            using (var w = new ChoCSVWriter(sb))
                                w.Write(p);
                        }

                        string id_to_delete = sb.ToString();

                        string get_line_delete = string.Empty;

                        string[] array = id_to_delete.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        foreach (String item in array)
                        {
                            string[] strArray_id = item.Split(",");

                            foreach (object obje in strArray_id)
                            {
                                string get_id = obje.ToString();

                                // delete
                                string line_delete;
                                StreamReader sr_delete = new StreamReader(notifications_file);
                                while ((line_delete = sr_delete.ReadLine()) != null)
                                {
                                    if (line_delete != "")
                                    {
                                        string[] strArray_inner = line_delete.Split("*|*");

                                        int count_update_inner = 0;
                                        foreach (object objec in strArray_inner)
                                        {
                                            count_update_inner++;

                                            if (count_update_inner == 1)
                                            {
                                                if (get_id == objec.ToString() && get_id != "")
                                                {
                                                    line_to_delete_1.Add(line_delete);
                                                    line_count_1++;
                                                    get_line_delete = line_delete;
                                                }
                                            }
                                        }
                                    }
                                }

                                sr_delete.Close();
                            }
                        }

                        if (!String.IsNullOrEmpty(get_line_delete))
                        {
                            isNewEntry = true;

                            for (int i = 0; i < line_count_1; i++)
                            {
                                string text = File.ReadAllText(notifications_file);
                                text = text.Replace(line_to_delete_1[i], "");
                                File.WriteAllText(notifications_file, text);
                            }
                        }
                        else
                        {
                            isNewEntry = false;
                        }
                    }
                }

                // version
                string path_version = Path.Combine(Path.GetTempPath(), "sb_version.txt");
                List<string> line_to_delete = new List<string>();
                int line_count = 0;
                if (File.Exists(path_version))
                {
                    string version = File.ReadAllText(path_version);
                    string message_type = string.Empty;
                    if (version != toolStripMenuItem_version.Text)
                    {
                        if (File.Exists(notifications_file))
                        {
                            string line;
                            StreamReader sr = new StreamReader(notifications_file);
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (line != "")
                                {
                                    string[] strArray = line.Split("*|*");

                                    int count = 0;
                                    foreach (object obj in strArray)
                                    {
                                        count++;

                                        if (count == 6)
                                        {
                                            message_type = obj.ToString();
                                        }
                                    }

                                    if (message_type == "1")
                                    {
                                        line_to_delete.Add(line);
                                        line_count++;

                                        File.Delete(path_version);
                                        StreamWriter sw = new StreamWriter(path_version);
                                        sw.Write(toolStripMenuItem_version.Text);
                                        sw.Close();
                                    }
                                }
                            }

                            sr.Close();
                        }
                    }

                    if (File.Exists(notifications_file))
                    {
                        for (int i = 0; i < line_count; i++)
                        {
                            string text = File.ReadAllText(notifications_file);
                            text = text.Replace(line_to_delete[i], "");
                            File.WriteAllText(notifications_file, text);
                        }
                    }
                }
                else
                {
                    StreamWriter sw = new StreamWriter(path_version);
                    sw.Write(toolStripMenuItem_version.Text);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: 1008", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //close = false;
                //Close();
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

                        panel_cefsharp.Visible = false;

                        pictureBox_loader.Visible = true;
                        label_loader.Visible = true;

                        timer_handler.Stop();
                        timer_handler.Start();

                        completed = true;
                        timeout = true;

                        chromeBrowser.Load(domain_get);
                    }
                    catch (Exception)
                    {
                        // Leave blank
                    }
                }
            }
        }

        // Timeout
        private void Timer_handler_Tick(object sender, EventArgs e)
        {
            chromeBrowser.Stop();
            timer_handler.Stop();
        }

        // Initialize Chromium
        private void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            settings.CefCommandLineArgs.Add("no-proxy-server", "1");
            settings.CefCommandLineArgs.Add("ppapi-flash-path", AppDomain.CurrentDomain.BaseDirectory + "pepflashplayer32_31_0_0_108.dll");
            settings.CefCommandLineArgs.Add("ppapi-flash-version", "31.0.0.108");
            settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            Cef.Initialize(settings);
            chromeBrowser = new ChromiumWebBrowser();
            chromeBrowser.MenuHandler = new CustomMenuHandler();
            chromeBrowser.LifeSpanHandler = new BrowserLifeSpanHandler();
            chromeBrowser.DownloadHandler = new DownloadHandler();
            panel_cefsharp.Controls.Add(chromeBrowser);

            chromeBrowser.LoadingStateChanged += BrowserLoadingStateChanged;
            chromeBrowser.TitleChanged += BrowserTitleChanged;
            chromeBrowser.AddressChanged += BrowserAddressChanged;
        }

        private void BrowserAddressChanged(object sender, AddressChangedEventArgs e)
        {
            handler_url = e.Address;
        }

        string start_load = "";
        string end_load = "";
        string datetime = "";

        // asd123
        private void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                pictureBox_back.Enabled = e.CanGoBack;
                goBackToolStripMenuItem.Enabled = e.CanGoBack;
                pictureBox_forward.Enabled = e.CanGoForward;
                forwardToolStripMenuItem.Enabled = e.CanGoForward;

                if (pictureBox_forward.Enabled == true)
                {
                    pictureBox_forward.Image = Properties.Resources.forward;
                }
                else
                {
                    pictureBox_forward.Image = Properties.Resources.forward_visible;
                }

                if (pictureBox_back.Enabled == true)
                {
                    pictureBox_back.Image = Properties.Resources.back;
                }
                else
                {
                    pictureBox_back.Image = Properties.Resources.back_visible;
                }
            }));

            if (e.IsLoading)
            {
                Invoke(new Action(() =>
                {
                    fully_loaded = 0;
                    elseloaded_i = 0;
                    timer_elseloaded.Stop();

                    if (domain_one_time)
                    {
                        pictureBox_loader.Visible = true;
                        label_loader.Visible = true;
                        panel_cefsharp.Visible = false;
                    }
                    else
                    {
                        pictureBox_loader_nav.Enabled = true;
                        pictureBox_loader_nav.Visible = true;

                        pictureBox_browserstop.Visible = true;
                        pictureBox_reload.Visible = false;

                        if (handler_url.Contains("page/player/wallet/deposit.jsp"))
                        {
                            string script = string.Format("document.getElementById('depositAmount3Party').value;");
                            chromeBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                            {
                                var response = x.Result;

                                if (response.Success && response.Result != null)
                                {
                                    var result = response.Result;
                                    SetAmount = result.ToString();
                                }
                            });
                        }
                    }

                    timer_loader_i = 1;
                    timer_loader.Start();

                    start_load = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    timer_timeout.Start();
                }));
            }

            if (!e.IsLoading)
            {
                Invoke(new Action(() =>
                {
                    if (!domain_one_time)
                    {
                        pictureBox_loader.Visible = false;
                        label_loader.Visible = false;

                        if (panel_help.Visible == true)
                        {
                            panel_cefsharp.Visible = false;
                        }
                        else
                        {
                            panel_cefsharp.Visible = true;
                        }

                        pictureBox_reload.Visible = true;
                        pictureBox_browserstop.Visible = false;

                        timer_loader.Stop();
                        label_loader.Text = "加载中。。。";

                        pictureBox_loader_nav.Visible = false;

                        if (last_url.Contains("/page/player/wallet/deposit.jsp"))
                        {
                            if (handler_url.Contains("about:blank"))
                            {
                                chromeBrowser.Back();

                                Form_YB_NewTab form_newtab = new Form_YB_NewTab(domain_get + "/player/payGateway?promoId=1&toBankId=-1&amount=" + SetAmount + "&method=1&bankType=1", "normal");
                                //Form_YB_NewTab form_newtab = new Form_YB_NewTab("http://jjdemoapi.ggow99038.com:1003/player/payGateway?promoId=1&toBankId=-1&amount=" + SetAmount + "&method=1&bankType=1", "normal");
                                int open_form = Application.OpenForms.Count;

                                if (open_form == 1)
                                {
                                    form_newtab.Show();
                                }
                                else
                                {
                                    Form_YB_NewTab.SetClose = true;
                                    form_newtab.Show();
                                }
                            }
                        }
                        
                        last_url = handler_url;
                    }
                }));
            }

            if (!e.IsLoading)
            {
                Invoke(new Action(async () =>
                {
                    chromeBrowser.Dock = DockStyle.Fill;

                    await Task.Run(async () =>
                    {
                        await Task.Delay(100);
                    });

                    if (domain_one_time)
                    {
                        fully_loaded++;

                        if (fully_loaded == 1)
                        {
                            timer_timeout.Stop();
                            end_load = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            datetime = DateTime.Now.ToString("HH");

                            string strValue = text_search;
                            string[] strArray = strValue.Split(',');

                            if (!String.IsNullOrEmpty(handler_title))
                            {
                                foreach (string obj in strArray)
                                {
                                    bool contains = handler_title.Contains(obj);
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
                            }
                            else
                            {
                                isHijacked = true;
                            }

                            if (isHijacked)
                            {
                                if (handler_title == "")
                                {
                                    string search_replace = handler_title;
                                    string upper_search = search_replace.ToUpper().ToString();

                                    StringBuilder sb = new StringBuilder(upper_search);
                                    sb.Replace("-", "");
                                    sb.Replace(".", "");
                                    sb.Replace(",", "");
                                    sb.Replace("!", "");

                                    string final_search = Regex.Replace(sb.ToString(), " {2,}", " ");
                                    var final_inaccessble_lists = inaccessble_lists.Select(m => m.ToUpper());
                                    string[] words = final_search.Split(' ');
                                    int i = 0;

                                    foreach (string word in words)
                                    {
                                        i++;

                                        if (word != "")
                                        {
                                            var match = final_inaccessble_lists.FirstOrDefault(stringToCheck => stringToCheck.Contains(word));

                                            if (match != null)
                                            {
                                                isInaccessible = true;
                                                break;
                                            }
                                            else
                                            {
                                                isInaccessible = false;
                                            }
                                        }

                                        if (i == 1 && search_replace == "")
                                        {
                                            isInaccessible = true;
                                        }
                                    }
                                }

                                char firstDigit = datetime[0];

                                if (int.Parse(datetime) % 2 == 0)
                                {
                                    if (firstDigit == '0')
                                    {
                                        datetime_created = DateTime.Now.ToString("yyyy-MM-dd 0" + datetime + ":00:00");
                                    }
                                    else
                                    {
                                        int final_get_hour = int.Parse(datetime) - 1;
                                        datetime_created = DateTime.Now.ToString("yyyy-MM-dd " + datetime + ":00:00");
                                    }
                                }
                                else
                                {
                                    if (firstDigit == '0')
                                    {
                                        int final_get_hour = int.Parse(datetime) - 1;
                                        datetime_created = DateTime.Now.ToString("yyyy-MM-dd 0" + final_get_hour + ":00:00");
                                    }
                                    else
                                    {
                                        int final_get_hour = int.Parse(datetime) - 1;
                                        datetime_created = DateTime.Now.ToString("yyyy-MM-dd " + final_get_hour + ":00:00");
                                    }
                                }

                                var html = "";

                                await chromeBrowser.GetSourceAsync().ContinueWith(taskHtml =>
                                {
                                    html = taskHtml.Result;
                                });

                                if (html.Contains("landing_image"))
                                {
                                    await Task.Run(async () =>
                                    {
                                        await Task.Delay(100);
                                    });

                                    back_button_i++;
                                    timer_detectifhijacked.Start();
                                    domain_one_time = false;
                                    last_index_hijacked_get = false;
                                    pictureBox_loader.Visible = false;
                                    label_loader.Visible = false;

                                    if (panel_help.Visible == true)
                                    {
                                        panel_cefsharp.Visible = false;
                                    }
                                    else
                                    {
                                        panel_cefsharp.Visible = true;
                                    }

                                    not_hijacked = true;

                                    pictureBox_reload.Enabled = true;
                                    pictureBox_reload.Image = Properties.Resources.refresh;
                                    pictureBox_browserhome.Enabled = true;
                                    pictureBox_browserhomehover.Enabled = true;
                                    pictureBox_browserhome.Image = Properties.Resources.browser_home;
                                    pictureBox_nofication.Enabled = true;
                                    pictureBox_noficationhover.Enabled = true;
                                    pictureBox_nofication.Image = Properties.Resources.notification;

                                    if (label_notificationscount.Text != "0")
                                    {
                                        label_notificationscount.Visible = true;
                                    }

                                    homeToolStripMenuItem.Enabled = true;
                                    reloadToolStripMenuItem.Enabled = true;
                                    cleanAndReloadToolStripMenuItem.Enabled = true;
                                    resetZoomToolStripMenuItem.Enabled = true;
                                    zoomInToolStripMenuItem.Enabled = true;
                                    zoomOutToolStripMenuItem.Enabled = true;
                                    label_clearcache.Enabled = true;
                                    label_getdiagnostics.Enabled = true;
                                    label_changedns.Enabled = true;
                                    elseload_return = false;

                                    label_chatus2.Enabled = true;
                                    label_emailus1.Enabled = true;
                                    label_chatus2.ForeColor = Color.FromArgb(235, 99, 6);
                                    label_emailus1.ForeColor = Color.FromArgb(235, 99, 6);

                                    timer_loader.Stop();
                                    label_loader.Text = "加载中。。。";

                                    string path_result = Path.GetTempPath() + "\\sb_result.txt";
                                    if (File.Exists(path_result))
                                    {
                                        UploadResult();
                                    }
                                }
                                else
                                {
                                    await Task.Run(async () =>
                                    {
                                        await Task.Delay(2000);
                                    });

                                    if (isInaccessible)
                                    {
                                        DataToTextFileInaccessible();
                                    }
                                    else if (isTimeout)
                                    {
                                        DataToTextFileTimeout();
                                        isTimeout = false;
                                    }
                                    else
                                    {
                                        DataToTextFileHijacked();
                                    }

                                    back_button_i++;
                                    last_index_hijacked_get = true;
                                    not_hijacked = false;
                                    label_loadingstate.Text = "1";
                                    label_loadingstate.Text = "0";
                                    elseload_return = true;
                                }
                            }
                            else if (isTimeout)
                            {
                                char firstDigit = datetime[0];

                                if (int.Parse(datetime) % 2 == 0)
                                {
                                    if (firstDigit == '0')
                                    {
                                        datetime_created = DateTime.Now.ToString("yyyy-MM-dd 0" + datetime + ":00:00");
                                    }
                                    else
                                    {
                                        int final_get_hour = int.Parse(datetime) - 1;
                                        datetime_created = DateTime.Now.ToString("yyyy-MM-dd " + datetime + ":00:00");
                                    }
                                }
                                else
                                {
                                    if (firstDigit == '0')
                                    {
                                        int final_get_hour = int.Parse(datetime) - 1;
                                        datetime_created = DateTime.Now.ToString("yyyy-MM-dd 0" + final_get_hour + ":00:00");
                                    }
                                    else
                                    {
                                        int final_get_hour = int.Parse(datetime) - 1;
                                        datetime_created = DateTime.Now.ToString("yyyy-MM-dd " + final_get_hour + ":00:00");
                                    }
                                }

                                await Task.Run(async () =>
                                {
                                    await Task.Delay(2000);
                                });

                                DataToTextFileTimeout();
                                isTimeout = false;

                                back_button_i++;
                                last_index_hijacked_get = true;
                                not_hijacked = false;
                                label_loadingstate.Text = "1";
                                label_loadingstate.Text = "0";
                                elseload_return = true;
                            }
                            else
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(100);
                                });

                                back_button_i++;
                                timer_detectifhijacked.Start();
                                domain_one_time = false;
                                last_index_hijacked_get = false;
                                pictureBox_loader.Visible = false;
                                label_loader.Visible = false;

                                if (panel_help.Visible == true)
                                {
                                    panel_cefsharp.Visible = false;
                                }
                                else
                                {
                                    panel_cefsharp.Visible = true;
                                }

                                not_hijacked = true;

                                pictureBox_reload.Enabled = true;
                                pictureBox_reload.Image = Properties.Resources.refresh;
                                pictureBox_browserhome.Enabled = true;
                                pictureBox_browserhomehover.Enabled = true;
                                pictureBox_browserhome.Image = Properties.Resources.browser_home;
                                pictureBox_nofication.Enabled = true;
                                pictureBox_noficationhover.Enabled = true;
                                pictureBox_nofication.Image = Properties.Resources.notification;

                                if (label_notificationscount.Text != "0")
                                {
                                    label_notificationscount.Visible = true;
                                }

                                homeToolStripMenuItem.Enabled = true;
                                reloadToolStripMenuItem.Enabled = true;
                                cleanAndReloadToolStripMenuItem.Enabled = true;
                                resetZoomToolStripMenuItem.Enabled = true;
                                zoomInToolStripMenuItem.Enabled = true;
                                zoomOutToolStripMenuItem.Enabled = true;
                                label_clearcache.Enabled = true;
                                label_getdiagnostics.Enabled = true;
                                label_changedns.Enabled = true;
                                elseload_return = false;
                                isNotHijackedLoaded = true;

                                label_chatus2.Enabled = true;
                                label_emailus1.Enabled = true;
                                label_chatus2.ForeColor = Color.FromArgb(235, 99, 6);
                                label_emailus1.ForeColor = Color.FromArgb(235, 99, 6);

                                timer_loader.Stop();
                                label_loader.Text = "加载中。。。";

                                string path_result = Path.GetTempPath() + "\\sb_result.txt";
                                if (File.Exists(path_result))
                                {
                                    UploadResult();
                                }
                            }
                        }
                        else
                        {
                            Invoke(new Action(() =>
                            {
                                if (elseload_return)
                                {
                                    elseload_return = false;
                                    timer_elseloaded.Start();
                                }
                            }));
                        }
                    }
                    else
                    {
                        if (isCacheClicked)
                        {
                            isCacheClicked = false;
                            label_clearcache.Text = "清除缓存";
                            label_clearcache.Cursor = Cursors.Hand;
                            MessageBox.Show("缓存已清除。", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }));
            }
        }

        private void timer_timeout_Tick(object sender, EventArgs e)
        {
            isTimeout = true;
            chromeBrowser.Stop();
        }

        private void UploadResult()
        {
            String read = "";
            // Insert
            string path_result = Path.GetTempPath() + "\\sb_result.txt";
            if (File.Exists(path_result))
            {
                read = File.ReadAllText(path_result);
            }

            StringBuilder sb = new StringBuilder();
            using (var p = ChoCSVReader.LoadText(read).WithFirstLineHeader())
            {
                using (var w = new ChoJSONWriter(sb))
                {
                    w.Write(p);
                }
            }

            int upload = 1;
            while (upload <= 5)
            {
                try
                {
                    WebRequest.DefaultWebProxy = new WebProxy();
                    using (var client = new WebClient())
                    {
                        string auth = "r@inCh3ckd234b70";
                        string type = "reports_normal";
                        string request = "http://raincheck.ssitex.com/api/api.php";
                        string reports = sb.ToString();

                        NameValueCollection postData = new NameValueCollection()
                                    {
                                        { "auth", auth },
                                        { "type", type },
                                        { "reports", reports },
                                    };

                        pagesource_history = Encoding.UTF8.GetString(client.UploadValues(request, postData));

                        if (pagesource_history == "SUCCESS")
                        {
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    upload++;
                }
            }

        }

        private void DataToTextFileHijacked()
        {
            try
            {
                string path_result = Path.GetTempPath() + "\\sb_result.txt";

                if (File.Exists(path_result))
                {
                    StreamWriter sw = new StreamWriter(path_result, true, Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path_result).Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path_result, true, Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_start_load = start_load;
                    if (File.ReadLines(path_result).Any(line => line.Contains(contain_start_load)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swwww = new StreamWriter(path_result, true, Encoding.UTF8);

                        if (string.IsNullOrEmpty(_isp))
                        {
                            _isp = "-";
                        }

                        if (string.IsNullOrEmpty(_city))
                        {
                            _city = "-";
                        }

                        string webtitle_replace = handler_title;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swwww.WriteLine("," + domain_get + ",H" + ",5" + "," + start_load + "," + end_load + "," + webtitle.ToString() + "," + handler_url + ",-" + ",-" + ",-" + "," + _isp + "," + _city + ",-," + datetime_created + "," + ",S");
                        swwww.Close();
                    }
                }
                else
                {
                    StreamWriter sw = new StreamWriter(path_result, true, Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path_result).Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path_result, true, Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_start_load = start_load;
                    if (File.ReadLines(path_result).Any(line => line.Contains(contain_start_load)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swwww = new StreamWriter(path_result, true, Encoding.UTF8);

                        if (string.IsNullOrEmpty(_isp))
                        {
                            _isp = "-";
                        }

                        if (string.IsNullOrEmpty(_city))
                        {
                            _city = "-";
                        }

                        string webtitle_replace = handler_title;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swwww.WriteLine("," + domain_get + ",H" + ",5" + "," + start_load + "," + end_load + "," + webtitle.ToString() + "," + handler_url + ",-" + ",-" + ",-" + "," + _isp + "," + _city + ",-," + datetime_created + "," + ",S");
                        swwww.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //var st = new StackTrace(ex, true);
                //var frame = st.GetFrame(0);
                //var line = frame.GetFileLineNumber();
                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: 1008", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }

        private void DataToTextFileInaccessible()
        {
            try
            {
                string path_result = Path.GetTempPath() + "\\sb_result.txt";

                if (File.Exists(path_result))
                {
                    StreamWriter sw = new StreamWriter(path_result, true, Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path_result).Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path_result, true, Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_start_load = start_load;
                    if (File.ReadLines(path_result).Any(line => line.Contains(contain_start_load)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swwww = new StreamWriter(path_result, true, Encoding.UTF8);

                        if (string.IsNullOrEmpty(_isp))
                        {
                            _isp = "-";
                        }

                        if (string.IsNullOrEmpty(_city))
                        {
                            _city = "-";
                        }

                        string webtitle_replace = handler_title;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swwww.WriteLine("," + domain_get + ",I" + ",5" + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + "," + handler_url + ",-" + "," + _isp + "," + _city + ",-," + datetime_created + "," + ",S");
                        swwww.Close();
                    }
                }
                else
                {
                    StreamWriter sw = new StreamWriter(path_result, true, Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path_result).Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path_result, true, Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_start_load = start_load;
                    if (File.ReadLines(path_result).Any(line => line.Contains(contain_start_load)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swwww = new StreamWriter(path_result, true, Encoding.UTF8);

                        if (string.IsNullOrEmpty(_isp))
                        {
                            _isp = "-";
                        }

                        if (string.IsNullOrEmpty(_city))
                        {
                            _city = "-";
                        }

                        string webtitle_replace = handler_title;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swwww.WriteLine("," + domain_get + ",I" + ",5" + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + "," + handler_url + ",-" + "," + _isp + "," + _city + ",-," + datetime_created + "," + ",S");
                        swwww.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //var st = new StackTrace(ex, true);
                //var frame = st.GetFrame(0);
                //var line = frame.GetFileLineNumber();
                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: 1008", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }

        private void DataToTextFileTimeout()
        {
            try
            {
                string path_result = Path.GetTempPath() + "\\sb_result.txt";

                if (File.Exists(path_result))
                {
                    StreamWriter sw = new StreamWriter(path_result, true, Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path_result).Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path_result, true, Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_start_load = start_load;
                    if (File.ReadLines(path_result).Any(line => line.Contains(contain_start_load)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swwww = new StreamWriter(path_result, true, Encoding.UTF8);

                        if (string.IsNullOrEmpty(_isp))
                        {
                            _isp = "-";
                        }

                        if (string.IsNullOrEmpty(_city))
                        {
                            _city = "-";
                        }

                        string webtitle_replace = handler_title;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swwww.WriteLine("," + domain_get + ",T" + ",5" + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + _isp + "," + _city + ",-," + datetime_created + "," + ",S");
                        swwww.Close();
                    }
                }
                else
                {
                    StreamWriter sw = new StreamWriter(path_result, true, Encoding.UTF8);
                    sw.Close();

                    // Header
                    string contain_text_header = "id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type";
                    if (File.ReadLines(path_result).Any(line => line.Contains(contain_text_header)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swww = new StreamWriter(path_result, true, Encoding.UTF8);
                        swww.WriteLine("id, domain_name, status, brand, start_load, end_load, text_search, url_hijacker, hijacker, remarks, printscreen, isp, city, t_id, datetime_created, action_by, type");

                        swww.Close();
                    }

                    string contain_start_load = start_load;
                    if (File.ReadLines(path_result).Any(line => line.Contains(contain_start_load)))
                    {
                        // Leave for blank
                    }
                    else
                    {
                        StreamWriter swwww = new StreamWriter(path_result, true, Encoding.UTF8);

                        if (string.IsNullOrEmpty(_isp))
                        {
                            _isp = "-";
                        }

                        if (string.IsNullOrEmpty(_city))
                        {
                            _city = "-";
                        }

                        string webtitle_replace = handler_title;
                        StringBuilder webtitle = new StringBuilder(webtitle_replace);
                        webtitle.Replace(",", "");
                        webtitle.Replace("，", " ");

                        swwww.WriteLine("," + domain_get + ",T" + ",5" + "," + start_load + "," + end_load + "," + webtitle.ToString() + ",-" + ",-" + ",-" + ",-" + "," + _isp + "," + _city + ",-," + datetime_created + "," + ",S");
                        swwww.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //var st = new StackTrace(ex, true);
                //var frame = st.GetFrame(0);
                //var line = frame.GetFileLineNumber();
                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: 1008", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Close();
            }
        }

        private void BrowserTitleChanged(object sender, TitleChangedEventArgs e)
        {
            handler_title = e.Title;
        }

        // Loading State Changed
        private async void Label_loadingstate_TextChangedAsync(object sender, EventArgs e)
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
                            //#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                            await GetTextToTextAsync(web_service[current_web_service]);
                            //#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        }
                        else
                        {
                            pictureBox_loader.Visible = false;
                            label_loader.Visible = false;
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

        // Form Closing
        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Invoke(new Action(() =>
            {
                Cef.Shutdown();
                Application.Exit();
            }));

            //if (close)
            //{
            //    DialogResult dr = MessageBox.Show("退出程序？", "永宝快线", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (dr == DialogResult.No)
            //    {
            //        e.Cancel = true;
            //    }
            //    else
            //    {
            //        Invoke(new Action(() =>
            //        {
            //            Cef.Shutdown();
            //            Application.Exit();
            //        }));
            //    }
            //}
            //else
            //{
            //    Invoke(new Action(() =>
            //    {
            //        Cef.Shutdown();
            //        Application.Exit();
            //    }));
            //}
        }

        // Get external IP
        private string GetExternalIp()
        {
            try
            {
                string externalIP;
                WebRequest.DefaultWebProxy = new WebProxy();
                externalIP = (new WebClient()).DownloadString("https://canihazip.com/s");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();

                return externalIP;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("There is a problem! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: 1004", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Get MAC Address
        private void GetMACAddress()
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

                if (macAddress.Equals("") || macAddress == null)
                {
                    foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                            nic.OperationalStatus == OperationalStatus.Up)
                        {
                            macAddress = nic.GetPhysicalAddress().ToString();
                        }
                    }

                    get_macAddress = macAddress;
                }
                else
                {
                    get_macAddress = macAddress;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("There is a problem! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: 1005", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Get IP Info
        private void GetIPInfo()
        {
            try
            {
                var API_PATH_IP_API = "http://ip-api.com/json/" + GetExternalIp();

                WebRequest.DefaultWebProxy = new WebProxy();
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.BaseAddress = new Uri(API_PATH_IP_API);
                    HttpResponseMessage response = client.GetAsync(API_PATH_IP_API).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        isIPInserted = true;
                        var locationDetails = response.Content.ReadAsAsync<IpInfo>().GetAwaiter().GetResult();
                        if (locationDetails != null)
                        {
                            _mac_address = get_macAddress;
                            _external_ip = GetExternalIp();
                            _isp = locationDetails.isp.Replace("'", "''");
                            _city = locationDetails.city.Replace("'", "''");
                            _province = locationDetails.regionName.Replace("'", "''");
                            _country = locationDetails.country.Replace("'", "''");
                            InsertDevice(send_service[current_web_service], _external_ip, _mac_address, _city, _province, _country);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("There is a problem! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: 1006", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertDevice(string domain, string ip, string mac_address, string city, string province, string country)
        {
            try
            {
                WebRequest.DefaultWebProxy = new WebProxy();
                using (var wb = new WebClient())
                {
                    var data = new NameValueCollection
                    {
                        ["api_key"] = API_KEY,
                        ["brand_code"] = BRAND_CODE,
                        ["ip"] = ip,
                        ["macid"] = mac_address,
                        ["city"] = city,
                        ["province"] = province,
                        ["country"] = country
                    };

                    var response = wb.UploadValues(domain, "POST", data);
                    string responseInString = Encoding.UTF8.GetString(response);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: 1003", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //close = false;
                //Close();
            }
        }

        private void pictureBox_minimize_MouseHover(object sender, EventArgs e)
        {
            pictureBox_minimize.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_minimize_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_minimize.BackColor = Color.FromArgb(235, 99, 6);
        }

        private void pictureBox_maximize_MouseHover(object sender, EventArgs e)
        {
            pictureBox_maximize.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_maximize_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_maximize.BackColor = Color.FromArgb(235, 99, 6);
        }

        private void pictureBox_close_MouseHover(object sender, EventArgs e)
        {
            if (!IsCloseVisible)
            {
                pictureBox_close.BackColor = Color.FromArgb(197, 112, 53);
            }
        }

        private void pictureBox_close_MouseLeave(object sender, EventArgs e)
        {
            if (!IsCloseVisible)
            {
                pictureBox_close.BackColor = Color.FromArgb(235, 99, 6);
            }
        }

        private void pictureBox_menu_MouseHover(object sender, EventArgs e)
        {
            Point position = new Point(pictureBox_menu.Left, pictureBox_menu.Height);
            ToolStripMenuItem.DropDown.Show(pictureBox_menu, position);

            pictureBox_hover.BackColor = Color.FromArgb(197, 112, 53);
            pictureBox_menu.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_menu_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_hover.BackColor = Color.FromArgb(235, 99, 6);
            pictureBox_menu.BackColor = Color.FromArgb(235, 99, 6);
        }

        private void pictureBox_hover_MouseHover(object sender, EventArgs e)
        {
            Point position = new Point(pictureBox_menu.Left, pictureBox_menu.Height);
            ToolStripMenuItem.DropDown.Show(pictureBox_menu, position);

            pictureBox_hover.BackColor = Color.FromArgb(197, 112, 53);
            pictureBox_menu.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_hover_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_hover.BackColor = Color.FromArgb(235, 99, 6);
            pictureBox_menu.BackColor = Color.FromArgb(235, 99, 6);
        }

        private void pictureBox_menu_Click(object sender, EventArgs e)
        {
            Point position = new Point(pictureBox_menu.Left, pictureBox_menu.Height);
            ToolStripMenuItem.DropDown.Show(pictureBox_menu, position);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (close)
            {
                IsCloseVisible = true;
                pictureBox_menu.BackColor = Color.FromArgb(197, 112, 53);
                pictureBox_hover.BackColor = Color.FromArgb(197, 112, 53);

                DialogResult dr = MessageBox.Show("退出程序？", "永宝快线", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    Invoke(new Action(() =>
                    {
                        Cef.Shutdown();
                        Application.Exit();
                    }));
                }
                else
                {
                    IsCloseVisible = false;
                    pictureBox_menu.BackColor = Color.FromArgb(235, 99, 6);
                    pictureBox_hover.BackColor = Color.FromArgb(235, 99, 6);
                }
            }
            else
            {
                Invoke(new Action(() =>
                {
                    Cef.Shutdown();
                    Application.Exit();
                }));
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (not_hijacked)
            {
                chromeBrowser.Reload(true);
            }
        }

        private void cleanAndReloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (not_hijacked)
            {
                chromeBrowser.Reload(false);
            }
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (not_hijacked)
            {
                chromeBrowser.SetZoomLevel(++defaultValue);
            }
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (not_hijacked)
            {
                chromeBrowser.SetZoomLevel(--defaultValue);
            }
        }

        private void resetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (not_hijacked)
            {
                defaultValue = 0;
                chromeBrowser.SetZoomLevel(0);
            }
        }

        private void goBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (not_hijacked)
            {
                chromeBrowser.Back();
            }
        }

        private void forwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (not_hijacked)
            {
                chromeBrowser.Forward();
            }
        }

        private void pictureBox_reload_Click(object sender, EventArgs e)
        {
            if (!hard_refresh)
            {
                chromeBrowser.Reload(true);
            }

            hard_refresh = false;
        }

        private void pictureBox_forward_Click(object sender, EventArgs e)
        {
            chromeBrowser.Forward();
        }

        private void pictureBox_back_Click(object sender, EventArgs e)
        {
            chromeBrowser.Back();
        }

        private void pictureBox_forward_MouseHover(object sender, EventArgs e)
        {
            pictureBox_forward.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_forward_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_forward.BackColor = Color.FromArgb(235, 99, 6);
        }

        private void pictureBox_back_MouseHover(object sender, EventArgs e)
        {
            pictureBox_back.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_back_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_back.BackColor = Color.FromArgb(235, 99, 6);
        }

        private void pictureBox_reload_MouseHover(object sender, EventArgs e)
        {
            pictureBox_reload.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_reload_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_reload.BackColor = Color.FromArgb(235, 99, 6);
        }

        private void pictureBox_reload_MouseDown(object sender, MouseEventArgs e)
        {
            timer_mouse.Start();
        }

        private void pictureBox_reload_MouseUp(object sender, MouseEventArgs e)
        {
            timer_mouse.Stop();
        }

        private void timer_mouse_Tick(object sender, EventArgs e)
        {
            chromeBrowser.Reload(false);
            pictureBox_reload.BackColor = Color.FromArgb(235, 99, 6);
            Cursor.Current = Cursors.Default;
            timer_mouse.Stop();
            hard_refresh = true;
        }

        private async void timer_detectifhijacked_TickAsync(object sender, EventArgs e)
        {
            timer_detectifhijacked.Stop();

            try
            {
                WebRequest.DefaultWebProxy = new WebProxy();
                using (var wc = new WebClient())
                {
                    wc.Headers.Add(HttpRequestHeader.UserAgent, "Leave blank");
                    wc.Encoding = Encoding.UTF8;
                    string data = await wc.DownloadStringTaskAsync(domain_get);
                    string title = Regex.Match(data, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;

                    timeout = false;
                    timer_handler.Stop();

                    string strValue = text_search;
                    string[] strArray = strValue.Split(',');
                    foreach (string obj in strArray)
                    {
                        bool contains = title.Contains(obj.Replace(" ", ""));
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
                                using (WebClient client = new WebClient())
                                {
                                    html = await Task.Run(() => client.DownloadString(replace_domain_get));
                                }
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
                                using (WebClient client = new WebClient())
                                {
                                    html = await Task.Run(() => client.DownloadString(domain_get));
                                }
                            }
                            catch (Exception)
                            {
                                html = "";
                            }
                        }

                        if (!html.Contains("landing_image"))
                        {
                            chromeBrowser.Dock = DockStyle.None;
                            pictureBox_loader.Visible = true;
                            label_loader.Visible = true;
                            domain_one_time = true;
                            label_loadingstate.Text = "1";
                            label_loadingstate.Text = "0";

                            pictureBox_reload.Enabled = false;
                            pictureBox_reload.Image = Properties.Resources.refresh_visible;
                            pictureBox_browserhome.Enabled = false;
                            pictureBox_browserhomehover.Enabled = false;
                            pictureBox_browserhome.Image = Properties.Resources.browser_homehover;
                            reloadToolStripMenuItem.Enabled = false;
                            cleanAndReloadToolStripMenuItem.Enabled = false;
                            resetZoomToolStripMenuItem.Enabled = false;
                            zoomInToolStripMenuItem.Enabled = false;
                            zoomOutToolStripMenuItem.Enabled = false;
                            //Form_YB_NewTab.Main_SetClose = true;
                            timer_detectifhijacked.Stop();
                            label_clearcache.Enabled = false;
                            label_getdiagnostics.Enabled = false;
                            label_changedns.Enabled = false;
                        }
                    }
                    else
                    {
                        timer_detectifhijacked.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        // Web Browser Loaded
        private async void WebBrowser_handler_DocumentCompletedAsync(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //if (webBrowser_handler.ReadyState == WebBrowserReadyState.Complete)
            //{
            //    if (e.Url == webBrowser_handler.Url)
            //    {
            //        // handlers
            //        webbrowser_handler_title = webBrowser_handler.DocumentTitle;
            //        webbrowser_handler_url = webBrowser_handler.Url;

            //        timer_detectifhijacked.Start();

            //        timeout = false;
            //        timer_handler.Stop();

            //        string strValue = text_search;
            //        string[] strArray = strValue.Split(',');
            //        foreach (string obj in strArray)
            //        {
            //            bool contains = webbrowser_handler_title.Contains(obj.Replace(" ", ""));
            //            if (contains == true)
            //            {
            //                Invoke(new Action(() =>
            //                {
            //                    isHijacked = false;
            //                }));

            //                break;
            //            }
            //            else if (!contains)
            //            {
            //                Invoke(new Action(() =>
            //                {
            //                    isHijacked = true;
            //                }));
            //            }
            //        }

            //        if (isHijacked)
            //        {
            //            var html = "";


            //            if (!domain_get.Contains("http"))
            //            {
            //                try
            //                {
            //                    replace_domain_get = "http://" + domain_get;
            //                    using (WebClient client = new WebClient())
            //                    {
            //                        html = await Task.Run(() => client.DownloadString(replace_domain_get));
            //                    }
            //                }
            //                catch (Exception)
            //                {
            //                    html = "";
            //                }
            //            }
            //            else
            //            {
            //                try
            //                {
            //                    using (WebClient client = new WebClient())
            //                    {
            //                        html = await Task.Run(() => client.DownloadString(domain_get));
            //                    }
            //                }
            //                catch (Exception)
            //                {
            //                    html = "";
            //                }
            //            }

            //            if (!html.Contains("landing_image"))
            //            {
            //                chromeBrowser.Dock = DockStyle.None;
            //                pictureBox_loader.BringToFront();
            //                pictureBox_loader.Visible = true;
            //                label_loader.Visible = true;
            //                domain_one_time = true;
            //                label_loadingstate.Text = "1";
            //                label_loadingstate.Text = "0";

            //                pictureBox_reload.Enabled = false;
            //                pictureBox_reload.Image = Properties.Resources.refresh_visible;
            //                pictureBox_browserhome.Enabled = false;
            //                pictureBox_browserhomehover.Enabled = false;
            //                pictureBox_browserhome.Image = Properties.Resources.browser_homehover;
            //                reloadToolStripMenuItem.Enabled = false;
            //                cleanAndReloadToolStripMenuItem.Enabled = false;
            //                resetZoomToolStripMenuItem.Enabled = false;
            //                zoomInToolStripMenuItem.Enabled = false;
            //                zoomOutToolStripMenuItem.Enabled = false;
            //                Form_YB_NewTab.Main_SetClose = true;
            //                timer_detectifhijacked.Stop();
            //                label_clearcache.Enabled = false;
            //                label_getdiagnostics.Enabled = false;
            //                label_changedns.Enabled = false;
            //            }
            //        }
            //    }
            //}
        }

        private void timer_elseloaded_Tick(object sender, EventArgs e)
        {
            Invoke(new Action(async () =>
            {
                elseloaded_i++;

                if (elseloaded_i == 1)
                {
                    end_load = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    datetime = DateTime.Now.ToString("HH");

                    string strValue = text_search;
                    string[] strArray = strValue.Split(',');

                    if (!String.IsNullOrEmpty(handler_title))
                    {
                        foreach (string obj in strArray)
                        {
                            bool contains = handler_title.Contains(obj);
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
                    }
                    else
                    {
                        isHijacked = true;
                    }

                    if (isHijacked)
                    {
                        if (handler_title == "")
                        {
                            string search_replace = handler_title;
                            string upper_search = search_replace.ToUpper().ToString();

                            StringBuilder sb = new StringBuilder(upper_search);
                            sb.Replace("-", "");
                            sb.Replace(".", "");
                            sb.Replace(",", "");
                            sb.Replace("!", "");

                            string final_search = Regex.Replace(sb.ToString(), " {2,}", " ");
                            var final_inaccessble_lists = inaccessble_lists.Select(m => m.ToUpper());
                            string[] words = final_search.Split(' ');
                            int i = 0;

                            foreach (string word in words)
                            {
                                i++;

                                if (word != "")
                                {
                                    var match = final_inaccessble_lists.FirstOrDefault(stringToCheck => stringToCheck.Contains(word));

                                    if (match != null)
                                    {
                                        isInaccessible = true;
                                        break;
                                    }
                                    else
                                    {
                                        isInaccessible = false;
                                    }
                                }

                                if (i == 1 && search_replace == "")
                                {
                                    isInaccessible = true;
                                }
                            }
                        }

                        char firstDigit = datetime[0];

                        if (int.Parse(datetime) % 2 == 0)
                        {
                            if (firstDigit == '0')
                            {
                                datetime_created = DateTime.Now.ToString("yyyy-MM-dd 0" + datetime + ":00:00");
                            }
                            else
                            {
                                int final_get_hour = int.Parse(datetime) - 1;
                                datetime_created = DateTime.Now.ToString("yyyy-MM-dd " + datetime + ":00:00");
                            }
                        }
                        else
                        {
                            if (firstDigit == '0')
                            {
                                int final_get_hour = int.Parse(datetime) - 1;
                                datetime_created = DateTime.Now.ToString("yyyy-MM-dd 0" + final_get_hour + ":00:00");
                            }
                            else
                            {
                                int final_get_hour = int.Parse(datetime) - 1;
                                datetime_created = DateTime.Now.ToString("yyyy-MM-dd " + final_get_hour + ":00:00");
                            }
                        }

                        var html = "";

                        await chromeBrowser.GetSourceAsync().ContinueWith(taskHtml =>
                        {
                            html = taskHtml.Result;
                        });

                        if (html.Contains("landing_image"))
                        {
                            await Task.Run(async () =>
                            {
                                await Task.Delay(100);
                            });

                            back_button_i++;
                            timer_detectifhijacked.Start();
                            domain_one_time = false;
                            last_index_hijacked_get = false;
                            pictureBox_loader.Visible = false;
                            label_loader.Visible = false;

                            if (panel_help.Visible == true)
                            {
                                panel_cefsharp.Visible = false;
                            }
                            else
                            {
                                panel_cefsharp.Visible = true;
                            }

                            not_hijacked = true;

                            pictureBox_reload.Enabled = true;
                            pictureBox_reload.Image = Properties.Resources.refresh;
                            pictureBox_browserhome.Enabled = true;
                            pictureBox_browserhomehover.Enabled = true;
                            pictureBox_browserhome.Image = Properties.Resources.browser_home;
                            pictureBox_nofication.Enabled = true;
                            pictureBox_noficationhover.Enabled = true;
                            pictureBox_nofication.Image = Properties.Resources.notification;

                            if (label_notificationscount.Text != "0")
                            {
                                label_notificationscount.Visible = true;
                            }

                            homeToolStripMenuItem.Enabled = true;
                            reloadToolStripMenuItem.Enabled = true;
                            cleanAndReloadToolStripMenuItem.Enabled = true;
                            resetZoomToolStripMenuItem.Enabled = true;
                            zoomInToolStripMenuItem.Enabled = true;
                            zoomOutToolStripMenuItem.Enabled = true;
                            label_clearcache.Enabled = true;
                            label_getdiagnostics.Enabled = true;
                            label_changedns.Enabled = true;
                            elseload_return = false;

                            label_chatus2.Enabled = true;
                            label_emailus1.Enabled = true;
                            label_chatus2.ForeColor = Color.FromArgb(235, 99, 6);
                            label_emailus1.ForeColor = Color.FromArgb(235, 99, 6);

                            timer_loader.Stop();
                            label_loader.Text = "加载中。。。";

                            string path_result = Path.GetTempPath() + "\\sb_result.txt";
                            if (File.Exists(path_result))
                            {
                                UploadResult();
                            }
                        }
                        else
                        {
                            await Task.Run(async () =>
                            {
                                await Task.Delay(2000);
                            });

                            if (isInaccessible)
                            {
                                DataToTextFileInaccessible();
                            }
                            else if (isTimeout)
                            {
                                DataToTextFileTimeout();
                                isTimeout = false;
                            }
                            else
                            {
                                DataToTextFileHijacked();
                            }

                            back_button_i++;
                            last_index_hijacked_get = true;
                            not_hijacked = false;
                            label_loadingstate.Text = "1";
                            label_loadingstate.Text = "0";
                            elseload_return = true;
                        }
                    }
                    else if (isTimeout)
                    {
                        char firstDigit = datetime[0];

                        if (int.Parse(datetime) % 2 == 0)
                        {
                            if (firstDigit == '0')
                            {
                                datetime_created = DateTime.Now.ToString("yyyy-MM-dd 0" + datetime + ":00:00");
                            }
                            else
                            {
                                int final_get_hour = int.Parse(datetime) - 1;
                                datetime_created = DateTime.Now.ToString("yyyy-MM-dd " + datetime + ":00:00");
                            }
                        }
                        else
                        {
                            if (firstDigit == '0')
                            {
                                int final_get_hour = int.Parse(datetime) - 1;
                                datetime_created = DateTime.Now.ToString("yyyy-MM-dd 0" + final_get_hour + ":00:00");
                            }
                            else
                            {
                                int final_get_hour = int.Parse(datetime) - 1;
                                datetime_created = DateTime.Now.ToString("yyyy-MM-dd " + final_get_hour + ":00:00");
                            }
                        }

                        await Task.Run(async () =>
                        {
                            await Task.Delay(2000);
                        });

                        DataToTextFileTimeout();
                        isTimeout = false;

                        back_button_i++;
                        last_index_hijacked_get = true;
                        not_hijacked = false;
                        label_loadingstate.Text = "1";
                        label_loadingstate.Text = "0";
                        elseload_return = true;
                    }
                    else
                    {
                        await Task.Run(async () =>
                        {
                            await Task.Delay(100);
                        });

                        back_button_i++;
                        timer_detectifhijacked.Start();
                        domain_one_time = false;
                        last_index_hijacked_get = false;
                        pictureBox_loader.Visible = false;
                        label_loader.Visible = false;

                        if (panel_help.Visible == true)
                        {
                            panel_cefsharp.Visible = false;
                        }
                        else
                        {
                            panel_cefsharp.Visible = true;
                        }

                        not_hijacked = true;

                        pictureBox_reload.Enabled = true;
                        pictureBox_reload.Image = Properties.Resources.refresh;
                        pictureBox_browserhome.Enabled = true;
                        pictureBox_browserhomehover.Enabled = true;
                        pictureBox_browserhome.Image = Properties.Resources.browser_home;
                        pictureBox_nofication.Enabled = true;
                        pictureBox_noficationhover.Enabled = true;
                        pictureBox_nofication.Image = Properties.Resources.notification;

                        if (label_notificationscount.Text != "0")
                        {
                            label_notificationscount.Visible = true;
                        }

                        homeToolStripMenuItem.Enabled = true;
                        reloadToolStripMenuItem.Enabled = true;
                        cleanAndReloadToolStripMenuItem.Enabled = true;
                        resetZoomToolStripMenuItem.Enabled = true;
                        zoomInToolStripMenuItem.Enabled = true;
                        zoomOutToolStripMenuItem.Enabled = true;
                        label_clearcache.Enabled = true;
                        label_getdiagnostics.Enabled = true;
                        label_changedns.Enabled = true;
                        elseload_return = false;
                        isNotHijackedLoaded = true;

                        label_chatus2.Enabled = true;
                        label_emailus1.Enabled = true;
                        label_chatus2.ForeColor = Color.FromArgb(235, 99, 6);
                        label_emailus1.ForeColor = Color.FromArgb(235, 99, 6);

                        timer_loader.Stop();
                        label_loader.Text = "加载中。。。";

                        string path_result = Path.GetTempPath() + "\\sb_result.txt";
                        if (File.Exists(path_result))
                        {
                            UploadResult();
                        }
                    }
                }
            }));
        }

        private void pictureBox_browserstop_Click(object sender, EventArgs e)
        {
            chromeBrowser.Stop();
        }

        private void pictureBox_browserstop_MouseHover(object sender, EventArgs e)
        {
            pictureBox_browserstop.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_browserstop_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_browserstop.BackColor = Color.FromArgb(235, 99, 6);
        }

        private void pictureBox_browserhome_MouseHover(object sender, EventArgs e)
        {
            pictureBox_browserhome.BackColor = Color.FromArgb(197, 112, 53);
            pictureBox_browserhomehover.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_browserhome_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_browserhome.BackColor = Color.FromArgb(235, 99, 6);
            pictureBox_browserhomehover.BackColor = Color.FromArgb(235, 99, 6);
        }

        private void pictureBox_browserhomehover_MouseHover(object sender, EventArgs e)
        {
            pictureBox_browserhome.BackColor = Color.FromArgb(197, 112, 53);
            pictureBox_browserhomehover.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_browserhomehover_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_browserhome.BackColor = Color.FromArgb(235, 99, 6);
            pictureBox_browserhomehover.BackColor = Color.FromArgb(235, 99, 6);
        }

        private void pictureBox_browserhome_Click(object sender, EventArgs e)
        {
            if (!help_click)
            {
                panel_help.Visible = false;
                help_click = true;

                if (pictureBox_loader.Visible == true)
                {
                    panel_cefsharp.Visible = false;
                }
                else
                {
                    panel_cefsharp.Visible = true;
                }

                if (!notification_click)
                {
                    panel_notification.Visible = true;
                    label_separator.Visible = true;
                    button_notification.Visible = true;
                }

                pictureBox_help.BackColor = Color.FromArgb(235, 99, 6);
                pictureBox_helphover.BackColor = Color.FromArgb(235, 99, 6);
            }

            chromeBrowser.Load(domain_get);
        }

        private void pictureBox_browserhomehover_Click(object sender, EventArgs e)
        {
            if (!help_click)
            {
                panel_help.Visible = false;
                help_click = true;

                if (pictureBox_loader.Visible == true)
                {
                    panel_cefsharp.Visible = false;
                }
                else
                {
                    panel_cefsharp.Visible = true;
                }

                if (!notification_click)
                {
                    panel_notification.Visible = true;
                    label_separator.Visible = true;
                    button_notification.Visible = true;
                }

                pictureBox_help.BackColor = Color.FromArgb(235, 99, 6);
                pictureBox_helphover.BackColor = Color.FromArgb(235, 99, 6);
            }

            chromeBrowser.Load(domain_get);
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!help_click)
            {
                panel_help.Visible = false;
                help_click = true;

                if (pictureBox_loader.Visible == true)
                {
                    panel_cefsharp.Visible = false;
                }
                else
                {
                    panel_cefsharp.Visible = true;
                }

                if (!notification_click)
                {
                    panel_notification.Visible = true;
                    label_separator.Visible = true;
                    button_notification.Visible = true;
                }
            }

            chromeBrowser.Load(domain_get);
        }

        private void pictureBox_help_MouseHover(object sender, EventArgs e)
        {
            pictureBox_help.BackColor = Color.FromArgb(197, 112, 53);
            pictureBox_helphover.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_help_MouseLeave(object sender, EventArgs e)
        {
            if (help_click)
            {
                pictureBox_help.BackColor = Color.FromArgb(235, 99, 6);
                pictureBox_helphover.BackColor = Color.FromArgb(235, 99, 6);
            }
        }

        private void pictureBox_helphover_MouseHover(object sender, EventArgs e)
        {
            pictureBox_help.BackColor = Color.FromArgb(197, 112, 53);
            pictureBox_helphover.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_helphover_MouseLeave(object sender, EventArgs e)
        {
            if (help_click)
            {
                pictureBox_help.BackColor = Color.FromArgb(235, 99, 6);
                pictureBox_helphover.BackColor = Color.FromArgb(235, 99, 6);
            }
        }

        private void pictureBox_nofication_MouseHover(object sender, EventArgs e)
        {
            pictureBox_nofication.BackColor = Color.FromArgb(197, 112, 53);
            pictureBox_noficationhover.BackColor = Color.FromArgb(197, 112, 53);
            label_notificationscount.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_nofication_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_nofication.BackColor = Color.FromArgb(235, 99, 6);
            pictureBox_noficationhover.BackColor = Color.FromArgb(235, 99, 6);
            label_notificationscount.BackColor = Color.FromArgb(235, 99, 6);
        }

        private void pictureBox_noficationhover_MouseHover(object sender, EventArgs e)
        {
            pictureBox_nofication.BackColor = Color.FromArgb(197, 112, 53);
            pictureBox_noficationhover.BackColor = Color.FromArgb(197, 112, 53);
            label_notificationscount.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_noficationhover_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_nofication.BackColor = Color.FromArgb(235, 99, 6);
            pictureBox_noficationhover.BackColor = Color.FromArgb(235, 99, 6);
            label_notificationscount.BackColor = Color.FromArgb(235, 99, 6);
        }

        private void pictureBox_help_Click(object sender, EventArgs e)
        {
            if (help_click)
            {
                panel_help.BringToFront();
                panel_help.Visible = true;
                help_click = false;
                panel_cefsharp.Visible = false;
                panel_notification.Visible = false;
                label_separator.Visible = false;
                button_notification.Visible = false;
            }
            else
            {
                panel_help.Visible = false;
                help_click = true;

                if (pictureBox_loader.Visible == true)
                {
                    panel_cefsharp.Visible = false;
                }
                else
                {
                    panel_cefsharp.Visible = true;
                }

                if (!notification_click)
                {
                    panel_notification.Visible = true;
                    label_separator.Visible = true;
                    button_notification.Visible = true;
                }

                pictureBox_help.BackColor = Color.FromArgb(235, 99, 6);
                pictureBox_helphover.BackColor = Color.FromArgb(235, 99, 6);
            }
        }

        private void pictureBox_helphover_Click(object sender, EventArgs e)
        {
            if (help_click)
            {
                panel_help.BringToFront();
                panel_help.Visible = true;
                help_click = false;
                panel_cefsharp.Visible = false;
                panel_notification.Visible = false;
                label_separator.Visible = false;
                button_notification.Visible = false;
            }
            else
            {
                panel_help.Visible = false;
                help_click = true;

                if (pictureBox_loader.Visible == true)
                {
                    panel_cefsharp.Visible = false;
                }
                else
                {
                    panel_cefsharp.Visible = true;
                }

                if (!notification_click)
                {
                    panel_notification.Visible = true;
                    label_separator.Visible = true;
                    button_notification.Visible = true;
                }
            }
        }

        [DefaultValue(30)]
        public int Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                this.RecreateRegion();
            }
        }

        private GraphicsPath GetRoundRectagle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y + bounds.Height - radius,
                        radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            return path;
        }

        private void RecreateRegion()
        {
            var bounds = new Rectangle(panel_help.ClientRectangle.Location, panel_help.ClientRectangle.Size);
            bounds.Inflate(-1, -1);
            using (var path = GetRoundRectagle(bounds, this.Radius))
                panel_help.Region = new Region(path);
            panel_help.Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RecreateRegion();
        }

        private void pictureBox_helpback_Click(object sender, EventArgs e)
        {
            if (help_click)
            {
                panel_help.BringToFront();
                panel_help.Visible = true;
                help_click = false;
                panel_cefsharp.Visible = false;
                panel_notification.Visible = false;
                label_separator.Visible = false;
                button_notification.Visible = false;
            }
            else
            {
                panel_help.Visible = false;
                help_click = true;

                if (pictureBox_loader.Visible == true)
                {
                    panel_cefsharp.Visible = false;
                }
                else
                {
                    panel_cefsharp.Visible = true;
                }

                if (!notification_click)
                {
                    panel_notification.Visible = true;
                    label_separator.Visible = true;
                    button_notification.Visible = true;
                }

                pictureBox_help.BackColor = Color.FromArgb(235, 99, 6);
                pictureBox_helphover.BackColor = Color.FromArgb(235, 99, 6);
            }
        }

        private void pictureBox_nofication_Click(object sender, EventArgs e)
        {
            if (help_click)
            {
                var panel_cefsharp_resize = panel_cefsharp.Width - 280;
                var panel_cefsharp_size = panel_cefsharp.Width + 280;

                if (notification_click)
                {
                    notification_click = false;

                    panel_cefsharp.Width = panel_cefsharp_resize;

                    panel_notification.Visible = true;
                    label_separator.Visible = true;
                    button_notification.Visible = true;
                    pictureBox_nofication.Image = Properties.Resources.notification_back;
                }
                else
                {
                    notification_click = true;

                    panel_cefsharp.Width = panel_cefsharp_size;

                    panel_notification.Visible = false;
                    label_separator.Visible = false;
                    button_notification.Visible = false;
                    pictureBox_nofication.Image = Properties.Resources.notification;
                }
            }
        }

        private void pictureBox_noficationhover_Click(object sender, EventArgs e)
        {
            if (help_click)
            {
                var panel_cefsharp_resize = panel_cefsharp.Width - 280;
                var panel_cefsharp_size = panel_cefsharp.Width + 280;

                if (notification_click)
                {
                    notification_click = false;

                    panel_cefsharp.Width = panel_cefsharp_resize;

                    panel_notification.Visible = true;
                    label_separator.Visible = true;
                    button_notification.Visible = true;
                    pictureBox_nofication.Image = Properties.Resources.notification_back;
                }
                else
                {
                    notification_click = true;

                    panel_cefsharp.Width = panel_cefsharp_size;

                    panel_notification.Visible = false;
                    label_separator.Visible = false;
                    button_notification.Visible = false;
                    pictureBox_nofication.Image = Properties.Resources.notification;
                }
            }
        }

        private void label_clearcache_Click(object sender, EventArgs e)
        {
            if (!isCacheClicked)
            {
                label_clearcache.Cursor = Cursors.Default;
                isCacheClicked = true;
                chromeBrowser.Reload(false);
                label_clearcache.Text = "缓存清除中。。。";

                panel_help.Visible = false;
                help_click = true;

                if (pictureBox_loader.Visible == true)
                {
                    panel_cefsharp.Visible = false;
                }
                else
                {
                    panel_cefsharp.Visible = true;
                }

                if (!notification_click)
                {
                    panel_notification.Visible = true;
                    label_separator.Visible = true;
                    button_notification.Visible = true;
                }

                pictureBox_help.BackColor = Color.FromArgb(235, 99, 6);
                pictureBox_helphover.BackColor = Color.FromArgb(235, 99, 6);
            }
        }

        private void pictureBox_hover_Click(object sender, EventArgs e)
        {
            Point position = new Point(pictureBox_menu.Left, pictureBox_menu.Height);
            ToolStripMenuItem.DropDown.Show(pictureBox_menu, position);
        }

        private void pictureBox_minimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void label_getdiagnostics_Click(object sender, EventArgs e)
        {
            if (isGetDiagnosticsClicked)
            {
                result_traceroute = "";
                result_ping = "";

                timer_diagnostics.Start();
                GetTraceRoute(domain_get);

                label_getdiagnostics.Cursor = Cursors.Default;
                isGetDiagnosticsClicked = false;

                panel_help.Visible = false;
                help_click = true;

                if (pictureBox_loader.Visible == true)
                {
                    panel_cefsharp.Visible = false;
                }
                else
                {
                    panel_cefsharp.Visible = true;
                    GetDeviceInfo();
                }

                if (!notification_click)
                {
                    panel_notification.Visible = true;
                    label_separator.Visible = true;
                    button_notification.Visible = true;
                }

                pictureBox_help.BackColor = Color.FromArgb(235, 99, 6);
                pictureBox_helphover.BackColor = Color.FromArgb(235, 99, 6);
            }
        }

        const int WS_MINIMIZEBOX = 0x20000;
        const int CS_DBLCLKS = 0x8;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                cp.ClassStyle |= CS_DBLCLKS;
                return cp;
            }
        }
        
        private void GetDeviceInfo()
        {
            try
            {
                string clockSpeed = "";
                string procName = "";
                string manufacturer = "";
                string get_ram = "";
                string description = "";

                using (ManagementObjectSearcher win32Proc = new ManagementObjectSearcher("select * from Win32_Processor"),
                win32CompSys = new ManagementObjectSearcher("select * from Win32_ComputerSystem"),
                win32Memory = new ManagementObjectSearcher("select * from Win32_PhysicalMemory"),
                ram_get = new ManagementObjectSearcher("select * from Win32_OperatingSystem "))
                {
                    foreach (ManagementObject obj in win32Proc.Get())
                    {
                        clockSpeed = obj["CurrentClockSpeed"].ToString();
                        procName = obj["Name"].ToString();
                        manufacturer = obj["Manufacturer"].ToString();
                        description = obj["Description"].ToString();
                    }

                    foreach (ManagementObject obj in ram_get.Get())
                    {
                        get_ram = obj["TotalVisibleMemorySize"].ToString();
                    }
                }

                string ram = get_ram.Substring(0, 2);

                string temp_deviceinfo = Path.Combine(Path.GetTempPath(), "deviceinfo.txt");

                if (File.Exists(temp_deviceinfo))
                {
                    File.Delete(temp_deviceinfo);

                    using (StreamWriter sw = new StreamWriter(temp_deviceinfo, true, Encoding.UTF8))
                    {
                        sw.WriteLine("\r\n" + "Windows: " + Environment.OSVersion + "\r\n" + "Processor: " + procName + "\r\n" + "Installed memory (RAM): " + ram + ".0 GB\r\n" + "System type: " + description + "\r\n");
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(temp_deviceinfo, true, Encoding.UTF8))
                    {
                        sw.WriteLine("\r\n" + "Windows: " + Environment.OSVersion + "\r\n" + "Processor: " + procName + "\r\n" + "Installed memory (RAM): " + ram + ".0 GB\r\n" + "System type: " + description + "\r\n");
                    }
                }
            }
            catch (Exception e)
            {
                // Leave blank
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Form_YB_NewTab form_newtab = new Form_YB_NewTab("https://static.meiqia.com/dist/standalone.html?_=t&eid=109551", "help");
            int open_form = Application.OpenForms.Count;

            if (open_form == 1)
            {
                form_newtab.Show();
            }
            else
            {
                Form_YB_NewTab.SetClose = true;
                form_newtab.Show();
            }
        }

        private void label_emailus1_Click(object sender, EventArgs e)
        {
            Process.Start("mailto:cs@yb188188.com");
        }

        private async void timer_notifications_TickAsync(object sender, EventArgs e)
        {
            timer_notifications.Stop();
            
            //#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            await GetNotificationAsync(notifications_service[current_web_service]);
            //#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void timer_close_Tick(object sender, EventArgs e)
        {
            Cef.Shutdown();
            Application.Exit();
        }

        private void timer_notifications_detect_Tick(object sender, EventArgs e)
        {
            //fdgfdg
        }

        private void button_notification_Paint(object sender, PaintEventArgs e)
        {
            GraphicsPath p = new GraphicsPath();
            p.AddEllipse(1, 1, button_notification.Width - 4, button_notification.Height - 4);
            button_notification.Region = new Region(p);
        }

        private void button_notification_Click(object sender, EventArgs e)
        {
            string notifications_file = Path.Combine(Path.GetTempPath(), "sb_notifications.txt");

            if (isNewEntry)
            {
                isNewEntry = false;

                if (File.Exists(notifications_file))
                {
                    label_notificationstatus.Visible = false;
                    flowLayoutPanel_notifications.Visible = true;
                    flowLayoutPanel_notifications.BringToFront();

                    NotificationsAsync();
                }
                else
                {
                    label_notificationstatus.Location = new Point(7, 32);
                    label_notificationstatus.Visible = true;
                    label_notificationstatus.BringToFront();
                    flowLayoutPanel_notifications.Visible = false;
                }
            }
            else
            {
                isNewEntry = false;
            }
        }

        int timer_loader_i = 1;
        private string handler_url;
        private string pagesource_history;
        private string datetime_created;
        private bool isInaccessible;
        private bool isTimeout;
        private string get_macAddress;
        private bool isPingStarted = true;
        private bool isGetDiagnosticsClicked = true;
        private bool isIPInserted = false;
        private bool isCacheClicked = false;
        private string last_url = "";

        private void timer_loader_Tick(object sender, EventArgs e)
        {
            timer_loader_i++;

            if (timer_loader_i < 5)
            {
                label_loader.Text = "加载中。。。";
            }
            else if (timer_loader_i < 10)
            {
                label_loader.Text = "资料收取中。。。";
            }
            else if (timer_loader_i > 15)
            {
                label_loader.Text = "准备中。。。";
            }
        }

        private void timer_yb_Tick(object sender, EventArgs e)
        {
            panel_landing.Visible = false;
            timer_landing.Stop();
        }

        private void pictureBox_maximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
            {
                //MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
                //WindowState = FormWindowState.Maximized;
                Screen screen = Screen.FromControl(this);
                int x = screen.WorkingArea.X - screen.Bounds.X;
                int y = screen.WorkingArea.Y - screen.Bounds.Y;
                this.MaximizedBounds = new Rectangle(x, y,
                    screen.WorkingArea.Width, screen.WorkingArea.Height);
                this.MaximumSize = screen.WorkingArea.Size;
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void pictureBox_close_Click(object sender, EventArgs e)
        {
            if (close)
            {
                IsCloseVisible = true;
                pictureBox_close.BackColor = Color.FromArgb(197, 112, 53);

                DialogResult dr = MessageBox.Show("退出程序？", "永宝快线", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    Invoke(new Action(() =>
                    {
                        Cef.Shutdown();
                        Environment.Exit(0);
                    }));
                }
                else
                {
                    IsCloseVisible = false;
                    pictureBox_close.BackColor = Color.FromArgb(235, 99, 6);
                }
            }
            else
            {
                Invoke(new Action(() =>
                {
                    Cef.Shutdown();
                    Environment.Exit(0);
                }));
            }
        }

        private void label_titlebar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks >= 2)
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                }
                else
                {
                    //MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
                    //WindowState = FormWindowState.Maximized;
                    Screen screen = Screen.FromControl(this);
                    int x = screen.WorkingArea.X - screen.Bounds.X;
                    int y = screen.WorkingArea.Y - screen.Bounds.Y;
                    this.MaximizedBounds = new Rectangle(x, y,
                        screen.WorkingArea.Width, screen.WorkingArea.Height);
                    this.MaximumSize = screen.WorkingArea.Size;
                    this.WindowState = FormWindowState.Maximized;
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            SolidBrush defaultColor = new SolidBrush(Color.FromArgb(235, 99, 6));
            e.Graphics.FillRectangle(defaultColor, Top);
            e.Graphics.FillRectangle(defaultColor, Left);
            e.Graphics.FillRectangle(defaultColor, Right);
            e.Graphics.FillRectangle(defaultColor, Bottom);
        }

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x84)
            {
                var cursor = this.PointToClient(Cursor.Position);

                if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;

                else if (Top.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (Left.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
        }

        private void GetPing(string domain)
        {
            StringBuilder replace_domain = new StringBuilder(domain);
            replace_domain.Replace("https://", "");
            replace_domain.Replace("http://", "");
            replace_domain.Replace("www.", "");
            replace_domain.Replace(".com/.", ".com");

            var startInfo = new ProcessStartInfo(@"cmd.exe", "/c ping -n 8 " + replace_domain.ToString())
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            var proc = new Process { StartInfo = startInfo };

            ThreadStart ths = new ThreadStart(() =>
            {
                proc.Start();

                while (!proc.HasExited)
                {
                    Invoke(new Action(delegate
                    {
                        label_getdiagnostics.Text = "诊断中。。。";
                    }));

                    proc.WaitForExit(30000);
                }

                Invoke(new Action(delegate
                {
                    label_getdiagnostics.Text = "诊断";
                }));

                result_ping = proc.StandardOutput.ReadToEnd();
            });
            Thread th = new Thread(ths);
            th.Start();
        }

        private void GetTraceRoute(string domain)
        {
            StringBuilder replace_domain = new StringBuilder(domain);
            replace_domain.Replace("https://", "");
            replace_domain.Replace("http://", "");
            replace_domain.Replace("www.", "");
            replace_domain.Replace(".com/.", ".com");

            var startInfo = new ProcessStartInfo(@"cmd.exe", "/c tracert " + replace_domain.ToString())
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                LoadUserProfile = true,
                RedirectStandardOutput = true
            };

            var proc = new Process { StartInfo = startInfo };

            ThreadStart ths = new ThreadStart(() =>
            {
                proc.Start();

                while (!proc.HasExited)
                {
                    Invoke(new Action(delegate
                    {
                        label_getdiagnostics.Text = "诊断中。。。";
                    }));

                    proc.WaitForExit(30000);
                }

                result_traceroute = proc.StandardOutput.ReadToEnd();
            });
            Thread th = new Thread(ths);
            th.Start();
        }
        
        private void timer_diagnostics_Tick(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(result_traceroute))
            {
                if (isPingStarted)
                {
                    isPingStarted = false;
                    GetPing(domain_get);
                }

                if (!String.IsNullOrEmpty(result_traceroute) && !String.IsNullOrEmpty(result_ping))
                {
                    timer_diagnostics.Stop();
                    isGetDiagnosticsClicked = true;
                    isPingStarted = true;
                    label_getdiagnostics.Cursor = Cursors.Hand;

                    try
                    {
                        string temp_traceroute = Path.Combine(Path.GetTempPath(), "traceroute.txt");

                        if (File.Exists(temp_traceroute))
                        {
                            File.Delete(temp_traceroute);

                            using (StreamWriter sw = new StreamWriter(temp_traceroute, true, Encoding.UTF8))
                            {
                                sw.WriteLine(result_traceroute);
                            }
                        }
                        else
                        {
                            using (StreamWriter sw = new StreamWriter(temp_traceroute, true, Encoding.UTF8))
                            {
                                sw.WriteLine(result_traceroute);
                            }
                        }

                        string temp_ping = Path.Combine(Path.GetTempPath(), "ping.txt");

                        if (File.Exists(temp_ping))
                        {
                            File.Delete(temp_ping);

                            using (StreamWriter sw = new StreamWriter(temp_ping, true, Encoding.UTF8))
                            {
                                sw.WriteLine(result_ping);
                            }
                        }
                        else
                        {
                            using (StreamWriter sw = new StreamWriter(temp_ping, true, Encoding.UTF8))
                            {
                                sw.WriteLine(result_ping);
                            }
                        }

                        ZipFile zip = new ZipFile();
                        
                        zip.AddFile(Path.GetTempPath() + "\\deviceinfo.txt", "");
                        zip.AddFile(Path.GetTempPath() + "\\traceroute.txt", "");
                        zip.AddFile(Path.GetTempPath() + "\\ping.txt", "");
                        zip.Save(Path.GetTempPath() + "\\Diagnostics.zip");

                        // Read file data
                        FileStream fs = new FileStream(Path.GetTempPath() + "\\Diagnostics.zip", FileMode.Open, FileAccess.Read);
                        byte[] data = new byte[fs.Length];
                        fs.Read(data, 0, data.Length);
                        fs.Close();

                        // Generate post objects
                        Dictionary<string, object> postParameters = new Dictionary<string, object>();
                        postParameters.Add("api_key", API_KEY);
                        postParameters.Add("brand_code", BRAND_CODE);
                        postParameters.Add("macid", _mac_address);
                        postParameters.Add("zipfile", new FormUpload.FileParameter(data, "Diagnostics.zip", "application/zip"));

                        // Create request and receive response
                        string postURL = diagnostics_service[current_web_service];
                        string userAgent = "admin";
                        HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters);

                        // Process response
                        StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                        //string fullResponse = responseReader.ReadToEnd();
                        //MessageBox.Show(fullResponse);

                        File.Delete(Path.GetTempPath() + "\\deviceinfo.txt");
                        File.Delete(Path.GetTempPath() + "\\traceroute.txt");
                        File.Delete(Path.GetTempPath() + "\\ping.txt");
                        File.Delete(Path.GetTempPath() + "\\Diagnostics.zip");
                    }
                    catch (Exception ex)
                    {
                        // Leave blank
                    }

                    MessageBox.Show("诊断报告已发送。", "永宝快线", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        const UInt32 WM_CLOSE = 0x0010;

        [DllImport("user32.dll", EntryPoint = "SetWindowText", CharSet = CharSet.Unicode)]
        public static extern bool SetWindowText(IntPtr hWnd, String strNewWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string className, string windowName);

        private void RenameMessageBox()
        {
            IntPtr windowPtr = FindWindowByCaption(IntPtr.Zero, "JavaScript Alert - " + domain_get);

            if (windowPtr == IntPtr.Zero)
            {
                return;
            }
            
            SetWindowText(windowPtr, "温馨提示");
        }

        private void timer_messagebox_Tick(object sender, EventArgs e)
        {
            RenameMessageBox();
        }
    }
}