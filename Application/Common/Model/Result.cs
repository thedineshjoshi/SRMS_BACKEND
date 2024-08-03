using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Model
{
    public class Result
    {
        public bool Succeded { get; set; }
        public string[] Errors { get; set; }
        public Result(bool succeded, IEnumerable<string> errors) 
        {
            Succeded = succeded;
            Errors = errors.ToArray();

        }
        public static Result Success()
        {
            return new Result(true, Array.Empty<string>());
        }
        public static Result Failure (IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }
    }
}
