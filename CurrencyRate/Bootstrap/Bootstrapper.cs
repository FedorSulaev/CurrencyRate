using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using CurrencyRate.Modules.BusinessLogic;
using CurrencyRate.Modules.CurrencyList;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;

namespace CurrencyRate.Bootstrap
{
	class Bootstrapper : UnityBootstrapper
	{
		private Mutex _mutex;
		private bool _singleInstance;

		protected override DependencyObject CreateShell()
		{
			return new Shell();
		}

		protected override void InitializeShell()
		{
			base.InitializeShell();
			_mutex = new Mutex(true, "mutex", out _singleInstance);
			CheckIfOnlyInstance();
			Application.Current.MainWindow = (Window) Shell;
			Application.Current.MainWindow.Show();
			Application.Current.DispatcherUnhandledException += OnUnhandledException;
		}

		private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			string path = @"ErrorLog.txt";
			if (!File.Exists(path))
			{
				File.Create(path);
			}
			TextWriter tw = new StreamWriter(path, true);
			tw.WriteLine(DateTime.Now.ToString());
			tw.WriteLine("Exception: {0}", ExceptionToString(e.Exception));
			tw.WriteLine("Inner Exception: {0}", ExceptionToString(e.Exception.InnerException));
			tw.WriteLine();
			tw.Close();
			MessageBox.Show("Произошла непредвиденная ошибка, сообщение сохранено в лог-файл.");
		}

		private string ExceptionToString(Exception exception)
		{
			if (exception == null)
			{
				return "null";
			}
			return string.Format(
				"HResult:    {1}{0}" +
				"HelpLink:   {2}{0}" +
				"Message:    {3}{0}" +
				"Source:     {4}{0}"
				+ "StackTrace: {5}{0}" + "{0}",
				Environment.NewLine,
				exception.HResult,
				exception.HelpLink,
				exception.Message,
				exception.Source,
				exception.StackTrace);
		}

		private void CheckIfOnlyInstance()
		{
			if (!_singleInstance)
			{
				if (MessageBox.Show("Приложение уже запущено") == MessageBoxResult.OK)
				{
					Application.Current.Shutdown();
				}
			}
		}

		protected override void ConfigureModuleCatalog()
		{
			base.ConfigureModuleCatalog();
			ModuleCatalog moduleCatalog = (ModuleCatalog)ModuleCatalog;
			moduleCatalog.AddModule(typeof (BusinessLogicModule), InitializationMode.WhenAvailable);
			moduleCatalog.AddModule(typeof (CurrencyListModule), InitializationMode.WhenAvailable);
		}
	}
}
