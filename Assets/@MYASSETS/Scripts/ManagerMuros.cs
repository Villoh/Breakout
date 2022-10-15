using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerMuros : MonoBehaviour
{
    //Comprueba que solo existe una instancia de este Manager
    #region Singleton

    private static ManagerMuros _instancia;
    public static ManagerMuros instancia => _instancia;

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

    public Sprite[] sprites;
}
