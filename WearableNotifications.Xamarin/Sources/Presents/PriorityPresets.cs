using System;
using Android.App;
using Android.Preview.Support.Wearable.Notifications;
using System.Collections.Generic;
using Android.Support.V4.App;

namespace Droibit.AndroidWear
{
	/// <summary>
	/// Collection of notification priority presets.
	/// </summary>
	public class PriorityPresets
	{
		public static readonly PriorityPreset DEFAULT = new SimplePriorityPreset();
		public static readonly PriorityPreset AMBIENT = new AmbientPriorityPreset();

		public static readonly PriorityPreset[] Presents = new PriorityPreset[] {
			AMBIENT,
			new SimplePriorityPreset(Resource.String.LowPriority, NotificationCompat.PriorityLow),
			DEFAULT,
			new SimplePriorityPreset(Resource.String.HighPriority, NotificationCompat.PriorityHigh),
			new SimplePriorityPreset(Resource.String.MaxPriority, NotificationCompat.PriorityMax)
		};

		/// <summary>
		/// Simple notification priority preset that sets a priority using
		/// <c>android.support.v4.app.NotificationCompat.Builder#setPriority</c>
		/// </summary>
		private class SimplePriorityPreset : PriorityPreset {
			private readonly int mPriority;

			public SimplePriorityPreset(int nameResId = Resource.String.DefaultPriority,
				int priority = NotificationCompat.PriorityDefault) :
			base(nameResId) 
			{
				mPriority = priority;
			}


			public override void Apply(WearableNotifications.Builder builder) {
				builder.CompatBuilder.SetPriority(mPriority);
			}
		}

		/// <summary>
		/// Notification priority preset that sets priority using
		/// <c>WearableNotifications.Builder#setMinPriority</c>
		/// </summary>
		private class AmbientPriorityPreset : PriorityPreset {
			public AmbientPriorityPreset() :
			base(Resource.String.AmbientMinPriority)
			{
			}

			public override void Apply(WearableNotifications.Builder builder) {
				builder.SetMinPriority();
			}
		}
	}
}

