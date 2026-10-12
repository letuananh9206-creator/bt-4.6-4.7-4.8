using System.ComponentModel;

namespace bt_4._6
{
    public sealed class Student : INotifyPropertyChanged
    {
        private string studentId = string.Empty;
        private string fullName = string.Empty;
        private int age;
        private string gender = "Nam";
        private string avatarPath = string.Empty;

        public string StudentId
        {
            get => studentId;
            set => SetField(ref studentId, value);
        }

        public string FullName
        {
            get => fullName;
            set => SetField(ref fullName, value);
        }

        public int Age
        {
            get => age;
            set => SetField(ref age, value);
        }

        public string Gender
        {
            get => gender;
            set => SetField(ref gender, value);
        }

        public string AvatarPath
        {
            get => avatarPath;
            set => SetField(ref avatarPath, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void SetField<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return;
            }

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
