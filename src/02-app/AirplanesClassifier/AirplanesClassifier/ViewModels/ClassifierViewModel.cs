using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

using Plugin.Media;
using Plugin.Media.Abstractions;

using AirplanesClassifier.Services;
using AirplanesClassifier.Helpers;
using AirplanesClassifier.Models;

namespace AirplanesClassifier.ViewModels
{
  public class ClassifierViewModel : ViewModelBase
  {
    private readonly CloudVisionPredictorService _cloudVisionPredictorService;
    private readonly LocalVisionPredictorService _localVisionPredictorService;

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
      _cloudVisionPredictorService = new CloudVisionPredictorService();
      _localVisionPredictorService = new LocalVisionPredictorService();

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
      AirplaneNameMessage = await BuildMessageAsync(file);
      DeletePhoto(file);
    }

    private async Task<string> BuildMessageAsync(MediaFile file)
    {
      var message = "Necesito una foto de avión";

      try
      {
        if (file != null)
        {
          ImagePrediction bestPrediction = null;
          if (NetworkStatusUtil.IsOnline)
            bestPrediction = await _cloudVisionPredictorService.GetBestTagAsync(file);
          else
            bestPrediction = await _localVisionPredictorService.GetBestTagAsync(file);

          if (bestPrediction == null)
            message = "Avión desconocido :S";
          else
            message = $"Avión: {bestPrediction.Tag} - {bestPrediction.Probability * 100}%";
        }
      }
      catch
      {
        message = "Avión desconocido :S";
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

