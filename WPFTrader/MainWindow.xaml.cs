using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GestionnaireBDD;
using MetierTrader;

namespace WPFTrader
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GstBdd gst;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gst = new GstBdd();
            lstTraders.ItemsSource = gst.getAllTraders();
        }

        private void lstTraders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstActions.ItemsSource = gst.getAllActionsByTrader((lstTraders.SelectedItem as Trader).NumTrader);
            lstActionsNonPossedees.ItemsSource = gst.getAllActionsNonPossedees((lstTraders.SelectedItem as Trader).NumTrader);
            txtTotalPortefeuille.Text = gst.getTotalPortefeuille((lstTraders.SelectedItem as Trader).NumTrader).ToString();
        }

        private void lstActions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstTraders.SelectedItem != null)
            {
                if (lstActions.SelectedItem == null)
                {
                    imgAction.Source = new BitmapImage(new Uri("", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    if ((lstActions.SelectedItem as ActionPerso).PrixAchat > gst.getCoursReel((lstActions.SelectedItem as ActionPerso).NumAction))
                    {
                        imgAction.Source = new BitmapImage(new Uri("/Images/Bas.png", UriKind.RelativeOrAbsolute));
                    }
                    else if ((lstActions.SelectedItem as ActionPerso).PrixAchat == gst.getCoursReel((lstActions.SelectedItem as ActionPerso).NumAction))
                    {
                        imgAction.Source = new BitmapImage(new Uri("/Images/Moyen.png", UriKind.RelativeOrAbsolute));
                    }
                    else
                    {
                        imgAction.Source = new BitmapImage(new Uri("/Images/Haut.png", UriKind.RelativeOrAbsolute));
                    }
                }
            }
        }

        private void btnVendre_Click(object sender, RoutedEventArgs e)
        {
            int quantiteVendue = 0;
            if (lstActions.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une action");
            }
            else if (txtQuantiteVendue.Text == "")
            {
                MessageBox.Show("Veuillez saisir la quantité");
            }
            else
            {
                try
                {
                    quantiteVendue = Convert.ToInt16(txtQuantiteVendue.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Que des nombres entiers, merci");
                    return;
                }
                if (quantiteVendue > (lstActions.SelectedItem as ActionPerso).Quantite)
                {
                    MessageBox.Show("Vous ne pouvez pas vendre plus d'actions que ce que vous possédez");
                }
                else
                {
                    if (quantiteVendue == (lstActions.SelectedItem as ActionPerso).Quantite)
                    {
                        gst.SupprimerActionAcheter((lstActions.SelectedItem as ActionPerso).NumAction, (lstActions.SelectedItem as ActionPerso).NumTrader);
                    }
                    else
                    {
                        gst.UpdateQuantite((lstActions.SelectedItem as ActionPerso).NumAction, (lstTraders.SelectedItem as Trader).NumTrader, quantiteVendue);
                    }
                    lstActions.ItemsSource = gst.getAllActionsByTrader((lstTraders.SelectedItem as Trader).NumTrader);
                    lstActionsNonPossedees.ItemsSource = gst.getAllActionsNonPossedees((lstTraders.SelectedItem as Trader).NumTrader);
                    txtTotalPortefeuille.Text = gst.getTotalPortefeuille((lstTraders.SelectedItem as Trader).NumTrader).ToString();
                    txtQuantiteVendue.Text = "";
                }
            }
        }

        private void btnAcheter_Click(object sender, RoutedEventArgs e)
        {
            int quantiteAchetee = 0;
            double prixAchat = 0;
            if (lstTraders.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un trader");
            }
            else if (lstActionsNonPossedees.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une action à acheter");
            }
            else if (txtQuantiteAchetee.Text == "")
            {
                MessageBox.Show("Veuillez saisir une quantité à acheter");
            }
            else if (txtPrixAchat.Text == "")
            {
                MessageBox.Show("Veuillez saisir un prix d'achat");
            }
            else
            {
                try
                {
                    quantiteAchetee = Convert.ToInt16(txtQuantiteAchetee.Text);
                    prixAchat = Convert.ToDouble(txtPrixAchat.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Que des nombres entiers, merci");
                    return;
                }
                gst.AcheterAction((lstActionsNonPossedees.SelectedItem as MetierTrader.Action).NumAction, (lstTraders.SelectedItem as Trader).NumTrader, prixAchat, quantiteAchetee);
                lstActions.ItemsSource = gst.getAllActionsByTrader((lstTraders.SelectedItem as Trader).NumTrader);
                lstActionsNonPossedees.ItemsSource = gst.getAllActionsNonPossedees((lstTraders.SelectedItem as Trader).NumTrader);
                txtTotalPortefeuille.Text = gst.getTotalPortefeuille((lstTraders.SelectedItem as Trader).NumTrader).ToString();
                txtPrixAchat.Text = "";
                txtQuantiteAchetee.Text = "";
                MessageBox.Show("Action enregistrée");
            }
        }
    }
}
