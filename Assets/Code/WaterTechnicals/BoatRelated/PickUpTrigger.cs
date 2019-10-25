using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUpTrigger : MonoBehaviour
{
    public delegate void OnAddPoints(int value);
    public OnAddPoints onAddPoints;

    public delegate void OnHealDamage(int value);
    public OnHealDamage onHealDamage;

    public delegate void OnLowerTime(int value);
    public OnLowerTime onLowerTime;

    [SerializeField] ParticleSystem vfxSystem;
    private Transform effectsPool;

    private void Start()
    {
        effectsPool = GameObject.Find("EffectsPool").transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            ItemEffect[] type = other.gameObject.GetComponents<ItemEffect>();
            other.gameObject.SetActive(false);
            
            for(int i=0; i<type.Length; i++)
            {
                if (type[i].itemEffect.ToString() == "AddPoints")
                    onAddPoints(type[i].amount);
                else if (type[i].itemEffect.ToString() == "HealDamage")
                    onHealDamage(type[i].amount);
                else if (type[i].itemEffect.ToString() == "LowerTime")
                    onLowerTime(type[i].amount);
            }

            Destroy(other.gameObject);

            GetComponent<AudioSource>().Play();

            ParticleSystem ps = Instantiate(vfxSystem, other.transform.position, Quaternion.identity) as ParticleSystem;
            ps.transform.SetParent(effectsPool);
            Destroy(ps.gameObject, 10);
        }
    }
}
