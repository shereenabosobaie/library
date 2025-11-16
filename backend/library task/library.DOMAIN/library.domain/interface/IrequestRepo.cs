using library.DOMAIN.library.domain.entities;
using library_mangment.library.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.DOMAIN.library.domain
{
    public interface IrequestRepo
    {
        bool CreatRequest(requests req);
        bool DeleteRequest(requests req);
        IEnumerable<requests> GetAllRequests();
        bool updaterequest(requests req);
        public requests? GetById(int id);
        

    }
}
