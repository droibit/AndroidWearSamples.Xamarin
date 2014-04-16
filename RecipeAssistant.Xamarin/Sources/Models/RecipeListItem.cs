using System;
using Android.Graphics;

namespace AndroidWear.Xamarin
{
	public class RecipeListItem
	{
		public string Title {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public string Summary {
			get;
			set;
		}

		public Bitmap Image {
			get;
			set;
		}

		public override string ToString ()
		{
			return Name;
		}
	}
}

