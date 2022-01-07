using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public UnityEngine.CharacterController character;

    private float speed = 4f;
    public float gravity = -9.8f;

    public float jumpHeight = 3f;
    Vector3 velocity;

    public Transform checkFloor;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    

    bool touchingFloor;

    public bool activeEnemy;

   
    void Update()
    {
        moveCharacter();
    
        
    }

    private void moveCharacter(){

        touchingFloor = Physics.CheckSphere(checkFloor.position, groundDistance, groundMask);

        if(Input.GetAxis("Run")==1 && touchingFloor)
            speed = 6f;
        else
            speed = 4f;
        
        if(touchingFloor && velocity.y < 0 ){

            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        character.Move(move * speed * Time.deltaTime);  

        if(Input.GetButtonDown("Jump") && touchingFloor){

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        character.Move(velocity * Time.deltaTime);

        

    }

    
}
