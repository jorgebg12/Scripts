using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoCamara : MonoBehaviour
{
    private CharacterController controlador;
    /// <summary>
    /// Velocidad de movimiento de la camara
    /// </summary>
    public float velocidad = 10;
    private Vector3 movement;
    /// <summary>
    /// Objetivo al que se observa constantemente
    /// </summary>
    public Transform objetivo;

    void awake()
    {
    }
    void Start()
    {
        controlador = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if ((transform.localPosition.y <= 5 && movement.z > 0) || (transform.localPosition.y >= 10 && movement.z < 0))
            controlador.Move(transform.TransformVector(new Vector3(movement.x, movement.y, 0)) * Time.deltaTime * velocidad);
        else
            controlador.Move(transform.TransformVector(movement) * Time.deltaTime * velocidad);
        //movement = Vector3.zero;
        transform.LookAt(objetivo);
    }

    /// <summary>
    /// Funcion que cambia los valores de movimiento
    /// </summary>
    /// <param name="direccion">El nuevo vector de movimiento</param>
    public void mover(InputAction.CallbackContext direccion)
    {
        Vector2 movimiento = direccion.ReadValue<Vector2>();
        movement = new Vector3(movimiento.x, 0, movimiento.y);
    }

}
