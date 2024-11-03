using CutOptimizer;
using LumberStoreSystem.Contracts;
using LumberStoreSystem.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Interfaces
{
    public interface  ICuttingOptimizationService
    {
       string Optimize(int boardWidth, int boardHeight, List<CuttingListItemModel> cuttingList, string clientName, DateTime orderDate, int orderId, string productName, string productId);
       List<List<CutPiece>> GroupItemsIntoBoards(List<CuttingListItemModel> cuttingList, int boardWidth, int boardHeight);
    }
}
