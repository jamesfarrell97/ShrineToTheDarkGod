﻿using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    /*
     * Allow the user to pass in offsets for a 2D curve so that platforms can use the same curve but
     * operate "out of sync" by the offsets specified. 
     */
    public class Transform2DCurveOffsets : ICloneable
    {
        public static Transform2DCurveOffsets Zero = new Transform2DCurveOffsets(Vector2.Zero, Vector2.One, 0, 0);

        #region Fields
        private Vector2 translation, scale;
        private float rotation;
        private float timeInSecs;
        #endregion

        #region Properties
        public Vector2 Translation
        {
            get
            {
                return this.translation;
            }
            set
            {
                this.translation = value;
            }
        }
        public Vector2 Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
            }
        }
        public float Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                this.rotation = value;
            }
        }
        public float TimeInSecs
        {
            get
            {
                return this.timeInSecs;
            }
            set
            {
                this.timeInSecs = value;
            }
        }
        public float TimeInMs
        {
            get
            {
                return this.timeInSecs * 1000;
            }
        }
        #endregion

        public Transform2DCurveOffsets(Vector2 translation, Vector2 scale, float rotation, float timeInSecs)
        {
            this.translation = translation;
            this.scale = scale;
            this.rotation = rotation;
            this.timeInSecs = timeInSecs;
        }
        public object Clone()
        {
            return this.MemberwiseClone(); //simple C# or XNA types so use MemberwiseClone()
        }
    }

    //Represents a 2D point on a curve (i.e. position, rotation, and scale) at a specified time in seconds
    public class Transform2DCurve
    {
        #region Fields
        private Curve1D rotationCurve;
        private Curve2D translationCurve, scaleCurve;
        #endregion

        public Transform2DCurve(CurveLoopType curveLoopType)
        {
            this.translationCurve = new Curve2D(curveLoopType);
            this.scaleCurve = new Curve2D(curveLoopType);
            this.rotationCurve = new Curve1D(curveLoopType);
        }
        public void Add(Vector2 translation, Vector2 scale, float rotation, float timeInSecs)
        {
            this.translationCurve.Add(translation, timeInSecs);
            this.scaleCurve.Add(scale, timeInSecs);
            this.rotationCurve.Add(rotation, timeInSecs);
        }
        public void Clear()
        {
            this.translationCurve.Clear();
            this.scaleCurve.Clear();
            this.rotationCurve.Clear();
        }

        //See https://msdn.microsoft.com/en-us/library/t3c3bfhx.aspx for information on using the out keyword
        public void Evalulate(float timeInMS, int precision, out Vector2 translation, out Vector2 scale, out float rotation)
        {
            translation = this.translationCurve.Evaluate(timeInMS, precision);
            scale = this.scaleCurve.Evaluate(timeInMS, precision);
            rotation = this.rotationCurve.Evaluate(timeInMS, precision);
        }

        //Add Equals, Clone, ToString, GetHashCode...
    }
}
