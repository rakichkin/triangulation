using Newtonsoft.Json.Linq;
using Triangulation.Primitives;

namespace Triangulation.KalmanFilter;

public class KalmanFilterProcessor
{
    private KalmanFilter _filter;

    public KalmanFilterProcessor(double processNoise = 0.01,
        double measurementNoise = 0.1, 
        double x = 0,
        double y  = 0,
        double varX = 1,
        double varY = 1)
    {
		_filter = new KalmanFilter(processNoise, measurementNoise, x, y, varX, varY);
    }

    public Position ProcessPosition(Position pos)
    {
		_filter.Update(pos.Point.X, pos.Point.Y);
        double filteredX = _filter.X;
        double filteredY = _filter.Y;

        Position filteredPos = new Position();
        filteredPos.Point = new(filteredX, filteredY);
        filteredPos.Timestamp = pos.Timestamp;

        return filteredPos;
    }

    public IEnumerable<Position> ProcessPositions(IEnumerable<Position> positions)
    {
        foreach (Position pos in positions)
        {
            yield return ProcessPosition(pos);
        }
    }

    public void ResetFilter(double processNoise, double measurementNoise, double x, double y, double varX, double varY)
    {
        _filter = new KalmanFilter(processNoise, measurementNoise, x, y, varX, varY);
	}
}
