using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_ : MonoBehaviour, IComparable<Task_>
{
    public string taskGuid;
    public string timeStart;
    public string timeEnd;
    public Action actionType; // Action to perform
    public GameObject target; // Could be Patient or Self
    // The following is used to sort by time and priority:
    public float timeTaken; // seconds
    public List<Vector3> pathTaken;
    public Level priority;
    public bool complete;
    // private MetricsManager metricsManager;
    private LogManager logManager;

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        this.taskGuid = System.Guid.NewGuid().ToString();
        this.pathTaken = new List<Vector3>();
        // this.metricsManager = GameObject.Find("MetricsManager").GetComponent<MetricsManager>();
        this.logManager = GameObject.Find("LogManager").GetComponent<LogManager>();
        this.timeStart = GetUtcNowTimeStampLogs();
    }

    public string GetUtcNowTimeStampFile()
    {
        return DateTime.UtcNow.ToString(@"yyyy-MM-dd'T'HH.mm.ss");
    }

    public string GetUtcNowTimeStampLogs()
    {
        return DateTime.UtcNow.ToString(@"yyyy-MM-dd'T'HH:mm:ss");
    }

    // Update is called once per frame
    public void Update()
    {
        this.timeTaken += Time.deltaTime;
        if (complete)
        {
            this.timeEnd = GetUtcNowTimeStampLogs();
            StartCoroutine(this.logManager.LogTask(this));
            // metricsManager.RecordTask(this);
            Destroy(this.gameObject);
        }
    }

    public int CompareTo(Task_ other)
    {
        if (timeTaken.Equals(other.timeTaken))
        {
            return priority.CompareTo(other.priority);
        }
        return timeTaken.CompareTo(other.timeTaken);
    }

    public override string ToString()
    {
        return "actionType: " + actionType.ToString() + ", timeTaken(s): " + timeTaken.ToString("0.000");
    }
}
