using Azure.Core;
using library.DOMAIN.library.domain;
using library.DOMAIN.library.domain.entities;
using library_mangment.library.Infrastructure.appdbcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.intrastructure.library.Infrastructure.Repos
{
    public class requestRepo : IrequestRepo
    {
        private readonly Dbcontext _context;
        public requestRepo(Dbcontext context) => _context = context;

        

        public bool CreatRequest(requests req)
        {
            _context.Requests.Add(req);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteRequest(requests req)
        {
            var request = _context.Requests.FirstOrDefault(r => r.Id == req.Id);
            if (request == null) return false;

            _context.Requests.Remove(request);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<requests> GetAllRequests()
        {
            return _context.Requests.ToList();
        }

 
        public bool updaterequest(requests req)
        {
            _context.Requests.Update(req);
            return _context.SaveChanges() > 0;
        }
        public requests? GetById(int id)
        {
            return _context.Requests.FirstOrDefault(r => r.Id == id);
        }
    }
}
