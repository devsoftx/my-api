using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tffp_api.Model
{
    public class TransactionLineResponse
    {
        private readonly List<string> errors;
        public TransactionLineResponse()
        {
            this.errors = new List<string>();
            this.Lines = new List<TransactionUnderline>();
        }
        public ICollection<TransactionUnderline> Lines { get; set; }
        public int Count { get { return this.Lines.Count(); } }
        public bool Success
        {
            get
            {
                return !this.errors.Any();
            }
        }

        public void AddError(string error)
        {
            this.errors.Add(error);
        }

        public List<string> Errors
        {
            get
            {
                return this.errors;
            }
        }
    }
}