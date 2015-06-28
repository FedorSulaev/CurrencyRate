﻿using System;

namespace CurrencyRate.Model
{
	public class CurrencyData
	{
		public DateTime Date { get; set; }

		public string CharCode { get; set; }

		public string NumCode { get; set; }

		public string Name { get; set; }

		public int Nominal { get; set; }

		public decimal Value { get; set; }
	}
}
