using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class BoatClass : FloatingObject
{
    [Header("Collision")]
    public float collisionKnockback = 100;
    public bool isCollided = true;
    private GameObject otherCollider;
    
    [Header("Counter Measures For Going Reverse River Direction")]
    public float counterMeasuresStrenght = 10;
    /// <summary>
    /// Inspector ReadOnly
    /// </summary>
    [SerializeField] private bool isApplyingCountermeasureForce;
    private int lastPassedNodeIndex = 0;

    [Header("Damage Controller")]
    public int MaxHull = 10;
    private int hull;
    public float InvincibilityFrames = 3;
    private float InvincibilityTimer = 0;
    private bool invincible = false;
    private GameObject healthBar = null;
    //public UnityEvent onDeath;
    [SerializeField] PickUpTrigger trigger = null;
    public delegate void OnDamageRecived(float value, Vector3 point);
    public OnDamageRecived onDamaged;
    public ParticleSystem onDamagedParticlePrefab;
    private GameObject WaterLevel = null;
    [SerializeField] private AudioClip[] onDamagedSoundClips;
    [SerializeField] private AudioSource source;

    [SerializeField] private ParticleSystem waterParticles;
    [SerializeField] private LineRenderer waterLines_0,waterLines_1;
    [SerializeField] private float waterLinesLenght;

    #region Damage Functions
    /*public void OnDeath()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().OnDeath();
    }*/
    public void RecoverHull(int amount)
    {
        hull += amount;
        if (hull > MaxHull)
            hull = MaxHull;

        UpdateDamage();
        //SetWaterInBoat();
    }
    private void SetWaterInBoat()
    {
        float boatImg = (float)hull / (float)MaxHull;
        Texture _texture = Resources.Load("Materials/Menus/Boat_9") as Texture;
        if (boatImg == 1)
            _texture = Resources.Load("Materials/Menus/Boat_1") as Texture;
        else if (boatImg >= 0.8f)
            _texture = Resources.Load("Materials/Menus/Boat_2") as Texture;
        else if (boatImg >= 0.7f)
            _texture = Resources.Load("Materials/Menus/Boat_3") as Texture;
        else if (boatImg >= 0.6f)
            _texture = Resources.Load("Materials/Menus/Boat_4") as Texture;
        else if (boatImg >= 0.5f)
            _texture = Resources.Load("Materials/Menus/Boat_5") as Texture;
        else if (boatImg >= 0.4f)
            _texture = Resources.Load("Materials/Menus/Boat_6") as Texture;
        else if (boatImg >= 0.2f)
            _texture = Resources.Load("Materials/Menus/Boat_7") as Texture;
        else if (boatImg > 0)
            _texture = Resources.Load("Materials/Menus/Boat_8") as Texture;
        healthBar.GetComponent<Renderer>().material.mainTexture = _texture;

        //Debug.Log(_texture.name);
        if (hull == MaxHull)
        {
            if (WaterLevel != null)
                WaterLevel.SetActive(false);
        }
        else
        {
            if (WaterLevel == null)
            {
                // Rewrite this once we have the proper dimensions and such
                GameObject newObj;
                newObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
                Destroy(newObj.GetComponent<MeshCollider>());
                //newObj.GetComponent<MeshCollider>().convex = true;
                newObj.name = "Boat Water";
                newObj.transform.parent = gameObject.transform;

                newObj.transform.localScale = new Vector3(0.3f, 1, 0.45f);
                newObj.transform.localEulerAngles = new Vector3(0, 0, 0);

                Material newMat = Resources.Load("Materials/SimpleWater", typeof(Material)) as Material;
                if (newMat != null)
                    newObj.GetComponent<MeshRenderer>().material = newMat;
                else
                    Debug.LogWarning("Error: Material not found");

                WaterLevel = newObj;
            }
            else if (!WaterLevel.activeInHierarchy)
                WaterLevel.SetActive(true);

            float waterHeight;
            waterHeight = (float)(hull - 1) / (float)(MaxHull - 1);
            WaterLevel.transform.localPosition = new Vector3(0, 0.1f + (0.7f * (1 - waterHeight)), 0.4f);
        }
    }
    #endregion

    protected override void Initialize()
    {
        base.Initialize();

        hull = MaxHull;
        trigger.onHealDamage += RecoverHull;

        healthBar = GameObject.Find("HealthBar");

        float windowWidth = (float)(Screen.width * 3) / (float)(Screen.height * 4);
        healthBar.transform.localPosition = new Vector3(-2.6f * windowWidth, 1.35f, 4);

        if (source == null)
            source = gameObject.GetComponent<AudioSource>();
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();

            AudioMixer mix = Resources.Load("AudioMixers/Sound Effects") as AudioMixer;
            source.outputAudioMixerGroup = mix.FindMatchingGroups("Master")[0];
        }
    }

    void Update()
    {

        //Knockback
        if (isCollided == true)
        {
            bool stillClose = false;
            Collider[] hits = Physics.OverlapSphere(transform.position, 2.5f);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].gameObject == otherCollider)
                {
                    stillClose = true;
                }
            }
            if (stillClose == false)
                isCollided = false;
        }

        //Counter Measures
        closestNode = river.riverAsset.GetNodeFromPosition(transform.position);
        if (closestNode.index > lastPassedNodeIndex)
        {
            lastPassedNodeIndex = closestNode.index;
        }

        if (closestNode.index < lastPassedNodeIndex)
        {
            GetComponent<Rigidbody>().AddForce(river.riverAsset.GetNodeFromIndex(closestNode.index).flowDirection * counterMeasuresStrenght * Time.deltaTime * (lastPassedNodeIndex - closestNode.index));
            isApplyingCountermeasureForce = true;
        }
        else
        {
            isApplyingCountermeasureForce = false;
        }

        //Damage Control
        if (invincible)
        {
            InvincibilityTimer += Time.deltaTime;
            if (InvincibilityTimer >= InvincibilityFrames)
            {
                InvincibilityTimer = 0;
                invincible = false;
            }
        }


        UpdateVFXs();

    }

    private void UpdateVFXs ()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        CapsuleCollider collider = GetComponent<CapsuleCollider>();

        waterParticles.emissionRate = body.velocity.magnitude;
        waterParticles.transform.position = collider.ClosestPoint(transform.position + body.velocity);
        waterParticles.transform.LookAt(transform);
        waterParticles.transform.Rotate(Vector3.up * 180);

        List<Vector3> line_0_positions = new List<Vector3>();
        line_0_positions.Add(collider.ClosestPointOnBounds(transform.position + body.velocity + ((Vector3.Cross(closestNode.finalFlowDirection,Vector3.up)) * 10)));
        line_0_positions.Add(Vector3.Lerp(line_0_positions[0], line_0_positions[0] - body.velocity, Time.deltaTime * waterLinesLenght * body.velocity.magnitude));
        line_0_positions[0] = collider.ClosestPoint(line_0_positions[1]);
        waterLines_0.SetPositions(line_0_positions.ToArray());

        List<Vector3> line_1_positions = new List<Vector3>();
        line_1_positions.Add(collider.ClosestPointOnBounds(transform.position + body.velocity - ((Vector3.Cross(closestNode.finalFlowDirection, Vector3.up)) * 10)));
        line_1_positions.Add(Vector3.Lerp(line_1_positions[0], line_1_positions[0] - body.velocity, Time.deltaTime * waterLinesLenght * body.velocity.magnitude));
        line_1_positions[0] = collider.ClosestPoint(line_1_positions[1]);
        waterLines_1.SetPositions(line_1_positions.ToArray());
    }

    public void UpdateDamage(int customSpeed=-1)
    {
        SetWaterInBoat();

        int newSpeed = MaxHull - hull;
        if (customSpeed != -1) newSpeed = customSpeed;

        if (SpeedValueManager.GetSpeedValues().Count >= newSpeed)
        {
            RiverController.instance.minimumSpeed = SpeedValueManager.GetSpeedValues()[newSpeed].riverSpeed;

            GameObject option = null;
            option = GameObject.Find("PlayerOneSpot");
            if(option != null)
                option.GetComponent<Paddling>().SetSpeedValues(newSpeed);

            option = GameObject.Find("PlayerTwoSpot");
            if (option != null)
                option.GetComponent<PlayerSpot>().SetSpeedValues(newSpeed);
        }
    }

    public float GetDamage()
    {
        return MaxHull - hull;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Item" || collision.transform.tag == "River")
            return;

        if (collision.gameObject.tag == "End")
        {
            GameController.instance.OnCompletedLevel();
            return;
        }

        if (invincible == false)
            source.PlayOneShot(onDamagedSoundClips[onDamagedSoundClips.Length - 1]);
        else
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                ParticleSystem particle = Instantiate(onDamagedParticlePrefab, contact.point, Quaternion.identity) as ParticleSystem;
                Destroy(particle.gameObject, particle.main.duration);

                int soundClip = Mathf.RoundToInt(Mathf.Clamp((collision.relativeVelocity.magnitude / 2) - 5, 0, onDamagedSoundClips.Length - 1));
                //Debug.Log((collision.relativeVelocity.magnitude / 2) - 5 + " m, " + soundClip + " s");
                if (soundClip >= onDamagedSoundClips.Length - 1)
                    soundClip = onDamagedSoundClips.Length - 2;
                source.PlayOneShot(onDamagedSoundClips[soundClip]);
            }
        }

        if (!invincible)
        {
            if (hull > 0)
            {
                hull--;

                GameObject cam = GameObject.Find("Main Camera");
                if(cam != null)
                    cam.GetComponent<CameraController>().StartShakeCam();
            }
            else
                hull = 0;

            invincible = true;
            UpdateDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "End")
        {
            GameController.instance.OnVictory();
            return;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //Vector3 target = river.riverAsset.GetNodeFromPosition(river.transform.position, body.transform.position).centerVector;
        if (collision.gameObject.tag == "River")
            return;

        Vector3 target = collision.GetContact(0).point;
        target = target - body.transform.position;
        target.y = 0;
        body.AddForce(-target.normalized * collisionKnockback);

        otherCollider = collision.gameObject;
        isCollided = true;
    }

    private void OnGUI()
    {
        //GUI.Box(new Rect(0, 0, 100, 25), "HULL: " + hull.ToString("F0") + " / " + MaxHull.ToString());
        if (invincible)
            GUI.Box(new Rect(0, 0, 100, 25), "Invincible");
    }
}
