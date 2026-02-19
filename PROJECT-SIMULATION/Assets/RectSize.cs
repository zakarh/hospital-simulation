using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectSize : MonoBehaviour
{
    public float XSize;
    public float YSize;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        XSize = sr.size.x;
        YSize = sr.size.y;
        // XSize = sr.rect.size.x;
        // YSize = sr.rect.size.y;
        // Debug.Log("XSIZE: " + XSize + " YSIZE: " + YSize);
    }
}
