using KeyAuth.Credentials;

namespace KeyAuth;

class Program
{
    static async Task Main(string[] args)
    {
        var keyAuthApp = new api(
            name: "KeyAuth - Loader Base",
            ownerid: "InkyonIZKN",
            secret: "518c99990d17a66b1cf6baf29e4cee25b7c8c05cdd8849b7fd5632e4c3764f16",
            version: "1.1"
        );
        var keyAuthOverlay = new Rendering.Renderer(keyAuthApp,new CredentialService());
        await keyAuthOverlay.Run();
    }
}
