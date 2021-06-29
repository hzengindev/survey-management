using Core.Utilities.Results;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;

namespace Core.Utilities.Dynamics
{
    public static class EntityHelper
    {
        public static Lookup GetLookup(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Contains(attributeLogicalName))
                return null;

            return new Lookup
            {
                Id = entity.GetAttributeValue<EntityReference>(attributeLogicalName).Id,
                Name = entity.GetAttributeValue<EntityReference>(attributeLogicalName).Name
            };
        }
        public static Lookup GetLookup(this Entity entity, string alias, string attributeLogicalName)
        {
            var logicalName = $"{alias}.{attributeLogicalName}";
            if (!entity.Contains(logicalName))
                return null;

            return new Lookup
            {
                Id = ((EntityReference)entity.GetAttributeValue<AliasedValue>(logicalName).Value).Id,
                Name = ((EntityReference)entity.GetAttributeValue<AliasedValue>(logicalName).Value).Name
            };
        }

        public static string GetString(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Contains(attributeLogicalName))
                return null;

            return entity.GetAttributeValue<string>(attributeLogicalName);
        }
        public static string GetString(this Entity entity, string alias, string attributeLogicalName)
        {
            var logicalName = $"{alias}.{attributeLogicalName}";
            if (!entity.Contains(logicalName))
                return null;

            return entity.GetAttributeValue<AliasedValue>(logicalName).Value.ToString();
        }

        public static int? GetInt(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Contains(attributeLogicalName))
                return (int?)null;

            return entity.GetAttributeValue<int>(attributeLogicalName);
        }
        public static int? GetInt(this Entity entity, string alias, string attributeLogicalName)
        {
            var logicalName = $"{alias}.{attributeLogicalName}";
            if (!entity.Contains(logicalName))
                return (int?)null;

            return (int)entity.GetAttributeValue<AliasedValue>(logicalName).Value;
        }

        public static decimal? GetDecimal(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Contains(attributeLogicalName))
                return (decimal?)null;

            return entity.GetAttributeValue<decimal>(attributeLogicalName);
        }
        public static decimal? GetDecimal(this Entity entity, string alias, string attributeLogicalName)
        {
            var logicalName = $"{alias}.{attributeLogicalName}";
            if (!entity.Contains(logicalName))
                return (decimal?)null;

            return (decimal)entity.GetAttributeValue<AliasedValue>(logicalName).Value;
        }

        public static decimal? GetMoney(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Contains(attributeLogicalName))
                return (decimal?)null;

            return entity.GetAttributeValue<Money>(attributeLogicalName).Value;
        }
        public static decimal? GetMoney(this Entity entity, string alias, string attributeLogicalName)
        {
            var logicalName = $"{alias}.{attributeLogicalName}";
            if (!entity.Contains(logicalName))
                return (decimal?)null;

            return ((Money)(entity.GetAttributeValue<AliasedValue>(logicalName).Value)).Value;
        }

        public static float? GetFloat(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Contains(attributeLogicalName))
                return (float?)null;

            return entity.GetAttributeValue<float>(attributeLogicalName);
        }
        public static float? GetFloat(this Entity entity, string alias, string attributeLogicalName)
        {
            var logicalName = $"{alias}.{attributeLogicalName}";
            if (!entity.Contains(logicalName))
                return (float?)null;

            return (float)entity.GetAttributeValue<AliasedValue>(logicalName).Value;
        }

        public static double? GetDouble(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Contains(attributeLogicalName))
                return (double?)null;

            return entity.GetAttributeValue<double>(attributeLogicalName);
        }
        public static double? GetDouble(this Entity entity, string alias, string attributeLogicalName)
        {
            var logicalName = $"{alias}.{attributeLogicalName}";
            if (!entity.Contains(logicalName))
                return (double?)null;

            return (double)entity.GetAttributeValue<AliasedValue>(logicalName).Value;
        }

        public static DateTime? GetDateTime(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Contains(attributeLogicalName))
                return (DateTime?)null;

            return entity.GetAttributeValue<DateTime>(attributeLogicalName);
        }
        public static DateTime? GetDateTime(this Entity entity, string alias, string attributeLogicalName)
        {
            var logicalName = $"{alias}.{attributeLogicalName}";
            if (!entity.Contains(logicalName))
                return (DateTime?)null;

            return (DateTime)entity.GetAttributeValue<AliasedValue>(logicalName).Value;
        }

        public static OptionSet GetOptionSet(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Contains(attributeLogicalName))
                return null;

            return new OptionSet
            {
                Value = entity.GetAttributeValue<OptionSetValue>(attributeLogicalName).Value,
                Label = entity.FormattedValues[attributeLogicalName]
            };
        }
        public static OptionSet GetOptionSet(this Entity entity, string alias, string attributeLogicalName)
        {
            var logicalName = $"{alias}.{attributeLogicalName}";
            if (!entity.Contains(logicalName))
                return null;

            return new OptionSet
            {
                Value = ((OptionSetValue)entity.GetAttributeValue<AliasedValue>(logicalName).Value).Value,
                Label = entity.FormattedValues[logicalName]
            };
        }

        public static bool? GetBoolean(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Contains(attributeLogicalName))
                return (bool?)null;

            return entity.GetAttributeValue<bool>(attributeLogicalName);
        }
        public static bool? GetBoolean(this Entity entity, string alias, string attributeLogicalName)
        {
            var logicalName = $"{alias}.{attributeLogicalName}";
            if (!entity.Contains(logicalName))
                return (bool?)null;

            return Convert.ToBoolean(entity.GetAttributeValue<AliasedValue>(logicalName).Value);
        }

        public static Guid? GetKey(this Entity entity, string attributeLogicalName)
        {
            if (!entity.Contains(attributeLogicalName))
                return (Guid?)null;

            return entity.GetAttributeValue<Guid>(attributeLogicalName);
        }
        public static Guid? GetKey(this Entity entity, string alias, string attributeLogicalName)
        {
            var logicalName = $"{alias}.{attributeLogicalName}";
            if (!entity.Contains(logicalName))
                return (Guid?)null;

            return (Guid)(entity.GetAttributeValue<AliasedValue>(logicalName).Value);
        }

        public static Entity RetrieveNoLock(this IOrganizationService service, string entityLogicalName, Guid recordId, ColumnSet columns, string primaryIdLogicalName = null)
        {
            var qe = new QueryExpression(entityLogicalName);
            qe.ColumnSet = columns;
            qe.Criteria.AddCondition(new ConditionExpression(string.IsNullOrEmpty(primaryIdLogicalName) ? entityLogicalName + "id" : primaryIdLogicalName, ConditionOperator.Equal, recordId));
            qe.NoLock = true;
            qe.TopCount = 1;

            var entity = service.RetrieveMultiple(qe);
            if (entity.Entities.Any())
                return entity.Entities.First();
            return null;
        }

        /// <summary>
        /// Return first entity result or null
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entityLogicalName"></param>
        /// <param name="recordId"></param>
        /// <param name="columns"></param>
        /// <param name="primaryIdLogicalName"></param>
        /// <returns></returns>
        public static Entity RetrieveFirst(this IOrganizationService service, QueryExpression qe)
        {
            var entity = service.RetrieveMultiple(qe);
            if (entity.Entities.Any())
                return entity.Entities.First();
            return null;
        }

        public static EntityCollection RetrieveAll(this IOrganizationService service, QueryExpression qe)
        {
            //paging info and topcount can't be using together
            qe.TopCount = null;
            qe.NoLock = true;

            EntityCollection ec = new EntityCollection();
            int pageNumber = 1;
            var moreRecord = true;
            while (moreRecord)
            {
                qe.PageInfo = new PagingInfo { Count = 5000, PageNumber = pageNumber };
                var _ec = service.RetrieveMultiple(qe);
                if (_ec.Entities.Any())
                    ec.Entities.AddRange(_ec.Entities);

                moreRecord = _ec.MoreRecords;
                pageNumber += 1;
            }

            return ec;
        }
    }
}
