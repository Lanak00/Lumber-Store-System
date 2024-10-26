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
        int CalculateNumberOfBoards(int boardWidth, int boardHeight, List<CuttingListItemModel> cuttingList);
    }
}
