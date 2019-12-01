namespace FF.Models
{
    public class ServiceError
    {
        public ServiceError() { }

        public ServiceError(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}
