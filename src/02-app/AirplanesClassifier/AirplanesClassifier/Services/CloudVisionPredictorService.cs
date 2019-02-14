using System.Linq;
using System.Threading.Tasks;

using Microsoft.Cognitive.CustomVision.Prediction;

using Plugin.Media.Abstractions;

using AirplanesClassifier.Helpers;
using AirplanesClassifier.Models;

namespace AirplanesClassifier.Services
{
  public class CloudVisionPredictorService
  {
    private PredictionEndpoint _endpoint = new PredictionEndpoint { ApiKey = ApiKeys.PredictionKey };
    private const double ProbabilityThreshold = 0.5;

    public async Task<ImagePrediction> GetBestTagAsync(MediaFile file)
    {
      using (var stream = file.GetStream())
      {
        var predictions = await _endpoint.PredictImageAsync(ApiKeys.ProjectId, stream);
        var prediction = predictions.Predictions.OrderByDescending(p => p.Probability)
                                    .FirstOrDefault(p => p.Probability >= ProbabilityThreshold);

        return new ImagePrediction(prediction.TagId, prediction.Tag, prediction.Probability);
      }
    }
  }
}
