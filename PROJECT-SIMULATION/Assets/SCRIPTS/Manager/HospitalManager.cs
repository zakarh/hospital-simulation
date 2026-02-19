using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class HospitalManager : MonoBehaviour
{
    [Header("Agents")]
    public List<GameObject> Patients = new List<GameObject>();
    public List<GameObject> Nurses = new List<GameObject>();
    private List<GameObject> Doctors = new List<GameObject>();
    private List<GameObject> Robots = new List<GameObject>();

    [Header("Spawners")]
    public GameObject SpawnerPatients;
    public GameObject SpawnerNurses;
    public GameObject SpawnerDoctors;
    public GameObject SpawnerRobots;

    [Header("Prefabs")]
    public GameObject PrefabPatient;
    public GameObject PrefabNurse;
    public GameObject PrefabDoctor;
    public GameObject PrefabRobot;
    public GameObject PrefabTask;
    public GameObject PrefabTaskLine;

    public GameObject[] PatientRooms;

    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.Find("PatientRooms");
        GameObject[] prs = new GameObject[g.transform.childCount];
        for (int i = 0; i < g.transform.childCount; i++)
        {
            // Debug.Log(i);
            prs[i] = g.transform.GetChild(i).gameObject;
        }
        PatientRooms = prs;
    }

    // Update is called once per frame
    void Update()
    {
        behavior();
    }

    public GameObject getAvailablePatientRoom()
    {
        foreach (GameObject patientRoom in PatientRooms)
        {
            LocationPatientRoom lpr = patientRoom.GetComponent<LocationPatientRoom>();
            if (lpr.Occupants.Count < lpr.MaxCapacity)
            {
                Debug.Log("found");
                return patientRoom;
            }
        }
        Debug.Log("not found");
        return null;
    }

    public void addPatient()
    {
        GameObject patientRoom = getAvailablePatientRoom();
        // Debug.Log("patientRoom: ", patientRoom);
        if (patientRoom != null)
        {
            Debug.Log("instantiated");
            GameObject patient = Instantiate(PrefabPatient, SpawnerPatients.transform);
            Debug.Log(patientRoom.GetType());
            patient.GetComponent<AgentPatient>().assignedRoom = patientRoom;
            patientRoom.GetComponent<LocationPatientRoom>().Occupants.Add(patient);
            Patients.Add(patient);
        }
        Debug.Log("not instantiated");
    } // Add prefab patients or configured via UI.

    public void addNurse() { }
    public void addDoctor() { }
    public void addRobot() { }
    public void deletePatient(int index) { }
    public void deleteNurse(int index) { }
    public void deleteDoctor(int index) { }
    public void deleteRobot(int index) { }
    // Distribute tasks amongst health professional and robot agents
    // given the state of patients.
    public PriorityQueue<Task_, Level> taskQueueExtreme;
    public PriorityQueue<Task_, Level> taskQueue;

    public GameObject getTaskForNurse()
    {
        // NOTE: Prototype implementation to demonstrate task requests given patient needs.
        // Look at patients and create a tast for the patient that requires treatment.
        foreach (GameObject patient in Patients)
        {
            AgentPatient agentPatient = patient.GetComponent<AgentPatient>();
            if (agentPatient.treatmentTimer < agentPatient.treatmentThreshold)
            {
                if (agentPatient.targetedBy == null)
                {
                    GameObject gTask = Instantiate(PrefabTask);
                    GameObject gTaskLine = Instantiate(PrefabTaskLine);
                    
                    Task_ task = gTask.GetComponent<Task_>();
                    task.actionType = Action.TreatPatientConditions;
                    task.target = patient;
                    task.priority = Level.Standard;

                    gTaskLine.transform.parent = gTask.transform;

                    return gTask;
                }
            }
        }
        return null;

    }

    private void behavior()
    {

    }
}
