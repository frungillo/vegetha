
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
		EditText txtSaldo;
		ToggleButton btnScade ;
		TextView txtDataScadenza;
		string numtessera;
		bool insertMode = true;
		bool puoScadere = true;
		bool richiestaUscita = false;

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
			txtSaldo = FindViewById<EditText> (Resource.Id.txtSaldo);
			txtDataScadenza = FindViewById<TextView> (Resource.Id.txtScadenza);

			Button btnSalva = FindViewById<Button> (Resource.Id.btnSalva);
			Button btnAnnulla = FindViewById<Button> (Resource.Id.btnAnnulla);
			Button btnElimina = FindViewById<Button> (Resource.Id.btnElimina);
			btnScade = FindViewById<ToggleButton> (Resource.Id.btnPuoScadere);
			btnScade.Activated = puoScadere;

			btnScade.CheckedChange+= BtnScade_CheckedChange;

			btnElimina.Enabled = false;
			if (this.Intent.HasExtra("numtessera")) {
				insertMode = false;
				numtessera =  Intent.GetIntExtra ("numtessera",0).ToString(); 
				btnElimina.Enabled = true;
				riprendiCliente (numtessera);
			}
			btnElimina.Click += BtnElimina_Click;
			btnSalva.Click+= BtnSalva_Click;
			btnAnnulla.Click += (sender, e) => {
				funzioni.MsgBox(this,"Annullare l'inserimento (Tutti i dati non salvati andranno persi!) ?",
					"Vegetha",
					"SI",
					()=>{SetResult(Result.Ok); richiestaUscita = true; Finish(); },
					"NO",
					()=>{});

			};

			txtCognome.FocusChange+= txtFocusChange;
			txtNome.FocusChange += txtFocusChange;
			radioTipo.CheckedChange += (sender, e) => {
				valutaScadenza ();
			};
			radioTipo.Check( Resource.Id.radioAnnuale);
			valutaScadenza ();
		}

		void BtnScade_CheckedChange (object sender, CompoundButton.CheckedChangeEventArgs e)
		{
			valutaScadenza();
		}

		private void valutaScadenza() {
			if (btnScade.Checked) {
				if (radioTipo.CheckedRadioButtonId == Resource.Id.radioAnnuale) {
					txtDataScadenza.Text = "Scadenza: " + dataIscrizione.DateTime.AddMonths (12).ToShortDateString ();
				} else if (radioTipo.CheckedRadioButtonId == Resource.Id.radioTrimestrale) {
					txtDataScadenza.Text = "Scadenza: " + dataIscrizione.DateTime.AddMonths (3).ToShortDateString ();
				} else {
					txtDataScadenza.Text = "Scadenza: " + dataIscrizione.DateTime.AddMonths (1).ToShortDateString ();
				}
			} else {
				txtDataScadenza.Text = "Abbonamento perpetuo";
			}

		}

		void BtnElimina_Click (object sender, EventArgs e)
		{
			customers c = customers.GetByNumeroTessera (numtessera);
			funzioni.MsgBox (this, 
				"Elimino il cliente? (l'operazione non è reversibile...)",
				"Elimiazione",
				"Si",
				() => {
					try {
						c.Elimina();
						SetResult(Result.Ok);
						Finish();
					} catch (Exception ex) {
						funzioni.MsgBox (this, "Errore Eliminazione:" + ex.Message);
					}
				},
				"No",
				() => {
				}
			);

		}

		void txtFocusChange (object sender, View.FocusChangeEventArgs e)
		{
			var t = sender as EditText;
			t.Text = t.Text.ToUpper ();
		}

	
		void BtnSalva_Click (object sender, EventArgs e)
		{
			List<customers> ls = customers.GetByCognomeNome (txtCognome.Text, txtNome.Text);
			if (ls.Count > 0 && insertMode) {
				funzioni.MsgBox (this, "Attenzione esistono altri " + ls.Count.ToString () + " clienti che si chiamano " + txtCognome.Text + " " + txtNome.Text + " in archivio.\n Inserire comunque ?",
					"Vegetha",
					"SI",
					() => {
						inserimentoCliente ();
					},
					"NO",
					() => {
						CancellaTutto ();
						Toast.MakeText (this, "Operazione annullata come da richiesta...", ToastLength.Short).Show ();
						return;
					});
			} else {
				inserimentoCliente ();
			}


		}

		private void riprendiCliente(string numTessera){
			customers c = customers.GetByNumeroTessera (numTessera);
			txtCognome.Text = c.Cognome;
			txtNome.Text = c.Nome;
			txtNumeroTessera.Text = c.Numero_tessera.ToString();
			dataIscrizione.DateTime = c.DataIscrizione;
			btnScade.Checked  = c.puoScadere;
			txtSaldo.Text = c.Saldo.ToString ();
			if (c.Tipo == "A") {
				radioTipo.Check (Resource.Id.radioAnnuale);
			} else if (c.Tipo == "T") {
				radioTipo.Check (Resource.Id.radioTrimestrale);
			} else {
				radioTipo.Check (Resource.Id.RadioMensile);
			}
			valutaScadenza();
		}

		private void inserimentoCliente() {
			customers c = new customers ();
			c.Cognome = txtCognome.Text.Trim().Replace("'","''");
			c.Nome = txtNome.Text.Trim().Replace("'","''");
			c.DataIscrizione = dataIscrizione.DateTime;
			c.Numero_tessera = int.Parse(txtNumeroTessera.Text);
			c.puoScadere = btnScade.Checked;
			c.Saldo = Double.Parse (txtSaldo.Text);
			if (radioTipo.CheckedRadioButtonId == Resource.Id.radioAnnuale) {
				c.Tipo = "A";
				c.DataScadenza = c.DataIscrizione.AddMonths (12);
			} else if (radioTipo.CheckedRadioButtonId == Resource.Id.radioTrimestrale) {
				c.Tipo = "T";
				c.DataScadenza = c.DataIscrizione.AddMonths (3);
			} else {
				c.Tipo = "M";
				c.DataScadenza = c.DataIscrizione.AddMonths (1);
			}
			try {
				if(insertMode){
					c.Aggiungi();
				} 
				else 
				{
					c.Aggiorna();
				}
			} catch (Exception ex) {
				if (ex.Message.Contains ("costraint")) {
					funzioni.MsgBox (this, "Il numero tessera è stato già inserito");
					txtNumeroTessera.Text = "";
					txtNumeroTessera.RequestFocus ();
				} else {
					funzioni.MsgBox(this, "Errore: " + ex.Message);
				}
			}
			funzioni.MsgBox (this, "Cliente inserito correttamente.\nInserire ancora?","VegethaCM", "SI", ()=>{CancellaTutto();},
				"NO", ()=>{SetResult(Result.Ok);Finish();});
		}

		public void CancellaTutto() {
			txtCognome.Text = "";txtNome.Text = ""; 
			txtNumeroTessera.Text = ""; dataIscrizione.DateTime = DateTime.Now;
			txtSaldo.Text = "";
			btnScade.Activated = true;
			txtCognome.RequestFocus ();
		}

		public override void OnBackPressed ()
		{
			funzioni.MsgBox(this,"Annullare l'inserimento (Tutti i dati non salvati andranno persi!) ?",
				"Vegetha",
				"SI",
				()=>{SetResult(Result.Ok);richiestaUscita = true;Finish();},
				"NO",
				()=>{});
		}



		protected override void OnPause ()
		{
			if (richiestaUscita) {
				base.OnPause ();
			} 
		}
		protected override void OnResume ()
		{
			base.OnResume ();
		}

	}
}

