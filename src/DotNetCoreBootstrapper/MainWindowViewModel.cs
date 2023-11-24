namespace DotNetCoreBootstrapper;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Threading;
using WixToolset.Mba.Core;

public class MainWindowViewModel : ObservableRecipient
{
    private readonly Bootstrapper bootstrapper;
    public IntPtr WindowHandle { get; set; }
    public RelayCommand InstallCommand { get; }
    public RelayCommand CancelCommand { get; }
    public RelayCommand UninstallCommand { get; private set; }

    private bool installEnabled;
    public bool InstallEnabled
    {
        get { return installEnabled; }
        set
        {
            installEnabled = value;
            SetProperty(ref installEnabled, value);
        }
    }

    private bool uninstallEnabled;
    public bool UninstallEnabled
    {
        get { return uninstallEnabled; }
        set
        {
            uninstallEnabled = value;
            SetProperty(ref uninstallEnabled, value);
        }
    }

    public MainWindowViewModel(Bootstrapper bootstrapper)
    {
        this.bootstrapper = bootstrapper;

        this.CancelCommand = new RelayCommand(() => Cancel());
        this.InstallCommand = new RelayCommand(() => Install(), () => InstallEnabled == true);
        this.UninstallCommand = new RelayCommand(() => Uninstall(), () => UninstallEnabled == true);

        this.bootstrapper.ApplyComplete += OnApplyComplete;
        this.bootstrapper.DetectPackageComplete += OnDetectPackageComplete;
        this.bootstrapper.PlanComplete += OnPlanComplete;

        this.bootstrapper.Engine.Detect();
    }

    /// <summary>
    /// Method that gets invoked when the Bootstrapper PlanComplete event is fired.
    /// If the planning was successful, it instructs the Bootstrapper Engine to 
    /// install the packages.
    /// </summary>
    private void OnPlanComplete(object sender, PlanCompleteEventArgs e)
    {
        bootstrapper.Engine.Log(LogLevel.Standard, "MainViewModel Plan Complete");

        if (e.Status >= 0)
            bootstrapper.Engine.Apply(WindowHandle);
    }

    /// <summary>
    /// Method that gets invoked when the Bootstrapper DetectPackageComplete event is fired.
    /// Checks the PackageId and sets the installation scenario. The PackageId is the ID
    /// specified in one of the package elements (msipackage, exepackage, msppackage,
    /// msupackage) in the WiX bundle.
    /// </summary>
    private void OnDetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
    {
        bootstrapper.Engine.Log(LogLevel.Standard, "MainViewModel Detect package Complete: " + e.PackageId);
        if (e.PackageId == "SimpleAppMsi")
        {
            if (e.State == PackageState.Absent)
                InstallEnabled = true;

            else if (e.State == PackageState.Present)
                UninstallEnabled = true;
        }
    }

    private void OnApplyComplete(object sender, ApplyCompleteEventArgs e)
    {
        bootstrapper.Engine.Log(LogLevel.Standard, "MainViewModel Apply Complete: ");
        InstallEnabled = false;
        UninstallEnabled = false;
    }

    private void Cancel()
    {
        Dispatcher.CurrentDispatcher.InvokeShutdown();
    }

    private void Install()
    {
        bootstrapper.Engine.Log(LogLevel.Standard, "MainViewModel Install Execute: ");
        bootstrapper.Engine.Plan(LaunchAction.Install);
    }

    private void Uninstall()
    {
        bootstrapper.Engine.Plan(LaunchAction.Uninstall);
    }
}
