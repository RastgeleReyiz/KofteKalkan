using System.Net;
using System.Net.Sockets;

namespace KofteKalkan.Helpers
{
	public class IPTool
	{
		public static bool ValidateIP(string ipString)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Invalid comparison between Unknown and I4
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Invalid comparison between Unknown and I4
			if (string.IsNullOrWhiteSpace(ipString))
			{
				return false;
			}
			IPAddress val = default(IPAddress);
			if (IPAddress.TryParse(ipString, ref val))
			{
				AddressFamily addressFamily = val.get_AddressFamily();
				if ((int)addressFamily == 2)
				{
					return true;
				}
				if ((int)addressFamily == 23)
				{
					return true;
				}
			}
			return false;
		}
	}
}
