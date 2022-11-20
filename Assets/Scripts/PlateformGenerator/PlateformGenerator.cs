using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
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
            public GameObject structureToUse;
        }
        
        public List<PlateformType> listOfPlateformUsable;
        public float minHeight = -12.0f;
        public float maxHeight = 50.0f;
        
        public int minOfPlateform = 10;

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

            for (int i = 0; i < minOfPlateform; i++)
            {
                CreatePillar();
            }
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
            
        // get new position for the next pillar
        public Vector3 GetNewPosition(PlateformType newPlateformType)
        {
            Vector3 newPos = lastPosition;

            // Set new X and Z pos
            double angle = random.Next(90) - 45;

            angle = angle * (Math.PI / 180);

            if (angle == 0)
            {
                newPos.z += newPlateformType.distance;
            }
            else
            {
                newPos.z += newPlateformType.distance * (float) Math.Cos(angle);
                newPos.x += newPlateformType.distance * (float) Math.Sin(angle);
            }
            

            // Change height
            newPos.y += newPlateformType.heightToChange;
            
            return newPos;
        }
        
        // Create a new Pillar
        public void CreatePillar()
        {
            GameObject lastPillar = plateformsList.First();
            
            // Random For new plateform
            PlateformType newPlateformType = ShuffleForNewPillar();
            
            // calculate new position for pillar
            Vector3 newPosition = GetNewPosition(newPlateformType);

            // Instantiate good gameobject
            GameObject go = GameObject.Instantiate(newPlateformType.structureToUse, newPlateformType.structureToUse.transform.parent);
            
            // SET OBJECT AND INFO
            go.transform.SetPositionAndRotation(newPosition, lastPillar.transform.rotation);
            lastPosition = newPosition;
            go.SetActive(true);
            plateformsList.Add(go);
        }
        
        // Update is called once per frame
        void Update()
        {
            // if (Input.GetKey(KeyCode.I))
            // {
            //     if (!isInit)
            //         InitFirstPillar();
            //     else
            //         CreatePillar();
            // }
            // if (Input.GetKey(KeyCode.O))
            // {
            //     Reset();
            // }
        }
    }
}
