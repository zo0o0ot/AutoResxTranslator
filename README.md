## AutoResxTranslator
A tool to automatically translate Resx files to any language using Google Translator. No API key is required for the translator. This tool also provides a text translator out of the box.

Text translation
------
![Text-Translation](/Wiki/Images/text-translation.png?raw=true)

Resx Translation
------
![ResX-Translation](/Wiki/Images/resx-translation.png?raw=true)

Resx translation is in progress
------
![ResX-Translation](/Wiki/Images/resx-translating.png?raw=true)

New Gibberish option checkboxes
------
![ResX-GibberishTranslation](/Wiki/Images/gibberishTranslator.png?raw=true)
+ Checking the "total gibberish" checkbox means that a nonsense string of the same length as the original value is returned instead of the language that you checked.
+ Checking the "leftover gibberish" option will default to the Google Translation API, but if that fails, it will add a gibberish string of the same length as the original value.  The errors in translating will still be logged if you want to see what failed.
