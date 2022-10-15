using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Comprueba que solo existe una instancia de este GameManager
    #region Singleton

    private static GameManager _instancia;
    public static GameManager instancia => _instancia;

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

    //Booleano para saber si el juego ha sido empezado
    public bool juegoEmpezado { get; set; }

    private void Start()
    {
        Screen.SetResolution(540, 960, false);
    }
}
