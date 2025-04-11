using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCardAction : MonoBehaviour
{
    [SerializeField] private GameObject GameLogic;
    private LogicPicturePoker logicPicturePoker;
    private SpriteRenderer SR;
    public int cardValue;
    public bool isSelected = false;
    [SerializeField] private Sprite[] spriteList;


    // Start is called before the first frame update
    void Start()
    {
        logicPicturePoker = GameLogic.GetComponent<LogicPicturePoker>();
        SR = GetComponent<SpriteRenderer>();

        logicPicturePoker.AddPlayerCard(this);

        AssignNewValue();

        switch (cardValue)
        {
            case 0:
                    SR.sprite = spriteList[0];
                    break;
            case 1:
                    SR.sprite = spriteList[1];
                    break;
            case 2:
                    SR.sprite = spriteList[2];
                    break;
            case 3:
                    SR.sprite = spriteList[3];
                    break;
            case 4:
                    SR.sprite = spriteList[4];
                    break;
            case 5:
                    SR.sprite = spriteList[5];
                    break;
        }
    }

    public void AssignNewValue() {
        cardValue = Random.Range(0, 6);
    }

    // Update is called once per frame
    void Update()
    {
        switch (cardValue)
        {
            case 0:
                    SR.sprite = spriteList[0];
                    break;
            case 1:
                    SR.sprite = spriteList[1];
                    break;
            case 2:
                    SR.sprite = spriteList[2];
                    break;
            case 3:
                    SR.sprite = spriteList[3];
                    break;
            case 4:
                    SR.sprite = spriteList[4];
                    break;
            case 5:
                    SR.sprite = spriteList[5];
                    break;

        }
    }

    private void OnMouseDown() {
        if (logicPicturePoker.moneyBetInRound > 0) {
            if (!isSelected) {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f);
                logicPicturePoker.selectedCards += 1;
                isSelected = true;
            } else if (isSelected) {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f);
                logicPicturePoker.selectedCards -= 1;
                isSelected = false;
            }

            if (logicPicturePoker.selectedCards > 0) {
                logicPicturePoker.drawButton.SetActive(true);
                logicPicturePoker.holdButton.SetActive(false);
            } else {
                logicPicturePoker.drawButton.SetActive(false);
                logicPicturePoker.holdButton.SetActive(true);
            }
        }
    }

}
