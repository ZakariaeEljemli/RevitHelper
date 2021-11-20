using System.Collections.Generic;
using System.Windows;

namespace AbbreviationDialog
{
    /// <summary>
    /// Interaction logic for AbbreviationsWPF.xaml
    /// </summary>
    public partial class AbbreviationsWPF : Window
    {
        public AbbreviationsWPF(List<Abbreviations> _Abbreviations)
        {
            InitializeComponent();
            DataContext = new AbbreviationViewModel(_Abbreviations);
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void dgAbbreviations_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Abbreviations selected = (Abbreviations)dgAbbreviations.SelectedItem;
            Clipboard.SetText(selected.Abbreviation);
        }
    }
}