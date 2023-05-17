using Newtonsoft.Json;
using System.Linq;
using Triangulation.KalmanFilter;

namespace Triangulation;

public class Program
{
	static void Main(string[] args)
	{
		TrilaterationExample();
		//Kalman();
		//MonteCarlo();
		//ParticleFilter();
	}

	private static void Kalman()
	{
		var dir = @"F:\Triangulation\scripts\points_visualizer\";

		var src = JsonConvert.DeserializeObject<List<Position>>(
			File.ReadAllText(dir + "gps_data.json"));
		var filteredResults = new KalmanFilterProcessor().ProcessPositions(src);

		File.WriteAllText(dir + "kalman_results.json", JsonConvert.SerializeObject(filteredResults));
	}

	private static void MonteCarlo()
	{
		var dir = @"F:\Triangulation\scripts\points_visualizer\";

		var src = JsonConvert.DeserializeObject<List<Position>>(
			File.ReadAllText(dir + "gps_data.json"));
		var filteredResults = new MonteCarlo_v3().Correct(src);

		File.WriteAllText(dir + "monte_carlo_results.json", JsonConvert.SerializeObject(filteredResults));
	}
	
	private static void ParticleFilter()
	{
		var dir = @"F:\Triangulation\scripts\points_visualizer\";

		var src = JsonConvert.DeserializeObject<List<Position>>(
			File.ReadAllText(dir + "gps_data.json"));


		

		//File.WriteAllText(dir + "particle_filter_results.json", JsonConvert.SerializeObject(filteredResults));
	}

	private static void TrilaterationExample()
	{
		var circlesDir = "F:\\Triangulation\\scripts\\circles_visualizer\\";

		var trilPointsStr = File.ReadAllText($"{circlesDir}\\trilateration_points.json");
		var trilPoints = JsonConvert.DeserializeObject<List<TriangulationPoint>>(trilPointsStr);

		var result = Triangulation.TriangulateManyPoints(trilPoints!);
		File.WriteAllText($"{circlesDir}\\desired_point.json", JsonConvert.SerializeObject(result, new PointFJsonConverter()));
	}
}