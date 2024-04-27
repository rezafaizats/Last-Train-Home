using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RF
{
    public class Station : MonoBehaviour
    {
        [SerializeField] private Vector2 worldPosition;
        [SerializeField] private string stationId;
        public string StationId => stationId;
        private List<Person> waitingPassengers = new List<Person>();
        [SerializeField] private List<Station> connectedStations = new List<Station>();
        public List<Station> ConnectedStations => connectedStations;

        public event Action<Station> OnStationHighlight;
        public event Action<Station> OnStationSelected;
        public event Action<Station> OnStationCleared;

        public void InitializeStation() {
            GenerateWaitingPassengers();
        }

        public void GenerateWaitingPassengers() {
            for (int i = 0; i < StationController.Instance.AllStation.Count; i++)
            {
                if(StationController.Instance.AllStation[i] == this) continue;

                var passengerCount = Random.Range(20, 101);
                for (int j = 0; j < passengerCount; j++)
                {
                    var passenger = new Person();
                    passenger.InitializePerson(this, StationController.Instance.AllStation[i]);
                    waitingPassengers.Add(passenger);
                }
            }
        }

        public void ShowWaitingPassengersDemand() {
            List<StationInformation> information = new List<StationInformation>();

            for (int i = 0; i < StationController.Instance.AllStation.Count; i++)
            {
                if(StationController.Instance.AllStation[i] == this) continue;

                int totalPassenger = 0;
                float totalFare = 0;
                for (int j = 0; j < waitingPassengers.Count; j++)
                {
                    if(waitingPassengers[j].TargetStation == StationController.Instance.AllStation[i]) {
                        totalPassenger++;
                        totalFare += waitingPassengers[j].FareOffer;
                    }
                }

                var passengerDemand = GetPassengerAverageCount(totalPassenger);
                var passengerOfferings = GetPassengerAverageFare(totalFare, totalPassenger);
                var newStationInfo = new StationInformation(StationController.Instance.AllStation[i].stationId, passengerDemand, passengerOfferings);
                information.Add(newStationInfo);
            }

            StationController.Instance.UpdateStationInfo(information);
        }

        public float GetDistance(Station target) {
            return Vector2.Distance(worldPosition, target.worldPosition);
        }

        public void HighlightConnectedStation() {
            OnStationHighlight?.Invoke(this);
        }
        
        public void SelectStation() {
            OnStationSelected?.Invoke(this);
        }
        
        public void ClearStation() {
            OnStationCleared?.Invoke(this);
        }

        public string GetPassengerAverageFare(float allPassengerOffer, int passengerCount) {
            string fareOffer = "~ " + (allPassengerOffer / passengerCount).ToString("F0") + "$";
            return fareOffer;
        }

        public string GetPassengerAverageCount(int passengerCount) {
            
            int lowQuarter = waitingPassengers.Count / 4;
            int average = waitingPassengers.Count / 2;
            int highQuarter = ((waitingPassengers.Count / 2) + waitingPassengers.Count) / 2;

            //Show passenger demands
            switch (passengerCount)
            {
                case int x when (x < lowQuarter):
                    //Low
                    return "Low";
                case int x when (x > lowQuarter && x < average):
                    //Medium Low
                    return "Medium Low";
                case int x when (x > average && x < highQuarter):
                    //Medium High
                    return "Medium High";
                default:
                    //High
                    return "High";
            }
        }
    }
}