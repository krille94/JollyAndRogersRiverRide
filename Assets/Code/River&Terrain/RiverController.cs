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

           /// Vector3 movement = movementDirection;
            //movement *= (minimumSpeed);

            //Use rb.velocity to set a specific speed
            //playerRigidbody.velocity = movement;
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

    [SerializeField]
    Vector3 flow;
    [SerializeField]
    RiverNode node;
    void PhysicsFlowUpdate()
    {
        node = riverAsset.GetNodeFromPosition(transform.position, playerRigidbody.transform.position);
        flow = riverAsset.GetFlow(transform.position, playerRigidbody.transform.position);
        Vector3 movement = flow;
        playerRigidbody.AddForce(movement * (minimumSpeed * Time.deltaTime), ForceMode.VelocityChange);
        /*
        //Use rb.AddForce to gradually increase or decrease speed
        //   Giving it 0.1f leeway so that the boat won't start going back and forth between 24.97f and 25.002f
        if (playerRigidbody.velocity.magnitude < minimumSpeed)
            playerRigidbody.AddForce(movement * (minimumSpeed * Time.deltaTime));
        else if (playerRigidbody.velocity.magnitude > minimumSpeed)
            playerRigidbody.velocity = movement;
        */
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

    private void OnDrawGizmos()
    {
        if (riverAsset != null)
        {
            for (int i = 0; i < riverAsset.nodes.Length - 1; i++)
            {
                Gizmos.DrawWireSphere(transform.position + riverAsset.nodes[i].centerVector, 1);
                Gizmos.DrawLine(transform.position + riverAsset.nodes[i].centerVector, transform.position + riverAsset.nodes[i + 1].centerVector);
            }
        }

        if (playerRigidbody != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(playerRigidbody.transform.position, transform.position + node.centerVector);

            if (node != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(playerRigidbody.transform.position, (Quaternion.LookRotation(node.flowDirection, Vector3.forward) * Quaternion.AngleAxis(node.flowDirectionOffset_Angle, Vector3.right)) * Vector3.right);
            }
        }
    }
}
