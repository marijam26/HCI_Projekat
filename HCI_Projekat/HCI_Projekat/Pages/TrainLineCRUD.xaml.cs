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
    /// Interaction logic for TrainLineCRUD.xaml
    /// </summary>
    public partial class TrainLineCRUD : Page
    {
        public Data dataBase { get; set; }

        public List<Control> controlList { get; set; }

        public bool tour = false;

        public TrainLineCRUD(Data database)
        {
            InitializeComponent();
            this.dataBase = database;
            DataContext = this;

            this.controlList = new List<Control>() { trainLine_table, btn_add, btn_delete, btn_edit, btn_tutorial};

        }

        private void Show_Stations(object sender, RoutedEventArgs e)
        {
            TrainLine t = (TrainLine)trainLine_table.SelectedItem;
            Stations s = new Stations(t,this.dataBase);
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.Content = s;

        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            int selectedCells = trainLine_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("Must select train line.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var Result = MessageBox.Show("Do you want to delete train line?", "Check", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                TrainLine t = (TrainLine)trainLine_table.SelectedItem;
                this.dataBase.trainLines.Remove(t);
                trainLine_table.ItemsSource = null;
                trainLine_table.ItemsSource = dataBase.trainLines;
            }
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            TrainLine t = (TrainLine)trainLine_table.SelectedItem;
            AddTrainLine s = new AddTrainLine(this.dataBase,null, tour);
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.Content = s;
            if (tour)
            {
                s.StartTour();
            }
            
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            int selectedCells = trainLine_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("Must select train line.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            TrainLine t = (TrainLine)trainLine_table.SelectedItem;
            this.dataBase.backupTrainLine = (TrainLine)t.Clone();
            EditTrainLine s = new EditTrainLine(this.dataBase, t);
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.Content = s;
        }


        private void btn_tutorial_Click(object sender, RoutedEventArgs e)
        {
            var navigator = FeatureTour.GetNavigator();

            //navigator.ForStep(ElementID.ComboBoxFrom).AttachDoable(s => fromPlace.SelectedItem = "beograd");
            //navigator.ForStep(ElementID.ComboBoxTo).AttachDoable(s => toPlace.SelectedItem = "novi sad");


            navigator.OnStepEntered(ElementID.TrainLineButtonAdd).Execute(s => btn_add.Focus());

            //navigator.OnStepEntered(ElementID.PinStart).Execute(s => pinSta.Focus());
           // navigator.OnStepEntered(ElementID.RadioWeekend).Execute(s => rb_weekend.Focus());
           // navigator.OnStepEntered(ElementID.ButtonSearch).Execute(s => btn_search.Focus());

            //toPlace.SelectionChanged += toPlaceSelectionChanged;
            //rb_weekend.Checked += Rb_weekend_Checked;
            //btn_search.Click += searchClicked;
            this.tour = true;

            foreach (Control c in this.controlList)
            {
                c.IsEnabled = false;
            }
            btn_add.IsEnabled = true;
            TourStarter.StartTrainLineTour();
        }
    }
}
