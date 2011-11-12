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
    public class AnimationTimeIndicator : Control
    {
        #region Private Members

        private bool istracing = false;

        #endregion

        #region Public Members

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum",
             typeof(float), typeof(AnimationTimeIndicator), new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum",
             typeof(float), typeof(AnimationTimeIndicator), new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register("Offset",
             typeof(float), typeof(AnimationTimeIndicator), new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
             typeof(float), typeof(AnimationTimeIndicator), new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale",
            typeof(float), typeof(AnimationTimeIndicator), new FrameworkPropertyMetadata(100.0f, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public float Minimum
        {
            get
            {
                return (float)this.GetValue(MinimumProperty);
            }
            set
            {
                this.SetValue(MinimumProperty, value);
            }
        }

        public float Maximum
        {
            get
            {
                return (float)this.GetValue(MaximumProperty);
            }
            set
            {
                this.SetValue(MaximumProperty, value);
            }
        }

        public float Offset
        {
            get
            {
                return (float)this.GetValue(OffsetProperty);
            }
            set
            {
                this.SetValue(OffsetProperty, value);
            }
        }

        public float Value
        {
            get
            {
                return (float)this.GetValue(ValueProperty);
            }
            set
            {
                this.SetValue(ValueProperty, value);
            }
        }

        public float Scale
        {
            get
            {
                return (float)this.GetValue(ScaleProperty);
            }
            set
            {
                this.SetValue(ScaleProperty, value);
            }
        }


        #endregion

        #region Protected Members

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            istracing = Mouse.Capture(this);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            istracing = false;
            Mouse.Capture(this, CaptureMode.None);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {           
            base.OnMouseMove(e);

            if (istracing)
            {
                Value = (int) ( (Offset + e.GetPosition(this).X) * 100 / Scale );
                if (Value < Minimum) Value = Minimum;
                if (Value > Maximum) Value = Maximum;

                InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Pen blackPen = new Pen();
            blackPen.Thickness = 0.2;
            blackPen.LineJoin = PenLineJoin.Bevel;
            blackPen.StartLineCap = PenLineCap.Triangle;
            blackPen.EndLineCap = PenLineCap.Round;
            blackPen.Brush = Brushes.Red;

            Pen blackPen2 = new Pen();
            blackPen2.Thickness = 1;
            blackPen2.LineJoin = PenLineJoin.Bevel;
            blackPen2.StartLineCap = PenLineCap.Triangle;
            blackPen2.EndLineCap = PenLineCap.Round;
            blackPen2.Brush = Brushes.Transparent;

            float p = -Offset + (Value) / 100.0f * Scale;
            drawingContext.DrawLine(blackPen2, new Point(p - 1, 0), new Point(p - 1, ActualHeight));
            drawingContext.DrawLine(blackPen, new Point(p, 0), new Point(p, ActualHeight));
            drawingContext.DrawLine(blackPen2, new Point(p + 1, 0), new Point(p + 1, ActualHeight));
        }

        #endregion

        #region Ctor / Dtor

        static AnimationTimeIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationTimeIndicator), new FrameworkPropertyMetadata(typeof(AnimationTimeIndicator)));
        }

        public AnimationTimeIndicator()
        {
            Cursor = Cursors.SizeWE;
        }

        #endregion
    }
}
