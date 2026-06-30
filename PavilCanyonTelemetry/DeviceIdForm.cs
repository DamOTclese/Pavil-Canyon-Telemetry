using System;
using System.Windows.Forms;

namespace PavilCanyonTelemetry;

public partial class DeviceIdForm : Form
{
    public string DeviceId => txtDeviceId.Text.Trim();

    public DeviceIdForm(string current)
    {
        InitializeComponent();
        txtDeviceId.Text = current ?? string.Empty;
        txtDeviceId.SelectAll();
    }

    private void btnOk_Click(object sender, EventArgs e) { DialogResult = DialogResult.OK; Close(); }
    private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
}
