using FileTypeChecker.Extensions;
using FileTypeChecker.Types;

namespace RecipeBook.Application.Extensions;

internal static class StreamImageExtensions
{
    public static (bool isValid, string extension) ValideAndGetImageExtension(this Stream stream)
    {
        var result = (false, string.Empty);

        if (stream.Is<PortableNetworkGraphic>())
            result = (true, NormalizeExtension(PortableNetworkGraphic.TypeExtension));
        else if (stream.Is<JointPhotographicExpertsGroup>())
            result = (true, NormalizeExtension(JointPhotographicExpertsGroup.TypeExtension));

        stream.Position = 0;
        return result;
    }

    private static string NormalizeExtension(string extension) => extension.StartsWith('.') ? extension : $".{extension}";
}
