﻿// <copyright file="AnonymousValuesFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class AnonymousValuesFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .Given(() => Config.Global.Reset());
        }

        [Scenario]
        public static void RetrievingAnAnonymousValue(Foo result)
        {
            "Given a local config file containing an anonymous Foo with a Bar of 'baz'"
                .Given(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get the Foo"
                .When(() => result = Config.Global.Get<Foo>());

            "Then the Foo has a Bar of 'baz'"
                .Then(() => result.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void RetrievingANamedValueAnonymously(Foo result)
        {
            "Given a local config file containing a named Foo with a Bar of 'baz'"
                .Given(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get the Foo"
                .When(() => result = Config.Global.Get<Foo>());

            "Then the Foo has a Bar of 'baz'"
                .Then(() => result.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void RetrievingAnAnonymousValueFromMultipleValues(int result)
        {
            "Given a local config file containing multiple values"
                .Given(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.WriteLine(@"Add(""stringId"", ""34"");");
                        writer.WriteLine(@"Add(""id"", 12);");
                        writer.WriteLine(@"Add(""foo 2"", new Foo { Bar = ""baz 2"" });");
                        writer.WriteLine(@"Add(""code"", 15);");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get int item"
                .When(() => result = Config.Global.Get<int>());

            "Then it should be '12'"
                .Then(() => result.Should().Be(12));
        }

        [Scenario]
        public static void TryingToRetrieveAnAnonymousValue(Foo value, bool result)
        {
            "Given a local config file containing a named Foo with a Bar of 'baz'"
                .Given(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I try to get a Foo"
                .When(() => result = Config.Global.TryGetValue(out value));

            "Then the result is true"
                .Then(() => result.Should().BeTrue());

            "And the Foo has a Bar of 'baz'"
                .And(() => value.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void RetrievingANonexistentAnonymousValue(Exception ex)
        {
            "Given a local config file not containing any string item"
                .Given(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.WriteLine(@"Add(""id"", 12);");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I try to get a string item"
                .When(() => ex = Record.Exception(() => Config.Global.Get<string>()));

            "Then an exception is thrown"
                .Then(() => ex.Should().NotBeNull());
        }

        [Scenario]
        public static void RetrievingANonexistentAnonymousValueOrDefault(string result)
        {
            "Given a local config file not containing any string item"
                .Given(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.WriteLine(@"Add(""id"", 12);");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get a string item or default"
                .When(() => result = Config.Global.GetOrDefault<string>());

            "Then the result should be the default string"
                .Then(() => result.Should().Be(default(string)));
        }

        [Scenario]
        public static void TryingToRetrieveANonexistentAnonymousValue(string value, bool result)
        {
            "Given a local config file not containing any string item"
                .Given(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.WriteLine(@"Add(""id"", 12);");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I try to get a string item"
                .When(() => result = Config.Global.TryGetValue(out value));

            "Then the result should be false"
                .Then(() => result.Should().BeFalse());
        }
    }
}
