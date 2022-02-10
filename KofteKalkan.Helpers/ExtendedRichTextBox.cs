using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KofteKalkan.Helpers
{
	public class ExtendedRichTextBox : RichTextBox
	{
		private static readonly object libraryLoadLock = new object();

		private static bool libraryLoadFlag;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = ((RichTextBox)this).get_CreateParams();
				try
				{
					lock (libraryLoadLock)
					{
						if (!libraryLoadFlag)
						{
							LoadLibrary("MsftEdit.dll");
							libraryLoadFlag = true;
						}
					}
					createParams.set_ClassName("RichEdit50W");
					return createParams;
				}
				catch
				{
					return createParams;
				}
			}
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern IntPtr LoadLibraryW(string s_File);

		public static IntPtr LoadLibrary(string s_File)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			IntPtr intPtr = LoadLibraryW(s_File);
			if (intPtr != IntPtr.Zero)
			{
				return intPtr;
			}
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		public ExtendedRichTextBox()
			: this()
		{
		}
	}
}
