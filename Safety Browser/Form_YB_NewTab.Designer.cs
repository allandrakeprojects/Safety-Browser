namespace Safety_Browser
{
    partial class Form_YB_NewTab
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_YB_NewTab));
            this.panel_connection = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel_cefsharp = new System.Windows.Forms.Panel();
            this.timer_close = new System.Windows.Forms.Timer(this.components);
            this.label_titlebar = new System.Windows.Forms.Label();
            this.pictureBox_maximize = new System.Windows.Forms.PictureBox();
            this.pictureBox_minimize = new System.Windows.Forms.PictureBox();
            this.pictureBox_close = new System.Windows.Forms.PictureBox();
            this.pictureBox_reload = new System.Windows.Forms.PictureBox();
            this.pictureBox_menu = new System.Windows.Forms.PictureBox();
            this.pictureBox_hover = new System.Windows.Forms.PictureBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanAndReloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.resetZoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_version = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_mouse = new System.Windows.Forms.Timer(this.components);
            this.pictureBox_browserstop = new System.Windows.Forms.PictureBox();
            this.pictureBox_loader_nav = new System.Windows.Forms.PictureBox();
            this.panel_connection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_maximize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_minimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_close)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_reload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_menu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_hover)).BeginInit();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_browserstop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loader_nav)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_connection
            // 
            this.panel_connection.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel_connection.Controls.Add(this.pictureBox1);
            this.panel_connection.Controls.Add(this.label1);
            this.panel_connection.Controls.Add(this.label8);
            this.panel_connection.Location = new System.Drawing.Point(961, 817);
            this.panel_connection.Name = "panel_connection";
            this.panel_connection.Size = new System.Drawing.Size(335, 289);
            this.panel_connection.TabIndex = 39;
            this.panel_connection.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Safety_Browser.Properties.Resources.connection;
            this.pictureBox1.Location = new System.Drawing.Point(81, 47);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(180, 140);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 40;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label8.ForeColor = System.Drawing.Color.DimGray;
            this.label8.Location = new System.Drawing.Point(3, 214);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(329, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "请查询你的网络连接";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel_cefsharp
            // 
            this.panel_cefsharp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_cefsharp.Location = new System.Drawing.Point(1, 38);
            this.panel_cefsharp.Name = "panel_cefsharp";
            this.panel_cefsharp.Size = new System.Drawing.Size(1268, 725);
            this.panel_cefsharp.TabIndex = 48;
            // 
            // timer_close
            // 
            this.timer_close.Enabled = true;
            this.timer_close.Interval = 1000;
            this.timer_close.Tick += new System.EventHandler(this.timer_close_Tick);
            // 
            // label_titlebar
            // 
            this.label_titlebar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_titlebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(84)))), ((int)(((byte)(89)))));
            this.label_titlebar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_titlebar.ForeColor = System.Drawing.Color.White;
            this.label_titlebar.Location = new System.Drawing.Point(1, 1);
            this.label_titlebar.Name = "label_titlebar";
            this.label_titlebar.Size = new System.Drawing.Size(1268, 37);
            this.label_titlebar.TabIndex = 40;
            this.label_titlebar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_titlebar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_titlebar_MouseDown);
            // 
            // pictureBox_maximize
            // 
            this.pictureBox_maximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_maximize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(84)))), ((int)(((byte)(89)))));
            this.pictureBox_maximize.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_maximize.Image")));
            this.pictureBox_maximize.Location = new System.Drawing.Point(1183, 1);
            this.pictureBox_maximize.Name = "pictureBox_maximize";
            this.pictureBox_maximize.Size = new System.Drawing.Size(43, 37);
            this.pictureBox_maximize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_maximize.TabIndex = 42;
            this.pictureBox_maximize.TabStop = false;
            this.pictureBox_maximize.Click += new System.EventHandler(this.pictureBox_maximize_Click);
            this.pictureBox_maximize.MouseLeave += new System.EventHandler(this.pictureBox_maximize_MouseLeave);
            this.pictureBox_maximize.MouseHover += new System.EventHandler(this.pictureBox_maximize_MouseHover);
            // 
            // pictureBox_minimize
            // 
            this.pictureBox_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_minimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(84)))), ((int)(((byte)(89)))));
            this.pictureBox_minimize.ErrorImage = null;
            this.pictureBox_minimize.Image = global::Safety_Browser.Properties.Resources.minimize;
            this.pictureBox_minimize.Location = new System.Drawing.Point(1140, 1);
            this.pictureBox_minimize.Name = "pictureBox_minimize";
            this.pictureBox_minimize.Size = new System.Drawing.Size(43, 37);
            this.pictureBox_minimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_minimize.TabIndex = 43;
            this.pictureBox_minimize.TabStop = false;
            this.pictureBox_minimize.Click += new System.EventHandler(this.pictureBox_minimize_Click);
            this.pictureBox_minimize.MouseLeave += new System.EventHandler(this.pictureBox_minimize_MouseLeave);
            this.pictureBox_minimize.MouseHover += new System.EventHandler(this.pictureBox_minimize_MouseHover);
            // 
            // pictureBox_close
            // 
            this.pictureBox_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(84)))), ((int)(((byte)(89)))));
            this.pictureBox_close.Image = global::Safety_Browser.Properties.Resources.close;
            this.pictureBox_close.Location = new System.Drawing.Point(1226, 1);
            this.pictureBox_close.Name = "pictureBox_close";
            this.pictureBox_close.Size = new System.Drawing.Size(43, 37);
            this.pictureBox_close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_close.TabIndex = 44;
            this.pictureBox_close.TabStop = false;
            this.pictureBox_close.Click += new System.EventHandler(this.pictureBox_close_Click);
            this.pictureBox_close.MouseLeave += new System.EventHandler(this.pictureBox_close_MouseLeave);
            this.pictureBox_close.MouseHover += new System.EventHandler(this.pictureBox_close_MouseHover);
            // 
            // pictureBox_reload
            // 
            this.pictureBox_reload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(84)))), ((int)(((byte)(89)))));
            this.pictureBox_reload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_reload.Enabled = false;
            this.pictureBox_reload.Image = global::Safety_Browser.Properties.Resources.refresh_visible;
            this.pictureBox_reload.Location = new System.Drawing.Point(61, 1);
            this.pictureBox_reload.Name = "pictureBox_reload";
            this.pictureBox_reload.Size = new System.Drawing.Size(35, 37);
            this.pictureBox_reload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_reload.TabIndex = 58;
            this.pictureBox_reload.TabStop = false;
            this.pictureBox_reload.Click += new System.EventHandler(this.pictureBox_reload_Click);
            this.pictureBox_reload.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_reload_MouseDown);
            this.pictureBox_reload.MouseLeave += new System.EventHandler(this.pictureBox_reload_MouseLeave);
            this.pictureBox_reload.MouseHover += new System.EventHandler(this.pictureBox_reload_MouseHover);
            this.pictureBox_reload.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_reload_MouseUp);
            // 
            // pictureBox_menu
            // 
            this.pictureBox_menu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(84)))), ((int)(((byte)(89)))));
            this.pictureBox_menu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_menu.Image = global::Safety_Browser.Properties.Resources.menu;
            this.pictureBox_menu.Location = new System.Drawing.Point(13, 4);
            this.pictureBox_menu.Name = "pictureBox_menu";
            this.pictureBox_menu.Size = new System.Drawing.Size(33, 29);
            this.pictureBox_menu.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_menu.TabIndex = 54;
            this.pictureBox_menu.TabStop = false;
            this.pictureBox_menu.Click += new System.EventHandler(this.pictureBox_menu_Click);
            this.pictureBox_menu.MouseLeave += new System.EventHandler(this.pictureBox_menu_MouseLeave);
            this.pictureBox_menu.MouseHover += new System.EventHandler(this.pictureBox_menu_MouseHover);
            // 
            // pictureBox_hover
            // 
            this.pictureBox_hover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(84)))), ((int)(((byte)(89)))));
            this.pictureBox_hover.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_hover.Location = new System.Drawing.Point(1, 1);
            this.pictureBox_hover.Name = "pictureBox_hover";
            this.pictureBox_hover.Size = new System.Drawing.Size(58, 37);
            this.pictureBox_hover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_hover.TabIndex = 53;
            this.pictureBox_hover.TabStop = false;
            this.pictureBox_hover.Click += new System.EventHandler(this.pictureBox_hover_Click);
            this.pictureBox_hover.MouseLeave += new System.EventHandler(this.pictureBox_hover_MouseLeave);
            this.pictureBox_hover.MouseHover += new System.EventHandler(this.pictureBox_hover_MouseHover);
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(84)))), ((int)(((byte)(89)))));
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(-5, 14);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(20, 24);
            this.menuStrip.TabIndex = 55;
            this.menuStrip.Text = "menuStrip1";
            // 
            // ToolStripMenuItem
            // 
            this.ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.versionToolStripMenuItem});
            this.ToolStripMenuItem.Name = "ToolStripMenuItem";
            this.ToolStripMenuItem.Size = new System.Drawing.Size(12, 20);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.fileToolStripMenuItem.Text = "文档";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.exitToolStripMenuItem.Text = "退出";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadToolStripMenuItem,
            this.cleanAndReloadToolStripMenuItem,
            this.toolStripMenuItem1,
            this.resetZoomToolStripMenuItem,
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.viewToolStripMenuItem.Text = "观看";
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Enabled = false;
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.ShortcutKeyDisplayString = "刷新键";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
            this.reloadToolStripMenuItem.Text = "刷新";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // cleanAndReloadToolStripMenuItem
            // 
            this.cleanAndReloadToolStripMenuItem.Enabled = false;
            this.cleanAndReloadToolStripMenuItem.Name = "cleanAndReloadToolStripMenuItem";
            this.cleanAndReloadToolStripMenuItem.ShortcutKeyDisplayString = "强制刷新键";
            this.cleanAndReloadToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
            this.cleanAndReloadToolStripMenuItem.Text = "强制刷新";
            this.cleanAndReloadToolStripMenuItem.Click += new System.EventHandler(this.cleanAndReloadToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(248, 6);
            // 
            // resetZoomToolStripMenuItem
            // 
            this.resetZoomToolStripMenuItem.Enabled = false;
            this.resetZoomToolStripMenuItem.Name = "resetZoomToolStripMenuItem";
            this.resetZoomToolStripMenuItem.ShortcutKeyDisplayString = "网页桌面字幕格式缩小";
            this.resetZoomToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
            this.resetZoomToolStripMenuItem.Text = "重置縮放";
            this.resetZoomToolStripMenuItem.Click += new System.EventHandler(this.resetZoomToolStripMenuItem_Click);
            // 
            // zoomInToolStripMenuItem
            // 
            this.zoomInToolStripMenuItem.Enabled = false;
            this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            this.zoomInToolStripMenuItem.ShortcutKeyDisplayString = "放大键";
            this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
            this.zoomInToolStripMenuItem.Text = "放大";
            this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
            // 
            // zoomOutToolStripMenuItem
            // 
            this.zoomOutToolStripMenuItem.Enabled = false;
            this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
            this.zoomOutToolStripMenuItem.ShortcutKeyDisplayString = "缩小键";
            this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
            this.zoomOutToolStripMenuItem.Text = "縮小";
            this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
            // 
            // versionToolStripMenuItem
            // 
            this.versionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_version});
            this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
            this.versionToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.versionToolStripMenuItem.Text = "版";
            // 
            // toolStripMenuItem_version
            // 
            this.toolStripMenuItem_version.Enabled = false;
            this.toolStripMenuItem_version.Name = "toolStripMenuItem_version";
            this.toolStripMenuItem_version.Size = new System.Drawing.Size(98, 22);
            this.toolStripMenuItem_version.Text = "1.0.0";
            // 
            // timer_mouse
            // 
            this.timer_mouse.Interval = 3000;
            this.timer_mouse.Tick += new System.EventHandler(this.timer_mouse_Tick);
            // 
            // pictureBox_browserstop
            // 
            this.pictureBox_browserstop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(84)))), ((int)(((byte)(89)))));
            this.pictureBox_browserstop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_browserstop.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_browserstop.Image")));
            this.pictureBox_browserstop.Location = new System.Drawing.Point(61, 1);
            this.pictureBox_browserstop.Name = "pictureBox_browserstop";
            this.pictureBox_browserstop.Size = new System.Drawing.Size(35, 37);
            this.pictureBox_browserstop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_browserstop.TabIndex = 60;
            this.pictureBox_browserstop.TabStop = false;
            this.pictureBox_browserstop.Visible = false;
            this.pictureBox_browserstop.Click += new System.EventHandler(this.pictureBox_browserstop_Click);
            this.pictureBox_browserstop.MouseLeave += new System.EventHandler(this.pictureBox_browserstop_MouseLeave);
            this.pictureBox_browserstop.MouseHover += new System.EventHandler(this.pictureBox_browserstop_MouseHover);
            // 
            // pictureBox_loader_nav
            // 
            this.pictureBox_loader_nav.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(84)))), ((int)(((byte)(89)))));
            this.pictureBox_loader_nav.Enabled = false;
            this.pictureBox_loader_nav.ErrorImage = null;
            this.pictureBox_loader_nav.Image = global::Safety_Browser.Properties.Resources.loader_nav;
            this.pictureBox_loader_nav.Location = new System.Drawing.Point(102, 12);
            this.pictureBox_loader_nav.Name = "pictureBox_loader_nav";
            this.pictureBox_loader_nav.Size = new System.Drawing.Size(36, 14);
            this.pictureBox_loader_nav.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_loader_nav.TabIndex = 82;
            this.pictureBox_loader_nav.TabStop = false;
            this.pictureBox_loader_nav.Visible = false;
            // 
            // Form_YB_NewTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1270, 764);
            this.Controls.Add(this.pictureBox_loader_nav);
            this.Controls.Add(this.pictureBox_reload);
            this.Controls.Add(this.pictureBox_browserstop);
            this.Controls.Add(this.pictureBox_menu);
            this.Controls.Add(this.pictureBox_hover);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.pictureBox_close);
            this.Controls.Add(this.pictureBox_minimize);
            this.Controls.Add(this.pictureBox_maximize);
            this.Controls.Add(this.panel_connection);
            this.Controls.Add(this.label_titlebar);
            this.Controls.Add(this.panel_cefsharp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1270, 764);
            this.Name = "Form_YB_NewTab";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.panel_connection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_maximize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_minimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_close)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_reload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_menu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_hover)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_browserstop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_loader_nav)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel_connection;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel_cefsharp;
        private System.Windows.Forms.Timer timer_close;
        private System.Windows.Forms.Label label_titlebar;
        private System.Windows.Forms.PictureBox pictureBox_maximize;
        private System.Windows.Forms.PictureBox pictureBox_minimize;
        private System.Windows.Forms.PictureBox pictureBox_close;
        private System.Windows.Forms.PictureBox pictureBox_reload;
        private System.Windows.Forms.PictureBox pictureBox_menu;
        private System.Windows.Forms.PictureBox pictureBox_hover;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cleanAndReloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem resetZoomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_version;
        private System.Windows.Forms.Timer timer_mouse;
        private System.Windows.Forms.PictureBox pictureBox_browserstop;
        private System.Windows.Forms.PictureBox pictureBox_loader_nav;
    }
}