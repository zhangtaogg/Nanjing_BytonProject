namespace winform_SeerAgv_MapDisplay
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel_back = new System.Windows.Forms.Panel();
            this.skinButton2 = new CCWin.SkinControl.SkinButton();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.skinButton1 = new CCWin.SkinControl.SkinButton();
            this.skinTextBox1 = new CCWin.SkinControl.SkinTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonskin_getMap = new CCWin.SkinControl.SkinButton();
            this.textBox_display = new System.Windows.Forms.TextBox();
            this.buttonskin_getPosi = new CCWin.SkinControl.SkinButton();
            this.btnskin_getLaser = new CCWin.SkinControl.SkinButton();
            this.textSkin_port = new CCWin.SkinControl.SkinTextBox();
            this.textskin_ip = new CCWin.SkinControl.SkinTextBox();
            this.buttonskin_connect = new CCWin.SkinControl.SkinButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel_back.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_back
            // 
            this.panel_back.BackColor = System.Drawing.Color.AliceBlue;
            this.panel_back.Controls.Add(this.skinButton2);
            this.panel_back.Controls.Add(this.pictureBox2);
            this.panel_back.Controls.Add(this.skinButton1);
            this.panel_back.Controls.Add(this.skinTextBox1);
            this.panel_back.Controls.Add(this.pictureBox1);
            this.panel_back.Controls.Add(this.buttonskin_getMap);
            this.panel_back.Controls.Add(this.textBox_display);
            this.panel_back.Controls.Add(this.buttonskin_getPosi);
            this.panel_back.Controls.Add(this.btnskin_getLaser);
            this.panel_back.Controls.Add(this.textSkin_port);
            this.panel_back.Controls.Add(this.textskin_ip);
            this.panel_back.Controls.Add(this.buttonskin_connect);
            this.panel_back.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_back.Location = new System.Drawing.Point(0, 0);
            this.panel_back.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel_back.Name = "panel_back";
            this.panel_back.Size = new System.Drawing.Size(1307, 545);
            this.panel_back.TabIndex = 0;
            this.panel_back.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_back_Paint);
            // 
            // skinButton2
            // 
            this.skinButton2.BackColor = System.Drawing.Color.Transparent;
            this.skinButton2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton2.DownBack = null;
            this.skinButton2.Location = new System.Drawing.Point(21, 489);
            this.skinButton2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.skinButton2.MouseBack = null;
            this.skinButton2.Name = "skinButton2";
            this.skinButton2.NormlBack = null;
            this.skinButton2.Size = new System.Drawing.Size(160, 32);
            this.skinButton2.TabIndex = 12;
            this.skinButton2.Text = "point";
            this.skinButton2.UseVisualStyleBackColor = false;
            this.skinButton2.Click += new System.EventHandler(this.skinButton2_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.DarkTurquoise;
            this.pictureBox2.Location = new System.Drawing.Point(796, 300);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(129, 81);
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // skinButton1
            // 
            this.skinButton1.BackColor = System.Drawing.Color.Transparent;
            this.skinButton1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton1.DownBack = null;
            this.skinButton1.Location = new System.Drawing.Point(21, 438);
            this.skinButton1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.skinButton1.MouseBack = null;
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.NormlBack = null;
            this.skinButton1.Size = new System.Drawing.Size(160, 32);
            this.skinButton1.TabIndex = 10;
            this.skinButton1.Text = "turn";
            this.skinButton1.UseVisualStyleBackColor = false;
            this.skinButton1.Click += new System.EventHandler(this.skinButton1_Click_1);
            // 
            // skinTextBox1
            // 
            this.skinTextBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinTextBox1.DownBack = null;
            this.skinTextBox1.Icon = null;
            this.skinTextBox1.IconIsButton = false;
            this.skinTextBox1.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox1.IsPasswordChat = '\0';
            this.skinTextBox1.IsSystemPasswordChar = false;
            this.skinTextBox1.Lines = new string[] {
        "192"};
            this.skinTextBox1.Location = new System.Drawing.Point(21, 392);
            this.skinTextBox1.Margin = new System.Windows.Forms.Padding(0);
            this.skinTextBox1.MaxLength = 32767;
            this.skinTextBox1.MinimumSize = new System.Drawing.Size(37, 32);
            this.skinTextBox1.MouseBack = null;
            this.skinTextBox1.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox1.Multiline = true;
            this.skinTextBox1.Name = "skinTextBox1";
            this.skinTextBox1.NormlBack = null;
            this.skinTextBox1.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.skinTextBox1.ReadOnly = false;
            this.skinTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.skinTextBox1.Size = new System.Drawing.Size(160, 32);
            // 
            // 
            // 
            this.skinTextBox1.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skinTextBox1.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTextBox1.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.skinTextBox1.SkinTxt.Location = new System.Drawing.Point(7, 6);
            this.skinTextBox1.SkinTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.skinTextBox1.SkinTxt.Multiline = true;
            this.skinTextBox1.SkinTxt.Name = "BaseText";
            this.skinTextBox1.SkinTxt.Size = new System.Drawing.Size(146, 20);
            this.skinTextBox1.SkinTxt.TabIndex = 0;
            this.skinTextBox1.SkinTxt.Text = "192";
            this.skinTextBox1.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinTextBox1.SkinTxt.WaterText = "";
            this.skinTextBox1.TabIndex = 9;
            this.skinTextBox1.Text = "192";
            this.skinTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.skinTextBox1.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinTextBox1.WaterText = "";
            this.skinTextBox1.WordWrap = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(497, 224);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(177, 88);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // buttonskin_getMap
            // 
            this.buttonskin_getMap.BackColor = System.Drawing.Color.Transparent;
            this.buttonskin_getMap.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.buttonskin_getMap.DownBack = null;
            this.buttonskin_getMap.Location = new System.Drawing.Point(12, 300);
            this.buttonskin_getMap.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonskin_getMap.MouseBack = null;
            this.buttonskin_getMap.Name = "buttonskin_getMap";
            this.buttonskin_getMap.NormlBack = null;
            this.buttonskin_getMap.Size = new System.Drawing.Size(160, 32);
            this.buttonskin_getMap.TabIndex = 7;
            this.buttonskin_getMap.Text = "GetMap";
            this.buttonskin_getMap.UseVisualStyleBackColor = false;
            this.buttonskin_getMap.Click += new System.EventHandler(this.buttonskin_getMap_Click);
            // 
            // textBox_display
            // 
            this.textBox_display.Location = new System.Drawing.Point(272, 30);
            this.textBox_display.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox_display.Multiline = true;
            this.textBox_display.Name = "textBox_display";
            this.textBox_display.Size = new System.Drawing.Size(980, 166);
            this.textBox_display.TabIndex = 6;
            // 
            // buttonskin_getPosi
            // 
            this.buttonskin_getPosi.BackColor = System.Drawing.Color.Transparent;
            this.buttonskin_getPosi.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.buttonskin_getPosi.DownBack = null;
            this.buttonskin_getPosi.Location = new System.Drawing.Point(12, 241);
            this.buttonskin_getPosi.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonskin_getPosi.MouseBack = null;
            this.buttonskin_getPosi.Name = "buttonskin_getPosi";
            this.buttonskin_getPosi.NormlBack = null;
            this.buttonskin_getPosi.Size = new System.Drawing.Size(160, 32);
            this.buttonskin_getPosi.TabIndex = 5;
            this.buttonskin_getPosi.Text = "GetPosition";
            this.buttonskin_getPosi.UseVisualStyleBackColor = false;
            this.buttonskin_getPosi.Click += new System.EventHandler(this.skinButton1_Click);
            // 
            // btnskin_getLaser
            // 
            this.btnskin_getLaser.BackColor = System.Drawing.Color.Transparent;
            this.btnskin_getLaser.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnskin_getLaser.DownBack = null;
            this.btnskin_getLaser.Location = new System.Drawing.Point(12, 182);
            this.btnskin_getLaser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnskin_getLaser.MouseBack = null;
            this.btnskin_getLaser.Name = "btnskin_getLaser";
            this.btnskin_getLaser.NormlBack = null;
            this.btnskin_getLaser.Size = new System.Drawing.Size(160, 32);
            this.btnskin_getLaser.TabIndex = 4;
            this.btnskin_getLaser.Text = "GetLaser";
            this.btnskin_getLaser.UseVisualStyleBackColor = false;
            this.btnskin_getLaser.Click += new System.EventHandler(this.btnskin_getMap_Click);
            // 
            // textSkin_port
            // 
            this.textSkin_port.BackColor = System.Drawing.Color.Transparent;
            this.textSkin_port.DownBack = null;
            this.textSkin_port.Icon = null;
            this.textSkin_port.IconIsButton = false;
            this.textSkin_port.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.textSkin_port.IsPasswordChat = '\0';
            this.textSkin_port.IsSystemPasswordChar = false;
            this.textSkin_port.Lines = new string[] {
        "19204"};
            this.textSkin_port.Location = new System.Drawing.Point(12, 63);
            this.textSkin_port.Margin = new System.Windows.Forms.Padding(0);
            this.textSkin_port.MaxLength = 32767;
            this.textSkin_port.MinimumSize = new System.Drawing.Size(37, 32);
            this.textSkin_port.MouseBack = null;
            this.textSkin_port.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.textSkin_port.Multiline = true;
            this.textSkin_port.Name = "textSkin_port";
            this.textSkin_port.NormlBack = null;
            this.textSkin_port.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.textSkin_port.ReadOnly = false;
            this.textSkin_port.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textSkin_port.Size = new System.Drawing.Size(160, 32);
            // 
            // 
            // 
            this.textSkin_port.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textSkin_port.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textSkin_port.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.textSkin_port.SkinTxt.Location = new System.Drawing.Point(7, 6);
            this.textSkin_port.SkinTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textSkin_port.SkinTxt.Multiline = true;
            this.textSkin_port.SkinTxt.Name = "BaseText";
            this.textSkin_port.SkinTxt.Size = new System.Drawing.Size(146, 20);
            this.textSkin_port.SkinTxt.TabIndex = 0;
            this.textSkin_port.SkinTxt.Text = "19204";
            this.textSkin_port.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.textSkin_port.SkinTxt.WaterText = "";
            this.textSkin_port.TabIndex = 2;
            this.textSkin_port.Text = "19204";
            this.textSkin_port.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textSkin_port.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.textSkin_port.WaterText = "";
            this.textSkin_port.WordWrap = true;
            // 
            // textskin_ip
            // 
            this.textskin_ip.BackColor = System.Drawing.Color.Transparent;
            this.textskin_ip.DownBack = null;
            this.textskin_ip.Icon = null;
            this.textskin_ip.IconIsButton = false;
            this.textskin_ip.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.textskin_ip.IsPasswordChat = '\0';
            this.textskin_ip.IsSystemPasswordChar = false;
            this.textskin_ip.Lines = new string[] {
        "192.168.0.106"};
            this.textskin_ip.Location = new System.Drawing.Point(12, 21);
            this.textskin_ip.Margin = new System.Windows.Forms.Padding(0);
            this.textskin_ip.MaxLength = 32767;
            this.textskin_ip.MinimumSize = new System.Drawing.Size(37, 32);
            this.textskin_ip.MouseBack = null;
            this.textskin_ip.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.textskin_ip.Multiline = true;
            this.textskin_ip.Name = "textskin_ip";
            this.textskin_ip.NormlBack = null;
            this.textskin_ip.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.textskin_ip.ReadOnly = false;
            this.textskin_ip.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textskin_ip.Size = new System.Drawing.Size(160, 32);
            // 
            // 
            // 
            this.textskin_ip.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textskin_ip.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textskin_ip.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.textskin_ip.SkinTxt.Location = new System.Drawing.Point(7, 6);
            this.textskin_ip.SkinTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textskin_ip.SkinTxt.Multiline = true;
            this.textskin_ip.SkinTxt.Name = "BaseText";
            this.textskin_ip.SkinTxt.Size = new System.Drawing.Size(146, 20);
            this.textskin_ip.SkinTxt.TabIndex = 0;
            this.textskin_ip.SkinTxt.Text = "192.168.0.106";
            this.textskin_ip.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.textskin_ip.SkinTxt.WaterText = "";
            this.textskin_ip.TabIndex = 1;
            this.textskin_ip.Text = "192.168.0.106";
            this.textskin_ip.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textskin_ip.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.textskin_ip.WaterText = "";
            this.textskin_ip.WordWrap = true;
            // 
            // buttonskin_connect
            // 
            this.buttonskin_connect.BackColor = System.Drawing.Color.Transparent;
            this.buttonskin_connect.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.buttonskin_connect.DownBack = null;
            this.buttonskin_connect.Location = new System.Drawing.Point(12, 113);
            this.buttonskin_connect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonskin_connect.MouseBack = null;
            this.buttonskin_connect.Name = "buttonskin_connect";
            this.buttonskin_connect.NormlBack = null;
            this.buttonskin_connect.Size = new System.Drawing.Size(160, 32);
            this.buttonskin_connect.TabIndex = 0;
            this.buttonskin_connect.Text = "Connect";
            this.buttonskin_connect.UseVisualStyleBackColor = false;
            this.buttonskin_connect.Click += new System.EventHandler(this.buttonskin_connect_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "9.png");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1307, 545);
            this.Controls.Add(this.panel_back);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel_back.ResumeLayout(false);
            this.panel_back.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_back;
        private CCWin.SkinControl.SkinButton buttonskin_connect;
        private CCWin.SkinControl.SkinTextBox textSkin_port;
        private CCWin.SkinControl.SkinTextBox textskin_ip;
        private CCWin.SkinControl.SkinButton btnskin_getLaser;
        private CCWin.SkinControl.SkinButton buttonskin_getPosi;
        private System.Windows.Forms.TextBox textBox_display;
        private CCWin.SkinControl.SkinButton buttonskin_getMap;
        private System.Windows.Forms.PictureBox pictureBox1;
        private CCWin.SkinControl.SkinButton skinButton1;
        private CCWin.SkinControl.SkinTextBox skinTextBox1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private CCWin.SkinControl.SkinButton skinButton2;
    }
}

