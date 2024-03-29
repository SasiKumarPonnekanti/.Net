﻿# API in ASP.NET Core 5/6
1. ControllerBase a BAse class
	- ABstract BAse Class for API COntrollers with following specifications
		- Properties
			- HttpContext
				- A Channel for Http Request Execution
				- A Current HTTP Request under Executin
			- HttpRequest and 	HttpResponse
				- HttpRequest
					- Header Information
				- HttpResponse
					- THe typeof Response
			- ClaimsPrincipal	
				- Identity Service for COntrolling API/ASP.NET COre App access
		- Methods
			- Result Methods
				- Ok(), NotFound(), NoContent(), BadRequest(), COnflict()
				- OkResult, ObjectResult
2. API Controller class
	- USed for Accepting HTTP Requests for HTTP Action Methods
		- HttpGetAttribute
			- [HttpGet],[HttpGet("String-Template")]
				- String-Template: The URL Parameters
				- E.g.
					- [HttpGet]
						- A Get Request	
					- [HttpGet("{id}")]
						- A Get Request with URL Contains 'id' parameter value

		- HttpPostAttribute
		- HttpPutAttribute
		- HttpDeleteAttribute
	-  HTTP Action Methods
		- Methods those are accessed using HTTP Requests
	- RouteAttribute
		- Class that is used to eveluate an incomming HTTP URL so that It can b mapped with COntoller and its Action Method
			- http://server/mycontroller
				- Get Request
			- Route
				- Map the Request with mycontroller and its HttpGet Action Method
	- ApiControllerAttribute class
		- APplied on Controller to make it as API COntroller
		- THis is used to Map the Json Data from Body of HTTP POST and HTTP PUT Request to CLR Object (Entity class / Model class / Class with public properties) passed to the Http Post and Http Put action methods in API COntroller 
- If HttpAction Method attributes are not applied on API Method then following error will be generated by Swagger Json: -

SwaggerGeneratorException: Ambiguous HTTP method for action - Csore_API.Controllers.CategoryController.Get (Csore_API). Actions require an explicit HttpMethod binding for Swagger/OpenAPI 3.0
IN ABove case the CategoryCOntroller does not apply HttpAction methods attributes

- The actual Error Message (Developer Error Page) for Non-Applyung of Http Action Method Attrbutes
	- An unhandled exception occurred while processing the request.
AmbiguousMatchException: The request matched multiple endpoints. 

3. DEsign the API for Accepting Requests
	- https://www.webnethelper.com/2019/12/using-model-binders-in-aspnet-core.html
	- USe Model Binders aka Parameter Binders
		- FromBody: The Data received from HTTP Request Body for POST Request will be 1.Parsed  2. Mapped with CLR Object based on Property NAmes
		- USing QueryStrig for Posting data using URL
			- QueryStrig: Portion of URL After Question Mark in Name=Value pair
			- http://server/mycontroller/myaction?namae1=value1&name2=value2&name3=value3
			- FromQuery
				- USed to Parse the QueryString and Map Name=Value pait in its appearnace order with CLR Object that is the input parameter to Action Method
		- FromRoute
			- Parse the URL upto
				- Server/api/COntrollerName/ActionNAme/{ROUTE-VALUES-FROM-THIS-ONWARDS}
			- And Map with CLR Object that is the input parameter to Action Method
		- FromFOrm
			- The Form Post, The data is send using Name:Value pair
			- NAme Must match with Property from CLR Object
- SUbscribing to API
	- Use the HTTP CLient Methods to call API
		- System.Net.Http
		- System.Web.Http
		- MAke sure that the Corss-Origin-Resource-Sharing (CORS) is Enabled on API
	- USe Policies to Access API
		- Corss-Origin-Resource-Sharing (CORS)
			- Make SUre that the Caller is KNown to API
				- Origin, the NAme or IP Address of Web Site
				- Methods, check the HTTP Method Restrictions Get/Post/Put/Delete
				- Headers, Make sure that to Access API, the request MUST contains SOme HTTP HEader VAlues
					- e.g. AUTHORIZATION, X-VERSION
			- CORS is a Service and a Middleware that MUST be configured on API
```` csharp
builder.Services.AddCors(options => {
    options.AddPolicy("corspolicy", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
    });
});
````
- Add Mddleware
```` csharp
app.UseCors("corspolicy");
````


		- Security Policies
			- USer BAsed
			- USer + Role
			- USer + Role + Access Policies
			- USer + Role + Token
			- USer + Role + Token + Access Policies
	- USing Exception Management and Logging at Global Level
		- Implemented using Middeware
	- Crating a Custom Middleware
		- The class MUST be onstructor injected with 'RequestDelegate' delegate
		- The class MUST have an 'InvokeAsync(HttpContext)' method
			- This method wil contain Middleware LOgic
		- RequestDelegate
			- This is a Delegate taht is used to invoke an InvokeAsync() Method inside the Http Request and Response Pipeline
			- This delegate accepts an input parameter as 'HttpContext' with Void / Task as Output parameter
		- DEfine a new Extension Method class
			- Static Class with Static Method
				- This will be an extension method for IApplicationBuilder
				- The IApplicationBuilder is an interface hat is used to Register the Custom Middlware in Http Request Pipeline
			- The UseMiddleware<T>() method
				- An Extension method to IApplicationBuilder
					- T is the Custom Middleware class which is COnstructor injected using RequestDelegate 
