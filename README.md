README
======

This sample ASP.NET MVC project demonstrates how to discover Telerik Reporting .trdx files in a directory, list them as links, and then render using the HTML5 report viewer.


Dependencies
------------
- This project depends on Telerik Reporting 2015 Q2.
- The Report Service is hosted via WebAPI (http://docs.telerik.com/reporting/telerik-reporting-rest-service-web-api)
- (Be sure to execute `Telerik.Reporting.Services.WebApi.ReportsControllerConfiguration.RegisterRoutes(config);` to register the routes. See `/App_Start/WebApiConfig.cs`)
- If reports call databases, then you must have connection strings defined in `web.config` that match the connection string names in the reports.

Reports
-------

- AnotherReport.trdx: A report with a string and a date parameter, default values are embedded in the reports
- ASimpleReport.trdx: A report with no parameters
- HelloReport.trdx: A report with a string parameter but no default value specified (to demonstrate that the report viewer will display an error until the parameter value is specified)