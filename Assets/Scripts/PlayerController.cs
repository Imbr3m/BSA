using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer playerSprite;

    

    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 movement;
    private const string IS_WALK_PARAM = "isWalk";

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        float x = playerControls.Player.Move.ReadValue<Vector2>().x;
        float z = playerControls.Player.Move.ReadValue<Vector2>().y;

        movement = new Vector3(x, 0, z).normalized;

        if (movement != Vector3.zero) 
        {
            anim.SetFloat("moveX", x);
            anim.SetFloat("moveY", z);
        }

        anim.SetBool(IS_WALK_PARAM, movement != Vector3.zero);
        

        // freeze player when dialogue
        if (DialogueManager.Instance != null && DialogueManager.Instance.PlayingDialogue)
        {
            anim.SetBool(IS_WALK_PARAM, false); // Go to idle animation
            movement = Vector3.zero; // Stop physics movement
            return; 
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);

    }

    public void SetOverworldVisuals(Animator animator, SpriteRenderer spriteRenderer, Vector3 scale)
    {
        anim = animator;
        playerSprite = spriteRenderer;
        transform.localScale = scale;
    }
}
