using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFwTypeLib
{
	[ComImport]
	[CompilerGenerated]
	[Guid("98325047-C671-4174-8D81-DEFCD3F03186")]
	[TypeIdentifier]
	public interface INetFwPolicy2
	{
		[DispId(7)]
		INetFwRules Rules
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[DispId(7)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		void _VtblGap1_11();
	}
}
