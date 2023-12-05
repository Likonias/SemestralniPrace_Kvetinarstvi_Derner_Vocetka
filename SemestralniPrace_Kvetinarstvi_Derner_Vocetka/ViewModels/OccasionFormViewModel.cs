using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OccasionFormViewModel : ViewModelBase
    {
        public RelayCommand BtnCancel { get; private set; }
        public RelayCommand BtnOk { get; private set; }
        private readonly AccountStore accountStore;
        public string errorMessage;
        public ObservableCollection<string> OccasionTypeComboBoxItems { get; set; }

        private INavigationService closeNavSer;
        private Occasion occasion;
        private OccasionStore occasionStore;
        private INavigationService openOccasionViewModel;

        public OccasionFormViewModel(INavigationService closeModalNavigationService, OccasionStore occasionStore, INavigationService? openOccasionViewModel)
        {
            closeNavSer = closeModalNavigationService;
            BtnCancel = new RelayCommand(Cancel);
            BtnOk = new RelayCommand(Ok);
            occasion = occasionStore.Occasion;
            this.occasionStore = occasionStore;
            this.openOccasionViewModel = openOccasionViewModel;
            if (occasion != null) { InitializeOccasion(); }
            OccasionTypeComboBoxItems = new ObservableCollection<string>();
            PopulateOccasionTypeComboBox();
        }

        private void PopulateOccasionTypeComboBox()
        {
            OccasionTypeComboBoxItems.Clear();

            foreach (OccasionTypeEnum value in Enum.GetValues(typeof(OccasionTypeEnum)))
            {
                OccasionTypeComboBoxItems.Add(value.ToString());
            }
        }

        private void Cancel()
        {
            closeNavSer.Navigate();
        }

        private async void Ok()
        {
            if (CheckOccasion())
            {
                OccasionRepository occasionRepository = new OccasionRepository();

                if (occasionStore.Occasion == null)
                {
                    occasion = new Occasion(0, OccasionType ?? OccasionTypeEnum.Birthday, Note);
                    await occasionRepository.Add(occasion);
                }
                else
                {
                    occasion = new Occasion(occasionStore.Occasion.Id, OccasionType ?? OccasionTypeEnum.Birthday, Note);
                    await occasionRepository.Update(occasion);
                }

                closeNavSer.Navigate();
                openOccasionViewModel.Navigate();
            }
            else
            {
                ErrorMessage = "Adding failed!";
            }
        }

        private bool CheckOccasion()
        {
            return Note != null;
        }

        private void InitializeOccasion()
        {
            _occasionType = occasion.OccasionType;
            _note = occasion.Note;
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        private OccasionTypeEnum? _occasionType;
        public OccasionTypeEnum? OccasionType
        {
            get { return _occasionType; }
            set
            {
                _occasionType = value;
                OnPropertyChanged(nameof(OccasionType));
            }
        }

        private string _note;
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged(nameof(Note));
            }
        }
    }
}
