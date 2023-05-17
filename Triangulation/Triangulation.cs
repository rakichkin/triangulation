using Triangulation.Primitives;


namespace Triangulation;

public static class Triangulation
{
	public static PointD Triangulate(List<TriangulationPoint> triangulationPoints)
	{
		if(triangulationPoints.Count < 3) throw new ArgumentException("Count of points should be more or equals than 3");
		PointD point;

		if(triangulationPoints.Count == 3) point = Triangulate3Points(triangulationPoints);
		else point = TriangulateManyPoints(triangulationPoints);

		return point;
	}

	// Принимает только три точки, работает хорошо
	public static PointD Triangulate3Points(List<TriangulationPoint> triangulationPoints)
	{
		var points = triangulationPoints.Select(tp => tp.Point).ToArray();
		var distances = triangulationPoints.Select(tp => tp.Distance).ToArray();

		// Вычисляем коэффициенты уравнений окружностей
		double a = 2 * (points[0].X - points[2].X);
		double b = 2 * (points[0].Y - points[2].Y);
		double c = Math.Pow(distances[2], 2) - Math.Pow(distances[0], 2) - Math.Pow(points[2].X, 2) + Math.Pow(points[0].X, 2) - Math.Pow(points[2].Y, 2) + Math.Pow(points[0].Y, 2);
		double d = 2 * (points[1].X - points[2].X);
		double e = 2 * (points[1].Y - points[2].Y);
		double f = Math.Pow(distances[2], 2) - Math.Pow(distances[1], 2) - Math.Pow(points[2].X, 2) + Math.Pow(points[1].X, 2) - Math.Pow(points[2].Y, 2) + Math.Pow(points[1].Y, 2);

		// Решаем систему уравнений для нахождения координат неизвестной точки
		double x = (b * f - e * c) / (b * d - e * a);
		double y = (d * c - a * f) / (b * d - e * a);

		return new PointD(x, y);
	}

	// Принимает любое количество точек, но с тремя точками работает так себе (алгоритм Лебо)
	public static PointD TriangulateManyPoints(List<TriangulationPoint> triangulationPoints)
	{
		var points = triangulationPoints.Select(tp => tp.Point).ToArray();
		var distances = triangulationPoints.Select(tp => tp.Distance).ToArray();

		List<PointD> intersections = new List<PointD>();

		for(int i = 0; i < points.Length - 1; i++)
		{
			for(int j = i + 1; j < points.Length; j++)
			{
				double d = distances[i] + distances[j];
				double dx = points[j].X - points[i].X;
				double dy = points[j].Y - points[i].Y;
				double dist = Math.Sqrt(dx * dx + dy * dy);

				// Если окружности пересекаются, добавляем точки пересечения в список
				if(dist <= d)
				{
					double a = (distances[i] * distances[i] - distances[j] * distances[j] + dist * dist) / (2 * dist);
					double h = Math.Sqrt(distances[i] * distances[i] - a * a);

					double cx2 = points[i].X + a * dx / dist;
					double cy2 = points[i].Y + a * dy / dist;

					double intersectionX1 = cx2 + h * dy / dist;
					double intersectionX2 = cx2 - h * dy / dist;
					double intersectionY1 = cy2 - h * dx / dist;
					double intersectionY2 = cy2 + h * dx / dist;

					intersections.Add(new PointD(intersectionX1, intersectionY1));
					intersections.Add(new PointD(intersectionX2, intersectionY2));
				}
			}
		}

		// Создаем список точек пересечения гипербол
		List<PointD> hyperbolaIntersections = new List<PointD>();

		for(int i = 0; i < intersections.Count - 1; i++)
		{
			for(int j = i + 1; j < intersections.Count; j++)
			{
				double dx = intersections[j].X - intersections[i].X;
				double dy = intersections[j].Y - intersections[i].Y;
				double d = Math.Sqrt(dx * dx + dy * dy);

				if(d > Math.Abs(distances[0] - distances[1]) && d < distances[0] + distances[1])
				{
					double a = (distances[0] * distances[0] - distances[1] * distances[1] + d * d) / (2 * d);
					double h = Math.Sqrt(distances[0] * distances[0] - a * a);

					double cx2 = intersections[i].X + a * dx / d;
					double cy2 = intersections[i].Y + a * dy / d;

					double intersectionX1 = cx2 + h * dy / d;
					double intersectionX2 = cx2 - h * dy / d;
					double intersectionY1 = cy2 - h * dx / d;
					double intersectionY2 = cy2 + h * dx / d;

					hyperbolaIntersections.Add(new PointD(intersectionX1, intersectionY1));
					hyperbolaIntersections.Add(new PointD(intersectionX2, intersectionY2));
				}
			}
		}

		// Находим среднее значение координат точек пересечения гипербол
		double sumX = 0;
		double sumY = 0;

		for(int i = 0; i < hyperbolaIntersections.Count; i++)
		{
			sumX += hyperbolaIntersections[i].X;
			sumY += hyperbolaIntersections[i].Y;
		}

		double avgX = sumX / hyperbolaIntersections.Count;
		double avgY = sumY / hyperbolaIntersections.Count;

		return new PointD(avgX, avgY);
	}
}
