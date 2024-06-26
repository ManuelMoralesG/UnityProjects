using Unity.VisualScripting;
using UnityEngine;

public class BobOmbControl : MonoBehaviour
{   
    private bool dragging = false;
    [SerializeField] private bool draggable = true;
    private Vector3 offset;
    private Rigidbody2D RB2D;
    private CircleCollider2D CC2D;
    [SerializeField] private float direction = 0.0f;
    private float BlackOrPink = 0;
    private SpriteRenderer SR;

    void Start() {
        SR = GetComponent<SpriteRenderer>();

        BlackOrPink = Random.Range(0, 2);
        if (BlackOrPink == 0) {
            gameObject.tag = "PinkBobOmb";
            SR.color = Color.black;
        } else if (BlackOrPink == 1) {
            gameObject.tag = "PinkBobOmb";
            SR.color = new Color(222f / 255f, 31f / 255f, 103f / 255f, 1f);
        }

        direction = Random.Range(0, 4);
        RB2D = GetComponent<Rigidbody2D>();
        CC2D = GetComponent<CircleCollider2D>();
    }
    void Update()
    {   
        // Dragging controls
        if (dragging){
            if (draggable) {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                transform.position = mousePosition + offset;
            }
        }

        RB2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate() {
        GoBobOmb();
    }

    private void OnMouseDown(){
        if (draggable) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            offset = transform.position - mousePosition;
            dragging = true;
            
            RB2D.velocity = Vector2.zero;
            CC2D.enabled = false;
            SetAllCollidersStatus(false);
        }
    }

    private void OnMouseUp(){
        if (draggable) {
            dragging = false;
            direction = Random.Range(0, 4);
            CC2D.enabled = true;
            SetAllCollidersStatus(true);
        }
    }

    public void SetAllCollidersStatus (bool active) {
        foreach(var c in gameObject.GetComponentsInChildren<BoxCollider2D>()) {
            c.enabled = active;
        }
    }

    [SerializeField]
    private float bobOmbVelocity = 1.5f;

    public float publicBobOmbVelocity
    {
        get { return bobOmbVelocity; }
        set { bobOmbVelocity = value; }
    }
    public void GoBobOmb() {
        switch (direction){
            case 0:
                RB2D.velocity = new Vector2(bobOmbVelocity, bobOmbVelocity);
                break;
            case 1:
                RB2D.velocity = new Vector2(-bobOmbVelocity, -bobOmbVelocity);
                break;
            case 2:
                RB2D.velocity = new Vector2(-bobOmbVelocity, bobOmbVelocity);
                break;
            case 3:
                RB2D.velocity = new Vector2(bobOmbVelocity, -bobOmbVelocity);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Top")) {
            if (direction == 0) {
                direction = 3;
            } else if (direction == 2) {
                direction = 1;
            }

        }

        if (collision.gameObject.CompareTag("Right")) {
            if (direction == 0) {
                direction = 2;
            } else if (direction == 3) {
                direction = 1;
            }

        }

        if (collision.gameObject.CompareTag("Bottom")) {
            if (direction == 1) {
                direction = 2;
            } else if (direction == 3) {
                direction = 0;
            }

        }

        if (collision.gameObject.CompareTag("Left")) {
            if (direction == 1) {
                direction = 3;
            } else if (direction == 2) {
                direction = 0;
            }
            
        }

        if (collision.gameObject.CompareTag("BlackBox")) {
            draggable = false;
        }

        if (collision.gameObject.CompareTag("PinkBox")) {
            draggable = false;
        }

    }
}
