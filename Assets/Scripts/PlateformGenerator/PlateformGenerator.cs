using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlateformGenerator
{
    public class PlateformGenerator : MonoBehaviour
    {
        public GameObject basicStructure;

        private bool isInit = false;
        private List<GameObject> plateformsList;
        private Vector3 lastPosition;

    // Start is called before the first frame update
        void Start()
        {
            plateformsList = new List<GameObject>();
            lastPosition = Vector3.zero;
        }

        private void Reset()
        {
            isInit = false;
            lastPosition = Vector3.zero;
            foreach (var plateform in plateformsList)
            {
                DestroyImmediate(plateform);
            }
            plateformsList.Clear();
        }

        // Create first plateform
        void InitFirstPillar()
        {
            isInit = true;
            GameObject go = GameObject.Instantiate(basicStructure, basicStructure.transform.parent);
            
            go.SetActive(true);
            plateformsList.Add(go);
        }
        
        // Create a new Pillar
        void CreatePillar()
        {
            GameObject lastPillar = plateformsList.First();
            GameObject go = GameObject.Instantiate(basicStructure, basicStructure.transform.parent);
            Vector3 newPosition = lastPosition;

            // CALCULATE NEW POS
            newPosition.z += 10.0f;
            
            // SET OBJECT AND INFO
            go.transform.SetPositionAndRotation(newPosition, lastPillar.transform.rotation);
            lastPosition = newPosition;
            go.SetActive(true);
            plateformsList.Add(go);
        }
        
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.I))
            {
                if (!isInit)
                    InitFirstPillar();
                else
                    CreatePillar();
            }
            if (Input.GetKey(KeyCode.O))
            {
                Reset();
            }
        }
    }
}
