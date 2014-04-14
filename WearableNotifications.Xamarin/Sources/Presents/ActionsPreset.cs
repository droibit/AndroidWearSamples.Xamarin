using System;
using Android.Content;
using Android.Preview.Support.Wearable.Notifications;

namespace Droibit.AndroidWear
{
	/// <summary>
	/// Base class for notification actions presets.
	/// </summary>
	public abstract class ActionsPreset : NamedPreset
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Droibit.AndroidWear.ActionsPreset"/> class.
		/// </summary>
		/// <param name="nameResId">Name res identifier.</param>
		public ActionsPreset(int nameResId) :
			base(nameResId)
		{
		}

		/// <summary>
		/// Apply the priority to a notification builder
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="builder">Builder.</param>
		public abstract void Apply(Context context, WearableNotifications.Builder builder);
	}
}

