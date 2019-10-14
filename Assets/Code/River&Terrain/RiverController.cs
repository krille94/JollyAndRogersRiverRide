using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverController : MonoBehaviour
{
    public static RiverController instance;

    public RiverObject riverAsset;
    
    public List<Rigidbody> observedObjects = new List<Rigidbody>();
    private List<RiverNode> observedObjectPreviousNodes = new List<RiverNode>();
    private List<RiverNode> observedObjectCurrentNodes = new List<RiverNode>();
    private void RemoveObservedObject (Rigidbody obj) {
        for (int i = 0; i < observedObjects.Count; i++)
        {
            if(observedObjects[i] == obj)
            {
                observedObjectCurrentNodes.RemoveAt(i);
                observedObjectPreviousNodes.RemoveAt(i);
                observedObjects.RemoveAt(i);
                return;
            }
        }
    }
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
    public float slopeSpeedBoost=-2;
    public Vector3 movementDirection;
    private float slopeAngle;

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
        

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            if(observedObjects.Contains(GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>()) == false)
            {
                observedObjects.Add(GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>());
            }
        }

        redo:
        for(int i = 0; i < observedObjects.Count; i++)
        {
            if (observedObjects[i] == null)
            {
                observedObjects.RemoveAt(i);
                observedObjectPreviousNodes.RemoveAt(i);
                observedObjectCurrentNodes.RemoveAt(i);
                goto redo;
            }                
        }

        foreach(Rigidbody body in observedObjects)
        {
            observedObjectPreviousNodes.Add(riverAsset.GetNodeFromPosition(body.position));
            observedObjectCurrentNodes.Add(riverAsset.GetNodeFromPosition(body.position));
        }

        if (allWorking)
        {
            GameObject pool = GameObject.Find("EffectsPool");
            if (pool != null)
                effectsPool = pool.transform;
            else
                effectsPool = new GameObject("EffectsPool").transform;

            BuildMesh();

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<MeshFilter>().mesh = mesh;
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
    /*
        if (!Application.isPlaying)
        {
            if(riverAsset != null)
            {
                BuildMesh();
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
    */
        PhysicsFlowUpdate();
        ArcadeFlowUpdate();

        //OutCommented Its Flattening the river, NO MORE WATERFALS :(
        //MeshWaveUpdate();

        ArcadeFloatingUpdate();
    }

    /// <summary>
    /// If mesh is corupted or missing
    /// </summary>
    private void BuildMesh ()
    {
        mesh = new Mesh();
        mesh.vertices = riverAsset.vertices;
        mesh.triangles = riverAsset.tris;
        mesh.uv = riverAsset.uvs;
        mesh.RecalculateNormals();
    }

    void ArcadeFlowUpdate ()
    {
        if(usedSystemType == SystemTypes.Arcade)
        {
            for (int i = 0; i < observedObjects.Count; i++)
            {
                Rigidbody body = observedObjects[i];
                node = riverAsset.GetNodeFromPosition(body.position);

                if (observedObjectCurrentNodes[i]!=node)
                {
                    // Experimental code for having boat sharply slow down when it reaches the end of a slope
                    // Not reliable yet ...
                    /*
                    slopeAngle = observedObjectCurrentNodes[i].centerVector.y - observedObjectPreviousNodes[i].centerVector.y;
                    float slopeAngle2 = node.centerVector.y - observedObjectCurrentNodes[i].centerVector.y;

                    flow = riverAsset.GetFlow(body.position);
                    if(slopeAngle<slopeAngle2)
                    {
                        Debug.Log("test");
                        body.AddForce(-flow * ((minimumSpeed + (slopeAngle * slopeSpeedBoost) * 10) * Time.deltaTime), ForceMode.VelocityChange);
                    }*/

                    observedObjectPreviousNodes[i] = observedObjectCurrentNodes[i];
                    observedObjectCurrentNodes[i] = node;
                }
                slopeAngle = observedObjectCurrentNodes[i].centerVector.y- observedObjectPreviousNodes[i].centerVector.y;

                flow = riverAsset.GetFlow(body.position);
                body.AddForce(flow * ((minimumSpeed + (slopeAngle*slopeSpeedBoost)) * Time.deltaTime), ForceMode.VelocityChange);

                //if (body.position.y != node.centerVector.y)
                //    body.transform.position = new Vector3(body.position.x, node.centerVector.y, body.position.z);
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

                if (observedObjectCurrentNodes[i] != node)
                {
                    observedObjectPreviousNodes[i] = observedObjectCurrentNodes[i];
                    observedObjectCurrentNodes[i] = node;
                }
                slopeAngle = observedObjectCurrentNodes[i].centerVector.y - observedObjectPreviousNodes[i].centerVector.y;
                
                flow = riverAsset.GetFlow(transform.position, body.position);
                Vector3 movement = flow * (minimumSpeed + (slopeAngle * slopeSpeedBoost));
                if(movement.magnitude > body.velocity.magnitude)
                    body.AddForce(movement * Time.deltaTime, ForceMode.VelocityChange);
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
                Debug.DrawRay(body.transform.position + (Vector3.up * 1000), Vector3.down * 2000, Color.yellow);
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
                    body.MovePosition(Vector3.Lerp(body.position, targetPosition, Time.fixedDeltaTime * arcadeBouance));

                    // Commented out because boat's not supposed to forcibly face the river flow
                    //Quaternion targetRotation = Quaternion.LookRotation(node.flowDirection, hit.normal);
                    //body.transform.rotation = Quaternion.Lerp(body.rotation, targetRotation, Time.fixedDeltaTime);

                    //targetRotation.y = body.rotation.y;
                    //body.MoveRotation(Quaternion.Lerp(body.rotation, targetRotation, Time.fixedDeltaTime * controllStrenght));
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
