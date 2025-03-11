using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the lizard in the end cutscene face the right direction
/// </summary>
public class scp_EndLizard : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("movex", 1);
    }
}
