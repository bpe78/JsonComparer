using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using JsonComparer.WPF.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using ViewModels;

namespace JsonComparer.WPF
{
    class MainViewModel : BindableBase
    {
        private readonly IUIService _uiService;

        public MainViewModel(IUIService uiService)
        {
            _uiService = uiService ?? throw new ArgumentNullException(nameof(uiService));

            CompareVM = new JsonCompareViewModel();
        }

        #region LoadLeftFileCmd

        DelegateCommand _loadLeftFileCmd;
		public ICommand LoadLeftFileCmd
        {
            get { return _loadLeftFileCmd ?? (_loadLeftFileCmd = new DelegateCommand(ExecuteLoadLeftFile, CanExecuteLoadLeftFile)); }
        }

        private void ExecuteLoadLeftFile()
        {
            if (!CanExecuteLoadLeftFile())
                throw new InvalidOperationException();

            var selectedFile = _uiService.LoadFileDialog();
            if (!string.IsNullOrEmpty(selectedFile))
            {
                LeftFilePath = selectedFile;
                CompareVM.LeftSource = LoadContent(selectedFile);
            }
        }

        private bool CanExecuteLoadLeftFile() => true;

        #endregion

        string _leftFilePath;
        public string LeftFilePath
        {
            get { return _leftFilePath; }
            private set { SetProperty(ref _leftFilePath, value); }
        }

        #region LoadRightFileCmd

        DelegateCommand _loadRightFileCmd;
        public ICommand LoadRightFileCmd
        {
            get { return _loadRightFileCmd ?? (_loadRightFileCmd = new DelegateCommand(ExecuteLoadRightFile, CanExecuteLoadRightFile)); }
        }

        private void ExecuteLoadRightFile()
        {
            if (!CanExecuteLoadRightFile())
                throw new InvalidOperationException();

            var selectedFile = _uiService.LoadFileDialog();
            if (!string.IsNullOrEmpty(selectedFile))
            {
                RightFilePath = selectedFile;
                CompareVM.RightSource = LoadContent(selectedFile);
            }
        }

        private bool CanExecuteLoadRightFile() => true;

        #endregion

        string _rightFilePath;
        public string RightFilePath
        {
            get { return _rightFilePath; }
            private set { SetProperty(ref _rightFilePath, value); }
        }

        public JsonCompareViewModel CompareVM { get; }

        private string LoadContent(string filename)
        {
            return string.Join(string.Empty, File.ReadAllLines(filename).Select(s => s.Replace("\r\n", string.Empty)).ToArray());
        }
    }
}
