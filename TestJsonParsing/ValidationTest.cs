using ChatLib;
using Newtonsoft.Json;

namespace TestJsonParsing;

public class ValidationTest
{
    [Fact]
    public void TestValidJson()
    {
        string json = @"
            {
                ""Sender"": ""Someone"",
                ""Message"": ""This should work""
            }";
        (bool success, string errors) = JsonSchemaValidator.Validate(json);
        Assert.True(success);
        Assert.Empty(errors);
    }

    [Fact]
    public void TestInvalidJson()
    {
        string json = @"
            {
                ""Senders"": ""Someone"",
                ""Message"": ""This should work""
            }";
        (bool success, string errors) = JsonSchemaValidator.Validate(json);
        Assert.False(success);
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void TestChatMessaageSerializationToJson()
    {
        ChatMessage message = new ChatMessage("Dr. A", "Hello World");
        string json = JsonConvert.SerializeObject(message);
        (bool success, string errors) = JsonSchemaValidator.Validate(json);
        Assert.True(success);
        Assert.Empty(errors);
    }
}