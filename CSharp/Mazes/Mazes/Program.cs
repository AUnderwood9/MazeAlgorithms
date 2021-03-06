﻿using MazeAlgorithms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grid;
using static Grid.Grid;
using DistanceAlgorithms;

namespace Mazes
{
    class Program
    {
        const int ROWS = 10;
        const int COLUMNS = 10;

        const int WIDTH_IN_PIXLES = 500;
        const int HEITH_IN_PIXLES = 500;

        const int PEN_THICKNESS_IN_PIXLES = 3;
        static void Main(string[] args)
        {
            IGrid grid = new Grid.Grid(ROWS, COLUMNS);
            IMazeAlgorithm binaryAlgorithm = new Binary();
            IDistanceAlgorithm solver = new Rectangular();

            MazeGenerator.Generator generator = new MazeGenerator.Generator(grid, binaryAlgorithm, solver);
            generator.ApplyAlgorithm();
            int maxDistance = generator.SolveMaze(new GridPosition(0, 0));
            

            Bitmap gridPng = new Bitmap(WIDTH_IN_PIXLES, HEITH_IN_PIXLES);
            Graphics tool = Graphics.FromImage(gridPng);
            Pen blackPen = new Pen(Color.Black, PEN_THICKNESS_IN_PIXLES);

            

            List<Room> rooms = grid.GetRooms();

            int cellWidth = WIDTH_IN_PIXLES / COLUMNS;
            int cellHeigth = HEITH_IN_PIXLES / ROWS;

            foreach (Room room in rooms)
            {
                int xOfUpperLeft = room.column * cellWidth;
                int yOfUpperLeft = room.row * cellHeigth;
                int xOfLowerRight = (room.column * cellWidth) + cellWidth;
                int yOfLowerRight = (room.row * cellHeigth) + cellHeigth;

                if (!room.Neighbors().Contains(Direction.NORTH))
                {
                    tool.DrawLine(blackPen, xOfUpperLeft, yOfUpperLeft, xOfLowerRight, yOfUpperLeft);
                }

                if (!room.Neighbors().Contains(Direction.WEST))
                {
                    tool.DrawLine(blackPen, xOfUpperLeft, yOfUpperLeft, xOfUpperLeft, yOfLowerRight);
                }

                if(!room.Neighbors().Contains(Direction.EAST))
                {
                    tool.DrawLine(blackPen, xOfLowerRight, yOfUpperLeft, xOfLowerRight, yOfLowerRight);
                }

                if(!room.Neighbors().Contains(Direction.SOUTH))
                {
                    tool.DrawLine(blackPen, xOfUpperLeft, yOfLowerRight, xOfLowerRight, yOfLowerRight);
                }


                double intensity = ((double)maxDistance - (double)room.distance) / ((double)maxDistance);
                double dark = Math.Round((255d * intensity));
                double bright = 128d + Math.Round(127d * intensity);

                Color roomColor = Color.FromArgb((int)dark, (int)bright, (int)dark);

                Brush roomColorBrush = new SolidBrush(roomColor);

                tool.FillRectangle(roomColorBrush, xOfUpperLeft, yOfUpperLeft, cellWidth, cellHeigth);
                
            }

            gridPng.Save("Hello.png");
            blackPen.Dispose();
            tool.Dispose();
            gridPng.Dispose();
        }
    }
}
