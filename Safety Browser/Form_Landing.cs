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
    public partial class Form_Landing : Form
    {
        Timer t1 = new Timer();

        public Form_Landing()
        {
            InitializeComponent();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Form_YB form = new Form_YB();
            form.Show();
            Hide();
            timer.Stop();
        }

        private void Form_Landing_Load(object sender, EventArgs e)
        {
            Opacity = 0; 

            t1.Interval = 10;
            t1.Tick += new EventHandler(fadeIn);
            t1.Start();
        }

        void fadeIn(object sender, EventArgs e)
        {
            if (Opacity >= 1)
            {
                t1.Stop();
            }
            else
            {
                Opacity += 0.05;
            }
        }
    }
}
