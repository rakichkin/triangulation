using Newtonsoft.Json.Linq;

namespace Triangulation.KalmanFilter;

public class PositionProcessor
{
    private KalmanFilter filter;

    public PositionProcessor()
    {
        filter = new KalmanFilter(0.01, 0.1, 0, 0, 1, 1);
    }

    public Position ProcessPosition(Position pos)
    {
        filter.Update(pos.X, pos.Y);
        double filteredX = filter.X;
        double filteredY = filter.Y;

        Position filteredPos = new Position();
        filteredPos.X = filteredX;
        filteredPos.Y = filteredY;
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
}
