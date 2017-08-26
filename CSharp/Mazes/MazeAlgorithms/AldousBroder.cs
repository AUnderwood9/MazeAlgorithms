using System;
using Grid;
using System.Collections.Generic;
using System.Text;
using static Grid.Grid;
using System.Linq;
using System.Diagnostics;

namespace MazeAlgorithms
{
    /// <summary>
    /// The Algorithm used to generate information for the maze and is used by another class to generate the actual rooms.
    /// This Algorithm is used to avoid the biases that the sidewinder and binary algorithms introduce to the maze to create a more uniform maze.
    /// </summary>
    public class AldousBroder :IMazeAlgorithm
    {
        private enum ValidConnectionDirections
        {
            NORTH = Direction.NORTH,
            EAST = Direction.EAST,
            SOUTH = Direction.SOUTH,
            WEST = Direction.WEST
        }

        public void TurnGridIntoMaze(IGrid gridToManipulate)
        {
            // Get the rooms that we are going to manipulate from the grid. For the sidewinder algorithm we will need a 2d array.
            List<Room> rooms = gridToManipulate.GetRooms();
            int unvisited = rooms.Count - 1;
            int roomIndex = 0;
            Room room = rooms.ElementAt(roomIndex);
            int numberOfRows = room.column;
            List<int> neighborIndex = new List<int>();

            // Get the number of rows and columns that we will manipulate
            int rows = gridToManipulate.GetRows();
            int columns = gridToManipulate.GetColumns();

            // Get a random number from 0-1 that will determine if a room will go either NORTH or EAST
            Random randomDirectionGenerator = new Random();

            while ( unvisited > 0 )
            {
                Debug.WriteLine("Current Index " + roomIndex);

                // The current room has been visited
                room.visited = true;
                List<Direction> boundries = RoomHelper.GetBoundriesRoomIsOn(room, rows, columns);

                ValidConnectionDirections? directionToConnect = null;

                // Get all of the indecies of the room if they are not out of bounds then they will be added to the neighbor index list.

                // Check for the northern room index
                if ((roomIndex += numberOfRows) < rooms.Count - 1)
                    neighborIndex.Add(roomIndex += numberOfRows);

                // Check for the southern room index
                if ((roomIndex -= numberOfRows) > 0)
                    neighborIndex.Add(roomIndex -= numberOfRows);

                // Check for the western room index
                if ((roomIndex -= 1) > 0)
                    neighborIndex.Add(roomIndex -= 1);

                //Check for the eastern room index
                if ((roomIndex += 1) < rooms.Count - 1)
                    neighborIndex.Add(roomIndex += 1);

                // Randomly select which room we may connect. 
                int randomDirectionIndex = neighborIndex[randomDirectionGenerator.Next(neighborIndex.Count - 1)];

                switch (randomDirectionIndex)
                {

                }

                // Determmine the direction of the neighbor we may connect to and get thier indexies
                if (IsRoomOnNorthBoundry(boundries) && IsRoomOnEastBoundry(boundries))
                {
                    roomIndex = 0;
                }
                else if (IsRoomOnNorthBoundry(boundries))
                {
                    directionToConnect = ValidConnectionDirections.EAST;

                    if ((roomIndex += 1) < rooms.Count - 1)
                        roomIndex += 1;
                    else
                        roomIndex = 0;
                }
                else if (IsRoomOnEastBoundry(boundries))
                {
                    directionToConnect = ValidConnectionDirections.NORTH;
                    if ((roomIndex += numberOfRows) < rooms.Count - 1)
                        roomIndex += numberOfRows;
                    else
                        roomIndex = 0;
                }
                // if the current room is not on a boundry then connect in a random direction
                else
                {
                    int direction = randomDirectionGenerator.Next(0, 2);

                    if (direction == 0)
                    {
                        directionToConnect = ValidConnectionDirections.NORTH;
                        
                        if ((roomIndex += numberOfRows) < rooms.Count - 1)
                            roomIndex += numberOfRows;
                        else
                            roomIndex = 0;
                    }
                    else if (direction == 1)
                    {
                        directionToConnect = ValidConnectionDirections.EAST;

                        if ((roomIndex += 1) < rooms.Count - 1)
                            roomIndex += 1;
                        else
                            roomIndex = 0;
                    }
                }

                // if there is a direction to connect to and if the neighbor that was randomly selected has been visited (If the neighbor has been connected to a room already)
                if (directionToConnect != null && !room.hasNeighbors())
                {
                    Debug.WriteLine("Number of unvisited " + unvisited.ToString());

                    unvisited--;

                    gridToManipulate.Connect(room, (Direction)directionToConnect);
                }


                room = rooms.ElementAt(roomIndex);
                //unvisited--;

            }
        }
        private bool IsRoomOnNorthBoundry(List<Direction> boundries)
        {
            return boundries.Contains(Direction.NORTH);
        }

        private bool IsRoomOnSouthBoundry(List<Direction> boundries)
        {
            return boundries.Contains(Direction.SOUTH);
        }

        private bool IsRoomOnEastBoundry(List<Direction> boundries)
        {
            return boundries.Contains(Direction.EAST);
        }

        private bool IsRoomOnWestBoundry(List<Direction> boundries)
        {
            return boundries.Contains(Direction.WEST);
        }


    }
}
