using TransafeRx.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TransafeRx.Shared.Utils
{
    public static class ItemHelper
    {
        private static TransafeRxEntities db = new TransafeRxEntities();
        private static List<GetPlaceholders_Result> placeholders = db.GetPlaceholders().ToList();
        public static string ReplacePlaceholders(string text, string userId)
        {
            if (!string.IsNullOrEmpty(text))
            {
                foreach (var placeholder in placeholders)
                {
                    if (placeholder.IsStatic)
                    {
                        text = text.Replace(placeholder.UniqueName, placeholder.ReplacementText);
                    }
                    else
                    {
                        if (text.Contains(placeholder.UniqueName))
                        {
                            var replacementText = db.EvaluateDynamicPlaceholder(userId, placeholder.Expression).SingleOrDefault();
                            if (!String.IsNullOrEmpty(replacementText))
                            {
                                text = text.Replace(placeholder.UniqueName, replacementText);
                            }
                        }
                    }
                }
            }

            return text;
        }
    }
}
