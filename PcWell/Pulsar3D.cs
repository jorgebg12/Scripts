using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pulsar3D : MonoBehaviour
{
    public Vector2 raton;
    private Camera camara;
    // Start is called before the first frame update
    void Start()
    {
        camara = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void actualizarRaton(InputAction.CallbackContext direction)
    {
        raton = direction.ReadValue<Vector2>();
    }

    public void PulsadoEncima()
    {
        Ray rayo = camara.ScreenPointToRay(new Vector3(raton.x, raton.y, camara.nearClipPlane));
        Debug.DrawRay(rayo.origin, rayo.direction * 100, Color.red);
        if (Physics.Raycast(rayo, out RaycastHit golpe))
            Debug.Log("Pulsado");
    }
}
