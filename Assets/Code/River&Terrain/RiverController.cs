using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverController : MonoBehaviour
{
    public RiverObject riverAsset;

    public Rigidbody playerRigidbody;

    [Header("MeshWaves")]
    [Range(0.0f, 1.0f)] public float modifyFreq = 0.75f;
    [Range(0.0f, 10.0f)] public float waveheight = 0.25f;

    [Header("Floating")]
    public SystemTypes usedSystemType;
    public enum SystemTypes { Arcade, Physics }
    
    public float arcadeBouance = 100;
    public float physicsBouance = 75000;

    [Header("Flow")]
    public int minimumSpeed;
    public Vector3 movementDirection;

    [Header("SplashEffect")]
    public ParticleSystem onImpactEffect = null;
    public AudioSource onImpactSound = null;

    private Transform effectsPool;
    private Mesh mesh;

    private void Start()
    {
        bool allWorking = true;

        mesh = riverAsset.GetMesh();

        if(mesh == null)
        {
            Debug.LogWarning("Mesh missing from RiverObject");
            allWorking = false;
        }

        GameObject pool = GameObject.Find("EffectsPool");
        if (pool != null)
            effectsPool = pool.transform;
        else
            effectsPool = new GameObject("EffectsPool").transform;

        if (allWorking)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh = mesh;
            }

            if (playerRigidbody == null)
                playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

            Vector3 movement = movementDirection;
            movement *= (minimumSpeed);

            //Use rb.velocity to set a specific speed
            playerRigidbody.velocity = movement;
        }
        else
        {
            Debug.LogError("Oh No! Something is wrong, dont worry. I stoped the game so you can fix it in ease.");
            if (Application.isEditor)
                Debug.DebugBreak();
        }
    }

    private void Update()
    {
        PhysicsFlowUpdate();

        MeshWaveUpdate();
    }

    void PhysicsFlowUpdate()
    {
        RiverNode node = riverAsset.GetNodeFromPosition(playerRigidbody.transform.position);

        Vector3 movement = node.flowDirection;
        movement *= (minimumSpeed);

        //Use rb.AddForce to gradually increase or decrease speed
        //   Giving it 0.1f leeway so that the boat won't start going back and forth between 24.97f and 25.002f
        if (playerRigidbody.velocity.x < minimumSpeed - 0.1f)
            playerRigidbody.AddForce(movement * (minimumSpeed * Time.deltaTime));
        else if (playerRigidbody.velocity.x > minimumSpeed + 0.1f)
            playerRigidbody.AddForce(-movement * (minimumSpeed * Time.deltaTime));
        else
            playerRigidbody.velocity = movement;
    }

    void MeshWaveUpdate ()
    {
        if (Random.value > modifyFreq)
            return;

        Vector3[] verts = mesh.vertices;

        for (int i = 0; i < verts.Length; i++)
        {
            float targetHeight = Random.value * waveheight;
            float oldHeight = verts[i].y;
            float newHeight = Mathf.Lerp(oldHeight, targetHeight, 0.5f);
            verts[i] = new Vector3(verts[i].x, newHeight, verts[i].z);
        }

        mesh.RecalculateNormals();

        mesh.vertices = verts;
    }

    void PhysicsFloatingUpdate (Collider other)
    {
        if (usedSystemType == SystemTypes.Physics)
        {
            float distance = 1;// collider.bounds.SqrDistance(other.transform.position);
            distance = Mathf.Abs(other.transform.position.y - transform.position.y);

            other.attachedRigidbody.AddForce(Vector3.up * (physicsBouance / other.attachedRigidbody.mass) * distance * Time.fixedDeltaTime);

            Debug.DrawRay(other.transform.position, Vector3.up * ((physicsBouance / other.attachedRigidbody.mass) * distance) / 2500, Color.blue);
            //Debug.Log(other.name + " is colliding with water surface");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
            other.GetComponent<Rigidbody>().drag = 2;

        if (other.gameObject.layer == LayerMask.NameToLayer("Boat"))
            return;

        ParticleSystem particle = Instantiate(onImpactEffect, other.transform.position, Quaternion.identity) as ParticleSystem;
        particle.transform.SetParent(effectsPool, true);
        Destroy(particle.gameObject, particle.main.duration);

        AudioSource audio = Instantiate(onImpactSound, other.transform.position, Quaternion.identity) as AudioSource;
        audio.transform.SetParent(effectsPool, true);
        Destroy(audio.gameObject, audio.clip.length);
    }

    private void OnTriggerStay(Collider other)
    {
        PhysicsFloatingUpdate(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
            other.GetComponent<Rigidbody>().drag = 1;
    }
}
