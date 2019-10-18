using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public enum ItemEffects { AddPoints, HealDamage, LowerTime }
    public ItemEffects itemEffect = 0;

    public int amount = 1;
}
