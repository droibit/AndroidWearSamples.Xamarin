using System;

namespace Droibit.AndroidWear
{
	/// <summary>
	/// Base class for presets that have a simple name to display.
	/// </summary>
	public abstract class NamedPreset
	{
		/// <summary>
		/// Gets the name res identifier.
		/// </summary>
		/// <value>The name res identifier.</value>
		public int NameResId {
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Droibit.AndroidWear.NamedPreset"/> class.
		/// </summary>
		/// <param name="nameResId">Name res identifier.</param>
		public NamedPreset (int nameResId)
		{
			NameResId = nameResId;
		}
	}
}

