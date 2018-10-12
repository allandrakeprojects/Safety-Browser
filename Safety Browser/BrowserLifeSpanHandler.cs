using CefSharp;
using System.Diagnostics;
using System.Windows.Forms;

namespace Safety_Browser
{
    public class BrowserLifeSpanHandler : ILifeSpanHandler
    {
        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName,
            WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo,
            IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            if (browserControl.CanExecuteJavascriptInMainFrame)
            {
                //Form_YB_NewTab form_newtab = new Form_YB_NewTab(targetUrl, "normal");
                //int open_form = Application.OpenForms.Count;

                //if (open_form == 1)
                //{
                //    form_newtab.Show();
                //}
                //else
                //{
                //    Form_YB_NewTab.SetClose = true;
                //    form_newtab.Show();
                //}

                if (targetUrl.Contains("ambassador"))
                {
                    Process.Start(targetUrl);
                }
                else if (targetUrl.Contains("about:blank"))
                {
                    browserControl.Load(targetUrl);
                }
                else
                {
                    MessageBox.Show(targetUrl);
                    Form_YB_NewTab form_newtab = new Form_YB_NewTab(targetUrl, "normal");
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

            newBrowser = null;
            return true;
        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            //nothing
        }

        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
            //nothing
        }
    }
}