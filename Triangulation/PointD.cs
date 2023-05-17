using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Triangulation;

[DataContract]
public struct PointD
{
    public PointD(double x, double y)
    {
        X = x;
        Y = y;
    }

    [JsonProperty("x")]
	public double X { get; set; }

	[JsonProperty("y")]
	public double Y { get; set; }
}
