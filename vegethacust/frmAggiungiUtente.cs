
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
using Android.Views.InputMethods;

namespace vegethacust
{
	[Activity (Label = "frmAggiungiUtente", NoHistory=true)]			
	public class frmAggiungiUtente : Activity
	{
		EditText txtCognome;
		EditText txtNome;
		EditText txtNumeroTessera;
		RadioGroup radioTipo;
		DatePicker dataIscrizione;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetTheme (Android.Resource.Style.ThemeHoloLightNoActionBar);
			SetContentView (Resource.Layout.frmAggiungiUtente);

			txtCognome = FindViewById<EditText> (Resource.Id.txtCognome);
			txtNome = FindViewById<EditText> (Resource.Id.txtNome);
			txtNumeroTessera = FindViewById<EditText> (Resource.Id.txtNumeroTessera);
			radioTipo = FindViewById<RadioGroup> (Resource.Id.gruppoTipi);
			dataIscrizione = FindViewById<DatePicker> (Resource.Id.datePicker1);
			Button btnSalva = FindViewById<Button> (Resource.Id.btnSalva);
			Button btnAnnulla = FindViewById<Button> (Resource.Id.btnAnnulla);
			btnSalva.Click+= BtnSalva_Click;
			btnAnnulla.Click += (sender, e) => {
				funzioni.MsgBox(this,"Annullare l'inserimento (Tutti i dati non salvati andranno persi!) ?",
					"Vegetha",
					"SI",
					()=>{Finish();},
					"NO",
					()=>{});

			};



		}

	
		void BtnSalva_Click (object sender, EventArgs e)
		{
			List<customers> ls = customers.GetByCognomeNome (txtCognome.Text, txtNome.Text);
			if (ls.Count > 0) {
				funzioni.MsgBox(this,"Attenzione esistono altri "+ ls.Count.ToString() +" clienti che si chiamano " +txtCognome.Text+" "+txtNome.Text+" in archivio.\n Inserire comunque ?",
					"Vegetha",
					"SI",
					()=>{},
					"NO",
					()=>{
						CancellaTutto();
						Toast.MakeText(this,"Operazione annullata come da richiesta...", ToastLength.Short).Show();
						return;
					});
			}

			customers c = new customers ();
			c.Cognome = txtCognome.Text;
			c.Nome = txtNome.Text;
			c.DataIscrizione = dataIscrizione.DateTime;
			c.Numero_tessera = int.Parse(txtNumeroTessera.Text);
			if (radioTipo.CheckedRadioButtonId == Resource.Id.radioAnnuale) {
				c.Tipo = "A";
			} else {
				c.Tipo = "T";
			}
			try {
				c.Aggiungi();
			} catch (Exception ex) {
				if (ex.Message.Contains ("costraint")) {
					funzioni.MsgBox (this, "Il numero tessera è stato già inserito");
					txtNumeroTessera.Text = "";
					txtNumeroTessera.RequestFocus ();
				}
			}
			funzioni.MsgBox (this, "Cliente inserito correttamente.\nInserire ancora?","VegethaCM", "SI", ()=>{CancellaTutto();},
				"NO", ()=>{Finish();});
		}


		public void CancellaTutto() {
			txtCognome.Text = "";txtNome.Text = ""; txtNumeroTessera.Text = ""; dataIscrizione.DateTime = DateTime.Now;
			txtCognome.RequestFocus ();
		}

		public override void OnBackPressed ()
		{
			funzioni.MsgBox(this,"Annullare l'inserimento (Tutti i dati non salvati andranno persi!) ?",
				"Vegetha",
				"SI",
				()=>{Finish();},
				"NO",
				()=>{});
		}
	}
}

