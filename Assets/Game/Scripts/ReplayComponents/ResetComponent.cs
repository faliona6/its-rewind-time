using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResetComponent : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        LoopReset.OnResetCalls += OnReset;
    }

    protected virtual void OnDestroy()
    {
        LoopReset.OnResetCalls -= OnReset;
    }

    public abstract void OnReset();
}
