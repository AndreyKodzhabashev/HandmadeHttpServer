namespace WebServer.Server.Common
{
  using Exceptions;

    public static class CoreValidator
    {

        public static void ThrowIfNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new BadRequestException($"{name} cannot be null.");
            }
        }

        public static void ThrowIfNullOrEmpty(string text, string name)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new BadRequestException($"Parameter {name} cannot be null or empty.");
            }
        }
    }
}