using System;
using Android.App;
using Android.Widget;
using Android.OS;

namespace Kon.Stormozhev.Torch
{
    [Activity (Label = "Torch", MainLauncher = true)]
	public class MainActivity : Activity
	{
        ITorch torch;
		Button button;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);
			button = FindViewById<Button> (Resource.Id.myButton);
            torch = new Torch();

            if (!TorchSupported()) {
                button.Enabled = false;
                button.Text = "Not supported";
            }
            else
            {
                button.Click += delegate {
                    ToggleTorch();
                };

                torch.Toggled += delegate() {
                    button.Text = String.Format ("{0}", torch.On ? "Off" : "On");
                };
            }
		}

        protected override void OnResume()
        {
            base.OnResume();
            ToggleTorch();
        }

		protected override void OnDestroy ()
		{
			torch.Dispose ();
			base.OnDestroy ();
		}

		protected override void OnStop ()
		{
			torch.Dispose ();
			base.OnStop ();
		}

        bool TorchSupported()
        {
            try
            {
                return torch.SupportedByDevice;
            }
            catch(Exception ex)
            {
                HandleTorchError(ex);
            }

            return false;
        }

        void ToggleTorch()
        {
            try
            {
                torch.Toggle ();
            }
            catch(Exception ex)
            {
                HandleTorchError(ex);
            }
        }

        void HandleTorchError(Exception ex)
        {
            Android.Util.Log.Info("Torch", ex.Message);
            button.Enabled = false;
            button.Text = "Error occured. Please relaunch app.";
        }
	}
}
