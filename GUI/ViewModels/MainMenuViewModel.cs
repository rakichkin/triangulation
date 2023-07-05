using GUI.Commands;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.ViewModels
{
	public class MainMenuViewModel : ViewModelBase
	{

		//The overall smoothingCoefficient
		//Initial errorСovariance
		//The ProcessNoise
		//Noise during measurement


		//Общий коэффициент сглаживания
		private double smoothingCoefficient;
		public double SmoothingCoefficient
		{
			get => smoothingCoefficient < 10 ? smoothingCoefficient : smoothingCoefficient = 10;
			set
			{				
				smoothingCoefficient = value < 0 ? 0 : value;
				OnPropertyChanged(nameof(SmoothingCoefficient));
				

				ErrorСovariance  = smoothingCoefficient/2;
				MeasurementNoize = smoothingCoefficient/2;
				ProcessNoise     = smoothingCoefficient/2 - 2;
			}
		}
		

		//Начальная ковариация ошибки                       ?
		private double errorСovariance;
		public double ErrorСovariance
		{
			get => errorСovariance < 10 ? errorСovariance : errorСovariance = 10;
			set
			{
				errorСovariance = value < 0 ? 0 : value;
				OnPropertyChanged(nameof(ErrorСovariance));
			}
		}

		//Процесс шума или шум процесса                      ?
		private double processNoise;
		public double ProcessNoise
		{
			get => processNoise < 10 ? processNoise : processNoise = 10;
			set
			{
				processNoise = value < 0 ? 0 : value;
				OnPropertyChanged(nameof(ProcessNoise));
			}
		}

		//Шум при измерении                                  ?
		private double measurementNoize;
		public double MeasurementNoize
		{
			get => measurementNoize < 10 ? measurementNoize : measurementNoize = 10;
			set
			{
				measurementNoize = value < 0 ? 0 : value;
				OnPropertyChanged(nameof(MeasurementNoize));
			}
		}

		//оповещение об успешном сохранении
		private int saveCompleteOpacity;
		public int SaveCompleteOpacity
		{
			get => saveCompleteOpacity;
			set
			{
				saveCompleteOpacity = value;
				OnPropertyChanged(nameof(SaveCompleteOpacity));

				if(value > 0)
				{
					Task.Delay(1500).ContinueWith(_ => SaveCompleteOpacity = 0);
				}
			}
		}

		public ICommand SaveParametersCommand { get; set; }

		public MainMenuViewModel()
		{
			SaveParametersCommand = new SaveParametersCommand(this);
			saveCompleteOpacity = 0;
			GetConfiguration();
		}

		private void GetConfiguration()
		{
			Algorithms.Configuration configuration = Algorithms.Configuration.GetConfiguration("C:\\ProgramData\\ObjectsPositionVisualization\\TestingGUIKalman\\config.json");

			SmoothingCoefficient = configuration.Kalman["smoothing_coefficient"];
			ErrorСovariance = configuration.Kalman["initial_error_covariance"];
			ProcessNoise = configuration.Kalman["process_noize"];
			MeasurementNoize = configuration.Kalman["measurement_noize"];
		}
	}
}
