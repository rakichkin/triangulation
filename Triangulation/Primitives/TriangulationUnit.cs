using System.Drawing;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Triangulation.Primitives;

[DataContract]
public record TriangulationUnit
{
	public TriangulationUnit(PointD staticPoint, double distanceToMovingPoint)
	{
		Point = staticPoint;
		Distance = distanceToMovingPoint;
	}

	[JsonProperty("point")]
    public PointD Point { get; set; }

    [JsonProperty("distance")]
    public double Distance { get; set; }
}
