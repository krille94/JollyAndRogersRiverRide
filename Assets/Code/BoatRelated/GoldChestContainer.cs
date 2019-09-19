using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldChestContainer : MonoBehaviour
{
    [SerializeField] GameObject Quad_obj = null;
    [SerializeField] Transform Quad_maxHeight = null;
    [SerializeField] Transform Quad_minHeight = null;

    public int gold;
    public int maxGold = 10;

    [SerializeField] PickUpTrigger trigger = null;
    [SerializeField] BoatDamageController controller = null;

    [Range(0.0f,1.0f)]public float onImpactLostValue = 0.25f;

    private void Start()
    {
        trigger.onPickUp += AddGoldAndUpdateModel;
        controller.onDamaged += RemoveGoldAndUpdateModel;
    }

    public void AddGoldAndUpdateModel (int amount)
    {
        gold += amount;
        if (gold > maxGold)
            gold = maxGold;

        YourScore.bonusesPickedUp = gold;
        UpdateModel();
    }

    public void UpdateModel ()
    {
        float level = (float)gold / (float)maxGold;
        //Debug.Log("gold value percent: " + level + "%");
        Quad_obj.transform.position = Vector3.Lerp(Quad_minHeight.position, Quad_maxHeight.position, level);
    }

    public void RemoveGoldAndUpdateModel(float impactForce, Vector3 point)
    {
        timedDamage += impactForce;
        if (!timedDamageActive)
            StartCoroutine("RecivedDamage");
    }

    float timedDamage = 0;
    bool timedDamageActive = false;
    IEnumerator RecivedDamage()
    {
        timedDamageActive = true;
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        gold -= Mathf.RoundToInt(timedDamage * onImpactLostValue);
        if (gold < 0)
            gold = 0;
        UpdateModel();
        YourScore.bonusesPickedUp = gold;
        timedDamageActive = false;
    }
}
