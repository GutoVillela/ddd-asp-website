using KadoshShared.ValueObjects;

namespace KadoshWebsite.ValueObjects
{
    public class RefreshToken : ValueObject
    {
        public RefreshToken(string username, string token)
        {
            Username = username;
            Token = token;
        }

        public string Username { get; private set; }
        public string Token { get; private set; }
    }
}
