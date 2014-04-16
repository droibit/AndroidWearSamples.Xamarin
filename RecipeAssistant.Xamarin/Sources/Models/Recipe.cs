using System;
using Android.OS;
using System.Linq;
using System.Collections.Generic;
using Android.Content;
using Org.Json;
using Android.Util;

namespace AndroidWear.Xamarin
{
	public class Recipe
	{
		#region Inner Classes

		public class RecipeStep 
		{
			public string StepImage {
				get;
				set;
			}

			public string StepText {
				get;
				set;
			}
				
			public RecipeStep() 
			{
			}

			public Bundle ToBundle() 
			{
				var bundle = new Bundle();
				bundle.PutString(Constants.RECIPE_FIELD_STEP_TEXT, StepText);
				bundle.PutString(Constants.RECIPE_FIELD_STEP_IMAGE, StepImage);
				return bundle;
			}

			public static RecipeStep FromBundle(Bundle bundle)
			{
				var recipeStep = new RecipeStep {
					StepText = bundle.GetString (Constants.RECIPE_FIELD_STEP_TEXT),
					StepImage = bundle.GetString (Constants.RECIPE_FIELD_STEP_IMAGE),
				};
				return recipeStep;
			}
		}

		#endregion

		private const string TAG = "RecipeAssistant";

		public string TitleText {
			get;
			set;
		}

		public string SummaryText {
			get;
			set;
		}

		public string RecipeImage {
			get;
			set;
		}

		public string IngredientsText {
			get;
			set;
		}

		public List<RecipeStep> RecipeSteps {
			get;
			set;
		}
			
		public Recipe ()
		{
			RecipeSteps = new List<RecipeStep> ();
		}

		public Bundle ToBundle()
		{
			var bundle = new Bundle();
			bundle.PutString(Constants.RECIPE_FIELD_TITLE, TitleText);
			bundle.PutString(Constants.RECIPE_FIELD_SUMMARY, SummaryText);
			bundle.PutString(Constants.RECIPE_FIELD_IMAGE, RecipeImage);
			bundle.PutString(Constants.RECIPE_FIELD_INGREDIENTS, IngredientsText);

			if (RecipeSteps != null) {
				var stepBundles = RecipeSteps.Select (rs => rs.ToBundle ()).ToArray ();
				bundle.PutParcelableArray (Constants.RECIPE_FIELD_STEPS, stepBundles);
			}
			return bundle;
		}

		public static Recipe FromJson(Context context, JSONObject json) {
			var recipe = new Recipe();
			try {
				recipe.TitleText = json.GetString(Constants.RECIPE_FIELD_TITLE);
				recipe.SummaryText = json.GetString(Constants.RECIPE_FIELD_SUMMARY);
				if (json.Has(Constants.RECIPE_FIELD_IMAGE)) {
					recipe.RecipeImage = json.GetString(Constants.RECIPE_FIELD_IMAGE);
				}
				var ingredients = json.GetJSONArray(Constants.RECIPE_FIELD_INGREDIENTS);
				recipe.IngredientsText = "";
				for (int i = 0; i < ingredients.Length(); i++) {
					recipe.IngredientsText += " - "
						+ ingredients.GetJSONObject(i).GetString(Constants.RECIPE_FIELD_TEXT) + "\n";
				}

				var steps = json.GetJSONArray(Constants.RECIPE_FIELD_STEPS);
				for (int i = 0; i < steps.Length(); i++) {
					var step = steps.GetJSONObject(i);
					var recipeStep = new RecipeStep();
					recipeStep.StepText = step.GetString(Constants.RECIPE_FIELD_TEXT);
					if (step.Has(Constants.RECIPE_FIELD_IMAGE)) {
						recipeStep.StepImage = step.GetString(Constants.RECIPE_FIELD_IMAGE);
					}
					recipe.RecipeSteps.Add(recipeStep);
				}
			} catch (JSONException e) {
				Log.Error(TAG, "Error loading recipe: " + e);
				return null;
			}
			return recipe;
		}

		public static Recipe FromBundle(Bundle bundle)
		{
			var recipe = new Recipe {
				TitleText = bundle.GetString (Constants.RECIPE_FIELD_TITLE),
				SummaryText = bundle.GetString (Constants.RECIPE_FIELD_SUMMARY),
				RecipeImage = bundle.GetString (Constants.RECIPE_FIELD_IMAGE),
				IngredientsText = bundle.GetString (Constants.RECIPE_FIELD_INGREDIENTS)
			};

			var stepBundles = bundle.GetParcelableArray(Constants.RECIPE_FIELD_STEPS);
			if (stepBundles != null) {
				recipe.RecipeSteps = stepBundles.Cast<Bundle> ()
					.Select (b => RecipeStep.FromBundle (b))
					.ToList();
			}
			return recipe;
		}
	}
}

