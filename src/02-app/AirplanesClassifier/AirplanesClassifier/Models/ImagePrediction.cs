using System;

namespace AirplanesClassifier.Models
{
  public class ImagePrediction
  {
    public Guid TagId { get; set; } = default(Guid);
    public string Tag { get; set; } = null;
    public double Probability { get; set; } = 0.0;

    public ImagePrediction()
    {
    }

    public ImagePrediction(Guid tagId, string tag, double probability)
    {
      TagId = tagId;
      Tag = tag;
      Probability = probability;
    }
  }
}
