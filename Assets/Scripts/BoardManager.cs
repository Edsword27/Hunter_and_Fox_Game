using UnityEngine;
using System;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

	public static BoardManager instance = null;
    public int columns = 8; 						//Columnas del escenario
    public int rows = 8;							//Filas del escenario
	public bool hunterSelected = false;				//Indica que se ha seleccionado a un Hunter
	public GameObject whiteTile;					//Casilla Blanca
	public GameObject blackTile;					//Casilla Negra
	public GameObject outerWallTile;				//Casilla Exterior
    public GameObject [] hunterTile;			    // Arreglo de Hunters. Son 4 Cazadores, agregados desde el inspector.
	public GameObject foxTile;						//Objeto Fox
	public GameObject hunter;						//Almacena el GameObject del Hunter seleccionado

	public Vector3 hunterPosition;					//Guarda la posicion del Hunter seleccionado
	public Vector3 floorPosition;					//Guarda la posicion de la casilla donde se movera el Hunter seleccionado	

	private Transform boardHolder;									//Guarda una referencia del Transform del escenario
	private List<Vector3> gridPositions = new List<Vector3>();	    //Lista de ubicaciones en el escenario

    //Limpia nuestra lista gridPositions y lo prepara para generar el escenario
    void InitialiseList() {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++) {
            for (int y = 1; y < rows - 1; y++) {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

	//Establece que solo haya una instancia del boardManager
	void Awake() {
		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}
		
    //Posiciona todas las casillas que conforman al escenario
	void BoardSetup() {
		
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++) {
            for (int y = -1; y < rows + 1; y++) {
				
                GameObject tileSelected;

                if (x % 2 == 0) {
                    if (y % 2 == 0) tileSelected = whiteTile;
                    else tileSelected = blackTile;
                }

                else {
                    if (y % 2 == 0) tileSelected = blackTile;
                    else tileSelected = whiteTile;
                }

				if (x == -1 || x == columns || y == -1 || y == rows)
					tileSelected = outerWallTile;
					
				GameObject instance = Instantiate (tileSelected, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    //Coloca a 4 Hunters y un Fox  en sus posiciones iniciales en el tablero. Recibe un arreglo de objetos hunters, un objecto fox y la posicion inicial de los hunters (y).
	void LayoutCharacters(GameObject[] hunterObj, GameObject foxObj, int y) {
		//Hunter 1
        Instantiate(hunterObj[0], new Vector3(1, y, 0f), Quaternion.identity);
        hunterTile[0].transform.SetPositionAndRotation(new Vector3(1, y, 0f), Quaternion.identity);
		//Hunter 2
        Instantiate(hunterObj[1], new Vector3(3, y, 0f), Quaternion.identity);
        hunterTile[1].transform.SetPositionAndRotation(new Vector3(3, y, 0f), Quaternion.identity);
		//Hunter 3
        Instantiate(hunterObj[2], new Vector3(5, y, 0f), Quaternion.identity);
        hunterTile[2].transform.SetPositionAndRotation(new Vector3(5, y, 0f), Quaternion.identity);
		//Hunter 4
        Instantiate(hunterObj[3], new Vector3(7, y, 0f), Quaternion.identity);
        hunterTile[3].transform.SetPositionAndRotation(new Vector3(7, y, 0f), Quaternion.identity);
		//Se instancea el zorro en una posicion predeterminada.
        Instantiate(foxObj, new Vector3(4, 0f, 0f), Quaternion.identity);
    }

    //Establecemos las configuraciones iniciales del escenario al llamar a las funciones previas
    public void SetupScene() {
		BoardSetup();								//Crea las casillas del tablero
        InitialiseList();							//Configuramos y limpiamos cada una de las posiciones del tablero
		LayoutCharacters(hunterTile, foxTile, 7);	//Colocamos a cuatro Hunters en la fila 8 del tablero, y a un Fox en la fila 1
    }

    void Update() {
		//Al hacer click izquierdo en el mouse, se procede a llamar a la funcion CastRay
		if (Input.GetMouseButtonDown(0) && GameManager.instance.huntersTurn)
			CastRay();
	}

	//Aqui se manejan todas las funciones asociadas al mouse
	void CastRay() {
		
		Vector2 ray = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (ray, Vector2.zero);

		if (hit.collider != null) {
			
			//Si encuentra a un Hunter guardaremos su posicion, el GameObject de ese Hunter e indicaremos que ya se seleccion a un Hunter
			if (hit.collider.tag == "Player1" || hit.collider.tag == "Player2" || hit.collider.tag == "Player3" || hit.collider.tag == "Player4") {
                hunterPosition = hit.collider.bounds.center;
				hunter = hit.collider.gameObject;
				hunterSelected = true;
			}

			//Si encuentra un piso comprobamos que sea un movimiento valido, y de serlo, enviamos esa posicion al metodo Move del Hunter seleccionado
			if (hit.collider.tag == "Floor" && hunterSelected) {
				floorPosition = hit.collider.bounds.center;
				if ( (floorPosition.y == hunterPosition.y - 1) && (floorPosition.x == hunterPosition.x - 1 || floorPosition.x == hunterPosition.x + 1) ) {
					hunter.GetComponent <Hunter>().Move (floorPosition);
					hit.collider.gameObject.GetComponent <BoxCollider2D> ().enabled = false;
					//Aqui se manejan unas comprobaciones que activan y desactivan ciertos BoxCollider2D para mejorar el control de lo Hunters
					if (!hunter.GetComponent<Hunter> ().GetFirstMove ()) {
						hunter.GetComponent<Hunter> ().GetActualFloor ().GetComponent <BoxCollider2D> ().enabled = true;
						hunter.GetComponent<Hunter> ().SetActualFloor (hit.collider.gameObject);
					} 
					else {
						hunter.GetComponent<Hunter> ().SetActualFloor (hit.collider.gameObject);
						hunter.GetComponent<Hunter> ().SetFirstMove (false);
					}
					hunterSelected = false;
					GameManager.instance.huntersTurn = false;
				}
			}
		}
	}

}