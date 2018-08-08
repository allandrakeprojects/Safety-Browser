using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Safety_Browser
{
    public partial class Form_Loader : Form
    {
        private int timer;

        public Form_Loader()
        {
            InitializeComponent();
        }
        
        // Timer Loader
        private void timer_loader_Tick(object sender, EventArgs e)
        {
            timer++;

            if (timer == 5)
            {
                label.Text = "Getting ready...";
            }

            if (timer == 10)
            {
                timer_loader.Stop();
                Form_Main form_main = new Form_Main();
                Hide();
                form_main.ShowDialog();
                Close();
            }

        }
    }
}
