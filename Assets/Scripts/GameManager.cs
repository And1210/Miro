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

    //Information of the gamespace
    public int size;
    public int blockWidth;
    public int distance;

    //Used variables by the game
    GameObject[,] Bricks;
    GameObject player;

    void Start() {
        mainCamera = Camera.main;

        //Setting up the gamegrid
        Bricks = new GameObject[size, size];
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                int row = (i-size/2)*blockWidth;
                int col = (j-size/2)*blockWidth;
                Bricks[i, j] = Instantiate(Block, new Vector3(row, col, distance), Quaternion.identity);
                Bricks[i, j].GetComponent<MeshRenderer>().material = Waiting;
            }
        }

        //Spawning in the player
        player = Instantiate(Player, new Vector3(0, 0, distance - blockWidth - 0.5f), Quaternion.identity);
        player.GetComponent<Player>().SetGrid(Bricks);
    }

    void Update() {
        Transform playerTransform = player.GetComponent<Transform>();
        Vector3 pos = playerTransform.position;

        //Setting any blocks underneath the player to be "completed"
        int row = (int)(Mathf.Round(pos.x/blockWidth) + size/2);
        int col = (int)(Mathf.Round(pos.y/blockWidth) + size/2);
        Bricks[row, col].GetComponent<MeshRenderer>().material = Completed;

        mainCamera.GetComponent<Transform>().position = new Vector3(pos.x, pos.y, 0);
    }
}
