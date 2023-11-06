using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (Equals(storage, value)) return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}