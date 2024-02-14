using KeyAuth;
using KeyAuth.Credentials;
using KeyAuth.Rendering;

namespace KeyAuth;

class Program
{
   

    static async Task Main(string[] args)
    {
        var keyAuthApp = new api(
            name: "demo",
            ownerid: "3cXo5tx34Y",
            secret: "016dd9c5652df8591ef638194a51cd4f05e0b9309595263072fcaba78655a9c0",
            version: "1.0"
        );
        var keyAuthOverlay = new Rendering.Renderer(keyAuthApp,new CredentialService());
        await keyAuthOverlay.Run();
    }
}

class ProgramImpl : Program
{
}
