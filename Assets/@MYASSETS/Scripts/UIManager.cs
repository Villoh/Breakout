using System.IO;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Comprueba que solo existe una instancia de este Manager
    #region Singleton

    private static UIManager _instancia;
    public static UIManager instancia => _instancia;

    private void Awake()
    {
        Muro.OnBrickDestruccion += OnBrickDestruccion;
        ManagerMuros.OnLevelLoaded += OnLevelLoaded;
        GameManager.OnLiveLost += OnLiveLost;
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

    public TMP_Text textoObjetivo;
    public TMP_Text textoPutuacion;
    public TMP_Text textoVidas;
    public TMP_Text textoRecord;
    public TMP_InputField jugador;

    public int puntuacion { get; set; }


    private void Start()
    {
        OnLiveLost(GameManager.instancia.vidasDisponibles);
        ActualizaTextoRecord();
    }

    private void OnLiveLost(int vidasDisponibles)
    {
        textoVidas.text = $"VIDAS: {vidasDisponibles}";
    }

    private void OnBrickDestruccion(Muro obj)
    {
        ActualizaTextoMurosRestantes();
        ActualizaTextoPuntuacion(10);
    }

    private void OnLevelLoaded()
    {
        ActualizaTextoMurosRestantes();
        ActualizaTextoPuntuacion(0);
    }

    private void ActualizaTextoPuntuacion(int incremento)
    {
        this.puntuacion += incremento;
        string puntuacionString = this.puntuacion.ToString().PadLeft(5, '0');
        textoPutuacion.text = $@"PUNTOS:
{puntuacionString}";
    }

    private void ActualizaTextoMurosRestantes()
    {
        textoObjetivo.text = $@"MUROS: 
{ManagerMuros.instancia.murosRestantes.Count} / {ManagerMuros.instancia.numInicialMuros}";
    }

    private void OnDisable()
    {
        Muro.OnBrickDestruccion -= OnBrickDestruccion;
        ManagerMuros.OnLevelLoaded -= OnLevelLoaded;
    }

    /**
     * public void ActualizaFicheroRecord()
     * Escribe en el fichero de texto el record, si es mayor que el que ya estaba
     */
    public void ActualizaFicheroRecord()
    {
        char[] alphabet = "abcdefghijklmnñopqrstuvwxyz:".ToCharArray();
        int recordActual = int.Parse((this.textoRecord.text.ToLower()).Trim(alphabet));
        if (this.puntuacion > recordActual)
        {
            string path = "./Assets/@MYASSETS/Resources/record.txt";
            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(jugador.text + ": "+ (this.puntuacion));
            writer.Close();
        }
    }

    public void ActualizaTextoRecord()
    {
        string path = "./Assets/@MYASSETS/Resources/record.txt";
        StreamReader reader = new StreamReader(path);
        this.textoRecord.text = reader.ReadToEnd();
        reader.Close();
    }
}
