using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RF
{    
    public class JourneyController : MonoBehaviour
    {
        [SerializeField] Button addRouteButton;
        [SerializeField] Button removeRouteButton;
        [SerializeField] Button clearRouteButton;
        [SerializeField] Button setRouteButton;

        [SerializeField] Button departButton;
        [SerializeField] TextMeshProUGUI journeyETATextUI;

        private Train currentTrain;

        // Start is called before the first frame update
        void Start()
        {            
            currentTrain = new Train();
            currentTrain.InitializeTrain(StationController.Instance.AllStation[0]);
            StationController.Instance.AllStation[0].SelectStation();
            StationController.Instance.AllStation[0].HighlightConnectedStation();
            currentTrain.OnTrainJourneyUpdate += UpdateTrainJourneyUI;
            currentTrain.OnTrainJourneyArrived += UpdateTrainJourneyUI;

            journeyETATextUI.text = $"Train is on station {currentTrain.CurrentStation.StationId}";

            addRouteButton.onClick.AddListener( () => currentTrain.CurrentRoute.AddDestination(StationController.Instance.CurrentHighlightedStation));
            removeRouteButton.onClick.AddListener( () => currentTrain.CurrentRoute.RemoveLastDestination());
            clearRouteButton.onClick.AddListener( () => currentTrain.CurrentRoute.ClearRoute());
            clearRouteButton.onClick.AddListener( () => StationController.Instance.ClearHighlightStationButton(currentTrain.CurrentStation));
            setRouteButton.onClick.AddListener( () => currentTrain.BoardPassengers());
            departButton.onClick.AddListener( () => currentTrain.DepartTrain());
        }

        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        void FixedUpdate()
        {
            if(currentTrain.IsTrainDeparted) {
                currentTrain.UpdateTrainJourney();
            }
        }

        public void UpdateTrainJourneyUI(Train train) {
            string journeyTextUI = $"Train is on station {train.CurrentStation}";
            if(train.IsTrainArrivedAtNextStation) 
                journeyTextUI = $"Train has arrived at station {train.CurrentStation.StationId}";
            else
                journeyTextUI = $"Train on journey to station {train.CurrentRoute.AssignedRoute[0].StationId}. {Environment.NewLine} ETA : {train.CurrentETAToNextStation.ToString("F0")}";
            journeyETATextUI.text = journeyTextUI;
        }
    }
}