namespace JsonSerializer.Tests;

public class SerializationTests
{
    class SimpleTestObject
    {
        public int Number { get; set; }
        public string SomeString { get; set; } = null!;
        public bool BoolValue { get; set; }
    }

    [Fact]
    public void SerializeSimpleObject_ReturnJsonObject()
    {
        // Arrange
        var obj = new SimpleTestObject
        {
            Number     = 10,
            SomeString = "gsfgsf",
            BoolValue  = true
        };

        const string expectedResult = "{\"number\":10,\"someString\":\"gsfgsf\",\"boolValue\":true}";

        // Act
        string json = JsonConverter.Serialize(obj);

        // Assert
        Assert.Matches(expectedResult, json);
    }

    [Fact]
    public void SerializeArray_ReturnJsonArray()
    {
        // Arrange
        var array = new List<SimpleTestObject>
        {
            new SimpleTestObject
            {
                Number     = 10,
                SomeString = "gsfgsf",
                BoolValue  = true
            },
            new SimpleTestObject
            {
                Number     = 10,
                SomeString = "gsfgsf",
                BoolValue  = true
            },
            new SimpleTestObject
            {
                Number     = 10,
                SomeString = "gsfgsf",
                BoolValue  = true
            }
        };

        const string expectedResult = @"[
            {""number"":10,""someString"":""gsfgsf"",""boolValue"":true},
            {""number"":10,""someString"":""gsfgsf"",""boolValue"":true},
            {""number"":10,""someString"":""gsfgsf"",""boolValue"":true}
        ]";

        // Act
        string json = JsonConverter.Serialize(array);

        // Assert
        Assert.Matches(expectedResult, json);
    }

    [Fact]
    public void SerializeArrayOfInt_ReturnJsonArray()
    {
        // Arrange
        var array = new int[5] { 1, 2, 3, 4, 5 };
        const string expectedResult = "[1,2,3,4,5]";

        // Act
        string json = JsonConverter.Serialize(array);

        // Assert
        Assert.Matches(expectedResult, json);
    }

    [Fact]
    public void SerializeArrayOfBoolean_ReturnJsonArray()
    {
        // Arrange
        var array = new bool[5] { true, false, true, false, true };
        const string expectedResult = "[true,false,true,false,true]";

        // Act
        string json = JsonConverter.Serialize(array);

        // Assert
        Assert.Matches(expectedResult, json);
    }

    [Fact]
    public void SerializeArrayOfString_ReturnJsonArray()
    {
        // Arrange
        var array = new string[3] { "test1", "test2", "test3" };
        const string expectedResult = "[\"test1\",\"test2\",\"test3\"]";

        // Act
        string json = JsonConverter.Serialize(array);

        // Assert
        Assert.Matches(expectedResult, json);
    }

    [Fact]
    public void SerializeArrayOfDifferentObjects_ReturnJsonArray()
    {
        // Arrange
        var array = new object[3] { 1, "test2", true };
        const string expectedResult = "[1,\"test2\",true]";

        // Act
        string json = JsonConverter.Serialize(array);

        // Assert
        Assert.Matches(expectedResult, json);
    }

    class TestObject
    {
        public bool BooleanValue { get; set; }
        public SimpleTestObject SimpleTestObject { get; set; } = null!;
        public object[] Array { get; set; } = null!;
    }

    [Fact]
    public void SerializeObjectWithArray_ReturnJsonObjectWithArray()
    {
        // Arrange
        var obj = new TestObject
        {
            BooleanValue     = true,
            SimpleTestObject = new SimpleTestObject
            {
                BoolValue  = false,
                Number     = 100,
                SomeString = "str_1"
            },
            Array            = new object[]
            {
                1,
                "test2",
                true,
                new SimpleTestObject
                {
                    BoolValue  = false,
                    Number     = 100,
                    SomeString = "str_1"
                }
            }
        };

        const string expectedResult = @"
        {
            ""booleanValue"":true,
            ""simpleTestObject"":
            {
                ""boolValue"":false,
                ""number"":100,
                ""SomeString"":""str_1""
            },
            ""array"":{
                1,
                ""test2"",
                true,
                {
                    ""boolValue"":false,
                    ""number"":100,
                    ""someString"":""str_1""
                }
            }
        }
        ";

        // Act
        string json = JsonConverter.Serialize(obj);

        // Assert
        Assert.Matches(expectedResult, json);
    }
}