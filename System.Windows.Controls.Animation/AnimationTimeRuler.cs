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
    public class AnimationTimeRuler : Control
    {

        #region Public Members

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register("Offset",
            typeof(float), typeof(AnimationTimeRuler), new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale",
            typeof(float), typeof(AnimationTimeRuler), new FrameworkPropertyMetadata(100.0f, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            Point pos = e.GetPosition(this);
            if (pos.Y > (ActualHeight / 2.0))
            {

                //Notify the curve needs to be redrawn...
                AnimationEditor editor = UIHelper.TryFindParent<AnimationEditor>(this);
                if (editor != null)
                {
                    float d = Offset + (float)e.GetPosition(this).X * 100 / Scale;
                    editor.AnimationCurrentTime = d;
                } 
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {




            float scale = Scale;
            int columScale = 1;

            if (scale < 50)
            {
                scale *= 2;
                columScale *= 2;
            }


            int columnOffset = (int)Math.Ceiling(Offset / scale);
            int lines = (int)Math.Ceiling(ActualWidth / scale);
            Typeface      face = new Typeface("Times New Roman");
   


            Pen blackPen = new Pen();
            blackPen.Thickness = 1;
            blackPen.LineJoin = PenLineJoin.Bevel;
            blackPen.StartLineCap = PenLineCap.Triangle;
            blackPen.EndLineCap = PenLineCap.Round;
            blackPen.Brush = Brushes.Black;



            System.Windows.Point startPoint = new System.Windows.Point(Offset, 0);
            drawingContext.PushClip( GetLayoutClip( new Size(ActualWidth, ActualHeight)));
            drawingContext.PushTransform(new TranslateTransform(-startPoint.X, -startPoint.Y));


            float startx = columnOffset * scale; int column = columnOffset * columScale;
            for (int i = columnOffset, max = columnOffset + lines; i < max; i++)
            {
                FormattedText text = new FormattedText(
                    string.Format("{0:###00}:{1:00}f", (float)column, 0), 
                    System.Globalization.CultureInfo.CurrentUICulture,
                   FlowDirection.LeftToRight, face, 12, Foreground);
                drawingContext.DrawLine(blackPen, 
                    new Point(startx, (ActualHeight / 2) - 7), 
                    new Point(startx, (ActualHeight / 2) + 7));
                drawingContext.DrawText(text, 
                    new Point(-(text.Width / 2) + startx, 0));
                startx += scale;
                column += columScale;


            }
            
            drawingContext.Pop();
            drawingContext.Pop();
            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(0, ActualHeight / 2.0f, ActualWidth, ActualHeight / 2.0f));


            


            base.OnRender(drawingContext);

        }

        #endregion

        #region Ctor / Dtor

        static AnimationTimeRuler()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationTimeRuler), new FrameworkPropertyMetadata(typeof(AnimationTimeRuler)));
        }

        #endregion

    }
}
