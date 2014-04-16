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
using Android.Preview.Support.V4.App;
using Android.Support.V4.App;
using Android.Graphics;
using Android.Preview.Support.Wearable.Notifications;

namespace AndroidWear.Xamarin
{
	[Service]
	public class RecipeService : Service
	{
		public class LocalBinder : Binder {
			public RecipeService Service {
				get;
				set;
			}
		}

		private NotificationManagerCompat mNotificationManager;
		private Binder mBinder;
		private Recipe mRecipe;

		public RecipeService()
		{
			mBinder = new LocalBinder {
				Service = this,
			};
		}

		public override void OnCreate() 
		{
			mNotificationManager = NotificationManagerCompat.From(this);
		}
			
		public override IBinder OnBind(Intent intent) 
		{
			return mBinder;
		}
			
		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId) {
			if (intent.Action == Constants.ACTION_START_COOKING) {
				CreateNotification(intent);
				return StartCommandResult.Sticky;
			}
			return StartCommandResult.NotSticky;
		}

		private void CreateNotification(Intent intent) {
			mRecipe = Recipe.FromBundle (intent.GetBundleExtra (Constants.EXTRA_RECIPE));
			var notificationPages = new List<Notification> ();

			int stepCount = mRecipe.RecipeSteps.Count;
			for (int i = 0; i < stepCount; ++i) {
				var recipeStep = mRecipe.RecipeSteps [i];
				var style = new NotificationCompat.BigTextStyle ();
				style.BigText (recipeStep.StepText);
				style.SetBigContentTitle (Resources.GetString (Resource.String.StepCount, i + 1, stepCount));
				style.SetSummaryText ("");

				var builder = new NotificationCompat.Builder (this)
					.SetStyle (style);
				notificationPages.Add (builder.Build ());
			}

			{
				var builder = new NotificationCompat.Builder(this);

				if (mRecipe.RecipeImage != null) {
					var recipeImage = Bitmap.CreateScaledBitmap(
						AssetUtils.LoadBitmapAsset(this, mRecipe.RecipeImage),
						Constants.NOTIFICATION_IMAGE_WIDTH, Constants.NOTIFICATION_IMAGE_HEIGHT, false);
					builder.SetLargeIcon(recipeImage);
				}
				builder.SetContentTitle(mRecipe.TitleText)
					.SetContentText(mRecipe.SummaryText)
					.SetSmallIcon(Resource.Drawable.NotificationRecipe);

				var notification = new WearableNotifications.Builder(builder)
					.AddPages(notificationPages)
					.Build();
				mNotificationManager.Notify(Constants.NOTIFICATION_ID, notification);
			}
		}
	}
}

