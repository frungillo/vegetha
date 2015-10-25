using System;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using Android.Views;

namespace vegethacust
{
	public class ClientiListAdapter : BaseAdapter<customers>
	{
		customers[] clienti;
		Activity context;

		public ClientiListAdapter (Activity context, List<customers> clienti)
		{
			this.clienti = clienti.ToArray ();
			this.context = context;
		}


		#region implemented abstract members of BaseAdapter
		public override long GetItemId (int position)
		{
			return position;
		}
		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
			if (view == null)
				view = context.LayoutInflater.Inflate (Android.Resource.Layout.TwoLineListItem, null);
			if (clienti.Length > 0) {
				view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = clienti [position].Cognome + " " + clienti [position].Nome;
				string Tipo = "";
				if (clienti [position].Tipo == "A") {
					Tipo = "Ann.";

				} else {
					Tipo = "Tri.";
				}
				view.FindViewById<TextView> (Android.Resource.Id.Text2).Text = "Scheda [" + clienti [position].Numero_tessera + "] ("
				+ Tipo + ") Iscr." + clienti [position].DataIscrizione.ToShortDateString ();
			} else {
				view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = "Nessun cliente trovato...";
				view.FindViewById<TextView> (Android.Resource.Id.Text1).TextAlignment = TextAlignment.Center;
			
			}
			return view;
		}
		public override int Count {
			get {
				return clienti.Length;
			}
		}
		#endregion
		#region implemented abstract members of BaseAdapter
		public override customers this [int index] {
			get {
				return clienti [index];
			}
		}
		#endregion
	}
}

