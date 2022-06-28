using HealthApp.Common.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HealthApp.ViewModels.Data
{
    public class RecordViewModel : Record, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isBookmark;
        public bool IsBookmark
        {
            get => isBookmark;
            set
            {
                isBookmark = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected static bool EqualsHelper(RecordViewModel first, RecordViewModel second)
        {
            return first.Id == second.Id;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var other = obj as RecordViewModel;

            if (other == null)
            {
                return false;
            }

            return EqualsHelper(this, other);
        }
    }
}