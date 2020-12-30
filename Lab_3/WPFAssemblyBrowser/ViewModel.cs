using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Listsoft.Lab3_AssemblyReader;
using Microsoft.Win32;

namespace WPFAssemblyBrowser
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Model _model;
        private ICommand _openCommand;
        public ViewModel(Model model)
        {
            _model = model;
        }
        public string AssemblyFile
        {
            get => _model.AssemblyFile;
            set
            {
                _model.AssemblyFile = value;
                AssemblyInfo = new ObservableCollection<AssemblyInfo>
                {
                    AssemblyReader.GetAssemblyInfo(_model.AssemblyFile)
                };
                OnPropertyChanged("AssemblyFile");
            }
        }
        public ObservableCollection<AssemblyInfo> AssemblyInfo
        {
            get => _model.AssemblyInfo;
            set
            {
                _model.AssemblyInfo = value;
                OnPropertyChanged("AssemblyInfo");
            }
        }
        public ICommand OpenCommand
        {
            get
            {
                return _openCommand ??= new RelayCommand(obj =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == true)
                    {
                        AssemblyFile = openFileDialog.FileName;
                    }
                });
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}