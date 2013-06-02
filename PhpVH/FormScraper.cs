using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Components;

namespace PhpVH
{
    public class FormScraper
    {
        public enum AttributeParserState
        {
            Start,
            Name,
            AssignmentOperator,
            Quote,
            QuotedValue,
            SingleQuote,
            SingleQuoteValue,
            UnquotedValue
        }

        private static Regex _formRegex = new Regex(
            @"<form(\s*[^>]*)>([\u0000-\uffff]*?)</\s*form>",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private static Regex _inputRegex = new Regex(
            @"<input\s*([^>]+)>",
            RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        public static Dictionary<string, string> GetAttributes(string tag)
        {
            tag = tag + " ";
            var attributes = new Dictionary<string, string>();
            var state = AttributeParserState.Start;

            StringBuilder name = null, value = null;

            for (int i = 0; i < tag.Length; i++)
            {
                var c = tag[i];
                if (state == AttributeParserState.Start)
                {
                    if (Char.IsLetterOrDigit(c))
                    {
                        name = new StringBuilder();
                        name.Append(c);

                        state = AttributeParserState.Name;
                    }
                }
                else if (state == AttributeParserState.Name)
                {
                    if (Char.IsLetterOrDigit(c))
                        name.Append(c);
                    else
                    {
                        if (!attributes.ContainsKey(name.ToString().ToLower()))
                            attributes.Add(name.ToString().ToLower(), null);

                        if (c == '=')
                            state = AttributeParserState.AssignmentOperator;
                        else
                        {
                            name = null;
                            state = AttributeParserState.Start;
                        }
                    }
                }
                else if (state == AttributeParserState.AssignmentOperator)
                {
                    if (c == '"')
                    {
                        value = new StringBuilder();
                        state = AttributeParserState.Quote;
                    }
                    else if (c == '\'')
                    {
                        value = new StringBuilder();
                        state = AttributeParserState.SingleQuote;
                    }
                    else if (c != ' ')
                    {
                        value = new StringBuilder();
                        value.Append(c);
                        state = AttributeParserState.UnquotedValue;
                    }
                }
                else if (state == AttributeParserState.Quote)
                {
                    if (c != '"')
                        value.Append(c);
                    else
                    {
                        attributes[name.ToString().ToLower()] = value.ToString();
                        name = null;
                        value = null;
                        state = AttributeParserState.Start;
                    }
                }
                else if (state == AttributeParserState.SingleQuote)
                {
                    if (c != '\'')
                        value.Append(c);
                    else
                    {
                        attributes[name.ToString().ToLower()] = value.ToString();
                        name = null;
                        value = null;
                        state = AttributeParserState.Start;
                    }
                }
                else if (state == AttributeParserState.UnquotedValue)
                {
                    if (c != ' ' && c != '>')
                        value.Append(c);
                    else
                    {
                        attributes[name.ToString().ToLower()] = value.ToString();
                        name = null;
                        value = null;
                        state = AttributeParserState.Start;
                    }
                }
            }

            return attributes;
        }

        public static FormTag[] GetForms(string html, string defaultAction)
        {
            var matches = _formRegex
                .Matches(html)
                .OfType<Match>()
                .Select(x =>
                {
                    var attributes = GetAttributes(x.Groups[1].Value);
                    var inputs = GetInputs(x.Groups[2].Value);
                    var action = attributes.GetValueOrDefault("action");

                    var actionLowered = (action ?? "").ToLower();

                    if (actionLowered.StartsWith("mailto:"))
                        return null;

                    if (!string.IsNullOrEmpty(action) &&
                        !actionLowered.StartsWith("http:") &&
                        !actionLowered.StartsWith("https:"))
                        action = new Uri(new Uri(defaultAction), action).ToString();

                    return new FormTag()
                    {
                        Id = attributes.GetValueOrDefault("id"),
                        Action = string.IsNullOrEmpty(action) ? defaultAction : action,
                        Method = (attributes.GetValueOrDefault("method") ?? "get").ToLower(),
                        Inputs = inputs
                    };
                })
                .Where(x => x != null)
                .ToArray();
            return matches;
        }

        public static InputTag[] GetInputs(string html)
        {
            var matches = _inputRegex
                .Matches(html)
                .OfType<Match>()
                .Select(x =>
                {
                    var attrs = GetAttributes(x.Groups[1].Value);
                    Nullable<int> size = null, maxLength = null;
                    int i;
                    if (int.TryParse(attrs.GetValueOrDefault("size"), out i))
                        size = i;

                    if (int.TryParse(attrs.GetValueOrDefault("maxlength"), out i))
                        maxLength = i;
                    return new InputTag()
                    {
                        Id = attrs.GetValueOrDefault("id"),
                        Name = attrs.GetValueOrDefault("name"),
                        Type = attrs.GetValueOrDefault("type"),
                        Value = attrs.GetValueOrDefault("value"),
                        Size = size,
                        MaxLength = maxLength
                    };
                })
                .ToArray();

            return matches;
        }
    }
}
