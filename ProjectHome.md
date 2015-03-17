
```
FormatExtensions.Format
(
    "{greeting}, {planet}!",
    "greeting:Hello",
    "planet:world"
);
```

```
FormatExtensions.MagicFormat
(
    "{Greeting}, {Planet}!",
    new
    {
        Greeting = "Hello",
        Planet = "world"
    }
);
```