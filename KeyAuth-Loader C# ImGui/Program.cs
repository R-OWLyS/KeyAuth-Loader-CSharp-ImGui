using KeyAuth;
using KeyAuth.Renderer;

class Program
{
    public static api KeyAuthApp = new api(
    name: "",
    ownerid: "",
    secret: "",
    version: "" /*,
    path: @"Your_Path_Here" */ 
);

    static async Task Main(string[] args)
    {
        var KeyAuthOverlay = new KeyAuth_Renderer();
        await KeyAuthOverlay.Run();
    }
}
