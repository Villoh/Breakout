using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject gameOverScreen;

    public TMP_InputField inputJugador;

    public TMP_Text textoFinal;

    public int vidasDisponibles = 3;

    public int Vidas { get; set; }

    //Booleano para saber si el juego ha sido empezado
    public bool juegoEmpezado { get; set; }

    public static event Action<int> OnLiveLost;

    private void Start()
    {
        this.Vidas = this.vidasDisponibles;
        Screen.SetResolution(540, 960, false);
        Bola.OnBallDeath += OnBallDeath;
        Muro.OnBrickDestruccion += OnBrickDestruction;
    }

    private void OnBrickDestruction(Muro obj)
    {
        if (ManagerMuros.instancia.murosRestantes.Count <= 0) 
        {
            ManagerMuros.instancia.CargaSiguienteNivel();
        }
    }

    /**
     * void ReiniciarJuego()
     * Carga la escena de nuevo
     */
    public void ReinciarJuego()
    {
        if (inputJugador.text != "")
        {
            gameOverScreen.transform.GetChild(0).gameObject.SetActive(false);
            UIManager.instancia.ActualizaFicheroRecord();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            gameOverScreen.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void Salir()
    {
        if (inputJugador.text != "")
        {
            gameOverScreen.transform.GetChild(0).gameObject.SetActive(false);
            UIManager.instancia.ActualizaFicheroRecord();
            Application.Quit();
        }
        else
        {
            gameOverScreen.transform.GetChild(0).gameObject.SetActive(true);
        }
        
    }

    /**
     * private void OnBallDeath(object sender)
     * Metodo que se activa cuando no hay bolas en la escena
     */
    private void OnBallDeath(Bola obj)
    {
        if (ManagerBola.instancia.bolas.Count <= 0)
        {
            this.Vidas--;
            if (this.Vidas < 1)
            {
                gameOverScreen.SetActive(true);
            }
            else 
            {
                UIManager.instancia.puntuacion -= UIManager.instancia.puntuacionResidual;
                UIManager.instancia.puntuacionResidual = 0;
                OnLiveLost?.Invoke(this.Vidas);
                ManagerBola.instancia.ResetBolas();
                juegoEmpezado = false;
                ManagerMuros.instancia.LimpiarMurosRestantes();
                ManagerMuros.instancia.CargarNivel(ManagerMuros.instancia.NivelActual);
            }
        }
    }

    private void OnDisable()
    {
        Bola.OnBallDeath -= OnBallDeath;
    }

    public void Victoria()
    {
        foreach (var bola in ManagerBola.instancia.bolas.ToList())
        {
            Destroy(bola.gameObject);
        }   
        this.textoFinal.text = "Victoria!";
        this.gameOverScreen.SetActive(true);
    }
}
