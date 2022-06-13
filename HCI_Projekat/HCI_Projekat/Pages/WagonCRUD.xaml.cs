using HCI_Projekat.help;
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
    /// Interaction logic for WagonCRUD.xaml
    /// </summary>
    public partial class WagonCRUD : Page
    {

        public Data dataBase { get; set; }

        public Train train { get; set; }

        public string action { get; set; }

        public WagonCRUD(Train t, Data dataBase, String action)
        {
            InitializeComponent();
            this.dataBase = dataBase;
            this.train = t;
            this.action = action;
            DataContext = this;
            btn_save.IsEnabled = false;
            if (action == "view")
            {
                btn_finish.Visibility = Visibility.Hidden;
            }
            else {
                btn_finish.Visibility = Visibility.Visible;
            }

        }

        public void StartTour()
        {
            var navigator = FeatureTour.GetNavigator();

            navigator.ForStep(ElementID.WagonCapacity).AttachDoable(s => tb_capacity.Text = "40");

            navigator.OnStepEntered(ElementID.WagonButtonAdd).Execute(s => btn_add.Focus());
            navigator.OnStepEntered(ElementID.WagonCapacity).Execute(s => tb_capacity.Focus());
            navigator.OnStepEntered(ElementID.WagonClass).Execute(s => rb_first.Focus());
            navigator.OnStepEntered(ElementID.ButtonFinish).Execute(s => btn_finish.Focus());

            trainWagons_table.IsEnabled = false;
            btn_edit.IsEnabled = false;
            btn_delete.IsEnabled = false;
            btn_back.IsEnabled = false;
            rb_first.IsEnabled = false;
            rb_second.IsEnabled = false;
            btn_finish.IsEnabled = false;
            btn_add.IsEnabled = false;

            btn_add.Click += Tour_btn_add_Click;
            tb_capacity.TextChanged += Tb_capacity_TextChanged;
            rb_first.Checked += Rb_first_Checked;

            TourStarter.StartAddWagonTour();
        }

        private void Tour_btn_add_Click(object sender, RoutedEventArgs e)
        {
            var navigator = FeatureTour.GetNavigator();
            btn_add.IsEnabled = false;
            btn_finish.IsEnabled = true;
            navigator.IfCurrentStepEquals(ElementID.WagonButtonAdd).GoNext();
        }

        private void Rb_first_Checked(object sender, RoutedEventArgs e)
        {
            var navigator = FeatureTour.GetNavigator();
            rb_first.IsEnabled = false;
            btn_add.IsEnabled = true;
            navigator.IfCurrentStepEquals(ElementID.WagonClass).GoNext();
        }

        private void Tb_capacity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tb_capacity.Text.ToString() == "40")
            {
                var navigator = FeatureTour.GetNavigator();
                tb_capacity.IsEnabled = false;
                rb_first.IsEnabled = true;
                navigator.IfCurrentStepEquals(ElementID.WagonCapacity).GoNext();
            }
        }

        private void trainWagons_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {



        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (!checkInput())
            {
                return;
            }


            var Result = MessageBox.Show("Do you want to add new wagon?", "Serbian Railways", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                Wagon wagon = new Wagon();
                wagon.capacity = int.Parse(tb_capacity.Text);
                wagon.wagonClass = (bool)rb_first.IsChecked ? Wagon.WagonClass.first : Wagon.WagonClass.second;
                int id = this.train.wagons.Max(x => x.id);
                wagon.id = id + 1;

                this.train.wagons.Add(wagon);
                tb_capacity.Text = "";
                trainWagons_table.ItemsSource = null;
                trainWagons_table.ItemsSource = train.wagons;
            }

        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            int selectedCells = trainWagons_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("You must select a wagon.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            btn_add.IsEnabled = false;
            btn_edit.IsEnabled = false;
            btn_delete.IsEnabled = false;
            btn_save.IsEnabled = true;

            Wagon t = (Wagon)trainWagons_table.SelectedItem;

            if (t != null)
            {
                tb_capacity.Text = t.capacity.ToString();

                if (t.wagonClass == Wagon.WagonClass.first)
                {
                    rb_first.IsChecked = true;
                }
                else
                {
                    rb_second.IsChecked = true;
                }
            }

        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {

            int selectedCells = trainWagons_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("You must select a wagon.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (train.wagons.Count == 1) {
                MessageBox.Show("Can't delete the wagon! Train must have at least one wagon.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var Result = MessageBox.Show("Do you want to delete the wagon?", "Serbian Railways", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                Wagon t = (Wagon)trainWagons_table.SelectedItem;
                this.train.wagons.Remove(t);
                trainWagons_table.ItemsSource = null;
                trainWagons_table.ItemsSource = train.wagons;
            }

        }

        private bool checkInput()
        {

            int res;
            bool num = int.TryParse(tb_capacity.Text, out res);
            if (!num)
            {
                MessageBox.Show("Capacity must be a number.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (rb_first.IsChecked == false && rb_second.IsChecked == false)
            {
                MessageBox.Show("You must choose a class.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }


        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (!checkInput())
            {
                return;
            }
            var Result = MessageBox.Show("Do you want to change the wagons ?", "Serbian Railways", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                Wagon t = (Wagon)trainWagons_table.SelectedItem;
                foreach (Wagon w in train.wagons)
                {
                    if (w.id == t.id)
                    {
                        t.capacity = int.Parse(tb_capacity.Text);
                        t.wagonClass = (bool)rb_first.IsChecked ? Wagon.WagonClass.first : Wagon.WagonClass.second;
                    }
                }
                trainWagons_table.ItemsSource = null;
                trainWagons_table.ItemsSource = train.wagons;
                MessageBox.Show("Successfully changed wagon!", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Information);

            }

            tb_capacity.Text = "";

            btn_add.IsEnabled = true;
            btn_edit.IsEnabled = true;
            btn_delete.IsEnabled = true;
            btn_save.IsEnabled = false;

        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            if (this.action == "view")
            {
                TrainCRUD r = new TrainCRUD(this.dataBase);
                ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
                window.managerHomepage.Navigate(r);
            }
            else if (this.action == "edit")
            {
                EditTrain r = new EditTrain(this.dataBase, this.train);
                ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
                window.managerHomepage.Navigate(r);
            }
            else {
                AddTrain r = new AddTrain(this.dataBase, this.train);
                ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
                window.managerHomepage.Navigate(r);
            }
        }

        
        
        private void btn_finish_Click(object sender, RoutedEventArgs e)
        {
            if (this.action == "add")
            {
                var Result = MessageBox.Show("Do you want to add the train?", "Serbian Railways", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    this.dataBase.trains.Add(this.train);
                    MessageBox.Show("Successfully added train!", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else {
                var Result = MessageBox.Show("Do you want to change the train?", "Serbian Railways", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    foreach (Train t in dataBase.trains)
                    {
                        if (t.id == train.id)
                        {
                            t.name = train.name;
                            t.rang = train.rang;
                            t.wagons = train.wagons;
                        }
                    }
                    MessageBox.Show("Successfully changed train!", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            TrainCRUD r = new TrainCRUD(this.dataBase);
            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            window.managerHomepage.Navigate(r);
        }

       


    }
}
