using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Models;
using Prism.Commands;
using Prism.Mvvm;

namespace ViewModels
{
    public sealed class JsonCompareViewModel : BindableBase
    {
        private Dictionary<JsonObject, List<JsonObjectViewModel>> _map;

        public JsonCompareViewModel()
        {
            _map = new Dictionary<JsonObject, List<JsonObjectViewModel>>();
        }

        string _leftSource;
        public string LeftSource
        {
            get { return _leftSource; }
            set
            {
                if (SetProperty(ref _leftSource, value))
                {
                    _compareCmd.RaiseCanExecuteChanged();
                    var tree = JsonObject.Create(_leftSource);
                    LeftVM = new[] { Transform(tree, null, ref _map) };
                };
            }
        }

        IEnumerable<JsonObjectViewModel> _leftVM;
        public IEnumerable<JsonObjectViewModel> LeftVM
        {
            get { return _leftVM; }
            private set { SetProperty(ref _leftVM, value); }
        }

        string _rightSource;
        public string RightSource
        {
            get { return _rightSource; }
            set
            {
                if (SetProperty(ref _rightSource, value))
                {
                    _compareCmd.RaiseCanExecuteChanged();
                    var tree = JsonObject.Create(_rightSource);
                    RightVM = new [] { Transform(tree, null, ref _map) };
                };
            }
        }

        IEnumerable<JsonObjectViewModel> _rightVM;
        public IEnumerable<JsonObjectViewModel> RightVM
        {
            get { return _rightVM; }
            private set { SetProperty(ref _rightVM, value); }
        }

        #region CompareCmd

        DelegateCommand _compareCmd;
        public ICommand CompareCmd
        {
            get { return _compareCmd ?? (_compareCmd = new DelegateCommand(ExecuteCompare, CanExecuteCompare)); }
        }

        private void ExecuteCompare()
        {
            if (!CanExecuteCompare())
                throw new InvalidOperationException();

            Differences = JsonComparer
                .Compare(LeftVM.First().Model, RightVM.First().Model)
                .Select(d => new DifferenceViewModel(d, _map[d.LeftNode].First(), _map[d.RightNode].First()))
                .ToList();
        }

        private bool CanExecuteCompare()
        {
            if (string.IsNullOrWhiteSpace(_leftSource))
                return false;
            if (string.IsNullOrWhiteSpace(_rightSource))
                return false;

            return true;
        }

        #endregion

        ICollection<DifferenceViewModel> _differences;
        public ICollection<DifferenceViewModel> Differences
        {
            get { return _differences; }
            set { SetProperty(ref _differences, value); }
        }

        DifferenceViewModel _currentDifference;
        public DifferenceViewModel CurrentDifference
        {
            get { return _currentDifference; }
            set
            {
                var oldDifference = _currentDifference;
                if (SetProperty(ref _currentDifference, value))
                {
                    if (oldDifference != null)
                        oldDifference.IsSelected = false;
                    if (_currentDifference != null)
                        _currentDifference.IsSelected = true;
                }
            }
        }

        private JsonObjectViewModel Transform(JsonObject root, JsonObjectViewModel parentVM, ref Dictionary<JsonObject, List<JsonObjectViewModel>> map)
        {
            if (root == null)
                throw new ArgumentNullException();

            var rootVM = new JsonObjectViewModel(root, parentVM);
            map.Add(root, new List<JsonObjectViewModel> { rootVM });

            foreach (var child in root.Fields)
            {
                rootVM.Fields.Add(Transform(child, rootVM, ref map));
            }

            return rootVM;
        }
    }
}
