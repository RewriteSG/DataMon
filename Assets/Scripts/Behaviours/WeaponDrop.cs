using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    public SelectedItemToCraft ItemDrop;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (ItemDrop)
            {
                case SelectedItemToCraft.HuntingRifle:

                    DataCraft.ObtainWeapon(GameManager.instance.player_progress.HuntingRifle, GameManager.instance.huntingRifle);
                    break;
                case SelectedItemToCraft.Shotgun:

                    DataCraft.ObtainWeapon(GameManager.instance.player_progress.Shotgun, GameManager.instance.shotgun);
                    break;
                case SelectedItemToCraft.AssaultRifle:

                    DataCraft.ObtainWeapon(GameManager.instance.player_progress.AssaultRifle, GameManager.instance.assaultRifle);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
