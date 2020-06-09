using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour : MonoBehaviour
{
    #region Variablse
    public static SingletonMonoBehaviour singleton;
    #endregion
    #region UnityCallbacks
    protected void Awake()
    {
        if (singleton != null)
            Destroy(singleton.gameObject);
        singleton = this;
    }

    protected void OnDestroy()
    {
        singleton = null;
    }
    #endregion
}
