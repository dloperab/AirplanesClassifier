using Xamarin.Essentials;
using Xamarin.Forms;

namespace AirplanesClassifier.Views
{
  public partial class ClassifierView : ContentPage
  {
    public ClassifierView()
    {
      InitializeComponent();
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();

      lblConnectivityStatus.Text = $"Conexión: {Connectivity.NetworkAccess}";
      Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
    }

    protected override void OnDisappearing()
    {
      base.OnDisappearing();

      Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
    }

    void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
      lblConnectivityStatus.Text = $"Conexión: {e.NetworkAccess}";
    }
  }
}
