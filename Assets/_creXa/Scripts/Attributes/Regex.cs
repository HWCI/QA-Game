using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace creXa.GameBase
{
    public class RegexAttribute : PropertyAttribute
    {
        public readonly string pattern;
        public readonly string helpMessage;

        public RegexAttribute(string pattern, string helpMessage)
        {
            this.pattern = pattern;
            this.helpMessage = helpMessage;
        }

    }
}   
