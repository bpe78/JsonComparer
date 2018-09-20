using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace JsonComparer.WPF.Controls
{
    public class JsonTreeView : Control
    {
        static JsonTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JsonTreeView), new FrameworkPropertyMetadata(typeof(JsonTreeView)));
        }

        #region Root

        public object Root
        {
            get { return GetValue(RootProperty); }
            set { SetValue(RootProperty, value); }
        }

        public static readonly DependencyProperty RootProperty =
            DependencyProperty.Register("Root", typeof(object), typeof(JsonTreeView), new PropertyMetadata(null));

        #endregion
    }
}
