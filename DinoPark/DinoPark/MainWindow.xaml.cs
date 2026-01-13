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
        public MainWindow()
        {
            InitializeComponent();
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

        private void btnFeedBruges_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFeedKortrijk_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}