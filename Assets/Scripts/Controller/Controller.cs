using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject touch_Marker;

    private Vector3 target_vector;
    private RectTransform rectTransform;

    [HideInInspector]
    public Vector3 GetVector;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.position = new Vector3(-1000f, 0f, -1000f);
        touch_Marker.transform.position = rectTransform.position;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rectTransform.position = Input.mousePosition;
            touch_Marker.transform.position = Input.mousePosition;

        }
        //Vector3 
        //if (Input.touchCount > 0 && Input.touchCount < 2)
        if (Input.GetMouseButton(0))
        {
            Vector3 touchPos = Input.mousePosition;// = Input.GetTouch(0).position;

            target_vector = touchPos - transform.position;

            if (target_vector.magnitude < 100)
            {
                touch_Marker.transform.position = touchPos;
                GetVector = target_vector.normalized;
            }
            else
            {
                touch_Marker.transform.position = rectTransform.position + target_vector.normalized * 100f;
                GetVector = target_vector.normalized;
            }
        }
        else
        {
            touch_Marker.transform.position = transform.position;
            GetVector = new Vector3(0, 0, 0);
        }

        if (Input.GetMouseButtonUp(0))
        {
            rectTransform.position = new Vector3(-1000f, 0f, -1000f);
            touch_Marker.transform.position = rectTransform.position;
            GetVector = new Vector3();
        }
    }
}
