using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Multiplayer.Center.Common;
using UnityEditor;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private GameObject puertaPuebloPrefab, puertaJefePrefab;

    // numero máximo y mínimo de habitaciones
    [SerializeField] private int maxRooms = 15;
    [SerializeField] private int minRooms = 10;

    // tamaño de las habitaciones
    private int roomWidht = 19;
    private int roomHeight = 11;

    // tamaño del grid
    [SerializeField] private int gridSizeX = 10;
    [SerializeField] private int gridSizeY = 10;

    // lista de habitaciones generadas
    private List<GameObject> roomObjects = new List<GameObject>();

    // cola de habitaciones a generar
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

    // grid de habitaciones, 
    // indica con 0 si la celda está vacía 
    // y con 1 si está ocupada por una habitación
    private int[,] roomGrid;

    // contador de habitaciones generadas
    private int roomCount;
    private bool generationComplete = false;



    private void Start()
    {
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();

        // inciar la generación en mitad del grid
        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeX / 2);
        InstantiateRoom(initialRoomIndex);

        // añade cerca de la mitad de la sala inicial el portal para salir de la mazmorra
        Vector3 offset = new Vector3(-1.5f, 0.5f, 0);
        Instantiate(puertaPuebloPrefab, GetPositionFromGridIndex(initialRoomIndex) + offset, Quaternion.identity);

    }

    private void Update()
    {
        // mientras haya habitaciones en la cola y no se haya alcanzado el máximo de habitaciones
        if (roomQueue.Count > 0 && roomCount < maxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int x = roomIndex.x;
            int y = roomIndex.y;

            // aunque aqui esten en un orden, luego en la función con un %
            // se descarta la room para que pase a la siguiente y así dar aleatoriedad
            // aunque es cierto que la ultima opcion es mas improvable que salga
            // hacerlo con random range 0,1,2,3 y elegir con switch sería lo mas aleatorio
            // pero la que saca si es fallida al tener mas adjacentes ya se corta y
            // tarda muchos mas intentos en generar una mazmorra valida
            TryGenerateRoom(new Vector2Int(x - 1, y));
            TryGenerateRoom(new Vector2Int(x + 1, y));
            TryGenerateRoom(new Vector2Int(x, y + 1));
            TryGenerateRoom(new Vector2Int(x, y - 1));
        }
        // si no hay habitaciones en cola para expandir
        // pero no hemos llegado al minimo de rooms
        // es que la solución no ha cumplido las restricciones y generamos otra
        else if (roomCount < minRooms && !generationComplete)
        {
            // si no se ha alcanzado el mínimo de habitaciones, regenerar
            Debug.Log("Regenerating rooms...");
            RegenerateRooms();
        }
        // si ha salido bien
        else if (!generationComplete)
        {
            // abrimos todas las puertas que conectan dos habitaciones adyacentes
            OpenDoorsForAllRooms();

            // añadimos en la habitacion mas lejana la puerta al boss
            AddBossRoom();

            Debug.Log("Generation complete");
            generationComplete = true;
        }

    }

    // busca la sala mas alejada del centro y le pone añade el portal al boss
    private void AddBossRoom()
    {
        Vector2Int roomInicial = new Vector2Int(gridSizeX / 2, gridSizeX / 2);
        Vector2Int roomGanadora = roomInicial;
        int mayorDistancia = 0;

        for (int i = 0; i < roomGrid.GetLength(0); i++)
        {
            for (int j = 0; j < roomGrid.GetLength(1); j++)
            {
                if (roomGrid[i, j] == 1)
                {
                    int distancia = Mathf.Abs(i - roomInicial.x) + Mathf.Abs(j - roomInicial.y);
                    if (distancia > mayorDistancia)
                    {
                        mayorDistancia = distancia;
                        roomGanadora = new Vector2Int(i, j);
                    }
                }
            }
        }

        Debug.Log("Room mas lejana: " + roomGanadora + " distancia " + mayorDistancia);
        Vector3 offset = new Vector3(0.5f, 0.5f, 0);
        Instantiate(puertaJefePrefab, GetPositionFromGridIndex(roomGanadora) + offset, Quaternion.identity);

    }

    private void RegenerateRooms()
    {
        // limpiar las habitaciones generadas
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;

        // reiniciar el grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                roomGrid[x, y] = 0;
            }
        }

        // empezamos generando la inicial y enconlandola (como en el start)
        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        InstantiateRoom(initialRoomIndex);
    }

    // genera una room en dicho indice
    private void InstantiateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;
        GameObject newRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        newRoom.name = "Room " + roomCount;
        newRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(newRoom);

        // la encolamos para que luego trate de expandirse en el update
        roomQueue.Enqueue(roomIndex);
    }


    // intenta generar una habitación en la posición indicada
    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        // Evitar índices fuera del grid
        if (x < 0 || x >= gridSizeX || y < 0 || y >= gridSizeY) return false;

        // Evitar generar en una celda ya ocupada
        if (roomGrid[x, y] == 1) return false;

        // salir si se llegó al maximo de habitaciones
        if (roomCount >= maxRooms) return false;

        // damos 50% de probabilidad de no generar una habitación en esa direccion
        // para que pase a la siguiente
        if (Random.value < 0.5f && roomIndex != Vector2Int.zero) return false;

        // evitar generar habitaciones adyacentes a más de una habitación
        if (CountAdjacentRooms(roomIndex) > 1) return false;

        // marcar la celda como ocupada y encolar para que se expanda esa nueva habitacion
        InstantiateRoom(roomIndex);

        return true;
    }

    // abre las puertas de todas las habitaciones generadas
    private void OpenDoorsForAllRooms()
    {
        foreach (GameObject room in roomObjects)
        {
            Room roomComponent = room.GetComponent<Room>();
            OpenDoors(room, roomComponent.RoomIndex);
        }
    }

    // abre las puertas de una habitación según las habitaciones adyacentes
    private void OpenDoors(GameObject room, Vector2Int currentIndex)
    {

        Room currentRoom = room.GetComponent<Room>();

        Debug.Log("-----------------");
        Debug.Log("Abre la puerta " + currentRoom.name + " posicion " + currentIndex.x + ", " + currentIndex.y);


        // Para cada dirección, si hay una habitación adyacente, abre puerta y asigna referencias
        TryOpenAndLinkDoor(currentRoom, currentIndex, Vector2Int.left);
        TryOpenAndLinkDoor(currentRoom, currentIndex, Vector2Int.right);
        TryOpenAndLinkDoor(currentRoom, currentIndex, Vector2Int.up);
        TryOpenAndLinkDoor(currentRoom, currentIndex, Vector2Int.down);
    }

    // intenta abrir y vincular una puerta entre la habitación actual y la vecina
    private void TryOpenAndLinkDoor(Room currentRoom, Vector2Int currentIndex, Vector2Int direction)
    {
        Debug.Log("--> Intento a la direccion: " + direction.x + ", " + direction.y);

        Vector2Int neighborIndex = currentIndex + direction;

        Debug.Log("El vecino es " + neighborIndex.x + ", " + neighborIndex.y);

        // Validar que esté dentro del grid
        if (neighborIndex.x < 0 || neighborIndex.x >= gridSizeX || neighborIndex.y < 0 || neighborIndex.y >= gridSizeY)
            return;

        // Verificar si hay habitación vecina
        if (roomGrid[neighborIndex.x, neighborIndex.y] != 1)
        {
            Debug.Log("Ahi no hay ningun vecino (salgo)");
            return;
        }

        Debug.Log("Ahi SI hay vecino");

        // Obtener la habitación vecina
        GameObject neighborRoomObj = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == neighborIndex);
        if (neighborRoomObj == null)
        {
            Debug.Log("Intento buscar su game object en lista roomobjects pero NO lo encuentra ");
            return;
        }


        Room neighborRoom = neighborRoomObj.GetComponent<Room>();

        Debug.Log("Si encuentra su game object y saco el Room");



        // Abrir puertas en ambas habitaciones
        currentRoom.OpenDoor(direction);
        Debug.Log("Abro mi puerta " + direction.x + ", " + direction.y);
        neighborRoom.OpenDoor(-direction);
        Debug.Log("Abro la puerta del vecino" + -direction.x + ", " + -direction.y);

    }



    // ------------------
    // Métodos auxiliares
    // ------------------

    // cuenta las habitaciones adyacentes
    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        // Evitar índices fuera del grid
        if (x < 0 || x >= gridSizeX || y < 0 || y >= gridSizeY) return 0;

        if (x > 0 && roomGrid[x - 1, y] == 1) count++; // izquierda
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] == 1) count++; // derecha
        if (y > 0 && roomGrid[x, y - 1] == 1) count++; // abajo
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] == 1) count++; // arriba

        return count;
    }


    // convierte un índice de grid a una posición en el mundo
    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidht * (gridX - gridSizeX / 2), roomHeight * (gridY - gridSizeY / 2));
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidht, roomHeight, 1));
                Handles.color = Color.white;
                Handles.Label(position, x.ToString() + " , " + y.ToString());
            }
        }
    }
#endif
    
}
