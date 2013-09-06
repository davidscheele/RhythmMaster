using System; 
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

    public class BeatTimerData
    {
        public BeatTimerData(int _timestamp, Vector2 _startPosition, Vector2 _endPosition, Boolean _isSlider, Boolean _isSpinner)
        {
            this.Timestamp = _timestamp;
            this.StartPosition = _startPosition;
            this.EndPosition = _endPosition;
            this.IsSlider = _isSlider;
            this.IsSpinner = _isSpinner;
        }

        private int timestamp;
        public int Timestamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                timestamp = value;
            }
        }
        private Vector2 startPosition;
        public Vector2 StartPosition
                {
            get
            {
                return startPosition;
            }
            set
            {
                startPosition = value;
            }
        }
        private Vector2 endPosition;
        public Vector2 EndPosition
        {
            get
            {
                return endPosition;
            }
            set
            {
                endPosition = value;
            }
        }
        private Boolean isSlider;
        public Boolean IsSlider
        {
            get
            {
                return isSlider;
            }
            set
            {
                isSlider = value;
            }
        }
        private Boolean isSpinner;
        public Boolean IsSpinner
        {
            get
            {
                return isSpinner;
            }
            set
            {
                isSpinner = value;
            }
        }
        }

    
