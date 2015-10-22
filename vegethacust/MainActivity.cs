using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace vegethacust
{
	[Activity (Label = "vegethacust", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			Button btnLeggi = FindViewById<Button> (Resource.Id.btnLeggi);
			TextView txtMonitor = FindViewById<TextView> (Resource.Id.txtMonitor);
			button.Click += delegate {
				customers.CreaMaster();
				customers c = new customers();
				c.Cognome = "La Matta";
				c.Nome = "Sabrina";
				c.DataIscrizione = new DateTime(2015,10,02);
				c.Tipo = "T";
				c.Numero_tessera = 11584;
				c.Aggiungi();


			};

			btnLeggi.Click+= delegate {
				txtMonitor.Text= "";
				List<customers> ls = customers.GetTutti();
				foreach(customers c in ls) {
					txtMonitor.Append(c.Cognome+" "+c.Nome+ " tipo:"+ c.Tipo+" ("+c.DataIscrizione.ToShortDateString()+")"+"\n");
				}
			};
		}
	}
}


