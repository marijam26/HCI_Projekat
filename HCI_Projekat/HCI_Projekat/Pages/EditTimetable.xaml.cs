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
    /// Interaction logic for EditTimetable.xaml
    /// </summary>
    public partial class EditTimetable : Page
    {
        public Data Database { get; set; }
        public Timetable Timetable { get; set; }

        public EditTimetable(Data database, Timetable timetable)
        {
            InitializeComponent();
            this.Database = database;
            this.Timetable = timetable;
        }
    }
}
