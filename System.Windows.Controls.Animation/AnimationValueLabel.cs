using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace System.Windows.Controls.Animation
{
    public class AnimationValueLabel  : Label
    {

        #region Private Members

        private bool startTracking = false;
        private double initialValue = 0.0f;

        #endregion

        #region Public Members

        protected override void OnMouseLeftButtonDown(Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            startTracking = Mouse.Capture(this);
            initialValue = e.GetPosition(this).X;
        }

        protected override void OnMouseMove(Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (startTracking)
            {                
                double diff = e.GetPosition(this).X - initialValue;
                initialValue = e.GetPosition(this).X;

                //Notify the curve needs to be redrawn...
                AnimationEditor editor = UIHelper.TryFindParent<AnimationEditor>(this);
                if (editor != null) {
                    editor.NotifyValueIncrement(diff);
                }   
            }
        }

        protected override void OnMouseLeftButtonUp(Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            startTracking = false;
            Mouse.Capture(this, CaptureMode.None);
            
        }

        #endregion

        #region Ctor / Dtor

        static AnimationValueLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationValueLabel), new FrameworkPropertyMetadata(typeof(AnimationValueLabel)));
        }

        #endregion
    }
}
