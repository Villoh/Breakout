using UnityEngine;

public class Muerte : MonoBehaviour
{
    /**
     * Cuando toca con la pared de abajo este metodo se activa destruyendo la bola
     */
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if(collision.tag == "Bola")
        {
                Bola bola = collision.GetComponent<Bola>();
                ManagerBola.instancia.bolas.Remove(bola);
                bola.Muere();
        }
    }
}
