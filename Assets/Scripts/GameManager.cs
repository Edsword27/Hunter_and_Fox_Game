using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	
	private BoardManager boardScript;						//Referencia del BoardManager para invocar sus funciones
    public static GameManager instance = null;				//Permite acceder a las funciones del GameManager desde otros Scripts
    private float turnDelay = 0.7f;							//Duracion (en segundos) del tiempo entre cada turno
	[HideInInspector] public bool huntersTurn = true;		//Permite identificar que nos encontramos en el turno de los Hunters
	private Fox foxy;										//Instancia del Fox en el tablero

    //Permite inicializar todos los datos necesarios antes de que se inicie el juego
    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

	//Permite asociar la instancia de tipo Fox con el Fox que sera usado en el juego
	public void AddFox(Fox script) {
		foxy = script;
	}

    //Llama a las funciones del BoardManager que permiten iniciar el juego
    void InitGame() {
        boardScript.SetupScene();
    }

    //Esta funcion es llamada en cada Frame
    void Update() {
		//Mientras sea turno de los Hunters, no se ejecuta el resto del codigo
		if (huntersTurn)
			return;
		//Cuando acabe el turno de los Hunters, autorizamos el movimiento del Fox
        StartCoroutine(MoveFox());
		//Volvemos a cederle el turno a los Hunters
        huntersTurn = true;
    }

	//Hace los llamados para ejecutar el movimiento del Fox
    IEnumerator MoveFox() {
		yield return new WaitForSeconds(turnDelay);		//Realizamos una espera antes del movimiento del Fox
		foxy.Move();									//Ejecutamos el movimiento del Fox
    }

    // Funcion que retornará el objeto del Fox.
    public Fox getFoxy() {
        return foxy;
    }
}