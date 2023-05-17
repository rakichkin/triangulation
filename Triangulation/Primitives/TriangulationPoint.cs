using System.Drawing;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Triangulation.Primitives;

[DataContract]
public record TriangulationPoint
{
    [JsonProperty("point")]
    public PointD Point { get; set; }

    [JsonProperty("distance")]
    public double Distance { get; set; }
}
