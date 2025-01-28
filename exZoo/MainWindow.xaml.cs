using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace exZoo
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Animal> Animals { get; set; }
        private Animal selectedAnimal;

        public MainWindow()
        {
            InitializeComponent();
            Animals = new ObservableCollection<Animal>();
            MyZOO.ItemsSource = Animals;

            // Add initial animals
            AddInitialAnimals();
        }

        private void AddInitialAnimals()
        {
            string[] initialAnimals = {
                "Leo,King,Lion,leo@zoo.com,190.5,2018-03-15,Yellow,Kenya,true",
                "Ellie,Gentle Giant,Monkey,ellie@zoo.com,3500.0,2010-07-22,Gray,Tanzania,false",
                "Tigger,Stripes,Tiger,tigger@zoo.com,220.0,2015-05-10,Brown,India,true",
                "Gigi,Spots,Bird,gigi@zoo.com,1200.0,2012-09-05,White,Kenya,false"
            };

            foreach (var animalData in initialAnimals)
            {
                AddAnimalFromString(animalData);
            }
        }

        private void AddAnimalFromString(string animalData)
        {
            var parts = animalData.Split(',');
            Animals.Add(new Animal
            {
                Name = parts[0],
                Nickname = parts[1],
                Type = parts[2],
                Email = parts[3],
                Weight = double.Parse(parts[4]),
                BirthDate = DateTime.Parse(parts[5]),
                Color = parts[6],
                Country = parts[7],
                IsPredator = bool.Parse(parts[8])
            });
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            sp.Visibility = Visibility.Visible;
       
            
            if (MyZOO.SelectedItem is Animal animalToEdit)
            {
                selectedAnimal = animalToEdit;

                // Populate fields for editing
                txtName.Text = selectedAnimal.Name;
                txtNickname.Text = selectedAnimal.Nickname;
                txtType.Text = selectedAnimal.Type;
                txtEmail.Text = selectedAnimal.Email;
                txtWeight.Text = selectedAnimal.Weight.ToString();
                cmbColor.Text = selectedAnimal.Color;
                txtCountry.Text = selectedAnimal.Country;
                dtBirthDate.SelectedDate = selectedAnimal.BirthDate;
                rdPredatorYes.IsChecked = selectedAnimal.IsPredator;
                rdPredatorNo.IsChecked = !selectedAnimal.IsPredator;
            }
            else
            {
                MessageBox.Show("Please select an animal to edit.");
            }
        }

        private void MyZOO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedAnimal = MyZOO.SelectedItem as Animal;

            if (selectedAnimal != null)
            {
                // Populate fields for editing
                txtName.Text = selectedAnimal.Name;
                txtNickname.Text = selectedAnimal.Nickname;
                txtType.Text = selectedAnimal.Type;
                txtEmail.Text = selectedAnimal.Email;
                txtWeight.Text = selectedAnimal.Weight.ToString();
                cmbColor.Text = selectedAnimal.Color;
                txtCountry.Text = selectedAnimal.Country;
                dtBirthDate.SelectedDate = selectedAnimal.BirthDate;
                rdPredatorYes.IsChecked = selectedAnimal.IsPredator;
                rdPredatorNo.IsChecked = !selectedAnimal.IsPredator;
            }
        }
       
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Validation
            if (!ValidateInput())
                return;

            // If editing existing animal
            if (selectedAnimal != null)
            {
                UpdateExistingAnimal();
            }
            else
            {
                CreateNewAnimal();
            }

            // Clear fields
            ClearInputFields();
            MessageBox.Show("Animal saved successfully!");
        }

        private bool ValidateInput()
        {
            // Name validation
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                txtName.Text.Length < 2 || txtName.Text.Length > 10)
            {
                MessageBox.Show("Name must be between 2 and 10 characters!");
                return false;
            }

            // Email validation
            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email!");
                return false;
            }

            // Weight validation
            if (string.IsNullOrWhiteSpace(txtWeight.Text) || !double.TryParse(txtWeight.Text, out _))
            {
                MessageBox.Show("Please enter a valid weight!");
                return false;
            }

            // Country validation
            if (string.IsNullOrWhiteSpace(txtCountry.Text) ||
                txtCountry.Text.Length < 2 || txtCountry.Text.Length > 10)
            {
                MessageBox.Show("Country must be between 2 and 10 characters!");
                return false;
            }

            // Color validation
            string[] validColors = { "Gray", "Black", "White", "Yellow", "Brown", "Red" };
            if (!Array.Exists(validColors, c => c.Equals(cmbColor.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Please select a valid color!");
                return false;
            }

            // Birth date validation
            if (!dtBirthDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select a birth date!");
                return false;
            }

            return true;
        }

        private void UpdateExistingAnimal()
        {
            selectedAnimal.Name = txtName.Text;
            selectedAnimal.Nickname = txtNickname.Text;
            selectedAnimal.Type = txtType.Text; // Fixed here to update 'Type' from the textbox
            selectedAnimal.Email = txtEmail.Text;
            selectedAnimal.Weight = double.Parse(txtWeight.Text);
            selectedAnimal.Color = cmbColor.Text;
            selectedAnimal.Country = txtCountry.Text;
            selectedAnimal.BirthDate = dtBirthDate.SelectedDate.Value;
            selectedAnimal.IsPredator = rdPredatorYes.IsChecked == true;

            MyZOO.Items.Refresh();
            //יש לעדכן גם את מסד הנתונים
        }

        private void CreateNewAnimal()
        {
            Animal newAnimal = new Animal
            {
                Name = txtName.Text,
                Nickname = txtNickname.Text,
                Type = txtType.Text,
                Email = txtEmail.Text,
                Weight = double.Parse(txtWeight.Text),
                Color = cmbColor.Text,
                Country = txtCountry.Text,
                BirthDate = dtBirthDate.SelectedDate.Value,
                IsPredator = rdPredatorYes.IsChecked == true
            };

            Animals.Add(newAnimal);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (MyZOO.SelectedItem is Animal animalToRemove)
            {
                Animals.Remove(animalToRemove);
                ClearInputFields();
                selectedAnimal = null;
                MessageBox.Show("Animal removed successfully!");
            }
            else
            {
                MessageBox.Show("Please select an animal to remove.");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ClearInputFields();
            selectedAnimal = null;
        }

        private void ClearInputFields()
        {
            txtName.Clear();
            txtNickname.Clear();
            txtEmail.Clear();
            txtType.Clear();
            txtWeight.Clear();
            txtCountry.Clear();
            cmbColor.SelectedIndex = -1; // Reset ComboBox
            dtBirthDate.SelectedDate = null;
            rdPredatorNo.IsChecked = true; // Default to "No"
        }
    }
}
