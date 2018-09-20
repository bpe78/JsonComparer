using System;
using System.Windows;
using System.Windows.Controls;
using Models;
using ViewModels;

namespace JsonComparer.WPF.DataTemplates
{
    sealed class JsonObjectTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PropertyTemplate { get; set; }
        public DataTemplate ObjectTemplate { get; set; }
        public DataTemplate ArrayTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is JsonObjectViewModel json)
            {
                switch (json.Model.JsonType)
                {
                    case JsonTypes.Value: return PropertyTemplate;
                    case JsonTypes.Object: return ObjectTemplate;
                    case JsonTypes.Array: return ArrayTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
