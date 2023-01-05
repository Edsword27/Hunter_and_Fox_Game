using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsGame : MonoBehaviour {

    // Manejará el Menu que hace que el zorro gane la partida.
    public GameObject winfoxImageUI;

    // Manejará el Menu que hace que los hunters ganen la partida.
    public GameObject winhuntersImageUI;

    [HideInInspector]
    public GameManager gamemanagerScript;

    [HideInInspector]
    private Fox foxyAux;

	// Aqui se inicializan las variables
	void Start () {
        // Asi comunicamos a una clase con la otra en C# with UNITY.
		gamemanagerScript = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    // Update is called once per frame
    void Update() {
        // Obtenemos al fox, un objeto nuevo cada frame, ya que el zorro se mueve y pues es necesario actualizar el objeto cada vez.
        foxyAux = gamemanagerScript.getFoxy();

        // Si ocurre esto, y == 7, quiere decir que el zorro llego a la cumbre del tablero, y gana la partida.
        if (foxyAux.transform.position.y == 7) { 
            winfoxImageUI.SetActive(true);
        }

		//Si se la funcion isTraped da true, significa que el zorro se encuentra rodeado. Es decir, ninguna de las 
		//4 posiciones a las que se puede mover estan disponibles
		if (foxyAux.isTraped () && !foxyAux.firstMove) {
			winhuntersImageUI.SetActive(true);
		}
			
    }

}