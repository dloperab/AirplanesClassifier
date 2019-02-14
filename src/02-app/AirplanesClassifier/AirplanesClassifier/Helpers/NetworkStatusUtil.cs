using Xamarin.Essentials;

namespace AirplanesClassifier.Helpers
{
  public static class NetworkStatusUtil
  {
    public static bool IsOnline
    {
      get
      {
        if (Connectivity.NetworkAccess == NetworkAccess.Internet ||
            Connectivity.NetworkAccess == NetworkAccess.Local)
          return true;

        return false;
      }
    }
  }
}
