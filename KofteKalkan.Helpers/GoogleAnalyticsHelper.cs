using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KofteKalkan.Helpers
{
	public class GoogleAnalyticsHelper
	{
		private readonly string endpoint = "http://www.google-analytics.com/collect";

		private readonly string googleVersion = "1";

		private readonly string googleTrackingId;

		private readonly string googleClientId;

		public GoogleAnalyticsHelper(string trackingId, string clientId)
		{
			googleTrackingId = trackingId;
			googleClientId = clientId;
		}

		public async Task<HttpResponseMessage> TrackEvent(string category, string action, string label, int? value = null)
		{
			try
			{
				if (string.IsNullOrEmpty(category))
				{
					throw new ArgumentNullException("category");
				}
				if (string.IsNullOrEmpty(action))
				{
					throw new ArgumentNullException("action");
				}
				using HttpClient httpClient = new HttpClient();
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>("v", googleVersion),
					new KeyValuePair<string, string>("tid", googleTrackingId),
					new KeyValuePair<string, string>("cid", googleClientId),
					new KeyValuePair<string, string>("t", "event"),
					new KeyValuePair<string, string>("ec", category),
					new KeyValuePair<string, string>("ea", action)
				};
				if (label != null)
				{
					list.Add(new KeyValuePair<string, string>("el", label));
				}
				if (value.HasValue)
				{
					list.Add(new KeyValuePair<string, string>("ev", value.ToString()));
				}
				return await httpClient.PostAsync(endpoint, new FormUrlEncodedContent(list)).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
