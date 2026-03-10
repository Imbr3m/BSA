using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberFollowAI : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private int speed;
    [SerializeField] private float minDistance = 1.5f;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private const string IS_WALK_PARAM = "isWalk";

    // Start is called before the first frame update
    void Start()
    {
        // Allow visuals to be structured with child objects.
        anim = gameObject.GetComponent<Animator>();
        if (anim == null) anim = gameObject.GetComponentInChildren<Animator>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followTarget == null) return;

        if (Vector3.Distance(transform.position, followTarget.position) > minDistance)
        {
            // walk towards the player
            if (anim != null) anim.SetBool(IS_WALK_PARAM, true);
            
            // Calculate direction for 4-way animation
            Vector3 direction = (followTarget.position - transform.position).normalized;
            if (anim != null)
            {
                anim.SetFloat("moveX", direction.x);
                anim.SetFloat("moveY", direction.z);
            }

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, followTarget.position, step);
        }
        else
        {
            // stop walking/return to idle
            if (anim != null) anim.SetBool(IS_WALK_PARAM, false);
        }
    }

    public void SetFollowDistance(float followDistance)
    {
        minDistance = followDistance;
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }
}
