using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace CurrencyRate
{
	public partial class Shell
	{
		private const uint WS_EX_CONTEXTHELP = 0x00000400;
		private const uint WS_MINIMIZEBOX = 0x00020000;
		private const uint WS_MAXIMIZEBOX = 0x00010000;
		private const int GWL_STYLE = -16;
		private const int GWL_EXSTYLE = -20;
		private const int SWP_NOSIZE = 0x0001;
		private const int SWP_NOMOVE = 0x0002;
		private const int SWP_NOZORDER = 0x0004;
		private const int SWP_FRAMECHANGED = 0x0020;
		private const int WM_SYSCOMMAND = 0x0112;
		private const int SC_CONTEXTHELP = 0xF180;


		[DllImport("user32.dll")]
		private static extern uint GetWindowLong(IntPtr hwnd, int index);

		[DllImport("user32.dll")]
		private static extern int SetWindowLong(IntPtr hwnd, int index, uint newStyle);

		[DllImport("user32.dll")]
		private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

		public Shell()
		{
			InitializeComponent();
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			IntPtr hwnd = new WindowInteropHelper(this).Handle;
			uint styles = GetWindowLong(hwnd, GWL_STYLE);
			styles &= 0xFFFFFFFF ^ (WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
			SetWindowLong(hwnd, GWL_STYLE, styles);
			styles = GetWindowLong(hwnd, GWL_EXSTYLE);
			styles |= WS_EX_CONTEXTHELP;
			SetWindowLong(hwnd, GWL_EXSTYLE, styles);
			SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
			((HwndSource)PresentationSource.FromVisual(this)).AddHook(HelpHook);
		}

		private IntPtr HelpHook(IntPtr hwnd,
				int msg,
				IntPtr wParam,
				IntPtr lParam,
				ref bool handled)
		{
			if (msg == WM_SYSCOMMAND &&
					((int)wParam & 0xFFF0) == SC_CONTEXTHELP)
			{
				Help.ShowHelp(null, @"Help.chm");
				handled = true;
			}
			return IntPtr.Zero;
		}
	}
}
