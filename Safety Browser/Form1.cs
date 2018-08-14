using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Safety_Browser
{
    public partial class Form1 : Form
    {
        private string[] web_service = { "www.ssicortex.com/GetTxt2Search", "www.ssitectonic.com/GetTxt2Search", "www.ssihedonic.com/GetTxt2Search" };
        private string[] domain_test = { "www.ssicortex.com/GetDomains", "www.ssitectonic.com/GetText2Search", "www.ssihedonic.com/GetText2Search" };

        private string BRAND_CODE = "YB";
        private string API_KEY_SSICORTEX = "0397c2be1d97aac330bc3d5278c47696";
        private string API_KEY_SSITECTONIC = "561b9fd16b50553213e4be2024fb4cf9";
        private string API_KEY_SSIHEDONIC = "6b8c7e5617414bf2d4ace37600b6ab71";

        public Form1()
        {
            InitializeComponent();
        }
        
        private async Task asdAsync()
        {
            var client = new HttpClient();

            // Create the HttpContent for the form to be posted.
            var requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("api_key", "6b8c7e5617414bf2d4ace37600b6ab71"),
                new KeyValuePair<string, string>("brand_code", BRAND_CODE),
            });

            // Get the response.
            HttpResponseMessage response = await client.PostAsync(
                "http://www.ssicortex.com/GetTxt2Search",
                requestContent);

            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                MessageBox.Show(await reader.ReadToEndAsync());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            asdAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            //HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(@"http://www.ssicortex.com/GetTxt2Search");
            //GETRequest.Method = "GET";
            //GETRequest.ContentType = "application/x-www-form-urlencoded";
            //GETRequest.Headers.Add("api_key", "6b8c7e5617414bf2d4ace37600b6ab71");
            //HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();

            //// Get the stream associated with the response.
            //Stream receiveStream = response.GetResponseStream();
            //StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

            //MessageBox.Show(readStream.ReadToEnd());

            //WebClient webClient = new WebClient();
            //webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //webClient.QueryString.Add("api-key", "6b8c7e5617414bf2d4ace37600b6ab71");
            //string result = webClient.DownloadString(@"http://www.ssicortex.com/GetTxt2Search");
            //MessageBox.Show(result);

            //try
            //{
            //    string json = " { ";

            //    json += " \"apdasdasi_key\":\"0397c2be1d97aac330bc3d5278c47696\", ";
            //    json += " \"brand_code\":\"YB\"";
            //    json += " } ";

            //    var client = new RestClient(@"https://www.ssicortex.com/GetTxt2Search");
            //    var request = new RestRequest(Method.POST);
            //    request.AddHeader("content-type", "application/x-www-form-urlencoded; charset=utf-8");
            //    request.AddParameter("api_key", API_KEY_SSICORTEX);
            //    request.AddParameter("brand_code", BRAND_CODE);
            //    var response = client.Execute(request);
            //    string mystring = response.Content;
            //    MessageBox.Show(mystring);
            //    label1.Text = mystring;
            //}
            //catch (Exception err)
            //{
            //    MessageBox.Show(err.Message);
            //}
            //MessageBox.Show(mystring);

            //WebClient webClient = new WebClient();
            //webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //byte[] response = client.UploadData("your url", "POST", new byte[] { });
            ////get the response as a string and do something with it...
            //string s = System.Text.Encoding.Default.GetString(response);



            // string address = string.Format(
            //"www.ssicortex.com/GetTxt2Search?api_key={0}&brand_code={1}",
            //Uri.EscapeDataString(API_KEY_SSICORTEX),
            //Uri.EscapeDataString(BRAND_CODE));
            // string text;
            // using (WebClient client = new WebClient())
            // {
            //     text = client.DownloadString(address);
            //     MessageBox.Show(text);
            // }



            //string apiKey = "xxxx11112222333";
            //string urlForShortening = @"http://www.codeproject.com/Tips/497123/How-to-make-REST-requests-with-Csharp";
            //string destination = @"https://internalurl/api/shorten";

            //RestClient client = new RestClient(destination, HttpVerb.POST,
            //    "{'api_key': '" + apiKey + "', 'url': '" + urlForShortening + "'}");
            //var json = client.MakeRequest();
            //MessageBox.Show(json);
        }
    }
}
