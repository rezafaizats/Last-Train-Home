using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF {
    public class Route
    {
        private List<Station> destinations = new List<Station>();
        public List<Station> AssignedRoute => destinations;
        private Station startingDestination;
        
        public event Action<Route> OnDestinationUpdated;

        public void InitializeRoute(Station startingStation, Train currenTrain) {
            startingDestination = startingStation;
            currenTrain.OnTrainJourneyArrived += UpdateRoute;
        }

        public void AddDestination(Station nextStation) {
            if(nextStation is null) {
                Debug.LogWarning($"Station value is null.");
                return;
            }

            nextStation.SelectStation();
            nextStation.HighlightConnectedStation();
            destinations.Add(nextStation);
            OnDestinationUpdated?.Invoke(this);
        }

        public void RemoveLastDestination() {
            if(destinations.Count == 0) {
                Debug.LogWarning("Your destinations is empty!");
                return;
            }
            
            var station = destinations[destinations.Count - 1];
            station.ClearStation();
            destinations.RemoveAt(destinations.Count - 1);
            OnDestinationUpdated?.Invoke(this);
        }

        public void UpdateRoute(Train train) {
            if(destinations.Count == 1)
                destinations.Clear();
            else
                destinations.RemoveAt(0);
            startingDestination = train.CurrentStation;
        }

        public void ClearRoute() {
            destinations.Clear();
            OnDestinationUpdated?.Invoke(this);
        }
    }
}