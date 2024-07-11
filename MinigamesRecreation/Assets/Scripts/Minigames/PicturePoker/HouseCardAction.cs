using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCardAction : MonoBehaviour
{
    [SerializeField] private GameObject GameLogic;
    private LogicPicturePoker logicPicturePoker;
    private SpriteRenderer SR;
    public int cardValue;
    [SerializeField] private Sprite[] spriteList;

    void Start()
    {
        logicPicturePoker = GameLogic.GetComponent<LogicPicturePoker>();
        SR = GetComponent<SpriteRenderer>();

        logicPicturePoker.AddHouseCard(this);

        AssignNewValue();
    }   

    public void AssignNewValue() {
        cardValue = Random.Range(0, 6);
    }

    public void ShowCard() {
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

    public void HideCard() {
        SR.sprite = spriteList[6];
    }
}
