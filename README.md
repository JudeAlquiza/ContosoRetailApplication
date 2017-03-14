# Contoso Retail Application
## Introduction
This is a sample application created to test, prototype, and evaluate DevExtreme's data grid, it consists of a web api back end, and an angular2 + typescript client web app, it also comes with a sample database, the ConstosoRetailDW database.

The grid loads data from a table containing almost 12 million records. Performance is great so far for sorting, paging, and filtering. 

## Cloning the repo
When cloning the repo, make sure to install the node packages first, I'm using visual studio code for client side web development.

I'm using Visual Studio 2015 for working with the web api. When opening the web api in Visual Studio 2015, make sure to restore the nuget packages first. 

When running locally, make sure to change the AppUrl by right clicking on the web api project in the solution explorer inside of Visual Studio 2015, then go to properties, then go to Debug, set the AppUrl to http://localhost:55392/.

As for the database, I cannot upload it here since the maximum size of a file that can be uploaded is only 100MB. Kindly download the sample database [here](https://www.microsoft.com/en-us/download/details.aspx?id=18279). Make sure to download only ContosoBIdemoBAK.exe which is around 630MB.

Make sure to update the connection string in the app.config files in BC_ContosoRecordsModule.DataAccess, BC_ContosoRecordsModule.DataModel, and ContosoRetail.WebAPI. 

```xml
<configuration>
    ... some other coder here ... 
    <connectionStrings>
        <add name="ContosoRecordsConnectionString" 
             connectionString="<paste connection string here depending on your setup>"                                                                providerName="System.Data.SqlClient" />
    </connectionStrings>
</configuration>
```

## Borrowed Code
I've copied some of the code from [DevExtreme.AspNet.Data](https://github.com/DevExpress/DevExtreme.AspNet.Data/tree/master/net/DevExtreme.AspNet.Data), note that I cannot use the original one on the lower layers of the app this is why I only copied some of the code that I need so that I could wrap the load options properly.

## Open Issues
#### a. 
When grouping is initiated and the user goes into the next page, the client doesn't make another request to the server to fetch data for that page.
#### b. 
Performance issues with grouping (I might need to revisit this since I modified some of the codes a bit here, but will be greatly          appreciated if you'll also confirm this one)

## Solved Issues
#### a. 
When grouping is initiated, paging doesn't work properly. Even if the total count is passed, the pager only displays pages for the        paged results instead of the whole.  
   - Solution: Include the items count for each grouping in the response data.
#### b. 
When grouping is initiated, it includes the group row headers in the row count. This means that if I have page size set to 10 rows        and three group row headers are on page 1, the grid only displays 7 of them because it adds the group row headers into the count          for that page, the excess records are then pushed into page 2.
   - Solution: This behaviour is by design and cannot be modified.

## Appreciation
I just want you to know that so far DevExtreme performs great, and the support that we're getting from you is the best :) Really appreciate it. 
Thanks in advance, for checking this as this is really really urgent.

Thanks!
Jude Alquiza.
