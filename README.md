yammerbot
=========

A bot to respond to Yammer posts. There are some assumptions made for this bot:

1. Low rate of messages being posted (maximum ever posted in a 30 second period is probably five) which makes sure we never need to exceed the API rate limit for retreiving messages. I plan to add a rate limiter at the service call level some day
2. You want to respond to all messages throughout the company.
3. You already have at least one message in your database, or there are very few messages in the target network. If you have a large number of messages in your network, and no messages stored in your database, the program will loop until it fetches them all, and you could exceed your rate limit



The first time you execute, you will probably need to load some data. Here's some code (put into Program.cs of the Runner):

    var fetcher = kernel.Get<IYammerMessageFetcher>();
    var databaseManager = kernel.Get<IYammerMessageDatabaseManager>();

    var messages = fetcher.GetLatestMessages();
    databaseManager.SaveMessages(messages);
    databaseManager.SaveChanges();

This will load the latest 20 messages from Yammer, and save them into the database without looping. Run this before or in place of the looping code in Program.cs. This creates a sort of checkpoint, and the bot will not attempt to fetch messages before this point.

You will be missing some NuGet packages. Use the Package Restore functionality to retreive the necessary packages.

The bot uses Entity Framework Code First. You will need to have permissions to create the database file.

The app assumes the Runner project has an app.config file that stores your keys and whatnot. Here is a sample:

    <configuration>
        <startup> 
            <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
        </startup>
      <appSettings>
        <add key="ConsumerKey" value="MyConsumerKey"/>
        <add key="ConsumerSecret" value="MyConsumerSecret"/>
        <add key="Token" value="MyToken"/>
        <add key="TokenSecret" value="MyTokenSecret"/>
      </appSettings>
    </configuration>



This bot can also be used to retreive all messages in your Yammer network, but there's no built-in functionality yet.