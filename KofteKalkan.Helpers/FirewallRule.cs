using System;
using System.Windows.Forms;
using KofteKalkan.Models;
using NetFwTypeLib;

namespace KofteKalkan.Helpers
{
	public class FirewallRule
	{
		public static void CreateRule(Profile profile)
		{
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (profile.InboundRule != null)
				{
					INetFwRule netFwRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
					netFwRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
					netFwRule.Protocol = profile.InboundRule.Protocol;
					netFwRule.Enabled = profile.InboundRule.Enabled;
					netFwRule.InterfaceTypes = "All";
					if (!string.IsNullOrWhiteSpace(profile.InboundRule.LocalPorts))
					{
						netFwRule.LocalPorts = profile.InboundRule.LocalPorts;
					}
					if (!string.IsNullOrWhiteSpace(profile.InboundRule.LocalIPs))
					{
						netFwRule.LocalAddresses = profile.InboundRule.LocalIPs;
					}
					if (!string.IsNullOrWhiteSpace(profile.InboundRule.RemoteIPs))
					{
						netFwRule.RemoteAddresses = profile.InboundRule.RemoteIPs;
					}
					if (!string.IsNullOrWhiteSpace(profile.InboundRule.RemotePorts))
					{
						netFwRule.RemotePorts = profile.InboundRule.RemotePorts;
					}
					netFwRule.Name = profile.InboundRule.Name;
					netFwRule.Direction = profile.InboundRule.Direction;
					INetFwPolicy2 obj = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
					obj.Rules.Remove(netFwRule.Name);
					obj.Rules.Add(netFwRule);
				}
				if (profile.OutboundRule != null)
				{
					INetFwRule netFwRule2 = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
					netFwRule2.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
					netFwRule2.Protocol = profile.OutboundRule.Protocol;
					netFwRule2.Enabled = profile.OutboundRule.Enabled;
					netFwRule2.InterfaceTypes = "All";
					if (!string.IsNullOrWhiteSpace(profile.OutboundRule.LocalPorts))
					{
						netFwRule2.LocalPorts = profile.OutboundRule.LocalPorts;
					}
					if (!string.IsNullOrWhiteSpace(profile.OutboundRule.LocalIPs))
					{
						netFwRule2.LocalAddresses = profile.OutboundRule.LocalIPs;
					}
					if (!string.IsNullOrWhiteSpace(profile.OutboundRule.RemoteIPs))
					{
						netFwRule2.RemoteAddresses = profile.OutboundRule.RemoteIPs;
					}
					if (!string.IsNullOrWhiteSpace(profile.OutboundRule.RemotePorts))
					{
						netFwRule2.RemotePorts = profile.OutboundRule.RemotePorts;
					}
					netFwRule2.Name = profile.OutboundRule.Name;
					netFwRule2.Direction = profile.OutboundRule.Direction;
					INetFwPolicy2 obj2 = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
					obj2.Rules.Remove(netFwRule2.Name);
					obj2.Rules.Add(netFwRule2);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public static void DeleteRule(Profile profile)
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (profile.InboundRule != null)
				{
					((INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"))).Rules.Remove(profile.InboundRule.Name);
				}
				if (profile.OutboundRule != null)
				{
					((INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"))).Rules.Remove(profile.OutboundRule.Name);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
