using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBola : MonoBehaviour
{
    //Comprueba que solo existe una instancia de este Manager
    #region Singleton

    private static ManagerBola _instancia;
    public static ManagerBola instancia => _instancia;

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

    [SerializeField]
    private Bola prefabBola; //Instancia con el prefab de la Bola

    private Bola bolaInicial; //Instancia para tratar la bola

    private Rigidbody2D rbBolaInicial; //RigidBody de la bola

    public float velocidadInicialBola = -250; //Velocidad que adquirirá la bola

    public List<Bola> bolas { get; set; }

    private void Start()
    {
        InicializarBola();
    }

    private void Update()
    {
        //Que la bola siga a la raqueta si el juego no ha empezado.
        if (!GameManager.instancia.juegoEmpezado)
        {
            Vector3 posicionRaqueta = Raqueta.instancia.gameObject.transform.position;
            Vector3 posicionInicial = new Vector3(posicionRaqueta.x, posicionRaqueta.y + 2F, 0);
            bolaInicial.transform.position = posicionInicial;

            //Comprueba que se hace click izquierdo, comienza el juego y añades movimiento a la bola
            if(Input.GetMouseButtonDown(0))
            {
                rbBolaInicial.isKinematic = false;
                rbBolaInicial.AddForce(new Vector2(0, velocidadInicialBola));
                GameManager.instancia.juegoEmpezado = true;
            }
        }
    }

    private void InicializarBola()
    {
        Vector3 posicionRaqueta = Raqueta.instancia.transform.position; //Posicion de la Raqueta
        Vector3 posicionInicial = new Vector3(posicionRaqueta.x, posicionRaqueta.y + .5F, 0); //Posicion donde aparecerá la bola
        bolaInicial = Instantiate(prefabBola, posicionInicial, Quaternion.identity); //Inicializa la instancia de la bola
        rbBolaInicial = bolaInicial.GetComponent<Rigidbody2D>(); //Inicializa el RigidBody de la bola
        
        this.bolas = new List<Bola>
        { 
            bolaInicial
        };
    }
}
