using Assignment_12._3._2.Data;
using Assignment_12._3._2.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
namespace Assignment_12._3._2
{
    public partial class MainPage : ContentPage
    {       
        readonly PetContext _context;
        public ObservableCollection<Pet> PetList { get; set; }
        bool isNewPet;
        Pet updatePetInfo;

        public MainPage(PetContext petContext)
        {
            InitializeComponent();
            btnSave.IsEnabled = false;
            isNewPet = false;
            _context=petContext;
            //make the observable collection
            PetList = new ObservableCollection<Pet>();
            BindingContext = this;  //looking for an observable collecion to bind to in this page
            LoadUsers();
            
        }

        public async void LoadUsers()
        {
            try
            {
                await _context.Database.EnsureCreatedAsync();
                var petList = await _context.PetList.ToListAsync();
                foreach (var pet in petList)
                {
                    PetList.Add(pet);
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }


        private async void OnAddNewClicked(object? sender, EventArgs e)
        {            
            isNewPet = true;
            btnSave.IsEnabled = true;
            btnAddNewDog.IsEnabled = false;
        }
        private async void OnDeleteClicked(object? sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Pet pet)
            {
                bool confirmDelete = await DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {pet.Name}", "Yes", "No");
                if (!confirmDelete) { return; }
                //remove from db
                _context.PetList.Remove(pet);
                await _context.SaveChangesAsync();
                PetList.Remove(pet); //refresh displayed list
            }
        }

        private async void OnEditClicked(object? sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Pet pet)
            {
                updatePetInfo = pet;
                btnSave.IsEnabled = true;
                btnAddNewDog.IsEnabled = false;
                txtNewDogName.Text = pet.Name;
                txtNewDogType.Text = pet.Type;
            }

        }
        private async void OnSaveClicked(object? sender, EventArgs e)
        {
            btnSave.IsEnabled = false;
            if (isNewPet)
            {
                isNewPet = false;
                btnAddNewDog.IsEnabled = true;
                try
                {
                    if (!string.IsNullOrWhiteSpace(txtNewDogName.Text))
                    {
                        Pet newPet = new Pet { Name = txtNewDogName.Text, Type = txtNewDogType.Text };
                        await _context.PetList.AddAsync(newPet);
                        await _context.SaveChangesAsync();
                        PetList.Add(newPet);
                        txtNewDogName.Text = string.Empty;
                        txtNewDogType.Text = string.Empty;
                    }
                    else
                    {
                        await DisplayAlert("Error","New Add Canceled, Name is Required","OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
            else
            {
                btnAddNewDog.IsEnabled = true;
                try
                {
                    if (!string.IsNullOrWhiteSpace(txtNewDogName.Text))
                    {
                        updatePetInfo.Name = txtNewDogName.Text;
                        updatePetInfo.Type = txtNewDogType.Text;
                        _context.PetList.Update(updatePetInfo);
                        await _context.SaveChangesAsync();
                        // Refresh the ObservableCollection
                        var index = PetList.IndexOf(updatePetInfo);
                        if (index >= 0)
                        {
                            PetList[index] = updatePetInfo;
                        }
                        txtNewDogName.Text = string.Empty;
                        txtNewDogType.Text = string.Empty;
                    }
                    else
                    {
                        await DisplayAlert("Error", "New Add Canceled, Name is Required", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }
    }
}
