using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class Train
    {
        private Station currentStation;
        public Station CurrentStation => currentStation;
        private List<Wagon> wagons;
        private Route currentRoute;
        public Route CurrentRoute => currentRoute;
        private float speed;

        public void InitializeTrain(Station startingStation) {
            currentStation = startingStation;

            currentRoute = new Route();
            currentRoute.InitializeRoute(startingStation);
        }

        public void AssignPassengers(List<Person> allPassenger) {
            //Filter through next route
            
            //Divide into multiple list

            //Assign each passengers list into wagons
        }
    }
}