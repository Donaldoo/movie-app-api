using System.Reflection;

namespace Movie.Application;

public static class AboutMe
{
    public static Assembly Assembly => typeof(AboutMe).Assembly;
}