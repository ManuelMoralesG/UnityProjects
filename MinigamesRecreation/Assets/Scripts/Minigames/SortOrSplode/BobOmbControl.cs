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
    [SerializeField] private GameObject Spawner;
    private Logic logic;

    void Start() {
        SR = GetComponent<SpriteRenderer>();

        // Determina si la bomba será color negro o rosa
        BlackOrPink = Random.Range(0, 2);
        if (BlackOrPink == 0) {
            gameObject.tag = "BlackBobOmb";
            SR.color = Color.black;
        } else if (BlackOrPink == 1) {
            gameObject.tag = "PinkBobOmb";
            SR.color = new Color(222f / 255f, 31f / 255f, 103f / 255f, 1f);
        }

        direction = Random.Range(0, 4);
        RB2D = GetComponent<Rigidbody2D>();
        CC2D = GetComponent<CircleCollider2D>();
        logic = Spawner.GetComponent<Logic>();
    }
    void Update()
    {   
        // Controles para arrastrar las bombas
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
        // Está funcion se ejecuta cada frame
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
        // Controla hacia donde se moveran las bombas
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

    // Controla las colisiones de las bombas
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

        // Control de que pasa cuando una bomba es arrastrada a su repectiva caja
        if (collision.gameObject.CompareTag("BlackBox")) {
            if (gameObject.CompareTag("BlackBobOmb")) {
                gameObject.transform.localPosition = new Vector2(5.85f,0.25f);
                draggable = false;
                logic.publicNoBobOmbsInBlack += 1;
            } else {
                if (!logic.isGameOver)
                {
                    logic.GameOver();
                }
            }

            if (logic.publicNoBobOmbsInBlack >= 15) {
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("PinkBox")) {
            if (gameObject.CompareTag("PinkBobOmb")) {
                gameObject.transform.localPosition = new Vector2(-5.85f,0.25f);
                draggable = false;
                logic.publicNoBobOmbsInPink += 1;
            } else {
                if (!logic.isGameOver)
                {
                    logic.GameOver();
                }
            }

            if (logic.publicNoBobOmbsInPink >= 15) {
                Destroy(gameObject);
            }
        }

    }
}
