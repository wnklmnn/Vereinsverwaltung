using System;

namespace Entitäten
{
    internal class Mitglied
    {
        public Mitglied()
        {
        }
        public string Name {
            get;
            private set;
        }
        public string Nachname {
            get;
            private set;
        }
        public DateTime Geburtsdatum {
            get;
            private set;
        }
        public object Erwerbstaetigkeit {
            get;
            private set;
        }
    public Konto Kontoverbindung {
      get;
      private set;
    }
    }
}

