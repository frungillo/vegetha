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

namespace vegethacust
{
	public class funzioni
	{
		
		public funzioni ()
		{
			
		}

		public static void MsgBox(Context context,
			string msg, 
			string Titolo = "Vegetha", 
			string PositiveText = "OK", 
			Action PositiveAction = null, 
			string NegativeText ="",
			Action NegativeAction = null) 
		{
			AlertDialog.Builder d = new AlertDialog.Builder (context);
			d.SetTitle(Titolo);
			d.SetMessage (msg);
			if (PositiveAction != null) {
				d.SetPositiveButton (PositiveText, (sender, e) => {
					PositiveAction.Invoke();
				});	
			} else {
				d.SetPositiveButton (PositiveText, (sender, e) => {
					
				});
			}

			if (NegativeAction != null) {
				d.SetNegativeButton (NegativeText, (sender, e) => {
					NegativeAction.Invoke ();
				});
			}
			d.Show ();
		}
	}
}

