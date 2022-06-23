using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLTaskWPF
{
    public class Product
    {
        public string Name { get; set; }
        public int ExpirationDate { get; set; }
        public int Price { get; set; }
        public override string ToString()
        {
            return new StringBuilder()
                .AppendLine($"--->Product is {Name}")
                .AppendLine($"\tExpiration Date is {ExpirationDate}")
                .AppendLine($"\tPrice: {Price} BYN")
                .ToString();
        }
    }
}      
        
   