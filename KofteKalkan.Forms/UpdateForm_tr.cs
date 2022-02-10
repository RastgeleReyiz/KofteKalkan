using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using KofteKalkan.Helpers;
using MetroFramework;
using MetroFramework.Forms;

namespace KofteKalkan.Forms
{
	public class UpdateForm_tr : MetroForm
	{
		private string version;

		private GoogleAnalyticsHelper googleAnalytics;

		private IContainer components;

		private LinkLabel linkLabelDownload;

		private Button btnOK;

		public UpdateForm_tr(string version)
			: this()
		{
			InitializeComponent();
			this.version = version;
			((Control)this).set_Text("Güncelleme (" + version + ")");
			googleAnalytics = new GoogleAnalyticsHelper("UA-203687822-2", "555");
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			((Form)this).Close();
		}

		private async void linkLabelDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://rastgelereyiz.com/KofteKalkan/Download");
			await googleAnalytics.TrackEvent("Download", "Application", version);
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
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Expected O, but got Unknown
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Expected O, but got Unknown
			ComponentResourceManager val = new ComponentResourceManager(typeof(UpdateForm_tr));
			linkLabelDownload = new LinkLabel();
			btnOK = new Button();
			((Control)this).SuspendLayout();
			((Control)linkLabelDownload).set_BackColor(Color.get_Transparent());
			((Control)linkLabelDownload).set_Font(new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0));
			linkLabelDownload.set_LinkArea(new LinkArea(50, 7));
			((Control)linkLabelDownload).set_Location(new Point(23, 59));
			((Control)linkLabelDownload).set_Name("linkLabelDownload");
			((Control)linkLabelDownload).set_Size(new Size(475, 24));
			((Control)linkLabelDownload).set_TabIndex(1);
			linkLabelDownload.set_TabStop(true);
			((Control)linkLabelDownload).set_Text("Köfte Kalkan yeni sürümü çıkmıştır.  Güncellemeyi buradan indirebilirsiniz.");
			((Label)linkLabelDownload).set_TextAlign((ContentAlignment)32);
			linkLabelDownload.set_UseCompatibleTextRendering(true);
			linkLabelDownload.add_LinkClicked(new LinkLabelLinkClickedEventHandler(linkLabelDownload_LinkClicked));
			((ButtonBase)btnOK).set_FlatStyle((FlatStyle)0);
			((Control)btnOK).set_Location(new Point(222, 94));
			((Control)btnOK).set_Name("btnOK");
			((Control)btnOK).set_Size(new Size(75, 23));
			((Control)btnOK).set_TabIndex(2);
			((Control)btnOK).set_Text("OK");
			((ButtonBase)btnOK).set_UseVisualStyleBackColor(true);
			((Control)btnOK).add_Click((EventHandler)btnOK_Click);
			((ContainerControl)this).set_AutoScaleDimensions(new SizeF(6f, 13f));
			((ContainerControl)this).set_AutoScaleMode((AutoScaleMode)1);
			((Form)this).set_ClientSize(new Size(521, 140));
			((Form)this).set_ControlBox(false);
			((Control)this).get_Controls().Add((Control)(object)btnOK);
			((Control)this).get_Controls().Add((Control)(object)linkLabelDownload);
			((Form)this).set_Icon((Icon)((ResourceManager)(object)val).GetObject("$this.Icon"));
			((MetroForm)this).set_Movable(false);
			((Control)this).set_Name("UpdateForm_tr");
			((MetroForm)this).set_Resizable(false);
			((MetroForm)this).set_Style((MetroColorStyle)2);
			((Control)this).set_Text("Güncelleme");
			((MetroForm)this).set_TextAlign((MetroFormTextAlign)1);
			((Control)this).ResumeLayout(false);
		}
	}
}
