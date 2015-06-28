using System.Globalization;
using System.Windows.Controls;

namespace CurrencyRate.Resources.Validation
{
	public class TextToDecimalValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string input = value as string;
			decimal result;
			if (decimal.TryParse(input, out result) && result >= 0)
				return new ValidationResult(true, null);
			return new ValidationResult(false, "Недопустимое значение");
		}
	}
}
