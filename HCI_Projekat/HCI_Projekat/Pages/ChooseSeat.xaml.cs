using HCI_Projekat.Model;
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

namespace HCI_Projekat.Pages
{
    /// <summary>
    /// Interaction logic for ChooseSeat.xaml
    /// </summary>
    public partial class ChooseSeat : Page
    {
        public Data database { get; set; }
        public TimetableDTO timetable { get; set; }

        public ChooseSeat(Data database,TimetableDTO timetable)
        {
            InitializeComponent();
            this.database = database;
            this.timetable = timetable;

            for (int i = 0; i < timetable.train.wagons.Count;i++) {
                wagonsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            int wagonCounter = 0;
            foreach (Wagon w in timetable.train.wagons) {
                wagonCounter++;
                Button wagonBtn = new Button();
                wagonBtn.Click += new RoutedEventHandler(btn_wagon_Click);
                wagonBtn.Width = 50;
                wagonBtn.Height = 30;
                wagonBtn.HorizontalAlignment = HorizontalAlignment.Center;
                wagonBtn.Content = "Wagon"+wagonCounter;
                Grid.SetRow(wagonBtn, 1);
                Grid.SetColumn(wagonBtn, wagonCounter);
                wagonsGrid.Children.Add(wagonBtn);
            }
        }

        private void btn_wagon_Click(object sender, RoutedEventArgs e)
        {
            seatGrid.ColumnDefinitions.Clear();
            seatGrid.RowDefinitions.Clear();
            seatGrid.Children.Clear();
            Button wagonBtn = sender as Button;
            Wagon wagon = timetable.train.wagons[Grid.GetColumn(wagonBtn)-1];
            for (int i = 0; i < 4; i++) {
                seatGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            double rowNum = wagon.capacity / 4;
            int rows = (int)rowNum;
            if (rowNum != rows)
            {
                rows += 1;
            }
            for (int i = 0; i < rows; i++) {
                seatGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i =0;i<wagon.capacity;i++) {
                int row = i / 4;
                int column = i % 4;
                Button seatBtn = new Button();
                seatBtn.Margin = new Thickness(5);
                seatBtn.MinWidth = 40;
                seatBtn.MaxHeight = 40;
                seatBtn.HorizontalAlignment = HorizontalAlignment.Center;
                seatBtn.Content = i;
                Grid.SetRow(seatBtn, row);
                Grid.SetColumn(seatBtn, column);
                seatGrid.Children.Add(seatBtn);
            }
        }
    }
}
