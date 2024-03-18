using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ServiceModel
{
    [ServiceContract]
    public interface INewsService
    {
        [OperationContract] string GetGroupData(string groupName);
    }
}
