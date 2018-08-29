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
using System.Web.UI;
using System.Windows.Forms;

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
        private string _mac_address;
        private string _external_ip;
        private string _city;
        private string _country;
        private string _province;
        private string BRAND_CODE = "YB";
        private string API_KEY = "6b8c7e5617414bf2d4ace37600b6ab71";
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
        private static string result_ping;
        private static string result_traceroute;
        private string dumpPath;
        private int back_button_i;
        private bool elseload_return;
        private string notifications_get;
        private string _message_id;
        private string _message_edited_id;
        private string _message_title;
        private string _message_type;
        private string _message_content;
        private string _message_date;
        private string _message_status;
        private string _message_unread;
        private int notificationscount = 1;
        private string _message_id_inner;
        private string _message_date_inner;
        private string _message_title_inner;
        private string _message_content_inner;
        private string _message_status_inner;
        private string _message_type_inner;
        private string _message_edited_id_inner;
        private string _message_unread_inner;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        public Form_YB()
        {
            InitializeComponent();
        }

        // Form Load
        private void Form_Main_Load(object sender, EventArgs e)
        {
            InitializeChromium();
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
            NetworkAvailability();
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            GetTextToTextAsync(web_service[current_web_service]);
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            GetNotificationAsync(notifications_service[current_web_service]);
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            PictureBoxCenter();
            gHook = new GlobalKeyboardHook();
            gHook.KeyDown += new KeyEventHandler(gHook_KeyDown);
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                gHook.HookedKeys.Add(key);
            }
            gHook.hook();
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
                    else if (e.KeyCode == Keys.Left)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                chromeBrowser.Back();
                            }));

                        }).Start();
                    }
                    else if (e.KeyCode == Keys.Right)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                chromeBrowser.Forward();
                            }));

                        }).Start();
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

                panel_cefsharp.Visible = false;
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

                            panel_cefsharp.Visible = false;
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

                        panel_cefsharp.Visible = false;
                        pictureBox_loader.Visible = false;
                    }
                    else if (!last_index_hijacked_get)
                    {
                        panel_connection.Visible = false;
                        panel_connection.Enabled = false;
                        pictureBox_loader.Visible = false;

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
                        }
                        else
                        {
                            if (!connection_handler)
                            {
                                pictureBox_loader.Visible = true;
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
                }
                else
                {
                    chromeBrowser.Stop();
                    timer_handler.Stop();

                    panel_cefsharp.Visible = false;
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

                panel_cefsharp.Visible = false;
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
            catch (Exception err)
            {
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1002", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                close = false;
                Close();
            }
        }

        // Get Notifications
        private async Task GetNotificationAsync(string webservice_notifications)
        {
            notificationscount = 1;
            try
            {
                var client = new HttpClient();
                var requestContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("macid", GetMACAddress()),
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
                        using (var csv = new ChoCSVWriter(temp_file).WithFirstLineHeader())
                        {
                            using (var p = ChoJSONReader.LoadText(json).WithJSONPath("$..data"))
                            {
                                csv.Write(p.Select(i => new {
                                    Header_Test_Header = i.id + "*|*" + i.message_date + "*|*" + "• " + i.message_title + "*|*" + i.message_content + "*|*" + i.status + "*|*" + i.message_type + "*|*" + i.edited_id + "*|*U"
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
                                        StreamWriter sw = new StreamWriter(notifications_file, true, Encoding.UTF8);
                                        sw.WriteLine(line_sr);
                                        sw.Close();
                                    }
                                }
                            }

                            streamReader.Close();
                            
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
                                        StreamWriter sw = new StreamWriter(notifications_file, true, Encoding.UTF8);
                                        sw.WriteLine(line_sr);
                                        sw.Close();
                                    }
                                }
                            }

                            streamReader.Close();
                            
                            NotificationsAsync();
                        }
                    }
                    else
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
                }

                timer_notifications.Start();
            }
            catch (Exception err)
            {
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1002", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                close = false;
                Close();
            }
        }

        private async Task NotificationsAsync()
        {
            //update
            string notifications_file = Path.Combine(Path.GetTempPath(), "sb_notifications.txt");
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
                                                if (delete_id == obje.ToString())
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

            // delete
            await GetNotificationDeleteAsync(notifications_delete_service[current_web_service]);
            // end delete
            
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
                            _message_title = obj.ToString();
                        }
                        else if (count == 4)
                        {
                            _message_content = @obj.ToString();
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
                        label_title.Name = "label_title_notification_" + _message_id;
                        label_title.Text = Ellipsis(_message_title, 20);

                        if (_message_unread.Contains("U"))
                        {
                            label_notificationscount.Text = notificationscount++.ToString();
                        }

                        label_title.Location = new Point(3, 0);
                        label_title.AutoSize = true;
                        label_title.ForeColor = Color.FromArgb(72, 72, 72);
                        label_title.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);

                        Label label_content = new Label();
                        label_content.Name = "label_content_notification_" + _message_id;
                        label_content.Text = Ellipsis(_message_content, 130);
                        label_content.Location = new Point(4, 19);
                        label_content.AutoSize = true;
                        label_content.MaximumSize = new Size(248, 0);
                        label_content.ForeColor = Color.FromArgb(72, 72, 72);
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
                            label_date.Text = "just now";
                        }
                        else if (delta < 2 * MINUTE)
                        {
                            label_date.Text = "a minute ago";
                        }
                        else if (delta < 45 * MINUTE)
                        {
                            if (ts.Minutes == 1)
                            {
                                label_date.Text = ts.Minutes + " minute ago";
                            }
                            else
                            {
                                label_date.Text = ts.Minutes + " minutes ago";
                            }
                        }
                        else if (delta < 90 * MINUTE)
                        {
                            label_date.Text = "an hour ago";
                        }
                        else if (delta < 24 * HOUR)
                        {
                            if (ts.Hours == 1)
                            {
                                label_date.Text = ts.Hours + " hour ago";
                            }
                            else
                            {
                                label_date.Text = ts.Hours + " hours ago";
                            }
                        }
                        else if (delta < 48 * HOUR)
                        {
                            label_date.Text = "yesterday";
                        }
                        else if (delta < 30 * DAY)
                        {
                            if (ts.Days == 1)
                            {
                                label_date.Text = ts.Days + " day ago";
                            }
                            else
                            {
                                label_date.Text = ts.Days + " days ago";
                            }
                        }
                        else if (delta < 12 * MONTH)
                        {
                            int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                            label_date.Text = months <= 1 ? "one month ago" : months + " months ago";
                        }
                        else
                        {
                            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                            label_date.Text = years <= 1 ? "one year ago" : years + " years ago";
                        }

                        label_date.AutoSize = true;
                        label_date.Location = new Point(4, 61);
                        label_date.ForeColor = Color.FromArgb(168, 168, 168);
                        label_date.Font = new Font("Microsoft Sans Serif", 8);

                        Label label_view = new Label();
                        label_view.Name = "label_view_notification_" + _message_id;
                        label_view.Text = "View";

                        if (line_count_panel > 7)
                        {
                            label_view.Location = new Point(220, 61);
                            p.Size = new Size(250, 83);
                            label_content.MaximumSize = new Size(230, 0);
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

            sr.Close();

            Update();
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
                            _message_title = obj.ToString();
                        }
                        else if (count == 4)
                        {
                            _message_content = @obj.ToString();
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
                            label_notificationscount.Text = notificationscount++.ToString();
                        }

                        label_title.Location = new Point(3, 0);
                        label_title.AutoSize = true;
                        label_title.ForeColor = Color.FromArgb(72, 72, 72);
                        label_title.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);

                        Label label_content = new Label();
                        label_content.Name = "label_content_notification_" + _message_id;
                        label_content.Text = Ellipsis(_message_content, 130);
                        label_content.Location = new Point(4, 19);
                        label_content.AutoSize = true;
                        label_content.MaximumSize = new Size(248, 0);
                        label_content.ForeColor = Color.FromArgb(72, 72, 72);
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
                            label_date.Text = "just now";
                        }
                        else if (delta < 2 * MINUTE)
                        {
                            label_date.Text = "a minute ago";
                        }
                        else if (delta < 45 * MINUTE)
                        {
                            if (ts.Minutes == 1)
                            {
                                label_date.Text = ts.Minutes + " minute ago";
                            }
                            else
                            {
                                label_date.Text = ts.Minutes + " minutes ago";
                            }
                        }
                        else if (delta < 90 * MINUTE)
                        {
                            label_date.Text = "an hour ago";
                        }
                        else if (delta < 24 * HOUR)
                        {
                            if (ts.Hours == 1)
                            {
                                label_date.Text = ts.Hours + " hour ago";
                            }
                            else
                            {
                                label_date.Text = ts.Hours + " hours ago";
                            }
                        }
                        else if (delta < 48 * HOUR)
                        {
                            label_date.Text = "yesterday";
                        }
                        else if (delta < 30 * DAY)
                        {
                            if (ts.Days == 1)
                            {
                                label_date.Text = ts.Days + " day ago";
                            }
                            else
                            {
                                label_date.Text = ts.Days + " days ago";
                            }
                        }
                        else if (delta < 12 * MONTH)
                        {
                            int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                            label_date.Text = months <= 1 ? "one month ago" : months + " months ago";
                        }
                        else
                        {
                            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                            label_date.Text = years <= 1 ? "one year ago" : years + " years ago";
                        }

                        label_date.AutoSize = true;
                        label_date.Location = new Point(4, 61);
                        label_date.ForeColor = Color.FromArgb(168, 168, 168);
                        label_date.Font = new Font("Microsoft Sans Serif", 8);

                        Label label_view = new Label();
                        label_view.Name = "label_view_notification_" + _message_id;
                        label_view.Text = "GO";

                        if (line_count_panel > 7)
                        {
                            label_view.Location = new Point(220, 61);
                            p.Size = new Size(250, 83);
                            label_content.MaximumSize = new Size(230, 0);
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
            Process.Start(@"updater.exe");
        }

        private void click_event(object sender, EventArgs e)
        {
            try
            {
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
                                            _message_title_inner = obje.ToString();
                                        }
                                        else if (count_inner == 4)
                                        {
                                            _message_content_inner = obje.ToString();
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
                                                _message_date_inner = "just now";
                                            }
                                            else if (delta < 2 * MINUTE)
                                            {
                                                _message_date_inner = "a minute ago";
                                            }
                                            else if (delta < 45 * MINUTE)
                                            {
                                                if (ts.Minutes == 1)
                                                {
                                                    _message_date_inner = ts.Minutes + " minute ago";
                                                }
                                                else
                                                {
                                                    _message_date_inner = ts.Minutes + " minutes ago";
                                                }
                                            }
                                            else if (delta < 90 * MINUTE)
                                            {
                                                _message_date_inner = "an hour ago";
                                            }
                                            else if (delta < 24 * HOUR)
                                            {
                                                if (ts.Hours == 1)
                                                {
                                                    _message_date_inner = ts.Hours + " hour ago";
                                                }
                                                else
                                                {
                                                    _message_date_inner = ts.Hours + " hours ago";
                                                }
                                            }
                                            else if (delta < 48 * HOUR)
                                            {
                                                _message_date_inner = "yesterday";
                                            }
                                            else if (delta < 30 * DAY)
                                            {
                                                if (ts.Days == 1)
                                                {
                                                    _message_date_inner = ts.Days + " day ago";
                                                }
                                                else
                                                {
                                                    _message_date_inner = ts.Days + " days ago";
                                                }
                                            }
                                            else if (delta < 12 * MONTH)
                                            {
                                                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                                                _message_date_inner = months <= 1 ? "one month ago" : months + " months ago";
                                            }
                                            else
                                            {
                                                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                                                _message_date_inner = years <= 1 ? "one year ago" : years + " years ago";
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
                                }
                            }
                        }
                    }
                }

                MessageBox.Show(_message_content_inner + "\n\n" + ((Label)flowLayoutPanel_notifications.Controls.Find("label_date_notification_" + output, true)[0]).Text, _message_title_inner.Replace("•", ""), MessageBoxButtons.OK, MessageBoxIcon.Information);

                string final_replace_message_title_inner = string.Empty;
                if (_message_title_inner.Contains("•"))
                {
                    string replace_message_title_inner = _message_title_inner.Replace("•", "");
                    final_replace_message_title_inner = "" + replace_message_title_inner.Remove(0, 1);
                    ((Label)flowLayoutPanel_notifications.Controls.Find("label_title_notification_" + output, true)[0]).Text = final_replace_message_title_inner;
                }

                sr.Close();
                File.WriteAllText(notifications_file, text);

                if (_message_title_inner.Contains("•"))
                {
                    string text_get = File.ReadAllText(notifications_file);

                    string final_replace_message_title_inner_text_file = string.Empty;
                    if (_message_title_inner.Contains("•"))
                    {
                        string replace_message_title_inner_text_file = _message_title_inner.Replace("•", "");
                        final_replace_message_title_inner_text_file = "" + replace_message_title_inner_text_file.Remove(0, 1);
                        text_get = text.Replace(_message_title_inner, final_replace_message_title_inner_text_file);
                        File.WriteAllText(notifications_file, text_get);
                    }

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
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        // Delete Notifications
        private async Task GetNotificationDeleteAsync(string webservice_notifications_delete)
        {
            try
            {
                string notifications_file = Path.Combine(Path.GetTempPath(), "sb_notifications.txt");
                var client = new HttpClient();
                var requestContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("macid", GetMACAddress()),
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

                    StringBuilder sb = new StringBuilder();
                    using (var p = ChoJSONReader.LoadText(json).WithJSONPath("$..data"))
                    {
                        using (var w = new ChoCSVWriter(sb))
                            w.Write(p);
                    }

                    string id_to_delete = sb.ToString();
                    string get_line_delete = string.Empty;
                    if (id_to_delete != "0")
                    {
                        // delete
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
                                        if (id_to_delete == obje.ToString())
                                        {
                                            get_line_delete = line_delete;
                                        }
                                    }
                                }
                            }
                        }

                        sr_delete.Close();

                        if (!String.IsNullOrEmpty(get_line_delete))
                        {
                            string text = File.ReadAllText(notifications_file);
                            text = text.Replace(get_line_delete, "");
                            File.WriteAllText(notifications_file, text);

                            if (label_notificationscount.Text == "1")
                            {
                                label_notificationscount.Text = "";
                            }
                        }
                    }
                }

                // version
                string path_version = Path.Combine(Path.GetTempPath(), "sb_version.txt");
                int line_count = 0;
                List<string> line_to_delete = new List<string>();
                if (File.Exists(path_version))
                {
                    string version = File.ReadAllText(path_version);
                    string message_type = string.Empty;
                    if (version != toolStripMenuItem_version.Text)
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

                    for (int i = 0; i < line_count; i++)
                    {
                        string text = File.ReadAllText(notifications_file);
                        text = text.Replace(line_to_delete[i], "");
                        File.WriteAllText(notifications_file, text);
                    }
                }
                else
                {
                    StreamWriter sw = new StreamWriter(path_version);
                    sw.Write(toolStripMenuItem_version.Text);
                    sw.Close();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1008", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                close = false;
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

                        panel_cefsharp.Visible = false;

                        pictureBox_loader.Visible = true;

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

        // Timeout timeasd
        private void Timer_handler_Tick(object sender, EventArgs e)
        {
            chromeBrowser.Stop();
            timer_handler.Stop();
        }

        // Initialize Chromium
        private void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);
            chromeBrowser = new ChromiumWebBrowser();
            chromeBrowser.MenuHandler = new CustomMenuHandler();
            chromeBrowser.LifeSpanHandler = new BrowserLifeSpanHandler();
            panel_cefsharp.Controls.Add(chromeBrowser);

            chromeBrowser.LoadingStateChanged += BrowserLoadingStateChanged;
            chromeBrowser.TitleChanged += BrowserTitleChanged;
        }

        // asd123
        private void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                pictureBox_forward.Enabled = e.CanGoForward;
                forwardToolStripMenuItem.Enabled = e.CanGoForward;
                pictureBox_back.Enabled = e.CanGoBack;
                goBackToolStripMenuItem.Enabled = e.CanGoBack;

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

                    if (!domain_one_time)
                    {
                        pictureBox_loader.Visible = true;
                        panel_cefsharp.Visible = false;

                        pictureBox_browserstop.Visible = true;
                        pictureBox_reload.Visible = false;
                    }
                }));
            }

            if (!e.IsLoading)
            {
                Invoke(new Action(() =>
                {
                    if (!domain_one_time)
                    {
                        pictureBox_loader.Visible = false;

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
                    }
                }));
            }

            if (domain_one_time)
            {
                if (!e.IsLoading)
                {
                    Invoke(new Action(async () =>
                    {
                        chromeBrowser.Dock = DockStyle.Fill;

                        await Task.Run(async () =>
                        {
                            await Task.Delay(2000);
                        });

                        fully_loaded++;

                        if (fully_loaded == 1)
                        {
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
                                var html = "";

                                await chromeBrowser.GetSourceAsync().ContinueWith(taskHtml =>
                                {
                                    html = taskHtml.Result;
                                });

                                if (html.Contains("landing_image"))
                                {
                                    await Task.Run(async () =>
                                    {
                                        await Task.Delay(2000);
                                    });

                                    back_button_i++;
                                    timer_detectifhijacked.Start();
                                    domain_one_time = false;
                                    last_index_hijacked_get = false;
                                    pictureBox_loader.Visible = false;
                                    not_hijacked = true;

                                    pictureBox_reload.Enabled = true;
                                    pictureBox_reload.Image = Properties.Resources.refresh;
                                    pictureBox_browserhome.Enabled = true;
                                    pictureBox_browserhomehover.Enabled = true;
                                    pictureBox_browserhome.Image = Properties.Resources.browser_home;
                                    pictureBox_nofication.Enabled = true;
                                    pictureBox_noficationhover.Enabled = true;
                                    pictureBox_nofication.Image = Properties.Resources.notification;
                                    label_notificationscount.Visible = true;
                                    homeToolStripMenuItem.Enabled = true;
                                    reloadToolStripMenuItem.Enabled = true;
                                    cleanAndReloadToolStripMenuItem.Enabled = true;
                                    resetZoomToolStripMenuItem.Enabled = true;
                                    zoomInToolStripMenuItem.Enabled = true;
                                    zoomOutToolStripMenuItem.Enabled = true;
                                    label_clearcache.Enabled = true;
                                    label_getdiagnostics.Enabled = true;
                                    elseload_return = false;
                                }
                                else
                                {
                                    back_button_i++;
                                    last_index_hijacked_get = true;
                                    not_hijacked = false;
                                    label_loadingstate.Text = "1";
                                    label_loadingstate.Text = "0";
                                    elseload_return = true;
                                }
                            }
                            else
                            {
                                await Task.Run(async () =>
                                {
                                    await Task.Delay(2000);
                                });

                                back_button_i++;
                                timer_detectifhijacked.Start();
                                domain_one_time = false;
                                last_index_hijacked_get = false;
                                pictureBox_loader.Visible = false;

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
                                label_notificationscount.Visible = true;
                                homeToolStripMenuItem.Enabled = true;
                                reloadToolStripMenuItem.Enabled = true;
                                cleanAndReloadToolStripMenuItem.Enabled = true;
                                resetZoomToolStripMenuItem.Enabled = true;
                                zoomInToolStripMenuItem.Enabled = true;
                                zoomOutToolStripMenuItem.Enabled = true;
                                label_clearcache.Enabled = true;
                                label_getdiagnostics.Enabled = true;
                                elseload_return = false;
                            }
                        }
                        else
                        {
                            Invoke(new Action(() =>
                            {
                                if (elseload_return)
                                {
                                    timer_elseloaded.Start();
                                }
                            }));
                        }
                    }));
                }
            }
        }

        private void BrowserTitleChanged(object sender, TitleChangedEventArgs e)
        {
            handler_title = e.Title;
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
                    Cef.Shutdown();
                }
            }
            else
            {
                Cef.Shutdown();
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
                MessageBox.Show("There is a problem! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1004", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("There is a problem! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1005", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            _city = locationDetails.city.Replace("'", "''");
                            _province = locationDetails.regionName.Replace("'", "''");
                            _country = locationDetails.country.Replace("'", "''");
                            InsertDevice(send_service[current_web_service], _external_ip, _mac_address, _city, _province, _country);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("There is a problem! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1006", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertDevice(string domain, string ip, string mac_address, string city, string province, string country)
        {
            try
            {
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
            catch (Exception err)
            {
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1003", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                close = false;
                Close();
            }
        }

        private void InsertDiagnostics(string domain, string zipfile)
        {
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

                DialogResult dr = MessageBox.Show("Are you sure you want to exit the program?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    Cef.Shutdown();
                    Environment.Exit(0);
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
                Cef.Shutdown();
                Environment.Exit(0);
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

        private void timer_detectifhijacked_Tick(object sender, EventArgs e)
        {
            webBrowser_handler.Navigate(domain_get);
            timer_detectifhijacked.Stop();
        }

        // Web Browser Loaded
        private async void WebBrowser_handler_DocumentCompletedAsync(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser_handler.ReadyState == WebBrowserReadyState.Complete)
            {
                if (e.Url == webBrowser_handler.Url)
                {
                    // handlers
                    webbrowser_handler_title = webBrowser_handler.DocumentTitle;
                    webbrowser_handler_url = webBrowser_handler.Url;

                    timer_detectifhijacked.Start();

                    timeout = false;
                    timer_handler.Stop();

                    string strValue = text_search;
                    string[] strArray = strValue.Split(',');
                    foreach (string obj in strArray)
                    {
                        bool contains = webbrowser_handler_title.Contains(obj.Replace(" ", ""));
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
                            pictureBox_loader.BringToFront();
                            pictureBox_loader.Visible = true;
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
                            Form_YB_NewTab.Main_SetClose = true;
                            timer_detectifhijacked.Stop();
                            label_clearcache.Enabled = false;
                            label_getdiagnostics.Enabled = false;
                        }
                    }
                }
            }
        }

        private void timer_elseloaded_Tick(object sender, EventArgs e)
        {
            Invoke(new Action(async () =>
            {
                elseloaded_i++;

                if (elseloaded_i++ == 1)
                {
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
                        var html = "";

                        await chromeBrowser.GetSourceAsync().ContinueWith(taskHtml =>
                        {
                            html = taskHtml.Result;
                        });

                        if (html.Contains("landing_image"))
                        {
                            await Task.Run(async () =>
                            {
                                await Task.Delay(2000);
                            });

                            back_button_i++;
                            timer_detectifhijacked.Start();
                            domain_one_time = false;
                            last_index_hijacked_get = false;
                            pictureBox_loader.Visible = false;
                            not_hijacked = true;

                            pictureBox_reload.Enabled = true;
                            pictureBox_reload.Image = Properties.Resources.refresh;
                            pictureBox_browserhome.Enabled = true;
                            pictureBox_browserhomehover.Enabled = true;
                            pictureBox_browserhome.Image = Properties.Resources.browser_home;
                            pictureBox_nofication.Enabled = true;
                            pictureBox_noficationhover.Enabled = true;
                            pictureBox_nofication.Image = Properties.Resources.notification;
                            label_notificationscount.Visible = true;
                            homeToolStripMenuItem.Enabled = true;
                            reloadToolStripMenuItem.Enabled = true;
                            cleanAndReloadToolStripMenuItem.Enabled = true;
                            resetZoomToolStripMenuItem.Enabled = true;
                            zoomInToolStripMenuItem.Enabled = true;
                            zoomOutToolStripMenuItem.Enabled = true;
                            label_clearcache.Enabled = true;
                            label_getdiagnostics.Enabled = true;
                            elseload_return = false;
                        }
                        else
                        {
                            back_button_i++;
                            last_index_hijacked_get = true;
                            not_hijacked = false;
                            label_loadingstate.Text = "1";
                            label_loadingstate.Text = "0";
                            elseload_return = true;
                        }
                    }
                    else
                    {
                        await Task.Run(async () =>
                        {
                            await Task.Delay(2000);
                        });

                        back_button_i++;
                        timer_detectifhijacked.Start();
                        domain_one_time = false;
                        last_index_hijacked_get = false;
                        pictureBox_loader.Visible = false;

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
                        label_notificationscount.Visible = true;
                        homeToolStripMenuItem.Enabled = true;
                        reloadToolStripMenuItem.Enabled = true;
                        cleanAndReloadToolStripMenuItem.Enabled = true;
                        resetZoomToolStripMenuItem.Enabled = true;
                        zoomInToolStripMenuItem.Enabled = true;
                        zoomOutToolStripMenuItem.Enabled = true;
                        label_clearcache.Enabled = true;
                        label_getdiagnostics.Enabled = true;
                        elseload_return = false;
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
            chromeBrowser.Load(domain_get);
        }

        private void pictureBox_browserhomehover_Click(object sender, EventArgs e)
        {
            chromeBrowser.Load(domain_get);
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                }
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
            this.RecreateRegion();
        }

        private void pictureBox_helpback_Click(object sender, EventArgs e)
        {
            panel_help.Visible = false;
            help_click = true;
            panel_cefsharp.Visible = true;

            pictureBox_help.BackColor = Color.FromArgb(235, 99, 6);
            pictureBox_helphover.BackColor = Color.FromArgb(235, 99, 6);
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

                    while (panel_cefsharp.Width > panel_cefsharp_resize)
                    {
                        if (!notification_click)
                        {
                            Application.DoEvents();
                            panel_cefsharp.Width -= 4;
                        }
                        else
                        {
                            break;
                        }
                    }

                    panel_notification.Visible = true;
                    label_separator.Visible = true;
                    pictureBox_nofication.Image = Properties.Resources.notification_back;
                }
                else
                {
                    notification_click = true;

                    while (panel_cefsharp_size > panel_cefsharp.Width)
                    {
                        if (notification_click)
                        {
                            Application.DoEvents();
                            panel_cefsharp.Width += 4;
                        }
                        else
                        {
                            break;
                        }
                    }

                    panel_notification.Visible = false;
                    label_separator.Visible = false;
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

                    while (panel_cefsharp.Width > panel_cefsharp_resize)
                    {
                        if (!notification_click)
                        {
                            Application.DoEvents();
                            panel_cefsharp.Width -= 4;
                        }
                        else
                        {
                            break;
                        }
                    }

                    panel_notification.Visible = true;
                    label_separator.Visible = true;
                    pictureBox_nofication.Image = Properties.Resources.notification_back;
                }
                else
                {
                    notification_click = true;

                    while (panel_cefsharp_size > panel_cefsharp.Width)
                    {
                        if (notification_click)
                        {
                            Application.DoEvents();
                            panel_cefsharp.Width += 4;
                        }
                        else
                        {
                            break;
                        }
                    }

                    panel_notification.Visible = false;
                    label_separator.Visible = false;
                    pictureBox_nofication.Image = Properties.Resources.notification;
                }
            }
        }

        private void label_clearcache_Click(object sender, EventArgs e)
        {
            chromeBrowser.Reload(false);
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
            GetTraceRoute(domain_get);
            GetPing(domain_get);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Title = "Save As";
            saveFileDialog.FileName = "Diagnostics";
            saveFileDialog.DefaultExt = "zip";
            saveFileDialog.Filter = "ZIP Files|*.zip";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFile(Path.GetTempPath() + "\\traceroute.txt", "");
                    zip.AddFile(Path.GetTempPath() + "\\ping.txt", "");
                    zip.Save(saveFileDialog.FileName);
                }

                using (var stream = File.Open(saveFileDialog.FileName, FileMode.Open))
                {
                    try
                    {
                        using (var wb = new WebClient())
                        {
                            var files = new[]
                            {
                                new UploadFile
                                {
                                    Name = "zipfile",
                                    Filename = Path.GetFileName(saveFileDialog.FileName),
                                    ContentType = "application/zip",
                                    Stream = stream
                                }
                            };

                            var data = new NameValueCollection
                            {
                                ["api_key"] = API_KEY,
                                ["brand_code"] = BRAND_CODE,
                                ["macid"] = _mac_address
                            };

                            byte[] result = UploadFiles(diagnostics_service[current_web_service], files, data);
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1003", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        close = false;
                        Close();
                    }

                }

                //InsertDiagnostics(diagnostics_service[current_web_service], saveFileDialog.FileName);
            }

        }

        private byte[] UploadFiles(string address, IEnumerable<UploadFile> files, NameValueCollection values)
        {
            var request = WebRequest.Create(address);
            request.Method = "POST";
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                // Write the values
                foreach (string name in values.Keys)
                {
                    var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                // Write the files
                foreach (var file in files)
                {
                    var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.Filename, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", file.ContentType, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    //file.Stream.CopyTo(requestStream);
                    buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            }

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var stream = new MemoryStream())
            {
                responseStream.CopyTo(stream);
                return stream.ToArray();
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

        private void timer_notifications_Tick(object sender, EventArgs e)
        {
            timer_notifications.Stop();
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            GetNotificationAsync(notifications_service[current_web_service]);
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void pictureBox_maximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
            }
        }

        private void pictureBox_close_Click(object sender, EventArgs e)
        {
            if (close)
            {
                IsCloseVisible = true;
                pictureBox_close.BackColor = Color.FromArgb(197, 112, 53);

                DialogResult dr = MessageBox.Show("Are you sure you want to exit the program?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    Cef.Shutdown();
                    Environment.Exit(0);
                }
                else
                {
                    IsCloseVisible = false;
                    pictureBox_close.BackColor = Color.FromArgb(235, 99, 6);
                }
            }
            else
            {
                Cef.Shutdown();
                Environment.Exit(0);
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
                    WindowState = FormWindowState.Maximized;
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
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            var pingProc = new Process { StartInfo = startInfo };
            pingProc.Start();

            while (!pingProc.HasExited)
            {
                label_getdiagnostics.Text = "GETTING READY...";
                pingProc.WaitForExit();
            }

            label_getdiagnostics.Text = "GET DIAGNOSTICS";
            Cursor.Current = Cursors.Default;

            result_ping = pingProc.StandardOutput.ReadToEnd();

            string temp_file = Path.Combine(Path.GetTempPath(), "ping.txt");

            if (File.Exists(temp_file))
            {
                File.Delete(temp_file);

                using (StreamWriter sw = new StreamWriter(temp_file))
                {
                    sw.WriteLine(result_ping);
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(temp_file))
                {
                    sw.WriteLine(result_ping);
                }
            }
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
                RedirectStandardOutput = true,
            };

            var pingProc = new Process { StartInfo = startInfo };
            pingProc.Start();

            while (!pingProc.HasExited)
            {
                label_getdiagnostics.Text = "PLEASE WAIT...";
                Cursor.Current = Cursors.WaitCursor;
                pingProc.WaitForExit();
            }

            result_traceroute = pingProc.StandardOutput.ReadToEnd();

            string temp_file = Path.Combine(Path.GetTempPath(), "traceroute.txt");

            if (File.Exists(temp_file))
            {
                File.Delete(temp_file);

                using (StreamWriter sw = new StreamWriter(temp_file))
                {
                    sw.WriteLine(result_traceroute);
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(temp_file))
                {
                    sw.WriteLine(result_traceroute);
                }
            }
        }
    }
}

