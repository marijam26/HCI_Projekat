using HCI_Projekat.Model;
using HCI_Projekat.touring;
using Microsoft.Maps.MapControl.WPF;
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
using System.Windows.Shapes;
using ThinkSharp.FeatureTouring.Navigation;

namespace HCI_Projekat.Pages
{
    /// <summary>
    /// Interaction logic for StationDialog.xaml
    /// </summary>
    public partial class StationDialog : Window
    {
        public bool isSaved { get; set; }
        public Data dataBase { get; set; }
        public TrainLine trainLine { get; set; }
        public Location location { get; set; }
        public string pinType { get; set; }
        public string parentPage { get; set; }
        public TrainLine backup { get; set; }
        public bool tour = false;

        public StationDialog(Data dataBase,TrainLine trainLine,Location location,string pinType,string parentPage, bool tour=false)
        {
            InitializeComponent();
            Uri uri = new Uri("../../Images/icon.png", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(uri);
            this.isSaved = false;
            this.dataBase = dataBase;
            this.trainLine = trainLine;
            this.location = location;
            this.pinType = pinType;
            this.parentPage = parentPage;
            if(trainLine.stations.Count == 0)
            {
                time_after.IsEnabled = false;
                time_before.IsEnabled = false;
            }else if (trainLine.stations.Count == 1 || (parentPage=="edit" && (pinType=="pin_start" || pinType=="pin_end")))
            {
                time_after.IsEnabled = false;
                time_before.IsEnabled = true;
            }
            this.tour = tour;
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            string name = station_name.Text;
            int res1;
            bool t1 = int.TryParse(time_before.Text, out res1);
            int res2;
            bool t2 = int.TryParse(time_after.Text, out res2);
            if((!t1 && time_before.IsEnabled) || (!t2 && time_after.IsEnabled ))
            {
                MessageBox.Show("Time must be a number.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(name == "")
            {
                MessageBox.Show("You must enter a name.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (this.trainLine.stations.Select(x => x.name).ToArray().Contains(name))
            {
                MessageBox.Show("Station name already exists.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            int idTrainLine = this.dataBase.trainLines.Max(x => x.id);
            if (parentPage == "add")
            {
                idTrainLine -= 1;
            }
            TrainLine lastTrainLine = this.dataBase.trainLines.Where(x => x.id == idTrainLine).First();
            int lastStation = lastTrainLine.stations.Max(x => x.id);
            Station s = new Station(lastStation + 1, name, this.location.Latitude, this.location.Longitude);

            if (this.pinType == "pin")
            {
                editBluePin(name,res1,res2,s);
            }else if(this.trainLine.stations.Count == 0)
            {
                this.trainLine.stations.Add(s);
            }
            else if(this.trainLine.stations.Count == 1)
            {
                if (trainLine.stations[0].latitude < s.latitude)
                {
                    this.trainLine.stations.Add(s);
                }
                else
                {
                    this.trainLine.stations.Insert(0, s);
                    this.trainLine.time.Add(res1);
                }
            }
            if (this.pinType == "pin_start")
            {
                Station from = this.trainLine.from;
                this.trainLine.from = s;
                if(this.parentPage == "edit")
                {
                    int index = this.trainLine.stations.IndexOf(from);
                    this.trainLine.stations.Remove(from);
                    this.trainLine.stations.Insert(index, s);
                    this.trainLine.time.RemoveAt(0);
                    this.trainLine.time.Insert(0, res1);
                }
            }else if (this.pinType == "pin_end")
            {
                Station to = this.trainLine.to;
                this.trainLine.to = s;
                if (this.parentPage == "edit")
                {
                    int index = this.trainLine.stations.IndexOf(to);
                    this.trainLine.stations.Remove(to);
                    this.trainLine.stations.Insert(index, s);
                    this.trainLine.time.RemoveAt(this.trainLine.time.Count-1);
                    this.trainLine.time.Insert(this.trainLine.time.Count - 1, res1);
                }
            }


            this.isSaved = true;
            this.Close();
            MessageBox.Show("Successfully added station!", "Success",MessageBoxButton.OK,MessageBoxImage.Information);
            ManagerHomepage window = (ManagerHomepage)App.Current.Windows[App.Current.Windows.Count-1];
            
            if (parentPage == "add")
            {
              
                AddTrainLine addTrainLine = new AddTrainLine(this.dataBase,this.trainLine, tour);
                window.managerHomepage.Navigate(addTrainLine);
                if (tour)
                {
                    addTrainLine.ContinueTour();
                }
               
            }
            else
            {
                EditTrainLine editLine = new EditTrainLine(this.dataBase, this.trainLine);
                window.managerHomepage.Navigate(editLine);
            }
            
        }

        public void editBluePin(string name, int res1,int res2,Station s)
        {

            if (this.trainLine.stations.Last().latitude > this.trainLine.stations.First().latitude)
            {
                //Station first = this.trainLine.stations.First();
                //this.trainLine.stations[0] = this.trainLine.stations.Last();
                //this.trainLine.stations[this.trainLine.stations.Count-1] = first;
                this.trainLine.stations = this.trainLine.stations.OrderByDescending(x => x.latitude).ToList();
            }

           

            for(int i = 0; i < this.trainLine.stations.Count-1; i++)
            {
                if (this.trainLine.stations[i].latitude > s.latitude && this.trainLine.stations[i+1].latitude < s.latitude)
                {
                    this.trainLine.stations.Insert(i+1, s);
                    this.trainLine.time.Remove(i);
                    this.trainLine.time.Insert(i, res2);
                    this.trainLine.time.Insert(i, res1);

                }
            }
                    
            if (this.trainLine.stations.Last().latitude > s.latitude)
            {
                int index = this.trainLine.stations.IndexOf(this.trainLine.stations.Last());
                this.trainLine.stations.Insert(index, s);
                this.trainLine.time.Remove(index - 1);
                this.trainLine.time.Insert(index - 1, res2);
                this.trainLine.time.Insert(index - 1, res1);
            }
            if (this.trainLine.stations.First().latitude < s.latitude)
            {
                this.trainLine.stations.Insert(1, s);
                this.trainLine.time.Remove(0);
                this.trainLine.time.Insert(0, res2);
                this.trainLine.time.Insert(0, res1);
            }

        }

        public void ContinueTour()
        {
            var navigator = FeatureTour.GetNavigator();
            

            navigator.IfCurrentStepEquals(ElementID.PinStart).GoNext();
            navigator.IfCurrentStepEquals(ElementID.PinEnd).GoNext();
            navigator.IfCurrentStepEquals(ElementID.Pin).GoNext();

            navigator.OnStepEntered(ElementID.StationName).Execute(s => station_name.Focus());
            if (trainLine.stations.Count == 0)
            {
                navigator.ForStep(ElementID.StationName).AttachDoable(s => station_name.Text = "Novi Sad");
            }

            if (trainLine.stations.Count == 1)
            {
                time_before.IsEnabled = false;
                navigator.ForStep(ElementID.StationName).AttachDoable(s => station_name.Text = "Loznica");

            }
            if (trainLine.stations.Count == 2)
            {
                time_before.IsEnabled = false;
                time_after.IsEnabled = false;
                navigator.ForStep(ElementID.StationName).AttachDoable(s => station_name.Text = "Sabac");

            }
            station_name.SelectionChanged += Station_name_SelectionChanged;
            time_before.TextChanged += Time_before_TextChanged;
            time_after.TextChanged += Time_after_TextChanged;
            btn_save.IsEnabled = false;
        }

        private void Time_after_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(time_after.Text.ToString() == "40")
            {
                time_after.IsEnabled = false;
                btn_save.IsEnabled = true;
                var navigator = FeatureTour.GetNavigator();
                navigator.IfCurrentStepEquals(ElementID.TimeAfter).GoNext();
            }
        }

        private void Time_before_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(time_before.Text.ToString() == "20")
            {
                time_before.IsEnabled = false;
                if (trainLine.stations.Count == 2)
                {
                    time_after.IsEnabled = true;
                }
                else
                {
                    btn_save.IsEnabled = true;
                }

                var navigator = FeatureTour.GetNavigator();
                navigator.IfCurrentStepEquals(ElementID.TimeBefore).GoNext();
            }
        }

        private void Station_name_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if ((station_name.Text.ToString() == "Novi Sad" && trainLine.stations.Count == 0) || (station_name.Text.ToString() == "Loznica" && trainLine.stations.Count == 1) || (station_name.Text.ToString() == "Sabac" && trainLine.stations.Count == 2))
            {
                station_name.IsEnabled = false;
                var navigator = FeatureTour.GetNavigator();
                if (trainLine.stations.Count == 1 || trainLine.stations.Count == 2)
                {
                    time_before.IsEnabled = true;
                }
                if (trainLine.stations.Count == 0)
                {
                    btn_save.IsEnabled = true;
                }
                navigator.IfCurrentStepEquals(ElementID.StationName).GoNext();

            }
        }
    }
}
