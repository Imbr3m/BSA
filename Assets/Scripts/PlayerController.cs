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

    // Update is called once per frame
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
        
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);

    }
}
