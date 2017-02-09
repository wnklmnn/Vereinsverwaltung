//
//  Mitglied.cs
//
//  Author:
//       Pascal Winkelmann <pascal@wnklmnn.de>
//
//  Copyright (c) 2017 
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
    }
}

