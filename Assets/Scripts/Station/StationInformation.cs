using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    [Serializable]
    public class StationInformation
    {
        public string stationId;
        public string demand;
        public string fareOffer;

        public StationInformation(string stationId, string demand, string fareOffer) {
            this.stationId = stationId;
            this.demand = demand;
            this.fareOffer = fareOffer;
        }
    }
}