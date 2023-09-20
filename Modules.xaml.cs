using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TimeManagementLibrary;

namespace TimeManagementApp
{
    public partial class Modules : Window
    {
        // Create a collection to store the modules
        private ObservableCollection<Module> modules = new ObservableCollection<Module>();

        private int numberOfWeeks;
        private DateTime startDate;
        private Dictionary<Module, Dictionary<DateTime, double>> moduleSelfStudyHours = new Dictionary<Module, Dictionary<DateTime, double>>();

        public Modules()
        {
            InitializeComponent();
            // Set the ListView's ItemsSource to the ObservableCollection
            ModuleListView.ItemsSource = modules;
        }

        private void AddModule_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve values from text boxes
            string code = ModuleCodeTextBox.Text;
            string name = ModuleNameTextBox.Text;
            int credits;
            int classHours;
            Module newItem = null; // Declare the variable at a higher scope

            if (int.TryParse(CreditsTextBox.Text, out credits) && int.TryParse(ClassHoursTextBox.Text, out classHours))
            {
                // Create a new module item
                newItem = new Module
                {
                    Code = code,
                    Name = name,
                    Credits = credits,
                    ClassHours = classHours
                };

                // Add the new module item to the collection
                modules.Add(newItem);

                // Clear text boxes
                ModuleCodeTextBox.Clear();
                ModuleNameTextBox.Clear();
                CreditsTextBox.Clear();
                ClassHoursTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter valid credits and class hours as integers.");
            }

            if (newItem != null)
            {
                // Calculate self-study hours
                double selfStudyHours = CalculateSelfStudyHours(newItem);

                // Add self-study hours to the dictionary
                moduleSelfStudyHours[newItem] = new Dictionary<DateTime, double>();
                moduleSelfStudyHours[newItem][startDate] = selfStudyHours;

                // Display self-study hours in the ListView
                ModuleListView.Items.Refresh();
            }
        }

        private void RecordHours_Click(object sender, RoutedEventArgs e)
        {
            Module selectedModule = ModuleListView.SelectedItem as Module;
            if (selectedModule == null)
            {
                MessageBox.Show("Select a module from the list first.");
                return;
            }

            DateTime? selectedDate = StartDatePicker.SelectedDate;

            if (selectedDate != null)
            {
                if (double.TryParse(HoursWorkedTextBox.Text, out double hoursWorked))
                {
                    DateTime dateValue = selectedDate.Value;

                    // Create or retrieve the dictionary for hours worked
                    Dictionary<DateTime, double> hoursWorkedDictionary = selectedModule.HoursWorked;

                    if (hoursWorkedDictionary.ContainsKey(dateValue))
                    {
                        // Update the existing entry
                        hoursWorkedDictionary[dateValue] = hoursWorked;
                    }
                    else
                    {
                        // Add a new entry
                        hoursWorkedDictionary.Add(dateValue, hoursWorked);
                    }

                    // Update the ListView to reflect the changes
                    ModuleListView.Items.Refresh();

                    // Recalculate self-study hours
                    double selfStudyHours = CalculateSelfStudyHours(selectedModule);

                    // Update the self-study hours for the selected module
                    selectedModule.SelfStudyHours = selfStudyHours;
                }
                else
                {
                    MessageBox.Show("Enter a valid number of hours worked.");
                }
            }
            else
            {
                MessageBox.Show("Select a date for recording hours.");
            }
        }

        private void WeeksTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (int.TryParse(WeeksTextBox.Text, out int weeks))
            {
                numberOfWeeks = weeks;
            }
        }

        private void StartDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            startDate = StartDatePicker.SelectedDate ?? DateTime.MinValue;
        }

        private double CalculateSelfStudyHours(Module module)
        {
            double selfStudyHours = ((module.Credits * 10.0) / numberOfWeeks) - module.ClassHours;
            return Math.Max(0, selfStudyHours); // Ensure non-negative self-study hours
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}