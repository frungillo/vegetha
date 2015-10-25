using System;
using Android.App;

namespace vegethacust
{
	[Service]
	public class serviceControlloVegetha : Service
	{
		public serviceControlloVegetha ()
		{
		}

		#region implemented abstract members of Service

		public override Android.OS.IBinder OnBind (Android.Content.Intent intent)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

