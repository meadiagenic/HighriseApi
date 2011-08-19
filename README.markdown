The Highrise API Wrapper for .NET 4
=====================================

This is a very, very basic wrapper for working with the Highrise API. Everything is here for you to expand and love as needed. Currently, it simply adds a Customer, tags, and notes. It will also pull down a Customer using Email.

How To Install It?
------------------
Reference the Highrise.dll in your project. Get your API Key from AccountSettings/MyInfo

How Do You Use It?
------------------
It's pretty simple to use:

    var highrise = new Highrise.Api("API_KEY", "subdomain");
    var person = highrise.AddPerson("Joe", "Blow", "joe@blow.com","tag1","tag2");

    Console.WriteLine("Dude's ID: " + person.id);


XML. Blech.
-----------
Highrise is built to work with ActiveResource (for Rails) so there's a lot of XML here, and no JSON unfortunately. To that end, dynamics are your friend and I've included an XMLHelper/Wrapper for turning an XML string into a dynamic object.