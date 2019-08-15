# ClaimsTransformation

### An implementation of the ADFS claims transformation language for .NET

Some informations exist;

* https://docs.microsoft.com/en-us/windows-server/identity/solution-guides/claims-transformation-rules-language
* https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/dd807118(v=ws.10)

Here is a usage example;

```C#
//Setup (this can be done once)
var parser = new ClaimsTransformationParser();
var cache = new ClaimsTransformationCache();
var engine = new ClaimsTransformationEngine(parser, cache);

var expression =
  @"C1:[] " +
  @"=> ISSUE(TYPE = ""http://namespace/role"", VALUE = ""Public"");";

//Read these claims from your authentication token.
var input = new Claim[] { };
//Use these claims for your security.
var output = engine.Transform(expression, input);
```

See the test suite for more examples.
