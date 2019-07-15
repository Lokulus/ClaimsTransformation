using System;
using System.Collections.Generic;
using System.Linq;

namespace ClaimsTransformation.Language.Parser
{
    public static class Extensions
    {
        public static TokenValue ForChannel(this TokenValue value, TokenChannel channel)
        {
            if (value.Token != null)
            {
                if (value.Token.Channel == channel)
                {
                    return value;
                }
                else
                {
                    return null;
                }
            }
            else if (value.Children != null)
            {
                return new TokenValue(
                    value.Syntax,
                    value.Children
                        .Select(child => child.ForChannel(channel))
                        .Where(child => child != null)
                        .ToArray()
                    );
            }
            throw new NotImplementedException();
        }
    }
}
