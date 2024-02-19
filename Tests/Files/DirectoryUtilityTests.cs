using Galaxon.Core.Files;

namespace Galaxon.Core.Tests.Files;

[TestClass]
public class DirectoryUtilityTests
{
    [TestMethod]
    public void GetParentDirectoryByFileType_Returns_Null_If_No_File_Found()
    {
        // Arrange
        string extension = "nonexistent";

        // Act
        var result = DirectoryUtility.GetParentDirectoryByFileType(extension);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetParentDirectoryByFileType_Returns_Correct_Directory_If_File_Found()
    {
        // Arrange
        string extension = "sln"; // Assuming there is at least one .sln file in the directory tree.

        // Act
        var result = DirectoryUtility.GetParentDirectoryByFileType(extension);
        Console.WriteLine(result);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.EndsWith("/Galaxon/Galaxon"));
        Assert.IsTrue(Directory.Exists(result));
    }

    [TestMethod]
    public void GetSolution_Returns_Correct_Directory_If_Solution_File_Found()
    {
        // Act
        var result = DirectoryUtility.GetSolution();

        // Assert
        Assert.IsNotNull(result);

        // Check directory.
        Assert.IsTrue(result.EndsWith("/Galaxon/Galaxon"));
        Assert.IsTrue(Directory.Exists(result));
    }

    [TestMethod]
    public void GetProject_Returns_Correct_Directory_If_Project_File_Found()
    {
        // Act
        var result = DirectoryUtility.GetProject();

        // Assert
        Assert.IsNotNull(result);

        // Check directory.
        Assert.IsTrue(result.EndsWith("/Galaxon/Galaxon/Tests"));
        Assert.IsTrue(Directory.Exists(result));
    }
}
