using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeOnDamage : MonoBehaviour
{
    [SerializeField] BoatClass boat;

    [SerializeField] float force = 0.25f;
    [SerializeField] float duration = 1.0f;

    bool isShaking = false;

    private void Start()
    {
        boat.onDamaged += Shake;
    }

    public void Shake (float impactForce, Vector3 point)
    {
        if (isShaking)
            return;
        StartCoroutine("TimedShake", impactForce);
    }

    IEnumerator TimedShake(float impact)
    {
        isShaking = true;
        float t = 0;
        while(t < duration)
        {
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            transform.Rotate(new Vector3(Random.Range(-1.0f,1.0f) * (force * impact), Random.Range(-1.0f, 1.0f) * (force * impact), Random.Range(-1.0f, 1.0f) * (force * impact)));
        }
        isShaking = false;
    }
}
