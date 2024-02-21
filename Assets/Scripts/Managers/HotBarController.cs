using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System;
public class HotBarController : MonoBehaviour
{
    public delegate void UseItem();
    public UseItem LeftClick;
    public UseItem LeftClickUp;

    public UseItem RightClick;
    public Color SelectedColor;
    public Color UnselectedColor;

    public static List<ItemInHotBar> ItemsInHotbar = new List<ItemInHotBar>();

    public static int selectedItem;
    public int prevSelectedItem;

    // Start is called before the first frame update
    void Start()
    {
        prevSelectedItem = -1;
        ItemsInHotbar.Clear();
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.player_progress.Melee.ItemInstance==null && GameManager.instance.player_progress.Melee.isUnlocked)
        {
            GameManager.instance.player_progress.Melee.InstantiatePrefab(transform);

            ItemsInHotbar.Add(new ItemInHotBar(ItemHolding.Melee, 
                GameManager.instance.player_progress.Melee.ItemInstance.GetComponent<RawImage>(),
                GameManager.instance.playerShootScript.Shoot_Melee, GameManager.instance.playerShootScript.StopFistsAnimation));

        }
        if (GameManager.instance.player_progress.DataBall.ItemInstance == null && GameManager.instance.player_progress.DataBall.isUnlocked)
        {
            GameManager.instance.player_progress.DataBall.InstantiatePrefab(transform); 

            ItemsInHotbar.Add(new ItemInHotBar(ItemHolding.DataBall,
                GameManager.instance.player_progress.DataBall.ItemInstance.GetComponent<RawImage>(),
                GameManager.instance.playerShootScript.Shoot_Databall, null));
        }
        //if (GameManager.instance.player_progress.Command.ItemInstance == null && GameManager.instance.player_progress.Command.isUnlocked)
        //{
        //    GameManager.instance.player_progress.Command.InstantiatePrefab(transform); 
            
        //    ItemsInHotbar.Add(new ItemInHotBar(ItemHolding.Melee,
        //        GameManager.instance.player_progress.Melee.ItemInstance.GetComponent<RawImage>(),
        //        GameManager.instance.playerShootScript.Shoot_Melee, null));

        //}
        if (GameManager.instance.player_progress.HuntingRifle.ItemInstance == null && GameManager.instance.player_progress.HuntingRifle.isUnlocked)
        {
            GameManager.instance.player_progress.HuntingRifle.InstantiatePrefab(transform); 

            ItemsInHotbar.Add(new ItemInHotBar(ItemHolding.HuntingRifle,
                GameManager.instance.player_progress.HuntingRifle.ItemInstance.GetComponent<RawImage>(),
                GameManager.instance.playerShootScript.Shoot_HuntingRifle, null));
        }
        if (GameManager.instance.player_progress.Shotgun.ItemInstance == null && GameManager.instance.player_progress.Shotgun.isUnlocked)
        {
            GameManager.instance.player_progress.Shotgun.InstantiatePrefab(transform);

            ItemsInHotbar.Add(new ItemInHotBar(ItemHolding.Shotgun,
                GameManager.instance.player_progress.Shotgun.ItemInstance.GetComponent<RawImage>(),
                GameManager.instance.playerShootScript.Shoot_Shotgun, null));
        }
        if (GameManager.instance.player_progress.AssaultRifle.ItemInstance == null && GameManager.instance.player_progress.AssaultRifle.isUnlocked)
        {
            GameManager.instance.player_progress.AssaultRifle.InstantiatePrefab(transform);

            ItemsInHotbar.Add(new ItemInHotBar(ItemHolding.AssaultRifle,
                GameManager.instance.player_progress.AssaultRifle.ItemInstance.GetComponent<RawImage>(),
                GameManager.instance.playerShootScript.Shoot_AssaultRifle, null));

        }
        //if (isUnsorted)
        //{
        //    SortHotBar();
        //    isUnsorted = false;
        //}
        if (DataCraft.instance.DisplayDataPad)
            return;
        // Select ITEM
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedItem = 0;
            //LeftClickUp = GameManager.instance.playerShootScript.StopFistsAnimation;

            //print("huh");
            //EquipItem(ItemHolding.Melee, GameManager.instance.playerShootScript.Shoot_Melee,
            //    GameManager.instance.player_progress.Melee.ItemInstance.GetComponent<RawImage>());

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            selectedItem = 1;
            //EquipItem(ItemHolding.DataBall,GameManager.instance.playerShootScript.Shoot_Databall, 
            //    GameManager.instance.player_progress.DataBall.ItemInstance.GetComponent<RawImage>());

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            selectedItem = 2;
            //EquipItem(ItemHolding.Command, DataMonCommand.Patrol,
            //    GameManager.instance.player_progress.Command.ItemInstance.GetComponent<RawImage>());

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {


            selectedItem = 3;
            //EquipItem(ItemHolding.HuntingRifle, GameManager.instance.playerShootScript.Shoot_HuntingRifle,
            //    GameManager.instance.player_progress.HuntingRifle.ItemInstance.GetComponent<RawImage>());

        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {

            selectedItem = 4;
            //EquipItem(ItemHolding.Shotgun, GameManager.instance.playerShootScript.Shoot_Shotgun,
            //    GameManager.instance.player_progress.Shotgun.ItemInstance.GetComponent<RawImage>());

        }
        if (Input.GetKeyDown(KeyCode.Alpha6) )
        {

            selectedItem = 5;
            //EquipItem(ItemHolding.AssaultRifle, GameManager.instance.playerShootScript.Shoot_AssaultRifle,
            //    GameManager.instance.player_progress.AssaultRifle.ItemInstance.GetComponent<RawImage>());

        }

        // =====================================================================================================
        if(prevSelectedItem!= selectedItem)
        {
            prevSelectedItem = selectedItem;
            for (int i = 0; i < ItemsInHotbar.Count; i++)
            {
                if (i == prevSelectedItem)
                    ItemsInHotbar[i].ItemImage.color = SelectedColor;
                else

                    ItemsInHotbar[i].ItemImage.color = UnselectedColor;
            }
        }
        GameManager.instance.playerShootScript.ShowWeaponModel(ItemsInHotbar[selectedItem].item);
        if (Input.GetKey(KeyCode.Mouse0) && ItemsInHotbar[selectedItem].item != ItemHolding.DataBall)
        {
            ItemsInHotbar[selectedItem].CallUseItem();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            ItemsInHotbar[selectedItem].CallUseItemUp();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && ItemsInHotbar[selectedItem].item != ItemHolding.DataBall)
        {
            ItemsInHotbar[selectedItem].CallUseItem();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            CameraFollow.isAiming = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {

            CameraFollow.isAiming = false;
        }
        //switch (holdingItem)
        //{
        //    case ItemHolding.Melee:
        //        //CheckInput(KeyCode.Mouse0, LeftClick);
        //        if (Input.GetKeyUp(KeyCode.Mouse0))
        //        {
        //            LeftClickUp();
        //        }
        //        break;
        //    case ItemHolding.Command:
        //    case ItemHolding.DataBall:
        //        //CheckInputDown(KeyCode.Mouse0,LeftClick);
        //        if (Input.GetKeyDown(KeyCode.Mouse1))
        //        {
        //            CameraFollow.isAiming = true;
        //        }
        //        if (Input.GetKeyUp(KeyCode.Mouse1))
        //        {

        //            CameraFollow.isAiming = false;
        //        }
        //        break;
        //    case ItemHolding.Shotgun:
        //    case ItemHolding.HuntingRifle:
        //    case ItemHolding.AssaultRifle:
        //        //CheckInput(KeyCode.Mouse0, LeftClick);
        //        if (Input.GetKeyDown(KeyCode.Mouse1))
        //        {
        //            CameraFollow.isAiming = true;
        //        }
        //        if (Input.GetKeyUp(KeyCode.Mouse1))
        //        {

        //            CameraFollow.isAiming = false;
        //        }
        //        break;
        //    default:
        //        break;
        //}
    }

    //private void EquipItem(ItemHolding item, System.Action action, RawImage ItemImage)
    //{
    //    holdingItem = item;
    //    LeftClick = () => action();

    //    SetColorAllItems();
    //    ItemImage.color = SelectedColor;
    //    GameManager.instance.playerShootScript.ShowWeaponModel();
    //}

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
public class ItemInHotBar
{
    public HotBarController.UseItem useItem;
    public HotBarController.UseItem useItemUp;

    public ItemHolding item;
    public RawImage ItemImage;
    public ItemInHotBar() { }

    public ItemInHotBar(ItemHolding _item, RawImage _ItemImage, HotBarController.UseItem _useItem, HotBarController.UseItem _useItemUp)
    {
        item = _item;
        ItemImage = _ItemImage;
        useItem = _useItem;
        useItemUp = _useItemUp;
    }
    public void CallUseItem() 
    {
        if (useItem == null)
            return;
        useItem();
    }
    public void CallUseItemUp()
    {
        if (useItemUp == null)
            return;
        useItemUp();
    }
}