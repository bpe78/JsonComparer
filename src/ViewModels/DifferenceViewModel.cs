using System;
using Models;
using Prism.Mvvm;

namespace ViewModels
{
    public sealed class DifferenceViewModel : BindableBase
    {
        readonly JsonObjectViewModel _leftVM;
        readonly JsonObjectViewModel _rightVM;

        public DifferenceViewModel(Difference model, JsonObjectViewModel leftVM, JsonObjectViewModel rightVM)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            _leftVM = leftVM ?? throw new ArgumentNullException(nameof(leftVM));
            _rightVM = rightVM ?? throw new ArgumentNullException(nameof(rightVM));

            _isSelected = false;
        }

        public Difference Model { get; }

        bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (SetProperty(ref _isSelected, value))
                {
                    _leftVM.IsSelected = _isSelected;
                    _rightVM.IsSelected = _isSelected;
                }
            }
        }
    }
}
