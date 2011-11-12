using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace System.Windows.Controls.Animation
{
    public class AnimationEditor : ItemsControl
    {

        #region Private Members

      
   


        private System.Collections.ObjectModel.ObservableCollection<object> visibleItems = 
                            new System.Collections.ObjectModel.ObservableCollection<object>();

        private static void OnAnimationCurrentTimeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            double time = (o as AnimationEditor).AnimationCurrentTime;
            foreach (AnimationChannel tweenable in (o as AnimationEditor).Items)
                tweenable.Value = tweenable.curve1.Evaluate((float)time);
        }

        private static void OnAnimationDurationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AnimationEditor sender = o as AnimationEditor;
            sender.AnimationViewportOffsetWidth = sender.AnimationDuration / 100.0f * sender.AnimationViewportScale;
        }

        private static void OnAnimationScaleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AnimationEditor sender = o as AnimationEditor;
            sender.AnimationViewportOffsetWidth = sender.AnimationDuration / 100.0f * sender.AnimationViewportScale;
        }


        private void AnimationEditor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Visible")
            {
                if (IsVisible(sender))
                {
                    visibleItems.Add(sender);
                }
                else
                {
                    visibleItems.Remove(sender);
                }
            }
        }

        private void Part_Easing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid Part_AnimationChannels = Template.FindName("Part_AnimationChannels", this) as DataGrid;
            AnimationChannel item = Part_AnimationChannels.SelectedItem as AnimationChannel;
            item.curve1.ComputeTangents(item.Easying);
            item.curve2.ComputeTangents(item.Easying);
            NotifyCurveInvalidated(item);
        }

        private void Part_Continuity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedKeyframeIndex > -1 && SelectedItemIndex > -1)
            {
                DataGrid Part_AnimationChannels = Template.FindName("Part_AnimationChannels", this) as DataGrid;
                AnimationChannel item = Items[SelectedItemIndex] as AnimationChannel;
                item.curve1.Keys[SelectedKeyframeIndex].Continuity = (CurveContinuity)(sender as ComboBox).SelectedIndex;
                item.curve2.Keys[SelectedKeyframeIndex].Continuity = (CurveContinuity)(sender as ComboBox).SelectedIndex;
                NotifyCurveInvalidated(item);
            }
        }

        private void Part_AnimationChannels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                AnimationChannel tweenable = e.AddedItems[e.AddedItems.Count - 1] as AnimationChannel;
            }
        }

        private void RoutedCommand_SelectColor_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectColorDialog dialog = new SelectColorDialog();
            if (dialog.ShowDialog() == true)
            {
                AnimationChannel channel = (e.OriginalSource as FrameworkElement).DataContext as AnimationChannel;
                Reflection.PropertyInfo info = (Reflection.PropertyInfo)dialog.Part_ColorListBox.SelectedItem;
                channel.Color = (Media.Color)info.GetValue(null, new object[] { });
                NotifyCurveInvalidated(channel);
            }
        }

        #endregion

        #region Public Members

        public static readonly DependencyProperty AnimationViewportScaleProperty = DependencyProperty.Register("AnimationViewportScale",
                            typeof(double), typeof(AnimationEditor), new FrameworkPropertyMetadata((double)100.0f, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                            new PropertyChangedCallback(OnAnimationScaleChanged)));
        public static readonly DependencyProperty AnimationCurrentTimeProperty = DependencyProperty.Register("AnimationCurrentTime",
                            typeof(double), typeof(AnimationEditor), new FrameworkPropertyMetadata((double)0.0f, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                            new PropertyChangedCallback(OnAnimationCurrentTimeChanged)));
        public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register("AnimationDuration",
                            typeof(float), typeof(AnimationEditor), new FrameworkPropertyMetadata((float)0.0f, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                            new PropertyChangedCallback(OnAnimationDurationChanged)));
        public static readonly DependencyProperty AnimationViewportOffsetProperty = DependencyProperty.Register("AnimationViewportOffset",
                            typeof(double), typeof(AnimationEditor));
        public static readonly DependencyProperty AnimationViewportOffsetWidthProperty = DependencyProperty.Register("AnimationViewportOffsetWidth",
                            typeof(double), typeof(AnimationEditor), new FrameworkPropertyMetadata((double)2400.0f, FrameworkPropertyMetadataOptions.None, null));
        public static readonly DependencyProperty SelectedItemIndexProperty = DependencyProperty.Register("SelectedItemIndex",
                            typeof(int), typeof(AnimationEditor), 
                            new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsRender, null));
        public static readonly DependencyProperty SelectedKeyframeIndexProperty = DependencyProperty.Register("SelectedKeyframeIndex",
                            typeof(int), typeof(AnimationEditor),
                            new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsRender, null));

        public float AnimationDuration
        {
            get
            {
                return (float)this.GetValue(AnimationDurationProperty);
            }
            set
            {
                this.SetValue(AnimationDurationProperty, value);
            }
        }

        public double AnimationCurrentTime
        {
            get
            {
                return (double)this.GetValue(AnimationCurrentTimeProperty);
            }
            set
            {
                this.SetValue(AnimationCurrentTimeProperty, value);
            }
        }

        public double AnimationViewportOffset
        {
            get
            {
                return (double)this.GetValue(AnimationViewportOffsetProperty);
            }
            set
            {
                this.SetValue(AnimationViewportOffsetProperty, value);
            }
        }

        public double AnimationViewportOffsetWidth
        {
            get
            {
                return (double)this.GetValue(AnimationViewportOffsetWidthProperty);
            }
            set
            {
                this.SetValue(AnimationViewportOffsetWidthProperty, value);
            }
        }

        public double AnimationViewportScale
        {
            get
            {
                return (double)this.GetValue(AnimationViewportScaleProperty);
            }
            set
            {
                this.SetValue(AnimationViewportScaleProperty, value);
            }
        }

        public int SelectedItemIndex
        {
            get
            {
                return (int)this.GetValue(SelectedItemIndexProperty);
            }
            set
            {
                this.SetValue(SelectedItemIndexProperty, value);


                DataGrid Part_AnimationChannels = Template.FindName("Part_AnimationChannels", this) as DataGrid;
                Part_AnimationChannels.ApplyTemplate();
                ScrollViewer viewer = Part_AnimationChannels.Template.FindName("Part_ScrollViewer", Part_AnimationChannels) as ScrollViewer;
                viewer.ApplyTemplate();
                ComboBox Part_Continuity = viewer.Template.FindName("Part_Continuity", viewer) as ComboBox;

                Part_Continuity.SelectionChanged -= new SelectionChangedEventHandler(Part_Continuity_SelectionChanged);
                if (SelectedItemIndex == -1)
                {
                    Part_Continuity.SelectedIndex = -1;
                }
                else if (SelectedKeyframeIndex == -1)
                {
                    Part_Continuity.SelectedIndex = -1;
                }
                else if (SelectedKeyframeIndex >= (Items[SelectedItemIndex] as AnimationChannel).curve1.Keys.Count)
                {
                    Part_Continuity.SelectedIndex = -1;
                }
                else
                {
                    int si = SelectedItemIndex;
                    int ki = SelectedKeyframeIndex;
                    AnimationChannel tweenable = Items[si] as AnimationChannel;
                    Part_Continuity.SelectedIndex = (int)tweenable.curve1.Keys[ki].Continuity;
                }

                Part_Continuity.SelectionChanged += new SelectionChangedEventHandler(Part_Continuity_SelectionChanged);










            }
        }

        public int SelectedKeyframeIndex
        {
            get
            {
                return (int)this.GetValue(SelectedKeyframeIndexProperty);
            }
            set
            {
                this.SetValue(SelectedKeyframeIndexProperty, value);

                DataGrid Part_AnimationChannels = Template.FindName("Part_AnimationChannels", this) as DataGrid;
                Part_AnimationChannels.ApplyTemplate();
                ScrollViewer viewer = Part_AnimationChannels.Template.FindName("Part_ScrollViewer", Part_AnimationChannels) as ScrollViewer;
                viewer.ApplyTemplate();
                ComboBox Part_Continuity = viewer.Template.FindName("Part_Continuity", viewer) as ComboBox;

                Part_Continuity.SelectionChanged -= new SelectionChangedEventHandler(Part_Continuity_SelectionChanged);
                if (SelectedItemIndex == -1)
                {
                    Part_Continuity.SelectedIndex = -1;
                }
                else if (SelectedKeyframeIndex == -1)
                {
                    Part_Continuity.SelectedIndex = -1;
                }
                else if (SelectedKeyframeIndex >= (Items[SelectedItemIndex] as AnimationChannel).curve1.Keys.Count)
                {
                    Part_Continuity.SelectedIndex = -1;
                }
                else
                {
                    int si = SelectedItemIndex;
                    int ki = SelectedKeyframeIndex;
                    AnimationChannel tweenable = Items[si] as AnimationChannel;
                    Part_Continuity.SelectedIndex = (int)tweenable.curve1.Keys[ki].Continuity;
                }

                Part_Continuity.SelectionChanged += new SelectionChangedEventHandler(Part_Continuity_SelectionChanged);
            }
        }

        public static RoutedUICommand SelectColor
        {
            get;
            private set;
        }

        public IList<object> VisibleItems
        {
            get
            {
                return visibleItems;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            DataGrid Part_AnimationChannels = Template.FindName("Part_AnimationChannels", this) as DataGrid;
            if (Part_AnimationChannels == null) return;
            Part_AnimationChannels.ApplyTemplate();

            ScrollViewer viewer = Part_AnimationChannels.Template.FindName("Part_ScrollViewer", Part_AnimationChannels) as ScrollViewer;
            if (viewer == null) return;
            viewer.ApplyTemplate();

            ComboBox Part_Easing = viewer.Template.FindName("Part_Easing", viewer) as ComboBox;
            if (Part_Easing == null) return;
            Part_Easing.SelectionChanged += new SelectionChangedEventHandler(Part_Easing_SelectionChanged);

            ComboBox Part_Continuity = viewer.Template.FindName("Part_Continuity", viewer) as ComboBox;
            if (Part_Continuity == null) return;
            Part_Continuity.SelectionChanged += new SelectionChangedEventHandler(Part_Continuity_SelectionChanged);


            Part_AnimationChannels.SelectionChanged += new SelectionChangedEventHandler(Part_AnimationChannels_SelectionChanged);
        }

        public void NotifyCurveInvalidated(AnimationChannel Tweenable)
        {
            DataGrid Part_AnimationChannels = Template.FindName("Part_AnimationChannels", this) as DataGrid;
            ScrollViewer viewer = Part_AnimationChannels.Template.FindName("Part_ScrollViewer", Part_AnimationChannels) as ScrollViewer;
            AnimationCurvePreview splineEditor = viewer.Template.FindName("Part_Preview", viewer) as AnimationCurvePreview;
            splineEditor.InvalidateVisual();
            Tweenable.Value = Tweenable.curve1.Evaluate((float)AnimationCurrentTime);
        }

        public void NotifyValueIncrement(double value)
        {
            int selectedKey = -1;
            DataGrid Part_AnimationChannels = Template.FindName("Part_AnimationChannels", this) as DataGrid;
            AnimationChannel channel = Items[Part_AnimationChannels.SelectedIndex] as AnimationChannel;


            double pos = Single.PositiveInfinity; double offset = AnimationCurrentTime;
            for (int i = 0; i < channel.curve1.Keys.Count; i++)
            {
                double distance = Math.Abs(channel.curve1.Keys[i].Position - offset);
                if (distance < pos)
                {
                    pos = distance;
                    selectedKey = i;
                }
            }

            if (selectedKey > -1)
            {
                CurveKey keya = channel.curve1.Keys[selectedKey];
                channel.curve1.Keys.RemoveAt(selectedKey);
                channel.curve1.Keys.Add(new CurveKey(keya.Position, keya.Value + (float)value, keya.TangentIn, keya.TangentOut, keya.Continuity));
                channel.curve1.ComputeTangents(channel.Easying);
                NotifyCurveInvalidated(channel);
            }            
        }

        #endregion

        #region Protected Members


        protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            visibleItems.Clear();

            if (oldValue != null) foreach (object c in oldValue)
                {
                    if ((c is System.ComponentModel.INotifyPropertyChanged)) (c as System.ComponentModel.INotifyPropertyChanged).PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(AnimationEditor_PropertyChanged);
                    if (IsVisible(c))
                    {
                        visibleItems.Remove(c);
                    }
                }

            if (newValue != null) foreach (object c in newValue)
                {
                    if ((c is AnimationChannel)) (c as AnimationChannel).Value = (c as AnimationChannel).curve1.Evaluate((float)AnimationCurrentTime);
                    if ((c is System.ComponentModel.INotifyPropertyChanged)) (c as System.ComponentModel.INotifyPropertyChanged).PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(AnimationEditor_PropertyChanged);
                    if (IsVisible(c))
                    {
                        visibleItems.Add(c);
                    }
                }
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (e.OldItems != null) foreach (object c in e.OldItems)
                    if (IsVisible(c))
                        visibleItems.Remove(c);
            if (e.NewItems != null) foreach (object c in e.NewItems)
                {
                    if ((c is AnimationChannel)) (c as AnimationChannel).Value = (c as AnimationChannel).curve1.Evaluate((float)AnimationCurrentTime);
                    if (IsVisible(c))
                    {
                        visibleItems.Add(c);
                    }
                }
                    
        }

        protected bool IsVisible(object c)
        {
            AnimationChannel tween = c as AnimationChannel;
            return tween.Visible;
        }

        #endregion

        #region Ctor / Dtor 

        static AnimationEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationEditor), new FrameworkPropertyMetadata(typeof(AnimationEditor)));
            SelectColor = new RoutedUICommand("Select Color", "SelectColor", typeof(AnimationEditor));           
            
        }

        public AnimationEditor()
        {
            CommandBindings.Add( new CommandBinding(SelectColor, new ExecutedRoutedEventHandler(RoutedCommand_SelectColor_Executed)));
        }

        #endregion

    }
}
