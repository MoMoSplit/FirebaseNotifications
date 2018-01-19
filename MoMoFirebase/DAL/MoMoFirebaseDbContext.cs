using MoMoFirebase.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MoMoFirebase.DAL
{
    public class MoMoFirebaseDbContext: DbContext
    {
        //Add parameter to base for AzureDB
        public MoMoFirebaseDbContext()
            : base("name=FirebaseDbContext")
        {
        }
        public DbSet<FCMToken> FCMTokens { get; set; }
    }
}