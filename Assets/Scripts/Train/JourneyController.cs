using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RF
{    
    public class JourneyController : MonoBehaviour
    {
        [SerializeField] Button addRouteButton;
        [SerializeField] Button removeRouteButton;
        [SerializeField] Button clearRouteButton;

        private Train currentTrain;

        // Start is called before the first frame update
        void Start()
        {            
            currentTrain = new Train();
            currentTrain.InitializeTrain(StationController.Instance.AllStation[0]);
            StationController.Instance.AllStation[0].SelectStation();
            StationController.Instance.AllStation[0].HighlightConnectedStation();

            addRouteButton.onClick.AddListener( () => currentTrain.CurrentRoute.AddDestination(StationController.Instance.CurrentHighlightedStation));
            removeRouteButton.onClick.AddListener( () => currentTrain.CurrentRoute.RemoveLastDestination());
            clearRouteButton.onClick.AddListener( () => currentTrain.CurrentRoute.ClearRoute());
            clearRouteButton.onClick.AddListener( () => StationController.Instance.ClearHighlightStationButton(currentTrain.CurrentStation));
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}