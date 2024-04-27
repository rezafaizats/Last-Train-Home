using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{    
    public class TrainController : MonoBehaviour
    {
        public static TrainController Instance {get; private set;}

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (Instance != null && Instance != this) 
                Destroy(this);
            else
                Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}