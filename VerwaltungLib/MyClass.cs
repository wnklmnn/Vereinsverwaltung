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

		public Jahresbilanz JahresbilanzErstellen(int jahr) {
			var kontoaktivitaeten = _bank.KontoaktivitaetenLaden (DateTime.MinValue, new DateTime (jahr, 12, 31));

			decimal vorjahr = 0, verlust = 0, gewinn = 0;

			foreach (var kontoaktivitaet in kontoaktivitaeten) {
				if (kontoaktivitaet.Datum.Year == jahr) {
					if (kontoaktivitaet.Betrag < 0) {
						verlust += kontoaktivitaet.Betrag;
					} else {
						gewinn += kontoaktivitaet.Betrag;
					}
				} else {
					vorjahr += kontoaktivitaet.Betrag;
				}
			}

			return new Jahresbilanz {
				Vorjahr = vorjahr,
				Gewinn = gewinn,
				Verlust = verlust,
				Kontostand = vorjahr + verlust + gewinn
			};
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
					mitglied.Bezahltesjahr = DateTime.Now.Year;
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
				Id = Guid.NewGuid(),
				Vorname = vorname, 
				Nachname = nachname, 
				Geburtstag = geburtstag, 
				Berufsstand = bs, 
				KontoverbindungsId = _db.KontoSpeichern(konto),
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
		public string IBAN {
			get;
			set;
		}
		public string Verwendungszweck {
			get;
			set;
		}
		public decimal Betrag { get; set; }
		public DateTime Datum { get; set; }
	}

	public interface IDatenbank{
		Kontoverbindung FindKonto (Guid kontoverbindungsId);
		IEnumerable<Mitglied> FindeMitglieder();
		void SpeicherMitglied (Mitglied m);
		Mitglied FindeMitglied (Guid id);
		Guid KontoSpeichern(Kontoverbindung konto);
	}

	public enum Berufsstand {
		Student,
		Erwerbstaetig
	}

	public class Kontoverbindung {
		public Guid Id{ get; set;}
		public string Kontoinhaber{ get; set;}
		public string IBAN{ get; set;}
		public  string BIC { get; set;}
	}

	public class Mitglied {
		public Guid Id{ get; set;}
		public string Vorname{ get; set;}
		public string Nachname{ get; set;}
		public DateTime Geburtstag{ get; set; }
		public Berufsstand Berufsstand{ get; set;}
		public Guid KontoverbindungsId{ get; set;}
		public int? Bezahltesjahr{ get; set;}
	}

	public class Jahresbilanz {
		public decimal Vorjahr { get; internal set; }
		public decimal Verlust { get; internal set; }
		public decimal Gewinn { get; internal set; }
		public decimal Kontostand { get; internal set; }
	}
}

