using HealthApp.Common.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HealthApp.Models
{
    public class RecordModel : Record, INotifyPropertyChanged
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

        protected static bool EqualsHelper(RecordModel first, RecordModel second)
        {
            return first.Id == second.Id;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            var other = obj as RecordModel;

            if (other == null)
            {
                return false;
            }

            return EqualsHelper(this, other);
        }
    }
}
