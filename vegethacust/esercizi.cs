using System;
using SQLite;
using System.IO;
using System.Collections.Generic;
using System.Net;

namespace vegethacust
{
	[Table("esercizi")]
	public class esercizi 
	{
		string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");

		[PrimaryKey,  Column("_id_esercizio")]
		public int IdEsercizio { get; set;}

		[MaxLength(50),NotNull]
		public string Descrizione { get; set;}


		public esercizi ()
		{
		}

		public static void Trunk() {
			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");

			var db = new SQLiteConnection (dbPath);
			db.DeleteAll<esercizi> ();
		}

		public static void Create() {
			Console.WriteLine ("Creo Database...");
			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");
			var db = new SQLiteConnection (dbPath);
			db.CreateTable<esercizi> ();

		}

		public void Elimina()
		{
			var db = new SQLiteConnection (dbPath);
			db.Delete (this);
		}

		public void Aggiorna ()
		{

			var db = new SQLiteConnection (dbPath);

			db.Update (this);
		}

		public void Aggiungi ()
		{
			var db = new SQLiteConnection (dbPath);

			db.Insert (this);
		}

		public static void AggiungiTutti(List<customers> c)
		{

			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");
			var db = new SQLiteConnection (dbPath);
			db.InsertAll (c);
		}

		public static List<customers> GetTutti() {
			List<customers> ls = new List<customers>();
			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");
			var db = new SQLiteConnection (dbPath);
			ls = db.Query<customers> ("select * from customers order by 2,3,5", new String[] {});
			/*
			var table = db.Table<customers> ();
			foreach (customers c in table) {
				ls.Add(c);
			}
			*/
			return ls;
		}

		public static List<customers> GetByCognomeNome(string Cognome, string Nome) {
			List<customers> ls = new List<customers> ();
			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");
			var db = new SQLiteConnection (dbPath);
			ls = db.Query<customers> ("select * from customers where Cognome = '"+Cognome+"' and Nome = '"+Nome+"'", new String[] {Cognome,Nome});
			return ls;
		}

		public static List<customers> GetByValues(string cerca) {
			List<customers> ls = new List<customers> ();
			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");
			var db = new SQLiteConnection (dbPath);
			if (cerca.ToLower().StartsWith("scad:")) {
				string dt = cerca.ToLower ().Replace ("scad:", "").Trim();
				try {
					DateTime data = DateTime.Parse (dt);

					List<customers> templs = db.Query<customers> ("select * from customers  order by 2,3,5", new String[] {});
					foreach (customers	item in templs) {
						if (item.DataScadenza <= data) {
							ls.Add (item);
						}
					}	
				} catch (Exception) {

				}


			} else {
				ls = db.Query<customers> ("select * from customers where Cognome like '%"+cerca+"%' or Nome like '%"+cerca+"%' or _numero_tessera ='"+cerca+"' order by 2,3,5", new String[] {});
			}

			return ls;
		}

		public static customers GetByNumeroTessera(string numerotessera){

			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");
			var db = new SQLiteConnection (dbPath);
			List<customers> ls = db.Query<customers> ("select * from customers where  _numero_tessera ='"+numerotessera+"'", new String[] {});
			return ls[0];
		}



	}
}