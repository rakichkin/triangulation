using Newtonsoft.Json;

using Triangulation.KalmanFilter;

namespace Triangulation;

internal class Program
{
	static void Main(string[] args)
	{
		var dir = @"F:\Triangulation\scripts\points_visualizer\";

		var src = JsonConvert.
			DeserializeObject<List<Position>>(
			File.ReadAllText(dir + "gps_data.json"));
		var filteredResults = new PositionProcessor().ProcessPositions(src);


		File.WriteAllText(dir + "f.json", JsonConvert.SerializeObject(filteredResults));
	}
}