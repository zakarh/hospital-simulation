using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AgentDoctor : Agent
{
    public bool conscious;
    public bool mobile;
    public List<Condition> conditions;
    public GameObject assignedRoom;

    void Awake()
    {
        this.nutrition = 100;
        this.hygiene = 100;
        this.sleep = 100;
        this.nutrition_required = true;
        this.hygiene_required = true;
        this.sleep_required = true;
        this.power_required = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addCondition(Condition condition) { }
    public void removeCondition(int index) { }
    public void Behavior()
    {

    } // Handles behavior logic given state.

    // public OnCollision(); // Handle logic of collision events.
    // public OnTrigger(); // Handle logic of trigger events.  
}
