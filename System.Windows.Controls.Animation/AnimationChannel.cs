using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace System.Windows.Controls.Animation
{
    public class AnimationChannel : INotifyPropertyChanged
    {
        #region Private Members

        private bool visible = true;
        private CurveTangent easying = CurveTangent.Smooth;
        private float value;
        private System.Windows.Media.Color color;

        #endregion

        #region Public Members

        public Curve curve1 = null;
        public Curve curve2 = null;  

        public string Department
        { get; set; }

        public string Name
        { get; set; }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                if (visible != value)
                {
                    visible = value;
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Visible"));
                }
            }
        }
        
        public CurveTangent Easying
        {
            get
            {
                return easying;
            }
            set
            {
                if (easying != value)
                {
                    easying = value;
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Easying"));
                }
            }
        }
                                           
        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }

        public System.Windows.Media.Color Color
        {
            get
            {
                return color;
            }
            set
            {
                if (this.color != value)
                {
                    this.color = value;
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Color"));
                }
            }
        }


        #endregion
      
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region INotifyPropertyChanged Members

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }

        #endregion

        #region Ctor/Dtor

        public AnimationChannel()
        {

            //curve1 = new Curve();
            //curve2 = new Curve();
        }

        #endregion
    }
}
