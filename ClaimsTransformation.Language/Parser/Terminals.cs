namespace ClaimsTransformation.Language.Parser
{
    public class Terminals
    {
        public const string EMPTY = "";

        public const string IMPLY = "=>";

        public const string SEMICOLON = ";";

        public const string COLON = ":";

        public const string COMMA = ",";

        public const string DOT = ".";

        public const string O_SQ_BRACKET = "[";

        public const string C_SQ_BRACKET = "]";

        public const string O_BRACKET = "(";

        public const string C_BRACKET = ")";

        public const string EQ = "==";

        public const string NEQ = "!=";

        public const string REGEXP_MATCH = "=~";

        public const string REGEXP_NOT_MATCH = "!~";

        public const string ASSIGN = "=";

        public const string CONCAT = "+";

        public const string AND = "&&";

        public const string ISSUE = "issue";

        public const string ADD = "add";

        public const string TYPE = "type";

        public const string VALUE = "value";

        public const string VALUE_TYPE = "valuetype";

        public const string CLAIM = "claim";

        public const string IDENTIFIER = "[_A-Za-z][_A-Za-z0-9]*";

        public const string QUOTE = "\"";

        public const string STRING = "\"[^\"\n]*\"";

        public const string UINT64_TYPE = "uint64";

        public const string INT64_TYPE = "int64";

        public const string STRING_TYPE = "string";

        public const string BOOLEAN_TYPE = "boolean";
    }
}
