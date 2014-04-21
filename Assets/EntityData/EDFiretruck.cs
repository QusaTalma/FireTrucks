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

	public TDStep PopPathStep(){
		return path.PopStep ();
	}
}