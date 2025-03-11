using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animates the AI
/// </summary>
public class scp_AIAnimation : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Animator animator;

    [Header("Animation")]
    public bool asymetricalAnimations;

    [Header("SpriteStacking")]
    int bufferLayers;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Add to sprite stacker
        scp_SpriteStacker.Instance.AddSprite(sprite, bufferLayers, bufferLayers);
    }

    /// <summary>
    /// Animate the AI while it's still alive
    /// </summary>
    /// <param name="aiMovement">Movement from the previous frame</param>
    /// <param name="minMovement">Minimum movement required for the moving animation</param>
    public void AliveAnimation(Vector2 aiMovement, float minMovement)
    {

        if (aiMovement.magnitude > minMovement*Time.deltaTime)
        {
            AnimateMovement(aiMovement);
        }
        else
        {
            animator.SetBool("running", false);
        }
    }

    // Moving animation
    void AnimateMovement(Vector2 movement)
    {
        if (!asymetricalAnimations && movement.x < 0)
        {
            SetAnimatedDirection(movement.x, -1, true);
        }
        else
        {
            SetAnimatedDirection(movement.x, 1, false);
        }

        animator.SetFloat("movey", movement.y);
        animator.SetBool("running", true);

    }

    // Sets the direction faced during the animation
    void SetAnimatedDirection(float xMovement, int multiplier, bool flipX)
    {
        sprite.flipX = flipX;
        animator.SetFloat("movex", multiplier*xMovement);
    }

    /// <summary>
    /// Plays the AI death animation
    /// </summary>
    public void DeathAnimation()
    {
        animator.SetBool("running", false);
        animator.SetTrigger("die");
    }
}
