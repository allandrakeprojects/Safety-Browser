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
    public partial class Form_Loader : Form
    {
        public Form_Loader()
        {
            InitializeComponent();
        }

        private void timer_loader_Tick(object sender, EventArgs e)
        {
            timer_loader.Stop();
            Form_Main form_main = new Form_Main();
            Hide();
            form_main.ShowDialog();
        }
    }
}
