using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AbbreviationDialog
{
    internal class AbbreviationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Abbreviations> abbreviations { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public string abbreviation { get; set; }
        public ICollectionView FilteredAbbreviations =>
        System.Windows.Data.CollectionViewSource.GetDefaultView(abbreviations);
        public AbbreviationViewModel(List<Abbreviations> _abbreviations)
        {
            abbreviations = new ObservableCollection<Abbreviations>(_abbreviations);
            FilteredAbbreviations.Filter = new System.Predicate<object>(a => Filter(a as Abbreviations));
        }
        private bool Filter(Abbreviations _abbv)
        {
            return Search == null
            || _abbv.Abbreviation.IndexOf(Search, System.StringComparison.OrdinalIgnoreCase) != -1
            || _abbv.Description.IndexOf(Search, System.StringComparison.OrdinalIgnoreCase) != -1;
        }
        private string search;
        public string Search
        {
            get { return search; }
            set
            {
                search = value;
                NotifyPropertyChanged("Search");
                FilteredAbbreviations.Refresh();
            }
        }
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class Abbreviations
    {
        public string Abbreviation { get; set; }
        public string Description { get; set; }
    }

}
