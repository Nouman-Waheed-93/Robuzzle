using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class SliderRail : MovableTile
    {
        #region Variables
        private Slider slider;
        #endregion
        #region Properties
        public Slider MySlider { get => slider; set => slider = value; }
        #endregion
    }
}