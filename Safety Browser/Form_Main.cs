using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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
        private string web_title = "";
        private int loaded_detect;
        private int i_timeout;
        private bool isHijacked;
        private bool loadOneMoreTime = false;
        private int current_domain_index;
        private int total_domain_index;
        private int current_web_service = 0;
        private int current_domain = 0;
        private int detectnotloading;

        public Form_Main()
        {
            InitializeComponent();
        }

        // Form Load
        private void Form_Main_Load(object sender, EventArgs e)
        {
            InitializeChromium();
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            GetTextToTextAsync(web_service[current_web_service]);
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            pictureBox_loader.Left = (ClientSize.Width - pictureBox_loader.Width) / 2;
            pictureBox_loader.Top = (ClientSize.Height - pictureBox_loader.Height) / 2;
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
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1000", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Get Website Title
        private void ChromiumWebBrowser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            web_title = e.Title;

            if (!loadOneMoreTime)
            {
                if (!String.IsNullOrEmpty(web_title))
                {
                    chromeBrowser.Stop();
                }
            }
        }

        // Loading State
        private void ChromiumWebBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            Invoke(new Action(async () =>
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
                            }
                            else
                            {
                                pictureBox_loader.Visible = false;
                                pictureBox_loader.Enabled = false;
                                
                                loadOneMoreTime = true;
                                dataGridView_domain.ClearSelection();
                                dataGridView_domain.Rows[current_domain_index].Selected = true;
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

            if (i_timeout == 60)
            {
                chromeBrowser.Stop();
                // next
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
                        //MessageBox.Show("done all!");
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
                            //MessageBox.Show("done all!");
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1000", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                close = false;
                Close();
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
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + err.Message + "\nError Code: 1000", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string domain;

                    try
                    {
                        loaded_detect = 0;
                        domain = row.Cells[0].Value.ToString();
                        chromeBrowser.Load(domain);
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
                        //MessageBox.Show("done all!");
                    }
                }
                else
                {
                    dataGridView_domain.FirstDisplayedScrollingRowIndex = current_domain_index;
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

            if (detectnotloading > 20)
            {
                dataGridView_domain.ClearSelection();
                i_timeout = 1;
                timer_timeout.Start();
                dataGridView_domain.Rows[current_domain_index].Selected = true;
                detectnotloading = 0;
                timer_detectnotloading.Stop();
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
