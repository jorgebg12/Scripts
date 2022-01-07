using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    
    public float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    LemonController lemonController;

    private int lemonScore = 0;
    private int lemonsCarried = 0;
    private bool lemonGrabbed;
    public LineRenderer lineRenderer;
    private GameObject myLemon;
    private GameObject proyectil;

    public float lineLifetime = 0.5f;
    public float velocidadLanzamiento = 200f;
    private float lineEraseTime;
    public GameObject rightHand;
    public TextMeshProUGUI lemonCountHUD;
    private Vector3 desiredForward;
    Vector3 posWorld;

    void Start ()
    {
        m_Animator = GetComponent<Animator> ();
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_AudioSource = GetComponent<AudioSource> ();
        posWorld = new Vector3(0, 0, 0);
        lemonGrabbed = false;
    }

    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);
        
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop ();
        }

        headMouse(); //Que lemon mire hacia el mouse
        if(lemonsCarried>0){
            //Forma de Javi de lanzar un limon (con mouse, se puede usar para lanzar a la pared)
            if (Input.GetMouseButtonDown(0)) {
                throwLemonForward();
            }
            //Forma de Jorge de lanzar el limon (se puede usar para dejar en el suelo)
            if(Input.GetMouseButtonDown(1)){
                putLemonDown(); 
            }
        }

        if(Time.time >= lineEraseTime){
            lineRenderer.enabled = false;
        }
    }

    void OnAnimatorMove ()
    {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
    }

    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "lemons"){
            lemonController = other.gameObject.GetComponent<LemonController>();
            if (lemonController.state == lemonStates.Idle) {
                lemonScore++;
                lemonsCarried++;
                lemonCountHUD.text=lemonScore.ToString();

                if(!lemonGrabbed){
                    other.gameObject.transform.localScale =new Vector3(20f,20f,20f);
                    
                    myLemon = other.gameObject;

                    other.gameObject.transform.parent = rightHand.transform;
                    other.gameObject.transform.localPosition = rightHand.transform.localPosition;

                    lemonController.grab();
                    lemonGrabbed=true;                
                }else{
                    Destroy(other.gameObject);
                }
            }
        }

    }

    void putLemonDown(){
        if(--lemonsCarried <= 0) lemonGrabbed = false;

        if (lemonGrabbed) proyectil = Instantiate(myLemon, transform.position, transform.rotation);
        else proyectil = myLemon;

        lemonController = proyectil.GetComponent<LemonController>();
        lemonController.thrownToFloor();
        
        proyectil.transform.parent = null;
        
        proyectil.transform.position = transform.position;

        lemonScore--;
        lemonCountHUD.text=lemonScore.ToString();

    }

    void throwLemonForward ()
    {       
        RaycastHit hit;
        Vector3 posicionInicial; 
        Vector3 posicionFinal;

        int layerMask = 1 << 8;
        if (Physics.Raycast(transform.position, desiredForward, out hit, 100.0f, layerMask)) 
        {
            if(--lemonsCarried <= 0) lemonGrabbed = false;

            if (lemonGrabbed) proyectil = Instantiate(myLemon, transform.position, transform.rotation);
            else proyectil = myLemon;
            
            posicionInicial = proyectil.transform.position;
            lemonController = proyectil.GetComponent<LemonController>();
            
            proyectil.transform.parent = null;
            
            print("Found an object - distance: " + hit.distance);
            Vector3 hitpoint = hit.point;
            Vector3 normalPared = hit.normal;

            posicionFinal = new Vector3(hitpoint.x, 1.0f, hitpoint.z);
            proyectil.transform.position = posicionFinal;
            lemonController.thrownToWall(normalPared);
            
            lineRenderer.SetPosition(0, posicionInicial);
            lineRenderer.SetPosition(1, posicionFinal); 
            lineEraseTime = Time.time + lineLifetime;
            lineRenderer.enabled = true; 

            lemonScore--;
            lemonCountHUD.text=lemonScore.ToString();   
        }
    }    

    public void headMouse()
    {
        RaycastHit hit;
                
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
            posWorld = hit.point;
            desiredForward = posWorld - transform.position;
            desiredForward.y = 0.0f;

            Quaternion newRotation = Quaternion.LookRotation(desiredForward);
            m_Rigidbody.MoveRotation(newRotation);
        }
        else m_Rigidbody.MoveRotation (m_Rotation);        
    }
}