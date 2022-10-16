using System;
using UnityEngine;

public class Bola : MonoBehaviour
{
    public static Action<Bola> OnBallDeath;
    public void Muere()
    {
        OnBallDeath?.Invoke(this);
        Destroy(gameObject, 1);
    }
}
