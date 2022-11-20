using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace PlateformGenerator
{
    public class PlateformGenerator : MonoBehaviour
    {
        public GameObject basicStructure;

        private bool isInit = false;
        private List<GameObject> plateformsList;
        private Vector3 lastPosition;
        private Random random = new Random();
        
        [System.Serializable]
        public class PlateformType
        {
            public String functionUsedName;
            public float heightToChange;
            public float distance;
        }
        
        public List<PlateformType> listOfPlateformUsable;
        public float minHeight = -12.0f;
        public float maxHeight = 50.0f;

    // Start is called before the first frame update
        void Start()
        {
            plateformsList = new List<GameObject>();
            lastPosition = Vector3.zero;
            InitFirstPillar();
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
        
        // Shuffle for a template of Pillar
        // ReSharper disable Unity.PerformanceAnalysis
        PlateformType ShuffleForNewPillar()
        {
            int i = 0;
            
            while (true)
            {
                i++;
                if (i == 10)
                {
                    UnityEngine.Debug.Log("Hello, this is a fail. no possibility was found !");
                    return listOfPlateformUsable[0];
                }
                int index = random.Next(listOfPlateformUsable.Count);
                PlateformType nextPlateform = listOfPlateformUsable[index];

                if (   nextPlateform.heightToChange + lastPosition.y >= minHeight
                    && nextPlateform.heightToChange + lastPosition.y <= maxHeight) {
                    return listOfPlateformUsable[index];
                }
            }
        }
            
        // Create a new Pillar
        public void CreatePillar()
        {
            GameObject lastPillar = plateformsList.First();
            GameObject go = GameObject.Instantiate(basicStructure, basicStructure.transform.parent);
            Vector3 newPosition = lastPosition;
            
            // 
            PlateformType newPlateformType = ShuffleForNewPillar();

            // CALCULATE NEW POS
            newPosition.z += newPlateformType.distance;
            newPosition.y += newPlateformType.heightToChange;
            
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
