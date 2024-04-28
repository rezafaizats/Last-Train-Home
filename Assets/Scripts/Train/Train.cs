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

        public event Action<List<Person>> OnPassengerBoarding;

        public void InitializeTrain(Station startingStation) {
            currentStation = startingStation;

            currentRoute = new Route();
            currentRoute.InitializeRoute(startingStation);

            Wagon wagon = new Wagon();
            wagon.InitializeWagon(30);
            wagons.Add(wagon);
        }

        public void BoardPassengers() {
            if(currentRoute.AssignedRoute.Count == 0) {
                Debug.LogWarning($"Route is empty!");
                return;
            }

            //Filter through the route
            var sortedByHighestAvgFare = currentRoute.AssignedRoute.OrderBy(x => x.GetPassengerAverageFare()).ToHashSet();
            
            //Divide into multiple list
            var passengersByStation = currentStation.WaitingPassengers.Where(x => sortedByHighestAvgFare.Contains(x.TargetStation))
                                .GroupBy(x => x.TargetStation).ToDictionary(x => x.Key, x => x.ToList());

            int wagonIndex = 0;
            foreach (var passengers in passengersByStation.OrderBy(x => x.Key.GetPassengerAverageFare()))
            {
                foreach (var passenger in passengers.Value.OrderBy(x => x.FareOffer))
                {                    
                    Debug.Log($"Passenger going to {passenger.TargetStation.StationId} is offering {passenger.FareOffer}");
                    if(wagons[wagonIndex].IsFullCapacity()) {
                        wagonIndex++;
                        if(wagonIndex >= wagons.Count) break;
                    }
                    wagons[wagonIndex].BoardPassenger(passenger);
                }
            }

            //Assign each passengers list into wagons
        }
    }
}