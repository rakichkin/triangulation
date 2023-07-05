using Algorithms;
using Algorithms.Primitives;

using Newtonsoft.Json;


namespace Testing;

public class Program
{
	private static Configuration _settings;

	static void Main(string[] args)
	{
		_settings = Configuration.GetConfiguration();
		string _pathToDataDirectory = Path.Combine("etc", "data");
		string _pathToAllRawDistances = Path.Combine(_pathToDataDirectory, "movements_short.json");
		if(!Directory.Exists(_pathToDataDirectory)) 
			Directory.CreateDirectory(_pathToDataDirectory);
		

		var allRawDistances = 
			JsonConvert.DeserializeObject<List<List<double>>>(File.ReadAllText(_pathToAllRawDistances));

		var staticPoints = ReadStaticPoints(_pathToDataDirectory);


		// Производим триангуляцию на сырых неотфильтрованных данных
		var trilateratedUnfilteredPoints = Trilaterate(allRawDistances, staticPoints);
		WritePointsResult(trilateratedUnfilteredPoints, "unfiltered_triangulation.json");

		// Производим триангуляцию на данных, отфильтрованных фильтром Калмана
		var filteredKalmanDistances = FilterResultsKalman(allRawDistances);
		var trilateratedKalmanPoints = Trilaterate(filteredKalmanDistances, staticPoints);
		WritePointsResult(trilateratedKalmanPoints, "kalman_triangulation.json");

		// Производим триангуляцию на данных, отфильтрованных фильтром Монте-Карло
		var filteredMonteCarloResults = FilterResultsMonteCarlo(allRawDistances);
		var trilateratedMonteCarloResults = Trilaterate(filteredMonteCarloResults, staticPoints);
		WritePointsResult(trilateratedMonteCarloResults, "monte_carlo_triangulation.json");
	}

	private static void WritePointsResult(List<PointD> trilateratedKalmanResults, string fileName)
		=> File.WriteAllText(Path.Combine("etc", "data", fileName),
			JsonConvert.SerializeObject(trilateratedKalmanResults));


	private static List<PointD> ReadStaticPoints(string scriptDirectory)
	{
		var rawStaticPoints = _settings.StaticPoints;

		var staticPoints = new List<PointD>();
		foreach(var rawPoint in rawStaticPoints)
		{
			staticPoints.Add(new PointD(rawPoint[0], rawPoint[1]));
		}

		return staticPoints;
	}

	private static List<List<double>> FilterResultsKalman(List<List<double>> allRawDistances)
	{
		var filters = new List<KalmanFilter>();
		for(int i = 0; i < allRawDistances[0].Count; i++) // Количество фильтров равно количеству статичных точек
		{
			filters.Add(new KalmanFilter((double) _settings.Kalman["initial_error_covariance"],
				(double) _settings.Kalman["process_noize"],
				(double) _settings.Kalman["measurement_noize"]));
		}

		var filteredResults = new List<List<double>>();
		for(int i = 0; i < allRawDistances!.Count; i++)
		{
			filteredResults.Add(new List<double>());
			for(int j = 0; j < allRawDistances[i].Count; j++)
			{
				filteredResults[i].Add(filters[j].Filter(allRawDistances[i][j]));
			}
		}

		return filteredResults;
	}

	private static List<PointD> Trilaterate(List<List<double>> distances, List<PointD> staticPoints)
	{
		var result = new List<PointD>();

		foreach(var distancesAtTime in distances)
		{
			var triangUnitsList = new List<TriangulationUnit>();
			for(int i = 0; i < distancesAtTime.Count(); i++)
			{
				triangUnitsList.Add(new TriangulationUnit(staticPoints[i], distancesAtTime[i]));
			}
			result.Add(Triangulation.Triangulate(triangUnitsList));
		}

		return result;
	}

	private static List<List<double>> FilterResultsMonteCarlo(List<List<double>> allRawDistances)
	{
		var filter = new MonteCarloFilter((double)_settings.MonteCarlo["std_dev"],
			(int)_settings.MonteCarlo["iteraions"]);

		var filteredResults = new List<List<double>>();
		for(int i = 0; i < allRawDistances!.Count; i++)
		{
			filteredResults.Add(new List<double>());
			for(int j = 0; j < allRawDistances[i].Count; j++)
			{
				filteredResults[i].Add(filter.Filter(allRawDistances[i][j]));
			}
		}

		return filteredResults;
	}
}