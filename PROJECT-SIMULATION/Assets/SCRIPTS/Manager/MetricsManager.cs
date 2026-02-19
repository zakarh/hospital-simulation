using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// RECORD METRICS FOR TASK COMPLETED IN THE CURRENT SESSION.
public class MetricsManager : MonoBehaviour
{
    public bool isEnabled;
    private StreamWriter streamWriter;
    private string documentName;
    void Awake()
    {
        // this.enabled = true;
        // this.documentName = "metrics-session-" + GetUtcNowTimeStampFile() + ".txt";
        // this.streamWriter = new StreamWriter(this.documentName, append: true);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetUtcNowTimeStampFile()
    {
        return DateTime.UtcNow.ToString(@"yyyy-MM-dd'T'HH.mm.ss");
    }

    public string GetUtcNowTimeStampLogs()
    {
        return DateTime.UtcNow.ToString(@"yyyy-MM-dd'T'HH:mm:ss");
    }

    public void RecordTask(Task_ task)
    {
        string timeStamp = GetUtcNowTimeStampLogs();
        string line = timeStamp + ":" + task.ToString();
        Debug.Log(task.ToString(), task);
        WriteLineAsyncWrapper(line);
    }

    private async void WriteLineAsyncWrapper(string line)
    {
        await this.streamWriter.WriteLineAsync(line);
        this.streamWriter.Flush();
        Debug.Log("metric:task:recorded:success");
    }
}
