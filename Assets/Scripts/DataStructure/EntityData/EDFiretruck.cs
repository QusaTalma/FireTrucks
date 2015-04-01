using DataStructure.TileData;

namespace DataStructure.EntityData{
	public class EDFiretruck{
		float speed;
		TDPath path;

		public float GetSpeed(){
			return speed;
		}

		public void SetSpeed(float speed){
			this.speed = speed;
		}

		public void SetPath(TDPath path){
			this.path = path;
		}

		public bool HasNextStep(){
			return path != null && path.GetStepCount() > 0;
		}

		public TDStep PopPathStep(){
			return path.PopStep();
		}

		public TDStep PeekPathStep(){
			return path.PeekStep ();
		}
	}
}