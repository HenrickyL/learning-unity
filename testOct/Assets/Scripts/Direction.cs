public enum Direction
{
  N, NE, E, SE, S, SW, W, NW
  //non-oriented
    //NE, E, SE, SW, W, NW -> 1 2 3 5 6 7 (N, S)
  //oriented
    //N, NE, SE, S, SW, NW -> 0 1 3 4 5 7 (E, W)
}

public enum HexDirectionOriented{
  N, NE, SE, S, SW, NW
}

public enum HexDirectionNonOriented{
  NE, E, SE, SW, W, NW
}

public static class DirectionExtensions {

  public static Direction Opposite (this Direction direction) {
		return (Direction)(((int)direction+4)%8);
	}
  public static Direction Left (this Direction direction) {
    int dir = ((int)direction-1)%8;
		return (Direction)( dir<0? 8+dir:dir);
	}
  public static Direction Right (this Direction direction) {
		return (Direction)( ((int)direction+1)%8);
	}
	public static int HexOpposite (this int direction) {
		return (direction+3)%6;
	}

  public static int HexLeft (this int direction) {
    int dir = (direction-1)%6;
		return  dir<0? 6+dir:dir;
	}
  public static int HexDoubleLeft (this int direction) {
		return direction.HexLeft().HexLeft();
	}
  public static int HexRight (this int direction) {
		return ((int)direction+1)%6;
	}
  public static int HexDoubleRight (this int direction) {
		return direction.HexRight().HexRight();
	}
  public static Direction Direction(this int direction){
    return (Direction) (direction%8);
  }
  public static HexDirectionOriented HexDirectionO(this int direction){
    return (HexDirectionOriented) (direction%6);
  }
  public static HexDirectionNonOriented HexDirection(this int direction){
    return (HexDirectionNonOriented) (direction%6);
  }
}
