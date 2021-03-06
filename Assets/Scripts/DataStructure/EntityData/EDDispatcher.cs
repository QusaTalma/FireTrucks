//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1022
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using Behaviors.EntityGraphics;

namespace DataStructure.EntityData{
	public class EDDispatcher {

		//Actual trucks that exist
		private List<EGFiretruck> _activeTrucks;
		private List<EGFiretruck> _idleTrucks;

		public EDDispatcher () {
			_activeTrucks = new List<EGFiretruck> ();
			_idleTrucks = new List<EGFiretruck> ();
		}

		public List<EGFiretruck> GetActiveTrucks(){
			return _activeTrucks;
		}

		public List<EGFiretruck> GetIdleTrucks(){
			return _idleTrucks;
		}

		public void AddIdleTruck(EGFiretruck truck){
			_idleTrucks.Add(truck);
		}

		public void AddActiveTruck(EGFiretruck truck){
			_activeTrucks.Add(truck);
		}

		public void RemoveIdleTruck(EGFiretruck truck){
			_idleTrucks.Remove(truck);
		}

		public void RemoveActiveTruck(EGFiretruck truck){
			_activeTrucks.Remove(truck);
		}

		public void RemoveTruck(EGFiretruck truck){
			if(_idleTrucks.Contains(truck)){
				_idleTrucks.Remove(truck);
			}else if(_activeTrucks.Contains(truck)){
				_activeTrucks.Remove(truck);
			}
		}

		public EGFiretruck GetClosestIdleTruck(Vector3 pos){
			EGFiretruck closest = null;
			float shortestDist = -1;

			for (int i=0; i<_idleTrucks.Count; i++) {
				EGFiretruck truck = _idleTrucks[i];
				float dist = Vector3.Distance(truck.transform.position, pos);
				if(closest == null){
					closest = truck;
					shortestDist = dist;
				}else if(dist < shortestDist){
					closest = truck;
					shortestDist = dist;
				}
			}

			if (closest != null) {
				_idleTrucks.Remove(closest);
			}

			return closest;
		}

		public EGFiretruck PopIdleTruck(){
			EGFiretruck poppedTruck = null;
			if (_idleTrucks.Count > 0) {
				poppedTruck = _idleTrucks[0];
				_idleTrucks.Remove(poppedTruck);
			}
			
			return poppedTruck;
		}

		public EGFiretruck GetIdleTruckAtIndex(int index){
			EGFiretruck truck = null;
			
			if(index >= 0 && index < _idleTrucks.Count){
				truck = _idleTrucks[index];
			}
			
			return truck;
		}

		public EGFiretruck GetActiveTruckAtIndex(int index){
			EGFiretruck truck = null;

			if(index >= 0 && index < _activeTrucks.Count){
				truck = _activeTrucks[index];
			}

			return truck;
		}

		public int GetTruckCount(){
			return _idleTrucks.Count + _activeTrucks.Count;
		}
	}
}