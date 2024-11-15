using CutOptimizer;
using LumberStoreSystem.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberStoreSystem.BussinessLogic.Interfaces
{
    public interface INewCuttingOptimizationService
    {
        string Optimize(int boardWidth, int boardHeight, List<CuttingListItemModel> cuttingList, string clientName, DateTime orderDate, int orderId, string productName, string productId);
        int GroupItemsIntoBoards(List<CuttingListItemModel> cuttingList, int boardWidth, int boardHeight);
    }
}
