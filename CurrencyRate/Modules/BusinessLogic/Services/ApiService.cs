using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using CurrencyRate.Model;
using CurrencyRate.Modules.BusinessLogic.Model;
using CurrencyRate.Properties;

namespace CurrencyRate.Modules.BusinessLogic.Services
{
	public class ApiService
	{
		public CurrencyData GetCurrencyData(string currencyCode, DateTime date)
		{
			string requestUrl = string.Format(@"{0}?date_req={1}",
				Settings.Default.ApiEndpoint,
				date.ToString("dd/MM/yyyy"));
			ApiSingleDateResponse response = GetApiSingleDateResponse(GetResponse(requestUrl));
			if (response == null) return null;
			ApiCurrencyData apiCurrencyData = response.ValCurs.FirstOrDefault(v => v.CharCode == currencyCode);
			if (apiCurrencyData == null) return null;
			CurrencyData currencyData = new CurrencyData
			{
				CharCode = apiCurrencyData.CharCode,
				Date = DateTime.ParseExact(response.Date, "dd/MM/yyyy", CultureInfo.CurrentCulture, 
					DateTimeStyles.None),
				Name = apiCurrencyData.Name,
				Nominal = apiCurrencyData.Nominal,
				NumCode = apiCurrencyData.NumCode,
				Value = decimal.Parse(apiCurrencyData.Value, CultureInfo.CurrentCulture)
			};
			return currencyData;
		}

		private ApiSingleDateResponse GetApiSingleDateResponse(WebResponse webResponse)
		{
			if (webResponse == null) return null;
			try
			{
				ApiSingleDateResponse response;
				XmlSerializer serializer = new XmlSerializer(typeof(ApiSingleDateResponse));
				using (StreamReader reader = new StreamReader(webResponse.GetResponseStream(),
					Encoding.GetEncoding(1251)))
				{
					response = (ApiSingleDateResponse)serializer.Deserialize(reader);
				}
				return response;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return null;
			}
		}

		private WebResponse GetResponse(string requestUrl)
		{
			try
			{
				WebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
				return webRequest.GetResponse();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return null;
			}
		}
	}
}
