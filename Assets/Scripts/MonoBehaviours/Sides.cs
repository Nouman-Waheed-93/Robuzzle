using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public enum SideName {left, right, front, back, up, down }

    [System.Serializable]
    public struct Sides
    {
        public bool left;
        public bool right;
        public bool front;
        public bool back;
        public bool up;
        public bool down;
    }
}
