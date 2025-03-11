using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Pathfinds to the mouse position
/// </summary>
public class scp_MenuLizard : MonoBehaviour
{
    // Components
    NavMeshAgent agent;
    Transform myTransform;
    Animator myAnimator;

    Vector3 myLastKnownPos;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise variables
        agent = GetComponent<NavMeshAgent>();
        myTransform = GetComponent<Transform>();
        myAnimator = GetComponent<Animator>();

        myLastKnownPos = myTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Follow mouse position
        myTransform.eulerAngles = new Vector3(0, 0, 0);
        myTransform.position = new Vector2(myTransform.position.x, myTransform.position.y);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        agent.SetDestination(new Vector2(mousePosition.x, mousePosition.y));

        // Animation
        Vector3 movement = transform.position - myLastKnownPos;
        if (movement.magnitude > Time.deltaTime*3)
        {
            myAnimator.SetFloat("movey", movement.y);
            myAnimator.SetBool("running", true);

            if (movement.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                myAnimator.SetFloat("movex", -movement.x);
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
                myAnimator.SetFloat("movex", movement.x);
            }
        }
        else
        {
            myAnimator.SetBool("running", false);
        }
        

        myLastKnownPos = myTransform.position;
    }
}
