using System.Collections.Generic;
using System.Xml.Serialization;

namespace CurrencyRate.Modules.BusinessLogic.Model
{
	public class ApiCurrencyData
	{
		[XmlAttribute("ID")]
		public string ID { get; set; }

		[XmlElement("NumCode")]
		public string NumCode { get; set; }

		[XmlElement("CharCode")]
		public string CharCode { get; set; }

		[XmlElement("Nominal")]
		public int Nominal { get; set; }

		[XmlElement("Name")]
		public string Name { get; set; }

		[XmlElement("Value")]
		public string Value { get; set; }
	}

	[XmlRoot("ValCurs")]
	public class ApiSingleDateResponse
	{
		[XmlAttribute("Date")]
		public string Date { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlElement("Valute")]
		public List<ApiCurrencyData> ValCurs { get; set; }
	}
}
