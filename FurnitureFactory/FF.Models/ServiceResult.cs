using System.Collections.Generic;
using System.Linq;

namespace FF.Models
{
    public class ServiceResult
    {
        private IList<ServiceError> _errors;

        public IList<ServiceError> Errors
        {
            get
            {
                _errors = _errors ?? new List<ServiceError>();
                return _errors;
            }
            set
            {
                _errors = value;
            }
        }

        public bool Succeeded => !Errors.Any();

        public static ServiceResult Failed(params ServiceError[] errors)
            => new ServiceResult
            {
                Errors = errors.ToList()
            };
    }
}
