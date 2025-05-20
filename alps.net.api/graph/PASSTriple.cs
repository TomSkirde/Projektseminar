using alps.net.api.util;
using System;
using System.Diagnostics;

namespace alps.net.api.parsing.graph
{
    public class PASSTriple : IPASSTriple
    {
        private readonly string subjectContent;
        private readonly string predicateContent;
        private readonly IStringWithExtra extraString;

        public class LiteralDataType
        {
            private readonly string dataType;
            public string getDataType() { return dataType; }

            public LiteralDataType(string dataType)
            {
                this.dataType = dataType;
            }
        }

        public class LiteralLanguage
        {
            private readonly string languageDefinition;
            public string getLanguageDefinition() { return languageDefinition; }

            public LiteralLanguage(string languageDefinition)
            {
                this.languageDefinition = languageDefinition;
            }
        }

        private PASSTriple(string subjectContent, string predicateContent)
        {
            this.subjectContent = subjectContent;
            this.predicateContent = predicateContent;
        }

        public PASSTriple(string subjectContent, string predicateContent, string objectContent) : this(subjectContent, predicateContent)
        {
            this.extraString = new StringWithoutExtra(objectContent);
        }


        public PASSTriple(string subject, string predicate, string objectContent, LiteralDataType dataType) : this(subject, predicate)
        {
            extraString = new DataTypeString(objectContent, dataType.getDataType());
        }

        public PASSTriple(string subject, string predicate, string objectContent, LiteralLanguage language) : this(subject, predicate)
        {
            extraString = new LanguageSpecificString(objectContent, language.getLanguageDefinition());
        }

        public PASSTriple(string subject, string predicate, IStringWithExtra objectWithExtra) : this(subject, predicate)
        {
            if (objectWithExtra is null) throw new ArgumentNullException(nameof(objectWithExtra));
            this.extraString = objectWithExtra;
        }


        public string getPredicate()
        { return predicateContent; }

        public string getSubject()
        {
            return subjectContent;
        }

        public string getObject()
        {
            return extraString.getContent();
        }

        public override bool Equals(object otherObject)
        {
            int matches = 0;
            if (otherObject is not PASSTriple triple) return false;

            if ((triple.getPredicate() != null
                 && getPredicate() != null
                 && triple.getPredicate().Equals(getPredicate()))
                || (triple.getPredicate() is null
                    && getPredicate() is null))
                matches++;
            if ((triple.getObjectWithExtra() != null
                 && getObjectWithExtra() != null
                 && triple.getObjectWithExtra().Equals(getObjectWithExtra()))
                || (triple.getObjectWithExtra() is null
                    && getObjectWithExtra() is null))
                matches++;
            return matches == 2;
        }

        public IStringWithExtra getObjectWithExtra()
        {
            Debug.Assert(extraString != null);
            var cloned = extraString.clone();
            Debug.Assert(cloned != null);
            return cloned;
        }

        public string getObjLang()
        {
            return extraString is LanguageSpecificString l ? l.getExtra() : null;
        }

        public string getObjDataType()
        {
            return extraString is DataTypeString d ? d.getExtra() : null;
        }

        public override int GetHashCode()
        {
            string baseString = "";
            if (getPredicate() != null)
                baseString += getPredicate();
            if (getObjectWithExtra() != null)
                baseString += getObjectWithExtra();
            return baseString.GetHashCode();
        }
    }
}
