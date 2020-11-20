using MySql.Data.MySqlClient;
using System;
using MetierTrader;
using System.Collections.Generic;

namespace GestionnaireBDD
{
    public class GstBdd
    {
        private MySqlConnection cnx;
        private MySqlCommand cmd;
        private MySqlDataReader dr;

        // Constructeur
        public GstBdd()
        {
            // Attention à changer le Port !!
            string chaine = "Server=localhost;Port=3308;Database=bourse;Uid=root;Pwd=";
            cnx = new MySqlConnection(chaine);
            cnx.Open();
        }

        public List<Trader> getAllTraders()
        {
            List<Trader> mesTraders = new List<Trader>();
            cmd = new MySqlCommand("SELECT `idTrader`, `nomTrader` FROM `trader`;", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Trader unTrader = new Trader(Convert.ToInt16(dr[0].ToString()), dr[1].ToString());
                mesTraders.Add(unTrader);
            }
            dr.Close();
            return mesTraders;
        }

        public List<ActionPerso> getAllActionsByTrader(int numTrader)
        {
            List<ActionPerso> mesActionsPerso = new List<ActionPerso>();
            cmd = new MySqlCommand("SELECT `numAction`, `numTrader`, nomAction, `prixAchat`, `quantite` FROM `acheter` INNER JOIN trader ON acheter.numTrader = trader.idTrader INNER JOIN action on acheter.numAction = action.idAction WHERE idtrader = " + numTrader, cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ActionPerso monActionPerso = new ActionPerso(Convert.ToInt16(dr[0].ToString()), Convert.ToInt16(dr[1].ToString()), dr[2].ToString(), Convert.ToDouble(dr[3].ToString()), Convert.ToInt16(dr[4].ToString()));
                mesActionsPerso.Add(monActionPerso);
            }
            dr.Close();
            return mesActionsPerso;
        }

        public List<MetierTrader.Action> getAllActionsNonPossedees(int numTrader)
        {
            List<MetierTrader.Action> pasMesActions = new List<MetierTrader.Action>();
            cmd = new MySqlCommand("SELECT `idAction`, `nomAction`, `coursReel` FROM `action` WHERE idAction NOT IN (SELECT acheter.numAction FROM acheter WHERE acheter.numTrader = " + numTrader + ")", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                MetierTrader.Action pasMonAction = new MetierTrader.Action(Convert.ToInt16(dr[0].ToString()), dr[1].ToString());
                pasMesActions.Add(pasMonAction);
            }
            dr.Close();
            return pasMesActions;
        }

        public void SupprimerActionAcheter(int numAction, int numTrader)
        {
            cmd = new MySqlCommand("DELETE FROM `acheter` WHERE `numAction` = " + numAction + " AND `numTrader` = " + numTrader, cnx);
            cmd.ExecuteNonQuery();
        }

        public void UpdateQuantite(int numAction, int numTrader, int quantite)
        {
            cmd = new MySqlCommand("UPDATE `acheter` SET `quantite`= `quantite` - " + quantite + " WHERE `numAction`= " + numAction + " AND `numTrader`= " + numTrader, cnx);
            cmd.ExecuteNonQuery();
        }

        public double getCoursReel(int numAction)
        {
            double cours;
            cmd = new MySqlCommand("select coursReel from action where idaction = " + numAction, cnx);
            dr = cmd.ExecuteReader();
            dr.Read();
            cours = Convert.ToDouble(dr[0].ToString());
            dr.Close();
            return cours;
        }

        public void AcheterAction(int numAction, int numTrader, double prix, int quantite)
        {
            cmd = new MySqlCommand("INSERT INTO `acheter`(`numAction`, `numTrader`, `prixAchat`, `quantite`) VALUES( " + numAction + "," + numTrader + "," + prix + "," + quantite + ")", cnx);
            cmd.ExecuteNonQuery();
        }

        public double getTotalPortefeuille(int numTrader)
        {
            double total;
            cmd = new MySqlCommand("select sum(prixAchat * quantite) from acheter where numTrader = " + numTrader, cnx);
            dr = cmd.ExecuteReader();
            dr.Read();
            total = Convert.ToDouble(dr[0].ToString());
            dr.Close();
            return total;
        }
    }
}
