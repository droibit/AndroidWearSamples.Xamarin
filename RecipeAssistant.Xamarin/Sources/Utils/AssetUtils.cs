using System;
using Android.Content;
using System.IO;
using Android.Graphics;
using System.Text;
using System.Threading.Tasks;
using Org.Json;
using Android.Util;

namespace AndroidWear.Xamarin
{
	public static class AssetUtils
	{
		private static readonly string TAG = "RecipeAssistant";

		public static async Task<JSONObject> LoadJSONAssetAsync(Context context, String asset)
		{
			try {
				using (var streamReader = new StreamReader (context.Assets.Open (asset))) {
					var sb = new StringBuilder ();
					string line = "";
					while ((line = await streamReader.ReadLineAsync ()) != null) {
						sb.Append (line);
					}
					return new JSONObject (sb.ToString ());
				}
			} catch(IOException e) {
				Log.Error(TAG, "Failed to load asset " + asset + ": " + e);
			}
			return null;
		}

		public static Bitmap LoadBitmapAsset(Context context, String asset)
		{
			try {
				var bitmap = BitmapFactory.DecodeStream(context.Assets.Open(asset));
				return bitmap;
			} catch (IOException e) {
				Log.Error(TAG, "Failed to load asset " + asset + ": " + e);
			}
			return null;
		}
	}
}

