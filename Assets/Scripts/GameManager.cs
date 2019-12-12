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
    GameObject[,] Overlays;
    Arrow[,] Arrows;
    GameObject player;
    Vector2 playerPos;
    private Vector2 targetIndex;

    void Start() {
        mainCamera = Camera.main;

        //Setting up the gamegrid
        Bricks = new GameObject[size, size];
        Overlays = new GameObject[size, size];
        Arrows = new Arrow[size, size];
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                int row = (i-size/2)*blockWidth;
                int col = (j-size/2)*blockWidth;

                Bricks[i, j] = Instantiate(Block, new Vector3(row, col, distance), Quaternion.identity);
                Overlays[i, j] = null;

                Arrows[i, j] = new Arrow(i, j, 0, ref Bricks[i,j], false);
                Arrows[i, j].ChangeMaterial(Waiting);
            }
        }

        Arrows[size-6, size-3].ChangeMaterial(Target);
        targetIndex = new Vector2(size - 5, size - 3);
        Overlays[size-6, size-3] = Arrows[size-6, size-3].SetOverlay(Target);

        //Spawning in the player
        player = Instantiate(Player, new Vector3(0, 0, distance - blockWidth - 0.5f), Quaternion.identity);
        player.GetComponent<Player>().SetGrid(Bricks);
        Transform playerTransform = player.GetComponent<Transform>();
        Vector3 pos = playerTransform.position;
        playerPos = new Vector2((int)(Mathf.Round(pos.x/blockWidth) + size/2), (int)(Mathf.Round(pos.y/blockWidth) + size/2));

        GetPath(new Vector2((int)playerPos.x, (int)playerPos.y), new Vector2(size - 5, size - 3));
        FillGrid();
        // Debug.Log(size*size);
        // Debug.Log(GetEmpty());
    }

    void Update() {
        Transform playerTransform = player.GetComponent<Transform>();
        Vector3 pos = playerTransform.position;

        //Setting any blocks underneath the player to be "completed"
        int i = (int)(Mathf.Round(pos.x/blockWidth) + size/2);
        int j = (int)(Mathf.Round(pos.y/blockWidth) + size/2);
        playerPos.Set(i, j);

        Arrows[i, j].ChangeMaterial(Completed);

        mainCamera.GetComponent<Transform>().position = new Vector3(pos.x, pos.y, 0);
    }

    void GetPath(Vector2 playerIndex, Vector2 targetIndex) {
        List<Vector2> output = new List<Vector2>();

        int i = (int)playerIndex.x;
        int j = (int)playerIndex.y;

        Vector2 step = new Vector2(0, 0);
        while (i != targetIndex.x && j != targetIndex.y) {
            int iDiff = (int)targetIndex.x - i;
            int jDiff = (int)targetIndex.y - j;

            if (iDiff / jDiff > 1) {
                step.Set(iDiff/Mathf.Abs(iDiff), 0);
            } else {
                step.Set(0, jDiff/Mathf.Abs(jDiff));
            }

            // Debug.Log(i + " " + j + " " + step);

            Arrows[i, j].SetRot(Vector2.Angle(new Vector2(0, 1), step));
            Arrows[i, j].MakeReal();

            Overlays[i, j] = Arrows[i, j].SetOverlay(Pointer);
            i += (int)step.x;
            j += (int)step.y;
        }
    }

    void FillGrid() {
        int empty = GetEmpty();
        while(empty > 0) {
            for (int i = 0; i < Overlays.GetLength(0); i++) {
                for (int j = 0; j < Overlays.GetLength(1); j++) {
                    for (int k = 1; k > -3; k--) {
                        if (Overlays[i, j] != null)
                            break;
                        int i_t = (int)Mathf.Max(Mathf.Min(Overlays.GetLength(0)-1, i + k % 2), 0);
                        int j_t = (int)Mathf.Max(Mathf.Min(Overlays.GetLength(1)-1, j + -1*((k+1) % 2)), 0);
                        if (Overlays[i_t, j_t] != null) {
                            Arrows[i, j].SetRot((Arrows[i_t, j_t].GetRot() + 180) % 360);
                            Overlays[i, j] = Arrows[i, j].SetOverlay(Pointer);
                            break;
                        }
                    }
                }
            }
            empty = GetEmpty();
        }
    }

    int GetEmpty() {
        int notFilled = 0;
        for (int i = 0; i < Overlays.GetLength(0); i++) {
            for (int j = 0; j < Overlays.GetLength(1); j++) {
                if (Overlays[i, j] == null) {
                    notFilled++;
                }
            }
        }

        return notFilled;
    }
}
