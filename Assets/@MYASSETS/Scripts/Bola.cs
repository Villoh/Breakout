using System;
using UnityEngine;

public class Bola : MonoBehaviour
{
    public static Action<Bola> OnBallDeath;

    public AudioSource hitSound;

    public AudioSource soundMuro;

    public void Muere()
    {
        OnBallDeath?.Invoke(this);
        Destroy(gameObject, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Paredes") || collision.gameObject.CompareTag("Raqueta"))
        {
            hitSound.Play();
        }
    }
}
