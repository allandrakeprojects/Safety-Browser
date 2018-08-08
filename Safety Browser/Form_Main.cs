using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Safety_Browser
{
    public partial class Form_Main : Form
    {
        private ChromiumWebBrowser chromeBrowser;

        public Form_Main()
        {
            InitializeComponent();
            InitializeChromium();
        }

        private void InitializeChromium()
        {
            try
            {
                CefSettings settings = new CefSettings();
                Cef.Initialize(settings);

                chromeBrowser = new ChromiumWebBrowser("google.com");
                chromeBrowser.MenuHandler = new CustomMenuHandler();

                panel_browser.Controls.Add(chromeBrowser);

                chromeBrowser.Dock = DockStyle.Fill;

                //chromeBrowser.LoadingStateChanged += ChromiumWebBrowser_LoadingStateChangedAsync;
                //chromeBrowser.AddressChanged += ChromiumWebBrowser_AddressChanged;
                //chromeBrowser.LoadError += ChromiumWebBrowser_BrowserLoadError;
                //chromeBrowser.TitleChanged += ChromiumWebBrowser_TitleChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is a problem with the server! Please contact IT support. \n\nError Message: " + ex.Message + "\nError Code: rc1000", "rainCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
