using System.Runtime.InteropServices;
using Ezvpn.Core.Interop;
using Microsoft.UI.Xaml;

namespace Ezvpn.App;

/// <summary>Application entry point.</summary>
public partial class App : Application
{
    private const uint MbIconError = 0x10;

    private Window? _window;

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        // Fail fast if the native DLLs aren't beside the app: without ezvpn.dll
        // the process would crash on the first P/Invoke with no window; without
        // wintun.dll the window would open but Connect would fail later. Surface a
        // clear message up front instead.
        try
        {
            EzvpnSession.EnsureNativeDependencies();
        }
        catch (EzvpnException ex)
        {
            MessageBox(IntPtr.Zero, ex.Message, "ezvpn cannot start", MbIconError);
            Exit();
            return;
        }

        // Route the Rust core's logs to stderr (honors RUST_LOG).
        EzvpnSession.InitLogging();

        _window = new MainWindow();
        _window.Activate();
    }

    [LibraryImport("user32.dll", EntryPoint = "MessageBoxW", StringMarshalling = StringMarshalling.Utf16)]
    private static partial int MessageBox(IntPtr hWnd, string text, string caption, uint type);
}
