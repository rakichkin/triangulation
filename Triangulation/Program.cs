using Newtonsoft.Json;

using Triangulation.Primitives;
using Triangulation.Tools;


namespace Triangulation;

public class Program
{
	static void Main(string[] args)
	{
		//WriteJson();
		TrilaterationExample();
		//Kalman();
		//MonteCarlo();
	}

	private static void Kalman()
	{
		var dir = @"F:\Severstal\Triangulation\scripts\points_visualizer\";
	}

	private static void MonteCarlo()
	{
		var dir = @"F:\Severstal\Triangulation\scripts\points_visualizer\";

		var src = JsonConvert.DeserializeObject<List<Position>>(
			File.ReadAllText(dir + "gps_data.json"));
		var filteredResults = new MonteCarlo(.05, 100000).Run(src);

		File.WriteAllText(dir + "monte_carlo_results.json", JsonConvert.SerializeObject(filteredResults));
	}

	private static void TrilaterationExample()
	{
		var circlesDir = "F:\\Severstal\\Triangulation\\scripts\\circles_visualizer\\";

		var trilPointsStr = File.ReadAllText($"{circlesDir}\\trilateration_points.json");
		var trilPoints = JsonConvert.DeserializeObject<List<TriangulationPoint>>(trilPointsStr);

		var result = Triangulation.Triangulate(trilPoints!);
		File.WriteAllText($"{circlesDir}\\desired_point.json", JsonConvert.SerializeObject(result));
	}

	private static void WriteJson()
	{
		var movement = CsvDataConverter.Convert("C:\\Users\\igors\\Desktop\\20230518_track\\track_points.csv");

		File.WriteAllText("F:\\Triangulation\\scripts\\points_visualizer\\gps_data.json",
			JsonConvert.SerializeObject(movement));
	}
}