using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RF
{    
    public class StationController : MonoBehaviour
    {
        public static StationController Instance {get; private set;}

        [SerializeField] private List<Station> allStation = new List<Station>();
        public List<Station> AllStation => allStation;

        private Station currentHighlightedStation = null;
        public Station CurrentHighlightedStation => currentHighlightedStation;

        private Dictionary<Station, Button> stationButton = new Dictionary<Station, Button>();
        private Dictionary<Station, Button> selectedStationButton = new Dictionary<Station, Button>();

        public event Action<List<StationInformation>> OnStationInfoUpdate;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (Instance != null && Instance != this) 
                Destroy(this);
            else
                Instance = this;
            
            foreach (var station in allStation)
            {
                station.InitializeStation();
                if(station.TryGetComponent<Button>(out var button)) {
                    button.onClick.AddListener( () => ShowStationInformation(station));
                }
                stationButton.Add(station, button);

                station.OnStationHighlight += HighlightStationButton;
                station.OnStationSelected += SelectedStationButton;
                station.OnStationCleared += ClearStationButton;
            }
        }

        public void ShowStationInformation(Station station) {
            currentHighlightedStation = station;
            station.ShowWaitingPassengersDemand();
        }

        public void UpdateStationInfo(List<StationInformation> information) {
            OnStationInfoUpdate?.Invoke(information);
        }

        public float GetStationOffer(Station station) {
            return station.GetPassengerAverageFare();
        }

        public void HighlightStationButton(Station station) {
            
            foreach (var item in stationButton)
            {
                if(selectedStationButton.ContainsKey(item.Key)) continue;

                item.Value.image.color = Color.white;
            }

            foreach (var connectedStation in station.ConnectedStations)
            {
                if(selectedStationButton.ContainsKey(connectedStation)) continue;

                stationButton[connectedStation].image.color = Color.green;
            }
        }
        
        public void SelectedStationButton(Station station) {
            selectedStationButton.Add(station, stationButton[station]);
            stationButton[station].image.color = Color.blue;
        }

        public void ClearStationButton(Station station) {
            selectedStationButton.Remove(station);
            selectedStationButton.Last().Key.HighlightConnectedStation();
        }

        public void ClearHighlightStationButton(Station startingStation) {
            foreach (var station in stationButton)
            {
                if(station.Key == startingStation) continue;

                station.Value.image.color = Color.white;
            }
        }
    }
}