﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AirplanesClassifier.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AirplanesClassifier
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();

      MainPage = new NavigationPage(new ClassifierView())
      {
        BarBackgroundColor = Color.FromHex("#000020"),
        BarTextColor = Color.White
      };
    }

    protected override void OnStart()
    {
      // Handle when your app starts
    }

    protected override void OnSleep()
    {
      // Handle when your app sleeps
    }

    protected override void OnResume()
    {
      // Handle when your app resumes
    }
  }
}
