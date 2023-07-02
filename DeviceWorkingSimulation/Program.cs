using Microsoft.EntityFrameworkCore;

using Algorithms;
using Algorithms.Primitives;

using DeviceWorkingSimulation.Contexts;

namespace DeviceWorkingSimulation;

internal class Program
{
	static async Task Main(string[] args)
	{
		var config = Configuration.GetConfiguration();

		using var triangulationContext = new TriangulationContext();
		triangulationContext.Database.ExecuteSql($"delete from Distances");
		await triangulationContext.SaveChangesAsync();

		using var visContext = new VisualizationContext();
		await visContext.Events.ExecuteDeleteAsync();

		visContext.Database.ExecuteSql($"delete from Events");
		await visContext.SaveChangesAsync();

		var filters = new List<KalmanFilter>();
		for(int i = 0; i < 3; i++)
		{
			filters.Add(new((double)config.Kalman["initial_error_covariance"],
				(double)config.Kalman["process_noize"],
				(double)config.Kalman["measurement_noize"]));
		}

		int countOfEntries = triangulationContext.Distances.Count();
		while(true)
		{
			if(triangulationContext.Distances.Count() <= countOfEntries) continue;
			countOfEntries = triangulationContext.Distances.Count();

			var lastEntry = (await triangulationContext.Distances.ToListAsync()).Last();

			var filteredDist1 = filters[0].Filter(lastEntry.Distance1);
			var filteredDist2 = filters[1].Filter(lastEntry.Distance2);
			var filteredDist3 = filters[2].Filter(lastEntry.Distance3);

			var staticPoint1 = new PointD(config.StaticPoints[0][0], config.StaticPoints[0][1]);
			var staticPoint2 = new PointD(config.StaticPoints[1][0], config.StaticPoints[1][1]);
			var staticPoint3 = new PointD(config.StaticPoints[2][0], config.StaticPoints[2][1]);

			var triangUnit1 = new TriangulationUnit(staticPoint1, filteredDist1);
			var triangUnit2 = new TriangulationUnit(staticPoint2, filteredDist2);
			var triangUnit3 = new TriangulationUnit(staticPoint3, filteredDist3);

			var foundPoint = Triangulation.Triangulate(new()
			{
				triangUnit1, triangUnit2, triangUnit3
			});

			visContext.Events.Add(new()
			{
				CreationDate = DateTime.Now,
				ObjectId = 1,
				Tag = "aaa",
				XCoordinate = (float)foundPoint.X,
				YCoordinate = (float)foundPoint.Y
			});
			await visContext.SaveChangesAsync();
		}
	}
}