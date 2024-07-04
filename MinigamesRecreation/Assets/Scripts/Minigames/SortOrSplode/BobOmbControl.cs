using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BobOmbControl : MonoBehaviour
{   
    private Rigidbody2D RB2D;
    private CircleCollider2D CC2D;
    private SpriteRenderer SR;
    private new Light light;
    [SerializeField] private TMP_Text ScoreInGameDisplay;
    [SerializeField] private GameObject Spawner;
    private Logic logic;

    private bool dragging = false;
    [SerializeField] private bool draggable = true;
    private Vector3 offset;

    [SerializeField] private float direction = 0.0f;
    private float BlackOrPink = 0;
    public float timeBeforeExploding = 13f;
    private bool isInBox = false;


    void Start() {
        SR = GetComponent<SpriteRenderer>();

        // Determina si la bomba será color negro o rosa
        BlackOrPink = UnityEngine.Random.Range(0, 2);
        if (BlackOrPink == 0) {
            gameObject.tag = "BlackBobOmb";
            SR.color = Color.black;
        } else if (BlackOrPink == 1) {
            gameObject.tag = "PinkBobOmb";
            SR.color = new Color(222f / 255f, 31f / 255f, 103f / 255f, 1f);
        }

        // Direccion con la que la bomba "spawneara"
        direction = UnityEngine.Random.Range(0, 4);

        RB2D = GetComponent<Rigidbody2D>();
        CC2D = GetComponent<CircleCollider2D>();
        light = GetComponent<Light>();

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

        // Se asegura que la bomba principal no explote y que las bombas que esten siendo arrastradas no exploten
        if (gameObject.transform.position.x > -15 && !dragging) {
            timeBeforeExploding -= Time.deltaTime;
        }

        // Cambia de color cuando esta cerca de explotar
        if (timeBeforeExploding <= 4.5f) {
            if (!isInBox) {
                light.enabled = true;
            }
        }

        // Ejecuta GameOver cuando la bomba lleva x tiempo si ser guardada
        if (timeBeforeExploding <= 0.0f) {
            timeBeforeExploding -= 0;

            if (!logic.isGameOver && !isInBox) {
                logic.GameOver();
            }
        }
    }

    void FixedUpdate() {
        // Este metodo se ejecuta cada frame
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
        }
    }

    private void OnMouseUp(){
        if (draggable) {
            dragging = false;
            direction = UnityEngine.Random.Range(0, 4);
            CC2D.enabled = true;
        }
    }

    
    public float bobOmbVelocity = 1.0f;
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

    // Controla las colisiones de las bombas con otros objetos
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
                isInBox = true;
                light.enabled = false;
                ScoreInGameDisplay.text = (int.Parse(ScoreInGameDisplay.text) + 1).ToString(); 
            } else {
                if (!logic.isGameOver) {
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
                isInBox = true;
                light.enabled = false;
                ScoreInGameDisplay.text = (int.Parse(ScoreInGameDisplay.text) + 1).ToString(); 
            } else {
                if (!logic.isGameOver) {
                    logic.GameOver();
                }
            }

            if (logic.publicNoBobOmbsInPink >= 15) {
                Destroy(gameObject);
            }
        }

    }
}
