using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject Player;
    Camera mainCamera;

    //Block information for the gamespace
    public GameObject Block;
    public Material Completed;
    public Material Waiting;
    public Material Target;
    public Material Pointer;

    //Information of the gamespace
    public int size;
    public int blockWidth;
    public int distance;

    //Used variables by the game
    GameObject[,] Bricks;
    Arrow[,] Arrows;
    GameObject player;
    private Vector2 targetIndex;

    void Start() {
        mainCamera = Camera.main;

        //Setting up the gamegrid
        Bricks = new GameObject[size, size];
        Arrows = new Arrow[size, size];
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                int row = (i-size/2)*blockWidth;
                int col = (j-size/2)*blockWidth;

                Bricks[i, j] = Instantiate(Block, new Vector3(row, col, distance), Quaternion.identity);
                ChangeMaterial(Bricks[i, j], Waiting);

                Arrows[i, j] = new Arrow(i, j, 0);
            }
        }

        ChangeMaterial(Bricks[size - 5, size - 3], Target);
        targetIndex = new Vector2(size - 5, size - 3);

        //Spawning in the player
        player = Instantiate(Player, new Vector3(0, 0, distance - blockWidth - 0.5f), Quaternion.identity);
        player.GetComponent<Player>().SetGrid(Bricks);
    }

    void Update() {
        Transform playerTransform = player.GetComponent<Transform>();
        Vector3 pos = playerTransform.position;

        //Setting any blocks underneath the player to be "completed"
        int i = (int)(Mathf.Round(pos.x/blockWidth) + size/2);
        int j = (int)(Mathf.Round(pos.y/blockWidth) + size/2);
        ChangeMaterial(Bricks[i, j], Completed);

        mainCamera.GetComponent<Transform>().position = new Vector3(pos.x, pos.y, 0);
    }

    //Vector2[] GetPath(Vector2 playerIndex, Vector2 targetIndex) {
    //    List<Vector2> output = new List<Vector2>();

    //    int i = playerIndex.x;
    //    int j = playerIndex.y;

    //}

    void ChangeMaterial(GameObject go, Material m) {
        go.GetComponent<MeshRenderer>().material = m;
    }
}
