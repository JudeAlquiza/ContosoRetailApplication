# ASP.NET Core Security

There are three major aspects of ASP.NET Core security that developers should consider, these are **encryption**, **authentication**, and **authorization**.

## 0. Introduction

**Encryption** in the context of web security is when a data that is passed thorugh the wire is encrypted to protect it from eavesdroppers that has malicious intentions.

**Authentication** is something that is done to validate identity. The user has to give some sort of credentials and these credentials are validated to prove the identity of the user.

**Authorization** is something that is done after a user has been authenticated. The system verifies the rights of a user to a specific resource.

## 1. Encryption

In ASP.NET Core and in web security in general the most common way of making sure that data is encrypted is using **SSL (secure sockets layer)**

### 1.1 Supporting SSL in ASP.NET Core

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
   
Here <code>SampleUser</code> is a type that derives from <code>IdentityUser</code>, <code>SampleRole</code> is a type that derives from <code>IdentityRole</code>, and <code>SampleContext</code> is the context where the persistence of objects of type <code>SampleUser</code> and <code>SampleRole</code> are being managed.

Also note that in an actual implementation, the <code>SampleUser</code> and <code>SampleRole</code> class are on a seperate layer, for example on a data layer, registration of dependencies might need to take some other form to accomodate data transfer objects or DTOs and view models.

Still inside of <code>Startup.cs</code> inside of the <code>Configure()</code> method, add the following code to use ASP.NET Identity.
   
   ``` C#
     public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
     {
        // some code here
        // add this before adding Mvc
        app.UseIdentity();
        app.UseMvc();
     }
   ```
   
It is important that we add ASP.NET Identity before we add Mvc because it's the Mvc middleware that we want to protect.

At this point we can already test it only to find that anonymous users can still access the app. This is because we need to configure 
the app what resources we want to protect from anonymous access. This is achieved by adding the <code>[Authorize]</code> attribute.

One way of doing this is at the controller level. In the code snippet below we're allowing anonymous access to the two <code>Get</code> 
controller actions but not with the <code>Post</code>, <code>Put</code>, and <code>Delete</code> controller actions. These three controller actions need the user accessing the app to be authenticated first before that user can gain access to these resources.
   
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
   
A much preferred way of doing this is to add the <code>[Authorize]</code> attribute at the controller level. This will make all controller actions protected from anonymous access. If we want to override this for some of the controller actions, we can make use of the <code>[AllowAnonymous]</code> attribute as shown below.
   
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
   
At this point we can already test it, if we're developing an api instead of a web application, if we try to access the api in chrome we will be redirected to something like (for this example) **'https://locahost:44316/Account/Login?ReturnUrl=someUrlHere'**. This might look a bit strange at first but the way ASP.NET Core works is that right now, MVC is integrated into WebAPI, when we access the WebAPI in a browser it thinks that we want to be redirected to a login page which is not we want. 

Since we are developing an api, chances are we will not be making these requests in a browser, that is the request can either come from a javascript client or a mobile client, and instead of the application redirecting us to a page, we want specific status codes to be returned instead. That is for users that are not properly authenticated, we want to receive an Http status code of **401 (unauthorized)**, and for authenticated users that don't have access to the resource, we want to receive an Htpp status code of **403 (Forbidden)**.

To make this work, as a final step, we need to go back to the Startup.cs and add the following configuration inside of the <code>ConfigureServices()</code> method.
  
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
   
Now that we have ASP.NET Identity setup, let's see how cookie authentication works with ASP.NET Identity.

### 2.2 Using Cookie Authentication

The way cookie authentication works is that when the user logs in, it sends its credentials to the server, the server validates the credentials, authenticates the user and sends a response with the cookie attached to it. This cookie is now included in every subsequent request to the server which is validated by the server, instead of validating the credentials. Cookie authentication is supported by ASP.NET Identity out of the box.

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

Cookie authentication has been around for quite a bit now and is still being used. But with the advent of single page applications or SPAs, and Web APIs, cookie authentication has become a lot less common. One major downside is that its stateful, this means that both the server and the client need to maintain sessonIDs to make sure that the validity of the cookie is verified.

A stateles alternative to this is token authentication which we'll discuss next.

### 2.3 Using Token Authentication

Token authentication has been tied to what is called JSON Web Tokens or JWTs and these JWTs are the actual 'Tokens' in Token Authentication. These JWTs has been the standard eversince in systems and application frameworks that implements token authentication.

Token-based authentication is stateless. The server does not keep a record of which users are logged in or which tokens have been issued. Instead, every request to the server is accompanied by a token which the server uses to verify the authenticity of the request.

How these tokens are transported across the wire is the same for cookies, the main difference is how the the token is used and structured that makes it stateless.

There are two steps that is done by the server to implement token authtentication, first is to generate the JWT, and second, validate the JWT.

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

In the <code>AuthController.cs</code>, let's pass in a <code>UserManager</code> and a <code>PasswordHasher</code> to the constructor.

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
   
Inside of the AuthController.cs, let's add a <code>CreateToken()</code> action to it. This <code>CreateToken()</code> action needs to retrieve the user using the <code>UserManager</code>, validate the password from the model against this user, and then generate the token. These steps are specified in the code snippet below.

   ``` C#
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private SignInManager<SampleUser> _signInManager;

        public AuthController(
            // the SampleUser type here is the SampleUser that we used earlier
            SignInManager<SampleUser> signInManager)
        {
            _signInManager = signInmanager;
        }

        [HttpPost("login")]
        public aysnc Task<IActionResult> Login([FromBody]UserCredentialsModel model)
        {
             // some code here
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
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SAMPLESECUREVERYSECURESUPERSECURERANDOMKEY"));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        
                        // Create the token.
                        JwtSecurityToken token = new JwtSecurityToken(
                               issuer: "http://mysuperawesomesite.com",
                               audience: "http://mysuperawesomesite.com",
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
   
Now the code snippet above needs a bit of cleaning as we have a bunch of hard coded strings in it. We can put all the information we need in <code>appsettings.json</code>. In our appsettings.json, we add the following json data.

   ``` Javascript 
   "Tokens": {
      "Key": "SAMPLESECUREVERYSECURESUPERSECURERANDOMKEY",
      "Issuer": "http://mysuperawesomesite.com",
      "Audience": "http://mysuperawesomesite.com"
   }
   ```
   
Now that we have these information in configuration, lets go back to <code>AuthController.cs</code> and pass in a <code>ConfigurationRoot</code> to the constructor.

   ``` C#
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private SignInManager<SampleUser> _signInManager;
        private UserManager<SampleUser> _userManager;
        private IPasswordHasher<SampleUser> _hasher;
        private IConfigurationRoot _config;

        public AuthController(
            SignInManager<SampleUser> signInManager,
            UserManager<SampleUser> userManager,
            IPasswordHasher<SampleUser> hasher,
            IConfigurationRoot config)
        {
            _signInManager = signInmanager;
            _userInManager = userInmanager;
            _hasher = hasher;
            _config = config;
        }

        // some code here
    }
   ```
   
Let's go back to the <code>CreateToken()</code> action and make modification to use configuration instead of hard coded strings. 
These changes are in the setup key and create token process respectively.

   ``` C#
        [HttpPost("token")]
        public aysnc Task<IActionResult> CreateToken([FromBody]UserCredentialsModel model)
        {
             try
             {
                 if (!ModelState.IsValid) return BadRequest("Failed to login");

                 // retrieve the user.
                 var user = await _userManager.FindByNameAsync(model.Username); 

                 if (user != null) 
                 {
                     // validate the password.
                     if (_hasher.VerifyHashedPassword(user, user.PasswordHashed, model.Password)
                            == PasswordVerificationResult.Success) 
                     {
                        // setup claims
                        var claims = new[]
                        {
                           new Claim(JwtRegisteredClaimNames.sub, user.Username),
                           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        }
                        
                        // setup key
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]);
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        
                        // create token
                        JwtSecurityToken token = new JwtSecurityToken(
                               issuer: _config["Tokens:Issuer"],
                               audience: _config["Tokens:Audience"],
                               claims: claims,
                               expires: DateTime.UtcNow.AddMinutes(15),
                               signingCredentials: creds
                            );
                            
                        // return the token to the caller
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
   ```

Next we'll proceed with validating these JWTs. First install the <code>Microsoft.AspNetCore.Authentication.JwtBearer</code> package.

Go to <code>Startup.cs</code> and add <code>app.UseJwtBearerAuthentication()</code> inside of the <code>Configure()</code> method as follows

   ``` C#
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
       {
          // some code here
          app.UseIdentity();
          
          app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
               AutomaticAuthenticate = true,
               AutomaticChallenge = true,
               TokenValidationParameters = new TokenValidationParameters()
                 {
                   ValidIssuer = _config["Tokens:Issuer"],
                   ValidAudience = _config["Tokens:Issuer"],
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"])),
                   ValidateLifeTime = true
                 }
            });
          
          app.UseMvc();
       }
   ```

A couple of things to note here. When adding <code>app.UseJwtBearerAuthentication()</code> we need to pass in a <code>JwtBearerOptions</code> object. In this <code>JwtBearerOptions</code> object, we then need to specify a number of paramters.

<code>AutomaticAuthenticate = true</code>, this tells ASP.NET that when it finds the token, you'd actually want to authenticate.
<code>AutomaticChallenge = true</code>, if the token is invalid or missing, allow to respond as a challenge.
<code>TokenValidationParameters = new TokenValidationParameters() { ... }</code>, this is the information that you want the JwtBearerAuthentication middleware to use to validate the token.

To take it a step further, we note that even though this might be a good alternative, the responsibility of securing, that is authenticating and authorizing access to the api, is still in the hands of the application. The ultimate goal is to delegate this responsibility to a seperate application so that they will be independent and any changes on either of them won't have major effects on the other. With this in mind, we turn our attention to what is called a **Secure Token Service** or **STS**.

### 2.4 Using a Secure Token Service or STS

A **secure token service** or **STS** is a dedicated application that handles all the token authentication and authorization features, and is a seperate application in itself.

Before moving on, let's define the following terms first. 

<code>Resource Owner</code> is considered as the user and the one who owns the protected resource.
<code>Client</code> is the application used by the resource owner to access the protected resource.

The way this works is that when a request for a protected resource is made by the resource owner, instead of the client application (or client) handling the authentication, this responsibility will be delegated to a secure token service instead. 

This secure token service then redirects the client application to a login page where the user enters his/her credentials. T

hese credentials will be sent to the secure token service for authentication. 

After this, the secure token service, will redirect the client application to a consent screen or page where the user or the resource can set what information it wants to allow the client application to use in order to access the resources on the user's behalf.

## 3. Authorization

