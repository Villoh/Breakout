using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Muro : MonoBehaviour
{
    private SpriteRenderer sr;

    public int tipoMuroPuntuacion = 0;

    public int golpes = 1; //Variable publica con los golpes que va a poder recibir cada muro

    public ParticleSystem efectoDestruccion; //Particulas que apareceran con la destruccion del muro

    public static event Action<Muro> OnBrickDestruccion;

    private void Awake()
    {
        this.sr = GetComponent<SpriteRenderer>(); //SpriteRender del muro
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bola bola = collision.gameObject.GetComponent<Bola>(); //Obtiene la el gameObject de la bola que ha colisionado
        bola.soundMuro.Play();
        if (ManagerMuros.instancia.murosRestantes.Count <= 1)
        {
            StartCoroutine(u�ltimoMuro(bola));
        }
        else 
        {
            AplicarLogicaColision(bola);
        }
    }

    IEnumerator u�ltimoMuro(Bola bola)
    {
        bola.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        bola.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        this.GetComponent<SpriteRenderer>().sprite = null;
        SpawnEefectoDestruccion();
        yield return new WaitForSeconds(0.7F);
        AplicarLogicaColision(bola);
    }

    /**
     * private void AplicarLogicaColision (Bola bola)
     * Metodo que aplica la l�gica del muro al colisionar.
     */
    private void AplicarLogicaColision(Bola bola)
    {
        this.golpes--; //Resta un golpe
        //Comprueba que los golpes que puede recibir son iguales o menores que 0
        if (this.golpes <= 0)
        {
            if (ManagerMuros.instancia.murosRestantes.Count > 1)
            {
                SpawnEefectoDestruccion();
            }
            ManagerMuros.instancia.murosRestantes.Remove(this);
            OnBrickDestruccion?.Invoke(this);
            Destroy(this.gameObject);
        }
        else 
        {
            //Cambiar el sprite
            this.sr.sprite = ManagerMuros.instancia.sprites[this.golpes-1];
            
        }
        
    }

    /**
     * private void SpawnEefectoDestruccion()
     * Instancia un sistema de particulas y lo elimina al de un tiempo.
     */
    private void SpawnEefectoDestruccion()
    {
        Vector3 posMuro = gameObject.transform.position; //Posicion del muro
        Vector3 posSpawn = new Vector3(posMuro.x, posMuro.y, posMuro.z - 0.2F); //Posicion en la que va a hacer spawn el stsema de particulas
        GameObject clonEfectoDestruccion = Instantiate(efectoDestruccion.gameObject, posSpawn, Quaternion.identity); //Instancia el sistema de particulas
        
        MainModule mm = clonEfectoDestruccion.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color; //Cambia el color del sistema de particulas por el color que tiene SpriteRenderer del muro.
        Destroy(clonEfectoDestruccion, efectoDestruccion.main.startLifetime.constant); //Destruye el sistema de particulas, el tiempo es asignado en las propiedades de este.
    }

    /**
     * private void Init()
     * Inicializa el muro con unos atributos pasados como parametro
     */
    public void Init(Transform contenedorTransform, Sprite sprite, Color color, int golpes)
    {
        this.transform.SetParent(contenedorTransform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.golpes = golpes;
        this.tipoMuroPuntuacion = golpes;
    }
}
