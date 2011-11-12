using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;

namespace System.Windows.Controls.Animation
{

    public enum CurveLoopType
    {
        Constant,
        Cycle,
        CycleOffset,
        Oscillate,
        Linear
    }

    public enum CurveTangent
    {
        Flat,
        Linear,
        Smooth
    }

    public enum CurveContinuity
    {
        Smooth,
        Step
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class CurveKey : IEquatable<CurveKey>, IComparable<CurveKey>
    {
        #region Internals

        internal CurveContinuity continuity;
        internal float internalValue;
        internal float position;
        internal float tangentIn;
        internal float tangentOut;

        #endregion

        #region Public Members

        public CurveKey Clone()
        {
            return new CurveKey(this.position, this.internalValue, this.tangentIn, this.tangentOut, this.continuity);
        }

        public int CompareTo(CurveKey other)
        {
            if (this.position == other.position)
            {
                return 0;
            }
            if (this.position >= other.position)
            {
                return 1;
            }
            return -1;
        }

        public bool Equals(CurveKey other)
        {
            return (((((other != null) && (other.position == this.position)) && ((other.internalValue == this.internalValue) && (other.tangentIn == this.tangentIn))) && (other.tangentOut == this.tangentOut)) && (other.continuity == this.continuity));
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as CurveKey);
        }

        public override int GetHashCode()
        {
            return ((((this.position.GetHashCode() + this.internalValue.GetHashCode()) + this.tangentIn.GetHashCode()) + this.tangentOut.GetHashCode()) + this.continuity.GetHashCode());
        }

        public static bool operator ==(CurveKey a, CurveKey b)
        {
            bool flag3 = null == (object)a;
            bool flag2 = null == (object)b;
            if (flag3 || flag2)
                return (flag3 == flag2);
            return a.Equals(b);
        }

        public static bool operator !=(CurveKey a, CurveKey b)
        {
            bool flag3 = null == (object)a;
            bool flag2 = null == (object)b;
            if (flag3 || flag2)
            {
                return (flag3 != flag2);
            }
            return ((((a.position != b.position) || (a.internalValue != b.internalValue)) || ((a.tangentIn != b.tangentIn) || (a.tangentOut != b.tangentOut))) || (a.continuity != b.continuity));
        }


        public CurveContinuity Continuity
        {
            get
            {
                return this.continuity;
            }
            set
            {
                this.continuity = value;
            }
        }

        public float Position
        {
            get
            {
                return this.position;
            }
        }

        public float TangentIn
        {
            get
            {
                return this.tangentIn;
            }
            set
            {
                this.tangentIn = value;
            }
        }

        public float TangentOut
        {
            get
            {
                return this.tangentOut;
            }
            set
            {
                this.tangentOut = value;
            }
        }

        public float Value
        {
            get
            {
                return this.internalValue;
            }
            set
            {
                this.internalValue = value;
            }
        }

        #endregion

        #region Ctor / Dtor

        public CurveKey(float position, float value)
        {
            this.position = position;
            this.internalValue = value;
        }

        public CurveKey(float position, float value, float tangentIn, float tangentOut)
        {
            this.position = position;
            this.internalValue = value;
            this.tangentIn = tangentIn;
            this.tangentOut = tangentOut;
        }

        public CurveKey(float position, float value, float tangentIn, float tangentOut, CurveContinuity continuity)
        {
            this.position = position;
            this.internalValue = value;
            this.tangentIn = tangentIn;
            this.tangentOut = tangentOut;
            this.continuity = continuity;
        }

        #endregion
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class CurveKeyCollection : ICollection<CurveKey>, IEnumerable<CurveKey>, IEnumerable
    {
        #region Internal Members

        internal float InvTimeRange;
        internal bool IsCacheAvailable = true;        
        internal float TimeRange;

        internal void ComputeCacheValues()
        {
            this.TimeRange = this.InvTimeRange = 0f;
            if (this.Keys.Count > 1)
            {
                this.TimeRange = this.Keys[this.Keys.Count - 1].Position - this.Keys[0].Position;
                if (this.TimeRange > float.Epsilon)
                {
                    this.InvTimeRange = 1f / this.TimeRange;
                }
            }
            this.IsCacheAvailable = true;
        }

        #endregion

        #region Private Members

        private List<CurveKey> Keys = new List<CurveKey>();

        #endregion

        #region Public Members

        public void Add(CurveKey item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            int index = this.Keys.BinarySearch(item);
            if (index >= 0)
            {
                while ((index < this.Keys.Count) && (item.Position == this.Keys[index].Position))
                {
                    index++;
                }
            }
            else
            {
                index = ~index;
            }
            this.Keys.Insert(index, item);
            this.IsCacheAvailable = false;
        }

        public void Clear()
        {
            this.Keys.Clear();
            this.TimeRange = this.InvTimeRange = 0f;
            this.IsCacheAvailable = false;
        }

        public CurveKeyCollection Clone()
        {
            return new CurveKeyCollection { Keys = new List<CurveKey>(this.Keys), InvTimeRange = this.InvTimeRange, TimeRange = this.TimeRange, IsCacheAvailable = true };
        }

        public bool Contains(CurveKey item)
        {
            return this.Keys.Contains(item);
        }

        public void CopyTo(CurveKey[] array, int arrayIndex)
        {
            this.Keys.CopyTo(array, arrayIndex);
            this.IsCacheAvailable = false;
        }

        public IEnumerator<CurveKey> GetEnumerator()
        {
            return this.Keys.GetEnumerator();
        }

        public int IndexOf(CurveKey item)
        {
            return this.Keys.IndexOf(item);
        }

        public bool Remove(CurveKey item)
        {
            this.IsCacheAvailable = false;
            return this.Keys.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.Keys.RemoveAt(index);
            this.IsCacheAvailable = false;
        }

        public int Count
        {
            get
            {
                return this.Keys.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public CurveKey this[int index]
        {
            get
            {
                return this.Keys[index];
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                if (this.Keys[index].Position == value.Position)
                {
                    this.Keys[index] = value;
                }
                else
                {
                    this.Keys.RemoveAt(index);
                    this.Add(value);
                }
            }
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Keys.GetEnumerator();
        }

        #endregion
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Curve
    {
        #region Private Members

        private CurveKeyCollection keys = new CurveKeyCollection();
        private CurveLoopType postLoop;
        private CurveLoopType preLoop;
        private float CalcCycle(float t)
        {
            float num = (t - this.keys[0].position) * this.keys.InvTimeRange;
            if (num < 0f)
            {
                num--;
            }
            int num2 = (int)num;
            return (float)num2;
        }
        private float FindSegment(float t, ref CurveKey k0, ref CurveKey k1)
        {
            float num2 = t;
            k0 = this.keys[0];
            for (int i = 1; i < this.keys.Count; i++)
            {
                k1 = this.keys[i];
                if (k1.position >= t)
                {
                    double position = k0.position;
                    double num6 = k1.position;
                    double num5 = t;
                    double num3 = num6 - position;
                    num2 = 0f;
                    if (num3 > 1E-10)
                    {
                        num2 = (float)((num5 - position) / num3);
                    }
                    return num2;
                }
                k0 = k1;
            }
            return num2;
        }
        private static float Hermite(CurveKey k0, CurveKey k1, float t)
        {
            if (k0.Continuity == CurveContinuity.Step)
            {
                if (t >= 1f)
                {
                    return k1.internalValue;
                }
                return k0.internalValue;
            }
            float num = t * t;
            float num2 = num * t;
            float internalValue = k0.internalValue;
            float num5 = k1.internalValue;
            float tangentOut = k0.tangentOut;
            float tangentIn = k1.tangentIn;
            return ((((internalValue * (((2f * num2) - (3f * num)) + 1f)) + (num5 * ((-2f * num2) + (3f * num)))) + (tangentOut * ((num2 - (2f * num)) + t))) + (tangentIn * (num2 - num)));
        }

        #endregion

        #region Public Members

        public Curve Clone()
        {
            return new Curve { preLoop = this.preLoop, postLoop = this.postLoop, keys = this.keys.Clone() };
        }

        public void ComputeTangent(int keyIndex, CurveTangent tangentType)
        {
            this.ComputeTangent(keyIndex, tangentType, tangentType);
        }

        public void ComputeTangent(int keyIndex, CurveTangent tangentInType, CurveTangent tangentOutType)
        {
            float num2;
            float num4;
            float num7;
            float num8;
            if ((this.keys.Count <= keyIndex) || (keyIndex < 0))
            {
                throw new ArgumentOutOfRangeException("keyIndex");
            }
            CurveKey key = this.Keys[keyIndex];
            float position = num8 = num4 = key.Position;
            float num = num7 = num2 = key.Value;
            if (keyIndex > 0)
            {
                position = this.Keys[keyIndex - 1].Position;
                num = this.Keys[keyIndex - 1].Value;
            }
            if ((keyIndex + 1) < this.keys.Count)
            {
                num4 = this.Keys[keyIndex + 1].Position;
                num2 = this.Keys[keyIndex + 1].Value;
            }
            if (tangentInType == CurveTangent.Smooth)
            {
                float num10 = num4 - position;
                float num6 = num2 - num;
                if (Math.Abs(num6) < 1.192093E-07f)
                {
                    key.TangentIn = 0f;
                }
                else
                {
                    key.TangentIn = (num6 * Math.Abs((float)(position - num8))) / num10;
                }
            }
            else if (tangentInType == CurveTangent.Linear)
            {
                key.TangentIn = num7 - num;
            }
            else
            {
                key.TangentIn = 0f;
            }
            if (tangentOutType == CurveTangent.Smooth)
            {
                float num9 = num4 - position;
                float num5 = num2 - num;
                if (Math.Abs(num5) < 1.192093E-07f)
                {
                    key.TangentOut = 0f;
                }
                else
                {
                    key.TangentOut = (num5 * Math.Abs((float)(num4 - num8))) / num9;
                }
            }
            else if (tangentOutType == CurveTangent.Linear)
            {
                key.TangentOut = num2 - num7;
            }
            else
            {
                key.TangentOut = 0f;
            }
        }

        public void ComputeTangents(CurveTangent tangentType)
        {
            this.ComputeTangents(tangentType, tangentType);
        }

        public void ComputeTangents(CurveTangent tangentInType, CurveTangent tangentOutType)
        {
            for (int i = 0; i < this.Keys.Count; i++)
            {
                this.ComputeTangent(i, tangentInType, tangentOutType);
            }
        }

        public float Evaluate(float position)
        {
            if (this.keys.Count == 0)
            {
                return 0f;
            }
            if (this.keys.Count == 1)
            {
                return this.keys[0].internalValue;
            }
            CurveKey key = this.keys[0];
            CurveKey key2 = this.keys[this.keys.Count - 1];
            float t = position;
            float num6 = 0f;
            if (t < key.position)
            {
                if (this.preLoop == CurveLoopType.Constant)
                {
                    return key.internalValue;
                }
                if (this.preLoop == CurveLoopType.Linear)
                {
                    return (key.internalValue - (key.tangentIn * (key.position - t)));
                }
                if (!this.keys.IsCacheAvailable)
                {
                    this.keys.ComputeCacheValues();
                }
                float num5 = this.CalcCycle(t);
                float num3 = t - (key.position + (num5 * this.keys.TimeRange));
                if (this.preLoop == CurveLoopType.Cycle)
                {
                    t = key.position + num3;
                }
                else if (this.preLoop == CurveLoopType.CycleOffset)
                {
                    t = key.position + num3;
                    num6 = (key2.internalValue - key.internalValue) * num5;
                }
                else
                {
                    t = ((((int)num5) & 1) != 0) ? (key2.position - num3) : (key.position + num3);
                }
            }
            else if (key2.position < t)
            {
                if (this.postLoop == CurveLoopType.Constant)
                {
                    return key2.internalValue;
                }
                if (this.postLoop == CurveLoopType.Linear)
                {
                    return (key2.internalValue - (key2.tangentOut * (key2.position - t)));
                }
                if (!this.keys.IsCacheAvailable)
                {
                    this.keys.ComputeCacheValues();
                }
                float num4 = this.CalcCycle(t);
                float num2 = t - (key.position + (num4 * this.keys.TimeRange));
                if (this.postLoop == CurveLoopType.Cycle)
                {
                    t = key.position + num2;
                }
                else if (this.postLoop == CurveLoopType.CycleOffset)
                {
                    t = key.position + num2;
                    num6 = (key2.internalValue - key.internalValue) * num4;
                }
                else
                {
                    t = ((((int)num4) & 1) != 0) ? (key2.position - num2) : (key.position + num2);
                }
            }
            CurveKey key4 = null;
            CurveKey key3 = null;
            t = this.FindSegment(t, ref key4, ref key3);
            return (num6 + Hermite(key4, key3, t));
        }

        public bool IsConstant
        {
            get
            {
                return (this.keys.Count <= 1);
            }
        }
        public CurveKeyCollection Keys
        {
            get
            {
                return this.keys;
            }
        }
        public CurveLoopType PostLoop
        {
            get
            {
                return this.postLoop;
            }
            set
            {
                this.postLoop = value;
            }
        }
        public CurveLoopType PreLoop
        {
            get
            {
                return this.preLoop;
            }
            set
            {
                this.preLoop = value;
            }
        }

        #endregion
    }


}
