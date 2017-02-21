using System;
using System.Collections.Generic;

namespace VerwaltungLib
{
	public class Vereinsverwaltung
	{
		private const string _iban = "HAHAHAHAHHAHAHHAHAHAHHA";

		private readonly IDatenbank _db;
		private readonly IBank _bank;
		public Vereinsverwaltung (IDatenbank db, IBank bank)
		{
			_db = db;
			_bank = bank;
		}

		public void JahresbilanzErstellen(int jahr){
			_bank.KontoaktivitaetenLaden (new DateTime (jahr, 1, 1), new DateTime (jahr, 12, 31));
		}

		public void Einziehen(decimal beitrag, decimal ermaessigterBeitrag) {
			foreach(Mitglied mitglied in _db.FindeMitglieder()) {
				if(DateTime.Now.Year != mitglied.Bezahltesjahr) {
					Kontoverbindung konto = _db.FindKonto (mitglied.KontoverbindungsId);
					_bank.SepaEinzug (
						konto.IBAN, 
						konto.BIC, 
						konto.Kontoinhaber, 
						mitglied.Berufsstand == Berufsstand.Erwerbstaetig ? 
							beitrag : 
							ermaessigterBeitrag,
						"GELD FÜR " + DateTime.Now.Year);
				}
			}
		}

		public IEnumerable<Kontoaktivitaeten> KontoaktivitaetenLaden(DateTime von, DateTime bis) {
			return _bank.KontoaktivitaetenLaden (von, bis);
		}

		public void MitgliedAnlegen(string vorname, string nachname, DateTime geburtstag, Berufsstand bs, string iban, string bic, string kontoinhaber)
		{
			var konto = new Kontoverbindung {
				IBAN = iban,
				BIC = bic,
				Kontoinhaber = kontoinhaber,
				Id = Guid.NewGuid()
			};

			_db.SpeicherMitglied (
				new Mitglied {
				Vorname = vorname, 
				Nachname = nachname, 
				Geburtstag = geburtstag, 
				Berufsstand = bs, 
				KontoverbindungsId = _db.FindeOderErstelleKontoverbindung (konto),
				Bezahltesjahr = null
			});
		}
	}

	public interface IBank
	{
		IEnumerable<Kontoaktivitaeten> KontoaktivitaetenLaden(DateTime von, DateTime bis);
		void SepaEinzug(string iban, string bic, string kontoinhaber, decimal betrag, string verwendungszweck);
	}

	public struct Kontoaktivitaeten {
		public decimal Betrag { get; set; }
		public DateTime Datum { get; set; }
	}

	public interface IDatenbank{
		Kontoverbindung FindKonto (Guid kontoverbindungsId);
		IEnumerable<Mitglied> FindeMitglieder();
		void SpeicherMitglied (Mitglied m);
		Mitglied FindMitglied (Guid id);
		Guid FindeOderErstelleKontoverbindung(Kontoverbindung konto);
	}

	public enum Berufsstand{
		Student,
		Erwerbstaetig
	}

	public struct Kontoverbindung{
		public Guid Id{ get; set;}
		public string Kontoinhaber{ get; set;}
		public string IBAN{ get; set;}
		public  string BIC { get; set;}
	}

	public struct Mitglied{
		public Guid Id{ get; set;}
		public string Vorname{ get; set;}
		public string Nachname{ get; set;}
		public DateTime Geburtstag{ get; set; }
		public Berufsstand Berufsstand{ get; set;}
		public Guid KontoverbindungsId{ get; set;}
		public int? Bezahltesjahr{ get; set;}
	}
}

