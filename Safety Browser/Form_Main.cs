using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Safety_Browser
{
    public partial class Form_Main : Form
    {
        private ChromiumWebBrowser chromeBrowser;
        private string[] web_service = { "http://raincheck.ssitex.com/testapi/getTxt2Search.php", "http://raincheck.ssitex.com/testapi/getTxt2Search2.php", "http://raincheck.ssitex.com/testapi/getTxt2Search.php" };
        private string[] domain_test = { "http://raincheck.ssitex.com/testapi/getDomains.php", "http://raincheck.ssitex.com/testapi/getDomains123.php", "http://raincheck.ssitex.com/testapi/getDomains2.php" };
        private string text_search;
        private bool close = true;
        private string web_title;
        private int loaded_detect;
        private int i_timeout;
        private bool isHijacked;
        private bool loadOneMoreTime = false;
        private int current_domain_index = 0;
        private int total_domain_index;
        private int current_web_service = 0;
        private int current_domain = 0;
        private int detectnotloading;
        private string domain_get;
        private bool last_index_hijacked = false;
        private bool loadingstate = true;
        private bool last_index_hijacked_get = false;
        private bool networkIsAvailable;

        public Form_Main()
        {
            InitializeComponent();
        }

        // Form Load
        private void Form_Main_Load(object sender, EventArgs e)
        {
            NetworkAvailability();
            InitializeChromium();
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            GetTextToTextAsync(web_service[current_web_service]);
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            PictureBoxCenter();
        }

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

            if (!networkIsAvailable)
            {
                i_timeout = 1;
                timer_timeout.Stop();
                detectnotloading = 0;
                timer_detectnotloading.Stop();

                panel_browser.Visible = false;
                panel_browser.Enabled = false;
                pictureBox_loader.Visible = false;
                pictureBox_loader.Enabled = false;

                panel_connection.Visible = true;
                panel_connection.Enabled = true;
            }

            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            Invoke(new Action(() =>
            {
                networkIsAvailable = e.IsAvailable;

                if (networkIsAvailable)
                {
                    if (dataGridView_domain.RowCount == 0)
                    {
                        #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        GetTextToTextAsync(web_service[current_web_service]);
                        #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                        panel_connection.Visible = false;
                        panel_connection.Enabled = false;

                        panel_browser.Visible = true;
                        panel_browser.Enabled = true;
                        pictureBox_loader.Visible = true;
                        pictureBox_loader.Enabled = true;
                    }
                    else
                    {
                        panel_connection.Visible = false;
                        panel_connection.Enabled = false;

                        panel_browser.Visible = true;
                        panel_browser.Enabled = true;
                        pictureBox_loader.Visible = true;
                        pictureBox_loader.Enabled = true;

                        dataGridView_domain.ClearSelection();
                        dataGridView_domain.Rows[current_domain_index].Selected = true;
                    }
                }
                else
                {
                    i_timeout = 1;
                    timer_timeout.Stop();
                    detectnotloading = 0;
                    timer_detectnotloading.Stop();

                    panel_browser.Visible = false;
                    panel_browser.Enabled = false;
                    pictureBox_loader.Visible = false;
                    pictureBox_loader.Enabled = false;

                    panel_connection.Visible = true;
                    panel_connection.Enabled = true;
                }
            }));
        }

        private void PictureBoxCenter()
        {
            pictureBox_loader.Left = (ClientSize.Width - pictureBox_loader.Width) / 2;
            pictureBox_loader.Top = (ClientSize.Height - pictureBox_loader.Height) / 2;

            panel_connection.Left = (ClientSize.Width - panel_connection.Width) / 2;
            panel_connection.Top = (ClientSize.Height - panel_connection.Height) / 2;
        }

        // Initialize Chromium
        private void InitializeChromium()
        {
            try
            {
                CefSettings settings = new CefSettings();
                Cef.Initialize(settings);
                chromeBrowser = new ChromiumWebBrowser("");
                chromeBrowser.MenuHandler = new CustomMenuHandler();
                panel_browser.Controls.Add(chromeBrowser);
                chromeBrowser.Dock = DockStyle.Fill;

                // Functions
                chromeBrowser.TitleChanged += ChromiumWebBrowser_TitleChanged;
                chromeBrowser.LoadingStateChanged += ChromiumWebBrowser_LoadingStateChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: 1000", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Get Website Title
        private void ChromiumWebBrowser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            web_title = e.Title;
        }

        // Loading State
        private void ChromiumWebBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            Invoke(new Action(async () =>
            {
                if (loadingstate)
                {
                    if (loadOneMoreTime)
                    {
                        SetVisibleBrowser(true);

                        if (e.IsLoading)
                        {
                            i_timeout = 1;
                            timer_timeout.Start();
                            detectnotloading = 0;
                            timer_detectnotloading.Stop();
                            label_loadingstate.Text = "1";
                        }
                        else
                        {
                            await Task.Run(async () =>
                            {
                                await Task.Delay(2000);
                            });

                            loaded_detect++;

                            if (loaded_detect == 1)
                            {
                                MessageBox.Show("loaded 1");
                                i_timeout = 1;
                                timer_timeout.Stop();
                                detectnotloading = 0;
                                timer_detectnotloading.Start();
                                label_loadingstate.Text = "0";
                                loadOneMoreTime = false;
                            }
                        }
                    }
                    else
                    {
                        SetVisibleBrowser(false);

                        if (e.IsLoading)
                        {
                            i_timeout = 1;
                            timer_timeout.Start();
                            detectnotloading = 0;
                            timer_detectnotloading.Stop();
                            pictureBox_loader.Visible = true;
                            pictureBox_loader.Enabled = true;
                            label_loadingstate.Text = "1";
                        }
                        else
                        {
                            await Task.Run(async () =>
                            {
                                await Task.Delay(2000);
                            });

                            loaded_detect++;

                            if (loaded_detect == 1)
                            {
                                MessageBox.Show("loaded");
                                i_timeout = 1;
                                timer_timeout.Stop();
                                detectnotloading = 0;
                                timer_detectnotloading.Start();

                                string strValue = text_search;
                                string[] strArray = strValue.Split(',');

                                foreach (string obj in strArray)
                                {
                                    bool contains = web_title.Contains(obj);

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
                                    label_loadingstate.Text = "0";
                                    last_index_hijacked_get = true;
                                }
                                else
                                {
                                    last_index_hijacked_get = false;

                                    if (string.IsNullOrEmpty(label_get.Text))
                                    {
                                        label_get.Text = domain_get;
                                    }

                                    pictureBox_loader.Visible = false;
                                    pictureBox_loader.Enabled = false;

                                    loadOneMoreTime = true;

                                    dataGridView_domain.ClearSelection();
                                    dataGridView_domain.Rows[current_domain_index].Selected = true;
                                }
                            }
                        }
                    }
                }
            }));
        }

        // Timeout
        private void timer_timeout_Tick(object sender, EventArgs e)
        {
            label_timeout_count.Text = i_timeout++.ToString();

            if (i_timeout > 30)
            {
                timer_timeout.Stop();
                chromeBrowser.Stop();
            }
        }

        // Visibility of Browser
        private void SetVisibleBrowser(bool browser)
        {
            Invoke(new Action(() =>
            {
                if (browser)
                {
                    panel_browser.Controls.Add(chromeBrowser);
                    chromeBrowser.Dock = DockStyle.Fill;
                }
                else
                {
                    panel_browser.Controls.Remove(chromeBrowser);
                    chromeBrowser.Dock = DockStyle.None;
                }
            }));
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
                    new KeyValuePair<string, string>("test", "test"),
                });

                    HttpResponseMessage response = await client.PostAsync(
                        web_service_test,
                        requestContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        text_search = "";
                        close = true;
                        web_title = "";
                        loaded_detect = 0;
                        i_timeout = 0;
                        loadOneMoreTime = false;
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
                            detectnotloading = 0;
                            timer_detectnotloading.Stop();

                            if (last_index_hijacked_get)
                            {
                                SetVisibleBrowser(true);
                                pictureBox_loader.Visible = false;
                                pictureBox_loader.Enabled = false;
                                loadingstate = false;
                                chromeBrowser.Load(label_get.Text);
                            }

                            //MessageBox.Show("done all! 3");
                        }

                        return;
                    }

                    HttpContent responseContent = response.Content;

                    using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                    {
                        JArray jArray = JArray.Parse(await reader.ReadToEndAsync());
                        int count = jArray.Count;
                        int minus_count = count - 1;
                        int i = 0;

                        while (i < count)
                        {
                            string get = jArray[i]["text_search"].Value<string>();
                            if (i == minus_count)
                            {
                                text_search += get;
                            }
                            else
                            {
                                text_search += get + ",";
                            }

                            i++;
                        }

                        if (!String.IsNullOrEmpty(text_search))
                        {
                            // Get Domains
                            await GetDomainsAsync(domain_test[current_domain]);
                            total_domain_index = dataGridView_domain.RowCount;
                            dataGridView_domain.ClearSelection();
                            dataGridView_domain.Rows[current_domain_index].Selected = true;
                        }
                        else
                        {
                            text_search = "";
                            close = true;
                            web_title = "";
                            loaded_detect = 0;
                            i_timeout = 0;
                            loadOneMoreTime = false;
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
                                detectnotloading = 0;
                                timer_detectnotloading.Stop();

                                if (last_index_hijacked_get)
                                {
                                    SetVisibleBrowser(true);
                                    pictureBox_loader.Visible = false;
                                    pictureBox_loader.Enabled = false;
                                    loadingstate = false;
                                    chromeBrowser.Load(label_get.Text);
                                }

                                //MessageBox.Show("done all! 1");
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
                i_timeout = 1;
                timer_timeout.Stop();
                detectnotloading = 0;
                timer_detectnotloading.Stop();

                panel_browser.Visible = false;
                panel_browser.Enabled = false;
                pictureBox_loader.Visible = false;
                pictureBox_loader.Enabled = false;

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
                    new KeyValuePair<string, string>("test", "test"),
                });

                HttpResponseMessage response = await client.PostAsync(
                    test_domain,
                    requestContent);

                HttpContent responseContent = response.Content;

                using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                {
                    JArray jArray = JArray.Parse(await reader.ReadToEndAsync());
                    int count = jArray.Count;
                    int i = 0;

                    while (i < count)
                    {
                        string get = jArray[i]["domain_name"].Value<string>();
                        dataGridView_domain.ClearSelection();
                        dataGridView_domain.Rows.Add(get);
                        i++;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1002", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        
        // Domain Changed
        private void dataGridView_domain_SelectionChanged(object sender, EventArgs e)
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
                        loaded_detect = 0;
                        domain_get = row.Cells[0].Value.ToString();
                        chromeBrowser.Load(domain_get);
                    }
                    catch (Exception)
                    {
                        // Leave blank
                    }
                }
            }
        }
        
        // Loading State Changed
        private void label_loadingstate_TextChanged(object sender, EventArgs e)
        {
            if (label_loadingstate.Text == "0")
            {
                current_domain_index = dataGridView_domain.SelectedRows[0].Index + 1;
                
                if (current_domain_index == total_domain_index)
                {
                    if (last_index_hijacked_get)
                    {
                        last_index_hijacked = true;
                    }

                    text_search = "";
                    close = true;
                    web_title = "";
                    loaded_detect = 0;
                    i_timeout = 0;
                    loadOneMoreTime = false;
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
                        detectnotloading = 0;
                        timer_detectnotloading.Stop();

                        if (last_index_hijacked_get)
                        {
                            SetVisibleBrowser(true);
                            pictureBox_loader.Visible = false;
                            pictureBox_loader.Enabled = false;
                            loadingstate = false;
                            chromeBrowser.Load(label_get.Text);
                        }

                        //MessageBox.Show("done all! 2");
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
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (close)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to exit the program?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    close = false;
                    Cef.Shutdown();
                    Close();
                }
            }
            else
            {
                Cef.Shutdown();
            }
        }

        // Detect Not Loading
        private void timer_detectnotloading_Tick(object sender, EventArgs e)
        {
            label_detectnotloading.Text = detectnotloading++.ToString();

            if (detectnotloading > 10)
            {
                if (current_domain_index != total_domain_index)
                {
                    i_timeout = 1;
                    timer_timeout.Start();
                    dataGridView_domain.ClearSelection();
                    dataGridView_domain.Rows[current_domain_index].Selected = true;
                    detectnotloading = 0;
                    timer_detectnotloading.Stop();
                }
                else
                {
                    detectnotloading = 0;
                    timer_detectnotloading.Stop();

                    pictureBox_loader.Visible = false;
                    pictureBox_loader.Enabled = false;
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
    }
}
