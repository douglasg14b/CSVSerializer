# CSVSerializer
A simple easy to use CSV serializer

##Usage

###Basics

This library allows you to take any object and turn it into a CSV. You can either ask the CsvSerializer to write to file for you, return a single string formatted as a CSV, or return a list of strings that is each line of the CSV (without line breaks). 

**Note:** This will only serialize your properties, it will ignore fields.

You also have a few options when it comes to your column headers (first row): 
* You can either let the CsvSerializer handle it automatically and it will write the column headers based on the names you assigned the properties in your class. 
* You can provide a list of strings that are identicle to your class's properties, except you may change their capitilization and spacing (ie. `conversationNumber` can have a header of `Conversation Number`).
* You can provide a list of `CustomHeaders` in which you write out your property name and what you wish the associated column header to be.

####To write your collection of objects to a CSV:
Use any of the `WriteCSV()` methods.
####To formated your collection of objects as a single string:
Use any of the `GetCSVString()` methods.
####To Format your collection of strings as a collection of rows:
Use any of the `GetCSVRows()` methods.

###Examples

**Automatically Assign Columns and Headers:**

    using CSVSerialization;
    CSVSerializer<MyObject> CsvWriter = new CSVSerializer<MyObject>();
    CsvWriter.WriteCSV("MyFolder\MyFile.csv", List<MyObject>);

**Assign your own modified column headers (based on your properties names):**

    using CSVSerialization;
    CSVSerializer<MyObject> CsvWriter = new CSVSerializer<MyObject>();
    List<string> columnNames = new List<string>()
    {
      "Total Conversations",
      "User ID"
    };
    CsvWriter.Write("MyFolder\MyFile.csv", List<MyObject>, columnNames);
  
**Assign your own custom column headers:**

    using CSVSerialization;
    CSVSerializer<MyObject> CsvWriter = new CSVSerializer<MyObject>();
    List<CustomHeaders> columnNames = new List<CustomHeaders>()
    {
      new CustomHeader("totalConversations", "Total Daily Conversations"),
      new CustomHeader("userID", "User Unique ID")
    };
    CsvWriter.Write("MyFolder\MyFile.csv", List<MyObject>, columnNames);

    



