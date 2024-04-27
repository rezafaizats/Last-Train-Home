using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RF
{    
    public class StationInformationUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI infoText;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            StationController.Instance.OnStationInfoUpdate += UpdateStationInformation;
        }

        public void UpdateStationInformation(List<StationInformation> informations) {
            infoText.text = "";
            foreach (var info in informations)
            {
                string textInfo = ($"{info.stationId} : {info.demand}, Average Offer : {info.fareOffer}\n");
                infoText.text += textInfo;
            }
        }
    }
}