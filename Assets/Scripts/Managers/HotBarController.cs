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

        ItemsInHotbar.Add(new ItemInHotBar(ItemHolding.Melee,
                GameManager.instance.player_progress.Melee.ItemPrefab,
                GameManager.instance.playerShootScript.Shoot_Melee, GameManager.instance.playerShootScript.StopFistsAnimation));

        ItemsInHotbar.Add(new ItemInHotBar(ItemHolding.HuntingRifle,
                GameManager.instance.player_progress.HuntingRifle.ItemPrefab,
                GameManager.instance.playerShootScript.Shoot_HuntingRifle,
                GameManager.instance.playerShootScript.StopShooting));

        ItemsInHotbar.Add(new ItemInHotBar(ItemHolding.Shotgun,
                GameManager.instance.player_progress.Shotgun.ItemPrefab,
                GameManager.instance.playerShootScript.Shoot_Shotgun,
                GameManager.instance.playerShootScript.StopShooting));

        ItemsInHotbar.Add(new ItemInHotBar(ItemHolding.AssaultRifle,
                GameManager.instance.player_progress.AssaultRifle.ItemPrefab,
                GameManager.instance.playerShootScript.Shoot_AssaultRifle, 
                GameManager.instance.playerShootScript.StopShooting));

        for (int i = 0; i < GameManager.instance.DataMonAbilities.Count; i++)
        {
        Debug.Log("HhAaaAAAaa");
            GameManager.instance.DataMonAbilities[i].Activate(GameManager.instance.DataTeam[i].dataMonData, GameManager.instance.DataTeam[i].dataMon
                , GameManager.instance, true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if(GameManager.instance.player_progress.Melee.ItemInstance==null && GameManager.instance.player_progress.Melee.isUnlocked)
        //{
        //    GameManager.instance.player_progress.Melee.InstantiatePrefab(transform);

            
        //}
        //if (GameManager.instance.player_progress.DataBall.isUnlocked)
        //{
        //    GameManager.instance.player_progress.DataBall.InstantiatePrefab(transform); 

        //}
        //if (GameManager.instance.player_progress.HuntingRifle.ItemInstance == null && GameManager.instance.player_progress.HuntingRifle.isUnlocked)
        //{
        //    GameManager.instance.player_progress.HuntingRifle.InstantiatePrefab(transform); 

            
        //}
        //if (GameManager.instance.player_progress.Shotgun.ItemInstance == null && GameManager.instance.player_progress.Shotgun.isUnlocked)
        //{
        //    GameManager.instance.player_progress.Shotgun.InstantiatePrefab(transform);

            
        //}
        //if (GameManager.instance.player_progress.AssaultRifle.ItemInstance == null && GameManager.instance.player_progress.AssaultRifle.isUnlocked)
        //{
        //    GameManager.instance.player_progress.AssaultRifle.InstantiatePrefab(transform);

            

        //}
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

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {

            selectedItem = 6;
            //EquipItem(ItemHolding.AssaultRifle, GameManager.instance.playerShootScript.Shoot_AssaultRifle,
            //    GameManager.instance.player_progress.AssaultRifle.ItemInstance.GetComponent<RawImage>());

        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {

            selectedItem = 7;
            //EquipItem(ItemHolding.AssaultRifle, GameManager.instance.playerShootScript.Shoot_AssaultRifle,
            //    GameManager.instance.player_progress.AssaultRifle.ItemInstance.GetComponent<RawImage>());

        }

        // =====================================================================================================
        if (selectedItem > ItemsInHotbar.Count)
            return;
        if(prevSelectedItem!= selectedItem)
        {
            ItemsInHotbar[prevSelectedItem == -1 ? 0 : prevSelectedItem].UnselectingItem();
            ItemsInHotbar[selectedItem].SelectingItem();
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && ItemsInHotbar[selectedItem].item == ItemHolding.DataBall)
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

    }
    //int sortOrder;
    public void AddItemInHotBar(ItemInHotBar item)
    {

        //sortOrder = 0;
        //if (GameManager.instance.player_progress.Melee.isUnlocked)
        //{
        //    GameManager.instance.player_progress.Melee.ItemInstance.transform.SetSiblingIndex(sortOrder);
        //    sortOrder++;
        //}
        //if (GameManager.instance.player_progress.DataBall.isUnlocked)
        //{
        //    GameManager.instance.player_progress.DataBall.ItemInstance.transform.SetSiblingIndex(sortOrder);
        //    sortOrder++;
        //}
        //if (GameManager.instance.player_progress.Command.isUnlocked)
        //{
        //    GameManager.instance.player_progress.Command.ItemInstance.transform.SetSiblingIndex(sortOrder);
        //    sortOrder++;
        //}
        //if (GameManager.instance.player_progress.HuntingRifle.isUnlocked)
        //{
        //    GameManager.instance.player_progress.HuntingRifle.ItemInstance.transform.SetSiblingIndex(sortOrder);
        //    sortOrder++;
        //}
        //if (GameManager.instance.player_progress.Shotgun.isUnlocked)
        //{
        //    GameManager.instance.player_progress.Shotgun.ItemInstance.transform.SetSiblingIndex(sortOrder);
        //    sortOrder++;
        //}
        //if (GameManager.instance.player_progress.AssaultRifle.isUnlocked)
        //{
        //    GameManager.instance.player_progress.AssaultRifle.ItemInstance.transform.SetSiblingIndex(sortOrder);
        //    sortOrder++;
        //}
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
    public void ShowDifferentCommands()
    {

    }
    public void CommandDataMons()
    {

    }

}
public enum ItemHolding
{
    Command, Melee, DataBall, HuntingRifle, Shotgun, AssaultRifle, DataMonSemiAuto ,DataMonAuto, None
}
public class ItemInHotBar
{
    public HotBarController.UseItem useItem { private get; set; }
    public HotBarController.UseItem useItemUp { private get; set; }
    public HotBarController.UseItem SelectedItem { private get; set; }
    public HotBarController.UseItem UnselectItem { private get; set; }


    public ItemHolding item;
    public RawImage ItemImage;
    public ItemInHotBar() { }

    public ItemInHotBar(ItemHolding _item, GameObject _ItemImage, HotBarController.UseItem _useItem = null, HotBarController.UseItem _useItemUp=null,
        HotBarController.UseItem _SelectedItem = null, HotBarController.UseItem _UnselectItem = null)
    {
        item = _item;
        ItemImage = Object.Instantiate(_ItemImage, GameManager.instance.hotBarController.transform).GetComponent<RawImage>();
        useItem = _useItem;
        useItemUp = _useItemUp;
        SelectedItem = _SelectedItem;
        UnselectItem = _UnselectItem;
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
    public void UnselectingItem()
    {
        if (UnselectItem == null)
            return;
        UnselectItem();
    }
    public void SelectingItem()
    {

        if (SelectedItem == null)
            return;
        SelectedItem();
    }
}