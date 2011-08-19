using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Dynamic;
using System.IO;

namespace Highrise {
    
    //thanks to http://blogs.msdn.com/b/mcsuksoldev/archive/2010/02/04/dynamic-xml-reader-with-c-and-net-4-0.aspx
    //I modified to set the indexer to work with Elements instead of attributes
    public class XmlHelper: DynamicObject {
        public static dynamic Decode(string xml) {
            return new XmlHelper(xml);
        }
        XElement element;

        public XmlHelper(string xml) {
            byte[] byteArray = Encoding.UTF8.GetBytes(xml);
            using (var stream = new MemoryStream(byteArray)) {
                element = XElement.Load(stream);
            }
        }
        public XmlHelper(XElement el) {
            element = el;
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            if (element == null) {
                result = null;
                return false;
            }

            XElement sub = element.Element(binder.Name);

            if (sub == null) {
                result = null;
                return false;
            } else {
                result = new XmlHelper(sub);
                return true;
            }
        }

        public override string ToString() {
            if (element != null) {
                return element.Value;
            } else {
                return string.Empty;
            }
        }
        public string this[string attr] {
            get {
                if (element == null) {
                    return string.Empty;
                }

                return element.Element(attr).Value;
            }
        }
    }
}
