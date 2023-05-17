using System;
using System.Drawing;
using Triangulation.KalmanFilter;

namespace Triangulation;

public class MonteCarlo
{
	private readonly Random _rand = new();

	public double Error { get; set; } = 0.0001;

	// Функция для генерации случайной погрешности
	private double GenerateError()
	{
		return _rand.NextDouble() * Error; // Можно изменять значение погрешности в зависимости от нужд
	}

	// Функция для исправления ошибок в данных трекера
	public PointD CorrectData(PointD data)
	{
		var errorX = GenerateError();
		var errorY = GenerateError();

		var correctedData = new PointD(data.X + errorX, data.Y + errorY);

		return correctedData;
	}
}

public class MonteCarlo_v2
{
	/// <summary>Количество итераций.</summary>
	public int Iterations { get; set; }

	/// <summary>Диапазон шума.</summary>
	public double NoiseRange { get; set; }

    public MonteCarlo_v2(int iterations = 10000, double nosieRange = 0.1)
	{
		Iterations = iterations;
		NoiseRange = nosieRange;
	}

	public List<Position> Correct(List<Position> data)
	{
		var random = new Random();
		var correctedData = new List<Position>();

		foreach(var position in data)
		{
			double sumX = 0;
			double sumY = 0;

			Parallel.For(0, Iterations, (i) =>
			{
				double randomNoiseX = GenerateRandomNoise(NoiseRange, random);
				double randomNoiseY = GenerateRandomNoise(NoiseRange, random);

				double candidateX = position.Point.X + randomNoiseX;
				double candidateY = position.Point.Y + randomNoiseY;

				// Вычисление оценки функции правдоподобия для кандидата
				double likelihood = ComputeLikelihood(position.Point, candidateX, candidateY);

				sumX += candidateX * likelihood;
				sumY += candidateY * likelihood;
			});

			double correctedValueX = position.Point.X + sumX / Iterations;
			double correctedValueY = position.Point.Y + sumY / Iterations;

			correctedData.Add(new Position
			{
				Point = new PointD(correctedValueX, correctedValueY),
				Timestamp = position.Timestamp
			});
		}

		return correctedData;
	}

	private double ComputeLikelihood(PointD observedPoint, double candidateX, double candidateY)
	{
		// Гауссова функция правдоподобия
		double exponentX = -Math.Pow(candidateX - observedPoint.X, 2) / (2 * Math.Pow(NoiseRange, 2));
		double exponentY = -Math.Pow(candidateY - observedPoint.Y, 2) / (2 * Math.Pow(NoiseRange, 2));
		double likelihoodX = Math.Exp(exponentX) / (Math.Sqrt(2 * Math.PI) * NoiseRange);
		double likelihoodY = Math.Exp(exponentY) / (Math.Sqrt(2 * Math.PI) * NoiseRange);
		return likelihoodX * likelihoodY;
	}

	private double GenerateRandomNoise(double noiseRange, Random random)
	{
		return (random.NextDouble() * 2 - 1) * noiseRange;
	}
}

public class MonteCarlo_v3
{
	/// <summary>Количество итераций.</summary>
	public int Iterations { get; set; }

	/// <summary>Диапазон шума.</summary>
	public double NoiseRange { get; set; }

	public MonteCarlo_v3(int iterations = 100000, double nosieRange = 0.001)
	{
		Iterations = iterations;
		NoiseRange = nosieRange;
	}

	public List<Position> Correct(List<Position> data)
	{
		Random random = new Random();
		var correctedData = new List<Position>();

		Parallel.For(0, data.Count, i =>
		{
			double sumX = 0.0;
			double sumY = 0.0;

			Parallel.For(0, Iterations, (j) =>
			{
				// Генерируем случайное значение согласно Гауссовому распределению
				double randomValueX = random.NextDouble();
				double randomValueY = random.NextDouble();

				double noiseX = RandomGaussianValue(randomValueX, NoiseRange);
				double noiseY = RandomGaussianValue(randomValueY, NoiseRange);

				sumX += data[i].Point.X + noiseX;
				sumY += data[i].Point.Y + noiseY;
			});

			// Вычисляем среднее значение
			double correctedX = sumX / Iterations;
			double correctedY = sumY / Iterations;

			correctedData.Add(new Position
			{
				Point = new PointD(correctedX, correctedY),
				Timestamp = data[i].Timestamp
			});
		});
		// Исправляем данные
	
		return correctedData;
	}

	private double RandomGaussianValue(double randomValue, double standardDeviation)
	{
		// Вместо генерации двух разных случайных значений u1 и u2, исправляем на random.NextDouble()
		double u1 = randomValue;
		double u2 = new Random().NextDouble();
		double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
		double randGaussian = standardDeviation * randStdNormal;
		return randGaussian;
	}
}

class ParticleFilter
{
	private const int numParticles = 1000;
	private Particle[] particles;
	private Random random = new Random();

	public ParticleFilter()
	{
		particles = new Particle[numParticles];

		// Инициализация частиц со случайными значениями
		for(int i = 0; i < numParticles; i++)
		{
			double x = random.NextDouble() * 10.0; // Диапазон значений для x: 0-10
			double y = random.NextDouble() * 10.0; // Диапазон значений для y: 0-10
			particles[i] = new Particle(x, y);
		}
	}

	public void Update(double measurementX, double measurementY)
	{
		// Обновление весов частиц на основе измерения
		for(int i = 0; i < numParticles; i++)
		{
			particles[i].X += NextGaussian() * 0.1; // Шум движения частицы по оси X
			particles[i].Y += NextGaussian() * 0.1; // Шум движения частицы по оси Y
			double weight = CalculateWeight(particles[i], measurementX, measurementY);
			particles[i].Weight = weight;
		}

		// Нормализация весов частиц
		double sumWeights = particles.Sum(p => p.Weight);
		for(int i = 0; i < numParticles; i++)
		{
			particles[i].Weight /= sumWeights;
		}
	}

	public PointD Estimate()
	{
		// Вычисление оценки позиции на основе взвешенных значений частиц
		double estimateX = particles.Sum(p => p.X * p.Weight);
		double estimateY = particles.Sum(p => p.Y * p.Weight);
		return new PointD(estimateX, estimateY);
	}

	public List<Position> Get(List<Position> positions)
	{
		var filteredPositions = new List<Position>();
		foreach(var position in positions)
		{
			Update(position.Point.X, position.Point.Y);
			filteredPositions.Add(new()
			{
				Point = Estimate(),
				Timestamp = position.Timestamp,
			});
		}

		return filteredPositions;
	}

	private double CalculateWeight(Particle particle, double measurementX, double measurementY)
	{
		// Расчет веса частицы на основе расстояния между частицей и измерением
		double distanceSquared = Math.Pow(particle.X - measurementX, 2) + Math.Pow(particle.Y - measurementY, 2);
		double weight = Math.Exp(-distanceSquared / (2.0 * 0.1 * 0.1)); // Гауссово распределение
		return weight;
	}

	// Вспомогательная функция для генерации случайного значения с нормальным распределением (алгоритм Бокса-Мюллера)
	private double NextGaussian()
	{
		// Используется алгоритм Бокса-Мюллера для генерации случайного значения с нормальным распределением
		double u1 = 1.0 - random.NextDouble();
		double u2 = 1.0 - random.NextDouble();
		double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
		return randStdNormal;
	}
}

class Particle
{
	public double X { get; set; }
	public double Y { get; set; }
	public double Weight { get; set; }

    public Particle()
    {
        
    }

    public Particle(double x, double y)
	{
		X = x;
		Y = y;
		Weight = 0;
	}
}

class ParticleFilter2
{
	private List<Particle> particles;
	private Random random;

	public ParticleFilter2(int numParticles)
	{
		particles = GenerateParticles(numParticles);
		random = new Random();
	}

	private List<Particle> GenerateParticles(int numParticles)
	{
		List<Particle> particles = new List<Particle>();

		for(int i = 0; i < numParticles; i++)
		{
			particles.Add(new Particle
			{
				X = random.NextDouble() * 100,
				Y = random.NextDouble() * 100,
				Weight = 0
			});
		}

		return particles;
	}

	private void SimulateMovement(Particle particle)
	{
		particle.X += random.NextDouble() * 10 - 5; // Random value between -5 and 5
		particle.Y += random.NextDouble() * 10 - 5; // Random value between -5 and 5
	}

	private void CalculateWeight(Particle particle, double measuredX, double measuredY)
	{
		double distance = CalculateDistance(particle.X, particle.Y, measuredX, measuredY);
		particle.Weight = 1 / (distance + 1); // Inverse distance as weight
	}

	private List<Particle> ResampleParticles(List<Particle> particles)
	{
		List<Particle> resampledParticles = new List<Particle>();
		double totalWeight = 0;

		foreach(var particle in particles)
		{
			totalWeight += particle.Weight;
		}

		for(int i = 0; i < particles.Count; i++)
		{
			double randomWeight = random.NextDouble() * totalWeight;
			double cumulativeWeight = 0;

			foreach(var particle in particles)
			{
				cumulativeWeight += particle.Weight;

				if(cumulativeWeight >= randomWeight)
				{
					resampledParticles.Add(new Particle
					{
						X = particle.X,
						Y = particle.Y,
						Weight = 0
					});
					break;
				}
			}
		}

		return resampledParticles;
	}

	private double CalculateDistance(double x1, double y1, double x2, double y2)
	{
		double dx = x1 - x2;
		double dy = y1 - y2;
		return Math.Sqrt(dx * dx + dy * dy);
	}

	public List<Particle> FilterData(List<double> measuredXList, List<double> measuredYList)
	{
		if(measuredXList.Count != measuredYList.Count)
		{
			throw new ArgumentException("The number of measured X and Y coordinates should be the same.");
		}

		int numMeasurements = measuredXList.Count;
		int numParticles = particles.Count;

		for(int i = 0; i < numMeasurements; i++)
		{
			double measuredX = measuredXList[i];
			double measuredY = measuredYList[i];

			foreach(var particle in particles)
			{
				SimulateMovement(particle);
				CalculateWeight(particle, measuredX, measuredY);
			}

			particles = ResampleParticles(particles);
		}

		return particles;
	}
	public (double X, double Y) GetFilteredPosition()
	{
		double sumX = 0;
		double sumY = 0;
		double totalWeight = 0;

		foreach(var particle in particles)
		{
			sumX += particle.X * particle.Weight;
			sumY += particle.Y * particle.Weight;
			totalWeight += particle.Weight;
		}

		double filteredX = sumX / totalWeight;
		double filteredY = sumY / totalWeight;

		return (filteredX, filteredY);
	}
}
