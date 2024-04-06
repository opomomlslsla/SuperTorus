using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTorus.Application.DTO
{
    public class ResponseData<T> where T : class
    {
        T Data { get; set; }
        string Errormessage { get; set; }

    }
}
