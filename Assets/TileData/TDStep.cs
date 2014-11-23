public class TDStep {
	public TDTile tile;
	public TDStep (TDTile tile) {
		this.tile = tile;
	}

	public override string ToString(){
		return tile.ToString ();
	}
}