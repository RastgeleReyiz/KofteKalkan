using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFwTypeLib
{
	[ComImport]
	[CompilerGenerated]
	[Guid("9C4C6277-5027-441E-AFAE-CA1F542DA009")]
	[TypeIdentifier]
	public interface INetFwRules : IEnumerable
	{
		void _VtblGap1_1();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[DispId(2)]
		void Add([In][MarshalAs(UnmanagedType.Interface)] INetFwRule rule);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[DispId(3)]
		void Remove([In][MarshalAs(UnmanagedType.BStr)] string Name);
	}
}
