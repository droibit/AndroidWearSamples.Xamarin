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
using Android.Util;
using Android.Views.Animations;

namespace AndroidWear.Xamarin
{
	[Activity (Label = "@string/AppName", Icon="@drawable/Icon", Theme = "@style/AppTheme")]			
	public class RecipeActivity : Activity
	{
		private static readonly string TAG = "RecipeAssistant";

		private Recipe mRecipe;
		private ImageView mImageView;
		private TextView mTitleTextView;
		private TextView mSummaryTextView;
		private TextView mIngredientsTextView;
		private LinearLayout mStepsLayout;
		private string mRecipeName;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView(Resource.Layout.Recipe);

			mTitleTextView = FindViewById<TextView>(Resource.Id.RecipeTextTitle);
			mSummaryTextView = FindViewById<TextView>(Resource.Id.RecipeTextSummary);
			mImageView = FindViewById<ImageView>(Resource.Id.RecipeImageView);
			mIngredientsTextView = FindViewById<TextView>(Resource.Id.TextIngredients);
			mStepsLayout = FindViewById<LinearLayout>(Resource.Id.LayoutSteps);
		}
			
		protected override void OnStart()
		{
			base.OnStart();
			mRecipeName = Intent.GetStringExtra(Constants.RECIPE_NAME_TO_LOAD);
			if (Log.IsLoggable(TAG, LogPriority.Debug)) {
				Log.Debug(TAG, "Intent: " + Intent.ToString() + " " + mRecipeName);
			}
			LoadRecipeAsync();
		}
			
		public override bool OnCreateOptionsMenu(IMenu menu) {
			// Inflate the menu; this adds items to the action bar if it is present.
			MenuInflater.Inflate(Resource.Menu.Main, menu);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
			switch(item.ItemId) {
			case Resource.Id.ActionCook:
				StartCooking();
				return true;
			}
			return base.OnOptionsItemSelected(item);
		}

		private async void LoadRecipeAsync()
		{
			var jsonObject = await AssetUtils.LoadJSONAssetAsync(this, mRecipeName);
			if (jsonObject == null) {
				return;
			}

			mRecipe = Recipe.FromJson(this, jsonObject);
			if (mRecipe != null) {
				DisplayRecipe(mRecipe);
			}
		}

		private void DisplayRecipe(Recipe recipe)
		{
			var fadeIn = AnimationUtils.LoadAnimation(this, Android.Resource.Animation.FadeIn);
			mTitleTextView.Animation = fadeIn;
			mTitleTextView.Text = recipe.TitleText;
			mSummaryTextView.Text =recipe.SummaryText;

			if (recipe.RecipeImage != null) {
				mImageView.Animation = fadeIn;
				var recipeImage = AssetUtils.LoadBitmapAsset(this, recipe.RecipeImage);
				mImageView.SetImageBitmap(recipeImage);
			}
			mIngredientsTextView.Text = recipe.IngredientsText;

			FindViewById<View>(Resource.Id.IngredientsHeader).Animation = fadeIn;
			FindViewById<View>(Resource.Id.IngredientsHeader).Visibility = ViewStates.Visible;
			FindViewById<View>(Resource.Id.StepsHeader).Animation = fadeIn;
			FindViewById<View>(Resource.Id.StepsHeader).Visibility = ViewStates.Visible;

			var inf = LayoutInflater.From(this);
			mStepsLayout.RemoveAllViews();
			int stepNumber = 1;
			foreach (var step in recipe.RecipeSteps) {
				var view = inf.Inflate(Resource.Layout.StepItem, null);
				var iv = view.FindViewById<ImageView>(Resource.Id.StepImageView);
				if (step.StepImage == null) {
					iv.Visibility = ViewStates.Gone;
				} else {
					var stepImage = AssetUtils.LoadBitmapAsset(this, step.StepImage);
					iv.SetImageBitmap(stepImage);
				}
				view.FindViewById<TextView>(Resource.Id.TextStep).Text = (stepNumber++) + ". " + step.StepText;
				mStepsLayout.AddView(view);
			}
		}

		private void StartCooking()
		{
			var intent = new Intent(this, typeof(RecipeService));
			intent.SetAction(Constants.ACTION_START_COOKING);
			intent.PutExtra(Constants.EXTRA_RECIPE, mRecipe.ToBundle());
			StartService(intent);
		}
	}
}

