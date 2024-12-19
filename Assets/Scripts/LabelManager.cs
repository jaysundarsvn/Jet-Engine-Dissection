using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class LabelManager : MonoBehaviour
{
    public GameObject labelCanvas;
    private LineRenderer lr;

    private GameObject currentObj;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        lr.startWidth = 0.001f;
        lr.endWidth = 0.002f;

        lr.startColor = Color.grey;
        lr.endColor = Color.white;    
    }

    // Update is called once per frame
    void Update()
    {
        labelCanvas.transform.LookAt(Camera.main.transform);

        if(currentObj)
        {
            lr.SetPosition(0, currentObj.transform.position);
            lr.SetPosition(1, labelCanvas.transform.position);
        }
    }

    public void UpdateLabel(GameObject obj)
    {
        currentObj = obj;

        labelCanvas.transform.SetParent(obj.transform);
        labelCanvas.transform.localPosition = Vector3.zero;

        labelCanvas.GetComponentInChildren<TextMeshProUGUI>().text = obj.name;

        Vector3 pos = labelCanvas.transform.localPosition;
        pos.y = 0.02f;
        pos.z = 0.01f;
        labelCanvas.transform.localPosition = pos;
    }
}
