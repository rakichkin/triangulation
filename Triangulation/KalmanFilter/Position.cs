using Newtonsoft.Json;
using System.Drawing;
using System.Runtime.Serialization;

namespace Triangulation.KalmanFilter;

[DataContract]
public record Position
{
    //[JsonProperty("point")]
    //[JsonConverter(typeof(PointFJsonConverter))]
    //public PointF Point { get; set; }

    [JsonProperty("x")]
    public double X { get; set; }

    [JsonProperty("y")]
    public double Y { get; set; }

    [JsonProperty("time")]
    public DateTime Timestamp { get; set; }
}
