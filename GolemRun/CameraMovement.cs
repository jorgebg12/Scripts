using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float mouse_sensi= 300f;
    public Transform playerBody;
    float xRotation = 0f;

    public LayerMask objectlayer;

    public float distanceCatch = 0.1f;
    void Start()
    {
        transform.rotation = Quaternion.Euler(90,0,0);
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouse_sensi * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouse_sensi * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f,90f);

        transform.localRotation = Quaternion.Euler(xRotation,0,0);
        playerBody.Rotate(Vector3.up * mouseX);

        if(Input.GetKey("e")){

            interactObject();

        }

    }

    private void interactObject(){

        RaycastHit hit;
        GameObject selectedObject;

        if(Physics.Raycast(transform.position,transform.forward,out hit ,distanceCatch, objectlayer)){
            
            Debug.Log(hit.collider.tag);

            if(hit.collider.tag == "pickUp"){
                 
                selectedObject = hit.collider.gameObject;
                Destroy(selectedObject,0f);
                

            }

            if(hit.collider.tag == "lever"){

                
                 
                selectedObject = hit.collider.gameObject;

                Animator animator = selectedObject.GetComponent<Animator>();

                animator.SetTrigger("Activar");

                

            }

        }
        
    }
}
