using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DataCraft : MonoBehaviour
{
    public static DataCraft instance;
    public SelectedItemToCraft SelectedtoCraft;
    int CraftingQuantity, DataBytesReq;
    public bool isEditingCraftingQuantity = false, DisplayDataPad;
    public GameObject CraftSelectedPanel, DataPad;
    public TMP_InputField CurrentQuantityToCraft;
    public TextMeshProUGUI DataBytesRequiredText, CurrentDataBytesAmount, ButtonText;

    public ItemsCraftClass[] ItemsCrafts;
    ItemsCraftClass DataBall, HuntingRifle, Shotgun, AssaultRifle;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        for (int i = 0; i < ItemsCrafts.Length; i++)
        {
            switch (ItemsCrafts[i].item)
            {
                case SelectedItemToCraft.DataBall:
                    DataBall = ItemsCrafts[i];
                    break;
                case SelectedItemToCraft.HuntingRifle:
                    HuntingRifle = ItemsCrafts[i];
                    break;
                case SelectedItemToCraft.Shotgun:
                    Shotgun = ItemsCrafts[i];
                    break;
                case SelectedItemToCraft.AssaultRifle:
                    AssaultRifle = ItemsCrafts[i];
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DisplayDataPad = !DisplayDataPad;
        }
        if (DisplayDataPad)
            CameraFollow.isAiming = false;
        DataPad.SetActive(DisplayDataPad);

        DataBall.UpdateUI(GameManager.instance.DataBallLauncher.AmmoAmount);
        //if (GameManager.instance.player_progress.HuntingRifle.isUnlocked)
        //{
        //    HuntingRifle.UpdateUI("Tier: " + GameManager.instance.player_progress.HuntingRifle.WeaponModifiers.CurrentTier);

            
        //}
        //else
            HuntingRifle.UpdateUI(GameManager.instance.huntingRifle.AmmoAmount);
        //if (GameManager.instance.player_progress.Shotgun.isUnlocked)
        //{
        //    Shotgun.UpdateUI("Tier: " + GameManager.instance.player_progress.Shotgun.WeaponModifiers.CurrentTier);

        //}
        //else
            Shotgun.UpdateUI(GameManager.instance.huntingRifle.AmmoAmount);
        //if (GameManager.instance.player_progress.AssaultRifle.isUnlocked)
        //{
        //    AssaultRifle.UpdateUI("Tier: "+GameManager.instance.player_progress.AssaultRifle.WeaponModifiers.CurrentTier);

        //}
        //else
            AssaultRifle.UpdateUI(GameManager.instance.huntingRifle.AmmoAmount);

        if (SelectedtoCraft == SelectedItemToCraft.None)
        {
            CraftSelectedPanel.SetActive(false);
            return;
        }

        CraftSelectedPanel.SetActive(true);
        UpdateDataCraft();
    }

    void UpdateDataCraft()
    {
        if (!isEditingCraftingQuantity)
            CurrentQuantityToCraft.text = CraftingQuantity.ToString();

        switch (SelectedtoCraft)
        {
            case SelectedItemToCraft.DataBall:
                if(GameManager.instance.Databytes != 0)
                CraftingQuantity = 
                    Mathf.Clamp(CraftingQuantity, 0, GameManager.instance.Databytes / DataBall.craftRecipe.DataBytesRequired);
                DataBytesReq = DataBall.craftRecipe.DataBytesRequired * CraftingQuantity;


                ButtonText.text = "Craft";

                break;
            case SelectedItemToCraft.HuntingRifle:
                if (GameManager.instance.Databytes != 0)
                    CraftingQuantity =
                    Mathf.Clamp(CraftingQuantity, 0, GameManager.instance.Databytes / HuntingRifle.craftRecipe.DataBytesRequired);
                //DataBytesReq = HuntingRifle.craftRecipe.DataBytesRequired * CraftingQuantity;

                if (GameManager.instance.player_progress.HuntingRifle.isUnlocked)
                {
                    DataBytesReq = HuntingRifle.craftRecipe.DataBytesRequired * CraftingQuantity;
                    ButtonText.text = "Ammo";
                }
                else
                    ButtonText.text = "Craft";
                break;
            case SelectedItemToCraft.Shotgun:

                if (GameManager.instance.Databytes != 0)
                    CraftingQuantity =
                    Mathf.Clamp(CraftingQuantity, 0, GameManager.instance.Databytes / Shotgun.craftRecipe.DataBytesRequired );
                //DataBytesReq = Shotgun.craftRecipe.DataBytesRequired * CraftingQuantity;

                if (GameManager.instance.player_progress.Shotgun.isUnlocked)
                {
                    DataBytesReq = Shotgun.craftRecipe.DataBytesRequired * CraftingQuantity;

                    ButtonText.text = "Ammo";
                }
                else
                    ButtonText.text = "Craft";
                break;
            case SelectedItemToCraft.AssaultRifle:

                if (GameManager.instance.Databytes != 0)
                    CraftingQuantity =
                    Mathf.Clamp(CraftingQuantity, 0, GameManager.instance.Databytes / AssaultRifle.craftRecipe.DataBytesRequired);
                //DataBytesReq = AssaultRifle.craftRecipe.DataBytesRequired * CraftingQuantity;

                if (GameManager.instance.player_progress.AssaultRifle.isUnlocked)
                {
                    DataBytesReq = AssaultRifle.craftRecipe.DataBytesRequired * CraftingQuantity;
                    ButtonText.text = "Ammo";
                }
                else
                    ButtonText.text = "Craft";

                break;
        }
        DataBytesRequiredText.text = DataBytesReq.ToString();
        CurrentDataBytesAmount.text = GameManager.instance.Databytes.ToString();
    }
    public void CraftItem()
    {
        switch (SelectedtoCraft)
        {
            case SelectedItemToCraft.DataBall:

                if (GameManager.instance.Databytes < DataBytesReq)
                    return;
                GameManager.instance.Databytes -= DataBytesReq;

                GameManager.instance.DataBallLauncher.AmmoAmount+= CraftingQuantity;
                if (!GameManager.instance.player_progress.DataBall.isUnlocked)
                {
                    GameManager.instance.player_progress.DataBall.isUnlocked = true;
                }
                    break;
            case SelectedItemToCraft.HuntingRifle:

                if (GameManager.instance.Databytes < DataBytesReq)
                    return;
                print("crafted Meet Req");

                GameManager.instance.huntingRifle.AmmoAmount += CraftingQuantity;




                GameManager.instance.Databytes -= DataBytesReq;
                break;
            case SelectedItemToCraft.Shotgun:

                if (GameManager.instance.Databytes < DataBytesReq)
                    return;

                GameManager.instance.shotgun.AmmoAmount += CraftingQuantity;

                GameManager.instance.Databytes -= DataBytesReq;

                break;
            case SelectedItemToCraft.AssaultRifle:
                if (GameManager.instance.Databytes < DataBytesReq)
                    return;


                GameManager.instance.shotgun.AmmoAmount += CraftingQuantity;

                GameManager.instance.Databytes -= DataBytesReq;
                break;
        }
    }

    public static void ObtainWeapon(Item PlayerWeapon, WeaponType weaponType)
    {
        if (!PlayerWeapon.isUnlocked)
        {
            PlayerWeapon.isUnlocked = true;
            weaponType.CurrentClipAmount = 999;
        }
    }

    public void IncreaseQuantity()
    {
        CraftingQuantity++;

    }
    public void DecreaseQuantity()
    {
        CraftingQuantity--;
    }
    public void EndEditQuantityText()
    {
        try
        {
            int temp = int.Parse(CurrentQuantityToCraft.text);
            CraftingQuantity = temp;
            isEditingCraftingQuantity = false;
        }
        catch (System.FormatException)
        {
            CurrentQuantityToCraft.text = 0.ToString();
            CraftingQuantity = 0;
            isEditingCraftingQuantity = false;
        }
    }
    public void OnEditQuantityText()
    {
        isEditingCraftingQuantity = true;

    }
    public void Select_Databall()
    {

        SelectedtoCraft = SelectedItemToCraft.DataBall;

        if (GameManager.instance.Databytes < DataBall.craftRecipe.DataBytesRequired)
            CraftingQuantity = 0;
        else
            CraftingQuantity = 1;
    }
    public void Select_Huntingrifle()
    {
        SelectedtoCraft = SelectedItemToCraft.HuntingRifle;

        if (GameManager.instance.Databytes < HuntingRifle.craftRecipe.DataBytesRequired)
            CraftingQuantity = 0;
        else
            CraftingQuantity = 1;
    }
    public void Select_Shotgun()
    {
        SelectedtoCraft = SelectedItemToCraft.Shotgun;

        if (GameManager.instance.Databytes < Shotgun.craftRecipe.DataBytesRequired)
            CraftingQuantity = 0;
        else
            CraftingQuantity = 1;
    }
    public void Select_Assaultrifle()
    {
        SelectedtoCraft = SelectedItemToCraft.AssaultRifle;

        if (GameManager.instance.Databytes < AssaultRifle.craftRecipe.DataBytesRequired)
            CraftingQuantity = 0;
        else
            CraftingQuantity = 1;
    }
}

public enum SelectedItemToCraft
{
    DataBall, HuntingRifle, Shotgun, AssaultRifle, None
}
[System.Serializable]
public class ItemsCraftClass
{
    public string name;
    public SelectedItemToCraft item;
    public GameObject PickupItem;
    public ItemCraftRecipe craftRecipe;
    public ItemCraftingUI craftingUI;
    public void UpdateUI(int Quantity)
    {
        craftingUI.SetTexts(Quantity, craftRecipe.DataBytesRequired);
    }
    public void UpdateUI(string Quantity)
    {
        craftingUI.SetTexts(Quantity, craftRecipe.DataBytesRequired);
    }
}
[System.Serializable]
public class ItemCraftRecipe
{
    public int DataBytesRequired;
    public bool isDataMonRequired;
    public DataMonsData dataMonsData;
    public float ProductionTimeReq;
    public int DataMonTierRequired;
    public float GlitchDamageToCraft = 2;
}
[System.Serializable]
public class ItemCraftingUI
{
    public TextMeshProUGUI QuantityOfItem,txt_DataBytesReq, txt_Req1, txt_Req2;
    public void SetTexts(int Quantity, int DataBytesReq)
    {
        QuantityOfItem.text = "x" + Quantity;
        txt_DataBytesReq.text = "Req. x" + DataBytesReq;
        //txt_Req1.text = "";
        //txt_Req2.text = "";
    }
    public void SetTexts(string Quantity, int DataBytesReq)
    {
        QuantityOfItem.text = Quantity;
        txt_DataBytesReq.text = "Req. x" + DataBytesReq;
        //txt_Req1.text = "";
        //txt_Req2.text = "";
    }
    public void SetTexts(int Quantity, int DataBytesReq, string Required1)
    {
        QuantityOfItem.text = "x" + Quantity;
        txt_DataBytesReq.text = "Req. x" + DataBytesReq;
        txt_Req1.text = Required1;
        //txt_Req2.text = "";
    }
    public void SetTexts(int Quantity, int DataBytesReq, string Required1, string Required2)
    {
        QuantityOfItem.text = "x" + Quantity;
        txt_DataBytesReq.text = "Req. x" + DataBytesReq;
        txt_Req1.text = Required1;
        txt_Req2.text = Required2;
    }
}