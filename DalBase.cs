using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MsSqlUtilityCore
{
    public abstract class DalBase
    {
        protected string _connString = "";

        public DalBase(IConfiguration configuration)
        {
            _connString = configuration["Database:connString"];
        }
    }
}
