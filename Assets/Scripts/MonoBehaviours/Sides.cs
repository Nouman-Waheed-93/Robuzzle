using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public enum SideName {left, right, front, back }

    [System.Serializable]
    public struct Sides
    {
        public bool left;
        public bool right;
        public bool front;
        public bool back;
    }
}
