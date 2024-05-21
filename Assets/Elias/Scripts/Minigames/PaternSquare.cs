using UnityEngine;

namespace Elias.Scripts.Minigames
{
    public class PatternSquare : MonoBehaviour
    {
        [System.Flags]
        public enum Direction
        {
            None = 0,
            Up = 1,
            Right = 2,
            Down = 4,
            Left = 8
        }

        public Direction direction;
        public int angle; // 0, 90, 180, 270

        private void Start()
        {
            // Ensure the initial rotation is applied
            transform.localEulerAngles = new Vector3(0, 0, angle);
        }

        public void RotateSquare()
        {
            angle = (angle + 90) % 360;
            transform.localEulerAngles = new Vector3(0, 0, angle);
            direction = RotateDirection(direction);
        }

        private Direction RotateDirection(Direction dir)
        {
            int dirInt = (int)dir;
            for (int i = 0; i < 1; i++) // Rotate by 90 degrees once
            {
                dirInt = ((dirInt & 1) << 3) | (dirInt >> 1);
            }
            return (Direction)dirInt;
        }

        public bool IsConnected(PatternSquare other, Direction directionToOther)
        {
            return (direction & directionToOther) != 0 && (other.direction & OppositeDirection(directionToOther)) != 0;
        }

        private Direction OppositeDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up: return Direction.Down;
                case Direction.Right: return Direction.Left;
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
                default: return Direction.None;
            }
        }
    }
}