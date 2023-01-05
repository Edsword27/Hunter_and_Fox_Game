using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {
	
	public float moveTime = 1f;			//Tiempo (en segundos) que le tomara moverse a un personaje
	private Rigidbody2D rb2D;			//Guarda el componente Rigidbody2D asociado a un personaje
	private float inverseMoveTime;		//Sirve para facilitar ciertos calculos

	//Inicializa los valores necesarios al momento de instanciar un MovingObject
	protected virtual void Start () {
		rb2D = GetComponent <Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;
	}

	//Realiza todos los calculos y procedimientos necesarios para que un personaje se mueva de forma fluida a una posicion especifica (pasada como parametro)
	protected IEnumerator SmoothMovement (Vector3 end) {
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		while(sqrRemainingDistance > float.Epsilon) {
			Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition (newPostion);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	//Permite obtener el valor de X del Rigidbody2D asociado a un objeto
	public float GetX() {
		return rb2D.position.x;
	}

	//Permite obtener el valor de Y del Rigidbody2D asociado a un objeto
	public float GetY() {
		return rb2D.position.y;
	}
}