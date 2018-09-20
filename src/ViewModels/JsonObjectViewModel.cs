using System;
using System.Collections.Generic;
using Models;
using Prism.Mvvm;

namespace ViewModels
{
    public sealed class JsonObjectViewModel : BindableBase
    {
        public JsonObjectViewModel(JsonObject model, JsonObjectViewModel parentVM)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            ParentVM = parentVM;

            _isExpanded = false;
            _isSelected = false;
        }

        public JsonObject Model { get; }
        public JsonObjectViewModel ParentVM { get; }

        public string Text
        {
            get
            {
                if (Model.JsonType == JsonTypes.Value)
                {
                    string val = (Model.Value == null ? "<null>" : Model.Value.ToString());
                    if (Model.Value.Type == Newtonsoft.Json.Linq.JTokenType.String)
                        val = $"\"{val}\"";

                    return $"{Model.Id} : {val}";
                }
                else
                    return Model.Id;
            }
        }

        public List<JsonObjectViewModel> Fields { get; } = new List<JsonObjectViewModel>();

        bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (SetProperty(ref _isExpanded, value))
                {
                    if (_isExpanded && (ParentVM != null))
                    {
                        ParentVM.IsExpanded = true;
                    }
                }
            }
        }

        bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (SetProperty(ref _isSelected, value))
                {
                    if (_isSelected)
                        IsExpanded = true;
                }
            }
        }
    }
}
