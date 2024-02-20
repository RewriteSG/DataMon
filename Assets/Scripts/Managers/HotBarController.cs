using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System;
public class HotBarController : MonoBehaviour
{
    public delegate void UseItem();
    public static ItemHolding holdingItem;
    public UseItem LeftClick;
    public UseItem LeftClickUp;

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
        if (DataDex.instance.DisplayDataPad)
            return;
        // Select ITEM
        if (Input.GetKeyDown(KeyCode.Alpha1) && GameManager.instance.player_progress.Melee.isUnlocked)
        {
            LeftClickUp = GameManager.instance.playerShootScript.StopFistsAnimation;
            

            EquipItem(ItemHolding.Melee, GameManager.instance.playerShootScript.Shoot_Melee,
                GameManager.instance.player_progress.Melee.ItemInstance.GetComponent<RawImage>());

        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && GameManager.instance.player_progress.DataBall.isUnlocked)
        {
            EquipItem(ItemHolding.DataBall,GameManager.instance.playerShootScript.Shoot_Databall, 
                GameManager.instance.player_progress.DataBall.ItemInstance.GetComponent<RawImage>());

        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && GameManager.instance.player_progress.Command.isUnlocked)
        {
            
            EquipItem(ItemHolding.Command, DataMonCommand.Patrol,
                GameManager.instance.player_progress.Command.ItemInstance.GetComponent<RawImage>());

        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && GameManager.instance.player_progress.HuntingRifle.isUnlocked)
        {
            EquipItem(ItemHolding.HuntingRifle, GameManager.instance.playerShootScript.Shoot_HuntingRifle,
                GameManager.instance.player_progress.HuntingRifle.ItemInstance.GetComponent<RawImage>());

        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && GameManager.instance.player_progress.Shotgun.isUnlocked)
        {
            EquipItem(ItemHolding.Shotgun, GameManager.instance.playerShootScript.Shoot_Shotgun,
                GameManager.instance.player_progress.Shotgun.ItemInstance.GetComponent<RawImage>());

        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && GameManager.instance.player_progress.AssaultRifle.isUnlocked)
        {
            EquipItem(ItemHolding.AssaultRifle, GameManager.instance.playerShootScript.Shoot_AssaultRifle,
                GameManager.instance.player_progress.AssaultRifle.ItemInstance.GetComponent<RawImage>());

        }

        // =====================================================================================================
        switch (holdingItem)
        {
            case ItemHolding.Melee:
                CheckInput(KeyCode.Mouse0, LeftClick);
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    LeftClickUp();
                }
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    CameraFollow.isAiming = true;
                }
                if (Input.GetKeyUp(KeyCode.Mouse1))
                {

                    CameraFollow.isAiming = false;
                }
                break;
            case ItemHolding.Command:
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

    private void EquipItem(ItemHolding item, System.Action action, RawImage ItemImage)
    {
        holdingItem = item;
        LeftClick = () => action();

        SetColorAllItems();
        ItemImage.color = SelectedColor;
        GameManager.instance.playerShootScript.ShowWeaponModel();
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
