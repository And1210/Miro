using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed;

    private bool moving = false;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (!moving) {
            if (horizontal >= 0.5) { //right
                StartCoroutine(Jump(0));
            } else if (horizontal <= -0.5) { //left
                StartCoroutine(Jump(1));
            } else if (vertical >= 0.5) { //up
                StartCoroutine(Jump(2));
            } else if (vertical <= -0.5) { //down
                StartCoroutine(Jump(3));
            }
        }
    }

    IEnumerator Jump(int dir) {
        moving = true;

        Vector3 relativeTarget = new Vector3(dir <= 1 ? -(2*dir-1) : 0, dir >= 2 ? -(2*(dir-2)-1) : 0, 0);
        Vector3 target = transform.position + relativeTarget;
        rb.AddForce(0, 0, -150);
        yield return new WaitForSeconds(0.1f);

        float step = speed * Time.deltaTime;
        while (Vector3.Magnitude(transform.position-target) > 0.01) {
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            yield return new WaitForEndOfFrame();
        }

        transform.position.Set(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);

        yield return new WaitForFixedUpdate();

        moving = false;
    }
}
