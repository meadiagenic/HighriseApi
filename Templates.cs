using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Highrise {
    public static class Templates {
        public static string PersonBasic(string first, string last, string email) {
                return string.Format(@"<person>
    <first-name>{0}</first-name>
    <last-name>{1}</last-name>
    <contact-data>
        <email-addresses>
            <email-address>
                <address>{2}</address>
                <location>Work</location>
            </email-address>
        </email-addresses>
    </contact-data>
</person>",first,last,email);
        }

        public static string NewNote(object id, string subject, string note) {
            return string.Format(@"<note>
  <body>{0}</body>
  <subject-id type='integer'>{1}</subject-id>
  <subject-type>{2}</subject-type>
</note>",note,id,subject);
        }
        public static string NewTag(string tag) {
            return string.Format(@"<name>{0}</name>", tag);
        }
    }
}
