using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AgentPatient : Agent
{
    public bool conscious;
    public bool mobile;
    public GameObject assignedRoom;
    public int conditionBasic; // Prototype
    public int conditionStandard; // Prototype
    public int conditionAdvanced; // Prototype
    public List<Condition> conditions; // For future
    public float treatmentTimer;
    public float treatmentThreshold;
    public GameObject targetedBy;
    public bool decommission;
    private HospitalManager hospitalManager;
    public float distanceFromSpawn;

    void Awake()
    {
        // Defaults
        this.nutrition = 100;
        this.hygiene = 100;
        this.sleep = 100;
        this.nutrition_required = true;
        this.hygiene_required = true;
        this.sleep_required = true;
        this.power_required = false;
        this.conditionStandard = 1;
        this.treatmentTimer = 1000;
        this.treatmentThreshold = 990;
    }

    // Start is called before the first frame update
    void Start()
    {
        hospitalManager = GameObject.Find("HospitalManager").GetComponent<HospitalManager>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromSpawn = Vector3.Distance(this.gameObject.transform.position, hospitalManager.SpawnerPatients.transform.position);
        Behavior();
    }

    public void setAssignedRoom(GameObject g)
    {
        this.assignedRoom = g;
    }

    public void addCondition(Condition condition) { }
    public void removeCondition(int index) { }

    public void Behavior()
    {
        if (this.decommission == true)
        {
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.destination = hospitalManager.SpawnerPatients.transform.position;
            if (Vector3.Distance(this.gameObject.transform.position, hospitalManager.SpawnerPatients.transform.position) < 2)
            {
                Debug.Log("DECOMMISIONED PATIENT AT DESTRUCT DESTINATION");
                Destroy(this.gameObject);
            }
            Debug.Log("DECOMMISIONED PATIENT NOT AT DESTRUCT SPAWN");
        }
        else if (Vector3.Distance(this.transform.position, assignedRoom.transform.position) > 0.1)
        {
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.destination = assignedRoom.transform.position;
        }

        if (this.treatmentTimer > 0)
        {
            this.treatmentTimer -= Time.deltaTime;
        }
    } // Handles behavior logic given state.

    // public OnCollision(); // Handle logic of collision events.
    // public OnTrigger(); // Handle logic of trigger events.  
}
