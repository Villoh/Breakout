using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaJuego : MonoBehaviour
{
    public void CargaJuego()
    {
        SceneManager.LoadScene("Juego", LoadSceneMode.Single);
    }
}
