using System;
using System.Linq;
using System.Collections.Generic;
using VerwaltungLib;
namespace Client
{
	class MainClass
	{
		private static int menu = 0;
		private static int zeiger = 1;
		private static string[] neueBeitraege = new[]{
			string.Empty,
			string.Empty
		};
		private static string[] neuerNutzer = new[] {
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty
		};

		private static readonly IBank bank = new Bank ();
		private static readonly  IDatenbank db = new VereinDB ();
		private static readonly Vereinsverwaltung service = new Vereinsverwaltung (db, bank);
		
		public static void Main (string[] args)
		{
		start:
			Console.Clear ();
			if (menu == 0) {
				menu = MaleHaupmenu ();
				if (menu != 0)
					zeiger = 1;
			} else if (menu == 1) {
				menu = MitgliedAnlegen ();
			} else if (menu == 2){
				menu = BeitraegeEinzihen ();
			} else if (menu == 3){

			} else if (menu == 3){

			} else if (menu == -1) {
				goto fluechten;
			}
		goto start;
		fluechten:
			service.MitgliedAnlegen("Vorname", "Nachname", DateTime.Now.Subtract(TimeSpan.FromDays(5)), Berufsstand.Student, "IBAAAANNNNN", "BIIIIC", "Vorname Nachname");
			service.Einziehen (5m, 2.5m);
			service.Einziehen (5m, 2.5m);
			var aktivitaeten = service.KontoaktivitaetenLaden (DateTime.MinValue, DateTime.MaxValue);
			foreach (var atk in aktivitaeten) {
				Console.Out.Write(String.Format("Iban: {1}{0}Datum: {2}{0}Betrag: {3}{0}VerWendungszweck: {4}{0}", Environment.NewLine, atk.IBAN, atk.Datum, atk.Betrag, atk.Verwendungszweck));
			}
		}

		static int BeitraegeEinzihen ()
		{
			Console.WriteLine ("Mitglied anlegen");
			Console.WriteLine ("[" + (zeiger == 1 ? "*" : " ") + "] Betrag: " + neueBeitraege[0]);
			Console.WriteLine ("[" + (zeiger == 2 ? "*" : " ") + "] Ermaeßigter Betrag: " + neueBeitraege[1]);
			var gedrueckteTaste = Console.ReadKey ();
			if (GeheZuVorherigemMenupunkt(gedrueckteTaste) && zeiger > 1) {
				zeiger--;
			} else if (GeheZuNaechstemMenupunkt (gedrueckteTaste.Key) && zeiger < 2) {
				zeiger++;
			} else if (gedrueckteTaste.Key == ConsoleKey.Escape) {
				zeiger = 1;
				return 0;
			} else if (gedrueckteTaste.Key == ConsoleKey.Enter) {
				service.Einziehen (decimal.Parse (neueBeitraege [0]), decimal.Parse (neueBeitraege [1]));
			} else if (gedrueckteTaste.Key == ConsoleKey.Backspace) {
				var laenge = neueBeitraege [zeiger - 1].Length;
				if (laenge > 0) {
					neueBeitraege [zeiger - 1] = neueBeitraege [zeiger - 1].Remove (laenge - 1);
				}
			} else {
				neueBeitraege [zeiger - 1] += gedrueckteTaste.KeyChar;
			}
			return menu;
		}

		private static int MitgliedAnlegen() {
			Console.WriteLine ("Mitglied anlegen");
			Console.WriteLine ("[" + (zeiger == 1 ? "*" : " ") + "] Vorname: " + neuerNutzer[0]);
			Console.WriteLine ("[" + (zeiger == 2 ? "*" : " ") + "] Nachname: " + neuerNutzer[1]);
			Console.WriteLine ("[" + (zeiger == 3 ? "*" : " ") + "] Geburtsdatum: " + neuerNutzer[2]);
			Console.WriteLine ("[" + (zeiger == 4 ? "*" : " ") + "] Erwerbstätig: " + neuerNutzer[3]);
			Console.WriteLine ("[" + (zeiger == 5 ? "*" : " ") + "] IBAN: " + neuerNutzer[4]);
			Console.WriteLine ("[" + (zeiger == 6 ? "*" : " ") + "] BIC: " + neuerNutzer[5]);
			Console.WriteLine ("[" + (zeiger == 7 ? "*" : " ") + "] Kontoinhaber: " + neuerNutzer[6]);
			var gedrueckteTaste = Console.ReadKey ();
			if (GeheZuVorherigemMenupunkt(gedrueckteTaste) &&zeiger > 1) {
				zeiger--;
			} else if (GeheZuNaechstemMenupunkt(gedrueckteTaste.Key) && zeiger < 7) {
				zeiger++;
			} else if (gedrueckteTaste.Key == ConsoleKey.Escape) {
				zeiger = 1;
				return 0;
			} else if (gedrueckteTaste.Key == ConsoleKey.Enter) {
				service.MitgliedAnlegen (
					neuerNutzer [0], 
					neuerNutzer [1], 
					DateTime.Parse (neuerNutzer [2]), 
					HoleBerufsstand (neuerNutzer [3]),
					neuerNutzer [4],
					neuerNutzer [5],
					neuerNutzer [6]);
				zeiger = 1;
				return 0;
			} else if (gedrueckteTaste.Key == ConsoleKey.Backspace) {
				var laenge = neuerNutzer [zeiger - 1].Length;
				if (laenge > 0) {
					neuerNutzer [zeiger - 1] = neuerNutzer [zeiger - 1].Remove (laenge - 1);
				}
			} else {
				neuerNutzer [zeiger - 1] += gedrueckteTaste.KeyChar;
			}
			return menu;
		}

		private static Berufsstand HoleBerufsstand(string eingabe) {
			switch (eingabe.ToLower()) {
			case "ja":
			case "j":
			case "y":
			case "ye":
			case "yes":
			case "1":
			case "t":
			case "tr":
			case "tru":
			case "true":
				return Berufsstand.Erwerbstaetig;
			default:
				return Berufsstand.Student;
			}
		}

		private static int MaleHaupmenu() {
			Console.WriteLine ("Menü:");
			Console.WriteLine ("[" + (zeiger == 1 ? "*" : " ") + "] Mitglied anlegen" + (zeiger == 1 ? "\t   <-" : string.Empty));
			Console.WriteLine ("[" + (zeiger == 2 ? "*" : " ") + "] Beiträge einziehen" + (zeiger == 2 ? "\t   <-" : string.Empty));
			Console.WriteLine ("[" + (zeiger == 3 ? "*" : " ") + "] Kontoaktivitäten laden" + (zeiger == 3 ? " <-" : string.Empty));
			Console.WriteLine ("[" + (zeiger == 4 ? "*" : " ") + "] Jahresbilanz anzeigen" + (zeiger == 4 ? "  <-" : string.Empty));
			var gedrueckteTaste = Console.ReadKey ();
			if (GeheZuVorherigemMenupunkt(gedrueckteTaste) &&zeiger > 1) {
				zeiger--;
			} else if (GeheZuNaechstemMenupunkt(gedrueckteTaste.Key) && zeiger < 4) {
				zeiger++;
			}  else if (gedrueckteTaste.Key == ConsoleKey.Escape) {
				return -1;
			} else if (gedrueckteTaste.Key == ConsoleKey.Enter) {
				return zeiger;
			}
			return menu;
		}

		static bool GeheZuVorherigemMenupunkt (ConsoleKeyInfo gedrueckteTaste)
		{
			return (gedrueckteTaste.Key == ConsoleKey.UpArrow || gedrueckteTaste.Modifiers == ConsoleModifiers.Shift && gedrueckteTaste.Key == ConsoleKey.Tab);
		}

		private static bool GeheZuNaechstemMenupunkt(ConsoleKey taste) {
			return taste == ConsoleKey.DownArrow || taste == ConsoleKey.Tab; 
		}
	}
	class Bank : VerwaltungLib.IBank
	{
		private List<Kontoaktivitaeten> ka;
		public Bank ()
		{
			this.ka = new List<Kontoaktivitaeten> ();
		}
		#region IBank implementation

		public IEnumerable<Kontoaktivitaeten> KontoaktivitaetenLaden (DateTime von, DateTime bis)
		{
			return this.ka.Where (_ => _.Datum >= von && _.Datum <= bis);
		}

		public void SepaEinzug (string iban, string bic, string kontoinhaber, decimal betrag, string verwendungszweck)
		{
			this.ka.Add (new Kontoaktivitaeten { Betrag = betrag, Datum = DateTime.Now.AddDays(1), IBAN = iban, Verwendungszweck = verwendungszweck });
		}

		#endregion


	}

	class VereinDB : VerwaltungLib.IDatenbank
	{
		private readonly List<VerwaltungLib.Mitglied> mitglieder;
		private readonly List<Kontoverbindung> _verbindungen;
		public VereinDB ()
		{
			this.mitglieder = new List<Mitglied> ();
			this._verbindungen = new List<Kontoverbindung> ();
		}
		#region IDatenbank implementation

		public Kontoverbindung FindKonto (Guid kontoverbindungsId)
		{
			return _verbindungen.Single (_ => _.Id == kontoverbindungsId);
		}

		public System.Collections.Generic.IEnumerable<Mitglied> FindeMitglieder ()
		{
			return this.mitglieder.AsEnumerable();
		}

		public void SpeicherMitglied (Mitglied m)
		{ 
			var DbMitglied = mitglieder.SingleOrDefault (_ => _.Id == m.Id);


			if (DbMitglied != null) {
				mitglieder.Remove (DbMitglied);
				mitglieder.Add (m);
			} else {
				mitglieder.Add (m);
			}
		}

		public Mitglied FindeMitglied (Guid id)
		{
			return mitglieder.Single (_ => _.Id == id);
		}

		public Guid KontoSpeichern (Kontoverbindung m)
		{
			var DbKonto = _verbindungen.SingleOrDefault (_ => _.Id == m.Id);

			if (DbKonto != null) {
				_verbindungen.Remove (DbKonto);
				_verbindungen.Add (m);
			} else {
				_verbindungen.Add (m);
			}
			return m.Id;
		}

		#endregion


	}
}
