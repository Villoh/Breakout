using System;
using UnityEngine;

public class Bola : MonoBehaviour
{
    public static Action<Bola> OnBallDeath;

    public AudioSource hitSound;

    public AudioSource muro;

    public void Muere()
    {
        OnBallDeath?.Invoke(this);
        Destroy(gameObject, 1);
    }

    private void Start()
    { 
        hitSound = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Paredes") || collision.gameObject.CompareTag("Raqueta"))
        {
            hitSound.Play();
        }
        if (collision.gameObject.CompareTag("Muros"))
        {
            muro.Play();
        }
    }
}
