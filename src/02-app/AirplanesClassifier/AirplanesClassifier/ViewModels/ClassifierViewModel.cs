using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

using Plugin.Media;
using Plugin.Media.Abstractions;

using AirplanesClassifier.Services;

namespace AirplanesClassifier.ViewModels
{
  public class ClassifierViewModel : ViewModelBase
  {
    private readonly CustomVisionService _customVisionService;

    private string _airplaneNameMessage = string.Empty;
    public string AirplaneNameMessage
    {
      get => _airplaneNameMessage;
      set => Set(ref _airplaneNameMessage, value);
    }

    private bool _canTakePhoto = true;
    public bool CanTakePhoto
    {
      get => _canTakePhoto;
      set
      {
        if (Set(ref _canTakePhoto, value))
          RaisePropertyChanged(nameof(ShowSpinner));
      }
    }

    public bool ShowSpinner => !CanTakePhoto;

    public ICommand TakePhotoCommand { get; }

    public ClassifierViewModel()
    {
      _customVisionService = new CustomVisionService();
      TakePhotoCommand = new Command(async () => await TakePhotoAsync());
    }

    private async Task TakePhotoAsync()
    {
      CanTakePhoto = false;
      await TakePhotoAndBuildMessageAsync();
      CanTakePhoto = true;
    }

    private async Task TakePhotoAndBuildMessageAsync()
    {
      var options = new StoreCameraMediaOptions { PhotoSize = PhotoSize.Medium };
      var file = await CrossMedia.Current.TakePhotoAsync(options);
      AirplaneNameMessage = BuildMessage(file);
      DeletePhoto(file);
    }

    private string BuildMessage(MediaFile file)
    {
      var message = "Se necesita una foto de un avión";

      try
      {
        if (file != null)
        {
          var mostLikely = _customVisionService.GetBestTag(file);
          if (mostLikely == null)
            message = "No conozco ese avión";
          else
            message = $"Avión: {mostLikely.Tag} - {mostLikely.Probability * 100}%";
        }
      }
      catch
      {
        message = "No conozco ese avión";
      }

      return message;
    }

    private static void DeletePhoto(MediaFile file)
    {
      var path = file?.Path;

      if (!string.IsNullOrEmpty(path) && File.Exists(path))
        File.Delete(file?.Path);
    }
  }
}

