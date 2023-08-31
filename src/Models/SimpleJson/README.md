## Simple JSON: JSON Dynamic Conversion
A tool designed to ease the loading of dynamically parsed JSON objects from `JsonSerializer.Deserialize<dynamic>` into objects.
- Often times when using APIs, they return more information than needed from their endpoint.
    - Trying to decode this information with a custom JSON deserializer can be annoying when working with 3rd party APIs we don't control.
    - No more errors stating you left the deserializer with more or less data to read! 
- Implement your own "custom" parsers of dynamic data (`IFromJsonConverter`) to avoid the hassle of writing a custom `JsonConverter`. 