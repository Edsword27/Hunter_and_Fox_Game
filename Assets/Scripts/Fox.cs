using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MovingObject {

	private Location[] floors = new Location[4];			//Manejan los datos relacionados a las posiciones donde el zorro se puede mover
	private GameObject[] hunters = new GameObject[4];		//Instancias de los cazadores que persiguen al zorro
	private Vector3 previousMove;							//Guarda la posicion previa en la que se encontraba el zorro
	private FreeZone safeZone;
	private int direccion; 			 //0=sin rumbo, 1=derecha, 2=de frente a la derecha, 3=de frente a la izquierda, 4=izquierda
	public bool firstMove;

	protected override void Start () {
		base.Start ();
		//Inicializamos las 4 variables de tipo Location y los 4 cazadores
		for (int i = 0; i < 4; i++) {
			floors [i] = ScriptableObject.CreateInstance<Location> ();
			hunters[i] = GameObject.FindGameObjectWithTag ("Player"+(i+1));
		}
		previousMove = new Vector3(base.GetX(),base.GetY(),0);
		safeZone = ScriptableObject.CreateInstance<FreeZone> ();
		direccion = 0;
		firstMove = true;
		GameManager.instance.AddFox (this);
	}

	//Identifica las posiciones disponibles a las que puede moverse el zorro en el turno actual
	public void Reconocimiento() {
		float x = base.GetX ();
		float y = base.GetY ();

		//Esquina Superior Izquierda
		floors[0].SetPosition (new Vector3(x-1, y+1, 0));
		if (floors[0].GetPosition ().x == -1 || floors[0].GetPosition ().y == -1 || floors[0].GetPosition ().x == 8 || floors[0].GetPosition ().y == 8 || IsHunterIn(floors[0]))
			floors[0].SetAvailable (false);

		//Esquina Superior Derecha
		floors[1].SetPosition (new Vector3(x + 1f, y + 1f, 0));
		if (floors[1].GetPosition ().x == -1 || floors[1].GetPosition ().y == -1 || floors[1].GetPosition ().x == 8 || floors[1].GetPosition ().y == 8 || IsHunterIn(floors[1]))
			floors[1].SetAvailable (false);

		//Esquina Inferior Izquierda
		floors[2].SetPosition (new Vector3(x - 1f, y - 1f, 0));
		if (floors[2].GetPosition ().x == -1 || floors[2].GetPosition ().y == -1 || floors[2].GetPosition ().x == 8 || floors[2].GetPosition ().y == 8 || IsHunterIn(floors[2]))
			floors[2].SetAvailable (false);

		//Esquina Inferior Derecha
		floors[3].SetPosition (new Vector3(x+1, y-1, 0));
		if (floors[3].GetPosition ().x == -1 || floors[3].GetPosition ().y == -1 || floors[3].GetPosition ().x == 8 || floors[3].GetPosition ().y == 8 || IsHunterIn(floors[3]))
			floors[3].SetAvailable (false);

		//Eleccion de direccion
		if (safeZone.GetAvailable ()) {
			if (x < safeZone.GetLeftLimit () && x < safeZone.GetRightLimit ())
				direccion = 1;
			if (x > safeZone.GetLeftLimit () && x > safeZone.GetRightLimit ())
				direccion = 4;
			if (x == safeZone.GetLeftLimit ())
				direccion = 2;
			if (x == safeZone.GetRightLimit ())
				direccion = 3;
		} else
			direccion = 0;
	}

	//Comprueba si un cazador esta en la posicion (floor) que estamos analizando, retornando true si
	//se encuentra un cazador y false en caso contrario
	public bool IsHunterIn(Location floor){
		for (int i = 0; i < 4; i++) {
			if (hunters [i].transform.position.Equals (floor.GetPosition()))
				return true;
		}
		return false;
	}

	//Permite comprobar si la posicion analizada hace al zorro avanzar en el escenario (eje Y),
	//asignando un mayor puntaje a esa posicion en caso de ser asi
	public void IsUpper(Location floor){
		if (floor.GetPosition ().y > base.GetY ())
			floor.SetScore (floor.GetScore ()+2);
		else
			floor.SetScore (floor.GetScore ()+1);
	}

	//Comprueba la posibilidad de que al moverse el zorro a la posicion analizada, en el siguiente
	//turno un cazador pueda alcanzarlo
	/*public void SeeDangerMove(Location floor){
		float x, y;
		for (int i = 0; i < 4; i++) {
			
			x = hunters [i].transform.position.x;
			y = hunters [i].transform.position.y;

			if (floor.GetPosition().Equals(new Vector3(x-1,y-1,0f)) || floor.GetPosition().Equals(new Vector3(x+1,y-1,0f))) {
				floor.SetScore (floor.GetScore()-1);
				return;
			}
		}
		floor.SetScore (floor.GetScore()+1);
	}*/

	//Reduce el puntaje de la posicion analizada si esa posicion fue la escogida 2 turnos atras
	public void IsPreviousMove(Location floor){
		if (floor.GetPosition ().Equals (previousMove))
			floor.SetScore (floor.GetScore()-1);
	}

	public bool SearchSafeZone(){
		float posH1 = hunters [0].transform.position.x;
		float posH2 = hunters [1].transform.position.x;
		float posH3 = hunters [2].transform.position.x;
		float posH4 = hunters [3].transform.position.x;

		for (int l = 0; l < 7; l++) {
			if (posH1 != l && posH2 != l && posH3 != l && posH4 != l) {
				if (posH1 != (l + 1) && posH2 != (l + 1) && posH3 != (l + 1) && posH4 != (l + 1)) {
					safeZone.SetLimits (l, l + 1);
					return true;
				}
			}
		}
		return false;
	}

	public void DirectionPoints(){
		if (direccion == 0) {
			floors [0].SetScore (floors[0].GetScore()+1);
			floors [1].SetScore (floors[1].GetScore()+1);
			floors [2].SetScore (floors[2].GetScore()+1);
			floors [3].SetScore (floors[3].GetScore()+1);
		}
		if (direccion == 1) {
			floors [1].SetScore (floors[1].GetScore()+2);
			floors [3].SetScore (floors[3].GetScore()+2);
		}
		if (direccion == 2) {
			floors [1].SetScore (floors[1].GetScore()+2);
		}
		if (direccion == 3) {
			floors [0].SetScore (floors[0].GetScore()+2);
		}
		if (direccion == 4) {
			floors [0].SetScore (floors[0].GetScore()+2);
			floors [2].SetScore (floors[2].GetScore()+2);
		}
	}

	public bool isTraped(){
		if (!floors [0].GetAvailable () && !floors [1].GetAvailable () && !floors [2].GetAvailable () && !floors [3].GetAvailable ())
			return true;
		return false;
	}

	public void Move() {
		int highScore = -1;
		Vector3 choosenMove = new Vector3(base.GetX(),base.GetY(),0);
		floors [0].ResetDefaults ();
		floors [1].ResetDefaults ();
		floors [2].ResetDefaults ();
		floors [3].ResetDefaults ();
		safeZone.SetAvailable (SearchSafeZone ());
		Reconocimiento ();
		DirectionPoints ();
		for (int i = 0; i < 4; i++) {
			if (floors [i].GetAvailable ()) {
				IsUpper (floors [i]);
				//SeeDangerMove (floors [i]);
				IsPreviousMove (floors [i]);
				if (floors [i].GetScore () > highScore) {
					choosenMove = floors [i].GetPosition ();
					highScore = floors [i].GetScore ();
				}
			}
		}
		previousMove = new Vector3(base.GetX(),base.GetY(),0);
		base.StartCoroutine (SmoothMovement (choosenMove));
		firstMove = false;
	}

}