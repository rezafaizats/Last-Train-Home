using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RF
{    
    [Serializable]
    public class Person
    {
        private Station targetStation;
        private float fareOffer;
        public Station TargetStation => targetStation;
        public float FareOffer => fareOffer;

        public void InitializePerson(Station startingStation, Station target) {
            targetStation = target;
            var baseOffer = startingStation.GetDistance(target) * 10;
            if(baseOffer >= 10 && baseOffer <= 20) {
                baseOffer -= Random.Range(3, 7);
            }
            else {
                baseOffer -= Random.Range(5, 9);
            }
            fareOffer = baseOffer;
        }
    }
}