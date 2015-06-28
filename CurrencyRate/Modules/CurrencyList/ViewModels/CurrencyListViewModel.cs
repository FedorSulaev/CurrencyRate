using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CurrencyRate.Modules.BusinessLogic.Services;
using CurrencyRate.Properties;
using CurrencyRate.Utility;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;

namespace CurrencyRate.Modules.CurrencyList.ViewModels
{
	public class CurrencyListViewModel : BindableBase
	{
		public ICommand AddCurrencyCommand { get; private set; }

		public List<string> CurrencyCodes { get; private set; }

		public int SelectedCurrency
		{
			get { return _selectedCurrency; }
			set
			{
				_selectedCurrency = value;
				((DelegateCommand)AddCurrencyCommand).RaiseCanExecuteChanged();
			}
		}

		public DateTime SelectedDate { get; set; }

		public ObservableCollection<CurrencyListElementViewModel> CurrencyListElements { get; private set; }

		private int _selectedCurrency;

		private readonly ApiService _apiService;
		private readonly IEventAggregator _eventAggregator;

		[InjectionConstructor]
		public CurrencyListViewModel(ApiService apiService, IEventAggregator eventAggregator)
		{
			_apiService = apiService;
			_eventAggregator = eventAggregator;
			InitializeProperties();
			InitializeCommands();
			SubscribeToEvents();
		}

		public CurrencyListViewModel()
		{
			InitializeProperties();
			InitializeCommands();
		}

		private void InitializeCommands()
		{
			AddCurrencyCommand = new DelegateCommand(AddCurrency, CanAddCurrency);
		}

		private void InitializeProperties()
		{
			CurrencyListElements = new ObservableCollection<CurrencyListElementViewModel>();
			CurrencyCodes = Settings.Default.CurrencyCodes.Cast<string>().OrderBy(s => s).ToList();
			_selectedCurrency = -1;
			SelectedDate = DateTime.Now;
		}

		private void SubscribeToEvents()
		{
			_eventAggregator.GetEvent<CurrencyListElementClosingEvent>()
				.Subscribe(e => CurrencyListElements.Remove(e));
		}

		private bool CanAddCurrency()
		{
			return SelectedCurrency >= 0;
		}

		private void AddCurrency()
		{
			CurrencyListElements.Insert(0, new CurrencyListElementViewModel(
				_apiService,
				_eventAggregator,
				CurrencyCodes[_selectedCurrency],
				SelectedDate));
		}
	}
}
