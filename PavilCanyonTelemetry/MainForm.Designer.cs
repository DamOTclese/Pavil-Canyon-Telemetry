namespace PavilCanyonTelemetry;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem menuFile;
    private System.Windows.Forms.ToolStripMenuItem menuExit;
    private System.Windows.Forms.ToolStripMenuItem menuConfig;
    private System.Windows.Forms.ToolStripMenuItem menuDeviceId;
    private System.Windows.Forms.SplitContainer splitMain;
    private System.Windows.Forms.GroupBox grpNodes;
    private System.Windows.Forms.FlowLayoutPanel flowNodes;
    private System.Windows.Forms.SplitContainer splitRight;
    private System.Windows.Forms.GroupBox grpDevice;
    private System.Windows.Forms.Label lblDeviceId;
    private System.Windows.Forms.Label lblDeviceIdValue;
    private System.Windows.Forms.Label lblMyNode;
    private System.Windows.Forms.Label lblMyNodeValue;
    private System.Windows.Forms.Label lblHw;
    private System.Windows.Forms.Label lblHwValue;
    private System.Windows.Forms.Label lblFw;
    private System.Windows.Forms.Label lblFwValue;
    private System.Windows.Forms.Label lblLog;
    private System.Windows.Forms.Label lblLogValue;
    private System.Windows.Forms.TextBox txtConsole;
    private System.Windows.Forms.Button btnClear;
    private System.Windows.Forms.Panel panelTop;
    private System.Windows.Forms.ComboBox cmbPorts;
    private System.Windows.Forms.Button btnRefreshPorts;
    private System.Windows.Forms.Button btnConnect;
    private System.Windows.Forms.Button btnDisconnect;
    private System.Windows.Forms.Panel panelSend;
    private System.Windows.Forms.TextBox txtSend;
    private System.Windows.Forms.Label lblSend;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        menuStrip1 = new MenuStrip();
        menuFile = new ToolStripMenuItem();
        menuExit = new ToolStripMenuItem();
        menuConfig = new ToolStripMenuItem();
        menuDeviceId = new ToolStripMenuItem();
        splitMain = new SplitContainer();
        grpNodes = new GroupBox();
        flowNodes = new FlowLayoutPanel();
        splitRight = new SplitContainer();
        grpDevice = new GroupBox();
        lblDeviceId = new Label();
        lblDeviceIdValue = new Label();
        lblMyNode = new Label();
        lblMyNodeValue = new Label();
        lblHw = new Label();
        lblHwValue = new Label();
        lblFw = new Label();
        lblFwValue = new Label();
        lblLog = new Label();
        lblLogValue = new Label();
        panelConsole = new Panel();
        txtConsole = new TextBox();
        btnClear = new Button();
        panelSend = new Panel();
        lblSend = new Label();
        txtSend = new TextBox();
        panelTop = new Panel();
        checkBoxIncludeDebug = new CheckBox();
        checkBoxTextOnly = new CheckBox();
        cmbPorts = new ComboBox();
        btnRefreshPorts = new Button();
        btnConnect = new Button();
        btnDisconnect = new Button();
        menuStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        grpNodes.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitRight).BeginInit();
        splitRight.Panel1.SuspendLayout();
        splitRight.Panel2.SuspendLayout();
        splitRight.SuspendLayout();
        grpDevice.SuspendLayout();
        panelConsole.SuspendLayout();
        panelSend.SuspendLayout();
        panelTop.SuspendLayout();
        SuspendLayout();
        // 
        // menuStrip1
        // 
        menuStrip1.ImageScalingSize = new Size(24, 24);
        menuStrip1.Items.AddRange(new ToolStripItem[] { menuFile, menuConfig });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(2717, 33);
        menuStrip1.TabIndex = 1;
        // 
        // menuFile
        // 
        menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuExit });
        menuFile.Name = "menuFile";
        menuFile.Size = new Size(54, 29);
        menuFile.Text = "File";
        // 
        // menuExit
        // 
        menuExit.Name = "menuExit";
        menuExit.Size = new Size(141, 34);
        menuExit.Text = "Exit";
        menuExit.Click += menuExit_Click;
        // 
        // menuConfig
        // 
        menuConfig.DropDownItems.AddRange(new ToolStripItem[] { menuDeviceId });
        menuConfig.Name = "menuConfig";
        menuConfig.Size = new Size(137, 29);
        menuConfig.Text = "Configuration";
        // 
        // menuDeviceId
        // 
        menuDeviceId.Name = "menuDeviceId";
        menuDeviceId.Size = new Size(189, 34);
        menuDeviceId.Text = "Device ID";
        menuDeviceId.Click += menuDeviceId_Click;
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.Location = new Point(0, 33);
        splitMain.Name = "splitMain";
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(grpNodes);
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(splitRight);
        splitMain.Panel2.Controls.Add(panelTop);
        splitMain.Size = new Size(2717, 1182);
        splitMain.SplitterDistance = 600;
        splitMain.TabIndex = 0;
        // 
        // grpNodes
        // 
        grpNodes.BackColor = Color.Cyan;
        grpNodes.Controls.Add(flowNodes);
        grpNodes.Dock = DockStyle.Fill;
        grpNodes.ForeColor = Color.Black;
        grpNodes.Location = new Point(0, 0);
        grpNodes.Name = "grpNodes";
        grpNodes.Size = new Size(600, 1182);
        grpNodes.TabIndex = 0;
        grpNodes.TabStop = false;
        grpNodes.Text = "Remote Devices";
        // 
        // flowNodes
        // 
        flowNodes.AutoScroll = true;
        flowNodes.BackColor = Color.FromArgb(255, 224, 192);
        flowNodes.Dock = DockStyle.Fill;
        flowNodes.FlowDirection = FlowDirection.TopDown;
        flowNodes.ForeColor = Color.Black;
        flowNodes.Location = new Point(3, 27);
        flowNodes.Name = "flowNodes";
        flowNodes.Size = new Size(594, 1152);
        flowNodes.TabIndex = 0;
        flowNodes.WrapContents = false;
        flowNodes.Paint += flowNodes_Paint;
        // 
        // splitRight
        // 
        splitRight.Dock = DockStyle.Fill;
        splitRight.Location = new Point(0, 88);
        splitRight.Name = "splitRight";
        splitRight.Orientation = Orientation.Horizontal;
        // 
        // splitRight.Panel1
        // 
        splitRight.Panel1.Controls.Add(grpDevice);
        // 
        // splitRight.Panel2
        // 
        splitRight.Panel2.Controls.Add(panelConsole);
        splitRight.Size = new Size(2113, 1094);
        splitRight.SplitterDistance = 202;
        splitRight.TabIndex = 0;
        // 
        // grpDevice
        // 
        grpDevice.BackColor = Color.FromArgb(255, 224, 192);
        grpDevice.Controls.Add(lblDeviceId);
        grpDevice.Controls.Add(lblDeviceIdValue);
        grpDevice.Controls.Add(lblMyNode);
        grpDevice.Controls.Add(lblMyNodeValue);
        grpDevice.Controls.Add(lblHw);
        grpDevice.Controls.Add(lblHwValue);
        grpDevice.Controls.Add(lblFw);
        grpDevice.Controls.Add(lblFwValue);
        grpDevice.Controls.Add(lblLog);
        grpDevice.Controls.Add(lblLogValue);
        grpDevice.Dock = DockStyle.Fill;
        grpDevice.ForeColor = Color.Black;
        grpDevice.Location = new Point(0, 0);
        grpDevice.Name = "grpDevice";
        grpDevice.Size = new Size(2113, 202);
        grpDevice.TabIndex = 0;
        grpDevice.TabStop = false;
        grpDevice.Text = "Device Info";
        // 
        // lblDeviceId
        // 
        lblDeviceId.AutoSize = true;
        lblDeviceId.Location = new Point(10, 25);
        lblDeviceId.Name = "lblDeviceId";
        lblDeviceId.Size = new Size(198, 25);
        lblDeviceId.TabIndex = 0;
        lblDeviceId.Text = "Device ID (Long Name):";
        // 
        // lblDeviceIdValue
        // 
        lblDeviceIdValue.AutoSize = true;
        lblDeviceIdValue.Location = new Point(220, 25);
        lblDeviceIdValue.Name = "lblDeviceIdValue";
        lblDeviceIdValue.Size = new Size(0, 25);
        lblDeviceIdValue.TabIndex = 1;
        // 
        // lblMyNode
        // 
        lblMyNode.AutoSize = true;
        lblMyNode.Location = new Point(10, 50);
        lblMyNode.Name = "lblMyNode";
        lblMyNode.Size = new Size(90, 25);
        lblMyNode.TabIndex = 2;
        lblMyNode.Text = "My Node:";
        // 
        // lblMyNodeValue
        // 
        lblMyNodeValue.AutoSize = true;
        lblMyNodeValue.Location = new Point(150, 50);
        lblMyNodeValue.Name = "lblMyNodeValue";
        lblMyNodeValue.Size = new Size(95, 25);
        lblMyNodeValue.TabIndex = 3;
        lblMyNodeValue.Text = "(unknown)";
        // 
        // lblHw
        // 
        lblHw.AutoSize = true;
        lblHw.Location = new Point(10, 75);
        lblHw.Name = "lblHw";
        lblHw.Size = new Size(46, 25);
        lblHw.TabIndex = 4;
        lblHw.Text = "HW:";
        // 
        // lblHwValue
        // 
        lblHwValue.AutoSize = true;
        lblHwValue.Location = new Point(150, 75);
        lblHwValue.Name = "lblHwValue";
        lblHwValue.Size = new Size(95, 25);
        lblHwValue.TabIndex = 5;
        lblHwValue.Text = "(unknown)";
        // 
        // lblFw
        // 
        lblFw.AutoSize = true;
        lblFw.Location = new Point(10, 100);
        lblFw.Name = "lblFw";
        lblFw.Size = new Size(42, 25);
        lblFw.TabIndex = 6;
        lblFw.Text = "FW:";
        // 
        // lblFwValue
        // 
        lblFwValue.AutoSize = true;
        lblFwValue.Location = new Point(150, 100);
        lblFwValue.Name = "lblFwValue";
        lblFwValue.Size = new Size(95, 25);
        lblFwValue.TabIndex = 7;
        lblFwValue.Text = "(unknown)";
        // 
        // lblLog
        // 
        lblLog.AutoSize = true;
        lblLog.Location = new Point(520, 25);
        lblLog.Name = "lblLog";
        lblLog.Size = new Size(46, 25);
        lblLog.TabIndex = 8;
        lblLog.Text = "Log:";
        // 
        // lblLogValue
        // 
        lblLogValue.AutoSize = true;
        lblLogValue.Location = new Point(560, 25);
        lblLogValue.Name = "lblLogValue";
        lblLogValue.Size = new Size(109, 25);
        lblLogValue.TabIndex = 9;
        lblLogValue.Text = "(not started)";
        // 
        // panelConsole
        // 
        panelConsole.Controls.Add(txtConsole);
        panelConsole.Controls.Add(btnClear);
        panelConsole.Controls.Add(panelSend);
        panelConsole.Dock = DockStyle.Fill;
        panelConsole.Location = new Point(0, 0);
        panelConsole.Name = "panelConsole";
        panelConsole.Size = new Size(2113, 888);
        panelConsole.TabIndex = 0;
        // 
        // txtConsole
        // 
        txtConsole.BackColor = Color.FromArgb(255, 224, 192);
        txtConsole.Dock = DockStyle.Fill;
        txtConsole.Font = new Font("Consolas", 9F);
        txtConsole.ForeColor = Color.Black;
        txtConsole.Location = new Point(0, 0);
        txtConsole.Multiline = true;
        txtConsole.Name = "txtConsole";
        txtConsole.ReadOnly = true;
        txtConsole.ScrollBars = ScrollBars.Vertical;
        txtConsole.Size = new Size(2113, 811);
        txtConsole.TabIndex = 0;
        // 
        // btnClear
        // 
        btnClear.BackColor = Color.FromArgb(192, 255, 255);
        btnClear.Dock = DockStyle.Bottom;
        btnClear.ForeColor = Color.Black;
        btnClear.Location = new Point(0, 811);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(2113, 43);
        btnClear.TabIndex = 1;
        btnClear.Text = "CLEAR";
        btnClear.UseVisualStyleBackColor = false;
        btnClear.Click += btnClear_Click;
        // 
        // panelSend
        // 
        panelSend.Controls.Add(lblSend);
        panelSend.Controls.Add(txtSend);
        panelSend.Dock = DockStyle.Bottom;
        panelSend.Location = new Point(0, 854);
        panelSend.Name = "panelSend";
        panelSend.Padding = new Padding(6);
        panelSend.Size = new Size(2113, 34);
        panelSend.TabIndex = 2;
        // 
        // lblSend
        // 
        lblSend.AutoSize = true;
        lblSend.BackColor = Color.FromArgb(255, 224, 192);
        lblSend.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblSend.ForeColor = Color.Black;
        lblSend.Location = new Point(6, 3);
        lblSend.Name = "lblSend";
        lblSend.Size = new Size(73, 32);
        lblSend.TabIndex = 0;
        lblSend.Text = "Send:";
        // 
        // txtSend
        // 
        txtSend.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtSend.BackColor = Color.FromArgb(255, 224, 192);
        txtSend.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtSend.ForeColor = Color.Black;
        txtSend.Location = new Point(97, 3);
        txtSend.Name = "txtSend";
        txtSend.Size = new Size(1993, 31);
        txtSend.TabIndex = 1;
        txtSend.KeyDown += txtSend_KeyDown;
        // 
        // panelTop
        // 
        panelTop.BackColor = Color.Blue;
        panelTop.Controls.Add(checkBoxIncludeDebug);
        panelTop.Controls.Add(checkBoxTextOnly);
        panelTop.Controls.Add(cmbPorts);
        panelTop.Controls.Add(btnRefreshPorts);
        panelTop.Controls.Add(btnConnect);
        panelTop.Controls.Add(btnDisconnect);
        panelTop.Dock = DockStyle.Top;
        panelTop.Location = new Point(0, 0);
        panelTop.Name = "panelTop";
        panelTop.Padding = new Padding(6);
        panelTop.Size = new Size(2113, 88);
        panelTop.TabIndex = 1;
        // 
        // checkBoxIncludeDebug
        // 
        checkBoxIncludeDebug.AutoSize = true;
        checkBoxIncludeDebug.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
        checkBoxIncludeDebug.Location = new Point(506, 50);
        checkBoxIncludeDebug.Name = "checkBoxIncludeDebug";
        checkBoxIncludeDebug.Size = new Size(180, 34);
        checkBoxIncludeDebug.TabIndex = 5;
        checkBoxIncludeDebug.Text = "Include Debug";
        checkBoxIncludeDebug.UseVisualStyleBackColor = true;
        // 
        // checkBoxTextOnly
        // 
        checkBoxTextOnly.AutoSize = true;
        checkBoxTextOnly.Checked = true;
        checkBoxTextOnly.CheckState = CheckState.Checked;
        checkBoxTextOnly.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
        checkBoxTextOnly.Location = new Point(506, 10);
        checkBoxTextOnly.Name = "checkBoxTextOnly";
        checkBoxTextOnly.Size = new Size(129, 34);
        checkBoxTextOnly.TabIndex = 4;
        checkBoxTextOnly.Text = "Text Only";
        checkBoxTextOnly.UseVisualStyleBackColor = true;
        // 
        // cmbPorts
        // 
        cmbPorts.BackColor = Color.FromArgb(255, 224, 192);
        cmbPorts.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPorts.Location = new Point(2, 18);
        cmbPorts.Name = "cmbPorts";
        cmbPorts.Size = new Size(120, 33);
        cmbPorts.TabIndex = 0;
        // 
        // btnRefreshPorts
        // 
        btnRefreshPorts.BackColor = Color.Cyan;
        btnRefreshPorts.ForeColor = Color.Black;
        btnRefreshPorts.Location = new Point(135, 5);
        btnRefreshPorts.Name = "btnRefreshPorts";
        btnRefreshPorts.Size = new Size(94, 46);
        btnRefreshPorts.TabIndex = 1;
        btnRefreshPorts.Text = "Refresh";
        btnRefreshPorts.UseVisualStyleBackColor = false;
        btnRefreshPorts.Click += btnRefreshPorts_Click;
        // 
        // btnConnect
        // 
        btnConnect.BackColor = Color.Green;
        btnConnect.ForeColor = Color.Black;
        btnConnect.Location = new Point(235, 5);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(90, 46);
        btnConnect.TabIndex = 2;
        btnConnect.Text = "Connect";
        btnConnect.UseVisualStyleBackColor = false;
        btnConnect.Click += btnConnect_Click;
        // 
        // btnDisconnect
        // 
        btnDisconnect.BackColor = Color.Yellow;
        btnDisconnect.Enabled = false;
        btnDisconnect.ForeColor = Color.Black;
        btnDisconnect.Location = new Point(331, 5);
        btnDisconnect.Name = "btnDisconnect";
        btnDisconnect.Size = new Size(118, 46);
        btnDisconnect.TabIndex = 3;
        btnDisconnect.Text = "Disconnect";
        btnDisconnect.UseVisualStyleBackColor = false;
        btnDisconnect.Click += btnDisconnect_Click;
        // 
        // MainForm
        // 
        ClientSize = new Size(2717, 1215);
        Controls.Add(splitMain);
        Controls.Add(menuStrip1);
        ForeColor = Color.FromArgb(192, 255, 255);
        MainMenuStrip = menuStrip1;
        Name = "MainForm";
        Text = "PavilCanyonTelemetry";
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        grpNodes.ResumeLayout(false);
        splitRight.Panel1.ResumeLayout(false);
        splitRight.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitRight).EndInit();
        splitRight.ResumeLayout(false);
        grpDevice.ResumeLayout(false);
        grpDevice.PerformLayout();
        panelConsole.ResumeLayout(false);
        panelConsole.PerformLayout();
        panelSend.ResumeLayout(false);
        panelSend.PerformLayout();
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    public bool GetCheckboxIncludeDebug() { return checkBoxIncludeDebug.Checked; }
    public bool GetCheckboxTextOnly() { return checkBoxTextOnly.Checked; }

    private Panel panelConsole;
    private CheckBox checkBoxIncludeDebug;
    private CheckBox checkBoxTextOnly;
}
