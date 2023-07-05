using Algorithms;
using GUI.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GUI.Commands
{
	public class SaveParametersCommand : CommandBase
	{
		private MainMenuViewModel _viewModel;

		public SaveParametersCommand(MainMenuViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		public override void Execute(object? parameter)
		{
			Algorithms.Configuration configuration = new Algorithms.Configuration
			{
				Kalman = new
				{
					smoothing_coefficient = _viewModel.SmoothingCoefficient,
					initial_error_covariance = _viewModel.ErrorСovariance,
					process_noize = _viewModel.ProcessNoise,
					measurement_noize = _viewModel.MeasurementNoize
				}
			};

			string json = JsonConvert.SerializeObject(configuration, Formatting.Indented);

			string configFilePath = Path.Combine("C:\\ProgramData\\ObjectsPositionVisualization\\TestingGUIKalman\\config.json");

			File.WriteAllText(configFilePath, json);

			_viewModel.SaveCompleteOpacity = 100;
		}
	}
}
