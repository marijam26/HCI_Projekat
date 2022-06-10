using HCI_Projekat.Model;
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

        public StationDialog(Data dataBase,TrainLine trainLine,Location location,string pinType,string parentPage)
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
                MessageBox.Show("Time must be number.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(name == "")
            {
                MessageBox.Show("Must enter name.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            MessageBox.Show("Sucfcessfully added station!", "Success",MessageBoxButton.OK,MessageBoxImage.Information);
            MainWindow window = (MainWindow)App.Current.MainWindow;
            if (parentPage == "add")
            {
                AddTrainLine addTrainLine = new AddTrainLine(this.dataBase,this.trainLine);
                window.Content = addTrainLine;
            }
            else
            {
                EditTrainLine editLine = new EditTrainLine(this.dataBase, this.trainLine);
                window.Content = editLine;
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
    }
}
