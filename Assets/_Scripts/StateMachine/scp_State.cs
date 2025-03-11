using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic state with Start and Update implemented
/// </summary>
public abstract class scp_State
{
    /// <summary>
    /// Called when the state machine enters this state
    /// </summary>
    /// 

    public virtual void OnStateEntered()
    {

    }

    // Start is called before the first frame update
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public abstract void Update();

}
