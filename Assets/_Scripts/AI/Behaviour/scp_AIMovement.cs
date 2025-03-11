using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handles AI movement and pathfinding
/// </summary>
public class scp_AIMovement : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 destination;

    [HideInInspector] public float myRotation;
    [HideInInspector] public Vector3 previousPosition;
    [HideInInspector] public Vector3 previousMovement;

    // Start is called before the first frame update
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        previousPosition = transform.position;
    }

    // LateUpdate is called after update
    private void LateUpdate()
    {
        previousMovement = transform.position - previousPosition;
        myRotation = Mathf.Atan2(previousMovement.y, previousMovement.x)*180/Mathf.PI;
        previousPosition = transform.position;
    }

    /// <summary>
    /// Sets a new destination
    /// </summary>
    /// <param name="position">New destination</param>
    public void SetNewDestination(Vector3 position)
    {
        destination = position;
        agent.SetDestination(position);
    }

    /// <summary>
    /// Returns if the AI has reached it's destination as a bool
    /// </summary>
    /// <returns>True if at destination</returns>
    public bool AtDestination()
    {
        Vector2 distance = transform.position - destination;

        if (distance.magnitude <= 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Makes a small movement in a direction to turn the AI
    /// </summary>
    /// <param name="direction">The intended direction</param>
    public void LookInDirection(Vector3 direction)
    {
        SetNewDestination(transform.position + direction*0.2f);
    }

    /// <summary>
    /// Gets the current destination
    /// </summary>
    /// <returns>Current destination</returns>
    public Vector3 GetDestination()
    {
        return destination;
    }

    /// <summary>
    /// Disables the pathfinding agent
    /// </summary>
    public void DisableMovement()
    {
        agent.enabled = false;
    }
}
