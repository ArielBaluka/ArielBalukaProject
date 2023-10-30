using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BaseEntity
    {
        protected int id;
        public int ID 
        { 
            get { return id; } 
            set { id = value; } 
        }
    }
}
