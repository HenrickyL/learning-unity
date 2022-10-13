public enum Direction
{
  N, NE, E, SE, S, SW, W, NW
  //non-oriented
    //NE, E, SE, SW, W, NW -> 1 2 3 5 6 7 (N, S)
  //oriented
    //N, NE, SE, S, SW, NW -> 0 1 3 4 5 7 (E, W)
}

public static class DirectionExtensions {

	// public static Direction Opposite (this Direction direction,bool) {
	// 	return (Direction)( ((int)direction+4)%8);
	// }
  public static Direction Opposite (this Direction direction,bool oriented=true) {
    if(oriented){
      if(direction == Direction.E || direction == Direction.W)
        direction = direction+1;
    }else{
      if(direction == Direction.N || direction == Direction.S)
        direction = direction+1;
    }
    var res = ((int)direction+4)%8;
    res = oriented? 
      (res == 2 || res == 6 ? res+1: res):
      (res == 0 || res ==4)? res+1: res;
		return (Direction)(res);
	}
  public static Direction Left (this Direction direction) {
    int dir = ((int)direction-1)%8;
		return (Direction)( dir<0? 8+dir:dir);
	}
  public static Direction Right (this Direction direction) {
		return (Direction)( ((int)direction+1)%8);
	}
}
