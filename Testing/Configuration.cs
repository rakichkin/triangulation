using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Testing;

[DataContract]
public record Configuration
{
	[JsonProperty("static_points")]
	public List<List<int>> StaticPoints { get; set; }

	[JsonProperty("kalman")]
	public dynamic Kalman { get; set; }

	[JsonProperty("monte_carlo")]
	public dynamic MonteCarlo { get; set; }
}
