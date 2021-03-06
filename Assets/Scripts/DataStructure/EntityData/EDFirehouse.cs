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

namespace DataStructure.EntityData{
	public class EDFirehouse {

		//Requested locations for new trucks to go to
		private List<Vector2> _truckSpawnQueue;

		public EDFirehouse () {
			_truckSpawnQueue = new List<Vector2> ();
		}

		public List<Vector2> GetSpawnQueue(){
			return _truckSpawnQueue;
		}

		public void AddPositionToSpawnQueue(Vector2 truckPosition){
			_truckSpawnQueue.Add (truckPosition);
		}

		public Vector2 PopSpawnQueue(){
			Vector2 popped = new Vector2();

			if(_truckSpawnQueue.Count > 0){
				popped = _truckSpawnQueue[0];
				_truckSpawnQueue.Remove(popped);
			}

			return popped;
		}
	}
}