using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
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

    private int maxFilas = 17;
    private int maxCols = 12;
    private GameObject contenedorMuros;
    private float posicionInicalSpawnMuroX = -1.96F;
    private float posicionInicalSpawnMuroY = 3.325F;
    private float variacion = 0.365F;

    public Muro prefabMuro;

    public Sprite[] sprites; //Lista con los sprites de los muros

    public Color[] coloresMuro; //Lista con los colores de los sprites

    public List<Muro> murosRestantes { get; set; }

    public List<int[,]> datosNiveles { get; set; } 

    public int numInicialMuros { get; set; }

    public int NivelActual;

    private void Start()
    {
        this.contenedorMuros = new GameObject("ContenedorMuros"); //Inicializa el objeto padre vacio donde se almacenaran los muros
        this.murosRestantes = new List<Muro>();
        this.datosNiveles = this.CargaNiveles();
        this.GenerarMuros();
    }

    /**
     * private void GenerarMuros()
     * Genera los muros mediante la lista datosNiveles
     */
    private void GenerarMuros()
    {
        int[,] datosNivelActual = this.datosNiveles[this.NivelActual]; //Carga los datos del nivel en una variable local
        float spawnActualX = posicionInicalSpawnMuroX; //Posicion del muro en X
        float spawnActualY = posicionInicalSpawnMuroY; //Posicion del muro en Y
        float variacionZ = 0;

        //Itera todas las filas
        for (int fila = 0; fila < this.maxFilas; fila++)
        {
            //Itera todas las columnas
            for (int col = 0; col < this.maxCols; col++)
            {
                int tipoMuro = datosNivelActual[fila, col];

                //Comprueba que el tipo del muro es valido
                if (tipoMuro > 0)
                {
                    Muro nuevoMuro = Instantiate(prefabMuro, new Vector3(spawnActualX, spawnActualY, 0.0F - variacionZ), Quaternion.identity) as Muro; //Instancia un nuevo muro
                    nuevoMuro.Init(contenedorMuros.transform, this.sprites[tipoMuro - 1], this.coloresMuro[tipoMuro], tipoMuro); //Inicializa el muro con unos atributos

                    this.murosRestantes.Add(nuevoMuro); 
                    variacionZ += 0.0001F;
                }

                spawnActualX += variacion;
                //Comprueba que ha llegado al final de las columnas
                if (col + 1 == this.maxCols)
                {
                    spawnActualX = posicionInicalSpawnMuroX;
                }
            }
            spawnActualY -= variacion;
        }

        this.numInicialMuros = this.murosRestantes.Count;
    }

    /**
     * private void CargaNiveles()
     * Carga los niveles del archivo niveles.txt
     */
    private List<int[,]> CargaNiveles()
    {
        TextAsset text = Resources.Load("niveles") as TextAsset; //Carga el texto del archivo niveles.txt
        string[] filas = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); //Rellena el array con el texto del archivo

        List<int[,]> niveles = new List<int[,]>();
        int[,] nivelActual = new int[maxFilas, maxCols]; //Declara un array de enteros con el tamaño maximo de filas y columnas.
        int filaActual = 0;

        //Bucle para iterar cada línea
        for (int fila = 0; fila < filas.Length; fila++)
        {
            string linea = filas[fila]; //Linea de esta iteracion

            //Comprueba que la linea no tiene los caracteres "--", si los contiene es el fin del nivel
            if (linea.IndexOf("--") == -1)
            {
                string[] muros = linea.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries); //Array con las columnas/muros de la linea/fila, cada muro separado por la coma.
                //Bucle para iterar cada muro/columna
                for (int col = 0; col < muros.Length; col++)
                {
                    nivelActual[filaActual, col] = int.Parse(muros[col]); //Rellena la matriz con cada iteracion
                }

                filaActual++; //Nueva fila
            }
            else
            {
                //Final del nivel
                filaActual = 0; //Reset
                niveles.Add(nivelActual); //Añade a la matriz niveles el nivelActual
                nivelActual = new int[maxFilas, maxCols]; //Reset
            }
        }
        return niveles;
    }
}
