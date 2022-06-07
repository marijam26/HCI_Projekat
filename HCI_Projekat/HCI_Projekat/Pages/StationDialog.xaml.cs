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

        public StationDialog(Data dataBase,TrainLine trainLine,Location location)
        {
            InitializeComponent();
            Uri uri = new Uri("../../Images/icon.png", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(uri);
            this.isSaved = false;
            this.dataBase = dataBase;
            this.trainLine = trainLine;
            this.location = location;
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            string name = station_name.Text;
            int res1;
            bool t1 = int.TryParse(time_before.Text, out res1);
            int res2;
            bool t2 = int.TryParse(time_after.Text, out res2);
            if(!t1 || !t2)
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
            TrainLine lastTrainLine = this.dataBase.trainLines.Where(x => x.id == idTrainLine).First();
            int lastStation = lastTrainLine.stations.Max(x => x.id);
            Station s = new Station(lastStation+1,name,this.location.Latitude,this.location.Longitude);
            foreach (TrainLine tl in this.dataBase.trainLines)
            {
                if(tl.id == this.trainLine.id)
                {
                    Station last = tl.stations.Last();
                    tl.stations.Remove(last);
                    tl.stations.Add(s);
                    tl.stations.Add(last);
                    this.trainLine = tl;
                }
            }
            this.isSaved = true;
            this.Close();
            AddTrainLine addLine = new AddTrainLine(this.dataBase, this.trainLine);
            //ono sa mainwindow

        }
    }
}
