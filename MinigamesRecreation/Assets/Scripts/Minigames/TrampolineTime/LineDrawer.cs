using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer line;
    private GameObject lineObject;
    private Vector3 mousePos;
    public Material material;
    public EdgeCollider2D EC2D;
    public LineController lineControllerScript;
    [SerializeField] public int noLines = 0;
    public float width;
    public float slope;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && noLines < 3) {
            if (line == null) {
                CreateLine();
            }

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            line.SetPosition(0, mousePos); 
            line.SetPosition(1, mousePos); 
        } else if (Input.GetMouseButtonUp(0) && line) {
            float length = Vector3.Distance(line.GetPosition(0), line.GetPosition(1));
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            if (length > 1.0) {
                Vector3 direction = line.GetPosition(1) - line.GetPosition(0);
                slope = direction.y / direction.x;
                Debug.Log(slope);

                if (Math.Abs(slope) > 0.85) {
                    Destroy(lineObject);
                } else {    
                    line.SetPosition(1, mousePos);
                    UpdateCollider(line, EC2D);
                    line = null;
                    lineControllerScript.isFinishedDrawing = true;
                    noLines++;
                }
                
            } else {
                Destroy(lineObject);
            }

        } else if (Input.GetMouseButton(0) && line) {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            line.SetPosition(1, mousePos);

            // Calculate slope based on current positions
            Vector3 direction = mousePos - line.GetPosition(0);
            slope = direction.y / direction.x;

            lineControllerScript.slope = slope; // Update slope in LineController

            float length = Vector3.Distance(line.GetPosition(0), line.GetPosition(1));
            width = 0.15f + length * 0.02f;

            line.startWidth = width;
            line.endWidth = width;
        }
        
    }

    void CreateLine() {
        lineObject = new GameObject("Line " + noLines) {
            tag = "TrampolineTT"
        };

        line = lineObject.AddComponent<LineRenderer>();
        EC2D = lineObject.AddComponent<EdgeCollider2D>();
        lineControllerScript = lineObject.AddComponent<LineController>();

        lineControllerScript.lineDrawer = this;

        line.material = material;
        line.positionCount = 2;
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;
        line.useWorldSpace = true;
        line.numCapVertices = 50;

        EC2D.isTrigger = true;
        EC2D.edgeRadius = 0.17f;
    }


    void UpdateCollider(LineRenderer lineRenderer, EdgeCollider2D edgeCollider) {
        Vector3 start = lineRenderer.GetPosition(0);
        Vector3 end = lineRenderer.GetPosition(1);

        Vector2[] points = new Vector2[2];
        points[0] = new Vector2(start.x, start.y);
        points[1] = new Vector2(end.x, end.y);

        edgeCollider.points = points;
    }

    
}
