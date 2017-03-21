# ASP.NET Core Security

There are three major aspects of ASP.NET Core security that developers should consider, these are **encryption**, **authentication**, and **authorization**.

## 0. Introduction

**Encryption** in the context of web security is when a data that is passed thorugh the wire is encrypted to protect it from eavesdroppers that has malicious intentions.

**Authentication** is something that is done to validate identity. The user has to give some sort of credentials and these credentials are validated to prove the identity of the user.

**Authorization** is something that is done after a user has been authenticated. The system verifies the rights of a user to a specific resource.

## 1. Encryption

In ASP.NET Core and in web security in general the most common way of making sure that data is encrypted is using **SSL (secure sockets layer)**

### 1.1 Supporting SSL in ASP.NET Core

Before continuing, just want to note that we can use any examples we want, but in this particular discussion, I'm using the [ContosoRetailApplication](https://github.com/JudeAlquiza/ContosoRetailApplication) sample application that is currently in my github repository. We can either clone this or use some other sample app or even our own sample to follow along.

Inside of our ASP.NET Core project, go to <code>Startup.cs</code> and inside of <code>ConfigureServices()</code> method, pass a lambda to the <code>services.AddMvc()</code> method call as follows. Before doing this make sure that you add a reference to <code>Microsoft.AspNetCore.Mvc</code>.

   ``` C#
     public void ConfigureServices(IServiceCollection services)
     {
        // some code here

        services.AddMvc(config => 
              {
                config.Filters.Add(new RequireHttpsAttribute());
                
                // some other code here
              });

        // some code here
     }
   ```

Right click on the ASP.NET Core project and go to properties, then go to Debug, make sure to set enable SSL under web server settings. Note that this will only work if we're using IIS Express as our development web server. This will generate an SSL that we can use to test (the standard SSL port is 443).

![Enable SSL in ASP.NET Core project](https://github.com/JudeAlquiza/ContosoRetailApplication/blob/master/Research/Security/1.1.1.b.1.JPG)

   Alternatively, we can open launchSettings.json and set the SSL port here.

   ![Enable SSL in ASP.NET Core project](https://github.com/JudeAlquiza/ContosoRetailApplication/blob/master/Research/Security/1.1.1.b.3.JPG)

   Note also that this will generate what is called a **self-signing certificate** which generally is not secure and should only be use for development purposes.

   When we test this inside of chrome (in this example go to https://localhost:44316/api/customerorders, the SSL port may vary depending on what is automatically assigned to you or what your settings are), we will get a message that says that the certificate we're using is not secure.

   ![Enable SSL in ASP.NET Core project](https://github.com/JudeAlquiza/ContosoRetailApplication/blob/master/Research/Security/1.1.1.b.2.JPG)

   Click on '**proceed to localhost**' to make sure that the API call is still working with SSL configured.

## 2. Authentication

There are two major categories of authentication, one is **app authentication**, and the other is **user authentication**. We'll focus only on user authentication in this section and leave the discussion of app authentication some other time.

**ASP.NET Identity** is ASP.NET Core's Identity API that manages user accounts, logins, passwords, roles, claims, and a whole lot more that has to do with authentication and authorization. It supports both cookie authentication and token authentication.

The first thing that we need to do is setup ASP.NET Core to use ASP.NET Identity. 

### 2.1 Setting up ASP.NET Identity

First make sure that the **Microsoft.AspNetCore.Identity** NuGet package is installed in our ASP.NET Core project.

  ![Using ASP.NET Identity](https://github.com/JudeAlquiza/ContosoRetailApplication/blob/master/Research/Security/2.1.1.a.1.JPG)  

Go to Startup.cs and inside of **ConfigureServices** method, add and configure the identity service.
   ``` C#
     public void ConfigureServices(IServiceCollection services)
     {
        // some code here

        services.AddIdentity<SampleUser,SampleRole>()
                .AddEntityFrameworkStores<SampleContext>();

        // some code here
     }
   ```
Where SampleUser is a type that derives from **IdentityUser**, SampleRole is a type that derives from **IdentityRole**, and 
SampleContext is the context where the persistence of objects of type SampleUser and SampleRole are being managed.

Also note that in an actual implementation, if the User and Role class are on a seperate layer, for example on a data layer, 
registration of dependencies might need to take some other form to accomodate data transfer objects and view models.

Still inside of Startup.cs inside of the **Configure()** method, add the following code to use identity.

   ``` C#
     public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
     {
        // some code here

        app.UseIdentity();

        app.UseMvc();
     }
   ```
It is important that we add the call to use identity before the call to use Mvc because it's the Mvc middleware that we want to 
protect.

At this point we can already test it only to find that anonymous users can still access the app. This is because we need to configure 
the app what resources we want to protect from anonymous access. This is achieved by adding the **Authorize** attribute.

One way of doing this is at the controller level. In the code snippet below we're allowing anonymous access to the two **Get** 
controller actions but not with the **Post**, **Put**, and **Delete** controller actions. These three controller actions need the the 
user accessing the app to be authenticated first before that use can gain access to these resources.

   ``` C#
    [Route("api/[controller]")]
    public class BuildingsController : Controller
    {       
        [HttpGet]
        public IActionResult Get()
        {
            // some code here       
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // some code here
        }

        [Authorize] // User needs to be authenticated
        [HttpPost]
        public IActionResult Post([FromBody]SomeViewModel vm)
        {
            // some code here
        }

        [Authorize] // User needs to be authenticated
        [HttpPut]
        public IActionResult Put([FromBody]SomeViewModel vm)
        {
            // some code here
        }

        [Authorize] // User needs to be authenticated
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // some code here
        }       
    }
   ```
A much preferred way of doing this is to add the **Authorize** attribute at the controller level. This will make all controller 
actions protected from anonymous access. If we want to override this for some of the controller actions, we can make use of the 
**AllowAnonymous** attribute as shown below.

   ``` C#
    [Authorize] // User needs to be authenticated to gain access to the resources on this controller.
    [Route("api/[controller]")]
    public class SomeController : Controller
    {     
        // This will override the Authorize attribute for this controller action, and allow anonymous
        // access. 
        [AllowAnonymous] 
        [HttpGet]
        public IActionResult Get()
        {
            // some code here       
        }

        // This will override the Authorize attribute for this controller action, and allow anonymous
        // access. 
        [AllowAnonymous] 
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // some code here
        }
       
        [HttpPost]
        public IActionResult Post([FromBody]SomeViewModel vm)
        {
            // some code here
        }
       
        [HttpPut]
        public IActionResult Put([FromBody]SomeViewModel vm)
        {
            // some code here
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // some code here
        }       
    }
   ```
At this point we can already test it, if we're developing an api instead of a web application, if   we try to access the api in chrome we will be redirected to something like (for this example) **'https://locahost:44316/Account/Login?ReturnUrl=someUrlHere'**. This might look a bit strange at    first but the way ASP.NET Core works is that right now, MVC is integrated into WebAPI, when we access the WebAPI in a browser it thinks that we want to be redirected to a login page which is not we want. 

Since we are developing an api, chances are we will not be making these requests in a browser, that is the request can either come from a javascript client or a mobile client, and instead of the application redirecting us to a page, we want specific status codes to be returned instead. That is for users that are not properly authenticated, we want to receive an Http status code of **401 (unauthorized)**, and for authenticated users that don't have access to the resource, we want to receive an Htpp status code of **403 (Forbidden)**.

To make this work, as a final step, we need to go back to the Startup.cs and add the following configuration inside of the **ConfigureServices()** method.

   ``` C#
     public void ConfigureServices(IServiceCollection services)
     {
        // some code here

        services.AddIdentity<TUser,TRole>()
                .AddEntityFrameworkStores<TContext>();

        services.Configure<IdentityOptions>(config =>
        {
            config.Cookies.ApplicationCookieEvents = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = (ctx) =>
                    {
                        // We can skip this check if this is a web api only project and 
                        // just set the status code.
                        if (ctx.Request.Path.StartsWithSegments("/api") && 
                            ctx.Response.StatusCode == (int)HttpStatusCodes.OK)
                        {
                            // If this is an api call, instead of redirecting to the login page,
                            // we'll just return the status code.
                            ctx.Response.StatusCode == (int)HttpStatusCodes.Unauthorized;
                        }

                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = (ctx) =>
                    {
                        // We can skip this check if this is a web api only project and 
                        // just set the status code.
                        if (ctx.Request.Path.StartsWithSegments("/api") && 
                            ctx.Response.StatusCode == (int)HttpStatusCodes.OK)
                        {
                            // If this is an api call, instead of redirecting to the access denied 
                            // page, we'll just return the status code.
                            ctx.Response.StatusCode == (int)HttpStatusCodes.Forbidden;
                        }

                        return Task.CompletedTask;
                    }
                };
        });

        // some code here
     }
   ```
There are pros and cons in using ASP.NET Identity, for one it is simple to use, already built in with ASP.NET Core, and just works out of the box.

The main downside is that this approach tends to slow down the workflow of the users because the user credentials has to be validated on every request.

With this in mind, we'll move and examine our next alternative which is cookie authentication.

### 2.2 Using Cookie Authentication

The way cookie authentication works is that when the user logs in, it sends its credentials to the server, the server validates the credentials, authenticates the user and sends a response with the cookie attached to it. This cookie is now included in every subsequent request to the server which is validated by the server, instead of validating the credentials.

One way to implement cookie authentication in ASP.NET Core is as follows. Note that this is just a very simple implementation and might differ depending on the actual application's architecture, this is just to show a simple implementation and how cookie authentication works.

Since we will be needing the username and password from the user, the first thing to do is to create a model that will hold these information, let's call it UserCredentialsModel.cs.

   ``` C#
    public class UserCredentialsModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
   ```
Let's create an API controller that will handle authentication, let's call it AuthController.cs, and let's add a Login action to it.

   ``` C#
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private SignInManager<SampleUser> _signInManager;

        public AuthController(
            // The SampleUser type here is the SampleUser that we used earlier
            SignInManager<SampleUser> signInManager)
        {
            _signInManager = signInmanager;
        }

        // We want this to be an Http POST so that the credentials won't be in 
        // the query string, specially the password, it has to be one level down
        [HttpPost("login")]
        public aysnc Task<IActionResult> Login([FromBody]UserCredentialsModel model)
        {
             try
             {
                 if (!ModelState.IsValid) return BadRequest("Failed to login");

                 var result = await _signInManager.PasswordSignInAsync(model.Username,      
                                model.Password, isPersistence: false, lockoutOnFailure: false); 

                 if (result.Succeeded) return Ok();            
             }
             catch (Exception ex)
             {
                // some code here to handle exceptions
             }

             return BadRequest("Failed to login");
        }
    }
   ```
   Note that in order for us to test this, we must have at least one user saved in the database that we can use.

If we try login with an Http POST to the Login action of this controller, we'll get a response with a cookie attached to it. This 
   cookie will be attached to each succeeding request the user makes and doesn't need to login. This cookie is valid only with the 
   session of the client that is accessing it, for example if we're accessing it using a browser, then if we close the browser, we need 
   to authenticate again.

While this alleviates the downside of relying purely on ASP.NET Identity and might be a promising solution indeed, a major drawback with this approach is that cookie based authentication is stateful. It still has to keep track of a sessionID in the server for a particular cookie. It needs to do this to keep track of the validity of the cookie for a given session.

Now we'll move to the most prevalent alternative to all of this which is token based authentication.

### 2.3 Using Token Authentication

Token authentication has been tied to what is called JSON Web Tokens or JWTs and these JWTs are the actual 'Tokens' in Token Authentication. These JWTs has been the standard eversince in systems and application frameworks that implements token authentication.

Token-based authentication is stateless. The server does not keep a record of which users are logged in or which tokens have been issued. Instead, every request to the server is accompanied by a token which the server uses to verify the authenticity of the request.

How this work is the same as how cookie authentication works, the main difference is how the the token is used and structured that makes it stateless.

There are two steps that is done by the server to implement token authtentication, first is to generate the token, and second, validate the token.

Before we proceed to the implementation of generating a JWT, we must first know what is the structure of a JWT. A JWT is composed of a header and a payload. The header is consist of the algorithm for the JWT, that is the algorithm used to encrypt the JWT, and the type of token which in most cases is "JWT". A sample of this JWT header is shown below

   ``` Javascript 
   // Example of JWT header
   {
      "alg": "HS256",
      "typ": "JWT"
   }
   ```
The JWT payload on the other hand consists of some information that the server may want, for instance, the name of the user, roles, a set claims, etc.

   ``` Javascript 
   // Example of JWT payload
   {
      "sub": "12345312345561",
      "name": "Jude",
      "admin": true
   }
   ```
That is **JWT = JWT HEADER + JWT PAYLOAD**, this token is issued by the server upon when a user is successfully authenticated. It gets encrypted first in a special way before it gets dropped to the response that is then sent back to the client. The token that is sent back then gets attached to all subsequent request and gets validated by the server.

First we'll show how JWTs are generated in ASP.NET Core. 

In the AuthController.cs, let's pass a **UserManager** and a **PasswordHasher** in the constructor.

   ``` C#
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private SignInManager<SampleUser> _signInManager;
        private UserManager<SampleUser> _userManager;
        private IPasswordHasher<SampleUser> _hasher;

        public AuthController(
            SignInManager<SampleUser> signInManager,
            UserManager<SampleUser> userManager,
            IPasswordHasher<SampleUser> hasher)
        {
            _signInManager = signInmanager;
            _userInManager = userInmanager;
            _hasher = hasher;
        }

        // ... Some code here
    }
   ```

Inside of the AuthController.cs, let's add a CreateToken action to it. This CreateToken action needs to retrieve the user using the      UserManager, validate the password from the model against this user, and then generate the token. These steps are specified in the 
code snippet below.

   ``` C#
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private SignInManager<SampleUser> _signInManager;

        public AuthController(
            // The SampleUser type here is the SampleUser that we used earlier
            SignInManager<SampleUser> signInManager)
        {
            _signInManager = signInmanager;
        }

        [HttpPost("login")]
        public aysnc Task<IActionResult> Login([FromBody]UserCredentialsModel model)
        {
             // ... Some code here
        }

        [HttpPost("token")]
        public aysnc Task<IActionResult> CreateToken([FromBody]UserCredentialsModel model)
        {
             try
             {
                 if (!ModelState.IsValid) return BadRequest("Failed to login");

                 // Retrieve the user.
                 var user = await _userManager.FindByNameAsync(model.Username); 

                 if (user != null) 
                 {
                     // Validate the password.
                     if (_hasher.VerifyHashedPassword(user, user.PasswordHashed, model.Password)
                            == PasswordVerificationResult.Success) 
                     {
                        // Setup claims.
                        var claims = new[]
                        {
                           new Claim(JwtRegisteredClaimNames.sub, user.Username),
                           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        }
                        
                        // Setup key.
                        // Normally the string literal that is the key should be somewhere else secure, not here in code.
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SAMPLESECUREKEY"));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        
                        // Create the token.
                        JwtSecurityToken token = new JwtSecurityToken(
                               issuer: "http://mysite.com",
                               audience: "http://mysite.com",
                               claims: claims,
                               expires: DateTime.UtcNow.AddMinutes(15),
                               signingCredentials: creds
                            );
                            
                        // Return the token to the caller.
                        return Ok(new 
                        {
                           token = new JwtSecurityTokenHandler.WriteToken(token)
                        });
                     }      
                 }       
             }
             catch (Exception ex)
             {
                // some code here to handle exceptions
             }

             return BadRequest("Failed to login");
        }
    }
   ```

### 2.4 Using OAuth



## 3. Authorization

