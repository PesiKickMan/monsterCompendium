namespace monsterCompendium.Tests;

public class SanityTests
{
    [Fact]
    public void Harness_Funciona()
    {
        // Arrange
        int esperado = 4;
        // Act
        int resultado = 2 + 2;
        // Assert
        Assert.Equal(esperado, resultado);
    }
}
