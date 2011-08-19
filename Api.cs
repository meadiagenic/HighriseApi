﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Highrise {
    public class Api {
        string _apiKey;
        string _siteDomain;
        string _baseUrl;
        public Api(string apiKey, string siteDomain) {
            _apiKey = apiKey;
            _siteDomain = siteDomain;
            _baseUrl = "https://"+siteDomain+".highrisehq.com/";
        }

        /// <summary>
        /// Adds a person to Highrise. Edit the XML template as-needed to add more info
        /// </summary>
        public dynamic AddPerson(string first, string last, string email, params string[] tags) {
            var url = string.Format(_baseUrl + "people.xml", email);
            var xml = Templates.PersonBasic(first, last, email);
            var response = Send(url, xml);
            var p = ToDynamic(response);
            if (tags.Length > 0)
                AddTags(p, tags);
            return ToDynamic(response);
        }

        /// <summary>
        /// Pulls down a Customer record from Highrise
        /// </summary>
        public dynamic GetPerson(string email) {
            //GET /people.xml?title=CEO
            var url = string.Format(_baseUrl+ "people.xml?email={0}", email);
            var result = Send(url, verb: "GET");
            return ToSingleDynamic(result);
        }

        /// <summary>
        /// Adds a note to a Customer's record
        /// </summary>
        public dynamic AddNote(string email, string note) {
            var person = GetPerson(email);
            var url = _baseUrl + "people/"+person.id+"/notes.xml";
            var xml = Templates.NewNote(person.id, "", note);
            var response = Send(url, xml);
            return ToDynamic(response);
        }

        /// <summary>
        /// Tags a Customer
        /// </summary>
        public void AddTags(string email, params string[] tags){
            var person = GetPerson(email);
            AddTags(person, tags);
        }

        /// <summary>
        /// Tags a Customer
        /// </summary>
        public void AddTags(dynamic person, params string[] tags) {
            var url = _baseUrl + "people/"+person.id+"/tags.xml";
            foreach (var tag in tags) {
                Send(url, Templates.NewTag(tag));
            }
        }

        /// <summary>
        /// Pops an XML string into an XDoc
        /// </summary>
        XDocument ToXDoc(string xml) {
            byte[] byteArray = Encoding.UTF8.GetBytes(xml);
            XDocument doc;
            using (var stream = new MemoryStream(byteArray)) {
                doc = XDocument.Load(stream);
            }
            return doc;
        }
        /// <summary>
        /// Uses the XMLHelper to wrap some XML in dynamic love - this helper will return the first element
        /// </summary>
        dynamic ToSingleDynamic(string xml) {
            return new XmlHelper(ToXDoc(xml).Descendants().First().Descendants().First()); 
        }
        /// <summary>
        /// Uses the XMLHelper to wrap some XML in dynamic love
        /// </summary>
        dynamic ToDynamic(string xml) {
            return new XmlHelper(ToXDoc(xml).Descendants().First());
        }
        /// <summary>
        /// A simple GET request to the Highrise API
        /// </summary>
        string Send(string url, string data="", string verb = "POST") {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = verb;
            request.ContentType = "application/xml";
            
            var cred = new NetworkCredential(userName: _apiKey, password: "X");
            request.Credentials = cred;
            
            if (!string.IsNullOrEmpty(data)) {
                //add the form data
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                request.ContentLength = byteArray.Length;
                using (var stream = request.GetRequestStream()) {
                    // Write the data to the request stream.
                    stream.Write(byteArray, 0, byteArray.Length);
                }
            }

            var response = (HttpWebResponse)request.GetResponse();
            string result = "";

            using (Stream stream = response.GetResponseStream()) {
                StreamReader sr = new StreamReader(stream);
                result = sr.ReadToEnd();
                sr.Close();
            }
            return result;
        }
    
    }


}