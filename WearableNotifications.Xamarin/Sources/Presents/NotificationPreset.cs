using System;
using Android.App;
using Android.Content;

namespace Droibit.AndroidWear
{
	/// <summary>
	/// Base class for notification preset generators.
	/// </summary>
	public abstract class NotificationPreset : NamedPreset
	{
		#region Inner Classes

		public class BuildOptions
		{
			/// <summary>
			/// Gets the priority preset.
			/// </summary>
			/// <value>The priority preset.</value>
			public PriorityPreset PriorityPreset {
				get;
				set;
			}

			/// <summary>
			/// Gets the actions preset.
			/// </summary>
			/// <value>The actions preset.</value>
			public ActionsPreset ActionsPreset {
				get;
				set;
			}

			/// <summary>
			/// Gets a value indicating whether this <see cref="Droibit.AndroidWear.NotificationPreset+BuildOption"/> include
			/// large icon.
			/// </summary>
			/// <value><c>true</c> if include large icon; otherwise, <c>false</c>.</value>
			public bool IncludeLargeIcon {
				get;
				set;
			}

			/// <summary>
			/// Gets a value indicating whether this instance is local only.
			/// </summary>
			/// <value><c>true</c> if this instance is local only; otherwise, <c>false</c>.</value>
			public bool IsLocalOnly {
				get;
				set;
			}

			/// <summary>
			/// Sets a value indicating whether this instance has content intent.
			/// </summary>
			/// <value><c>true</c> if this instance has content intent; otherwise, <c>false</c>.</value>
			public bool HasContentIntent {
				get;
				set;
			}
		}

		#endregion

		/// <summary>
		/// Whether the content for this preset is required.
		/// </summary>
		/// <value><c>true</c> if content intent required; otherwise, <c>false</c>.</value>
		public virtual bool ContentIntentRequired {
			get {
				return false;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Droibit.AndroidWear.NotificationPreset"/> class.
		/// </summary>
		/// <param name="nameResId">Name res identifier.</param>
		public NotificationPreset (int nameResId) :
			base(nameResId)
		{
		}

		/// <summary>
		/// Builds the notifications.
		/// </summary>
		/// <returns>The notifications.</returns>
		/// <param name="context">Context.</param>
		/// <param name="options">Options.</param>
		public abstract Notification[] BuildNotifications(Context context, BuildOptions options);
	}
}

