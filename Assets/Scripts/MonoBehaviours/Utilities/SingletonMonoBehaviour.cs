using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Variablse
    public static T singleton;
    #endregion
    #region UnityCallbacks
    protected void Awake()
    {
        if (singleton != null)
            Destroy(singleton.gameObject);
        singleton = this as T;
    }

    protected void OnDestroy()
    {
        singleton = null;
    }
    #endregion
}
