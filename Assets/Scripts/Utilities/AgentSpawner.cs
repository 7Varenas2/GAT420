using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{

    public Agent agent;
    public LayerMask layerMask;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, 100, layerMask))
            {
                Instantiate(agent, hitInfo.point, Quaternion.identity);
            }
        }
    }
}
