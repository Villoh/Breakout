using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Raqueta : MonoBehaviour
{

    private Camera mainCamera; //Objeto de la camara para obtener la posicion del raton en Y
    private float posicionRatonInicialY; //Variable para la posicion del ratón en Y
    private float anchuraRaquetaPixeles = 200; //Anchura por defecto de la raqueta en pixeles
    private SpriteRenderer sr; //Componente del Renderer para obtener la anchura de este Sprite
    private float limiteIzquierdoDefecto = 135; //Limite izquierdo por defecto del fondo
    private float limiteDerechoDefecto = 410; //Limite derecho por defecto del fondo

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>(); //Asigna a la variable la camara de la escena
        posicionRatonInicialY = this.transform.position.y; //La posicion de Y que será la misma siempre al iniciar el script.
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        movimientoRaqueta();
    }

    /**
     * private void movimientoRaqueta()
     * Obtiene el valor de la posicion de X en relación al ratón y lo actualiza en este GameObject.
     */
    private void movimientoRaqueta() 
    {
        float variacionRaqueta = (anchuraRaquetaPixeles - ((anchuraRaquetaPixeles / 2) * this.sr.size.x)) /2; //Formula para obtener la variacion de anchura de la raqueta
        float limiteIzquierdo = limiteIzquierdoDefecto - variacionRaqueta; //Limite izquierdo del Fondo
        float limiteDerecho = limiteDerechoDefecto + variacionRaqueta; //Limite derecho del fondo
        float posicionRatonPixeles = Mathf.Clamp(Input.mousePosition.x, limiteIzquierdo, limiteDerecho);
        float posicionRatonX = mainCamera.ScreenToWorldPoint(new Vector3(posicionRatonPixeles, 0, 0)).x;
        this.transform.position = new Vector3(posicionRatonX, posicionRatonInicialY, 0);
    }
}
