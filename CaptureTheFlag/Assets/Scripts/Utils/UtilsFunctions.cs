using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class UtilFunctions
    {
        public static GameObject SearchForObjectInHierarchy(Transform parent, string nameOfGameObject)
        {
            
            // Check if the parent contains the object named "FlagPos"
            GameObject flagPosObject = parent.Find(nameOfGameObject)?.gameObject;

            // If the object is found, do something with it
            if (flagPosObject != null)
            {
                return flagPosObject;
            }

            // Recursively search through all children of the parent
            foreach (Transform child in parent)
            {
                SearchForObjectInHierarchy(child, nameOfGameObject);
            }

            return null;
        }
    }
}