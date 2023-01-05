using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MovingObject {

	private GameObject actualFloor;		//Guarda una instancia del Floor actual donde se encuentra el Hunter
	private bool firstMove = true;		//Indica si el movimiento del Hunter sera su primer movimiento

	//Aqui llamamos a las funciones iniciales de la clase padre
	protected override void Start () {
		base.Start ();
	}

	//Llamamos a la funcion SmoothMovement de la clase padre, pasandole la posicion donde se movera el Hunter
	public void Move(Vector3 movePosition) {
		base.StartCoroutine (SmoothMovement (movePosition));
	}

	//Permite obtener el valor de firstMove
	public bool GetFirstMove(){
		return firstMove;
	}

	//Permite establecer el valor de firstMove
	public void SetFirstMove( bool fM ){
		firstMove = fM;
	}

	//Permite obtener el actualFloor
	public GameObject GetActualFloor(){
		return actualFloor;
	}

	//Permite establecer el valor del actualFloor
	public void SetActualFloor( GameObject aF ){
		actualFloor = aF;
	}
}