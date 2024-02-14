using KeyAuth.Credentials;

namespace KeyAuth;

class Program
{
    static async Task Main(string[] args)
    {
        var keyAuthApp = new api(
            name: "",
            ownerid: "",
            secret: "",
            version: ""
        );
        var keyAuthOverlay = new Rendering.Renderer(keyAuthApp,new CredentialService());
        await keyAuthOverlay.Run();
    }
}

class ProgramImpl : Program
{
}
