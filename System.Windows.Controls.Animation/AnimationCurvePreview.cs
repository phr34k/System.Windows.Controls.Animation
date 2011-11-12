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
    public class AnimationCurvePreview : ItemsControl
    {

        #region Public Members

        public static readonly DependencyProperty MinimumRangeTimeProperty = DependencyProperty.Register("MinimumRangeTime",
            typeof(float), typeof(AnimationCurvePreview));
        public static readonly DependencyProperty MaximumRangeTimeProperty = DependencyProperty.Register("MaximumRangeTime",
            typeof(float), typeof(AnimationCurvePreview));
        public static readonly DependencyProperty MinimumRangeValueProperty = DependencyProperty.Register("MinimumRangeValue",
            typeof(float), typeof(AnimationCurvePreview));
        public static readonly DependencyProperty MaximumRangeValueProperty = DependencyProperty.Register("MaximumRangeValue",
            typeof(float), typeof(AnimationCurvePreview));
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register("Offset",
            typeof(float), typeof(AnimationCurvePreview), new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale",
            typeof(float), typeof(AnimationCurvePreview), new FrameworkPropertyMetadata(100.0f, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public float MinimumRangeTime
        {
            get
            {
                return (float)this.GetValue(MinimumRangeTimeProperty);
            }
            set
            {
                this.SetValue(MinimumRangeTimeProperty, value);
            }
        }
        public float MaximumRangeTime
        {
            get
            {
                return (float)this.GetValue(MaximumRangeTimeProperty);
            }
            set
            {
                this.SetValue(MaximumRangeTimeProperty, value);
            }
        }
        public float MinimumRangeValue
        {
            get
            {
                return (float)this.GetValue(MinimumRangeValueProperty);
            }
            set
            {
                this.SetValue(MinimumRangeValueProperty, value);
            }
        }
        public float MaximumRangeValue
        {
            get
            {
                return (float)this.GetValue(MaximumRangeValueProperty);
            }
            set
            {
                this.SetValue(MaximumRangeValueProperty, value);
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

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            InvalidateVisual();
            base.OnItemsChanged(e);
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);



            
            List<Geometry> animationCurves = new List<Geometry>();
            foreach (System.Windows.Controls.Animation.AnimationChannel animation in Items)
            {
                List<System.Windows.Point> points = new List<System.Windows.Point>();
                StreamGeometry geometry = new StreamGeometry() { FillRule = FillRule.Nonzero };
                using (StreamGeometryContext ctx = geometry.Open())
                {
                    ctx.BeginFigure(new System.Windows.Point(animation.curve1.Keys[0].Position, animation.curve1.Keys[0].Value), false, false);
                    for (int i = 0; i < animation.curve1.Keys.Count - 1; i++)
                    {
                        CurveKey k0 = animation.curve1.Keys[i + 0];
                        CurveKey k1 = animation.curve1.Keys[i + 1];
                        CurveKey k2 = animation.curve2.Keys[i + 0];
                        CurveKey k3 = animation.curve2.Keys[i + 1];
                        if (k0.Continuity == CurveContinuity.Step)
                        {
                            ctx.LineTo(new System.Windows.Point(k1.Position, k0.Value), true, false);
                            ctx.LineTo(new System.Windows.Point(k1.Position, k1.Value), true, false);
                            continue;
                        }


                        float B0 = animation.curve1.Keys[i + 0].Value;
                        float B1 = animation.curve1.Keys[i + 0].Value + k0.TangentOut / 3.0f;
                        float B2 = animation.curve1.Keys[i + 1].Value - k1.TangentIn / 3.0f;
                        float B3 = animation.curve1.Keys[i + 1].Value;
                        float P0 = animation.curve2.Keys[i + 0].Value;
                        float P1 = animation.curve2.Keys[i + 0].Value + k2.TangentOut / 3.0f;
                        float P2 = animation.curve2.Keys[i + 1].Value - k3.TangentIn / 3.0f;
                        float P3 = animation.curve2.Keys[i + 1].Value;
                        points.Clear();
                        points.Add(new System.Windows.Point(P1, B1));
                        points.Add(new System.Windows.Point(P2, B2));
                        points.Add(new System.Windows.Point(P3, B3));
                        ctx.PolyBezierTo(points, true, false);
                    }

                    ctx.Close();
                }

                animationCurves.Add(geometry);
            }


            {
                float scale = Scale;
                Pen blackPen = new Pen(), blackPen2 = new Pen();
                blackPen2.Thickness = blackPen.Thickness = 1.3;
                blackPen2.LineJoin = blackPen.LineJoin = PenLineJoin.Bevel;
                blackPen2.StartLineCap = blackPen.StartLineCap = PenLineCap.Triangle;
                blackPen2.EndLineCap = blackPen.EndLineCap = PenLineCap.Round;
                blackPen2.Brush = blackPen.Brush = Brushes.White;
                drawingContext.DrawLine(blackPen, new System.Windows.Point(0, ActualHeight / 2), new System.Windows.Point(ActualWidth, ActualHeight / 2));


                int columScale = 1;
                if (scale < 50)
                {
                    scale *= 2;
                    columScale *= 2;
                }




                {
                    int columnOffset = (int)Math.Ceiling(Offset / scale);
                    int lines = (int)Math.Ceiling(ActualWidth / scale);
                    blackPen2.Thickness = 0.7;


                    drawingContext.PushClip(GetLayoutClip(new Size(ActualWidth, ActualHeight)));
                    drawingContext.PushTransform(new TranslateTransform(-Offset, 0));
                    float startx = columnOffset * scale; int column = columnOffset * 2;
                    for (int i = columnOffset, max = columnOffset + lines; i < max; i++)
                    {
                        drawingContext.DrawLine(blackPen2,
                            new System.Windows.Point(startx, 0),
                            new System.Windows.Point(startx, ActualHeight));
                        startx += scale;
                        column += columScale;
                    }

                    drawingContext.Pop();
                    drawingContext.Pop();
                }


                {
                    int columnOffset = (int)Math.Ceiling(-(ActualHeight / 2) / scale);
                    int lines = (int)Math.Ceiling(ActualHeight / scale);
                    blackPen2.Thickness = 0.7;


                    drawingContext.PushClip(GetLayoutClip(new Size(ActualWidth, ActualHeight)));
                    drawingContext.PushTransform(new TranslateTransform(0, ActualHeight / 2));
                    float startx = columnOffset * scale; int column = columnOffset * 2;
                    for (int i = columnOffset, max = columnOffset + lines; i < max; i++)
                    {
                        drawingContext.DrawLine(blackPen2,
                            new System.Windows.Point(0, startx),
                            new System.Windows.Point(ActualWidth, startx));
                        startx += scale;
                        column += columScale;
                    }

                    drawingContext.Pop();
                    drawingContext.Pop();
                }
            }



            {

                float scale = Scale;
                StreamGeometry geometry2 = new StreamGeometry() { FillRule = FillRule.Nonzero };
                using (StreamGeometryContext ctx = geometry2.Open())
                {
                    ctx.BeginFigure(new System.Windows.Point(0, 0), true, true);
                    ctx.LineTo(new System.Windows.Point(ActualWidth, 0), true, true);
                    ctx.LineTo(new System.Windows.Point(ActualWidth, ActualHeight), true, true);
                    ctx.LineTo(new System.Windows.Point(0, ActualHeight), true, true);
                    ctx.Close();
                }


                float min = MinimumRangeTime, max = MaximumRangeTime;
                System.Windows.Point startPoint = new System.Windows.Point(Offset, 0);
                drawingContext.PushClip(geometry2);
                drawingContext.PushTransform(new TranslateTransform(-startPoint.X, -startPoint.Y));
                drawingContext.PushTransform(new ScaleTransform(1 / 100.0f * scale, 1));

                for (int i = 0; i < animationCurves.Count; i++)
                {
                    Pen blackPen = new Pen();
                    blackPen.Thickness = 0.7;
                    blackPen.LineJoin = PenLineJoin.Bevel;
                    blackPen.StartLineCap = PenLineCap.Triangle;
                    blackPen.EndLineCap = PenLineCap.Round;
                    blackPen.Brush = new SolidColorBrush((Items[i] as AnimationChannel).Color);
                    drawingContext.DrawGeometry(null, blackPen, animationCurves[i]);
                }

                drawingContext.Pop();
                drawingContext.Pop();
                drawingContext.Pop();
            }





          


        }

        #endregion

        #region Ctor / Dtor

        static AnimationCurvePreview()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationCurvePreview), new FrameworkPropertyMetadata(typeof(AnimationCurvePreview)));
        }

        public AnimationCurvePreview()
        {
            MinimumRangeTime = 10.0f;
            MaximumRangeTime = 420.0f;
        }

        #endregion

    }
}
