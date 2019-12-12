using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow {

    int row;
    int col;
    float rotation;
    bool real;

    GameObject brickRef;

    public Arrow(int r, int c, float rot, ref GameObject brick, bool isReal) {
        row = r;
        col = c;
        rotation = rot;
        real = isReal;

        brickRef = brick;
    }

    public void ChangeMaterial(Material m) {
        brickRef.GetComponent<MeshRenderer>().material = m;
    }

    public GameObject SetOverlay(Material m) {
        Vector3 pos = brickRef.GetComponent<Transform>().position;
        pos.z -= 0.01f;

        GameObject overlayRef = GameObject.Instantiate(brickRef, pos, Quaternion.AngleAxis(rotation, new Vector3(0, 0, -1)));
        overlayRef.GetComponent<MeshRenderer>().material = m;

        return overlayRef;
    }


    public int GetRow() {
        return row;
    }
    public int GetCol() {
        return col;
    }
    public float GetRot() {
        return rotation;
    }
    public bool IsReal() {
        return real;
    }

    public void SetRot(float angle) {
        rotation = angle;
    }
    public void MakeReal() {
        real = true;
    }
}
