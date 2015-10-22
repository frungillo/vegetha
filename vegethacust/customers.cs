using System;
using SQLite;
using System.IO;
using System.Collections.Generic;

namespace vegethacust
{
	[Table("customers")]
	public class customers 
	{
		string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");

		[PrimaryKey,  Column("_numero_tessera")]
		public int Numero_tessera { get; set;}

		[MaxLength(50)]
		public string Cognome { get; set;}

		[MaxLength(50)]
		public string Nome { get; set;}

		[MaxLength(1)]
		public string Tipo { get; set;}

		public DateTime DataIscrizione { get; set;}

		public customers ()
		{
		}

		public static void CreaMaster() {
			Console.WriteLine ("Creo Database...");
			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");
			var db = new SQLiteConnection (dbPath);
			db.CreateTable<customers> ();
		
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
			var table = db.Table<customers> ();
			foreach (customers c in table) {
				ls.Add(c);
			}
			return ls;
		}

	}
}

