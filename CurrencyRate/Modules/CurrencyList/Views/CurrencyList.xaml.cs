using CurrencyRate.Modules.CurrencyList.ViewModels;

namespace CurrencyRate.Modules.CurrencyList.Views
{
	public partial class CurrencyList
	{
		public CurrencyList(CurrencyListViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}
	}
}
