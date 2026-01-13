using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DinoPark.Core.Classes;
using DinoPark.Core.Enums;

namespace DinoPark
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DinoParc _brugesParc;
        private DinoParc _kortrijkParc;
        private readonly Random _random = new();
        public MainWindow()
        {
            InitializeComponent();


            _brugesParc = new DinoParc("Dinos' GoWest Bruges", true);
            _kortrijkParc = new DinoParc("Dino's GoWest Kortrijk", false);

            lstDinosaurBruges.ItemsSource = _brugesParc.Dinosaurs;
            lstDinosaurKortrijk.ItemsSource = _kortrijkParc.Dinosaurs;
        }
        private void FeedParc(DinoParc parc, string parcName)
        {
            // 50% kans vlees / 50% plant
            bool giveMeat = _random.Next(2) == 0;
            int amount = _random.Next(10, 101);

            if (giveMeat)
            {
                Meat meat = Meat.Chicken;   // examen-safe
                parc.Feed(meat, amount);
                MessageBox.Show($"{parcName}: {amount}kg {meat} gegeven.");
            }
            else
            {
                Plant plant = Plant.Leaf;   // examen-safe
                parc.Feed(plant, amount);
                MessageBox.Show($"{parcName}: {amount}kg {plant} gegeven.");
            }

            lstDinosaurBruges.Items.Refresh();
            lstDinosaurKortrijk.Items.Refresh();
        }
        private void TglSedateYesNo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggle)
            {
                Dinosaur? selectedDino =
                lstDinosaurBruges.SelectedItem as Dinosaur ??
                lstDinosaurKortrijk.SelectedItem as Dinosaur;

                if (selectedDino == null)
                {
                    MessageBox.Show("Geen dinosaurus geselecteerd.");
                    return;
                }

                if (!selectedDino.IsAlive)
                {
                    MessageBox.Show("Een dode dinosaurus kan niet verdoofd worden.");
                    toggle.IsChecked = false;
                    return;
                }

                selectedDino.IsSedated = toggle.IsChecked == true;

                // Refresh listboxes
                lstDinosaurBruges.Items.Refresh();
                lstDinosaurKortrijk.Items.Refresh();
            }
        }

        private void BtnFeedBruges_Click(object sender, RoutedEventArgs e)
        {
            FeedParc(_brugesParc, "Bruges");
        }
        private void BtnFeedKortrijk_Click(object sender, RoutedEventArgs e)
        {
            FeedParc(_kortrijkParc, "Kortrijk");
        }
        private void btnMoveRight_Click(object sender, RoutedEventArgs e)
        {
            // 1. Check selectie
            if (lstDinosaurBruges.SelectedItem is not Dinosaur selectedDino)
            {
                MessageBox.Show("Geen dinosaurus geselecteerd.");
                return;
            }

            try
            {
                // 2. Transport uitvoeren
                _brugesParc.Transport(selectedDino, _kortrijkParc);

                // 3. UI verversen
                lstDinosaurBruges.Items.Refresh();
                lstDinosaurKortrijk.Items.Refresh();
            }
            catch (Exception ex)
            {
                // 4. Fouten tonen (niet transporteerbaar / gestorven)
                MessageBox.Show(ex.Message);

                // UI verversen (voor geval dino gestorven is)
                lstDinosaurBruges.Items.Refresh();
                lstDinosaurKortrijk.Items.Refresh();
            }
        }

        private void btnMoveLeft_Click(object sender, RoutedEventArgs e)
        {
            // 1. Check selectie
            if (lstDinosaurKortrijk.SelectedItem is not Dinosaur selectedDino)
            {
                MessageBox.Show("Geen dinosaurus geselecteerd.");
                return;
            }

            try
            {
                // 2. Transport uitvoeren
                _kortrijkParc.Transport(selectedDino, _brugesParc);

                // 3. UI verversen
                lstDinosaurKortrijk.Items.Refresh();
                lstDinosaurBruges.Items.Refresh();
            }
            catch (Exception ex)
            {
                // 4. Foutmelding tonen
                MessageBox.Show(ex.Message);

                // UI verversen (belangrijk bij sterfte)
                lstDinosaurKortrijk.Items.Refresh();
                lstDinosaurBruges.Items.Refresh();
            }
        }   
    }
}