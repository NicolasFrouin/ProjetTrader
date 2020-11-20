using System;
using System.Collections.Generic;
using System.Text;

namespace MetierTrader
{
    public class ActionPerso
    {
        public int NumAction { get; set; }
        public int NumTrader { get; set; } // Pas obligé
        public string NomAction { get; set; }
        public double PrixAchat { get; set; }
        public int Quantite { get; set; }
        public double Total { get; set; }

        public ActionPerso(int unNumAction, int unNumTrader, string unNomAction, double unPrixAchat, int uneQuantite)
        {
            NumAction = unNumAction;
            NumTrader = unNumTrader;
            NomAction = unNomAction;
            PrixAchat = unPrixAchat;
            Quantite = uneQuantite;
            Total = PrixAchat * Quantite;
        }
    }
}
