using System.Collections;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject gameManager;

    //Carga las funciones iniciales del juego
	void Awake () {
        if (GameManager.instance == null)
            Instantiate(gameManager);
	}
	
}