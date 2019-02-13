using System.Linq;

using Microsoft.Cognitive.CustomVision.Prediction;
using Microsoft.Cognitive.CustomVision.Prediction.Models;

using Plugin.Media.Abstractions;

using AirplanesClassifier.Helpers;

namespace AirplanesClassifier.Services
{
  public class CustomVisionService
  {
    private PredictionEndpoint _endpoint = new PredictionEndpoint { ApiKey = ApiKeys.PredictionKey };
    private const double ProbabilityThreshold = 0.5;

    public ImageTagPredictionModel GetBestTag(MediaFile file)
    {
      using (var stream = file.GetStream())
      {
        var predictions = _endpoint.PredictImage(ApiKeys.ProjectId, stream)
                        .Predictions;

        return predictions.OrderByDescending(p => p.Probability)
                          .FirstOrDefault(p => p.Probability > ProbabilityThreshold);
      }
    }
  }
}
