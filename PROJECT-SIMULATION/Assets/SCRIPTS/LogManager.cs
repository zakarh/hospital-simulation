using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LogManager : MonoBehaviour
{
    // NOTE: If this is required in more than one place
    // then move it to a separate script, instantiate on gObj, and access it.
    public string sessionTime = "";
    // private static readonly HttpClient client = new HttpClient();
    public string sessionGuid = "";
    public string ip_address = "127.0.0.1:5432";

    public bool testLogging = false;

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        this.sessionTime = this.GetUtcNowTimeStampFile();
        this.sessionGuid = System.Guid.NewGuid().ToString();
        // Ping the server:
        // Raise an warning or error if it cannot be reached.
    }

    // Update is called once per frame
    void Update()
    {
        if (testLogging)
        {
            StartCoroutine(TestLogTask());
            testLogging = false;
        }
    }

    public string GetUtcNowTimeStampFile()
    {
        return DateTime.UtcNow.ToString(@"yyyy-MM-dd'T'HH.mm.ss");
    }

    public string GetUtcNowTimeStampLogs()
    {
        return DateTime.UtcNow.ToString(@"yyyy-MM-dd'T'HH:mm:ss");
    }

    public IEnumerator TestLogTask()
    {
        WWWForm form = new WWWForm();
        Debug.Log(this.sessionTime.ToString());
        form.AddField("session_time", "test-" + this.sessionTime.ToString());
        form.AddField("session_guid", this.sessionGuid.ToString());
        form.AddField("task_guid", System.Guid.NewGuid().ToString());
        form.AddField("time_start", GetUtcNowTimeStampLogs());
        form.AddField("time_end", GetUtcNowTimeStampLogs());
        form.AddField("action_type", "null");
        form.AddField("total_time", "0");
        form.AddField("distance_traveled", "0");
        form.AddField("path_taken", "[]");

        UnityWebRequest www = UnityWebRequest.Post(ip_address + "/log/task", form);
        www.chunkedTransfer = false;

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

    private string ComputeDistanceToString(List<Vector3> pathTaken)
    {
        float distance = 0;
        Vector3 lastPosition = pathTaken[0];
        foreach (Vector3 position in pathTaken)
        {
            distance += Vector3.Distance(lastPosition, position);
        }
        return distance.ToString();
    }

    private string ConvertPathTakenToString(List<Vector3> pathTaken)
    {
        List<string> positions = new List<string>();
        foreach (Vector3 position in pathTaken)
        {
            // UNITY USES NON-STANDARD AXIS SUCH THAT THE Y-AXIS IS UP.
            // CONVERT TO STANDARD AXIS TO MAP Y-AXIS TO Z-AXIS WHERE Z-AXIS IS UP.
            List<string> vector = new List<string>() {
                position.x.ToString(),
                position.z.ToString(),
                position.y.ToString(),
            };
            positions.Add("[" + string.Join(", ", vector) + "]");
        }
        return "[" + string.Join(", ", positions) + "]";

    }

    public IEnumerator LogTask(Task_ task)
    {
        // Dictionary<string, string> values = new Dictionary<string, string>
        // {
        //     { "time_start", task.timeStart },
        //     { "time_end", task.timeEnd },
        //     { "action_type", nameof(task.actionType) },
        //     { "total_time", task.timeTaken.ToString() },
        //     { "distance_traveled", task.distanceTraveled.ToString() },
        //     { "path_taken", task.pathTaken.ToString() }
        // };
        Debug.Log("TASK ACTION TYPE: " + task.actionType.ToString());
        WWWForm form = new WWWForm();
        // form.AddField("time_start", "task.timeStart");
        // form.AddField("time_end", "task.timeEnd");
        // form.AddField("action_type", "nameof(task.actionType)");
        // form.AddField("total_time", "task.timeTaken.ToString()");
        // form.AddField("distance_traveled", "task.distanceTraveled.ToString()");
        // form.AddField("path_taken", "task.pathTaken.ToString()");
        Debug.Log(this.sessionTime.ToString());
        form.AddField("session_time", this.sessionTime.ToString());
        form.AddField("session_guid", this.sessionGuid.ToString());
        form.AddField("task_guid", task.taskGuid.ToString());
        form.AddField("time_start", task.timeStart.ToString());
        form.AddField("time_end", task.timeEnd.ToString());
        form.AddField("action_type", task.actionType.ToString());
        form.AddField("total_time", task.timeTaken.ToString());
        form.AddField("distance_traveled", ComputeDistanceToString(task.pathTaken));
        form.AddField("path_taken", ConvertPathTakenToString(task.pathTaken));
        UnityWebRequest www = UnityWebRequest.Post(ip_address + "/log/task", form);
        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            Debug.Log("Form upload not complete!");
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}
