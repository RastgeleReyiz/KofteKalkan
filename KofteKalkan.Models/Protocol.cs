namespace KofteKalkan.Models
{
	public class Protocol
	{
		public string Name
		{
			get;
			set;
		}

		public int Value
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
