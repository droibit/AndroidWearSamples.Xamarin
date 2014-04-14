using System;
using Android.Content;
using Android.Support.V4.App;
using Android.Preview.Support.Wearable.Notifications;
using Android.Graphics;
using Android.App;

namespace Droibit.AndroidWear
{
	using BuildOptions = Droibit.AndroidWear.NotificationPreset.BuildOptions;

	/// <summary>
	/// Collection of notification builder presets.
	/// </summary>
	public class NotificationPresets
	{
		private const String EXAMPLE_GROUP_KEY = "example";

		public static NotificationPreset[] Presents = new NotificationPreset[] {
			new BasicNotificationPreset(),
			new InboxNotificationPreset(),
			new BigPictureNotificationPreset(),
			new BigTextNotificationPreset(),
			new BigActionNotificationPreset(),
			new MultiplePageNotificationPreset(),
			new NotificationBundlePreset()
		};

		private static WearableNotifications.Builder BuildBasicNotification(Context context, BuildOptions options) {
			var builder = new NotificationCompat.Builder(context)
				.SetContentTitle(context.GetString(Resource.String.ExampleContentTitle))
				.SetContentText(context.GetString(Resource.String.ExampleContentText))
				.SetSmallIcon(Resource.Drawable.NotificationApp)
				.SetDeleteIntent(NotificationUtils.GetExamplePendingIntent(
					context, Resource.String.ExampleNotificationDeleted));

			var wearableBuilder = new WearableNotifications.Builder(builder);
			options.ActionsPreset.Apply(context, wearableBuilder);
			options.PriorityPreset.Apply(wearableBuilder);
			if (options.IncludeLargeIcon) {
				builder.SetLargeIcon(BitmapFactory.DecodeResource(
					context.Resources, Resource.Drawable.ExampleLargeIcon));
			}
			if (options.IsLocalOnly) {
				wearableBuilder.SetLocalOnly(true);
			}
			if (options.HasContentIntent) {
				builder.SetContentIntent(NotificationUtils.GetExamplePendingIntent(context,
					Resource.String.ContentIntentClicked));
			}
			return wearableBuilder;
		}


		private class BasicNotificationPreset : NotificationPreset {
			public BasicNotificationPreset() :
			base (Resource.String.BasicExample) {
			}
				
			public override Notification[] BuildNotifications(Context context, BuildOptions options) {
				var notification = BuildBasicNotification(context, options)
					.Build();
				return new Notification[] { notification };
			}
		}

		private class InboxNotificationPreset : NotificationPreset {
			public InboxNotificationPreset() :
			base(Resource.String.InboxExample)
			{
			}
				
			public override Notification[] BuildNotifications(Context context, BuildOptions options) {
				var style = new NotificationCompat.InboxStyle();
				style.AddLine(context.GetString(Resource.String.InboxStyleExampleLine1));
				style.AddLine(context.GetString(Resource.String.InboxStyleExampleLine2));
				style.AddLine(context.GetString(Resource.String.InboxStyleExampleLine3));
				style.SetBigContentTitle(context.GetString(Resource.String.InboxStyleExampleTitle));
				style.SetSummaryText(context.GetString(Resource.String.InboxStyleExampleSummaryText));

				var builder = BuildBasicNotification(context, options);
				builder.CompatBuilder.SetStyle(style);
				return new Notification[] { builder.Build() };
			}
		}

		private class BigPictureNotificationPreset : NotificationPreset {
			public BigPictureNotificationPreset() :
			base (Resource.String.BigPictureExample)
			{
			}

			public override Notification[] BuildNotifications(Context context, BuildOptions options) {
				var style = new NotificationCompat.BigPictureStyle();
				style.BigPicture(BitmapFactory.DecodeResource(context.Resources,
					Resource.Drawable.ExampleBigPicture));
				style.SetBigContentTitle(context.GetString(Resource.String.BigPictureStyleExampleTitle));
				style.SetSummaryText(context.GetString(
					Resource.String.BigPictureStyleExampleSummaryText));

				var builder = BuildBasicNotification(context, options);
				builder.CompatBuilder.SetStyle(style);
				return new Notification[] { builder.Build() };
			}
		}

		private class BigTextNotificationPreset : NotificationPreset {
			public BigTextNotificationPreset() :
			base(Resource.String.BigTextExample)
			{
			}
				
			public override Notification[] BuildNotifications(Context context, BuildOptions options) {
				var style = new NotificationCompat.BigTextStyle();
				style.BigText(context.GetString(Resource.String.BigTextExampleBigText));
				style.SetBigContentTitle(context.GetString(Resource.String.BigTextExampleTitle));
				style.SetSummaryText(context.GetString(Resource.String.BigTextExampleSummaryText));

				var builder = BuildBasicNotification(context, options);
				builder.CompatBuilder.SetStyle(style);
				return new Notification[] { builder.Build() };
			}
		}

		private class BigActionNotificationPreset : NotificationPreset {
			public BigActionNotificationPreset() :
			base (Resource.String.BigActionExample)
			{
			}

			public override bool ContentIntentRequired 
			{
				get {
					return true;
				}
			}
				
			public override Notification[] BuildNotifications(Context context, BuildOptions options) {
				var builder = BuildBasicNotification(context, options)
					.SetBigActionIcon(Resource.Drawable.NotificationApp,
						context.GetString(Resource.String.IconSubtext))
					.SetHintHideIcon(true);
				builder.CompatBuilder.SetContentIntent(NotificationUtils.GetExamplePendingIntent(
					context, Resource.String.ContentIntentClicked));
				return new Notification[] { builder.Build() };
			}
		}

		private class MultiplePageNotificationPreset : NotificationPreset {
			public MultiplePageNotificationPreset() :
			base(Resource.String.MultiplePageExample)
			{
			}

			public override Notification[] BuildNotifications(Context context, BuildOptions options) {
				var secondPage = new NotificationCompat.Builder(context)
					.SetContentTitle(context.GetString(Resource.String.SecondPageContentTitle))
					.SetContentText(context.GetString(Resource.String.SecondPageContentText))
					.Build();

				var notification = BuildBasicNotification(context, options)
					.AddPage(secondPage)
					.Build();
				return new Notification[] { notification };
			}
		}

		private class NotificationBundlePreset : NotificationPreset {
			public NotificationBundlePreset() :
			base(Resource.String.NotificationBundleExample)
			{
			}

			public override Notification[] BuildNotifications(Context context, BuildOptions options) {
				var childBuilder1 = new NotificationCompat.Builder(context)
					.SetContentTitle(context.GetString(Resource.String.FirstChildContentTitle))
					.SetContentText(context.GetString(Resource.String.FirstChildContentText));
				var child1 = new WearableNotifications.Builder(childBuilder1)
					.SetGroup(EXAMPLE_GROUP_KEY, 0)
					.Build();

				var childBuilder2 = new NotificationCompat.Builder(context)
					.SetContentTitle(context.GetString(Resource.String.SecondChildContentTitle))
					.SetContentText(context.GetString(Resource.String.SecondChildContentText))
					.AddAction(Resource.Drawable.NotificationApp,
						context.GetString(Resource.String.SecondChildAction),
						NotificationUtils.GetExamplePendingIntent(
							context, Resource.String.SecondChildActionClicked));
				var child2 = new WearableNotifications.Builder(childBuilder2)
					.SetGroup(EXAMPLE_GROUP_KEY, 1)
					.Build();

				var summary = BuildBasicNotification(context, options)
					.SetGroup(EXAMPLE_GROUP_KEY, WearableNotifications.GroupOrderSummary)
					.Build();

				return new Notification[] { summary, child1, child2 };
			}
		}
	}
}

