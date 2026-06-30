namespace PavilCanyonTelemetry;

partial class DeviceIdForm
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lbl;
    private System.Windows.Forms.TextBox txtDeviceId;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancel;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.lbl = new System.Windows.Forms.Label();
        this.txtDeviceId = new System.Windows.Forms.TextBox();
        this.btnOk = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.SuspendLayout();

        this.lbl.AutoSize = true;
        this.lbl.Location = new System.Drawing.Point(12, 15);
        this.lbl.Text = "Device ID (Long Name, e.g., FredPavC):";

        this.txtDeviceId.Location = new System.Drawing.Point(12, 40);
        this.txtDeviceId.Size = new System.Drawing.Size(410, 23);

        this.btnOk.Location = new System.Drawing.Point(266, 78);
        this.btnOk.Size = new System.Drawing.Size(75, 27);
        this.btnOk.Text = "OK";
        this.btnOk.Click += new System.EventHandler(this.btnOk_Click);

        this.btnCancel.Location = new System.Drawing.Point(347, 78);
        this.btnCancel.Size = new System.Drawing.Size(75, 27);
        this.btnCancel.Text = "Cancel";
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

        this.AcceptButton = this.btnOk;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(434, 121);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnOk);
        this.Controls.Add(this.txtDeviceId);
        this.Controls.Add(this.lbl);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Device ID";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
