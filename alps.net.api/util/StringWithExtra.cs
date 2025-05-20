
using alps.net.api.parsing;

namespace alps.net.api
{
    public abstract class StringWithExtra : IStringWithExtra
    {
        protected string content = "";
        protected string extra = "";

        public StringWithExtra(string input)
        {
            setContent(input);
        }

        public StringWithExtra(string content, string extra)
        {
            setContent(content);
            setExtra(extra);
        }

        public string getContent()
        {
            return content;
        }

        public string getExtra()
        {
            return extra;
        }


        public virtual void setContent(string content)
        {
            if (content == null)
            {
                this.content = "";
                return;
            }
            else
            {
                this.content = content;
            }
        }

        public virtual void setExtra(string extra)
        {
            if (extra == null)
            {
                this.extra = "";
                return;
            }
            else { this.extra = extra; }
        }

        public abstract IStringWithExtra clone();

        public override bool Equals(object obj)
        {
            if (obj is IStringWithExtra extra)
                if (GetType().Equals(extra.GetType()))
                {
                    int matches = 0;
                    if ((extra.getContent() != null && getContent() != null && extra.getContent().Equals(getContent())) || (extra.getContent() is null && getContent() is null))
                        matches++;
                    if ((extra.getExtra() != null && getExtra() != null && extra.getExtra().Equals(getExtra())) || (extra.getExtra() is null && getExtra() is null))
                        matches++;
                    if (matches == 2) return true;
                }
            return false;
        }

        public override int GetHashCode()
        {
            return (getContent() + getExtra()).GetHashCode();
        }

    }
}
