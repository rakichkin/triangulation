using Triangulation.Primitives;

using MathNet.Numerics.LinearAlgebra.Double;


namespace Triangulation;

public static class Triangulation
{
	public static PointD Triangulate(List<TriangulationUnit> triangulationPoints)
	{
		if(triangulationPoints.Count < 3) throw new ArgumentException("Count of points should be more or equals than 3");
		PointD point;

		if(triangulationPoints.Count == 3) point = Triangulate3Points(triangulationPoints);
		else point = TriangulateManyPoints(triangulationPoints);

		return point;
	}

	private static PointD Triangulate3Points(List<TriangulationUnit> triangulationPoints)
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

	private static PointD TriangulateManyPoints(List<TriangulationUnit> points)
	{
		if(points == null || points.Count < 3)
			throw new ArgumentException("At least three points are required.");

		// Build the A and b matrices
		var A = DenseMatrix.Create(points.Count - 1, 2, 0.0);
		var b = DenseMatrix.Create(points.Count - 1, 1, 0.0);

		for(int i = 1; i < points.Count; i++)
		{
			var point = points[i];
			var dx = point.Point.X - points[0].Point.X;
			var dy = point.Point.Y - points[0].Point.Y;
			var distanceDiff = Math.Pow(point.Distance, 2) - Math.Pow(points[0].Distance, 2);

			A[i - 1, 0] = 2 * dx;
			A[i - 1, 1] = 2 * dy;
			b[i - 1, 0] = dx * dx + dy * dy - distanceDiff;
		}

		// Perform QR factorization
		var qr = A.QR();

		// Solve the linear system
		var x = qr.Solve(b);

		// Calculate the estimated location
		PointD calculatedLocation = new PointD
		{
			X = points[0].Point.X + x[0, 0],
			Y = points[0].Point.Y + x[1, 0]
		};

		return calculatedLocation;
	}
}
