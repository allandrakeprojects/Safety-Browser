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
    public partial class Form_Notification : Form
    {
        public Form_Notification()
        {
            InitializeComponent();
            FadeIn(this);


        }

        private async void FadeIn(Form o, int interval = 20)
        {
            while (o.Opacity < 1.0)
            {
                await Task.Delay(interval);
                o.Opacity += 0.05;
            }

            o.Opacity = 1; 

            timer_open.Start();
        }

        private async void FadeOut(Form o, int interval = 20)
        {
            while (o.Opacity > 0.0)
            {
                await Task.Delay(interval);
                o.Opacity -= 0.05;
            }

            o.Opacity = 0;    

            Close();
        }

        private void timer_open_Tick(object sender, EventArgs e)
        {
            FadeOut(this);
            timer_open.Stop();
        }
    }
}
