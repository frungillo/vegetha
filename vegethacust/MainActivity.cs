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
	[Activity (Label = "Vegetha CM", MainLauncher = true, Icon = "@drawable/vegetha_512")]
	public class MainActivity : Activity
	{
		EditText txtRicerca;
		ListView lstClienti;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetTheme (Android.Resource.Style.ThemeHoloLightNoActionBar);
			SetContentView (Resource.Layout.Main);
			customers.CreaMaster();


			Button button = FindViewById<Button> (Resource.Id.myButton);
			Button btnLeggi = FindViewById<Button> (Resource.Id.btnLeggi);
			lstClienti = FindViewById<ListView> (Resource.Id.listView1);
			txtRicerca = FindViewById<EditText> (Resource.Id.txtRicerca);
			button.Click += delegate {
				Intent frmAggiungi = new Intent(this, typeof(frmAggiungiUtente));
				StartActivityForResult(frmAggiungi,0);
				/*
				customers.CreaMaster();
				customers c = new customers();
				c.Cognome = "La Matta";
				c.Nome = "Sabrina";
				c.DataIscrizione = new DateTime(2015,10,02);
				c.Tipo = "T";
				c.Numero_tessera = 11584;
				c.Aggiungi();
				*/
			};

			btnLeggi.Click+= delegate {
				
				List<customers> ls = customers.GetTutti();
				lstClienti.Adapter = new ClientiListAdapter(this, ls);

			};
			txtRicerca.TextChanged += TxtRicerca_TextChanged;
			lstClienti.ItemClick += LstClienti_ItemClick;

			if (this.Intent.HasExtra ("cerca")) {
				txtRicerca.Text = "scad:" + this.Intent.GetStringExtra ("cerca");
				lstClienti.Adapter = new ClientiListAdapter (this, customers.GetByValues (txtRicerca.Text));
			} else {
				lstClienti.Adapter = new ClientiListAdapter (this, customers.GetTutti ());
			}

		}

		void LstClienti_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
		   customers c = lstClienti.GetItemAtPosition (e.Position).Cast<customers> ();
			Intent frmAggiungi = new Intent (this, typeof(frmAggiungiUtente));
			frmAggiungi.PutExtra ("numtessera", c.Numero_tessera);
			StartActivityForResult (frmAggiungi,0);
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if (resultCode == Result.Ok) {
				List<customers> ls = customers.GetTutti();
				lstClienti.Adapter = new ClientiListAdapter(this, ls);
			}
		}

		void TxtRicerca_TextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
			
			List<customers> ls = customers.GetByValues (txtRicerca.Text.ToUpper());
			lstClienti.Adapter = new ClientiListAdapter(this, ls);

		}
	}

	public static class ObjectTypeHelper
	{
		public static T Cast<T>(this Java.Lang.Object obj) where T : class
		{
			var propertyInfo = obj.GetType().GetProperty("Instance");
			return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
		}
	}
}


