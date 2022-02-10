using System.Collections.Generic;
using NetFwTypeLib;

namespace KofteKalkan.Models
{
	public class Rule
	{
		public int Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public bool Enabled
		{
			get;
			set;
		}

		public int Protocol
		{
			get;
			set;
		}

		public string LocalPorts
		{
			get;
			set;
		}

		public string RemotePorts
		{
			get;
			set;
		}

		public List<string> LocalBlacklistIPs
		{
			get;
			set;
		}

		public List<string> LocalWhitelistIPs
		{
			get;
			set;
		}

		public string LocalIPs
		{
			get;
			set;
		}

		public List<string> RemoteBlacklistIPs
		{
			get;
			set;
		}

		public List<string> RemoteWhitelistIPs
		{
			get;
			set;
		}

		public string RemoteIPs
		{
			get;
			set;
		}

		public NET_FW_RULE_DIRECTION_ Direction
		{
			get;
			set;
		}
	}
}
