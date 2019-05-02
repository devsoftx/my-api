using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tffp_api.Model
{
    public class FormModel
    {
        public string Data { get; set; }
        public IFormFile File { get; set; }
    }
}
