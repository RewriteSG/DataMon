using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commands;
public class DataMonCommand : MonoBehaviour
{
    [SerializeField] DataMonCommands SerializedCommand;
    public static DataMonCommands command;
    public static GameObject ToTarget;
    static Vector3 gotoCoord;
    private void Update()
    {
        SerializedCommand = command;
    }
    public static void TargetEnemy()
    {
        command = DataMonCommands.TargetEnemy;
    }
    public static void DontAttack()
    {
        command = DataMonCommands.DontAttack;
    }
    public static void AttackAggressive()
    {
        command = DataMonCommands.AttackAggressive;
    }
    public static void Patrol()
    {
        command = DataMonCommands.Patrol;
    }
    public static void GoTo()
    {
        command = DataMonCommands.Patrol;
    }
}
namespace Commands
{
    public enum DataMonCommands
    {
        TargetEnemy, DontAttack, AttackAggressive, Patrol, GoTo
    }

}
