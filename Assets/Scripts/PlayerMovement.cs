using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;


    public float runSpeed = 25f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    Animator anim;

    


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch") && (Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Horizontal") < 0))
        {
            crouch = true;
            anim.SetBool("isCrouching", true);
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
            anim.SetBool("isCrouching", false);
        }




    }

    void FixedUpdate()
    {

        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;






    }


}
