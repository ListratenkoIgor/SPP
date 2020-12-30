using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Listsoft.Lab3_AssemblyReader;

namespace WPFAssemblyBrowser
{
    public class Model : INotifyPropertyChanged
    {
        private string _assemblyFile;
        private ObservableCollection<AssemblyInfo> _assemblyInfo;
        public event PropertyChangedEventHandler PropertyChanged;
        public string AssemblyFile
        {
            get => _assemblyFile;
            set
            {
                _assemblyFile = value;
                OnPropertyChanged("AssemblyFile");
            }
        }
        public ObservableCollection<AssemblyInfo> AssemblyInfo
        {
            get => _assemblyInfo;
            set
            {
                _assemblyInfo = value;
                OnPropertyChanged("AssemblyInfo");
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}