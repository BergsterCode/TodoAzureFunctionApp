using System;
using System.Collections.Generic;
using System.Text;

namespace TodoAzureFunctionApp
{
    class TodoUpdateModel
    {
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
}
