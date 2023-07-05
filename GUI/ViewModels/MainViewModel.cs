namespace GUI.ViewModels
{
	class MainViewModel : ViewModelBase
	{
		public ViewModelBase CurrentViewModel { get; }

		public MainViewModel()
		{
			CurrentViewModel = new MainMenuViewModel();
		}
	}
}
