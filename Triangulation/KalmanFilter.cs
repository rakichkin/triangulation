namespace Triangulation;

public class KalmanFilter
{
	private int _countOfFiltrations = 0;

	private double _currentDistance;
	private double _currentErrorCovariance;
	private double _processNoise;
	private double _measurementNoise;

	public KalmanFilter(double initialErrorCovariance = 1, double processNoise = 0.1, double measurementNoise = 0.5)
	{
		_currentErrorCovariance = initialErrorCovariance;
		_processNoise = processNoise;
		_measurementNoise = measurementNoise;
	}

	public double Filter(double distance)
	{
		if( _countOfFiltrations++ == 0 ) _currentDistance = distance;

		// Prediction phase
		double predictedDistance = _currentDistance;
		double predictedErrorCovariance = _currentErrorCovariance + _processNoise;

		// Update phase
		double kalmanGain = predictedErrorCovariance / (predictedErrorCovariance + _measurementNoise);
		_currentDistance = predictedDistance + kalmanGain * (distance - predictedDistance);
		_currentErrorCovariance = (1 - kalmanGain) * predictedErrorCovariance;

		return _currentDistance;
	}
}
