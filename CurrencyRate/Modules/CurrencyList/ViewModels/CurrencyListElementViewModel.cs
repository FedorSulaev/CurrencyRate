using System;
using System.Globalization;
using System.Windows.Input;
using CurrencyRate.Model;
using CurrencyRate.Modules.BusinessLogic.Services;
using CurrencyRate.Utility;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;

namespace CurrencyRate.Modules.CurrencyList.ViewModels
{
	public class CurrencyListElementViewModel : BindableBase
	{
		public ICommand CloseCommand { get; private set; }

		public string CurrencyCode { get; private set; }

		public string CurrencyCodeFieldLabel
		{
			get { return CurrencyCode + ": "; }
		}

		public string CurrencyDescription
		{
			get { return _currencyDescription; }
			private set { SetProperty(ref _currencyDescription, value); }
		}

		public DateTime SelectedDate
		{
			get { return _selectedDate; }
			set
			{
				_selectedDate = value; 
				UpdateCurrencyData();
			}
		}

		public string RateDescription
		{
			get { return _rateDescription; }
			set { SetProperty(ref _rateDescription, value); }
		}

		public string CurAmount
		{
			get { return _curAmount.ToString("N2", CultureInfo.CurrentCulture); }
			set
			{
				decimal newValue = decimal.Parse(value);
				if (newValue == _curAmount)
					return;
				SetProperty(ref _curAmount, newValue);
				_rubAmount = _curAmount * _currencyRate / _nominal;
				OnPropertyChanged(() => RubAmount);
			}
		}

		public string RubAmount
		{
			get { return _rubAmount.ToString("N2", CultureInfo.CurrentCulture); }
			set
			{
				decimal newValue = decimal.Parse(value);
				if (newValue == _rubAmount)
					return;
				SetProperty(ref _rubAmount, newValue);
				_curAmount = _rubAmount / _currencyRate * _nominal;
				OnPropertyChanged(() => CurAmount);
			}
		}

		private decimal _rubAmount;
		private decimal _curAmount;
		private decimal _currencyRate;
		private int _nominal;
		private DateTime _selectedDate;
		private string _rateDescription;
		private string _currencyDescription;

		private readonly ApiService _apiService;
		private readonly IEventAggregator _eventAggregator;

		public CurrencyListElementViewModel()
		{
			CurrencyCode = "USD";
			CurrencyDescription = "Доллар США (код 840)";
			RateDescription = "Курс ЦБ на 01.01.2015 за 10 единиц(у): 500,00";
		}

		public CurrencyListElementViewModel(ApiService apiService, IEventAggregator eventAggregator,
			string currencyCode, DateTime date)
		{
			_apiService = apiService;
			_eventAggregator = eventAggregator;
			InitializeProperties(currencyCode, date);
			InitializeCommands();
		}

		private void InitializeProperties(string currencyCode, DateTime date)
		{
			CurrencyCode = currencyCode;
			SelectedDate = date;
		}

		private void InitializeCommands()
		{
			CloseCommand = new DelegateCommand(Close);
		}

		private void UpdateCurrencyData()
		{
			CurrencyData currencyData = _apiService.GetCurrencyData(CurrencyCode, _selectedDate);
			if (currencyData == null) return;
			CurrencyDescription = string.Format("{0} (код {1})", currencyData.Name, currencyData.NumCode);
			_nominal = currencyData.Nominal;
			_currencyRate = currencyData.Value;
			RateDescription = String.Format("Курс ЦБ на {0} за {1} единиц(у): {2}",
				currencyData.Date.ToShortDateString(), _nominal,
				_currencyRate.ToString("C", CultureInfo.GetCultureInfo("ru-RU")));
			_rubAmount = _curAmount*_currencyRate/_nominal;
			OnPropertyChanged(() => RubAmount);
		}

		private void Close()
		{
			_eventAggregator.GetEvent<CurrencyListElementClosingEvent>().Publish(this);
		}
	}
}
