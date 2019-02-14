using System;
using System.Linq;
using System.Threading.Tasks;

using Plugin.Media.Abstractions;
using Xam.Plugins.OnDeviceCustomVision;

using AirplanesClassifier.Models;

namespace AirplanesClassifier.Services
{
  public class LocalVisionPredictorService
  {
    private const double ProbabilityThreshold = 0.5;

    public async Task<ImagePrediction> GetBestTagAsync(MediaFile file)
    {
      using (var stream = file.GetStream())
      {
        var predictions = await CrossImageClassifier.Current.ClassifyImage(stream);

        var bestTag = predictions.OrderByDescending(p => p.Probability)
                                 .FirstOrDefault(p => p.Probability >= ProbabilityThreshold);

        return new ImagePrediction(default(Guid), bestTag.Tag, bestTag.Probability);
      }
    }
  }
}
