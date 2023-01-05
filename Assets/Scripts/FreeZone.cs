using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeZone : ScriptableObject {

	private float leftLimit;		//Limite izquierdo del espacio de la FreeZone
	private float rightLimit;		//Limite derecho del espacio de la FreeZone
	private bool available;			//Indica la disponibilidad de esta FreeZone

	//Inicializamos las variables
	void Start () {
		leftLimit = 0;
		rightLimit = 0;
		available = false;
	}

	//Permite establecer los limites izquierdo y derecho de la FreeZone
	public void SetLimits (float lLimit, float rLimit){
		leftLimit = lLimit;
		rightLimit = rLimit;
	}

	//Permite establecer la disponibilidad de la FreeZone
	public void SetAvailable (bool aval){
		available = aval;
	}

	//Permite obtener el limite izquierdo
	public float GetLeftLimit(){ 
		return leftLimit; 
	}

	//Permite obtener el limite derecho
	public float GetRightLimit(){ 
		return rightLimit; 
	}

	//Permite obtener la disponibilidad
	public bool GetAvailable(){ 
		return available; 
	}
}
