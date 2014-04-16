using System;

namespace AndroidWear.Xamarin
{
	public static class Constants
	{
		public static readonly string RECIPE_LIST_FILE = "recipelist.json";
		public static readonly string RECIPE_NAME_TO_LOAD = "recipe_name";

		public static readonly string RECIPE_FIELD_LIST = "recipe_list";
		public static readonly string RECIPE_FIELD_IMAGE = "img";
		public static readonly string RECIPE_FIELD_INGREDIENTS = "ingredients";
		public static readonly string RECIPE_FIELD_NAME = "name";
		public static readonly string RECIPE_FIELD_SUMMARY = "summary";
		public static readonly string RECIPE_FIELD_STEPS = "steps";
		public static readonly string RECIPE_FIELD_TEXT = "text";
		public static readonly string RECIPE_FIELD_TITLE = "title";
		public static readonly string RECIPE_FIELD_STEP_TEXT = "step_text";
		public static readonly string RECIPE_FIELD_STEP_IMAGE = "step_image";

		public static readonly string ACTION_START_COOKING = "com.example.android.wearable.recipeassistant.START_COOKING";
		public static readonly string EXTRA_RECIPE = "recipe";

		public static readonly int NOTIFICATION_ID = 0;
		public static readonly int NOTIFICATION_IMAGE_WIDTH = 280;
		public static readonly int NOTIFICATION_IMAGE_HEIGHT = 280;
	}
}

