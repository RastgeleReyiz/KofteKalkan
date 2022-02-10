using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace KofteKalkan.Helpers
{
	public class RangeCalculator
	{
		public static List<IPAddress> GetIPAddresses(List<string> ipAdresses)
		{
			List<IPAddress> list = new List<IPAddress>();
			foreach (string ipAdress in ipAdresses)
			{
				list.Add(IPAddress.Parse(ipAdress));
			}
			return list;
		}

		public static string GetRemoteAddresses(List<IPAddress> addresses)
		{
			SortedDictionary<uint, IPAddress> val = new SortedDictionary<uint, IPAddress>();
			foreach (IPAddress address in addresses)
			{
				val.Add(GetIntFromIp(address), address);
			}
			return ConstructRange(Enumerable.ToList<IPAddress>(Enumerable.Select<KeyValuePair<uint, IPAddress>, IPAddress>((IEnumerable<KeyValuePair<uint, IPAddress>>)val, (Func<KeyValuePair<uint, IPAddress>, IPAddress>)((KeyValuePair<uint, IPAddress> kvp) => kvp.Value))));
		}

		private static uint GetIntFromIp(IPAddress address)
		{
			byte[] addressBytes = address.GetAddressBytes();
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse((Array)addressBytes);
			}
			return BitConverter.ToUInt32(addressBytes, 0);
		}

		private static string Add(IPAddress address)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			byte[] addressBytes = new IPAddress((long)(GetIntFromIp(address) + 1)).GetAddressBytes();
			Array.Reverse((Array)addressBytes);
			return ((object)new IPAddress(addressBytes)).ToString();
		}

		private static string Substract(IPAddress address)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			byte[] addressBytes = new IPAddress((long)(GetIntFromIp(address) - 1)).GetAddressBytes();
			Array.Reverse((Array)addressBytes);
			return ((object)new IPAddress(addressBytes)).ToString();
		}

		private static string ConstructRange(List<IPAddress> list)
		{
			if (list.Count > 0)
			{
				string text = "1.1.1.1-";
				foreach (IPAddress item in list)
				{
					text = text + Substract(item) + "," + Add(item) + "-";
				}
				return text + "255.255.255.254";
			}
			return "";
		}
	}
}
