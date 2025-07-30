using AtiExamSite.Data;
using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Data.Repositories.Implementations;
using AtiExamSite.Services.Contracts;
using AtiExamSite.Services.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseSqlServer(connectionString));

//configure services
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IExamQuestionRepository, ExamQuestionRepository>();
builder.Services.AddScoped<IOptionRepository, OptionRepository>();
builder.Services.AddScoped<IQuestionOptionRepository, QuestionOptionRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IUserResponseRepository, UserResponseRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IExamSessionRepository, ExamSessionRepository>();

builder.Services.AddScoped<IExamSessionService, ExamSessionService>();

builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IExamQuestionService, ExamQuestionService>();
builder.Services.AddScoped<IOptionService, OptionService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuestionOptionService, QuestionOptionService>();
builder.Services.AddScoped<IUserResponseService, UserResponseService>();
//builder.Services.AddScoped<IUserService, UserService>();
// Make sure these exist

builder.Services.AddScoped<IUserResponseService, UserResponseService>();
builder.Services.AddScoped<IExamQuestionService, ExamQuestionService>();
builder.Services.AddScoped<IQuestionOptionService, QuestionOptionService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
