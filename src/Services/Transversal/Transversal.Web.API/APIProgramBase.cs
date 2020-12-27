namespace Transversal.Web.API
{
    public abstract class APIProgramBase<TProgram> : WebProgramBase<TProgram>
        where TProgram : class, new()
    {
    }
}
