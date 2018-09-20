using System;
using System.Windows;
using System.Windows.Controls;
using Models;

namespace JsonComparer.WPF.DataTemplates
{
    class DifferenceTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TemplateProperty { get; set; }
        public DataTemplate TemplateObject { get; set; }
        public DataTemplate TemplateArray { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Difference diff)
            {
                switch (diff.DifferenceType)
                {
                    case DifferenceTypes.Values: return TemplateProperty;
                    case DifferenceTypes.OnlyLeftHasProperty: return TemplateObject;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
