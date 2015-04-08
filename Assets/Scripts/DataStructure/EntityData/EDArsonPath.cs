using System.Collections.Generic;
using DataStructure.TileData;

namespace DataStructure.EntityData{
	public class EDArsonPath{
		private List<TDTile> pathSteps;
		private List<float> pathTimes;

		public EDArsonPath(List<TDTile> pathSteps, List<float> pathTimes){
			this.pathSteps = pathSteps;
			this.pathTimes = pathTimes;
		}

		public int GetStepCount(){
			return pathSteps.Count;
		}

		public bool HasMoreSteps(){
			return pathTimes.Count > 0 && pathSteps.Count > 0;
		}

		public bool TimeForNextStep(float elapsedTime){
			float nextStepTime = pathTimes [0];
			return elapsedTime >= nextStepTime;
		}

		public TDTile PopStep(){
			if (pathTimes.Count > 0) {
				pathTimes.RemoveAt (0);
			}

			TDTile popped = null;
			if (pathSteps.Count > 0) {
				popped = pathSteps[0];
				pathSteps.RemoveAt(0);
			}

			return popped;
		}
	}
}