using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersTest.Models.Entities.Base
{
    public abstract class NamedEntity :BaseEntity
    {
        public string Name { get; set; }
    }
}
