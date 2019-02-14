using Windows.UI.Xaml.Navigation;

using Xam.Plugins.OnDeviceCustomVision;

namespace AirplanesClassifier.UWP
{
  public sealed partial class MainPage
  {
    public MainPage()
    {
      this.InitializeComponent();

      LoadApplication(new AirplanesClassifier.App());
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
      await WindowsImageClassifier.Init("AirplanesClassifier", new[] { "Airbus", "Boeing" });

      base.OnNavigatedTo(e);
    }
  }
}
