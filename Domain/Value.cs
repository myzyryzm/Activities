using System;

namespace Domain
{
    public class Value
    {
        //entity framework; create code then 
        //knows that b/c Id is integer and first in class then it will know to give the Value an Id when it is made
        public int Id {get; set;}
        public string Name {get; set;}
    }
}
