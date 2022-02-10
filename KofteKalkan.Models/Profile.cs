namespace KofteKalkan.Models
{
	public class Profile
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

		public Rule InboundRule
		{
			get;
			set;
		}

		public Rule OutboundRule
		{
			get;
			set;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
