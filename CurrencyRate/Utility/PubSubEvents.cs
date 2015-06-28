using CurrencyRate.Modules.CurrencyList.ViewModels;
using Microsoft.Practices.Prism.PubSubEvents;

namespace CurrencyRate.Utility
{
	public class CurrencyListElementClosingEvent : PubSubEvent<CurrencyListElementViewModel> { }
}
