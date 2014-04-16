using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;

namespace AndroidWear.Xamarin
{
	[Activity (Label = "@string/AppName", Icon="@drawable/Icon", MainLauncher = true, Theme = "@style/AppTheme")]
	public class MainActivity : ListActivity
	{
		private static readonly string TAG = "RecipeAssistant";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Android.Resource.Layout.ListContent);

			var listAdapter = new RecipeListAdapter (this);
			listAdapter.LoadRecipeList ();
			ListAdapter = listAdapter;
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id) {
			if (Log.IsLoggable(TAG, LogPriority.Debug)) {
				Log.Debug(TAG , "onListItemClick " + position);
			}

			var itemName = ListAdapter.GetItem(position).ToString();
			var intent = new Intent(ApplicationContext, typeof(RecipeActivity));
			intent.PutExtra(Constants.RECIPE_NAME_TO_LOAD, itemName);
			StartActivity(intent);
		}
	}
}


