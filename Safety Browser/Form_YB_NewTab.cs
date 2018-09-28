using CefSharp;
using CefSharp.WinForms;
using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Safety_Browser
{
    public partial class Form_YB_NewTab : Form
    {
        private string[] web_service = { "http://www.ssicortex.com/GetTxt2Search", "http://www.ssitectonic.com/GetTxt2Search", "http://www.ssihedonic.com/GetTxt2Search" };
        private string[] domain_service = { "http://www.ssicortex.com/GetDomains", "http://www.ssitectonic.com/GetText2Search", "http://www.ssihedonic.com/GetText2Search" };
        private string[] send_service = { "http://www.ssicortex.com/SendDetails", "http://www.ssitectonic.com/SendDetails", "http://www.ssihedonic.com/SendDetails" };
        private bool close = true;
        private bool networkIsAvailable;
        private ChromiumWebBrowser chromeBrowser;
        private double defaultValue = 0;
        private GlobalKeyboardHook gHook;
        public static bool SetClose = false;
        public static bool Main_SetClose = false;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private const int
             HTLEFT = 10,
             HTRIGHT = 11,
             HTTOP = 12,
             HTTOPLEFT = 13,
             HTTOPRIGHT = 14,
             HTBOTTOM = 15,
             HTBOTTOMLEFT = 16,
             HTBOTTOMRIGHT = 17;
        const int _ = 1; // you can rename this variable if you like
        new Rectangle Top { get { return new Rectangle(0, 0, this.ClientSize.Width, _); } }
        new Rectangle Left { get { return new Rectangle(0, 0, _, this.ClientSize.Height); } }
        new Rectangle Bottom { get { return new Rectangle(0, this.ClientSize.Height - _, this.ClientSize.Width, _); } }
        new Rectangle Right { get { return new Rectangle(this.ClientSize.Width - _, 0, _, ClientSize.Height); } }
        Rectangle TopLeft { get { return new Rectangle(0, 0, _, _); } }
        Rectangle TopRight { get { return new Rectangle(this.ClientSize.Width - _, 0, _, _); } }
        Rectangle BottomLeft { get { return new Rectangle(0, this.ClientSize.Height - _, _, _); } }
        Rectangle BottomRight { get { return new Rectangle(this.ClientSize.Width - _, this.ClientSize.Height - _, _, _); } }
        public bool IsCloseVisible { get; private set; }
        public bool not_hijacked = true;
        private string newtab_link;
        private bool hard_refresh;
        private string get_status;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        public Form_YB_NewTab(string newtab_link_get, string status)
        {
            InitializeComponent();
            newtab_link = newtab_link_get;
            InitializeChromium();
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);

            gHook = new GlobalKeyboardHook();
            gHook.KeyDown += new KeyEventHandler(gHook_KeyDown);
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                gHook.HookedKeys.Add(key);
            }
            gHook.hook();

            get_status = status;
        }

        public void gHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (not_hijacked)
            {
                if (ContainsFocus)
                {
                    if (e.KeyData.ToString().ToUpper().IndexOf("Control".ToUpper()) >= 0 && e.KeyData.ToString().ToUpper().IndexOf("Shift".ToUpper()) >= 0 && e.KeyCode == Keys.R)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                chromeBrowser.Reload(false);
                            }));

                        }).Start();
                    } 
                    else if (e.KeyData.ToString().ToUpper().IndexOf("Control".ToUpper()) >= 0 && e.KeyCode == Keys.R)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                chromeBrowser.Reload(true);
                            }));

                        }).Start();
                    }
                    else if (e.KeyCode == Keys.Left)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                chromeBrowser.Back();
                            }));

                        }).Start();
                    }
                    else if (e.KeyCode == Keys.Right)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                chromeBrowser.Forward();
                            }));

                        }).Start();
                    }
                    else if (e.KeyData.ToString().ToUpper().IndexOf("Control".ToUpper()) >= 0 && e.KeyCode == Keys.D0)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                defaultValue = 0;
                                chromeBrowser.SetZoomLevel(0);
                            }));

                        }).Start();
                    }
                    else if (e.KeyData.ToString().ToUpper().IndexOf("Control".ToUpper()) >= 0 && e.KeyCode == Keys.Oemplus)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                chromeBrowser.SetZoomLevel(++defaultValue);
                            }));

                        }).Start();
                    }
                    else if (e.KeyData.ToString().ToUpper().IndexOf("Control".ToUpper()) >= 0 && e.KeyCode == Keys.OemMinus)
                    {
                        new Thread(() =>
                        {
                            Invoke(new Action(delegate
                            {
                                chromeBrowser.SetZoomLevel(--defaultValue);
                            }));

                        }).Start();
                    }
                }
            }
        }

        // Form Load
        private void Form_Main_Load(object sender, EventArgs e)
        {
            chromeBrowser.LoadingStateChanged += BrowserLoadingStateChanged;
            NetworkAvailability();
            PictureBoxCenter();
            
            if (get_status == "help")
            {
                MinimumSize = new Size(710, 710);
                Size = new Size(710, 710);
            }
            else
            {
                MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
                WindowState = FormWindowState.Maximized;
            }
        }

        private void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (e.IsLoading)
            {
                Invoke(new Action(() =>
                {
                    pictureBox_browserstop.Visible = true;
                    pictureBox_reload.Visible = false;

                    pictureBox_loader_nav.Enabled = true;
                    pictureBox_loader_nav.Visible = true;
                }));
            }

            if (!e.IsLoading)
            {
                Invoke(new Action(() =>
                {
                    pictureBox_browserstop.Visible = false;
                    pictureBox_reload.Visible = true;

                    pictureBox_loader_nav.Visible = false;
                }));
            }
        }

        // Network Handler
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
                panel_cefsharp.Visible = false;

                panel_connection.Visible = true;
                panel_connection.Enabled = true;
            }

            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
        }

        // Network Handler Changed
        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            Invoke(new Action(() =>
            {
                networkIsAvailable = e.IsAvailable;

                if (networkIsAvailable)
                {
                    panel_cefsharp.Visible = true;

                    panel_connection.Visible = false;
                    panel_connection.Enabled = false;
                }
                else
                {
                    panel_cefsharp.Visible = false;

                    panel_connection.Visible = true;
                    panel_connection.Enabled = true;
                }
            }));
        }

        // Loader Center
        private void PictureBoxCenter()
        {
            panel_connection.Left = (ClientSize.Width - panel_connection.Width) / 2;
            panel_connection.Top = (ClientSize.Height - panel_connection.Height) / 2;
        }

        // Initialize Chromium
        private void InitializeChromium()
        {
            chromeBrowser = new ChromiumWebBrowser(newtab_link);
            chromeBrowser.MenuHandler = new CustomMenuHandler();
            panel_cefsharp.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            pictureBox_reload.Enabled = true;
            pictureBox_reload.Image = Properties.Resources.refresh;
            reloadToolStripMenuItem.Enabled = true;
            cleanAndReloadToolStripMenuItem.Enabled = true;
            resetZoomToolStripMenuItem.Enabled = true;
            zoomInToolStripMenuItem.Enabled = true;
            zoomOutToolStripMenuItem.Enabled = true;
        }

        // Form Closing
        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {            
            if (close)
            {
                DialogResult dr = MessageBox.Show("退出程序？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Hide();
                }
            }
            else
            {
                Hide();
            }
        }

        private void pictureBox_minimize_MouseHover(object sender, EventArgs e)
        {
            pictureBox_minimize.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_minimize_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_minimize.BackColor = Color.FromArgb(74, 84, 89);
        }

        private void pictureBox_maximize_MouseHover(object sender, EventArgs e)
        {
            pictureBox_maximize.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_maximize_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_maximize.BackColor = Color.FromArgb(74, 84, 89);
        }

        private void pictureBox_close_MouseHover(object sender, EventArgs e)
        {
            if (!IsCloseVisible)
            {
                pictureBox_close.BackColor = Color.FromArgb(197, 112, 53);
            }
        }

        private void pictureBox_close_MouseLeave(object sender, EventArgs e)
        {
            if (!IsCloseVisible)
            {
                pictureBox_close.BackColor = Color.FromArgb(74, 84, 89);
            }
        }

        private void pictureBox_menu_MouseHover(object sender, EventArgs e)
        {
            Point position = new Point(pictureBox_menu.Left, pictureBox_menu.Height);
            ToolStripMenuItem.DropDown.Show(pictureBox_menu, position);

            pictureBox_hover.BackColor = Color.FromArgb(197, 112, 53);
            pictureBox_menu.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_menu_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_hover.BackColor = Color.FromArgb(74, 84, 89);
            pictureBox_menu.BackColor = Color.FromArgb(74, 84, 89);
        }

        private void pictureBox_hover_MouseHover(object sender, EventArgs e)
        {
            Point position = new Point(pictureBox_menu.Left, pictureBox_menu.Height);
            ToolStripMenuItem.DropDown.Show(pictureBox_menu, position);

            pictureBox_hover.BackColor = Color.FromArgb(197, 112, 53);
            pictureBox_menu.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_hover_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_hover.BackColor = Color.FromArgb(74, 84, 89);
            pictureBox_menu.BackColor = Color.FromArgb(74, 84, 89);
        }

        private void pictureBox_menu_Click(object sender, EventArgs e)
        {
            Point position = new Point(pictureBox_menu.Left, pictureBox_menu.Height);
            ToolStripMenuItem.DropDown.Show(pictureBox_menu, position);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (close)
            {
                IsCloseVisible = true;
                pictureBox_menu.BackColor = Color.FromArgb(197, 112, 53);
                pictureBox_hover.BackColor = Color.FromArgb(197, 112, 53);

                DialogResult dr = MessageBox.Show("退出程序？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    Hide();
                }
                else
                {
                    IsCloseVisible = false;
                    pictureBox_menu.BackColor = Color.FromArgb(74, 84, 89);
                    pictureBox_hover.BackColor = Color.FromArgb(74, 84, 89);
                }
            }
            else
            {
                Hide();
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (not_hijacked)
            {
                chromeBrowser.Reload(true);
            }
        }
        
        private void cleanAndReloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (not_hijacked)
            {
                chromeBrowser.Reload(false);
            }
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (not_hijacked)
            {
                chromeBrowser.SetZoomLevel(++defaultValue);
            }
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (not_hijacked)
            {
                chromeBrowser.SetZoomLevel(--defaultValue);
            }
        }

        private void resetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (not_hijacked)
            {
                defaultValue = 0;
                chromeBrowser.SetZoomLevel(0);
            }
        }

        private void timer_close_Tick(object sender, EventArgs e)
        {
            if (SetClose)
            {
                close = false;
                Close();
                SetClose = false;
            } else if (Main_SetClose)
            {
                close = false;
                Close();
                Main_SetClose = false;
            }
        }

        private void pictureBox_reload_Click(object sender, EventArgs e)
        {
            if (!hard_refresh)
            {
                chromeBrowser.Reload(true);
            }

            hard_refresh = false;
        }

        private void pictureBox_reload_MouseDown(object sender, MouseEventArgs e)
        {
            timer_mouse.Start();
        }

        private void pictureBox_reload_MouseUp(object sender, MouseEventArgs e)
        {
            timer_mouse.Stop();
        }

        private void timer_mouse_Tick(object sender, EventArgs e)
        {
            chromeBrowser.Reload(false);
            pictureBox_reload.BackColor = Color.FromArgb(74, 84, 89);
            Cursor.Current = Cursors.Default;
            timer_mouse.Stop();
            hard_refresh = true;
        }

        private void pictureBox_reload_MouseHover(object sender, EventArgs e)
        {
            pictureBox_reload.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_reload_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_reload.BackColor = Color.FromArgb(74, 84, 89);
        }

        private void pictureBox_browserstop_Click(object sender, EventArgs e)
        {
            chromeBrowser.Stop();
        }

        private void pictureBox_browserstop_MouseHover(object sender, EventArgs e)
        {
            pictureBox_browserstop.BackColor = Color.FromArgb(197, 112, 53);
        }

        private void pictureBox_browserstop_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_browserstop.BackColor = Color.FromArgb(74, 84, 89);
        }

        private void pictureBox_hover_Click(object sender, EventArgs e)
        {
            Point position = new Point(pictureBox_menu.Left, pictureBox_menu.Height);
            ToolStripMenuItem.DropDown.Show(pictureBox_menu, position);
        }
        
        private void pictureBox_minimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void pictureBox_maximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
            {
                MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
                WindowState = FormWindowState.Maximized;
            }
        }

        private void pictureBox_close_Click(object sender, EventArgs e)
        {
            if (close)
            {
                IsCloseVisible = true;
                pictureBox_close.BackColor = Color.FromArgb(197, 112, 53);

                DialogResult dr = MessageBox.Show("退出程序？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    Hide();
                }
                else
                {
                    IsCloseVisible = false;
                    pictureBox_close.BackColor = Color.FromArgb(74, 84, 89);
                }
            }
            else
            {
                Hide();
            }
        }
         
        private void label_titlebar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks >= 2)
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                }
                else
                {
                    MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
                    WindowState = FormWindowState.Maximized;
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            SolidBrush defaultColor = new SolidBrush(Color.FromArgb(74, 84, 89));
            e.Graphics.FillRectangle(defaultColor, Top);
            e.Graphics.FillRectangle(defaultColor, Left);
            e.Graphics.FillRectangle(defaultColor, Right);
            e.Graphics.FillRectangle(defaultColor, Bottom);
        }  
        
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x84)
            {
                var cursor = this.PointToClient(Cursor.Position);

                if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;

                else if (Top.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (Left.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
        }
    }
}