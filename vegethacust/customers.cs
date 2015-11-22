using System;
using SQLite;
using System.IO;
using System.Collections.Generic;
using System.Net;

namespace vegethacust
{
	[Table("customers")]
	public class customers 
	{
		string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");

		[PrimaryKey,  Column("_numero_tessera")]
		public int Numero_tessera { get; set;}

		[MaxLength(50),NotNull]
		public string Cognome { get; set;}

		[MaxLength(50), NotNull]
		public string Nome { get; set;}

		[MaxLength(1),NotNull]
		public string Tipo { get; set;}

		[NotNull]
		public DateTime DataIscrizione { get; set;}

		public bool puoScadere { get; set; }

		public DateTime DataScadenza { get; set; }

		public Double Saldo { get; set; }

		[MaxLength(500)]
		public string Note { get; set; }

		public customers ()
		{
		}

		public static void TrunkDB() {
			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");

			var db = new SQLiteConnection (dbPath);
			db.DeleteAll<customers> ();
		}

		public static void CreaMaster() {
			Console.WriteLine ("Creo Database...");
			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");
			var db = new SQLiteConnection (dbPath);
			db.CreateTable<customers> ();
		
		}

		public void Elimina()
		{
			var db = new SQLiteConnection (dbPath);
			db.Delete (this);
		}

		public void Aggiorna ()
		{
			
			var db = new SQLiteConnection (dbPath);
			if (this.Tipo == "A" && this.puoScadere ) {
				this.DataScadenza = this.DataIscrizione.AddMonths (12);
			} else {
				this.DataScadenza = this.DataIscrizione.AddMonths (3);
			}
			db.Update (this);
		}

		public void Aggiungi ()
		{
			var db = new SQLiteConnection (dbPath);
			if (this.Tipo == "A" && this.puoScadere ) {
				this.DataScadenza = this.DataIscrizione.AddMonths (12);
			} else {
				this.DataScadenza = this.DataIscrizione.AddMonths (3);
			}
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
				} catch (Exception ex) {
					
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

		public static void backupDB() {
			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db");
			FileInfo f = new FileInfo (dbPath);
			f.CopyTo(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path , "cust.db"));
				
		}

		public static string backupOverFTP()
		{
			try {
				string ftpHost = "test.frungillo.org";
				string ftpUser = "test_frungillo";
				string ftpPassword = "genny.1975";
				string ftpfullpath = "ftp://test.frungillo.org/ser/vegetha/cust.db";
				FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);

				//userid and password for the ftp server  

				ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);

				ftp.KeepAlive = true;
				ftp.UseBinary = true;
				ftp.Method = WebRequestMethods.Ftp.UploadFile;

				FileStream fs = File.OpenRead(Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "cust.db"));

				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, buffer.Length);

				fs.Close();

				Stream ftpstream = ftp.GetRequestStream();
				ftpstream.Write(buffer, 0, buffer.Length);
				ftpstream.Close();
				ftpstream.Flush();	
			} catch (Exception ex) {
				return "Errore Backup:" + ex.Message;

			}
			return "Backup Completato";



		}
	}
}

