using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition3D : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform prefab;
    
    private Quaternion buildingRotation = Quaternion.identity;
    private Transform buildingGhost;
    private int name;


    private void Start()
    {
        buildingGhost = Instantiate(prefab, transform.position, Quaternion.identity, transform);
        // disable building ghost colliders
        Collider[] colliders = buildingGhost.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    void Update()
    {
        
        if (CanSpawnBuilding())
        {
            buildingGhost.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            buildingGhost.GetComponent<Renderer>().material.color = Color.red;
        }
        
        // some code to make the game object follow the mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _layerMask))
        {
            Vector3 mousePosition3D = hit.point;
            transform.position = mousePosition3D;
        }
        
        // press space to rotate the building
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buildingRotation *= Quaternion.Euler(0f, 90f, 0f);
            // rotate buildingGhost
            buildingGhost.rotation = buildingRotation;
        }
        
        // create the building
        if (Input.GetMouseButtonDown(0) && CanSpawnBuilding())
        {
            Transform newName = Instantiate(prefab, transform.position, buildingRotation);
            newName.name = name.ToString();
            name++;
        }
        
    }
    
    private bool CanSpawnBuilding()
    {
        
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, prefab.GetComponent<MeshRenderer>().bounds.size, buildingRotation, ~_layerMask);
        
        int i = 0;
        //Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            //Output all of the collider names
            Debug.Log("Hit : " + hitColliders[i].name);
            //Increase the number of Colliders in the array
            i++;
        }
        return hitColliders.Length == 0;
        
    }
    
    
}
