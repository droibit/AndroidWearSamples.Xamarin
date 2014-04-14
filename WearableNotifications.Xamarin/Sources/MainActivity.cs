using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preview.Support.V4.App;

namespace Droibit.AndroidWear
{
	public interface IUpdateNotificationListener
	{
		void UpdateNotifications ();
	}

	[Activity (Label = "@string/ActivityLabel", MainLauncher = true, Icon="@drawable/Icon")]
	public class MainActivity : Activity, Handler.ICallback, IUpdateNotificationListener
	{
		private const int MSG_POST_NOTIFICATIONS = 0;
		private static readonly long POST_NOTIFICATIONS_DELAY_MS = 100;

		private Handler mHandler;
		private Spinner mPresetSpinner;
		private Spinner mPrioritySpinner;
		private Spinner mActionsSpinner;
		private CheckBox mIncludeLargeIconCheckbox;
		private CheckBox mLocalOnlyCheckbox;
		private CheckBox mIncludeContentIntentCheckbox;
		private CheckBox mIncludeContentIntentRequiredCheckbox;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			mHandler = new Handler(this);

			InitPresetSpinner();
			InitPrioritySpinner();
			InitActionsSpinner();
			InitIncludeLargeIconCheckbox();
			InitLocalOnlyCheckbox();
			InitIncludeContentIntentCheckbox();
		}

		protected override void OnResume()
		{
			base.OnResume();
			UpdateNotifications();
		}

		private void InitPresetSpinner()
		{
			mPresetSpinner = FindViewById<Spinner>(Resource.Id.PresetSpinner);
			mPresetSpinner.Adapter = new NamedPresetSpinnerArrayAdapter(this, NotificationPresets.Presents);
			mPresetSpinner.OnItemSelectedListener = new UpdateNotificationsOnItemSelectedListener(this);
		}

		private void InitPrioritySpinner() 
		{
			mPrioritySpinner = FindViewById<Spinner>(Resource.Id.PrioritySpinner);
			mPrioritySpinner.Adapter = new NamedPresetSpinnerArrayAdapter(this, PriorityPresets.Presents);
			mPrioritySpinner.SetSelection(PriorityPresets.Presents.ToList().IndexOf(PriorityPresets.DEFAULT));
			mPrioritySpinner.OnItemSelectedListener = new UpdateNotificationsOnItemSelectedListener(this);
		}

		private void InitActionsSpinner() 
		{
			mActionsSpinner = FindViewById<Spinner>(Resource.Id.ActionsSpinner);
			mActionsSpinner.Adapter = new NamedPresetSpinnerArrayAdapter(this, ActionsPresets.Presents);
			mActionsSpinner.OnItemSelectedListener = new UpdateNotificationsOnItemSelectedListener(this);
		}

		private void InitIncludeLargeIconCheckbox()
		{
			mIncludeLargeIconCheckbox = FindViewById<CheckBox>(Resource.Id.IncludeLargeIconCheckbox);
			mIncludeLargeIconCheckbox.SetOnCheckedChangeListener(new UpdateNotificationsOnCheckedChangeListener(this));
		}

		private void InitLocalOnlyCheckbox() 
		{
			mLocalOnlyCheckbox = FindViewById<CheckBox>(Resource.Id.LocalOnlyCheckbox);
			mLocalOnlyCheckbox.SetOnCheckedChangeListener(new UpdateNotificationsOnCheckedChangeListener(this));
		}

		private void InitIncludeContentIntentCheckbox() 
		{
			mIncludeContentIntentCheckbox = FindViewById<CheckBox>(Resource.Id.IncludeContentIntentCheckbox);
			mIncludeContentIntentRequiredCheckbox = FindViewById<CheckBox>(Resource.Id.IncludeContentIntentRequiredCheckbox);
			mIncludeContentIntentCheckbox.SetOnCheckedChangeListener(new UpdateNotificationsOnCheckedChangeListener(this));
		}

		/// <summary>
		/// Begin to re-post the sample notification(s).
		/// </summary>
		public void UpdateNotifications() 
		{
			// Disable messages to skip notification deleted messages during cancel.
			SendBroadcast(new Intent(NotificationUtils.ACTION_DISABLE_MESSAGES)
				.SetClass(this, typeof(NotificationIntentReceiver)));

			// Cancel all existing notifications to trigger fresh-posting behavior: For example,
			// switching from HIGH to LOW priority does not cause a reordering in Notification Shade.
			NotificationManagerCompat.From(this).CancelAll();

			// Post the updated notifications on a delay to avoid a cancel+post race condition
			// with notification manager.
			mHandler.RemoveMessages(MSG_POST_NOTIFICATIONS);
			mHandler.SendEmptyMessageDelayed(MSG_POST_NOTIFICATIONS, POST_NOTIFICATIONS_DELAY_MS);
		}

		/// <summary>
		/// Post the sample notification(s) using current options.
		/// </summary>
		private void PostNotifications() 
		{
			SendBroadcast(new Intent(NotificationUtils.ACTION_ENABLE_MESSAGES)
				.SetClass(this, typeof(NotificationIntentReceiver)));

			var preset = NotificationPresets.Presents[mPresetSpinner.SelectedItemPosition];
			var priorityPreset = PriorityPresets.Presents[mPrioritySpinner.SelectedItemPosition];
			var actionsPreset = ActionsPresets.Presents[mActionsSpinner.SelectedItemPosition];
			mIncludeContentIntentCheckbox.Visibility = preset.ContentIntentRequired ?
															ViewStates.Gone : ViewStates.Visible;
			mIncludeContentIntentRequiredCheckbox.Visibility = preset.ContentIntentRequired ?
																	ViewStates.Visible : ViewStates.Gone;
			var options = new NotificationPreset.BuildOptions {
				PriorityPreset = priorityPreset,
				ActionsPreset = actionsPreset,
				IncludeLargeIcon = mIncludeLargeIconCheckbox.Checked,
				IsLocalOnly = mLocalOnlyCheckbox.Checked,
				HasContentIntent = mIncludeContentIntentCheckbox.Checked
			};
			var notifications = preset.BuildNotifications(this, options);

			// Post new notifications
			for (int i = 0; i < notifications.Length; i++) {
				NotificationManagerCompat.From(this).Notify(i, notifications[i]);
			}
		}

		public bool HandleMessage(Message message) 
		{
			switch (message.What) {
			case MSG_POST_NOTIFICATIONS:
				PostNotifications();
				return true;
			}
			return false;
		}

		private class UpdateNotificationsOnItemSelectedListener : Java.Lang.Object, AdapterView.IOnItemSelectedListener
		{
			private readonly IUpdateNotificationListener mListener;

			public UpdateNotificationsOnItemSelectedListener(IUpdateNotificationListener listener)
			{
				mListener = listener;
			}

			public void OnItemSelected(AdapterView parent, View view, int position, long id)
			{
				mListener.UpdateNotifications();
			}

			public void OnNothingSelected(AdapterView adapterView) 
			{
			}
		}

		private class UpdateNotificationsOnCheckedChangeListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener 
		{
			private readonly IUpdateNotificationListener mListener;

			public UpdateNotificationsOnCheckedChangeListener(IUpdateNotificationListener listener)
			{
				mListener = listener;
			}
			
			public void OnCheckedChanged(CompoundButton compoundButton, bool c) 
			{
				mListener.UpdateNotifications();
			}
		}

		private class NamedPresetSpinnerArrayAdapter : ArrayAdapter<NamedPreset>
		{
			public NamedPresetSpinnerArrayAdapter(Context context, NamedPreset[] presets) :
			base(context, Resource.Layout.SimpleSpinnerItem, presets)
			{
			}
				
			public override View GetDropDownView(int position, View convertView, ViewGroup parent)
			{
				var view = base.GetDropDownView(position, convertView, parent) as TextView;
				view.Text = Context.GetString(GetItem(position).NameResId);
				return view;
			}

			public override View GetView(int position, View convertView, ViewGroup parent)
			{
				var view = LayoutInflater.From(Context).Inflate (
					                Android.Resource.Layout.SimpleSpinnerItem, parent, false) as TextView;
				view.Text = Context.GetString(GetItem(position).NameResId);
				return view;
			}
		}
	}
}
