using HCI_Projekat.Model;
using HCI_Projekat.touring;
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
using ThinkSharp.FeatureTouring.Navigation;

namespace HCI_Projekat.Pages
{
    /// <summary>
    /// Interaction logic for AddTrain.xaml
    /// </summary>
    public partial class AddTrain : Page
    {

        public Data dataBase { get; set; }
        public Train newTrain { get; set; }

        public bool tour = false;

        public AddTrain(Data database, bool tour = false)
        {
            InitializeComponent();
            this.dataBase = database;
            DataContext = this;
            this.newTrain = null;
            this.tour = tour;
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#e8cfa5");
            brush.Freeze();
            Background = brush;
        }
        
        public AddTrain(Data database,Train train)
        {
            InitializeComponent();
            this.dataBase = database;
            DataContext = this;
            this.newTrain = train;
            tb_name.Text = train.name;

            if (train.rang == Rang.Soko)
            {
                rb_soko.IsChecked = true;
            }
            else
            {
                rb_simple.IsChecked = true;
            }
        }

        public void StartTour()
        {
            var navigator = FeatureTour.GetNavigator();

            navigator.ForStep(ElementID.TrainName).AttachDoable(s => tb_name.Text = "Soko5");


            navigator.OnStepEntered(ElementID.TrainName).Execute(s => tb_name.Focus());
            navigator.OnStepEntered(ElementID.TrainRang).Execute(s => rb_soko.Focus());
            navigator.OnStepEntered(ElementID.TrainButtonNext).Execute(s => btn_next.Focus());

            rb_soko.IsEnabled = false;
            rb_simple.IsEnabled = false;
            btn_next.IsEnabled = false;
            btn_cancel.IsEnabled = false;

            tb_name.SelectionChanged += Tb_name_SelectionChanged;
            rb_soko.Checked += Rb_soko_Checked;

            TourStarter.StartAddTrainTour();
        }

        private void Rb_soko_Checked(object sender, RoutedEventArgs e)
        {
            var navigator = FeatureTour.GetNavigator();
            rb_soko.IsEnabled = false;
            btn_next.IsEnabled = true;
            navigator.IfCurrentStepEquals(ElementID.TrainRang).GoNext();
        }

        private void Tb_name_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (tb_name.Text.ToString() == "Soko5")
            {
                tb_name.IsEnabled = false;
                rb_soko.IsEnabled = true;
                var navigator = FeatureTour.GetNavigator();
                navigator.IfCurrentStepEquals(ElementID.TrainName).GoNext();
            }
        }

        private bool checkInput()
        {

            String name = tb_name.Text.Trim();
            if (name == "")
            {
                MessageBox.Show("Must enter name.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (rb_simple.IsChecked == false && rb_soko.IsChecked == false)
            {
                MessageBox.Show("Must choose rang.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            if (!checkInput()) {
                return;
            }
            Train t;
            if (this.newTrain == null)
            {
                t = new Train();
                int id = this.dataBase.trains.Max(x => x.id);
                t.id = id + 1;
                t.name = tb_name.Text.Trim();
                t.rang = (bool)rb_simple.IsChecked ? Rang.obicni : Rang.Soko;
            }
            else {
                t = this.newTrain;
            }
            MainWindow window = (MainWindow)Window.GetWindow(this);
            WagonCRUD r = new WagonCRUD(t,this.dataBase,"add");
            window.Content = r;
            if (tour)
            {
                r.StartTour();
            }
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            TrainCRUD r = new TrainCRUD(this.dataBase);
            window.Content = r;
        }
    }
}
