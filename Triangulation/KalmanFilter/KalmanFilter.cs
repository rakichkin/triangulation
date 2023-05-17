namespace Triangulation.KalmanFilter;

public class KalmanFilter
{
    public double X { get; private set; }
    public double Y { get; private set; }

    private double varX;
    private double varY;
    private double processNoise;
    private double measurementNoise;

    public KalmanFilter(double processNoise, double measurementNoise, double x, double y, double varX, double varY)
    {
        X = x;
        Y = y;

        this.processNoise = processNoise;
        this.measurementNoise = measurementNoise;
        this.varX = varX;
        this.varY = varY;
    }

    public void Update(double newX, double newY)
    {
        double kx = varX / (varX + measurementNoise);
        double ky = varY / (varY + measurementNoise);

        X = X + kx * (newX - X);
        Y = Y + ky * (newY - Y);

        varX = (1 - kx) * varX + processNoise;
        varY = (1 - ky) * varY + processNoise;
    }
}
