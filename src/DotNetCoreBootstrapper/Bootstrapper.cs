namespace DotNetCoreBootstrapper;

using System.Windows.Threading;
using WixToolset.Mba.Core;

public class Bootstrapper : BootstrapperApplication
{
    public readonly IEngine Engine;
    public readonly IBootstrapperCommand BootstrapperCommand;

    public Dispatcher BADispatcher { get; private set; }

    public Bootstrapper(IEngine engine, IBootstrapperCommand bootstrapperCommand)
        : base(engine)
    {
        this.Engine = engine;
        this.BootstrapperCommand = bootstrapperCommand;

        this.BADispatcher = Dispatcher.CurrentDispatcher;

        this.engine.Log(LogLevel.Verbose, "Bootstrapper Constructed");
    }

    protected override void Run()
    {
        this.engine.Log(LogLevel.Verbose, "Running the TestBA.");
        var viewModel = new MainWindowViewModel(this);
        var view = new MainWindow();

        view.DataContext = viewModel;
        view.Closed += (s, e) => this.BADispatcher.InvokeShutdown();
        view.Show();

        var wih = new System.Windows.Interop.WindowInteropHelper(view);
        viewModel.WindowHandle = wih.Handle;

        Dispatcher.Run();

        this.engine.Quit(0);
    }

    protected override void OnStartup(StartupEventArgs args)
    {
        base.OnStartup(args);

        this.engine.Log(LogLevel.Standard, "Bootstrapper OnStartup");
    }

    protected override void OnShutdown(ShutdownEventArgs args)
    {
        base.OnShutdown(args);

        var message = "Shutdown," + args.Action.ToString() + "," + args.HResult.ToString();
        this.engine.Log(LogLevel.Standard, message);
    }
}
