using Finanzauto.Domain.Entities;
using Finanzauto.Domain.Repositories;
using Finanzauto.Domain.Tokens;
using Finanzauto.FileRoot;
using Finanzauto.Infrastructure.Data;
using Finanzauto.Infrastructure.Mappers;
using Finanzauto.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

/*string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
    builder.Configuration.GetConnectionString("ConexionSqlServer");*/
string connectionString =builder.Configuration.GetConnectionString("ConexionSqlServer");

builder.Services.AddDbContext<DataContext>(o =>
{
    //CADENA DE CONEXION
    o.UseSqlServer(connectionString);
});

//TODO: HACER LOS PASSWORD MAS SEGURO
builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider; //ES EL GENERADOR DE TOKEN POR DEFECTO, SE PUEDE CREAR UNO
    cfg.SignIn.RequireConfirmedEmail = false; //LOS USUARIOS DEBEN SER CONFIRMADOS

    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequireDigit = false;
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequireUppercase = false;
    //cfg.Password.RequiredLength = 8; //TODO: colocar limites de caracteres

    cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); //TIEMPO DE BLOQUEO DEL USUARIO
    cfg.Lockout.MaxFailedAccessAttempts = 5; //TRES INTENTOS Y SE BLOQUEAN
    cfg.Lockout.AllowedForNewUsers = true;//TODOS LOS USUARIOS SE BLOQUEAN

}).AddDefaultTokenProviders()//SE AGREGA POR DEFECTO EL TOKEN
  .AddEntityFrameworkStores<DataContext>();

builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IPhaseRepository, PhaseRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICreateToken, CreateToken>();
builder.Services.AddScoped<IUploadFileRepository, UploadFileRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddTransient<SeedDb>();

//PARA ACCEDER A LA URL DE APLICACION
builder.Services.AddHttpContextAccessor();

// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(options => // Adding Jwt Bearer 
 {
     options.SaveToken = true;
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidAudience = builder.Configuration["Tokens:Audience"],
         ValidIssuer = builder.Configuration["Tokens:Issuer"],
         ClockSkew = TimeSpan.Zero,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"]))
     };
 });


//SE AGREGA EL AUTOMAPPER 
builder.Services.AddAutoMapper(typeof(FinanzautoMapper));

//SE HABILITAN LOS CORS
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowSpecificOrigin",
        builder => builder
        .WithOrigins("http://localhost:3000") // El origen de tu frontend
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath); //SE INCLUYEN COMETARIOS XML

    //HABILITAR AUTORIZACI�N EN SWAGGER
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticaci�n JWT usando el esquema Bearer. \r\n\r\n " +
        "Ingresa la palabra 'Bearer' seguida de un [espacio] y despues su token en el campo de abajo \r\n\r\n " +
        "Ejemplo : \"Bearer tkdjfjdkdkd\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }

    });
});


var app = builder.Build();
//ESTA FUNCION REALIZA LA INYECCION MANUAL DEL SEEDDB
SeedData(app);
void SeedData(WebApplication app)
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (IServiceScope? scope = scopedFactory.CreateScope())
    {
        SeedDb? service = scope.ServiceProvider.GetService<SeedDb>();
        service.SeedAsync().Wait();
    }
}

//HABILITAR LOS CORS PARA ACCEDER A LA API
app.UseCors("AllowSpecificOrigin");

app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();//SE AGREGA QUE TIENE AUTENTICACION
app.UseAuthorization();
app.MapControllers();
app.Run();
