using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLTaskWPF
{
    public interface IXmlWorker

    {
        void Load(string xmlDocPath);
        void Add(Product product);
        void Delete(string name);
        Product FindBy(string name);

        List<Product> GetAll();
    }
}


