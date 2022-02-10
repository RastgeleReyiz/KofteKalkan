using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using KofteKalkan.Helpers;
using KofteKalkan.Models;
using KofteKalkan.Properties;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;
using NetFwTypeLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KofteKalkan.Forms
{
	public class MainForm : MetroForm
	{
		private KofteKalkan.Models.Settings settings;

		private List<Profile> profiles;

		private bool friendMode;

		private bool singleMode;

		private bool twitchLive;

		private GoogleAnalyticsHelper googleAnalytics;

		private int? notificationId;

		private string language = "en";

		private string version = "1.0.0.0";

		private IContainer components;

		private MetroTabPage metroTabPageSettings;

		private MetroTabPage metroTabPageShields;

		private MetroTabControl metroTabControl1;

		private MetroTabPage metroTabPageAbout;

		private MetroToggle metroToggleStreamNotifications;

		private MetroToggle metroToggleFreeGamesNotifications;

		private PictureBox pictureBoxBottomBanner;

		private NotifyIcon notifyIconStreamNotifications;

		private NotifyIcon notifyIconFreeGamesNotifications;

		private Timer timerFreeGamesNotifications;

		private MetroTabPage metroTabPageSponsorshipAndSupport;

		private MetroTabPage metroTabPageForStreamers;

		private PictureBox pictureBoxTopBanner;

		private Button btnFriendMode;

		private Button btnSingleMode;

		private Label lblSingleMode;

		private Label lblFriendMode;

		private Label lblStopShields;

		private Button btnStopShields;

		private Button btnEn;

		private Button btnTr;

		private Button btnYoutube;

		private PictureBox pictureBoxPurple;

		private Button btnTwitch;

		private Button btnInstagram;

		private Button btnTwitter;

		private Button btnHowToUse;

		private ExtendedRichTextBox richTextBoxSettings;

		private ExtendedRichTextBox richTextBoxAbout1;

		private ExtendedRichTextBox richTextBoxSponsorshipAndSupport;

		private ExtendedRichTextBox richTextBoxForStreamers;

		private ExtendedRichTextBox richTextBoxAbout2;

		private Label lblStatus;

		private Label lblFreeGamesNotifications;

		private Label lblStreamNotifications;

		private Label label1;

		private Timer timerStreamNotifications;

		private PictureBox pictureBoxRastgeleReyiz;

		private PictureBox pictureBoxFlameLionGames;

		public MainForm()
			: this()
		{
			InitializeComponent();
			ServicePointManager.set_SecurityProtocol((SecurityProtocolType)3072);
			FillSettings();
			if ((settings == null && CultureInfo.CurrentCulture.TwoLetterISOLanguageName != "tr") || settings?.Language == "en")
			{
				ChangeLanguage("en-US");
			}
			else
			{
				ChangeLanguage("tr-TR");
			}
			googleAnalytics = new GoogleAnalyticsHelper("UA-203687822-2", "555");
			if (CheckVersion())
			{
				CreateProfiles();
				FillProfiles();
				DeleteAllRules();
				SetTopBanner();
				googleAnalytics.TrackEvent("Application Usage", "Start Application", version);
			}
		}

		private void SetTopBanner()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			((Control)pictureBoxTopBanner).get_Controls().Add((Control)(object)pictureBoxRastgeleReyiz);
			PictureBox obj = pictureBoxRastgeleReyiz;
			Point location = ((Control)pictureBoxRastgeleReyiz).get_Location();
			int num = ((Point)(ref location)).get_X() - 20;
			location = ((Control)pictureBoxRastgeleReyiz).get_Location();
			((Control)obj).set_Location(new Point(num, ((Point)(ref location)).get_Y() - 24));
			((Control)pictureBoxTopBanner).get_Controls().Add((Control)(object)pictureBoxFlameLionGames);
			PictureBox obj2 = pictureBoxFlameLionGames;
			location = ((Control)pictureBoxFlameLionGames).get_Location();
			int num2 = ((Point)(ref location)).get_X() - 20;
			location = ((Control)pictureBoxFlameLionGames).get_Location();
			((Control)obj2).set_Location(new Point(num2, ((Point)(ref location)).get_Y() - 24));
		}

		private bool CheckVersion()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				string text = "https://rastgelereyiz.com/KofteKalkan/Version.txt";
				WebClient val = new WebClient();
				string a = val.DownloadString(text);
				((Component)val).Dispose();
				bool num = a == version;
				if (!num)
				{
					Form val2 = (Form)((!(language == "tr")) ? ((object)new UpdateForm_en(a)) : ((object)new UpdateForm_tr(a)));
					val2.ShowDialog();
					if (Application.get_MessageLoop())
					{
						Application.Exit();
					}
					else
					{
						Environment.Exit(1);
					}
				}
				return num;
			}
			catch (Exception)
			{
				if (Application.get_MessageLoop())
				{
					Application.Exit();
				}
				else
				{
					Environment.Exit(1);
				}
				return false;
			}
		}

		private void UpdateSettings()
		{
			settings = new KofteKalkan.Models.Settings
			{
				FreeGamesNotifications = ((CheckBox)metroToggleFreeGamesNotifications).get_Checked(),
				StreamNotifications = ((CheckBox)metroToggleStreamNotifications).get_Checked(),
				Language = language
			};
			string text = JsonConvert.SerializeObject((object)settings);
			File.WriteAllText("Settings.json", text, Encoding.UTF8);
		}

		private void FillSettings()
		{
			if (File.Exists("Settings.json"))
			{
				string text = File.ReadAllText("Settings.json", Encoding.UTF8);
				settings = JsonConvert.DeserializeObject<KofteKalkan.Models.Settings>(text);
				((CheckBox)metroToggleFreeGamesNotifications).set_Checked(settings.FreeGamesNotifications);
				((CheckBox)metroToggleStreamNotifications).set_Checked(settings.StreamNotifications);
			}
		}

		private void CreateProfiles()
		{
			if (!Directory.Exists("Profiles"))
			{
				Directory.CreateDirectory("Profiles");
			}
			if (!File.Exists("Profiles/1.json"))
			{
				string text = JsonConvert.SerializeObject((object)new Profile
				{
					Id = 1,
					Name = "Soft",
					OutboundRule = new Rule
					{
						Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT,
						Enabled = true,
						Id = 1,
						Name = "Kofte Kalkan - Profile 1 Outbound",
						Protocol = 17,
						RemotePorts = "6672,61455,61457,61456,61458"
					}
				});
				File.WriteAllText("Profiles/1.json", text, Encoding.UTF8);
			}
			if (!File.Exists("Profiles/2.json"))
			{
				string text2 = JsonConvert.SerializeObject((object)new Profile
				{
					Id = 2,
					Name = "Hard",
					InboundRule = new Rule
					{
						Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN,
						Enabled = true,
						Id = 2,
						LocalPorts = "6672",
						Name = "Kofte Kalkan - Profile 2 Inbound",
						Protocol = 17
					},
					OutboundRule = new Rule
					{
						Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT,
						Enabled = true,
						Id = 2,
						LocalPorts = "6672",
						Name = "Kofte Kalkan - Profile 2 Outbound",
						Protocol = 17
					}
				});
				File.WriteAllText("Profiles/2.json", text2, Encoding.UTF8);
			}
		}

		private void FillProfiles()
		{
			string[] files = Directory.GetFiles("Profiles");
			profiles = new List<Profile>();
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				Profile item = JsonConvert.DeserializeObject<Profile>(File.ReadAllText(array[i], Encoding.UTF8));
				profiles.Add(item);
			}
		}

		private void DeleteAllRules()
		{
			foreach (Profile profile in profiles)
			{
				FirewallRule.DeleteRule(profile);
			}
		}

		private void ProfileActivate(Profile profile)
		{
			FirewallRule.CreateRule(profile);
		}

		private void ProfileDeactivate(Profile profile)
		{
			FirewallRule.DeleteRule(profile);
		}

		private void CheckFreeGamesNotifications()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (((CheckBox)metroToggleFreeGamesNotifications).get_Checked())
				{
					string text = "https://rastgelereyiz.com/Games/LastGameSummary";
					WebClient val = new WebClient();
					val.set_Encoding(Encoding.UTF8);
					string text2 = val.DownloadString(text);
					((Component)val).Dispose();
					FreeGamesNotification freeGamesNotification = JsonConvert.DeserializeObject<FreeGamesNotification>(text2);
					if (File.Exists("FreeGamesNotifications.txt"))
					{
						notificationId = int.Parse(File.ReadAllText("FreeGamesNotifications.txt"));
					}
					if (freeGamesNotification.Id != notificationId)
					{
						notificationId = freeGamesNotification.Id;
						notifyIconFreeGamesNotifications.set_BalloonTipTitle(freeGamesNotification.Name);
						notifyIconFreeGamesNotifications.set_BalloonTipText(freeGamesNotification.Summary);
						notifyIconFreeGamesNotifications.set_Visible(true);
						notifyIconFreeGamesNotifications.ShowBalloonTip(5000);
						File.WriteAllText("FreeGamesNotifications.txt", freeGamesNotification.Id.ToString());
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void CheckStreamNotifications()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (!((CheckBox)metroToggleStreamNotifications).get_Checked())
				{
					return;
				}
				string text = "https://api.twitch.tv/helix/search/channels?query=rastgelereyiz&live_only=true&first=1";
				WebClient val = new WebClient();
				((NameValueCollection)val.get_Headers()).Add("client-id", "k47b98317d3zydmzsi8t691uh4798w");
				((NameValueCollection)val.get_Headers()).Add("Authorization", "Bearer js3327d47ow8eb48v2ixsl1gama0ul");
				string text2 = val.DownloadString(text);
				((Component)val).Dispose();
				JToken val2 = JsonConvert.DeserializeObject<JObject>(text2).get_Item("data");
				if (val2.get_HasValues())
				{
					if (Extensions.Value<string>((IEnumerable<JToken>)val2.get_Item((object)0).get_Item((object)"id")) == "43971358")
					{
						if (!twitchLive)
						{
							twitchLive = true;
							((Control)btnTwitch).set_BackgroundImage((Image)(object)Resources.TwitchLive);
							notifyIconStreamNotifications.set_BalloonTipText(Resources.TwitchLiveDescription);
							notifyIconFreeGamesNotifications.set_Visible(true);
							notifyIconFreeGamesNotifications.ShowBalloonTip(5000);
						}
					}
					else if (twitchLive)
					{
						twitchLive = false;
						((Control)btnTwitch).set_BackgroundImage((Image)(object)Resources.Twitch);
					}
				}
				else if (twitchLive)
				{
					twitchLive = false;
					((Control)btnTwitch).set_BackgroundImage((Image)(object)Resources.Twitch);
				}
			}
			catch (Exception)
			{
			}
		}

		private async void metroTileStopShields_Click(object sender, EventArgs e)
		{
			DeleteAllRules();
			await googleAnalytics.TrackEvent("All Shields", "Stop All Shields", version);
		}

		private async void pictureBoxBottomBanner_Click(object sender, EventArgs e)
		{
			Clipboard.SetText("DMdH9LvWKT8UTyJLnkwovv5qs7ZvKaM42v");
			MessageBox.Show(Resources.CopiedToClipboard);
			await googleAnalytics.TrackEvent("Banner", "Bottom Banner", version);
		}

		private void timerFreeGamesNotifications_Tick(object sender, EventArgs e)
		{
			CheckFreeGamesNotifications();
		}

		private async void notifyIconFreeGamesNotifications_BalloonTipClicked(object sender, EventArgs e)
		{
			Process.Start($"https://rastgelereyiz.com/Games/Details/{notificationId}");
			await googleAnalytics.TrackEvent("Notification", "Free Games", version, notificationId);
		}

		private async void notifyIconFreeGamesNotifications_Click(object sender, EventArgs e)
		{
			Process.Start($"https://rastgelereyiz.com/Games/Details/{notificationId}");
			notifyIconFreeGamesNotifications.set_Visible(false);
			await googleAnalytics.TrackEvent("Notification", "Free Games", version, notificationId);
		}

		private void notifyIconFreeGamesNotifications_BalloonTipClosed(object sender, EventArgs e)
		{
			notifyIconFreeGamesNotifications.set_Visible(false);
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			googleAnalytics.TrackEvent("Application Usage", "Close Application", version).Wait();
			DeleteAllRules();
			UpdateSettings();
		}

		private async void btnFriendMode_Click(object sender, EventArgs e)
		{
			if (!singleMode)
			{
				if (!friendMode)
				{
					((Control)lblFriendMode).set_Text(Resources.CloseFriendlyMode);
					((Control)lblFriendMode).set_ForeColor(Color.FromArgb(239, 255, 0));
					((Control)btnFriendMode).set_BackgroundImage((Image)(object)Resources.FriendModeActiveHover);
					((Control)lblStatus).set_Text(Resources.StatusFriendActive);
					((Control)btnSingleMode).set_Enabled(false);
					ProfileActivate(profiles[0]);
					await googleAnalytics.TrackEvent("Friend Mode", "Open Friend Mode", version);
				}
				else
				{
					((Control)lblFriendMode).set_Text(Resources.OpenFriendlyMode);
					((Control)lblFriendMode).set_ForeColor(Color.FromArgb(239, 255, 0));
					((Control)btnFriendMode).set_BackgroundImage((Image)(object)Resources.FriendModeHover);
					((Control)lblStatus).set_Text(Resources.StatusShieldsDown);
					((Control)btnSingleMode).set_Enabled(true);
					ProfileDeactivate(profiles[0]);
					await googleAnalytics.TrackEvent("Friend Mode", "Close Friend Mode", version);
				}
				friendMode = !friendMode;
			}
		}

		private void btnFriendMode_MouseHover(object sender, EventArgs e)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			if (!singleMode)
			{
				if (!friendMode)
				{
					((Control)btnFriendMode).set_BackgroundImage((Image)(object)Resources.FriendModeHover);
				}
				else
				{
					((Control)btnFriendMode).set_BackgroundImage((Image)(object)Resources.FriendModeActiveHover);
				}
				((Control)lblFriendMode).set_ForeColor(Color.FromArgb(239, 255, 0));
			}
		}

		private void btnFriendMode_MouseDown(object sender, MouseEventArgs e)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (!singleMode)
			{
				if (!friendMode)
				{
					((Control)btnFriendMode).set_BackgroundImage((Image)(object)Resources.FriendModeActive);
				}
				else
				{
					((Control)btnFriendMode).set_BackgroundImage((Image)(object)Resources.FriendModePassive);
				}
				((Control)lblFriendMode).set_ForeColor(Color.get_White());
			}
		}

		private void btnFriendMode_MouseLeave(object sender, EventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			if (singleMode)
			{
				return;
			}
			Rectangle clientRectangle = ((Control)lblFriendMode).get_ClientRectangle();
			if (!((Rectangle)(ref clientRectangle)).Contains(((Control)lblFriendMode).PointToClient(Cursor.get_Position())))
			{
				if (!friendMode)
				{
					((Control)btnFriendMode).set_BackgroundImage((Image)(object)Resources.FriendModePassive);
				}
				else
				{
					((Control)btnFriendMode).set_BackgroundImage((Image)(object)Resources.FriendModeActive);
				}
				((Control)lblFriendMode).set_ForeColor(Color.get_White());
			}
		}

		private async void btnSingleMode_Click(object sender, EventArgs e)
		{
			if (!friendMode)
			{
				if (!singleMode)
				{
					((Control)lblSingleMode).set_Text(Resources.CloseSoloMode);
					((Control)lblSingleMode).set_ForeColor(Color.FromArgb(239, 255, 0));
					((Control)btnSingleMode).set_BackgroundImage((Image)(object)Resources.SingleModeActiveHover);
					((Control)lblStatus).set_Text(Resources.StatusSoloActive);
					((Control)btnFriendMode).set_Enabled(false);
					ProfileActivate(profiles[1]);
					await googleAnalytics.TrackEvent("Single Mode", "Open Single Mode", version);
				}
				else
				{
					((Control)lblSingleMode).set_Text(Resources.OpenSoloMode);
					((Control)lblSingleMode).set_ForeColor(Color.FromArgb(239, 255, 0));
					((Control)btnSingleMode).set_BackgroundImage((Image)(object)Resources.SingleModeHover);
					((Control)lblStatus).set_Text(Resources.StatusShieldsDown);
					((Control)btnFriendMode).set_Enabled(true);
					ProfileDeactivate(profiles[1]);
					await googleAnalytics.TrackEvent("Single Mode", "Close Single Mode", version);
				}
				singleMode = !singleMode;
			}
		}

		private void btnSingleMode_MouseHover(object sender, EventArgs e)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			if (!friendMode)
			{
				if (!singleMode)
				{
					((Control)btnSingleMode).set_BackgroundImage((Image)(object)Resources.SingleModeHover);
				}
				else
				{
					((Control)btnSingleMode).set_BackgroundImage((Image)(object)Resources.SingleModeActiveHover);
				}
				((Control)lblSingleMode).set_ForeColor(Color.FromArgb(239, 255, 0));
			}
		}

		private void btnSingleMode_MouseDown(object sender, MouseEventArgs e)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (!friendMode)
			{
				if (!singleMode)
				{
					((Control)btnSingleMode).set_BackgroundImage((Image)(object)Resources.SingleModeActive);
				}
				else
				{
					((Control)btnSingleMode).set_BackgroundImage((Image)(object)Resources.SingleModePassive);
				}
				((Control)lblSingleMode).set_ForeColor(Color.get_White());
			}
		}

		private void btnSingleMode_MouseLeave(object sender, EventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			if (friendMode)
			{
				return;
			}
			Rectangle clientRectangle = ((Control)lblSingleMode).get_ClientRectangle();
			if (!((Rectangle)(ref clientRectangle)).Contains(((Control)lblSingleMode).PointToClient(Cursor.get_Position())))
			{
				if (!singleMode)
				{
					((Control)btnSingleMode).set_BackgroundImage((Image)(object)Resources.SingleModePassive);
				}
				else
				{
					((Control)btnSingleMode).set_BackgroundImage((Image)(object)Resources.SingleModeActive);
				}
				((Control)lblSingleMode).set_ForeColor(Color.get_White());
			}
		}

		private async void btnStopShields_Click(object sender, EventArgs e)
		{
			((Control)btnFriendMode).set_BackgroundImage((Image)(object)Resources.FriendModePassive);
			((Control)btnSingleMode).set_BackgroundImage((Image)(object)Resources.SingleModePassive);
			((Control)lblFriendMode).set_Text(Resources.OpenFriendlyMode);
			((Control)lblFriendMode).set_ForeColor(Color.get_White());
			((Control)lblSingleMode).set_Text(Resources.OpenSoloMode);
			((Control)lblSingleMode).set_ForeColor(Color.get_White());
			((Control)lblStatus).set_Text(Resources.StatusShieldsDown);
			((Control)btnFriendMode).set_Enabled(true);
			((Control)btnSingleMode).set_Enabled(true);
			friendMode = false;
			singleMode = false;
			DeleteAllRules();
			await googleAnalytics.TrackEvent("All Shields", "Stop All Shields", version);
		}

		private void btnStopShields_MouseHover(object sender, EventArgs e)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			((Control)btnStopShields).set_BackgroundImage((Image)(object)Resources.StopShieldsHover);
			((Control)lblStopShields).set_ForeColor(Color.FromArgb(239, 255, 0));
		}

		private void btnStopShields_MouseDown(object sender, MouseEventArgs e)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			((Control)btnStopShields).set_BackgroundImage((Image)(object)Resources.StopShieldsClick);
			((Control)lblStopShields).set_ForeColor(Color.get_White());
		}

		private void btnStopShields_MouseUp(object sender, MouseEventArgs e)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			((Control)btnStopShields).set_BackgroundImage((Image)(object)Resources.StopShieldsHover);
			((Control)lblStopShields).set_ForeColor(Color.FromArgb(239, 255, 0));
		}

		private void btnStopShields_MouseLeave(object sender, EventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			Rectangle clientRectangle = ((Control)lblStopShields).get_ClientRectangle();
			if (!((Rectangle)(ref clientRectangle)).Contains(((Control)lblStopShields).PointToClient(Cursor.get_Position())))
			{
				((Control)btnStopShields).set_BackgroundImage((Image)(object)Resources.StopShieldsPassive);
				((Control)lblStopShields).set_ForeColor(Color.get_White());
			}
		}

		private void ChangeLanguage(string language)
		{
			CultureInfo.DefaultThreadCurrentUICulture = (CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(language));
			((Control)metroTabPageAbout).set_Text(Resources.About);
			((Control)metroTabPageForStreamers).set_Text(Resources.ForStreamers);
			((Control)metroTabPageSettings).set_Text(Resources.Settings);
			((Control)metroTabPageShields).set_Text(Resources.Shields);
			((Control)metroTabPageSponsorshipAndSupport).set_Text(Resources.SponsorshipAndSupport);
			if (friendMode)
			{
				((Control)lblFriendMode).set_Text(Resources.CloseFriendlyMode);
				((Control)lblStatus).set_Text(Resources.StatusFriendActive);
			}
			else
			{
				((Control)lblFriendMode).set_Text(Resources.OpenFriendlyMode);
			}
			if (singleMode)
			{
				((Control)lblSingleMode).set_Text(Resources.CloseSoloMode);
				((Control)lblStatus).set_Text(Resources.StatusSoloActive);
			}
			else
			{
				((Control)lblSingleMode).set_Text(Resources.OpenSoloMode);
			}
			if (!friendMode && !singleMode)
			{
				((Control)lblStatus).set_Text(Resources.StatusShieldsDown);
			}
			if (!twitchLive)
			{
				((Control)btnTwitch).set_BackgroundImage((Image)(object)Resources.Twitch);
			}
			else
			{
				((Control)btnTwitch).set_BackgroundImage((Image)(object)Resources.TwitchLive);
			}
			((Control)lblStopShields).set_Text(Resources.StopShields);
			((Control)pictureBoxBottomBanner).set_BackgroundImage((Image)(object)Resources.BottomBanner);
			((Control)lblFreeGamesNotifications).set_Text(Resources.FreeGamesNotifications);
			((Control)lblStreamNotifications).set_Text(Resources.StreamNotifications);
		}

		private void btnEn_Click(object sender, EventArgs e)
		{
			language = "en";
			ChangeLanguage("en-US");
			FillRichTextBoxes();
		}

		private void btnTr_Click(object sender, EventArgs e)
		{
			language = "tr";
			ChangeLanguage("tr-TR");
			FillRichTextBoxes();
		}

		private async void btnTwitch_Click(object sender, EventArgs e)
		{
			Process.Start("https://www.twitch.tv/rastgelereyiz");
			await googleAnalytics.TrackEvent("Contact", "Twitch", version);
		}

		private void btnTwitch_MouseHover(object sender, EventArgs e)
		{
			if (!twitchLive)
			{
				((Control)btnTwitch).set_BackgroundImage((Image)(object)Resources.TwitchHover);
			}
			else
			{
				((Control)btnTwitch).set_BackgroundImage((Image)(object)Resources.TwitchLiveHover);
			}
		}

		private void btnTwitch_MouseLeave(object sender, EventArgs e)
		{
			if (!twitchLive)
			{
				((Control)btnTwitch).set_BackgroundImage((Image)(object)Resources.Twitch);
			}
			else
			{
				((Control)btnTwitch).set_BackgroundImage((Image)(object)Resources.TwitchLive);
			}
		}

		private async void btnYoutube_Click(object sender, EventArgs e)
		{
			Process.Start("https://www.youtube.com/c/rastgelereyiz");
			await googleAnalytics.TrackEvent("Contact", "YouTube", version);
		}

		private void btnYoutube_MouseHover(object sender, EventArgs e)
		{
			((Control)btnYoutube).set_BackgroundImage((Image)(object)Resources.YoutubeHover);
		}

		private void btnYoutube_MouseLeave(object sender, EventArgs e)
		{
			((Control)btnYoutube).set_BackgroundImage((Image)(object)Resources.Youtube);
		}

		private async void btnInstagram_Click(object sender, EventArgs e)
		{
			Process.Start("https://www.instagram.com/rastgelereyiz");
			await googleAnalytics.TrackEvent("Contact", "Instagram", version);
		}

		private void btnInstagram_MouseHover(object sender, EventArgs e)
		{
			((Control)btnInstagram).set_BackgroundImage((Image)(object)Resources.InstagramHover);
		}

		private void btnInstagram_MouseLeave(object sender, EventArgs e)
		{
			((Control)btnInstagram).set_BackgroundImage((Image)(object)Resources.Instagram);
		}

		private async void btnTwitter_Click(object sender, EventArgs e)
		{
			Process.Start("https://twitter.com/RastgeleReyiz");
			await googleAnalytics.TrackEvent("Contact", "Twitter", version);
		}

		private void btnTwitter_MouseHover(object sender, EventArgs e)
		{
			((Control)btnTwitter).set_BackgroundImage((Image)(object)Resources.TwitterHover);
		}

		private void btnTwitter_MouseLeave(object sender, EventArgs e)
		{
			((Control)btnTwitter).set_BackgroundImage((Image)(object)Resources.Twitter);
		}

		private void btnHowToUse_Click(object sender, EventArgs e)
		{
			((TabControl)metroTabControl1).set_SelectedIndex(1);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			FillRichTextBoxes();
			CheckFreeGamesNotifications();
			CheckStreamNotifications();
		}

		private void FillRichTextBoxes()
		{
			if (language == "tr")
			{
				((RichTextBox)richTextBoxSettings).set_Rtf(Resources.Settings_tr);
				((RichTextBox)richTextBoxAbout1).set_Rtf(Resources.About1_tr);
				((RichTextBox)richTextBoxAbout2).set_Rtf(Resources.About2_tr);
				((RichTextBox)richTextBoxSponsorshipAndSupport).set_Rtf(Resources.SponsorshipAndSupport_tr);
				((RichTextBox)richTextBoxForStreamers).set_Rtf(Resources.ForStreamers_tr);
			}
			else
			{
				((RichTextBox)richTextBoxSettings).set_Rtf(Resources.Settings_en);
				((RichTextBox)richTextBoxAbout1).set_Rtf(Resources.About1_en);
				((RichTextBox)richTextBoxAbout2).set_Rtf(Resources.About2_en);
				((RichTextBox)richTextBoxSponsorshipAndSupport).set_Rtf(Resources.SponsorshipAndSupport_en);
				((RichTextBox)richTextBoxForStreamers).set_Rtf(Resources.ForStreamers_en);
			}
		}

		private void RichTextBoxLinkClicked(object sender, LinkClickedEventArgs e)
		{
			Process.Start(e.get_LinkText());
		}

		private void timerStreamNotifications_Tick(object sender, EventArgs e)
		{
			CheckStreamNotifications();
		}

		private async void notifyIconStreamNotifications_BalloonTipClicked(object sender, EventArgs e)
		{
			Process.Start("https://www.twitch.tv/rastgelereyiz");
			notifyIconStreamNotifications.set_Visible(false);
			await googleAnalytics.TrackEvent("Notification", "Twitch", version);
		}

		private void notifyIconStreamNotifications_BalloonTipClosed(object sender, EventArgs e)
		{
			notifyIconStreamNotifications.set_Visible(false);
		}

		private async void notifyIconStreamNotifications_Click(object sender, EventArgs e)
		{
			Process.Start("https://www.twitch.tv/rastgelereyiz");
			notifyIconStreamNotifications.set_Visible(false);
			await googleAnalytics.TrackEvent("Notification", "Twitch", version);
		}

		private async void pictureBoxRastgeleReyiz_Click(object sender, EventArgs e)
		{
			Process.Start("https://rastgelereyiz.com/KofteKalkan");
			await googleAnalytics.TrackEvent("Contact", "Rastgele Reyiz", version);
		}

		private async void pictureBoxFlameLionGames_Click(object sender, EventArgs e)
		{
			Process.Start("https://flameliongames.com");
			await googleAnalytics.TrackEvent("Contact", "Flame Lion Games", version);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				((IDisposable)components).Dispose();
			}
			((MetroForm)this).Dispose(disposing);
		}

		private void InitializeComponent()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Expected O, but got Unknown
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Expected O, but got Unknown
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Expected O, but got Unknown
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Expected O, but got Unknown
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Expected O, but got Unknown
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Expected O, but got Unknown
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Expected O, but got Unknown
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Expected O, but got Unknown
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Expected O, but got Unknown
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Expected O, but got Unknown
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Expected O, but got Unknown
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Expected O, but got Unknown
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Expected O, but got Unknown
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Expected O, but got Unknown
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Expected O, but got Unknown
			//IL_03de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_044a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Expected O, but got Unknown
			//IL_045d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_056b: Unknown result type (might be due to invalid IL or missing references)
			//IL_058f: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_070c: Unknown result type (might be due to invalid IL or missing references)
			//IL_071d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0747: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_081f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0829: Expected O, but got Unknown
			//IL_0839: Unknown result type (might be due to invalid IL or missing references)
			//IL_0860: Unknown result type (might be due to invalid IL or missing references)
			//IL_089a: Unknown result type (might be due to invalid IL or missing references)
			//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_091a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0924: Expected O, but got Unknown
			//IL_092a: Unknown result type (might be due to invalid IL or missing references)
			//IL_094d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0971: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_09dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_09e6: Expected O, but got Unknown
			//IL_0a0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a56: Expected O, but got Unknown
			//IL_0a5c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b0b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b15: Expected O, but got Unknown
			//IL_0b3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b52: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b7b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b85: Expected O, but got Unknown
			//IL_0b8b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bcf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c1f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c3c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c46: Expected O, but got Unknown
			//IL_0c4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c8d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ce5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cef: Expected O, but got Unknown
			//IL_0d13: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d1d: Expected O, but got Unknown
			//IL_0d23: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d65: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d7a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0da3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dad: Expected O, but got Unknown
			//IL_0db3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dd9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e00: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e57: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e61: Expected O, but got Unknown
			//IL_0e9c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ea6: Expected O, but got Unknown
			//IL_0eb8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ed5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0edf: Expected O, but got Unknown
			//IL_0ee5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0efc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f23: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f7a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f84: Expected O, but got Unknown
			//IL_0fad: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd4: Expected O, but got Unknown
			//IL_0fda: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ff1: Unknown result type (might be due to invalid IL or missing references)
			//IL_1018: Unknown result type (might be due to invalid IL or missing references)
			//IL_106f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1079: Expected O, but got Unknown
			//IL_1096: Unknown result type (might be due to invalid IL or missing references)
			//IL_10d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_10ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_1116: Unknown result type (might be due to invalid IL or missing references)
			//IL_1120: Expected O, but got Unknown
			//IL_1126: Unknown result type (might be due to invalid IL or missing references)
			//IL_1149: Unknown result type (might be due to invalid IL or missing references)
			//IL_1170: Unknown result type (might be due to invalid IL or missing references)
			//IL_11c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_11d0: Expected O, but got Unknown
			//IL_1204: Unknown result type (might be due to invalid IL or missing references)
			//IL_1246: Unknown result type (might be due to invalid IL or missing references)
			//IL_125b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1284: Unknown result type (might be due to invalid IL or missing references)
			//IL_128e: Expected O, but got Unknown
			//IL_1294: Unknown result type (might be due to invalid IL or missing references)
			//IL_12b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_12de: Unknown result type (might be due to invalid IL or missing references)
			//IL_1334: Unknown result type (might be due to invalid IL or missing references)
			//IL_133e: Expected O, but got Unknown
			//IL_13e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_141d: Unknown result type (might be due to invalid IL or missing references)
			//IL_146c: Unknown result type (might be due to invalid IL or missing references)
			//IL_14d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_14e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_150b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1577: Unknown result type (might be due to invalid IL or missing references)
			//IL_15c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_15d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1600: Unknown result type (might be due to invalid IL or missing references)
			//IL_166c: Unknown result type (might be due to invalid IL or missing references)
			//IL_16ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_16cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_16f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_1789: Unknown result type (might be due to invalid IL or missing references)
			//IL_1793: Expected O, but got Unknown
			//IL_1805: Unknown result type (might be due to invalid IL or missing references)
			//IL_180f: Expected O, but got Unknown
			//IL_189c: Unknown result type (might be due to invalid IL or missing references)
			//IL_18a6: Expected O, but got Unknown
			//IL_18bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_18e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_192f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1956: Unknown result type (might be due to invalid IL or missing references)
			//IL_199e: Unknown result type (might be due to invalid IL or missing references)
			//IL_19e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a07: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a39: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a5d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ad4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1aee: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b15: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b47: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b92: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bb0: Unknown result type (might be due to invalid IL or missing references)
			//IL_1be2: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c06: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c7d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cc8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ce6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d18: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d3c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1db3: Unknown result type (might be due to invalid IL or missing references)
			//IL_1dfe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e72: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f07: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f45: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f85: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f9c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1fc0: Unknown result type (might be due to invalid IL or missing references)
			//IL_200f: Unknown result type (might be due to invalid IL or missing references)
			//IL_202f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2065: Unknown result type (might be due to invalid IL or missing references)
			//IL_2098: Unknown result type (might be due to invalid IL or missing references)
			//IL_20a2: Expected O, but got Unknown
			//IL_20b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_20d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_210c: Unknown result type (might be due to invalid IL or missing references)
			//IL_213f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2149: Expected O, but got Unknown
			//IL_215e: Unknown result type (might be due to invalid IL or missing references)
			//IL_217d: Unknown result type (might be due to invalid IL or missing references)
			//IL_21bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_21f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_21fc: Expected O, but got Unknown
			//IL_2211: Unknown result type (might be due to invalid IL or missing references)
			//IL_222f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2265: Unknown result type (might be due to invalid IL or missing references)
			//IL_2298: Unknown result type (might be due to invalid IL or missing references)
			//IL_22a2: Expected O, but got Unknown
			//IL_22b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_22d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_230b: Unknown result type (might be due to invalid IL or missing references)
			//IL_233e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2348: Expected O, but got Unknown
			//IL_2353: Unknown result type (might be due to invalid IL or missing references)
			//IL_236f: Unknown result type (might be due to invalid IL or missing references)
			//IL_242f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2439: Expected O, but got Unknown
			//IL_2444: Unknown result type (might be due to invalid IL or missing references)
			//IL_2459: Unknown result type (might be due to invalid IL or missing references)
			//IL_249d: Unknown result type (might be due to invalid IL or missing references)
			//IL_24a7: Expected O, but got Unknown
			components = (IContainer)new Container();
			ComponentResourceManager val = new ComponentResourceManager(typeof(MainForm));
			metroTabPageSettings = new MetroTabPage();
			lblFreeGamesNotifications = new Label();
			lblStreamNotifications = new Label();
			metroToggleFreeGamesNotifications = new MetroToggle();
			metroToggleStreamNotifications = new MetroToggle();
			metroTabPageShields = new MetroTabPage();
			label1 = new Label();
			lblStatus = new Label();
			btnHowToUse = new Button();
			btnTr = new Button();
			btnEn = new Button();
			lblStopShields = new Label();
			btnStopShields = new Button();
			lblSingleMode = new Label();
			lblFriendMode = new Label();
			btnSingleMode = new Button();
			btnFriendMode = new Button();
			metroTabControl1 = new MetroTabControl();
			metroTabPageAbout = new MetroTabPage();
			metroTabPageSponsorshipAndSupport = new MetroTabPage();
			metroTabPageForStreamers = new MetroTabPage();
			notifyIconStreamNotifications = new NotifyIcon(components);
			notifyIconFreeGamesNotifications = new NotifyIcon(components);
			timerFreeGamesNotifications = new Timer(components);
			pictureBoxTopBanner = new PictureBox();
			pictureBoxBottomBanner = new PictureBox();
			btnYoutube = new Button();
			pictureBoxPurple = new PictureBox();
			btnTwitch = new Button();
			btnInstagram = new Button();
			btnTwitter = new Button();
			timerStreamNotifications = new Timer(components);
			pictureBoxRastgeleReyiz = new PictureBox();
			pictureBoxFlameLionGames = new PictureBox();
			richTextBoxSettings = new ExtendedRichTextBox();
			richTextBoxAbout2 = new ExtendedRichTextBox();
			richTextBoxAbout1 = new ExtendedRichTextBox();
			richTextBoxSponsorshipAndSupport = new ExtendedRichTextBox();
			richTextBoxForStreamers = new ExtendedRichTextBox();
			((Control)metroTabPageSettings).SuspendLayout();
			((Control)metroTabPageShields).SuspendLayout();
			((Control)metroTabControl1).SuspendLayout();
			((Control)metroTabPageAbout).SuspendLayout();
			((Control)metroTabPageSponsorshipAndSupport).SuspendLayout();
			((Control)metroTabPageForStreamers).SuspendLayout();
			((ISupportInitialize)pictureBoxTopBanner).BeginInit();
			((ISupportInitialize)pictureBoxBottomBanner).BeginInit();
			((ISupportInitialize)pictureBoxPurple).BeginInit();
			((ISupportInitialize)pictureBoxRastgeleReyiz).BeginInit();
			((ISupportInitialize)pictureBoxFlameLionGames).BeginInit();
			((Control)this).SuspendLayout();
			((Control)metroTabPageSettings).set_BackColor(Color.FromArgb(219, 239, 255));
			((Control)metroTabPageSettings).get_Controls().Add((Control)(object)lblFreeGamesNotifications);
			((Control)metroTabPageSettings).get_Controls().Add((Control)(object)lblStreamNotifications);
			((Control)metroTabPageSettings).get_Controls().Add((Control)(object)richTextBoxSettings);
			((Control)metroTabPageSettings).get_Controls().Add((Control)(object)metroToggleFreeGamesNotifications);
			((Control)metroTabPageSettings).get_Controls().Add((Control)(object)metroToggleStreamNotifications);
			metroTabPageSettings.set_HorizontalScrollbarBarColor(true);
			metroTabPageSettings.set_HorizontalScrollbarHighlightOnWheel(false);
			metroTabPageSettings.set_HorizontalScrollbarSize(10);
			((TabPage)metroTabPageSettings).set_Location(new Point(4, 38));
			((Control)metroTabPageSettings).set_Margin(new Padding(0));
			((Control)metroTabPageSettings).set_Name("metroTabPageSettings");
			((Control)metroTabPageSettings).set_Size(new Size(622, 304));
			((TabPage)metroTabPageSettings).set_TabIndex(1);
			((Control)metroTabPageSettings).set_Text("| AYARLAR");
			metroTabPageSettings.set_UseCustomBackColor(true);
			metroTabPageSettings.set_VerticalScrollbarBarColor(true);
			metroTabPageSettings.set_VerticalScrollbarHighlightOnWheel(false);
			metroTabPageSettings.set_VerticalScrollbarSize(10);
			((Control)lblFreeGamesNotifications).set_AutoSize(true);
			((Control)lblFreeGamesNotifications).set_Font(new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0));
			((Control)lblFreeGamesNotifications).set_Location(new Point(8, 45));
			((Control)lblFreeGamesNotifications).set_Name("lblFreeGamesNotifications");
			((Control)lblFreeGamesNotifications).set_Size(new Size(179, 16));
			((Control)lblFreeGamesNotifications).set_TabIndex(5);
			((Control)lblFreeGamesNotifications).set_Text("Bedava Oyun Bildirimleri");
			((Control)lblStreamNotifications).set_AutoSize(true);
			((Control)lblStreamNotifications).set_Font(new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0));
			((Control)lblStreamNotifications).set_Location(new Point(8, 25));
			((Control)lblStreamNotifications).set_Name("lblStreamNotifications");
			((Control)lblStreamNotifications).set_Size(new Size(125, 16));
			((Control)lblStreamNotifications).set_TabIndex(5);
			((Control)lblStreamNotifications).set_Text("Yayın Bildirimleri");
			((Control)metroToggleFreeGamesNotifications).set_AutoSize(true);
			metroToggleFreeGamesNotifications.set_DisplayStatus(false);
			((Control)metroToggleFreeGamesNotifications).set_Location(new Point(243, 44));
			((Control)metroToggleFreeGamesNotifications).set_Name("metroToggleFreeGamesNotifications");
			((Control)metroToggleFreeGamesNotifications).set_Size(new Size(50, 17));
			metroToggleFreeGamesNotifications.set_Style((MetroColorStyle)5);
			((Control)metroToggleFreeGamesNotifications).set_TabIndex(2);
			((Control)metroToggleFreeGamesNotifications).set_Text("Off");
			metroToggleFreeGamesNotifications.set_UseSelectable(true);
			((Control)metroToggleStreamNotifications).set_AutoSize(true);
			((CheckBox)metroToggleStreamNotifications).set_Checked(true);
			((CheckBox)metroToggleStreamNotifications).set_CheckState((CheckState)1);
			metroToggleStreamNotifications.set_DisplayStatus(false);
			((Control)metroToggleStreamNotifications).set_Location(new Point(243, 21));
			((Control)metroToggleStreamNotifications).set_Name("metroToggleStreamNotifications");
			((Control)metroToggleStreamNotifications).set_Size(new Size(50, 17));
			metroToggleStreamNotifications.set_Style((MetroColorStyle)5);
			((Control)metroToggleStreamNotifications).set_TabIndex(2);
			((Control)metroToggleStreamNotifications).set_Text("On");
			metroToggleStreamNotifications.set_UseSelectable(true);
			((Control)metroTabPageShields).set_BackColor(Color.FromArgb(219, 239, 255));
			((Control)metroTabPageShields).get_Controls().Add((Control)(object)label1);
			((Control)metroTabPageShields).get_Controls().Add((Control)(object)lblStatus);
			((Control)metroTabPageShields).get_Controls().Add((Control)(object)btnHowToUse);
			((Control)metroTabPageShields).get_Controls().Add((Control)(object)btnTr);
			((Control)metroTabPageShields).get_Controls().Add((Control)(object)btnEn);
			((Control)metroTabPageShields).get_Controls().Add((Control)(object)lblStopShields);
			((Control)metroTabPageShields).get_Controls().Add((Control)(object)btnStopShields);
			((Control)metroTabPageShields).get_Controls().Add((Control)(object)lblSingleMode);
			((Control)metroTabPageShields).get_Controls().Add((Control)(object)lblFriendMode);
			((Control)metroTabPageShields).get_Controls().Add((Control)(object)btnSingleMode);
			((Control)metroTabPageShields).get_Controls().Add((Control)(object)btnFriendMode);
			metroTabPageShields.set_HorizontalScrollbarBarColor(true);
			metroTabPageShields.set_HorizontalScrollbarHighlightOnWheel(false);
			metroTabPageShields.set_HorizontalScrollbarSize(10);
			((TabPage)metroTabPageShields).set_Location(new Point(4, 38));
			((Control)metroTabPageShields).set_Margin(new Padding(0));
			((Control)metroTabPageShields).set_Name("metroTabPageShields");
			((Control)metroTabPageShields).set_Size(new Size(622, 304));
			((TabPage)metroTabPageShields).set_TabIndex(0);
			((Control)metroTabPageShields).set_Text("| KALKANLAR");
			metroTabPageShields.set_UseCustomBackColor(true);
			metroTabPageShields.set_VerticalScrollbarBarColor(true);
			metroTabPageShields.set_VerticalScrollbarHighlightOnWheel(false);
			metroTabPageShields.set_VerticalScrollbarSize(10);
			((Control)label1).set_Location(new Point(463, 281));
			((Control)label1).set_Name("label1");
			((Control)label1).set_Size(new Size(159, 23));
			((Control)label1).set_TabIndex(15);
			((Control)label1).set_Text("1.0.0.0");
			label1.set_TextAlign((ContentAlignment)1024);
			((Control)lblStatus).set_Font(new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0));
			((Control)lblStatus).set_Location(new Point(130, 258));
			((Control)lblStatus).set_Name("lblStatus");
			((Control)lblStatus).set_Size(new Size(422, 26));
			((Control)lblStatus).set_TabIndex(14);
			((Control)lblStatus).set_Text("Durum: Kalkanlar Kapalı");
			lblStatus.set_TextAlign((ContentAlignment)32);
			((Control)btnHowToUse).set_BackColor(Color.get_Transparent());
			((Control)btnHowToUse).set_BackgroundImage((Image)(object)Resources.HowToUse);
			((Control)btnHowToUse).set_BackgroundImageLayout((ImageLayout)4);
			((ButtonBase)btnHowToUse).get_FlatAppearance().set_BorderSize(0);
			((ButtonBase)btnHowToUse).get_FlatAppearance().set_MouseDownBackColor(Color.get_Transparent());
			((ButtonBase)btnHowToUse).get_FlatAppearance().set_MouseOverBackColor(Color.get_Transparent());
			((ButtonBase)btnHowToUse).set_FlatStyle((FlatStyle)0);
			((Control)btnHowToUse).set_Font(new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0));
			((Control)btnHowToUse).set_ForeColor(Color.get_White());
			((ButtonBase)btnHowToUse).set_ImageIndex(0);
			((Control)btnHowToUse).set_Location(new Point(35, 179));
			((Control)btnHowToUse).set_Name("btnHowToUse");
			((Control)btnHowToUse).set_Size(new Size(48, 48));
			((Control)btnHowToUse).set_TabIndex(13);
			((ButtonBase)btnHowToUse).set_TextAlign((ContentAlignment)512);
			((ButtonBase)btnHowToUse).set_UseVisualStyleBackColor(false);
			((Control)btnHowToUse).add_Click((EventHandler)btnHowToUse_Click);
			((Control)btnTr).set_BackColor(Color.get_Transparent());
			((Control)btnTr).set_BackgroundImage((Image)((ResourceManager)(object)val).GetObject("btnTr.BackgroundImage"));
			((Control)btnTr).set_BackgroundImageLayout((ImageLayout)4);
			((ButtonBase)btnTr).get_FlatAppearance().set_BorderSize(0);
			((ButtonBase)btnTr).get_FlatAppearance().set_MouseDownBackColor(Color.get_Transparent());
			((ButtonBase)btnTr).get_FlatAppearance().set_MouseOverBackColor(Color.get_Transparent());
			((ButtonBase)btnTr).set_FlatStyle((FlatStyle)0);
			((Control)btnTr).set_Font(new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0));
			((Control)btnTr).set_ForeColor(Color.get_White());
			((ButtonBase)btnTr).set_ImageIndex(0);
			((Control)btnTr).set_Location(new Point(35, 67));
			((Control)btnTr).set_Name("btnTr");
			((Control)btnTr).set_Size(new Size(48, 48));
			((Control)btnTr).set_TabIndex(12);
			((ButtonBase)btnTr).set_TextAlign((ContentAlignment)512);
			((ButtonBase)btnTr).set_UseVisualStyleBackColor(false);
			((Control)btnTr).add_Click((EventHandler)btnTr_Click);
			((Control)btnEn).set_BackColor(Color.get_Transparent());
			((Control)btnEn).set_BackgroundImage((Image)((ResourceManager)(object)val).GetObject("btnEn.BackgroundImage"));
			((Control)btnEn).set_BackgroundImageLayout((ImageLayout)4);
			((ButtonBase)btnEn).get_FlatAppearance().set_BorderSize(0);
			((ButtonBase)btnEn).get_FlatAppearance().set_MouseDownBackColor(Color.get_Transparent());
			((ButtonBase)btnEn).get_FlatAppearance().set_MouseOverBackColor(Color.get_Transparent());
			((ButtonBase)btnEn).set_FlatStyle((FlatStyle)0);
			((Control)btnEn).set_Font(new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0));
			((Control)btnEn).set_ForeColor(Color.get_White());
			((ButtonBase)btnEn).set_ImageIndex(0);
			((Control)btnEn).set_Location(new Point(35, 121));
			((Control)btnEn).set_Name("btnEn");
			((Control)btnEn).set_Size(new Size(48, 48));
			((Control)btnEn).set_TabIndex(11);
			((ButtonBase)btnEn).set_TextAlign((ContentAlignment)512);
			((ButtonBase)btnEn).set_UseVisualStyleBackColor(false);
			((Control)btnEn).add_Click((EventHandler)btnEn_Click);
			((Control)lblStopShields).set_BackColor(Color.get_Red());
			((Control)lblStopShields).set_Font(new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0));
			((Control)lblStopShields).set_ForeColor(Color.get_White());
			((Control)lblStopShields).set_Location(new Point(139, 215));
			((Control)lblStopShields).set_Name("lblStopShields");
			((Control)lblStopShields).set_Size(new Size(400, 23));
			((Control)lblStopShields).set_TabIndex(10);
			((Control)lblStopShields).set_Text("KALKANLARI DURDUR");
			lblStopShields.set_TextAlign((ContentAlignment)32);
			((Control)lblStopShields).add_Click((EventHandler)btnStopShields_Click);
			((Control)lblStopShields).add_MouseDown(new MouseEventHandler(btnStopShields_MouseDown));
			((Control)lblStopShields).add_MouseHover((EventHandler)btnStopShields_MouseHover);
			((Control)lblStopShields).add_MouseUp(new MouseEventHandler(btnStopShields_MouseUp));
			((Control)btnStopShields).set_BackColor(Color.get_Transparent());
			((Control)btnStopShields).set_BackgroundImage((Image)(object)Resources.StopShieldsPassive);
			((Control)btnStopShields).set_BackgroundImageLayout((ImageLayout)4);
			((ButtonBase)btnStopShields).get_FlatAppearance().set_BorderSize(0);
			((ButtonBase)btnStopShields).get_FlatAppearance().set_MouseDownBackColor(Color.get_Transparent());
			((ButtonBase)btnStopShields).get_FlatAppearance().set_MouseOverBackColor(Color.get_Transparent());
			((ButtonBase)btnStopShields).set_FlatStyle((FlatStyle)0);
			((Control)btnStopShields).set_Font(new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0));
			((Control)btnStopShields).set_ForeColor(Color.get_White());
			((ButtonBase)btnStopShields).set_ImageIndex(0);
			((Control)btnStopShields).set_Location(new Point(130, 157));
			((Control)btnStopShields).set_Name("btnStopShields");
			((Control)btnStopShields).set_Size(new Size(424, 94));
			((Control)btnStopShields).set_TabIndex(9);
			((ButtonBase)btnStopShields).set_TextAlign((ContentAlignment)512);
			((ButtonBase)btnStopShields).set_UseVisualStyleBackColor(false);
			((Control)btnStopShields).add_Click((EventHandler)btnStopShields_Click);
			((Control)btnStopShields).add_MouseDown(new MouseEventHandler(btnStopShields_MouseDown));
			((Control)btnStopShields).add_MouseLeave((EventHandler)btnStopShields_MouseLeave);
			((Control)btnStopShields).add_MouseHover((EventHandler)btnStopShields_MouseHover);
			((Control)btnStopShields).add_MouseUp(new MouseEventHandler(btnStopShields_MouseUp));
			((Control)lblSingleMode).set_BackColor(Color.FromArgb(28, 255, 150));
			((Control)lblSingleMode).set_Font(new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0));
			((Control)lblSingleMode).set_ForeColor(Color.get_White());
			((Control)lblSingleMode).set_Location(new Point(347, 117));
			((Control)lblSingleMode).set_Name("lblSingleMode");
			((Control)lblSingleMode).set_Size(new Size(196, 23));
			((Control)lblSingleMode).set_TabIndex(8);
			((Control)lblSingleMode).set_Text("TEKLİ MOD AÇ");
			lblSingleMode.set_TextAlign((ContentAlignment)32);
			((Control)lblSingleMode).add_Click((EventHandler)btnSingleMode_Click);
			((Control)lblSingleMode).add_MouseDown(new MouseEventHandler(btnSingleMode_MouseDown));
			((Control)lblSingleMode).add_MouseHover((EventHandler)btnSingleMode_MouseHover);
			((Control)lblFriendMode).set_BackColor(Color.FromArgb(77, 196, 255));
			((Control)lblFriendMode).set_Font(new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0));
			((Control)lblFriendMode).set_ForeColor(Color.get_White());
			((Control)lblFriendMode).set_Location(new Point(135, 117));
			((Control)lblFriendMode).set_Name("lblFriendMode");
			((Control)lblFriendMode).set_Size(new Size(196, 23));
			((Control)lblFriendMode).set_TabIndex(7);
			((Control)lblFriendMode).set_Text("ARKADAŞ MODU AÇ");
			lblFriendMode.set_TextAlign((ContentAlignment)32);
			((Control)lblFriendMode).add_Click((EventHandler)btnFriendMode_Click);
			((Control)lblFriendMode).add_MouseDown(new MouseEventHandler(btnFriendMode_MouseDown));
			((Control)lblFriendMode).add_MouseHover((EventHandler)btnFriendMode_MouseHover);
			((Control)btnSingleMode).set_BackColor(Color.get_Transparent());
			((Control)btnSingleMode).set_BackgroundImage((Image)(object)Resources.SingleModePassive);
			((Control)btnSingleMode).set_BackgroundImageLayout((ImageLayout)4);
			((ButtonBase)btnSingleMode).get_FlatAppearance().set_BorderSize(0);
			((ButtonBase)btnSingleMode).get_FlatAppearance().set_MouseDownBackColor(Color.get_Transparent());
			((ButtonBase)btnSingleMode).get_FlatAppearance().set_MouseOverBackColor(Color.get_Transparent());
			((ButtonBase)btnSingleMode).set_FlatStyle((FlatStyle)0);
			((Control)btnSingleMode).set_Font(new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0));
			((Control)btnSingleMode).set_ForeColor(Color.get_White());
			((ButtonBase)btnSingleMode).set_ImageIndex(0);
			((Control)btnSingleMode).set_Location(new Point(342, 55));
			((Control)btnSingleMode).set_Name("btnSingleMode");
			((Control)btnSingleMode).set_Size(new Size(210, 92));
			((Control)btnSingleMode).set_TabIndex(6);
			((ButtonBase)btnSingleMode).set_TextAlign((ContentAlignment)512);
			((ButtonBase)btnSingleMode).set_UseVisualStyleBackColor(false);
			((Control)btnSingleMode).add_Click((EventHandler)btnSingleMode_Click);
			((Control)btnSingleMode).add_MouseDown(new MouseEventHandler(btnSingleMode_MouseDown));
			((Control)btnSingleMode).add_MouseLeave((EventHandler)btnSingleMode_MouseLeave);
			((Control)btnSingleMode).add_MouseHover((EventHandler)btnSingleMode_MouseHover);
			((Control)btnFriendMode).set_BackColor(Color.get_Transparent());
			((Control)btnFriendMode).set_BackgroundImage((Image)(object)Resources.FriendModePassive);
			((Control)btnFriendMode).set_BackgroundImageLayout((ImageLayout)4);
			((ButtonBase)btnFriendMode).get_FlatAppearance().set_BorderSize(0);
			((ButtonBase)btnFriendMode).get_FlatAppearance().set_MouseDownBackColor(Color.get_Transparent());
			((ButtonBase)btnFriendMode).get_FlatAppearance().set_MouseOverBackColor(Color.get_Transparent());
			((ButtonBase)btnFriendMode).set_FlatStyle((FlatStyle)0);
			((Control)btnFriendMode).set_Font(new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0));
			((Control)btnFriendMode).set_ForeColor(Color.get_White());
			((ButtonBase)btnFriendMode).set_ImageIndex(0);
			((Control)btnFriendMode).set_Location(new Point(130, 55));
			((Control)btnFriendMode).set_Name("btnFriendMode");
			((Control)btnFriendMode).set_Size(new Size(210, 92));
			((Control)btnFriendMode).set_TabIndex(5);
			((ButtonBase)btnFriendMode).set_TextAlign((ContentAlignment)512);
			((ButtonBase)btnFriendMode).set_UseVisualStyleBackColor(false);
			((Control)btnFriendMode).add_Click((EventHandler)btnFriendMode_Click);
			((Control)btnFriendMode).add_MouseDown(new MouseEventHandler(btnFriendMode_MouseDown));
			((Control)btnFriendMode).add_MouseLeave((EventHandler)btnFriendMode_MouseLeave);
			((Control)btnFriendMode).add_MouseHover((EventHandler)btnFriendMode_MouseHover);
			((Control)metroTabControl1).get_Controls().Add((Control)(object)metroTabPageShields);
			((Control)metroTabControl1).get_Controls().Add((Control)(object)metroTabPageSettings);
			((Control)metroTabControl1).get_Controls().Add((Control)(object)metroTabPageAbout);
			((Control)metroTabControl1).get_Controls().Add((Control)(object)metroTabPageSponsorshipAndSupport);
			((Control)metroTabControl1).get_Controls().Add((Control)(object)metroTabPageForStreamers);
			((Control)metroTabControl1).set_Location(new Point(20, 131));
			((Control)metroTabControl1).set_Name("metroTabControl1");
			((TabControl)metroTabControl1).set_SelectedIndex(0);
			((Control)metroTabControl1).set_Size(new Size(630, 346));
			metroTabControl1.set_Style((MetroColorStyle)1);
			((Control)metroTabControl1).set_TabIndex(0);
			metroTabControl1.set_UseCustomBackColor(true);
			metroTabControl1.set_UseSelectable(true);
			((Control)metroTabPageAbout).set_BackColor(Color.FromArgb(219, 239, 255));
			((Control)metroTabPageAbout).get_Controls().Add((Control)(object)richTextBoxAbout2);
			((Control)metroTabPageAbout).get_Controls().Add((Control)(object)richTextBoxAbout1);
			metroTabPageAbout.set_HorizontalScrollbarBarColor(true);
			metroTabPageAbout.set_HorizontalScrollbarHighlightOnWheel(false);
			metroTabPageAbout.set_HorizontalScrollbarSize(10);
			((TabPage)metroTabPageAbout).set_Location(new Point(4, 38));
			((Control)metroTabPageAbout).set_Margin(new Padding(0));
			((Control)metroTabPageAbout).set_Name("metroTabPageAbout");
			((Control)metroTabPageAbout).set_Size(new Size(622, 304));
			((TabPage)metroTabPageAbout).set_TabIndex(2);
			((Control)metroTabPageAbout).set_Text("| HAKKINDA");
			metroTabPageAbout.set_UseCustomBackColor(true);
			metroTabPageAbout.set_VerticalScrollbarBarColor(true);
			metroTabPageAbout.set_VerticalScrollbarHighlightOnWheel(false);
			metroTabPageAbout.set_VerticalScrollbarSize(10);
			((Control)metroTabPageSponsorshipAndSupport).set_BackColor(Color.FromArgb(219, 239, 255));
			((Control)metroTabPageSponsorshipAndSupport).get_Controls().Add((Control)(object)richTextBoxSponsorshipAndSupport);
			metroTabPageSponsorshipAndSupport.set_HorizontalScrollbarBarColor(true);
			metroTabPageSponsorshipAndSupport.set_HorizontalScrollbarHighlightOnWheel(false);
			metroTabPageSponsorshipAndSupport.set_HorizontalScrollbarSize(10);
			((TabPage)metroTabPageSponsorshipAndSupport).set_Location(new Point(4, 38));
			((Control)metroTabPageSponsorshipAndSupport).set_Margin(new Padding(0));
			((Control)metroTabPageSponsorshipAndSupport).set_Name("metroTabPageSponsorshipAndSupport");
			((Control)metroTabPageSponsorshipAndSupport).set_Size(new Size(622, 304));
			((TabPage)metroTabPageSponsorshipAndSupport).set_TabIndex(4);
			((Control)metroTabPageSponsorshipAndSupport).set_Text("| SPONSORLUK VE DESTEK");
			metroTabPageSponsorshipAndSupport.set_UseCustomBackColor(true);
			metroTabPageSponsorshipAndSupport.set_VerticalScrollbarBarColor(true);
			metroTabPageSponsorshipAndSupport.set_VerticalScrollbarHighlightOnWheel(false);
			metroTabPageSponsorshipAndSupport.set_VerticalScrollbarSize(10);
			((Control)metroTabPageForStreamers).set_BackColor(Color.FromArgb(219, 239, 255));
			((Control)metroTabPageForStreamers).get_Controls().Add((Control)(object)richTextBoxForStreamers);
			metroTabPageForStreamers.set_HorizontalScrollbarBarColor(true);
			metroTabPageForStreamers.set_HorizontalScrollbarHighlightOnWheel(false);
			metroTabPageForStreamers.set_HorizontalScrollbarSize(10);
			((TabPage)metroTabPageForStreamers).set_Location(new Point(4, 38));
			((Control)metroTabPageForStreamers).set_Margin(new Padding(0));
			((Control)metroTabPageForStreamers).set_Name("metroTabPageForStreamers");
			((Control)metroTabPageForStreamers).set_Size(new Size(622, 304));
			((TabPage)metroTabPageForStreamers).set_TabIndex(5);
			((Control)metroTabPageForStreamers).set_Text("| YAYINCILAR İÇİN");
			metroTabPageForStreamers.set_UseCustomBackColor(true);
			metroTabPageForStreamers.set_VerticalScrollbarBarColor(true);
			metroTabPageForStreamers.set_VerticalScrollbarHighlightOnWheel(false);
			metroTabPageForStreamers.set_VerticalScrollbarSize(10);
			notifyIconStreamNotifications.set_BalloonTipIcon((ToolTipIcon)1);
			notifyIconStreamNotifications.set_BalloonTipText("Twitch");
			notifyIconStreamNotifications.set_BalloonTipTitle("Yayındayım. İzlemek için tıklayın.");
			notifyIconStreamNotifications.set_Icon((Icon)((ResourceManager)(object)val).GetObject("notifyIconStreamNotifications.Icon"));
			notifyIconStreamNotifications.set_Text("Köfte Kalkan");
			notifyIconStreamNotifications.add_BalloonTipClicked((EventHandler)notifyIconStreamNotifications_BalloonTipClicked);
			notifyIconStreamNotifications.add_BalloonTipClosed((EventHandler)notifyIconStreamNotifications_BalloonTipClosed);
			notifyIconStreamNotifications.add_Click((EventHandler)notifyIconStreamNotifications_Click);
			notifyIconFreeGamesNotifications.set_BalloonTipIcon((ToolTipIcon)1);
			notifyIconFreeGamesNotifications.set_Icon((Icon)((ResourceManager)(object)val).GetObject("notifyIconFreeGamesNotifications.Icon"));
			notifyIconFreeGamesNotifications.set_Text("Köfte Kalkan");
			notifyIconFreeGamesNotifications.add_BalloonTipClicked((EventHandler)notifyIconFreeGamesNotifications_BalloonTipClicked);
			notifyIconFreeGamesNotifications.add_BalloonTipClosed((EventHandler)notifyIconFreeGamesNotifications_BalloonTipClosed);
			notifyIconFreeGamesNotifications.add_Click((EventHandler)notifyIconFreeGamesNotifications_Click);
			timerFreeGamesNotifications.set_Interval(300000);
			timerFreeGamesNotifications.add_Tick((EventHandler)timerFreeGamesNotifications_Tick);
			((Control)pictureBoxTopBanner).set_BackgroundImage((Image)((ResourceManager)(object)val).GetObject("pictureBoxTopBanner.BackgroundImage"));
			((Control)pictureBoxTopBanner).set_BackgroundImageLayout((ImageLayout)4);
			((Control)pictureBoxTopBanner).set_Location(new Point(20, 24));
			((Control)pictureBoxTopBanner).set_Name("pictureBoxTopBanner");
			((Control)pictureBoxTopBanner).set_Size(new Size(720, 91));
			pictureBoxTopBanner.set_TabIndex(9);
			pictureBoxTopBanner.set_TabStop(false);
			((Control)pictureBoxBottomBanner).set_BackgroundImage((Image)(object)Resources.BottomBanner);
			((Control)pictureBoxBottomBanner).set_BackgroundImageLayout((ImageLayout)4);
			((Control)pictureBoxBottomBanner).set_Location(new Point(20, 491));
			((Control)pictureBoxBottomBanner).set_Name("pictureBoxBottomBanner");
			((Control)pictureBoxBottomBanner).set_Size(new Size(720, 101));
			pictureBoxBottomBanner.set_TabIndex(7);
			pictureBoxBottomBanner.set_TabStop(false);
			((Control)pictureBoxBottomBanner).add_Click((EventHandler)pictureBoxBottomBanner_Click);
			((Control)btnYoutube).set_BackColor(Color.FromArgb(72, 65, 143));
			((Control)btnYoutube).set_BackgroundImage((Image)(object)Resources.Youtube);
			((Control)btnYoutube).set_BackgroundImageLayout((ImageLayout)4);
			((ButtonBase)btnYoutube).get_FlatAppearance().set_BorderSize(0);
			((ButtonBase)btnYoutube).get_FlatAppearance().set_MouseDownBackColor(Color.FromArgb(72, 65, 143));
			((ButtonBase)btnYoutube).get_FlatAppearance().set_MouseOverBackColor(Color.FromArgb(72, 65, 143));
			((ButtonBase)btnYoutube).set_FlatStyle((FlatStyle)0);
			((ButtonBase)btnYoutube).set_ImageIndex(0);
			((Control)btnYoutube).set_Location(new Point(657, 220));
			((Control)btnYoutube).set_Name("btnYoutube");
			((Control)btnYoutube).set_Size(new Size(83, 83));
			((Control)btnYoutube).set_TabIndex(13);
			((ButtonBase)btnYoutube).set_UseVisualStyleBackColor(false);
			((Control)btnYoutube).add_Click((EventHandler)btnYoutube_Click);
			((Control)btnYoutube).add_MouseLeave((EventHandler)btnYoutube_MouseLeave);
			((Control)btnYoutube).add_MouseHover((EventHandler)btnYoutube_MouseHover);
			((Control)pictureBoxPurple).set_BackColor(Color.FromArgb(72, 65, 143));
			((Control)pictureBoxPurple).set_Location(new Point(657, 131));
			((Control)pictureBoxPurple).set_Name("pictureBoxPurple");
			((Control)pictureBoxPurple).set_Size(new Size(83, 346));
			pictureBoxPurple.set_TabIndex(14);
			pictureBoxPurple.set_TabStop(false);
			((Control)btnTwitch).set_BackColor(Color.FromArgb(72, 65, 143));
			((Control)btnTwitch).set_BackgroundImage((Image)(object)Resources.Twitch);
			((Control)btnTwitch).set_BackgroundImageLayout((ImageLayout)4);
			((ButtonBase)btnTwitch).get_FlatAppearance().set_BorderSize(0);
			((ButtonBase)btnTwitch).get_FlatAppearance().set_MouseDownBackColor(Color.FromArgb(72, 65, 143));
			((ButtonBase)btnTwitch).get_FlatAppearance().set_MouseOverBackColor(Color.FromArgb(72, 65, 143));
			((ButtonBase)btnTwitch).set_FlatStyle((FlatStyle)0);
			((ButtonBase)btnTwitch).set_ImageIndex(0);
			((Control)btnTwitch).set_Location(new Point(657, 131));
			((Control)btnTwitch).set_Name("btnTwitch");
			((Control)btnTwitch).set_Size(new Size(83, 83));
			((Control)btnTwitch).set_TabIndex(15);
			((ButtonBase)btnTwitch).set_UseVisualStyleBackColor(false);
			((Control)btnTwitch).add_Click((EventHandler)btnTwitch_Click);
			((Control)btnTwitch).add_MouseLeave((EventHandler)btnTwitch_MouseLeave);
			((Control)btnTwitch).add_MouseHover((EventHandler)btnTwitch_MouseHover);
			((Control)btnInstagram).set_BackColor(Color.FromArgb(72, 65, 143));
			((Control)btnInstagram).set_BackgroundImage((Image)(object)Resources.Instagram);
			((Control)btnInstagram).set_BackgroundImageLayout((ImageLayout)4);
			((ButtonBase)btnInstagram).get_FlatAppearance().set_BorderSize(0);
			((ButtonBase)btnInstagram).get_FlatAppearance().set_MouseDownBackColor(Color.FromArgb(72, 65, 143));
			((ButtonBase)btnInstagram).get_FlatAppearance().set_MouseOverBackColor(Color.FromArgb(72, 65, 143));
			((ButtonBase)btnInstagram).set_FlatStyle((FlatStyle)0);
			((ButtonBase)btnInstagram).set_ImageIndex(0);
			((Control)btnInstagram).set_Location(new Point(657, 309));
			((Control)btnInstagram).set_Name("btnInstagram");
			((Control)btnInstagram).set_Size(new Size(83, 83));
			((Control)btnInstagram).set_TabIndex(16);
			((ButtonBase)btnInstagram).set_UseVisualStyleBackColor(false);
			((Control)btnInstagram).add_Click((EventHandler)btnInstagram_Click);
			((Control)btnInstagram).add_MouseLeave((EventHandler)btnInstagram_MouseLeave);
			((Control)btnInstagram).add_MouseHover((EventHandler)btnInstagram_MouseHover);
			((Control)btnTwitter).set_BackColor(Color.FromArgb(72, 65, 143));
			((Control)btnTwitter).set_BackgroundImage((Image)(object)Resources.Twitter);
			((Control)btnTwitter).set_BackgroundImageLayout((ImageLayout)4);
			((ButtonBase)btnTwitter).get_FlatAppearance().set_BorderSize(0);
			((ButtonBase)btnTwitter).get_FlatAppearance().set_MouseDownBackColor(Color.FromArgb(72, 65, 143));
			((ButtonBase)btnTwitter).get_FlatAppearance().set_MouseOverBackColor(Color.FromArgb(72, 65, 143));
			((ButtonBase)btnTwitter).set_FlatStyle((FlatStyle)0);
			((ButtonBase)btnTwitter).set_ImageIndex(0);
			((Control)btnTwitter).set_Location(new Point(657, 394));
			((Control)btnTwitter).set_Name("btnTwitter");
			((Control)btnTwitter).set_Size(new Size(83, 83));
			((Control)btnTwitter).set_TabIndex(17);
			((ButtonBase)btnTwitter).set_UseVisualStyleBackColor(false);
			((Control)btnTwitter).add_Click((EventHandler)btnTwitter_Click);
			((Control)btnTwitter).add_MouseLeave((EventHandler)btnTwitter_MouseLeave);
			((Control)btnTwitter).add_MouseHover((EventHandler)btnTwitter_MouseHover);
			timerStreamNotifications.set_Interval(60000);
			timerStreamNotifications.add_Tick((EventHandler)timerStreamNotifications_Tick);
			((Control)pictureBoxRastgeleReyiz).set_BackColor(Color.get_Transparent());
			((Control)pictureBoxRastgeleReyiz).set_Location(new Point(147, 89));
			((Control)pictureBoxRastgeleReyiz).set_Name("pictureBoxRastgeleReyiz");
			((Control)pictureBoxRastgeleReyiz).set_Size(new Size(168, 26));
			pictureBoxRastgeleReyiz.set_TabIndex(18);
			pictureBoxRastgeleReyiz.set_TabStop(false);
			((Control)pictureBoxRastgeleReyiz).add_Click((EventHandler)pictureBoxRastgeleReyiz_Click);
			((Control)pictureBoxFlameLionGames).set_BackColor(Color.get_Transparent());
			((Control)pictureBoxFlameLionGames).set_Location(new Point(657, 24));
			((Control)pictureBoxFlameLionGames).set_Name("pictureBoxFlameLionGames");
			((Control)pictureBoxFlameLionGames).set_Size(new Size(83, 91));
			pictureBoxFlameLionGames.set_TabIndex(19);
			pictureBoxFlameLionGames.set_TabStop(false);
			((Control)pictureBoxFlameLionGames).add_Click((EventHandler)pictureBoxFlameLionGames_Click);
			((Control)richTextBoxSettings).set_BackColor(Color.FromArgb(219, 239, 255));
			((TextBoxBase)richTextBoxSettings).set_BorderStyle((BorderStyle)0);
			((Control)richTextBoxSettings).set_Location(new Point(11, 78));
			((Control)richTextBoxSettings).set_Name("richTextBoxSettings");
			((TextBoxBase)richTextBoxSettings).set_ReadOnly(true);
			((Control)richTextBoxSettings).set_Size(new Size(608, 223));
			((Control)richTextBoxSettings).set_TabIndex(4);
			((Control)richTextBoxSettings).set_Text("");
			((RichTextBox)richTextBoxSettings).add_LinkClicked(new LinkClickedEventHandler(RichTextBoxLinkClicked));
			((Control)richTextBoxAbout2).set_BackColor(Color.FromArgb(219, 239, 255));
			((TextBoxBase)richTextBoxAbout2).set_BorderStyle((BorderStyle)0);
			((Control)richTextBoxAbout2).set_Location(new Point(3, 122));
			((Control)richTextBoxAbout2).set_Name("richTextBoxAbout2");
			((TextBoxBase)richTextBoxAbout2).set_ReadOnly(true);
			((Control)richTextBoxAbout2).set_Size(new Size(608, 175));
			((Control)richTextBoxAbout2).set_TabIndex(6);
			((Control)richTextBoxAbout2).set_Text("");
			((RichTextBox)richTextBoxAbout2).add_LinkClicked(new LinkClickedEventHandler(RichTextBoxLinkClicked));
			((Control)richTextBoxAbout1).set_BackColor(Color.FromArgb(219, 239, 255));
			((TextBoxBase)richTextBoxAbout1).set_BorderStyle((BorderStyle)0);
			((Control)richTextBoxAbout1).set_Location(new Point(3, 12));
			((Control)richTextBoxAbout1).set_Name("richTextBoxAbout1");
			((TextBoxBase)richTextBoxAbout1).set_ReadOnly(true);
			((RichTextBox)richTextBoxAbout1).set_ScrollBars((RichTextBoxScrollBars)0);
			((Control)richTextBoxAbout1).set_Size(new Size(608, 141));
			((Control)richTextBoxAbout1).set_TabIndex(5);
			((Control)richTextBoxAbout1).set_Text("");
			((RichTextBox)richTextBoxAbout1).add_LinkClicked(new LinkClickedEventHandler(RichTextBoxLinkClicked));
			((Control)richTextBoxSponsorshipAndSupport).set_BackColor(Color.FromArgb(219, 239, 255));
			((TextBoxBase)richTextBoxSponsorshipAndSupport).set_BorderStyle((BorderStyle)0);
			((Control)richTextBoxSponsorshipAndSupport).set_Location(new Point(7, 8));
			((Control)richTextBoxSponsorshipAndSupport).set_Name("richTextBoxSponsorshipAndSupport");
			((TextBoxBase)richTextBoxSponsorshipAndSupport).set_ReadOnly(true);
			((Control)richTextBoxSponsorshipAndSupport).set_Size(new Size(608, 289));
			((Control)richTextBoxSponsorshipAndSupport).set_TabIndex(6);
			((Control)richTextBoxSponsorshipAndSupport).set_Text("");
			((RichTextBox)richTextBoxSponsorshipAndSupport).add_LinkClicked(new LinkClickedEventHandler(RichTextBoxLinkClicked));
			((Control)richTextBoxForStreamers).set_BackColor(Color.FromArgb(219, 239, 255));
			((TextBoxBase)richTextBoxForStreamers).set_BorderStyle((BorderStyle)0);
			((Control)richTextBoxForStreamers).set_Location(new Point(7, 8));
			((Control)richTextBoxForStreamers).set_Name("richTextBoxForStreamers");
			((TextBoxBase)richTextBoxForStreamers).set_ReadOnly(true);
			((Control)richTextBoxForStreamers).set_Size(new Size(608, 289));
			((Control)richTextBoxForStreamers).set_TabIndex(6);
			((Control)richTextBoxForStreamers).set_Text("");
			((RichTextBox)richTextBoxForStreamers).add_LinkClicked(new LinkClickedEventHandler(RichTextBoxLinkClicked));
			((ContainerControl)this).set_AutoScaleDimensions(new SizeF(6f, 13f));
			((ContainerControl)this).set_AutoScaleMode((AutoScaleMode)1);
			((Form)this).set_ClientSize(new Size(759, 615));
			((Control)this).get_Controls().Add((Control)(object)pictureBoxFlameLionGames);
			((Control)this).get_Controls().Add((Control)(object)pictureBoxRastgeleReyiz);
			((Control)this).get_Controls().Add((Control)(object)btnTwitter);
			((Control)this).get_Controls().Add((Control)(object)btnInstagram);
			((Control)this).get_Controls().Add((Control)(object)btnTwitch);
			((Control)this).get_Controls().Add((Control)(object)pictureBoxTopBanner);
			((Control)this).get_Controls().Add((Control)(object)btnYoutube);
			((Control)this).get_Controls().Add((Control)(object)pictureBoxBottomBanner);
			((Control)this).get_Controls().Add((Control)(object)metroTabControl1);
			((Control)this).get_Controls().Add((Control)(object)pictureBoxPurple);
			((Form)this).set_Icon((Icon)((ResourceManager)(object)val).GetObject("$this.Icon"));
			((Control)this).set_MaximumSize(new Size(759, 615));
			((Control)this).set_MinimumSize(new Size(759, 615));
			((Control)this).set_Name("MainForm");
			((MetroForm)this).set_Resizable(false);
			((MetroForm)this).set_Style((MetroColorStyle)1);
			((Control)this).set_Text("Köfte Kalkan");
			((MetroForm)this).set_TextAlign((MetroFormTextAlign)1);
			((MetroForm)this).set_Theme((MetroThemeStyle)2);
			((Form)this).add_FormClosing(new FormClosingEventHandler(MainForm_FormClosing));
			((Form)this).add_Load((EventHandler)MainForm_Load);
			((Control)metroTabPageSettings).ResumeLayout(false);
			((Control)metroTabPageSettings).PerformLayout();
			((Control)metroTabPageShields).ResumeLayout(false);
			((Control)metroTabControl1).ResumeLayout(false);
			((Control)metroTabPageAbout).ResumeLayout(false);
			((Control)metroTabPageSponsorshipAndSupport).ResumeLayout(false);
			((Control)metroTabPageForStreamers).ResumeLayout(false);
			((ISupportInitialize)pictureBoxTopBanner).EndInit();
			((ISupportInitialize)pictureBoxBottomBanner).EndInit();
			((ISupportInitialize)pictureBoxPurple).EndInit();
			((ISupportInitialize)pictureBoxRastgeleReyiz).EndInit();
			((ISupportInitialize)pictureBoxFlameLionGames).EndInit();
			((Control)this).ResumeLayout(false);
		}
	}
}
