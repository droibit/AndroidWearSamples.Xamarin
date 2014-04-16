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
using Org.Json;
using Android.Util;

namespace AndroidWear.Xamarin
{
	public class RecipeListAdapter : ArrayAdapter<RecipeListItem>
	{
		private static readonly String TAG = "RecipeListAdapter";

		public RecipeListAdapter (Context context) :
			base (context, Resource.Layout.ListItem)
		{
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView;
			if (view == null) {
				view = LayoutInflater.From(Context).Inflate(Resource.Layout.ListItem, null);
			}
			var item = GetItem(position) as RecipeListItem;
			var titleView = view.FindViewById<TextView>(Resource.Id.TextTitle);
			var summaryView = view.FindViewById<TextView>(Resource.Id.TextSummary);
			var iv = view.FindViewById<ImageView>(Resource.Id.ImageView);

			titleView.Text = item.Title;
			summaryView.Text = item.Summary;
			if (item.Image != null) {
				iv.SetImageBitmap(item.Image);
			} else {
				iv.SetImageDrawable(Context.Resources.GetDrawable(Resource.Drawable.Noimage));
			}
			return view;
		}

		public async void LoadRecipeList()
		{
			var jsonObject = await AssetUtils.LoadJSONAssetAsync(Context, Constants.RECIPE_LIST_FILE);
			if (jsonObject != null) {
				ParseJson (jsonObject).ForEach (i => Add (i));
			}
		}

		private List<RecipeListItem> ParseJson(JSONObject json)
		{
			var result = new List<RecipeListItem>();
			try {
				var items = json.GetJSONArray(Constants.RECIPE_FIELD_LIST);
				for (int i = 0; i < items.Length(); i++) {
					var item = items.GetJSONObject(i);
					var parsed = new RecipeListItem();
					parsed.Name = item.GetString(Constants.RECIPE_FIELD_NAME);
					parsed.Title = item.GetString(Constants.RECIPE_FIELD_TITLE);
					if (item.Has(Constants.RECIPE_FIELD_IMAGE)) {
						var imageFile = item.GetString(Constants.RECIPE_FIELD_IMAGE);
						parsed.Image = AssetUtils.LoadBitmapAsset(Context, imageFile);
					}
					parsed.Summary = item.GetString(Constants.RECIPE_FIELD_SUMMARY);
					result.Add(parsed);
				}
			} catch (JSONException e) {
				Log.Error(TAG, "Failed to parse recipe list: " + e);
			}
			return result;
		}
	}
}

