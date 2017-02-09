using System;

namespace Entitäten
{
  public class Konto
  {
    public Konto()
    {
    }
    public string IBAN {
      get;
      private set;
    }
    public string BIC {
      get;
      private set;
    }
  }
}