﻿using System;
using SDebug = System.Diagnostics.Debug;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Media;
using Plugin.Permissions;

namespace PartyTimeline.Droid
{
	[Activity(Label = "PartyTimeline.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override async void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

		    await CrossMedia.Current.Initialize();

            global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App());
		}
	    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
	    {
	        PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
	    }

    }
}