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
    public class AnimationKeyFrameRuler : Control
    {
        #region Private Members

        private bool istracing = false;
        private const int GeometrySize = 4, GeometryHitTestSize = 7;

        private static void OnKeyFramesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as AnimationKeyFrameRuler).InvalidateVisual();
        }



        #endregion

        #region Public Members


        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source",
            typeof(AnimationChannel), typeof(AnimationKeyFrameRuler), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnKeyFramesChanged)));
        public static readonly DependencyProperty SelectedItemIndexProperty = DependencyProperty.Register("SelectedItemIndex",
            typeof(int), typeof(AnimationKeyFrameRuler), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender, null));
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex",
            typeof(int), typeof(AnimationKeyFrameRuler), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender, null));
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register("Offset",
            typeof(float), typeof(AnimationKeyFrameRuler), new FrameworkPropertyMetadata(0.0f, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale",
            typeof(float), typeof(AnimationKeyFrameRuler), new FrameworkPropertyMetadata(100.0f, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public AnimationChannel Source
        {
            get
            {
                return (AnimationChannel)this.GetValue(SourceProperty);
            }
            set
            {
                this.SetValue(SourceProperty, value);
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
            }
        }

        public int SelectedIndex
        {
            get
            {
                return (int)this.GetValue(SelectedIndexProperty);
            }
            set
            {
                this.SetValue(SelectedIndexProperty, value);
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

        protected override void OnRender(DrawingContext drawingContext)
        {



            AnimationEditor grid = UIHelper.TryFindParent<AnimationEditor>(this);
            bool selected = grid.SelectedItemIndex == grid.Items.IndexOf(Source);
            float scale = Scale;
            
            



            StreamGeometry geometry = new StreamGeometry() { FillRule = FillRule.EvenOdd };
            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(new System.Windows.Point(0, -GeometrySize), true, true);
                ctx.LineTo(new System.Windows.Point(GeometrySize, 0), false, false);
                ctx.LineTo(new System.Windows.Point(0, GeometrySize), false, false);
                ctx.LineTo(new System.Windows.Point(-GeometrySize, 0), false, false);
                ctx.LineTo(new System.Windows.Point(0, -GeometrySize), false, false);
                ctx.Close();
            }


            drawingContext.DrawGeometry(Brushes.Transparent, null, 
                new RectangleGeometry(new Rect(0,0, ActualWidth, ActualHeight), 0,0));            
            if (Source != null)
            {
                int selIndex = SelectedIndex;
                drawingContext.PushClip(GetLayoutClip(new Size(ActualWidth, ActualHeight)));
                drawingContext.PushTransform(new TranslateTransform(-Offset, 0));
                for (int i = 0; i < Source.curve1.Keys.Count; i++)
                {
                    CurveKey key = Source.curve1.Keys[i];
                    drawingContext.PushTransform(new TranslateTransform(key.Position / 100.0f * scale, (ActualHeight / 2)));
                    drawingContext.DrawGeometry(i == grid.SelectedKeyframeIndex && selected ? Brushes.Orange : Brushes.Black, null, geometry);
                    drawingContext.Pop();
                }

                drawingContext.Pop();
                drawingContext.Pop();
            }


            


            








        }
       
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (Source != null)
            {
                int sIndex = -1;
                float d = Offset + (float)e.GetPosition(this).X * 100 / Scale;
                for (int i = 0; i < Source.curve1.Keys.Count; i++)
                {
                    CurveKey key = Source.curve1.Keys[i];
                    if (Math.Abs(key.Position - d) < GeometryHitTestSize)
                    {
                        sIndex = i; break;
                    }
                }

                SelectedIndex = sIndex;
                AnimationEditor grid = UIHelper.TryFindParent<AnimationEditor>(this);
                grid.SelectedItemIndex = grid.Items.IndexOf(Source);
                grid.SelectedKeyframeIndex = SelectedIndex;                
            }

            if (SelectedIndex > -1) {
                istracing = Mouse.Capture(this);
            }            
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);

            if (Source != null)
            {
                int sIndex = -1;
                float d = Offset + (float)e.GetPosition(this).X * 100 / Scale;
                for (int i = 0; i < Source.curve1.Keys.Count; i++)
                {
                    CurveKey key = Source.curve1.Keys[i];
                    if (Math.Abs(key.Position - d) < GeometryHitTestSize)
                    {
                        sIndex = i; break;
                    }
                }

                SelectedIndex = sIndex;
                AnimationEditor grid = UIHelper.TryFindParent<AnimationEditor>(this);
                grid.SelectedItemIndex = grid.Items.IndexOf(Source);
                grid.SelectedKeyframeIndex = SelectedIndex;
            }

            if (SelectedIndex == -1)
            {
                if (Source != null)
                {
                    float d = Offset + (float)e.GetPosition(this).X * 100 / Scale;
                    Source.curve1.Keys.Add(new CurveKey(d, Source.curve1.Evaluate(d), 0, 0, CurveContinuity.Smooth));
                    Source.curve2.Keys.Add(new CurveKey(d, d, 0, 0, CurveContinuity.Smooth));
                    Source.curve1.ComputeTangents(Source.Easying);
                    Source.curve2.ComputeTangents(Source.Easying);
                    InvalidateVisual();

                    //Notify the curve needs to be redrawn...
                    AnimationEditor editor = UIHelper.TryFindParent<AnimationEditor>(this);
                    if (editor != null)
                    {
                        editor.NotifyCurveInvalidated(Source);
                    }
                }
            }
            else
            {
                if (Source != null)
                {
                    Source.curve1.Keys.RemoveAt(SelectedIndex);
                    Source.curve2.Keys.RemoveAt(SelectedIndex);
                    Source.curve1.ComputeTangents(Source.Easying);
                    Source.curve2.ComputeTangents(Source.Easying);
                    InvalidateVisual();
                    SelectedIndex = -1;

                    //Notify the curve needs to be redrawn...
                    AnimationEditor editor = UIHelper.TryFindParent<AnimationEditor>(this);
                    if (editor != null)
                    {
                        editor.NotifyCurveInvalidated(Source);
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (istracing == true) {
                double pos = Offset + (float)e.GetPosition(this).X * 100 / Scale;
                if (pos < 0) {
                    pos = 0;
                }


                if (SelectedIndex > 0)
                {
                    if (pos <= Source.curve1.Keys[SelectedIndex - 1].Position)
                        return;
                }

                if (SelectedIndex < Source.curve1.Keys.Count - 1)
                {
                    if (pos >= Source.curve1.Keys[SelectedIndex + 1].Position)
                        return;
                }

     

                CurveKey keya = Source.curve1.Keys[SelectedIndex];
                CurveKey keyb = Source.curve2.Keys[SelectedIndex];                
                Source.curve1.Keys.RemoveAt( SelectedIndex );
                Source.curve2.Keys.RemoveAt(SelectedIndex);
                Source.curve1.Keys.Add(new CurveKey((float)pos, keya.Value, keya.TangentIn, keya.TangentOut, keya.Continuity));
                Source.curve2.Keys.Add(new CurveKey((float)pos, (float)pos, keyb.TangentIn, keyb.TangentOut, keyb.Continuity));
                Source.curve1.ComputeTangents(Source.Easying);
                Source.curve2.ComputeTangents(Source.Easying);
                InvalidateVisual();


                //Notify the curve needs to be redrawn...
                AnimationEditor editor = UIHelper.TryFindParent<AnimationEditor>(this);
                if (editor != null) {
                    editor.NotifyCurveInvalidated(Source);
                }               
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            istracing = false;
            Mouse.Capture(this, CaptureMode.None);
            base.OnMouseLeftButtonUp(e);
        }

        #endregion

        #region Ctor / Dtor

        static AnimationKeyFrameRuler()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationKeyFrameRuler), new FrameworkPropertyMetadata(typeof(AnimationKeyFrameRuler)));
        }

        #endregion
    }






}
