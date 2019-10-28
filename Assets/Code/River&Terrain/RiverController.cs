using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverController : MonoBehaviour
{
    public static RiverController instance;

    public RiverObject riverAsset;
    private bool isPlaying;

    public List<FloatingObject> observedObjects = new List<FloatingObject>();
    private void RemoveObservedObject (FloatingObject obj) {
        observedObjects.Remove(obj);
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
    public float slopeBoatTurnSpeed=5;
    public Vector3 movementDirection;
    private float slopeAngle;

    [Header("SplashEffect")]
    public ParticleSystem onImpactEffect = null;
    public AudioSource onImpactSound = null;
    
    private Transform effectsPool;
    private Mesh mesh;
    
    private void Start()
    {
        minimumSpeed = SpeedValueManager.GetSpeedValues()[0].riverSpeed;
        if (instance != null)
            Debug.LogWarning("Multiple RiverControllers Detected");
        instance = this;

        bool allWorking = true;
        

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            if(observedObjects.Contains(GameObject.FindGameObjectWithTag("Player").GetComponent<FloatingObject>()) == false)
            {
                observedObjects.Add(GameObject.FindGameObjectWithTag("Player").GetComponent<FloatingObject>());
            }
        }

        redo:
        for(int i = 0; i < observedObjects.Count; i++)
        {
            if (observedObjects[i] == null)
            {
                observedObjects.RemoveAt(i);
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

            BuildMesh();

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<MeshFilter>().mesh = mesh;
            }
            /*
            if(gameObject.GetComponent<MeshCollider>() == null)
                gameObject.AddComponent<MeshCollider>();
            gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
            */
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
        if (isPlaying)
        {
            foreach (FloatingObject obj in observedObjects)
            {
                RiverNode closest = riverAsset.GetNodeFromPosition(obj.transform.position);
                obj.UpdateNodes(closest);
            }

            ArcadeFlowUpdate();

            //OutCommented Its Flattening the river, NO MORE WATERFALS :(
            MeshWaveUpdate();

            ArcadeFloatingUpdate();
        }
        else
        {
            if (GameController.isPlaying)
                isPlaying = true;
        }
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
                FloatingObject obj = observedObjects[i];
                if (obj.enabled == false)
                    continue;
                slopeAngle = obj.GetNodes().closest.centerVector.y- obj.GetNodes().last.centerVector.y;

                Vector3 flow = obj.GetNodes().closest.finalFlowDirection;
                obj.GetRigidbody().AddForce(flow * ((minimumSpeed + (slopeAngle*slopeSpeedBoost)) * Time.deltaTime), ForceMode.VelocityChange);
            }
        }
    }

    void ArcadeFloatingUpdate ()
    {
        if (usedSystemType == SystemTypes.Arcade)
        {
            for (int i = 0; i < observedObjects.Count; i++)
            {
                FloatingObject obj = observedObjects[i];
                if (obj.enabled == false)
                    continue;
                RaycastHit hit;
                Debug.DrawRay(obj.transform.position + (Vector3.up * 1000), Vector3.down * 2000, Color.yellow);
                if (Physics.Raycast(obj.transform.position + (Vector3.up * 1000), Vector3.down, out hit, 2000, arcadeRiverLayer))
                {
                    if (hit.transform != transform)
                    {
                        Debug.Log(hit.transform.name);
                        return;
                    }
                    Vector3 targetPosition = new Vector3(
                        obj.transform.position.x,
                        hit.point.y,
                        obj.transform.position.z
                    );

                    obj.GetRigidbody().MovePosition(Vector3.Lerp(obj.transform.position, targetPosition, Time.fixedDeltaTime * arcadeBouance));
                                        
                    Quaternion target2 = Quaternion.LookRotation(Vector3.right, hit.normal);
                    Quaternion targetRotation = obj.transform.rotation;

                    targetRotation.x = target2.x * obj.transform.rotation.w;
                    targetRotation.z = target2.z * obj.transform.rotation.y;
                    
                    /*float boatTilt=PlayerData.boatTiltOffset;

                    Quaternion oldRot = obj.transform.rotation;
                    if(!PlayerData.boatTilted&&PlayerData.boatTiltOffset!=0)
                    {
                        PlayerData.boatTilted = true;
                        oldRot.x -= PlayerData.boatTiltOffset * -obj.transform.rotation.y;
                        oldRot.z -= PlayerData.boatTiltOffset * -obj.transform.rotation.w;
                    }*/
                    obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetRotation, Time.deltaTime*slopeBoatTurnSpeed);
                }
            }
        }
    }

    void MeshWaveUpdate()
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
