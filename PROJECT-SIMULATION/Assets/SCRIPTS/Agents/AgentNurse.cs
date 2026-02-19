using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AgentNurse : Agent
{
    public GameObject currentTask;
    private HospitalManager hospitalManager;
    public bool drawTaskLine;

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
        hospitalManager = GameObject.Find("HospitalManager").GetComponent<HospitalManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Behavior();
    }

    public void Behavior()
    {
        // Get task for nurse
        if (this.currentTask == null)
        {
            this.currentTask = hospitalManager.getTaskForNurse();
            if (this.currentTask != null)
            {
                Debug.Log("GOT NURSE TASK");

                GameObject gTaskLine = this.currentTask.transform.GetChild(0).gameObject;
                Task_ task = this.currentTask.GetComponent<Task_>();
                // Add start position of path taken:
                task.pathTaken.Add(this.transform.position);

                if (task.actionType == Action.TreatPatientConditions)
                {
                    AgentPatient patient = task.target.GetComponent<AgentPatient>();
                    patient.targetedBy = this.gameObject;

                    // Draw line
                    gTaskLine.GetComponent<LineRenderer>().SetPosition(0, this.gameObject.transform.position);
                    gTaskLine.GetComponent<LineRenderer>().SetPosition(1, task.target.transform.position);
                }
            }
            else
            {
                Debug.Log("NO NURSE TASK - WAITING ON STANDBY");
                // Wait on standby
                if (Vector3.Distance(this.transform.position, hospitalManager.SpawnerNurses.transform.position) > 1)
                {
                    // Travel to patient
                    UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
                    agent.destination = hospitalManager.SpawnerNurses.transform.position;
                }
            }
            return;
        }
        else
        {
            Task_ task = this.currentTask.GetComponent<Task_>();

            // Draw line
            GameObject gTaskLine = this.currentTask.transform.GetChild(0).gameObject;
            gTaskLine.GetComponent<LineRenderer>().SetPosition(0, this.gameObject.transform.position);
            gTaskLine.GetComponent<LineRenderer>().SetPosition(1, task.target.transform.position);

            // Add position of path taken:
            task.pathTaken.Add(this.transform.position);

            // Behavior-Tree:
            if (task.actionType == Action.TreatPatientConditions)
            {
                if (Vector3.Distance(this.transform.position, task.target.transform.position) > 1)
                {
                    // Travel to patient
                    UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
                    agent.destination = task.target.transform.position;
                }
                else
                {
                    // Apply treatment
                    AgentPatient patient = task.target.GetComponent<AgentPatient>();
                    patient.conditionBasic = Math.Max(patient.conditionBasic - 1, 0);
                    patient.conditionStandard = Math.Max(patient.conditionStandard - 1, 0);
                    if (patient.conditionBasic == 0 && patient.conditionStandard == 0 && patient.conditionAdvanced == 0)
                    {
                        // Decommission patient.
                        patient.decommission = true;
                        patient.assignedRoom.GetComponent<LocationPatientRoom>().Occupants.Remove(patient.gameObject);
                        hospitalManager.Patients.Remove(patient.gameObject);
                    }
                    else
                    {
                        patient.treatmentTimer = 1000;
                    }
                    // Set task as complete
                    Debug.Log("TASK COMPLETE");
                    // Add end position of path taken:
                    task.pathTaken.Add(this.transform.position);
                    task.complete = true;
                    patient.targetedBy = null;
                    // DestroyImmediate(this.currentTask);
                    this.currentTask = null;
                }
            }
            // else if (currentTask.actionType == Action.WaitOnStandby)  {
            //     if (Vector3.Distance(this.transform.position, hospitalManager.SpawnerNurses.transform.position) > 1) {
            //         // Travel to patient
            //         UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            //         agent.destination = hospitalManager.SpawnerNurses.transform.position;
            //     } else {
            //         currentTask
            //     }
            // }

        }
    } // Handles behavior logic given state.

    // public OnCollision(); // Handle logic of collision events.
    // public OnTrigger(); // Handle logic of trigger events.  
}
