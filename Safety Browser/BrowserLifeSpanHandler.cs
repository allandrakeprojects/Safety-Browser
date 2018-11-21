using CefSharp;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Safety_Browser
{
    public class BrowserLifeSpanHandler : ILifeSpanHandler
    {
        public event Action<string> PopupRequest;
        
        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            if (browserControl.CanExecuteJavascriptInMainFrame)
            {
                PopupRequest?.Invoke(targetUrl);

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

                // updated
                //MessageBox.Show(targetUrl);

                if (targetUrl.Contains("ambassador"))
                {
                    Process.Start(targetUrl);
                }
                // comment
                else if (targetUrl.Contains("about:blank"))
                {
                    browserControl.Load(targetUrl);
                    //const string script = @"document.getElementById('depositAmount3Party').value;";
                    //browserControl.EvaluateScriptAsync(script).ContinueWith(x =>
                    //{
                    //    var response = x.Result;

                    //    if (response.Success && response.Result != null)
                    //    {
                    //        var onePlusOne = (string)response.Result;
                    //        MessageBox.Show(onePlusOne.ToString());
                    //    }
                    //});
                }
                else
                {
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