using Unity.VisualScripting;
using UnityEngine;

public class BobOmbControl : MonoBehaviour
{
    private bool dragging = false;
    private Vector3 offset;
    private Rigidbody2D RB2D;

    void Start() {
        RB2D = GetComponent<Rigidbody2D>();
        GoBobOmb();
    }
    void Update()
    {   
        // Dragging controls
        if (dragging){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition + offset;
        }

        // Bouncing controls
        if (RB2D.velocity.x == 0 || RB2D.velocity.y == 0) {
            Debug.Log("Velocity reached 0");
            GoBobOmb();
        }

        Debug.Log(RB2D.velocity);
    }

    void FixedUpdate() {
        
    }

    private void OnMouseDown(){
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
        RB2D.velocity = Vector2.zero;
    }

    private void OnMouseUp(){
        dragging = false;
        GoBobOmb();
    }

    public void GoBobOmb() {
        float randomNumber = Random.Range(0, 4);

        if (randomNumber < 1) {
            RB2D.AddForce(new Vector2(70, 70));
        } else if (randomNumber < 2) {
            RB2D.AddForce(new Vector2(-70, -70));
        } else if (randomNumber < 3) {
            RB2D.AddForce(new Vector2(70, -70));
        } else if (randomNumber < 4) {
            RB2D.AddForce(new Vector2(-70, 70));
        }
    }
}
