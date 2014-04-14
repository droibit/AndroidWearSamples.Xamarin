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
	[BroadcastReceiver]
	[IntentFilter(new[] {NotificationUtils.ACTION_EXAMPLE,
						 NotificationUtils.ACTION_ENABLE_MESSAGES, 
						 NotificationUtils.ACTION_DISABLE_MESSAGES})]
	public class NotificationIntentReceiver : BroadcastReceiver
	{
		private bool mEnableMessages = true;

		public override void OnReceive (Context context, Intent intent)
		{
			if (intent.Action == NotificationUtils.ACTION_EXAMPLE) {
				if (mEnableMessages) {
					var message = intent.GetStringExtra(NotificationUtils.EXTRA_MESSAGE);
					var replyMessage = intent.GetStringExtra(NotificationUtils.EXTRA_REPLY);
					if (!String.IsNullOrEmpty(replyMessage)) {
						message = String.Format("{0} : \"{1}\"", message, replyMessage);
					}
					Toast.MakeText(context, message, ToastLength.Short).Show();
				}
			} else if (intent.Action == NotificationUtils.ACTION_ENABLE_MESSAGES) {
				mEnableMessages = true;
			} else if (intent.Action == NotificationUtils.ACTION_DISABLE_MESSAGES) {
				mEnableMessages = false;
			}
		}
	}
}

