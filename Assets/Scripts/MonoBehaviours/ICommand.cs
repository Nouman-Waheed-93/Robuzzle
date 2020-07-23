using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public interface ICommand
    {
        void Execute();
        void Undue();
    }
}
