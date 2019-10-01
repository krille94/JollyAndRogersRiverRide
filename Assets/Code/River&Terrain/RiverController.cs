using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverController : MonoBehaviour
{
    public static RiverController instance;

    public RiverObject riverAsset;

    [HideInInspector]
    public Rigidbody playerRigidbody;

    public List<Rigidbody> observedObjects = new List<Rigidbody>();

    [Header("MeshWaves")]
    [Range(0.0f, 1.0f)] public float modifyFreq = 0.75f;
    [Range(0.0f, 10.0f)] public float waveheight = 0.25f;

    [Header("Floating")]
    public SystemTypes usedSystemType;
    public enum SystemTypes { Arcade, Physics }

    public LayerMask arcadeRiverLayer;
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
        if (instance != null)
            Debug.LogWarning("Multiple RiverControllers Detected");
        instance = this;

        bool allWorking = true;

        mesh = riverAsset.GetMesh();

        if(mesh == null)
        {
            Debug.LogWarning("Mesh missing from RiverObject");
            allWorking = false;
        }

        if (allWorking)
        {
            GameObject pool = GameObject.Find("EffectsPool");
            if (pool != null)
                effectsPool = pool.transform;
            else
                effectsPool = new GameObject("EffectsPool").transform;

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh = mesh;
            }

            gameObject.AddComponent<MeshCollider>();
            gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
            //gameObject.GetComponent<MeshCollider>().convex = true;
            //gameObject.GetComponent<MeshCollider>().isTrigger = true;
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
        ArcadeFlowUpdate();

        MeshWaveUpdate();

        ArcadeFloatingUpdate();
    }

    [SerializeField]
    Vector3 flow;
    [SerializeField]
    RiverNode node;
    void ArcadeFlowUpdate ()
    {
        if(usedSystemType == SystemTypes.Arcade)
        {
            for (int i = 0; i < observedObjects.Count; i++)
            {
                Rigidbody body = observedObjects[i];
                node = riverAsset.GetNodeFromPosition(body.position);
                flow = riverAsset.GetFlow(body.position);
                body.AddForce(flow * (minimumSpeed * Time.deltaTime), ForceMode.VelocityChange);
            }
        }
    }

    void PhysicsFlowUpdate()
    {
        if (usedSystemType == SystemTypes.Physics)
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
    }

    void MeshWaveUpdate ()
    {
        if (mesh == null)
            return;

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

    void ArcadeFloatingUpdate ()
    {
        if (usedSystemType == SystemTypes.Arcade)
        {
            for (int i = 0; i < observedObjects.Count; i++)
            {
                Rigidbody body = observedObjects[i];
                RaycastHit hit;
                if (Physics.Raycast(body.transform.position + (Vector3.up * 1000), Vector3.down, out hit, 2000, arcadeRiverLayer))
                {
                    Debug.Log(hit.transform.name);
                    if (hit.transform != transform)
                        return;

                    RiverNode node = riverAsset.GetNodeFromPosition(hit.point);

                    body.transform.position = new Vector3(
                        body.transform.position.x,
                        node.centerVector.y,
                        body.transform.position.z
                    );
                }
            }
        }
    }

    [SerializeField] float heightAboveWater;
    void PhysicsFloatingUpdate (Collider other)
    {
        if (usedSystemType == SystemTypes.Physics)
        {
            float distance = 1;// collider.bounds.SqrDistance(other.transform.position);
            distance = Mathf.Abs(other.transform.position.y - transform.position.y);

            node = riverAsset.GetNodeFromPosition(transform.position, other.transform.position);
            heightAboveWater = Mathf.Abs(other.transform.position.y - node.centerVector.y);
            if (Mathf.Abs(other.transform.position.y - node.centerVector.y) > 0.5f)
                other.attachedRigidbody.AddForce(Vector3.up * (physicsBouance / other.attachedRigidbody.mass) * distance * Time.fixedDeltaTime);

            Debug.DrawRay(other.transform.position, Vector3.up * ((physicsBouance / other.attachedRigidbody.mass) * distance) / 2500, Color.blue);
            //Debug.Log(other.name + " is colliding with water surface");
        }
    }
    /*
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
    */
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
        /*
        if (playerRigidbody != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(playerRigidbody.transform.position, transform.position + node.centerVector);

            if (node != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(playerRigidbody.transform.position, node.finalFlowDirection * 10);
            }
        }
        */
    }
}
