using System.Globalization;
using Triangulation.Primitives;


namespace Triangulation.Tools;

public static class CsvDataConverter
{
	public static List<Position> Convert(string pathToCsv)
	{
		var movement = new List<Position>();

		List<string> linesFromFile = File.ReadAllText(pathToCsv).Split("\n").ToList();
		linesFromFile.RemoveAt(0);

		foreach(var line in linesFromFile)
		{
			string[] data = line.Split(",");
			if(data.Length <= 1 ) continue;

			var x = double.Parse(data[0], CultureInfo.InvariantCulture);
			var y = double.Parse(data[1], CultureInfo.InvariantCulture);
			var date = DateTime.Parse(data[6]).AddYears(2000);

			movement.Add(new()
			{
				Point = new(x, y),
				Timestamp = date
			});
		}

		return movement;
	}
}
