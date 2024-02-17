using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HotBarController : MonoBehaviour
{
    public delegate void UseItem();
    public static ItemHolding holdingItem;
    public UseItem LeftClick;

    public UseItem RightClick;
    public Color SelectedColor;
    public Color UnselectedColor;
    bool isUnsorted = false;
    // Start is called before the first frame update
    void Start()
    {
        holdingItem = ItemHolding.None;
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.player_progress.Melee.ItemInstance==null && GameManager.instance.player_progress.Melee.isUnlocked)
        {
            GameManager.instance.player_progress.Melee.InstantiatePrefab(transform); isUnsorted = true;

        }
        if (GameManager.instance.player_progress.DataBall.ItemInstance == null && GameManager.instance.player_progress.DataBall.isUnlocked)
        {
            GameManager.instance.player_progress.DataBall.InstantiatePrefab(transform); isUnsorted = true;

        }
        if (GameManager.instance.player_progress.Command.ItemInstance == null && GameManager.instance.player_progress.Command.isUnlocked)
        {
            GameManager.instance.player_progress.Command.InstantiatePrefab(transform); isUnsorted = true;

        }
        if (GameManager.instance.player_progress.HuntingRifle.ItemInstance == null && GameManager.instance.player_progress.HuntingRifle.isUnlocked)
        {
            GameManager.instance.player_progress.HuntingRifle.InstantiatePrefab(transform); isUnsorted = true;

        }
        if (GameManager.instance.player_progress.Shotgun.ItemInstance == null && GameManager.instance.player_progress.Shotgun.isUnlocked)
        {
            GameManager.instance.player_progress.Shotgun.InstantiatePrefab(transform); isUnsorted = true;

        }
        if (GameManager.instance.player_progress.AssaultRifle.ItemInstance == null && GameManager.instance.player_progress.AssaultRifle.isUnlocked)
        {
            GameManager.instance.player_progress.AssaultRifle.InstantiatePrefab(transform); isUnsorted = true;

        }
        if (isUnsorted)
        {
            SortHotBar();
            isUnsorted = false;
        }
        // Select ITEM
        if (Input.GetKeyDown(KeyCode.Alpha1) && GameManager.instance.player_progress.Melee.isUnlocked)
        {
            holdingItem = ItemHolding.Melee;

            SetColorAllItems();
            GameManager.instance.player_progress.Melee.ItemInstance.GetComponent<RawImage>().color = SelectedColor;

        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && GameManager.instance.player_progress.DataBall.isUnlocked)
        {
            holdingItem = ItemHolding.DataBall;
            LeftClick = GameManager.instance.playerShootScript.Shoot_Databall;

            SetColorAllItems();
            GameManager.instance.player_progress.DataBall.ItemInstance.GetComponent<RawImage>().color = SelectedColor;

        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && GameManager.instance.player_progress.Command.isUnlocked)
        {
            holdingItem = ItemHolding.Command;
            LeftClick = DataMonCommand.Patrol;
            SetColorAllItems();
            GameManager.instance.player_progress.Command.ItemInstance.GetComponent<RawImage>().color = SelectedColor;

        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && GameManager.instance.player_progress.HuntingRifle.isUnlocked)
        {
            holdingItem = ItemHolding.HuntingRifle;
            LeftClick = GameManager.instance.playerShootScript.Shoot_HuntingRifle;

            SetColorAllItems();
            GameManager.instance.player_progress.HuntingRifle.ItemInstance.GetComponent<RawImage>().color = SelectedColor;

        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && GameManager.instance.player_progress.Shotgun.isUnlocked)
        {
            holdingItem = ItemHolding.Shotgun;
            LeftClick = GameManager.instance.playerShootScript.Shoot_Shotgun;

            SetColorAllItems();
            GameManager.instance.player_progress.Shotgun.ItemInstance.GetComponent<RawImage>().color = SelectedColor;

        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && GameManager.instance.player_progress.AssaultRifle.isUnlocked)
        {
            holdingItem = ItemHolding.AssaultRifle;
            LeftClick = GameManager.instance.playerShootScript.Shoot_AssaultRifle;

            SetColorAllItems();
            GameManager.instance.player_progress.AssaultRifle.ItemInstance.GetComponent<RawImage>().color = SelectedColor;

        }
        // =====================================================================================================
        switch (holdingItem)
        {
            case ItemHolding.Command:
            case ItemHolding.Melee:
            case ItemHolding.DataBall:
                CheckInputDown(KeyCode.Mouse0,LeftClick);
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    CameraFollow.isAiming = true;
                }
                if (Input.GetKeyUp(KeyCode.Mouse1))
                {

                    CameraFollow.isAiming = false;
                }
                break;
            case ItemHolding.Shotgun:
            case ItemHolding.HuntingRifle:
            case ItemHolding.AssaultRifle:
                CheckInput(KeyCode.Mouse0, LeftClick);
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    CameraFollow.isAiming = true;
                }
                if (Input.GetKeyUp(KeyCode.Mouse1))
                {

                    CameraFollow.isAiming = false;
                }
                break;
            default:
                break;
        }
    }
    public void SetColorAllItems()
    {
        if (GameManager.instance.player_progress.Melee.isUnlocked)
            GameManager.instance.player_progress.Melee.ItemInstance.GetComponent<RawImage>().color = UnselectedColor;

        if (GameManager.instance.player_progress.DataBall.isUnlocked)
            GameManager.instance.player_progress.DataBall.ItemInstance.GetComponent<RawImage>().color = UnselectedColor;

        if (GameManager.instance.player_progress.Command.isUnlocked)
            GameManager.instance.player_progress.Command.ItemInstance.GetComponent<RawImage>().color = UnselectedColor;

        if (GameManager.instance.player_progress.HuntingRifle.isUnlocked)
            GameManager.instance.player_progress.HuntingRifle.ItemInstance.GetComponent<RawImage>().color = UnselectedColor;

        if (GameManager.instance.player_progress.Shotgun.isUnlocked)
            GameManager.instance.player_progress.Shotgun.ItemInstance.GetComponent<RawImage>().color = UnselectedColor;

        if (GameManager.instance.player_progress.AssaultRifle.isUnlocked)
            GameManager.instance.player_progress.AssaultRifle.ItemInstance.GetComponent<RawImage>().color = UnselectedColor;


    }
    int sortOrder;
    public void SortHotBar()
    {
        sortOrder = 0;
        if (GameManager.instance.player_progress.Melee.isUnlocked)
        {
            GameManager.instance.player_progress.Melee.ItemInstance.transform.SetSiblingIndex(sortOrder);
            sortOrder++;
        }
        if (GameManager.instance.player_progress.DataBall.isUnlocked)
        {
            GameManager.instance.player_progress.DataBall.ItemInstance.transform.SetSiblingIndex(sortOrder);
            sortOrder++;
        }
        if (GameManager.instance.player_progress.Command.isUnlocked)
        {
            GameManager.instance.player_progress.Command.ItemInstance.transform.SetSiblingIndex(sortOrder);
            sortOrder++;
        }
        if (GameManager.instance.player_progress.HuntingRifle.isUnlocked)
        {
            GameManager.instance.player_progress.HuntingRifle.ItemInstance.transform.SetSiblingIndex(sortOrder);
            sortOrder++;
        }
        if (GameManager.instance.player_progress.Shotgun.isUnlocked)
        {
            GameManager.instance.player_progress.Shotgun.ItemInstance.transform.SetSiblingIndex(sortOrder);
            sortOrder++;
        }
        if (GameManager.instance.player_progress.AssaultRifle.isUnlocked)
        {
            GameManager.instance.player_progress.AssaultRifle.ItemInstance.transform.SetSiblingIndex(sortOrder);
            sortOrder++;
        }
    }
    public void CheckInputDown(KeyCode input, UseItem click)
    {
        if (Input.GetKeyDown(input))
        {
            click();
        }
    }
    public void CheckInput(KeyCode input, UseItem click)
    {
        if (Input.GetKey(input))
        {
            click();
        }
    }
    public void Aiming()
    {

    }
    public void ShowDifferentCommands()
    {

    }
    public void CommandDataMons()
    {

    }
}

public enum ItemHolding
{
    Command, Melee, DataBall, HuntingRifle, Shotgun, AssaultRifle, None
}
