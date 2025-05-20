using System;
using System.Collections.Generic;
using System.Text;

namespace alps.net.api.util
{
    public interface IHasSiSiChoiceChance
    {
        // <summary>
        /// For DoTransition or TimeTransition or UserCancelTransition 
        /// SHOULD be a value between 0 and 1 but can also be higher
        /// </summary>
        double getSisiChoiceChance();
           
         // <summary>
        /// For DoTransition or TimeTransition or UserCancelTransition 
        /// SHOULD be a value between 0 and 1 but can also be higher (but not below 0)
        /// </summary>
        void setSisiChoiceChance(double value);
    }
}

