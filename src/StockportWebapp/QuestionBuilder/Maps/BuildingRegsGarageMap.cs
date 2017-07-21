using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockportWebapp.Dtos;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Maps
{
    public class BuildingRegsGarageMap : IMap<BuildingRegsGarageModel>
    {
        public BuildingRegsGarageModel Map(IList<Answer> answers)
        {
            var request = new BuildingRegsGarageModel();

            return request;
        }
    }
}
