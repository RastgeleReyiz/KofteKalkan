using System;
using System.Windows.Forms;
using KofteKalkan.Forms;

namespace KofteKalkan
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run((Form)(object)new MainForm());
		}
	}
}
