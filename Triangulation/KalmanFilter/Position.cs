using Newtonsoft.Json;
using System.Drawing;
using System.Runtime.Serialization;

namespace Triangulation.KalmanFilter;

[DataContract]
public record Position
{
    [JsonProperty("point")]
    public PointD Point { get; set; }

    [JsonProperty("date")]
    public DateTime Timestamp { get; set; }
}
