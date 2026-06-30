using System;
using System.Windows.Forms;

namespace PavilCanyonTelemetry;

internal static class Program
{
    public static MainForm ourForm = new();

    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        // ourForm = new MainForm();

        Application.Run(ourForm);
    }
}
