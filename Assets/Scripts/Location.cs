using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : ScriptableObject {

	private Vector3 position;	//Posicion asociada a un Location
	private int score;			//Puntaje que señala el nivel de beneficio de moverse a esta Location
	private bool available;		//Indica si es posible moverse a esa Location

	//Inicializamos todas las variables con valores por Default
	void Start () {
		position = new Vector3 (-1, -1, 0);
		score = 0;
		available = true;
	}

	//Reinicia el score y el available a sus valores por Default
	public void ResetDefaults() {
		score = 0;
		available = true;
	}

	//Permite establecer el valor de position
	public void SetPosition (Vector3 pos){
		position = pos;
	}

	//Permite establecer el valor del score
	public void SetScore (int scor){
		score = scor;
	}

	//Permite establecer el valor de available
	public void SetAvailable (bool aval){
		available = aval;
	}

	//Permite obtener el valor de position
	public Vector3 GetPosition() { 
		return position; 
	}

	//Permite obtener el score
	public int GetScore() {
		return score; 
	}

	//Permite obtener el valor de available
	public bool GetAvailable() {
		return available; 
	}
}