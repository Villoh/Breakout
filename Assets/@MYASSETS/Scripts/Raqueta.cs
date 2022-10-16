using UnityEngine;

public class Raqueta : MonoBehaviour
{
    //Comprueba que solo existe una instancia de esta Raqueta
    #region Singleton

    private static Raqueta _instancia;
    public static Raqueta instancia => _instancia;

    private void Awake()
    {
        if (_instancia != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instancia = this;
        }
    }
    #endregion

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
        MovimientoRaqueta();
    }

    /**
     * private void movimientoRaqueta()
     * Obtiene el valor de la posicion de X en relación al ratón y lo actualiza en este GameObject.
     */
    private void MovimientoRaqueta() 
    {
        float variacionRaqueta = (anchuraRaquetaPixeles - ((anchuraRaquetaPixeles / 2) * this.sr.size.x)) /2; //Formula para obtener la variacion de anchura de la raqueta
        float limiteIzquierdo = limiteIzquierdoDefecto - variacionRaqueta; //Limite izquierdo del Fondo
        float limiteDerecho = limiteDerechoDefecto + variacionRaqueta; //Limite derecho del fondo
        float posicionRatonPixeles = Mathf.Clamp(Input.mousePosition.x, limiteIzquierdo, limiteDerecho); //Dependiendo del valor de la posición del ratón devolverá el limite Izquierdo, el derecho o la posción actual en x
        float posicionRatonX = mainCamera.ScreenToWorldPoint(new Vector3(posicionRatonPixeles, 0, 0)).x; //Posición del ratón en X en la camara principal
        this.transform.position = new Vector3(posicionRatonX, posicionRatonInicialY, 0); //Cambia la posición de la raqueta
    }

    /**
     * Cuando detecta una colision entra en el metodo
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Comprueba i la colision es con el objeto Bola
        if (collision.gameObject.tag == "Bola")
        {
            //Calcula la posición de la raqueta en la que ha golpeado la bola

            Rigidbody2D rbBola = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 puntoGolpe = collision.contacts[0].point;
            Vector3 centroRaqueta = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            rbBola.velocity = Vector2.zero;

            float diferencia = centroRaqueta.x - puntoGolpe.x;

            if (puntoGolpe.x > centroRaqueta.x)
            {
                rbBola.AddForce(new Vector2(-(Mathf.Abs(diferencia * 200)), ManagerBola.instancia.velocidadInicialBola)*-1);
            }
            else
            {
                rbBola.AddForce(new Vector2((Mathf.Abs(diferencia * 200)), ManagerBola.instancia.velocidadInicialBola)*-1);
            }
        }
    }
}
