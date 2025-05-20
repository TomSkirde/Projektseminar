using alps.net.api.parsing;
using VDS.RDF;

namespace alps.net.api
{
    /// <summary>
    /// A class to represent a string along with a specified language identifier
    /// This class is useful to represent an object (in a triple store context),
    /// where the literal node might contain an additional language information.
    /// The language is stored as extra.
    /// If no extra is given, a default language identifier is used (such as en).
    /// </summary>
    public class LanguageSpecificString : StringWithExtra
    {
        protected string standardLang = "en";

        /// <summary>
        /// Takes a label as string to create a new language specified string.
        /// The label might be passed as label@lang (i.e. "someSubj@en") to be parsed correctly.
        /// The label must not contain an @. If not, the standard language is safed for the label.
        /// </summary>
        /// <param name="content">the label</param>
        public LanguageSpecificString(string content) : base(content)
        {
            // No need for calls here, base calls our overwriten setContent
        }

        /// <summary>
        /// Creates a new label, specifying the label and the language in different strings.
        /// If label and lang are currently together in one string, separated via @ (i.e. parsed from an owl file),
        /// use the other constructor instead.
        /// </summary>
        /// <param name="content">the label</param>
        /// <param name="lang">the language</param>
        public LanguageSpecificString(string content, string lang) : base(content, lang)
        {
            // No need for calls here, base calls our overwriten setContent
        }

        public override IStringWithExtra clone()
        {
            return new LanguageSpecificString(getContent(), getExtra());
        }
        

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
                if (extra.Equals("")) setExtra(standardLang);
            }
        }

        public override void setExtra(string lang)
        {
            if (lang == null)
            {
                this.extra = standardLang;
                return;
            }
            if (lang.Contains("@"))
            {
                setContent(lang);
            }
            else { this.extra = lang; }
        }

        public override string ToString()
        {
            if (!getExtra().Equals(""))
                return getContent() + "@" + getExtra();
            else return getContent();
        }
    }
}
