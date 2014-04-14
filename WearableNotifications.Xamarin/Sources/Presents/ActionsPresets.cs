using System;
using Android.Content;
using Android.Preview.Support.Wearable.Notifications;
using System.Collections.Generic;

namespace Droibit.AndroidWear
{
	/// <summary>
	/// Collection of notification actions presets.
	/// </summary>
	public class ActionsPresets
	{
		public static readonly ActionsPreset[] Presents = new ActionsPreset[] {
			new NoActionsPreset(),
			new SingleActionPreset(),
			new ReplyActionPreset(),
			new ReplyWithChoicesActionPreset()
		};

		#region Inner Classes

		private class NoActionsPreset : ActionsPreset {
			public NoActionsPreset() :
			base(Resource.String.NoActions) 
			{
			}
				
			public override void Apply(Context context, WearableNotifications.Builder builder) {
			}
		}

		private class SingleActionPreset : ActionsPreset {
			public SingleActionPreset() :
			base(Resource.String.SingleAction) 
			{
			}

			public override void Apply(Context context, WearableNotifications.Builder builder) {
				var action = new WearableNotifications.Action.Builder(
									Resource.Drawable.FullAction,
									context.GetString(Resource.String.ExampleAction),
									NotificationUtils.GetExamplePendingIntent(context, Resource.String.ExampleActionClicked))
								.Build();
				builder.AddAction(action);
			}
		}

		private class ReplyActionPreset : ActionsPreset {
			public ReplyActionPreset() :
			base(Resource.String.ReplyAction){
			}
				
			public override void Apply(Context context, WearableNotifications.Builder builder) {
				var remoteInput = new RemoteInput.Builder(NotificationUtils.EXTRA_REPLY)
										.SetLabel(context.GetString(Resource.String.ExampleReplyLabel))
										.Build();
				var action = new WearableNotifications.Action.Builder(
									Resource.Drawable.FullReply,
									context.GetString(Resource.String.ExampleReplyAction),
									NotificationUtils.GetExamplePendingIntent(context, Resource.String.ExampleReplyActionClicked))
								.AddRemoteInput(remoteInput)
								.Build();
				builder.AddAction(action);
			}
		}

		private class ReplyWithChoicesActionPreset : ActionsPreset {
			public ReplyWithChoicesActionPreset() :
			base(Resource.String.ReplyActionWithChoices) 
			{
			}

			public override void Apply(Context context, WearableNotifications.Builder builder) {
				var remoteInput = new RemoteInput.Builder(NotificationUtils.EXTRA_REPLY)
										.SetLabel(context.GetString(Resource.String.ExampleReplyAnswerLabel))
										.SetChoices(new String[] { context.GetString(Resource.String.Yes),
																   context.GetString(Resource.String.No), 
																   context.GetString(Resource.String.Maybe) })
										.Build();
				var action = new WearableNotifications.Action.Builder(
										Resource.Drawable.FullReply,
										context.GetString(Resource.String.ExampleReplyAction),
										NotificationUtils.GetExamplePendingIntent(context, Resource.String.ExampleReplyActionClicked))
									.AddRemoteInput(remoteInput)
									.Build();
				builder.AddAction(action);
			}
		}

		#endregion
	}
}

