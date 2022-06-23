using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLTaskWPF
{
    public class XmlDocumentWorker : IXmlWorker
    {
        private readonly XmlDocument _document;
        private readonly ILogger _logger;
        private string _xmlFilePath;
        public XmlDocumentWorker(ILogger logger)
        {
            _logger = logger;
            _document = new XmlDocument();
        }
        public void Add(Product product)
        {
            var xRoot = _document.DocumentElement;
            XmlElement productElem = _document.CreateElement("product");
            XmlAttribute nameAttribute = _document.CreateAttribute("name");
            XmlText nameText = _document.CreateTextNode(product.Name);
            nameAttribute.AppendChild(nameText);
            XmlElement expirationDateElem = _document.CreateElement("expirationDate");
            XmlText expirationDateInnerText = _document.CreateTextNode(product.ExpirationDate.ToString());
            expirationDateElem.AppendChild(expirationDateInnerText);
            productElem.AppendChild(expirationDateElem);
            XmlElement priceElem = _document.CreateElement("price");
            XmlText priceInnerText = _document.CreateTextNode(product.Price.ToString());
            priceElem.AppendChild(priceInnerText);
            xRoot.AppendChild(productElem);
            _document.Save(_xmlFilePath);
        }
        public void Delete(string name)
        {
            var xRoot = _document.DocumentElement;
            foreach (XmlNode xNode in xRoot)
            {
                if (xNode.Attributes.Count > 0)
                {
                    var attributeName = xNode.Attributes.GetNamedItem("name");
                    try
                    {
                        var attributeNameText = attributeName?.InnerText;
                        if (attributeName.InnerText.Equals(name))
                        {
                            xRoot.RemoveChild(xNode);
                        }
                    }
                    catch (Exception ex) when (ex is XmlException || ex is NullReferenceException)
                    {
                        _logger.LogWarning(ex.Message, nameof(attributeName));
                    }
                }
            }
        }
        public Product FindBy(string name)
        {
            Product product = null;
            var xRoot = _document.DocumentElement;
            foreach (XmlNode xmlNode in xRoot)
            {
                product = GetProduct(xmlNode);
                if (product.Name.Equals(name))
                {
                    return product;
                }
            }
            return product;
        }
        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();
            var xRoot = _document.DocumentElement;
            foreach (XmlNode node in xRoot)
            {
                var product = GetProduct(node);
                products.Add(product);
            }
            return products;
        }
        public void Load(string xmlFilePath)
        {
            _xmlFilePath = xmlFilePath;
            _document.Load(xmlFilePath);
        }
        private Product GetProduct(XmlNode node)
        {
            var product = new Product();
            if (node.Attributes.Count > 0)
            {
                var attributeName = node.Attributes.GetNamedItem("name");
                product.Name = attributeName?.Value;
            }
            foreach (XmlNode childNode in node.ChildNodes)
            {
                try
                {
                    if (childNode.Name.Equals("expirationDate"))
                    {
                        product.ExpirationDate = int.Parse(childNode.InnerText);
                    }
                    if (childNode.Name.Equals("price"))
                    {
                        product.Price = int.Parse(childNode.InnerText);
                    }
                }
                catch (Exception ex) when (ex is FormatException || ex is NullReferenceException)
                {
                    _logger.LogError(ex.Message, ex.StackTrace, nameof(childNode.InnerText));
                }
            }
            return product;
        }
    }
}
