using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Droibit.AndroidWear
{
	public static class NotificationUtils
	{
		public const String ACTION_EXAMPLE = "droibit.androidwear.notifications.ACTION_EXAMPLE";
		public const String ACTION_ENABLE_MESSAGES = "droibit.androidwear.notifications.ACTION_ENABLE_MESSAGES";
		public const String ACTION_DISABLE_MESSAGES = "droibit.androidwear.notifications.ACTION_DISABLE_MESSAGES";

		public static readonly string EXTRA_MESSAGE = "droibit.androidwear.notifications.MESSAGE";
		public static readonly string EXTRA_REPLY = "droibit.androidwear.notifications.REPLY";

		public static PendingIntent GetExamplePendingIntent(Context context, int messageResId) {
			var intent = new Intent(ACTION_EXAMPLE)
				.SetClass(context, typeof(NotificationIntentReceiver));

			intent.PutExtra(EXTRA_MESSAGE, context.GetString(messageResId));
			return PendingIntent.GetBroadcast(context, messageResId /* requestCode */, intent,
				PendingIntentFlags.UpdateCurrent);
		}
	}
}

