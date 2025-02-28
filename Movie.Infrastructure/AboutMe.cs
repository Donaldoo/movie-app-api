using System.Reflection;

namespace Movie.Infrastructure;

public static class AboutMe
{
    public static Assembly Assembly => typeof(AboutMe).Assembly;
    
}