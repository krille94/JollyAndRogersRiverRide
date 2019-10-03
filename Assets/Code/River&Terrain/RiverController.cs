using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RiverController : MonoBehaviour
{
    public static RiverController instance;

    public RiverObject riverAsset;
    
    public List<Rigidbody> observedObjects = new List<Rigidbody>();
    public Transform endTransform;

    [Header("MeshWaves")]
    [Range(0.0f, 1.0f)] public float modifyFreq = 0.75f;
    [Range(0.0f, 10.0f)] public float waveheight = 0.25f;

    [Header("Floating")]
    public SystemTypes usedSystemType;
    public enum SystemTypes { Arcade, Physics, non }

    public LayerMask arcadeRiverLayer;
    public float arcadeBouance = 100;
    public float physicsBouance = 75000;

    [Header("Flow")]
    public int minimumSpeed;
    public Vector3 movementDirection;

    [Header("SplashEffect")]
    public ParticleSystem onImpactEffect = null;
    public AudioSource onImpactSound = null;

    [Header("Debuging")]
    [SerializeField] Vector3 flow;
    [SerializeField] RiverNode node;
    [SerializeField] float heightAboveWater;

    private Transform effectsPool;
    private Mesh mesh;
    
    private void Start()
    {
        if (instance != null)
            Debug.LogWarning("Multiple RiverControllers Detected");
        instance = this;

        bool allWorking = true;

        if(riverAsset != null)
        {
            mesh = riverAsset.GetMesh();
        }
        else
        {
            Debug.LogWarning("Mesh missing from RiverObject");
            allWorking = false;
        }
        
        if (mesh == null)
        {
            Debug.LogWarning("Mesh missing from RiverObject");
            allWorking = false;
        }

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            if(observedObjects.Contains(GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>()) == false)
            {
                observedObjects.Add(GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>());
            }
        }

        redo:
        foreach(Rigidbody body in observedObjects)
        {
            if (body == null)
            {
                observedObjects.Remove(body);
                goto redo;
            }                
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

            if(gameObject.GetComponent<MeshCollider>() == null)
                gameObject.AddComponent<MeshCollider>();
            gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;

            if(endTransform != null)
                endTransform.position = riverAsset.nodes[riverAsset.nodes.Length - 1].centerVector;
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
        if (!Application.isPlaying)
        {
            if(riverAsset != null)
            {
                mesh = riverAsset.GetMesh();
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh = mesh;
                }

                if (gameObject.GetComponent<MeshCollider>() == null)
                    gameObject.AddComponent<MeshCollider>();
                gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
            }

            if (endTransform != null)
                endTransform.position = riverAsset.nodes[riverAsset.nodes.Length - 1].centerVector;
            return;
        }

        PhysicsFlowUpdate();
        ArcadeFlowUpdate();

        //OutCommented Its Flattening the river, NO MORE WATERFALS :(
        //MeshWaveUpdate();

        ArcadeFloatingUpdate();
    }

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
            for (int i = 0; i < observedObjects.Count; i++)
            {
                Rigidbody body = observedObjects[i];
                node = riverAsset.GetNodeFromPosition(transform.position, body.position);
                flow = riverAsset.GetFlow(transform.position, body.position);
                Vector3 movement = flow;
                body.AddForce(movement * (minimumSpeed * Time.deltaTime), ForceMode.VelocityChange);
            }
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
                    //Debug.Log(hit.transform.name);
                    if (hit.transform != transform)
                        return;

                    RiverNode node = riverAsset.GetNodeFromPosition(hit.point);

                    Vector3 targetPosition = new Vector3(
                        body.transform.position.x,
                        hit.point.y,
                        body.transform.position.z
                    );
                    body.transform.position = Vector3.Lerp(body.transform.position, targetPosition, Time.deltaTime);
                }
            }
        }
    }

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
    }

    /*
        ParticleSystem particle = Instantiate(onImpactEffect, other.transform.position, Quaternion.identity) as ParticleSystem;
        particle.transform.SetParent(effectsPool, true);
        Destroy(particle.gameObject, particle.main.duration);

        AudioSource audio = Instantiate(onImpactSound, other.transform.position, Quaternion.identity) as AudioSource;
        audio.transform.SetParent(effectsPool, true);
        Destroy(audio.gameObject, audio.clip.length);
    */
}
