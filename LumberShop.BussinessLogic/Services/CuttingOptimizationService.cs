using Google.OrTools.Sat;
using LumberStoreSystem.BussinessLogic.Interfaces;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace LumberStoreSystem.BussinessLogic.Services
{
    public class CuttingOptimizationService : ICuttingOptimizationService
    {
        public int CalculateNumberOfBoards(int boardWidth, int boardHeight, List<CuttingListItemModel> cuttingList)
        {
            // Expand the cutting list based on the amount of each piece
            List<CuttingListItem> expandedCuttingList = new List<CuttingListItem>();

            foreach (var item in cuttingList)
            {
                for (int i = 0; i < item.Amount; i++)
                {
                    if (item.Width > boardWidth || item.Length > boardHeight)
                    {
                        throw new ArgumentException("A piece is too large to fit on the board.");
                    }

                    expandedCuttingList.Add(new CuttingListItem
                    {
                        Width = item.Width,
                        Length = item.Length
                    });
                }
            }

            // Sort pieces by area (larger ones first) for better packing
            expandedCuttingList.Sort((a, b) => (b.Width * b.Length).CompareTo(a.Width * a.Length));

            CpModel model = new CpModel();

            int numPieces = expandedCuttingList.Count;
            int maxBoards = numPieces; // Maximum number of boards (1 piece per board is the worst-case scenario)

            // Variables to indicate if a board is used
            BoolVar[] boardUsed = new BoolVar[maxBoards];

            // Variables for piece assignments and positions
            BoolVar[,] assigned = new BoolVar[maxBoards, numPieces];
            IntVar[,] xVars = new IntVar[maxBoards, numPieces];
            IntVar[,] yVars = new IntVar[maxBoards, numPieces];

            for (int b = 0; b < maxBoards; b++)
            {
                boardUsed[b] = model.NewBoolVar($"board_used_{b}");

                for (int p = 0; p < numPieces; p++)
                {
                    CuttingListItem piece = expandedCuttingList[p];

                    // Assign each piece to a board
                    assigned[b, p] = model.NewBoolVar($"assigned_b{b}_p{p}");

                    // Position variables for each piece
                    xVars[b, p] = model.NewIntVar(0, boardWidth - piece.Width, $"x_b{b}_p{p}");
                    yVars[b, p] = model.NewIntVar(0, boardHeight - piece.Length, $"y_b{b}_p{p}");

                    // If a piece is assigned to a board, that board must be used
                    model.AddImplication(assigned[b, p], boardUsed[b]);
                }
            }

            // Each piece must be assigned to exactly one board
            for (int p = 0; p < numPieces; p++)
            {
                List<ILiteral> assignments = new List<ILiteral>();
                for (int b = 0; b < maxBoards; b++)
                {
                    assignments.Add(assigned[b, p]);
                }
                model.AddExactlyOne(assignments);
            }

            // Non-overlapping constraints for each board
            for (int b = 0; b < maxBoards; b++)
            {
                for (int p = 0; p < numPieces; p++)
                {
                    for (int q = p + 1; q < numPieces; q++)
                    {
                        // Non-overlapping constraints: either pieces must be separated horizontally or vertically

                        // Create Boolean variables representing each condition
                        BoolVar leftOf = model.NewBoolVar($"left_of_b{b}_p{p}_q{q}");
                        BoolVar rightOf = model.NewBoolVar($"right_of_b{b}_p{p}_q{q}");
                        BoolVar aboveOf = model.NewBoolVar($"above_of_b{b}_p{p}_q{q}");
                        BoolVar belowOf = model.NewBoolVar($"below_of_b{b}_p{p}_q{q}");

                        // Add constraints that bind the Boolean variables to the actual conditions
                        model.Add(xVars[b, p] + expandedCuttingList[p].Width <= xVars[b, q]).OnlyEnforceIf(leftOf);
                        model.Add(xVars[b, q] + expandedCuttingList[q].Width <= xVars[b, p]).OnlyEnforceIf(rightOf);
                        model.Add(yVars[b, p] + expandedCuttingList[p].Length <= yVars[b, q]).OnlyEnforceIf(aboveOf);
                        model.Add(yVars[b, q] + expandedCuttingList[q].Length <= yVars[b, p]).OnlyEnforceIf(belowOf);

                        // Ensure that at least one of the non-overlapping conditions holds if both pieces are assigned to the same board
                        model.AddBoolOr(new ILiteral[] { leftOf, rightOf, aboveOf, belowOf }).OnlyEnforceIf(new ILiteral[] { assigned[b, p], assigned[b, q] });
                    }
                }
            }


            // Objective: Minimize the number of boards used
            model.Minimize(LinearExpr.Sum(boardUsed));

            // Solve the model
            CpSolver solver = new CpSolver();
            solver.StringParameters = "max_time_in_seconds:60"; // Adjust time limit if necessary
            CpSolverStatus status = solver.Solve(model);

            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
            {
                int boardsUsed = 0;
                for (int b = 0; b < maxBoards; b++)
                {
                    if (solver.BooleanValue(boardUsed[b]))
                    {
                        boardsUsed++;
                        Console.WriteLine($"Board {b + 1} used:");
                        for (int p = 0; p < numPieces; p++)
                        {
                            if (solver.BooleanValue(assigned[b, p]))
                            {
                                CuttingListItem piece = expandedCuttingList[p];
                                long xVal = solver.Value(xVars[b, p]);
                                long yVal = solver.Value(yVars[b, p]);

                                Console.WriteLine($" Piece {p + 1} at (x={xVal}, y={yVal}), width={piece.Width}, length={piece.Length}");
                            }
                        }
                    }
                }
                return boardsUsed;
            }
            else
            {
                Console.WriteLine("No solution found.");
                return -1;
            }
        }
    }
}
