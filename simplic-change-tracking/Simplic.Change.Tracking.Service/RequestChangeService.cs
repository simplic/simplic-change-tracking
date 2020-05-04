using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Change.Tracking.Service
{
    public class RequestChangeService : IRequestChangeService
    {
        IRequestChangeRepository requestChangeRepository;
        public RequestChangeService(IRequestChangeRepository requestChangeRepository)
        {
            this.requestChangeRepository = requestChangeRepository;
        }
        public bool Delete(RequestChange obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public RequestChange Get(Guid id)
        {
            return requestChangeRepository.Get(id);
        }

        public IEnumerable<RequestChange> GetAll()
        {
            return requestChangeRepository.GetAll();
        }

        public bool Save(RequestChange obj)
        {
            return requestChangeRepository.Save(obj);
        }
    }
}
