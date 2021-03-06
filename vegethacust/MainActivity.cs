﻿using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace vegethacust
{
	[Activity (Label = "Vegetha CM", MainLauncher = true, Icon = "@drawable/vegetha_512")]
	public class MainActivity : Activity
	{
		EditText txtRicerca;
		ListView lstClienti;

		private void RestoreData(){
			string esito = "";
			ProgressDialog d = new ProgressDialog (this);
			d.SetCancelable (false);
			d.SetMessage ("Attendere, ripristino in corso...");
			d.SetTitle ("Ripristino...");


			Thread t = new Thread (() => {
				esito = customers.restoreFromFTP();
				d.Dismiss();
				RunOnUiThread(()=>{ funzioni.MsgBox(this,esito);});
			});
			d.Show ();
			t.Start ();

	
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetTheme (Android.Resource.Style.ThemeHoloLightNoActionBar);
			SetContentView (Resource.Layout.Main);
			customers.CreaMaster();

			//ImageView im1 = FindViewById<ImageView> (Resource.Id.imageView2);
			Button button = FindViewById<Button> (Resource.Id.myButton);
			Button btnLeggi = FindViewById<Button> (Resource.Id.btnLeggi);
			lstClienti = FindViewById<ListView> (Resource.Id.listView1);
			txtRicerca = FindViewById<EditText> (Resource.Id.txtRicerca);
			Button btnBackup = FindViewById<Button> (Resource.Id.btnBakupDati);
			Button btnRipristina = FindViewById<Button> (Resource.Id.btnRipristina);

			btnBackup.Click+= (object sender, EventArgs e) => {
				string res = customers.backupOverFTP();
				funzioni.MsgBox(this,res);
			};

			btnRipristina.Click+= (sender, e) => {
				FileInfo f = new FileInfo(Path.Combine( System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), "cust.db"));
				if (f.Exists && f.Length > 16000) {
					funzioni.MsgBox(this,"Attenzione l'operazionie di ripristino sostituira tutti i dati presenti con quelli memorizzati nell'ultimo backup, procedo?",
						"Ripristino Dati", "SI", 
						()=>{ RestoreData();},
						"NO");
					
				} else {
					RestoreData();
				}
			};


			button.Click += delegate {
				Intent frmAggiungi = new Intent(this, typeof(frmAggiungiUtente));
				StartActivity(frmAggiungi);

			};

			btnLeggi.Click+= delegate {
				
				txtRicerca.Text = "scad:" + DateTime.Now.ToShortDateString();
				lstClienti.Adapter = new ClientiListAdapter (this, customers.GetByValues (txtRicerca.Text));

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
			StartActivity (frmAggiungi);
		}
		/*
		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if (resultCode == Result.Ok) {
				List<customers> ls = customers.GetTutti();
				lstClienti.Adapter = new ClientiListAdapter(this, ls);
			}
		}
		*/

		protected override void OnResume ()
		{
			List<customers> ls = customers.GetTutti();
			lstClienti.Adapter = new ClientiListAdapter(this, ls);
			base.OnResume ();
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


