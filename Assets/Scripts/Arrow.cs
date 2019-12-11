using UnityEngine;

public class Arrow {

    int row;
    int col;
    float rotation;

    public Arrow(int r, int c, float rot) {
        row = r;
        col = c;
        rotation = rot;
    }

    int GetRow() {
        return row;
    }
    int GetCol() {
        return col;
    }
    float GetRot() {
        return rotation;
    }
}
