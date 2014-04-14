using System;
using Android.Preview.Support.Wearable.Notifications;

namespace Droibit.AndroidWear
{
	/// <summary>
	/// Base class for notification priority presets.
	/// </summary>
	public abstract class PriorityPreset : NamedPreset
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Droibit.AndroidWear.PriorityPreset"/> class.
		/// </summary>
		/// <param name="nameResId">Name res identifier.</param>
		public PriorityPreset (int nameResId) :
			base (nameResId)
		{
		}

		/// <summary>
		/// Apply the priority to a notification builder
		/// </summary>
		/// <param name="builder">Builder.</param>
		public abstract void Apply(WearableNotifications.Builder builder);
	}
}

