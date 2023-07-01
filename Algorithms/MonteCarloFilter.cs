namespace Algorithms;

public class MonteCarloFilter
{
	/// <summary>Standard deviation of the Gaussian noise</summary>
	public double StdDev { get; set; }
	public int Iterations { get; set; }

	public MonteCarloFilter(double standardDeviation = 0.05, int iterations = 1000)
	{
		StdDev = standardDeviation;
		Iterations = iterations;
	}

	public double Filter(double rawDistance)
	{
		var rand = new Random();
		var minDistance = double.MaxValue;

		// Generate 1000 random distances around the given distance
		Parallel.For(0, Iterations, _ =>
		{
			double distance = rawDistance + (rand.NextDouble() * StdDev * 2) - StdDev;

			// Keep track of the distance with the minimum difference
			if(Math.Abs(distance - rawDistance) < minDistance)
			{
				minDistance = Math.Abs(distance - rawDistance);
			}
		});

		// Return the filtered distance
		return rawDistance + minDistance;
	}
}