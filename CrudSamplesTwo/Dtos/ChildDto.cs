using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudSamplesTwo.Dtos
{
    //Only used to train on Filter child list in the DTOs
    internal class ChildDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public char Gender { get; set; }
    }
}
