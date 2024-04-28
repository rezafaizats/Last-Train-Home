using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RF
{
    public class Train
    {
        private Station currentStation;
        public Station CurrentStation => currentStation;
        private List<Wagon> wagons = new List<Wagon>();
        private Route currentRoute;
        public Route CurrentRoute => currentRoute;
        private float speed;
        private float currentDistanceToNextStation;
        private float currentETAToNextStation;
        public float CurrentETAToNextStation => currentETAToNextStation;
        private bool isTrainDeparted = false;
        public bool IsTrainDeparted => isTrainDeparted;
        private bool isTrainArrivedAtNextStation = false;
        public bool IsTrainArrivedAtNextStation => isTrainArrivedAtNextStation;

        public event Action<Train> OnTrainJourneyUpdate;
        public event Action<Train> OnTrainJourneyArrived;
        public event Action<float> OnTrainDisembarkPassengers;

        public void InitializeTrain(Station startingStation) {
            currentStation = startingStation;
            currentETAToNextStation = 0f;
            speed = 1f;

            currentRoute = new Route();
            currentRoute.InitializeRoute(startingStation, this);

            Wagon wagon = new Wagon();
            wagon.InitializeWagon(30);
            wagons.Add(wagon);
            Debug.Log($"Wagon length is {wagons.Count}");
        }

        public void BoardPassengers() {
            if(currentRoute.AssignedRoute.Count == 0) {
                Debug.LogWarning($"Route is empty!");
                return;
            }

            //Filter through the route
            var sortedByHighestAvgFare = currentRoute.AssignedRoute.OrderBy(x => x.GetPassengerAverageFare()).ToHashSet();
            
            //Assign each passengers list into wagons
            var passengersByStation = currentStation.WaitingPassengers.Where(x => sortedByHighestAvgFare.Contains(x.TargetStation))
                                .GroupBy(x => x.TargetStation).ToDictionary(x => x.Key, x => x.ToList());

            int wagonIndex = 0;
            foreach (var passengers in passengersByStation.OrderBy(x => x.Key.GetPassengerAverageFare()))
            {
                foreach (var passenger in passengers.Value.OrderByDescending(x => x.FareOffer))
                {                    
                    if(wagons[wagonIndex].IsFullCapacity()) {
                        wagonIndex++;
                        if(wagonIndex >= wagons.Count) break;
                    }
                    wagons[wagonIndex].BoardPassenger(passenger);
                    Debug.Log($"Boarding passenger going to {passenger.TargetStation.StationId} with offering {passenger.FareOffer}");
                }
                if(wagonIndex >= wagons.Count) break;
            }
        }

        public void DisembarkPassengers() {
            float moneyGained = 0;
            List<Person> disembarkingPassenger = new List<Person>();

            foreach (var wagon in wagons)
            {
                foreach (var passenger in wagon.Passengers)
                {
                    if(!passenger.TargetStation == currentRoute.AssignedRoute[0]) continue;

                    moneyGained += passenger.FareOffer;
                    disembarkingPassenger.Add(passenger);
                }
                wagon.DisembarkPassengers(disembarkingPassenger);
                disembarkingPassenger.Clear();
            }
            OnTrainDisembarkPassengers?.Invoke(moneyGained);
        }

        public void DepartTrain() {
            if(currentRoute.AssignedRoute.Count <= 0) {
                Debug.LogWarning("Currently there are no assigned route");
                return;
            }

            isTrainDeparted = true;
            isTrainArrivedAtNextStation = false;
        }

        public void UpdateTrainJourney() {
            float distanceToNextStation = Vector2.Distance(currentStation.WorldPosition, currentRoute.AssignedRoute[0].WorldPosition);
            if(distanceToNextStation <= currentDistanceToNextStation || isTrainArrivedAtNextStation) {
                Debug.Log($"Train is arrived on station {currentRoute.AssignedRoute[0].StationId}");
                //Disembark Pasengers
                DisembarkPassengers();

                //Reset parameter
                isTrainDeparted = false;
                isTrainArrivedAtNextStation = true;
                currentStation = currentRoute.AssignedRoute[0];
                currentDistanceToNextStation = 0f;
                OnTrainJourneyArrived?.Invoke(this);
                return;
            }

            currentETAToNextStation = JourneyETAInSeconds(currentRoute.AssignedRoute[0]);
            OnTrainJourneyUpdate?.Invoke(this);
        }

        public float JourneyETAInSeconds(Station destination) {
            float distance = Vector2.Distance(currentStation.WorldPosition, destination.WorldPosition);
            currentDistanceToNextStation += speed * Time.deltaTime;
            float journeyETA = distance - currentDistanceToNextStation;
            return journeyETA / speed;
        }

    }
}