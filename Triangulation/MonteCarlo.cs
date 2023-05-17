using Triangulation.Primitives;

namespace Triangulation;


public class MonteCarlo
{
	/// <summary>Standard deviation of the Gaussian noise</summary>
	public double StdDev { get; set; }

	public int Iterations { get; set; }

    public MonteCarlo(double standardDeviation = 0.05, int iterations = 1000)
    {
		StdDev = standardDeviation;
		Iterations = iterations;
	}

    public List<Position> Run(List<Position> rawPositions)
	{
		var rand = new Random();

		var filteredData = new List<Position>();
		foreach(var pos in rawPositions) 
		{
			var minDistance = double.MaxValue;

			double newX = 0.0;
			double newY = 0.0;

			// Generate 1000 random points around the given coordinate
			Parallel.For(0, Iterations, _ =>
			{
				double rx = pos.Point.X + rand.NextDouble() * StdDev * 2 - StdDev;
				double ry = pos.Point.Y + rand.NextDouble() * StdDev * 2 - StdDev;

				// Calculate the distance between the generated point and the given coordinate
				double distance = Math.Sqrt(Math.Pow(rx - pos.Point.X, 2) + Math.Pow(ry - pos.Point.Y, 2));

				// Keep track of the point with the minimum distance
				if(distance < minDistance)
				{
					minDistance = distance;
					newX = rx;
					newY = ry;
				}
			});

			// Add the corrected coordinate to the list
			filteredData.Add(new()
			{
				Point = new(newX, newY),
				Timestamp = pos.Timestamp
			});
		}

		return filteredData;
	}
}