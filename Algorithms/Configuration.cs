using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Algorithms;

[DataContract]
public record Configuration
{
	[JsonProperty("static_points")]
	public List<List<int>> StaticPoints { get; set; }

	[JsonProperty("kalman")]
	public dynamic Kalman { get; set; }

	[JsonProperty("monte_carlo")]
	public dynamic MonteCarlo { get; set; }

	public static Configuration GetConfiguration(string pathToConfig = null)
	{
		if(pathToConfig != null )
		{
			return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(pathToConfig));
		}
		return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("config.json"));
	}
}
