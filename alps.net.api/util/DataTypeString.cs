using alps.net.api.parsing;
using System;

namespace alps.net.api.util
{
    /// <summary>
    /// A class to represent a string along with a specified dataType
    /// This class is useful to represent an object (in a triple store context),
    /// where the literal node might contain an additional data type information.
    /// The datatype is stored as extra.
    /// </summary>
    public class DataTypeString : StringWithExtra
    {
        public DataTypeString(string input) : base(input) { }

        public DataTypeString(string content, string extra) : base(content, extra) { }

        public override void setContent(string content)
        {
            if (content == null)
            {
                this.content = "";
                return;
            }
            if (content.Contains("@"))
            {
                this.content = content.Split('@')[0];
                setExtra(content.Split('@')[1]);
            }
            else
            {
                this.content = content;
            }
        }

        public override void setExtra(string extra)
        {
            if (extra == null)
            {
                this.extra = "";
                return;
            }
            if (extra.Contains("@"))
            {
                setContent(extra);
            }
            else { this.extra = extra; }
        }
        

        public override IStringWithExtra clone()
        {
            return new DataTypeString(getContent(), getExtra());
        }
    }
}
